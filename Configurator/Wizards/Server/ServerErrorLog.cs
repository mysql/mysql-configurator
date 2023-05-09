/* Copyright (c) 2019, 2023, Oracle and/or its affiliates.

 This program is free software; you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation; version 2 of the License.

 This program is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 GNU General Public License for more details.

 You should have received a copy of the GNU General Public License
 along with this program; if not, write to the Free Software
 Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301  USA */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MySql.Configurator.Wizards.Server
{
  public class ServerErrorLog
  {
    #region Constants

    /// <summary>
    /// The default number of loops to wait until calling the <see cref="ReportWaitingDelegate"/>.
    /// </summary>
    public const int DEFAULT_REPORT_WAITING_EVERY_LOOPS_COUNT = 5;

    /// <summary>
    /// The default time (in milliseconds) between log checks.
    /// </summary>
    public const int DEFAULT_TIME_BETWEEN_LOG_CHECKS_IN_MILLISECONDS = 1000;

    /// <summary>
    /// The number of loops to wait before timing out if the accepting connections message is not read.
    /// </summary>
    public const int DEFAULT_TIMEOUT_LOOPS_IF_NOT_ACCEPTING_CONNECTIONS = 120;

    /// <summary>
    /// The regex string to check if a server is accepting connections after being started.
    /// </summary>
    public const string SERVER_READY_FOR_CONNECTIONS_REGEX = @"mysqld\.exe: ready for connections\.";

    /// <summary>
    /// The regex string to check information about a started server, like its version, socket name, port, product name and license.
    /// </summary>
    public const string SERVER_INFORMATION_REGEX = @"Version:\s+'(?<Version>\d{1,3}\.\d{1,3}\.\d{1,3})(?:(?<BuildPrefix>-\w+)?(?:(?:-community|-(?:enterprise-)?(?:commercial)(?:-advanced)?)?(?:-log)?))?'\s+socket:\s+'(?<Socket>\w*)'\s+port:\s+(?<Port>\d*)\s+(?<ProductName>MySQL (?:Community|Enterprise) Server(?: - Advanced Edition)?)\s+(?:\(|- )(?<License>(?:GPL|Commercial))\)*";

    /// <summary>
    /// The regex string to check if a server upgrade completed successfully.
    /// </summary>
    public const string SERVER_UPGRADE_COMPLETED_REGEX = @"Server upgrade from (')?(?<OldVersion>\d{4,6})(')? to (')?(?<NewVersion>\d{4,6})(')?( has)? completed";

    /// <summary>
    /// The regex string to check if a server upgrade has been started.
    /// </summary>
    public const string SERVER_UPGRADE_STARTED_REGEX = @"Server upgrade from (')?(?<OldVersion>\d{4,6})(')? to (')?(?<NewVersion>\d{4,6})(')?( has)? started";

    /// <summary>
    /// The regex string to check if a server upgrade failed.
    /// </summary>
    public const string SERVER_UPGRADE_FAILED_REGEX = "Failed to upgrade server";

    #endregion Constants

    #region Fields

    /// <summary>
    /// The <see cref="FileInfo"/> related to the error log file.
    /// </summary>
    private readonly FileInfo _fileInfo;

    /// <summary>
    /// The last accessed position of the error log file.
    /// </summary>
    private long _lastFilePosition;

    /// <summary>
    /// The last recorded size of the error log file.
    /// </summary>
    private long _lastFileSize;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerErrorLog"/> class.
    /// </summary>
    /// <param name="filePath">The file path of the MySQL error log file.</param>
    /// <param name="logLines">A list of error log lines.</param>
    public ServerErrorLog(string filePath, List<string> logLines = null)
    {
      _fileInfo = string.IsNullOrEmpty(filePath)
        ? null
        : new FileInfo(filePath);
      _lastFilePosition = _fileInfo != null && _fileInfo.Exists
        ? _fileInfo.Length
        : -1;
      _lastFileSize = _lastFilePosition;
      FilePath = filePath;
      LogLines = new List<ServerErrorLogLine>();
      if (logLines != null)
      {
        foreach (var logLine in logLines)
        {
          LogLines.Add(ServerErrorLogLine.Parse(logLine));
        }
      }
      
      ReportStatusDelegate = null;
      ReportWaitingDelegate = null;
      ReportWaitingEveryLoopsCount = DEFAULT_REPORT_WAITING_EVERY_LOOPS_COUNT;
      TimeBetweenLogChecksInMilliseconds = DEFAULT_TIME_BETWEEN_LOG_CHECKS_IN_MILLISECONDS;
    }

    #region Properties

    /// <summary>
    /// Gets or sets a <see cref="CancellationToken"/> used to cancel operations.
    /// </summary>
    public CancellationToken CancellationToken { get; set; }

    /// <summary>
    /// Gets the file path of the MySQL error log file.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Gets a list of error log lines.
    /// </summary>
    public List<ServerErrorLogLine> LogLines { get; }

    /// <summary>
    /// Gets an <seealso cref="System.Action"/> to output status messages.
    /// </summary>
    public Action<string> ReportStatusDelegate { get; set;  }

    /// <summary>
    /// Gets an <seealso cref="System.Action"/> to output a waiting character periodically to signal a wait for an ongoing asynchronous operation.
    /// </summary>
    public Action<string> ReportWaitingDelegate { get; set; }

    /// <summary>
    /// Gets or sets the number of loops to wait until calling the <see cref="ReportWaitingDelegate"/>.
    /// </summary>
    public int ReportWaitingEveryLoopsCount { get; set; }

    /// <summary>
    /// Gets or sets the time (in milliseconds) between log checks.
    /// </summary>
    public int TimeBetweenLogChecksInMilliseconds { get; set; }

    #endregion Properties

    /// <summary>
    /// Parses log messages to check if the server is accepting connections.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version being upgraded to.</param>
    /// <param name="fromLogFile">Flag indicating if the parsing is done from the error log file or directly from the list of log lines (being fed from standard output redirection).</param>
    /// <param name="reportStatus">Flag indicating whether added lines are output using the <see cref="ReportStatusDelegate"/>.</param>
    /// <param name="timeoutLoopsIfNotAcceptingConnections">The number of loops to wait before timing out if the accepting connections message is not read.</param>
    /// <returns><c>true</c> if the server is accepting connections, <c>false</c> otherwise.</returns>
    public bool ParseServerAcceptingConnectionMessage(Version serverVersion, bool fromLogFile = true, bool reportStatus = true, int timeoutLoopsIfNotAcceptingConnections = DEFAULT_TIMEOUT_LOOPS_IF_NOT_ACCEPTING_CONNECTIONS)
    {
      return ParseServerMessages(serverVersion, true, fromLogFile, reportStatus, timeoutLoopsIfNotAcceptingConnections).AcceptingConnections;
    }

    /// <summary>
    /// Parses log messages related to a MySQL Server upgrade.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version being upgraded to.</param>
    /// <param name="fromLogFile">Flag indicating if the parsing is done from the error log file or directly from the list of log lines (being fed from standard output redirection).</param>
    /// <param name="timeoutLoopsIfNotAcceptingConnections">The number of loops to wait before timing out if the accepting connections message is not read.</param>
    /// <returns>A <see cref="ServerUpgradeStatus"/> with flags about the parsed messages.</returns>
    public ServerUpgradeStatus ParseServerUpgradeMessages(Version serverVersion, bool fromLogFile = true, int timeoutLoopsIfNotAcceptingConnections = DEFAULT_TIMEOUT_LOOPS_IF_NOT_ACCEPTING_CONNECTIONS)
    {
      return ParseServerMessages(serverVersion, false, fromLogFile, true, timeoutLoopsIfNotAcceptingConnections);
    }

    /// <summary>
    /// Reads new lines added to the error log file and adds them into the <see cref="LogLines"/>.
    /// </summary>
    /// <param name="reportStatus">Flag indicating whether added lines are output using the <see cref="ReportStatusDelegate"/>.</param>
    public void ReadNewLinesFromFile(bool reportStatus)
    {
      if (_fileInfo == null)
      {
        throw new ArgumentNullException(nameof(_fileInfo));
      }

      _fileInfo.Refresh();
      if (!_fileInfo.Exists)
      {
        return;
      }

      var currentFileSize = _fileInfo.Length;
      if (CancellationToken.IsCancellationRequested
          || currentFileSize == _lastFileSize)
      {
        return;
      }

      using (var stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
      {
        if (_lastFilePosition >= 0)
        {
          stream.Seek(_lastFilePosition, SeekOrigin.Begin);
        }

        using (var reader = new StreamReader(stream, Encoding.UTF8))
        {
          while (!CancellationToken.IsCancellationRequested
                 && reader.Peek() >= 0)
          {
            var logLine = ServerErrorLogLine.Parse(reader.ReadLine());
            LogLines?.Add(logLine);
            if (!reportStatus)
            {
              continue;
            }

            ReportStatusDelegate?.Invoke(logLine.Parsed ? logLine.Message : logLine.UnparsedLine);
          }

          _lastFilePosition = stream.Position;
        }
      }

      _lastFileSize = currentFileSize;
    }

    /// <summary>
    /// Parses log messages related to a MySQL Server upgrade.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version being upgraded to.</param>
    /// <param name="onlyAcceptingConnections">Flag indicating if the parsing is done just for the message about accepting connections or for a server upgrade as well.</param>
    /// <param name="fromLogFile">Flag indicating if the parsing is done from the error log file or directly from the list of log lines (being fed from standard output redirection).</param>
    /// <param name="reportStatus">Flag indicating if the status should be reported to the user.</param>
    /// <param name="timeoutLoopsIfNotAcceptingConnections">The number of loops to wait before timing out if the accepting connections message is not read.</param>
    /// <returns>A <see cref="ServerUpgradeStatus"/> with flags about the parsed messages.</returns>
    private ServerUpgradeStatus ParseServerMessages(Version serverVersion, bool onlyAcceptingConnections, bool fromLogFile = true, bool reportStatus = true, int timeoutLoopsIfNotAcceptingConnections = DEFAULT_TIMEOUT_LOOPS_IF_NOT_ACCEPTING_CONNECTIONS)
    {
      var upgradeStatus = new ServerUpgradeStatus();
      var lastErrorLogLinesCount = 0;
      var newErrorLogLinesCount = LogLines.Count;
      var serverUpgradeStartedRegex = onlyAcceptingConnections ? null : new Regex(SERVER_UPGRADE_STARTED_REGEX, RegexOptions.IgnoreCase);
      var serverUpgradeCompletedRegex = onlyAcceptingConnections ? null : new Regex(SERVER_UPGRADE_COMPLETED_REGEX, RegexOptions.IgnoreCase);
      var serverUpgradeFailedRegex = onlyAcceptingConnections ? null : new Regex(SERVER_UPGRADE_FAILED_REGEX, RegexOptions.IgnoreCase);
      var serverReadyForConnectionsRegex = new Regex(SERVER_READY_FOR_CONNECTIONS_REGEX, RegexOptions.IgnoreCase);
      var startedServerInformationRegex = new Regex(SERVER_INFORMATION_REGEX, RegexOptions.IgnoreCase);
      var versionPlainText = $"{serverVersion.Major}{serverVersion.Minor:D2}{serverVersion.Build:D2}";
      var loopCount = 0;
      var loopCountForTimeOut = 0;
      var acceptingConnectionsOrTimeout = false;
      var serverVersionAsString = serverVersion.ToString(3);
      while (!acceptingConnectionsOrTimeout
             && (onlyAcceptingConnections || !upgradeStatus.UpgradeFailed)
             && !CancellationToken.IsCancellationRequested)
      {
        if (lastErrorLogLinesCount != newErrorLogLinesCount)
        {
          if (!onlyAcceptingConnections
              && !upgradeStatus.UpgradeStarted)
          {
            var lastServerUpgradeStartedIndex = LogLines.FindLastIndex(line =>
            {
              var match = serverUpgradeStartedRegex.Match(line.Message);
              return match.Success && match.Groups["NewVersion"].Value.Equals(versionPlainText);
            });
            upgradeStatus.UpgradeStarted = lastServerUpgradeStartedIndex >= 0;
          }

          if (!onlyAcceptingConnections
              && upgradeStatus.UpgradeStarted
              && !upgradeStatus.UpgradeFinished)
          {
            var lastServerUpgradeCompletedIndex = LogLines.FindLastIndex(line =>
            {
              var match = serverUpgradeCompletedRegex.Match(line.Message);
              return match.Success && match.Groups["NewVersion"].Value.Equals(versionPlainText);
            });
            upgradeStatus.UpgradeFinished = lastServerUpgradeCompletedIndex >= 0;
          }

          if (!upgradeStatus.AcceptingConnections)
          {
            var lastServerReadyForConnectionsIndex = LogLines.FindLastIndex(line => serverReadyForConnectionsRegex.Match(line.Message).Success);
            var lastStartedServerInformationRegex = LogLines.FindLastIndex(line =>
            {
              var match = startedServerInformationRegex.Match(line.Message);
              return match.Success && match.Groups["Version"].Value.Equals(serverVersionAsString);
            });
            upgradeStatus.AcceptingConnections = lastServerReadyForConnectionsIndex >= 0
                                                 && lastStartedServerInformationRegex >= 0;
          }

          if (!onlyAcceptingConnections
              && upgradeStatus.UpgradeStarted
              && !upgradeStatus.UpgradeFailed)
          {
            upgradeStatus.UpgradeFailed = LogLines.FindLastIndex(line => serverUpgradeFailedRegex.IsMatch(line.Message)) >= 0;
          }
        }

        var checkForTimeOut = !onlyAcceptingConnections && upgradeStatus.UpgradeFinished
                              || onlyAcceptingConnections;
        acceptingConnectionsOrTimeout = upgradeStatus.AcceptingConnections
                                        || checkForTimeOut && loopCountForTimeOut > timeoutLoopsIfNotAcceptingConnections;
        var delayingTask = Task.Delay(TimeBetweenLogChecksInMilliseconds, CancellationToken);
        if (!CancellationToken.IsCancellationRequested)
        {
          try
          {
            delayingTask.Wait(CancellationToken);
          }
          catch (OperationCanceledException)
          {
          }
        }

        if (fromLogFile)
        {
          ReadNewLinesFromFile(reportStatus);
        }

        lastErrorLogLinesCount = newErrorLogLinesCount;
        newErrorLogLinesCount = LogLines.Count;
        if (checkForTimeOut)
        {
          loopCountForTimeOut++;
        }

        if (++loopCount % ReportWaitingEveryLoopsCount == 0)
        {
          ReportWaitingDelegate?.Invoke(".");
        }
      }

      return upgradeStatus;
    }
  }
}