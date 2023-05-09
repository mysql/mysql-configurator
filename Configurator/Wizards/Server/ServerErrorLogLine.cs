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

using System.Text.RegularExpressions;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Represents a single line read from the error log.
  /// </summary>
  public class ServerErrorLogLine
  {
    #region Constants

    /// <summary>
    /// The regex to parse an error log line.
    /// </summary>
    public const string ERROR_LOG_LINE_REGEX = @"(?<TimeStamp>(?<Year>[0-9]{4})-(?<Month>(0[1-9]|1[0-2]))-(?<Day>(0[1-9]|[1-2][0-9]|3[0-1]))T(?<Time>(2[0-3]|[01][0-9]):[0-5][0-9]:[0-5][0-9]\.(\d)+)Z) (?<Severity>\d)\s\[(?<Type>\w*)\](\s\[(?<InternalCode>\w{2}-\d+)\])?(\s\[(?<Category>\w*)\])?\s(?<Message>.+)";

    #endregion Constants

    #region Properties

    /// <summary>
    /// Gets the category of the message.
    /// </summary>
    public string Category { get; private set; }

    /// <summary>
    /// Gets an internal code for the message.
    /// </summary>
    public string InternalCode { get; private set; }

    /// <summary>
    /// Gets the actual message of the log line.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the line could be parsed.
    /// </summary>
    public bool Parsed { get; private set; }

    /// <summary>
    /// Gets the severity of the error.
    /// </summary>
    public int Severity { get; private set; }

    /// <summary>
    /// Gets the timestamp for the error message.
    /// </summary>
    public string TimeStamp { get; private set; }

    /// <summary>
    /// Gets the type of error message.
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    /// Gets the full unparsed log line.
    /// </summary>
    public string UnparsedLine { get; private set; }

    #endregion Properties

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerErrorLogLine"/> class.
    /// </summary>
    /// <param name="unparsedLine">The full unparsed log line.</param>
    public ServerErrorLogLine(string unparsedLine)
    {
      Parsed = false;
      UnparsedLine = unparsedLine;
      if (string.IsNullOrEmpty(unparsedLine))
      {
        return;
      }

      var regex = new Regex(ERROR_LOG_LINE_REGEX);
      var match = regex.Match(unparsedLine);
      if (!match.Success)
      {
        Message = unparsedLine;
        return;
      }

      Parsed = true;
      TimeStamp = match.Groups["TimeStamp"].Value;
      Severity = int.Parse(match.Groups["Severity"].Value);
      Type = match.Groups["Type"].Value;
      InternalCode = match.Groups["InternalCode"].Value;
      Category = match.Groups["Category"].Value;
      Message = match.Groups["Message"].Value;
    }

    /// <summary>
    /// Gets a <see cref="ServerErrorLogLine"/> from a log file line.
    /// </summary>
    /// <param name="unparsedLine">A log file line.</param>
    /// <returns>A <see cref="ServerErrorLogLine"/> from a log file line.</returns>
    public static ServerErrorLogLine Parse(string unparsedLine)
    {
      return new ServerErrorLogLine(unparsedLine);
    }
  }
}