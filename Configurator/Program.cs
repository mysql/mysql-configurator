/* Copyright (c) 2023, Oracle and/or its affiliates.

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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Classes.Options;
using MySql.Configurator.Core.Common;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Forms;
using MySql.Configurator.Dialogs;
using MySql.Configurator.Properties;
using Utilities = MySql.Configurator.Core.Classes.Utilities;

namespace MySql.Configurator
{
  public class Program
  {
    #region Fields

    private static string _version;

    private static string _dataDirPath;

    private static string _action;

    #endregion

    /// <summary>
    /// Customizes the looks of some dialogs found in the MySQL.Utility for ExcelInterop.
    /// </summary>
    private static void CustomizeUtilityDialogs()
    {
      InfoDialog.ApplicationName = Application.ProductName;
      InfoDialog.SuccessLogo = Resources.MainLogo_Success;
      InfoDialog.ErrorLogo = Resources.MainLogo_Error;
      InfoDialog.WarningLogo = Resources.MainLogo_Warn;
      InfoDialog.InformationLogo = Resources.MainLogo;
      PasswordDialog.ApplicationIcon = Resources.mysql_server;
      PasswordDialog.SecurityLogo = Resources.MainLogo_Security;
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      try
      {
        AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        Utilities.InitializeLogger(false);
        CustomizeUtilityDialogs();
        Application.ApplicationExit += ApplicationExit;
#if COMMERCIAL
        AppConfiguration.License = LicenseType.Commercial;
#else
        AppConfiguration.License = LicenseType.Community;
#endif

        // Make sure our app cannot run twice.
        var exists = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() > 1;
        if (exists)
        {
          InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties(Resources.AppName, Resources.AppAlreadyRunning));
          return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ProcessCommandLineArguments();
        
        // Uncomment the following line to print to the debug output console messages indicating what control got focus.
        //Application.AddMessageFilter(new LastFocusedControlFilter(true));
        Application.Run(new MainForm(_version, _dataDirPath, _action));
      }
      catch (ConfiguratorException ex)
      {
        InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties("Error loading the specified MySQL Server product", ex.Message));
        Logger.LogError(ex.Message);
      }
      catch (Exception ex)
      {
        ReportUnhandledException(ex);

#if (DEBUG)
        // For internal debug only.
        throw;
#endif
      }
      finally
      {
        Logger.LogInformation("Configurator exit");
      }
    }

    private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs e)
    {
      //The namespace of the project is embeddll, and the embedded dll resources are in the libs folder, so the namespace used here is: embeddll.libs.
      var assembly = Assembly.GetExecutingAssembly();
      var resources = assembly.GetManifestResourceNames();
      string _resName = "MySql.Configurator.Resources.MySql.Data.dll";
      using (var _stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_resName))
      {
        byte[] _data = new byte[_stream.Length];
        _stream.Read(_data, 0, _data.Length);
        return Assembly.Load(_data);
      }
    }

    static void ApplicationExit(object sender, EventArgs e)
    {
    }

    /// <summary>
    /// Processes the command line arguments provided when executing the application.
    /// </summary>
    private static void ProcessCommandLineArguments()
    {
      string[] arguments = Environment.GetCommandLineArgs();
      // Process arguments.
      //if (arguments.Length != 3)
      //{
      //  throw new ConfiguratorException(ConfiguratorError.IncorrectArgumentCount);
      //}

      arguments = arguments.Skip(1).ToArray();
      foreach (var argument in arguments)
      {
        var items = argument.Split('=');
        var option = items[0].Substring(2);
        var value = items.Length > 1
          ? items[1]
          : null;

        switch (option)
        {
          case "version":
            _version = value;
            if (!Version.TryParse(_version, out Version tempVersion))
            {
              throw new ConfiguratorException(ConfiguratorError.InvalidVersion, _version);
            }

            if (_version.Split('.').Length < 3)
            {
              throw new ConfiguratorException(ConfiguratorError.ShortVersion, _version);
            }
            break;

          case "data_dir":
            _dataDirPath = value;
            break;

          case "configure":
          case "remove":
          case "upgrade":
            _action = option;
            break;

          default:
            throw new ConfiguratorException(ConfiguratorError.InvalidOption, option);
        }
      }

      // Set default version.
      if (string.IsNullOrEmpty(_version))
      {
        var assembly = Assembly.GetExecutingAssembly();
        var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
        var version = new Version(fvi.FileVersion);
        _version = $"{version.Major}.{version.Minor}.{version.Build}";
      }

      // Set default data dir.
      if (string.IsNullOrEmpty(_dataDirPath))
      {
        var version = new Version(_version);
        _dataDirPath = Path.Combine(
          Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
          $@"MySQL\MySQL Server {version.Major}.{version.Minor}\");
      }

      // Set default action.
      if (string.IsNullOrEmpty(_action))
      {
        _action = "configure";
      }
    }

    /// <summary>
    /// Processes the command line arguments provided when executing the application without enforcing the order on which to provide each argument.
    /// </summary>
    /// <param name="arguments">An array of string arguments.</param>
    /// <returns><c>true</c> if the processing of the command line arguments was successful; otherwise, <c>false</c>.</returns>
    private static bool ProcessCommandLineArguments(string[] arguments)
    {
      for (int index = 0; index < arguments.Length; index++)
      {
        arguments[index] = arguments[index].ToLowerInvariant();
      }

      var options = new CommandLineOptions
      {
        new CommandLineOption("license", arg => ProcessArgument(new KeyValuePair<string, string>("license", arg)), true, "community,commercial"),
        new CommandLineOption("version", arg => ProcessArgument(new KeyValuePair<string, string>("version", arg)), true),
        new CommandLineOption("productcode", arg => ProcessArgument(new KeyValuePair<string, string>("productcode", arg)), true),
        new CommandLineOption("nosplash", arg => ProcessArgument(new KeyValuePair<string, string>("nosplash", arg)), true, "true,false")
      };
      options.Parse(arguments);
      return true;
    }

    /// <summary>
    /// Processes the argument found in the key-value pair.
    /// </summary>
    /// <param name="keyValuePair">Contains the type of argument to process and the value being assigned to that argument.</param>
    private static void ProcessArgument(KeyValuePair<string, string> keyValuePair)
    {
      if (string.IsNullOrEmpty(keyValuePair.Key))
      {
        return;
      }

      switch (keyValuePair.Key.ToLowerInvariant())
      {
        case "version":
          Version v;
          if (!Version.TryParse(keyValuePair.Value, out v))
          {
            throw new Exception(string.Format(Resources.InvalidCommandLineArguments, $"{keyValuePair.Key}={keyValuePair.Value}"));
          }

          break;
        default:
          throw new Exception(string.Format(Resources.InvalidCommandLineArguments, keyValuePair.Key));
      }
    }

    /// <summary>
    /// Cancels the processing of arguments and sends an error message.
    /// </summary>
    private static void CancelArgumentProcessing()
    {
      InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties("Error", Resources.BadLaunchWrongArguments));
    }

    static void ReportUnhandledException(Exception ex)
    {
      Logger.LogException(ex);
      InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties("Error", string.Format(Resources.UnhandledException, ex.Message)));
    }
  }
}
