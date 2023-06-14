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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
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

    private static string _installDirPath;

    private static string _action;

    #endregion

    /// <summary>
    /// Customizes the looks of common dialogs.
    /// </summary>
    private static void CustomizeUtilityDialogs()
    {
      InfoDialog.ApplicationName = Application.ProductName;
      InfoDialog.SuccessLogo = Resources.MainLogo;
      InfoDialog.ErrorLogo = Resources.MainLogo_Error;
      InfoDialog.WarningLogo = Resources.MainLogo_Warn;
      InfoDialog.InformationLogo = Resources.MainLogo;
      PasswordDialog.ApplicationIcon = Resources.mysql_server;
      PasswordDialog.SecurityLogo = Resources.MainLogo;
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

#if DEBUG
        /* Before debugging, update the path to the server installation directory in the "installationDirectory" key of the app.config file.
           The path set as the installation directory must be the root directory of the server installation. 
           This directory is expected to contain the bin, share, etc and other server directories.
           
           For MSI installations this path is usually "C:\Program Files\MySQL\MySQL Server 8.1" or the custom path set during installation.
           For ZIP installations this path is whichever location where the server files were extracted to.
        */
        _installDirPath = ConfigurationManager.AppSettings["installationDirectory"];
#endif

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
        Application.Run(new MainForm(_version, _dataDirPath, _installDirPath, _action));
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
      arguments = arguments.Skip(1).ToArray();
      foreach (var argument in arguments)
      {
        var items = argument.Split('=');
        var option = items[0].Substring(2).ToLowerInvariant();
        var value = items.Length > 1
          ? items[1]
          : null;

        switch (option)
        {
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
      try
      {
        var assembly = Assembly.GetExecutingAssembly();

#if !DEBUG
        // Set install dir.
        var assemblyFileInfo = new FileInfo(assembly.Location);
        var installDirPath = assemblyFileInfo.Directory.Parent.FullName;
        _installDirPath = installDirPath;
#endif


        // Validate install dir.
        var pathToMySqld = Path.Combine(_installDirPath, "bin\\mysqld.exe");
        if (!Directory.Exists(_installDirPath)
            || !File.Exists(pathToMySqld))
        {
          _installDirPath = null;
        }

        // Set version.
        FileVersionInfo versionInfo = null;
        Version versionItem = null;
        if (!string.IsNullOrEmpty(_installDirPath))
        {
          versionInfo = FileVersionInfo.GetVersionInfo(pathToMySqld);
          versionItem = new Version(versionInfo.FileVersion);
        }
        else
        {
          versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
          versionItem = new Version(versionInfo.FileVersion);
        }

        _version = $"{versionItem.Major}.{versionItem.Minor}.{versionItem.Build}";

        // Set default data dir.
        if (string.IsNullOrEmpty(_dataDirPath)
            && versionItem != null)
        {
          _dataDirPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
            $@"MySQL\MySQL Server {versionItem.Major}.{versionItem.Minor}\");
        }
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }


      // Set default action.
      if (string.IsNullOrEmpty(_action))
      {
        _action = "configure";
      }
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
