/* Copyright (c) 2023, 2024, Oracle and/or its affiliates.

  This program is free software; you can redistribute it and/or modify 
  it under the terms of the GNU General Public License, version 2.0, as 
  published by the Free Software Foundation.

  This program is designed to work with certain software (including
  but not limited to OpenSSL) that is licensed under separate terms, as
  designated in a particular file or component or in included license
  documentation. The authors of MySQL hereby grant you an additional
  permission to link the program and your derivative works with the
  separately licensed software that they have either included with
  the program or referenced in the documentation.

  This program is distributed in the hope that it will be useful, but 
  WITHOUT ANY WARRANTY; without even the implied warranty of 
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
  See the GNU General Public License, version 2.0, for more details. 

  You should have received a copy of the GNU General Public License 
  along with this program; if not, write to the Free Software Foundation, Inc., 
  51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA */

using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using MySql.Configurator.Base.Classes;
using MySql.Configurator.Base.Enums;
using MySql.Configurator.Core.Logging;
using MySql.Configurator.Core.Server;
using MySql.Configurator.Core.Settings;
using MySql.Configurator.Properties;
using MySql.Configurator.UI.Dialogs;
using MySql.Configurator.UI.Forms;
using Utilities = MySql.Configurator.Base.Classes.Utilities;

namespace MySql.Configurator
{
  public class Program
  {
    #region Fields

    /// <summary>
    /// The installation directory path of the current installation.
    /// </summary>
    private static string _installDirPath;

    /// <summary>
    /// The version number of the current installation.
    /// </summary>
    private static string _version;

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
        var executionMode = ProcessCommandLineArguments(Environment.GetCommandLineArgs());

        // Do not show form if running in removal mode and option --show-removal-warning was not provided.
        ServerInstallation serverInstallation = null;
        serverInstallation = ServerInstallationManager.LoadServerInstallation(_version, _installDirPath);
        if (executionMode == ExecutionMode.RemoveNoShow)
        {
          var controller = serverInstallation.Controller;
          if (controller == null)
          {
            throw new ArgumentNullException(nameof(controller));
          }

          if (!controller.IsRemovalExecutionNeeded)
          {
            Logger.LogWarning(string.Format(Resources.RemoveWithNoUIWarningMessage));
            return;
          }
        }

        // Uncomment the following line to print to the debug output console messages indicating what control got focus.
        //Application.AddMessageFilter(new LastFocusedControlFilter(true));
        Application.Run(new MainForm(serverInstallation, executionMode));
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
    /// <param name="arguments">The command line options provided by the user.</param>
    /// <returns>An enumeration value representing the execution mode.</returns>
    private static ExecutionMode ProcessCommandLineArguments(string[] arguments)
    {
      var executionMode = ExecutionMode.Configure;
      if (arguments == null)
      {
        throw new ArgumentNullException(nameof(arguments));
      }

      if (arguments.Length > 1)
      {
        arguments = arguments.Skip(1).ToArray();
        foreach (var argument in arguments)
        {
          var option = argument.StartsWith("--")
            ? argument.Substring(2).ToLowerInvariant()
            : null;
          if (option == null)
          {
            throw new ConfiguratorException(ConfiguratorError.InvalidOptionStart, argument);
          }

          if (!Enum.TryParse<ExecutionMode>(option, true, out executionMode))
          {
            throw new ConfiguratorException(ConfiguratorError.InvalidOption, option);
          }
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

        if (string.IsNullOrEmpty(_installDirPath))
        {
          throw new ArgumentNullException(_installDirPath);
        }

        // Validate install dir.
        var pathToMySqld = Path.Combine(_installDirPath, "bin\\mysqld.exe");
        if (!Directory.Exists(_installDirPath)
            || !File.Exists(pathToMySqld))
        {
          throw new ConfiguratorException(ConfiguratorError.MysqldExeNotFound, _installDirPath);
        }

        // Set version.
        var versionInfo = FileVersionInfo.GetVersionInfo(pathToMySqld);
        var mysqldExeVersion = new Version(versionInfo.FileVersion);
        var assemblyVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        var configuratorVersion = new Version(versionInfo.FileVersion);
        if (mysqldExeVersion != configuratorVersion)
        {
          throw new ConfiguratorException(ConfiguratorError.VersionMismatch);
        }

        _version = $"{mysqldExeVersion.Major}.{mysqldExeVersion.Minor}.{mysqldExeVersion.Build}";
      }
      catch (ConfiguratorException ex)
      {
        Logger.LogException(ex);
        throw ex;
      }
      catch (Exception ex)
      {
        Logger.LogException(ex);
      }


      return executionMode;
    }

    static void ReportUnhandledException(Exception ex)
    {
      Logger.LogException(ex);
      InfoDialog.ShowDialog(InfoDialogProperties.GetErrorDialogProperties("Error", string.Format(Resources.UnhandledException, ex.Message)));
    }
  }
}
