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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.Devices;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.IniFile.Template.Formula;
using MySql.Configurator.Properties;

namespace MySql.Configurator.Core.IniFile.Template
{
  public sealed class IniTemplate
  {
    #region Fields

    private FormulaEngine _formulaEngine;
    private ServerInstallationType _iniServerType;
    private List<DeprecatedServerVariable> _deprecatedServerVariables;
    private Queue _output;
    private string _template;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of <see cref="IniTemplate"/> with default values.
    /// </summary>
    /// <param name="serverVersion">The MySQL Server version.</param>
    public IniTemplate(Version serverVersion, ServerInstallationType iniServerType)
    {
      _deprecatedServerVariables = null;
      SetDefaults();
      InitializeDeprecatedServerVariables();
      ServerVersion = serverVersion;
      ServerType = iniServerType;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="IniTemplate"/> from an existing config file.
    /// </summary>
    /// <param name="baseDir">Directory of the MySQL Server install folder</param>
    /// <param name="dataDir">Directory of the MySQL Data folder</param>
    /// <param name="existingConfigFilePath">Path of the existing configuration file</param>
    /// <param name="serverVersion">The MySQL Server version.</param>
    public IniTemplate(string baseDir, string dataDir, string existingConfigFilePath, Version serverVersion, ServerInstallationType iniServerType)
      :this(serverVersion, iniServerType)
    {
      BaseDir = baseDir;
      DataDir = dataDir;

      ConfigurationFile = existingConfigFilePath;
      if (File.Exists(ConfigurationFile))
      {
        BackupConfigFile(ConfigurationFile, Path.GetDirectoryName(ConfigurationFile));
      }

      IsValid = ParseConfigurationFile(ConfigurationFile);
    }

    /// <summary>
    /// Initializes a new instance of <see cref="IniTemplate"/> from an ini file template.
    /// </summary>
    /// <param name="baseDir">Directory of the MySQL Server install folder</param>
    /// <param name="dataDir">Directory of the MySQL Data folder</param>
    /// <param name="templateName">The path of the base template that will be used to create config file </param>
    /// <param name="outputDir">Directory where the config file will be located</param>
    /// <param name="configFileName">The name of the config file that will be created</param>
    /// <param name="serverVersion">The MySQL Server version.</param>
    public IniTemplate(string baseDir, string dataDir, string templateName, string outputDir, string configFileName, Version serverVersion, ServerInstallationType iniServerType)
      :this(serverVersion, iniServerType)
    {
      BaseDir = baseDir;
      DataDir = dataDir;
      ConfigurationFile = outputDir.Contains(configFileName) ? outputDir : Path.Combine(outputDir, configFileName);
      if (File.Exists(ConfigurationFile))
      {
        BackupConfigFile(ConfigurationFile, outputDir);
      }

      _template = templateName;
      IsValid = ParseConfigurationFile(_template);
    }

    #region Properties

    public string AuthenticationPolicy { get; set; }
    public string BaseDir { get; set; }
    public string ConfigurationFile { get; set; }
    public string DataDir { get; set; }
    public List<DeprecatedServerVariable> DeprecatedVariablesForVersion
    {
      get
      {
        if (ServerVersion == null || _deprecatedServerVariables == null)
        {
          return null;
        }

        var serverSeries = ServerVersion.GetServerSeries();
        return _deprecatedServerVariables.FindAll(dv => (dv.Version == null
                                                         && dv.Series.HasFlag(serverSeries)) ||
                                                        (dv.Version != null
                                                         && ServerVersion >= dv.Version));
      }
    }
    public string DefaultCharacterSet { get; set; }
    public string DefaultStorageEngine { get; set; }
    public bool EnableNamedPipe { get; set; }
    public bool EnableNetworking { get; set; }
    public bool EnableQueryCache { get; set; }
    public bool EnableQueryType { get; set; }
    public bool EnableSharedMemory { get; set; }
    public bool EnableStrictMode { get; set; }
    public string GeneralLog { get; set; }
    public string GeneralLogFile { get; set; }
    public double InnoDBBPSUsage { get; set; }
    public string InnoDBHomeDir { get; set; }
    public bool IsValid { get; private set; }
    public string LogBin { get; set; }
    public string LogError { get; set; }
    public string LogOutput { get; set; }
    public string LongQueryTime { get; set; }
    public uint LooseMySqlXPort { get; set; }
    public LowerCaseTableNamesTypes LowerCaseTableNames { get; set; }
    public string MemoryName { get; set; }
    public double MyisamUsage { get; set; }
    public string NamedPipeFullAccessGroup { get; set; }
    public double NumberConnections { get; set; }
    public bool OutputExists { get; private set; }
    public string PipeName { get; set; }
    public string PluginLoad { get; set; }
    public uint Port { get; set; }
    public string ReportHost { get; set; }
    public string SecureFilePriv { get; set; }
    public MySqlAuthenticationPluginType DefaultAuthenticationPlugin { get; set; }
    public uint? ServerId { get; set; }

    public Version ServerVersion { get; }

    public ServerInstallationType ServerType
    {
      get
      {
        return _iniServerType;
      }
      set
      {
        _iniServerType = value;
        double systemAvailableMemory = 536870912; // By default, assume 512MB of available memory.
        double mysqlMemoryPercentage = 0.08333;   // By default, all servers should use 1/12 available system memory.
        try
        {
          ComputerInfo ci = new ComputerInfo();
          systemAvailableMemory = (double)ci.AvailablePhysicalMemory;
        }
        catch
        {
          // Use the default value of 512MB.
        }

        _formulaEngine.AssignFormulaVariable("cpus", Environment.ProcessorCount.ToString());

        switch (_iniServerType)
        {
          case ServerInstallationType.Dedicated:
            mysqlMemoryPercentage = 0.90;
            _formulaEngine.AssignFormulaVariable("server_type", "1");
            break;

          case ServerInstallationType.Server:
            mysqlMemoryPercentage = 0.50;
            _formulaEngine.AssignFormulaVariable("server_type", "2");
            break;

          case ServerInstallationType.Manual:
            _formulaEngine.AssignFormulaVariable("server_type", "4");
            break;

          case ServerInstallationType.InternalUnitTest:
            mysqlMemoryPercentage = 0;
            systemAvailableMemory = 0.0;
            _formulaEngine.AssignFormulaVariable("cpus", "0.0");
            _formulaEngine.AssignFormulaVariable("server_type", "5");
            break;

          default: // IniServerType.Developer
            _formulaEngine.AssignFormulaVariable("server_type", "3");
            if (systemAvailableMemory < (48 * 1024 * 1024))
            {
              systemAvailableMemory = 48 * 1024 * 1024;
              mysqlMemoryPercentage = 1.0;
            }
            break;
        }

        _formulaEngine.AssignFormulaVariable("memory", (systemAvailableMemory * mysqlMemoryPercentage).ToString(CultureInfo.CurrentCulture));
      }
    }

    public bool SkipInnodb { get; set; }
    public string SlowQueryLog { get; set; }
    public string SlowQueryLogFile { get; set; }
    public double UseQueryCache { get; set; }

    #endregion Properties

    /// <summary>
    /// Parses the given configuration file, which can either be an instance of our template
    /// or an existing configuration file. Keep in mind that we cannot rely on our template syntax
    /// entirely as the user might have manually changed the configuration file for his server.
    /// </summary>
    public bool ParseConfigurationFile(string templateName)
    {
      if (_output == null)
      {
        _output = new Queue();
      }
      else
      {
        _output.Clear();
      }

      bool greatSuccess = false;
      try
      {
        StreamReader reader = null;
        if (File.Exists(templateName))
        {
          reader = new StreamReader(templateName);
        }
        else
        {
          var assembly = Assembly.GetExecutingAssembly();
          var resources = assembly.GetManifestResourceNames();
          Stream stream = assembly.GetManifestResourceStream($"MySql.Configurator.Core.Resources.{templateName}");
          reader = new StreamReader(stream);
        }
        
        string currentSection = "default";
        string currentLine = null;
        var deprecatedVariables = DeprecatedVariablesForVersion;
        while (!reader.EndOfStream)
        {
          var previousLine = currentLine;
          currentLine = reader.ReadLine();
          if (currentLine == null
              || (string.IsNullOrWhiteSpace(currentLine)
                  && string.IsNullOrWhiteSpace(previousLine))
              || currentLine.StartsWith("##")   // Template comment, do not output.
              || (deprecatedVariables != null
                  && deprecatedVariables.Any(dv => currentLine.IndexOf(dv.Name, StringComparison.OrdinalIgnoreCase) >= 0)))
          {
            continue;
          }

          // A formula line in the form of
          // # [VARIABLE_NAME]="Formula", "Options"
          if (currentLine.StartsWith("# ["))
          {
            ParseFormula(currentLine, reader);
          }
          else
          {
            bool processed = false;

            // See if we enter a new section in the file. We need that later to translate
            // configuration value names to our internal variable names.
            Regex section = new Regex(@"\s*\[(?<section>[a-zA-Z]+)]");
            Match sectionMatch = section.Match(currentLine);
            if (sectionMatch.Success)
            {
              currentSection = sectionMatch.Groups["section"].Value.ToLowerInvariant();
            }
            else
            {
              // Check for simple name=value pairs not associated with a formula and process them.
              Regex nameValuePair = new Regex(@"^(?<disabled>[#\s]+)?(?<name>[a-zA-Z_-]+)(\s*)(?:$|=(?<value>.+)?)");
              Match match = nameValuePair.Match(currentLine);
              if (match.Success)
              {
                // Process all those settings which can be set through the UI during the configuration
                // step. Everything else will just get passed through.
                // The list of supported var names might need to be passed in by the UI
                string name = match.Groups["name"].Value.ToLowerInvariant();
                string variableName = string.Empty;
                switch (name)
                {
                  case "enable-named-pipe":
                    variableName = "SERVER_PIPE";
                    break;

                  case "named-pipe-full-access-group":
                    variableName = "NAMED_PIPE_FULL_ACCESS_GROUP";
                    break;

                  case "port":
                    variableName = currentSection == "client" ? "CLIENT_PORT" : "SERVER_PORT";
                    break;

                  case "skip-networking":
                    variableName = "SERVER_SKIP"; // Only possible with a server section.
                    break;

                  case "shared-memory":
                    variableName = "SHARED_MEMORY";
                    break;

                  case "shared-memory-base-name":
                    variableName = "SHARED_MEMORY_BASE_NAME";
                    break;

                  case "socket":
                    variableName = currentSection == "client" ? "CLIENT_SOCKET" : "SERVER_SOCKET";
                    break;

                }

                if (variableName.Length > 0)
                {
                  processed = true;
                  string value = match.Groups["value"].Value;
                  var itv = new IniTemplateVariable(currentLine, variableName, value, string.Empty)
                  {
                    DefaultValue = value,
                    Disabled = match.Groups["disabled"].Value.Contains("#"),
                    OutputParameter = match.Groups["name"].Value
                  };

                  switch (variableName)
                  {
                    case "SERVER_DEFAULT_AUTHENTICATION_PLUGIN":
                      MySqlAuthenticationPluginType defaultAuthenticationPlugin;
                      if (!string.IsNullOrEmpty(itv.DefaultValue)
                          && Enum.TryParse(itv.DefaultValue, out defaultAuthenticationPlugin))
                      {
                        DefaultAuthenticationPlugin = defaultAuthenticationPlugin;
                      }
                      break;

                    case "SERVER_PORT":
                      if (itv.DefaultValue != null)
                      {
                        Port = uint.Parse(value);
                      }
                      itv.Formula = "port";
                      break;

                    case "SERVER_SKIP":
                      EnableNetworking = itv.Disabled;
                      break;

                    case "SERVER_SOCKET":
                      PipeName = itv.DefaultValue;
                      break;

                    case "SHARED_MEMORY_BASE_NAME":
                      MemoryName = itv.DefaultValue;
                      break;

                    case "SERVER_PIPE":
                      EnableNamedPipe = !itv.Disabled;
                      break;

                    case "SHARED_MEMORY":
                      EnableSharedMemory = !itv.Disabled;
                      break;

                    case "QUERY_CACHE_SIZE":
                      EnableQueryCache = !itv.Disabled;
                      break;

                    case "QUERY_CACHE_TYPE":
                      EnableQueryType = !itv.Disabled;
                      break;

                    case "NAMED_PIPE_FULL_ACCESS_GROUP":
                      NamedPipeFullAccessGroup = value;
                      break;
                  }

                  _output.Enqueue(itv);
                }
              }
            }

            // Anything else we just pipe through.
            if (!processed)
            {
              _output.Enqueue(new IniTemplateStatic(currentLine));
            }
          }
        }

        reader.Close();
        reader.Dispose();
        greatSuccess = true;
      }
      catch (Exception ex)
      {
        _output.Clear();
        Logger.LogError(ex.Message);
      }

      return greatSuccess;
    }

    /// <summary>
    /// Processes the current ini template and saves it to disk as defined in the ConfigurationFile property.
    /// </summary>
    /// <param name="isReconfigure">Indicates if an existing ini file is being reconfigured.</param>
    /// <param name="writeTemplate">Indicates if the template should replace the existing ini file.</param>
    /// <param name="skipExistingValues">If an ini file already exists, indicates if existing values should not be replaced with the ones in the template.</param>
    public void ProcessTemplate(bool isReconfigure = false, bool writeTemplate = true, bool skipExistingValues = false)
    {
      _formulaEngine.AssignFormulaVariable("basedir", string.Format("\"{0}\"", BaseDir));
      _formulaEngine.AssignFormulaVariable("datadir", Path.Combine(DataDir, "Data")); // String.Format("\"{0}\\data\\\"", DataDir));
      _formulaEngine.AssignFormulaVariable("port", Port.ToString());
      _formulaEngine.AssignFormulaVariable("socket", PipeName);
      _formulaEngine.AssignFormulaVariable("shared_memory_base_name", MemoryName);

      _formulaEngine.AssignFormulaVariable("default_storage_engine", DefaultStorageEngine);
      _formulaEngine.AssignFormulaVariable("default_character_set", DefaultCharacterSet);
      _formulaEngine.AssignFormulaVariable("myisam_percentage", MyisamUsage.ToString(CultureInfo.CurrentCulture));
      _formulaEngine.AssignFormulaVariable("innodb_buffer_pool_size_percentage", InnoDBBPSUsage.ToString(CultureInfo.CurrentCulture));
      _formulaEngine.AssignFormulaVariable("active_connections", NumberConnections.ToString(CultureInfo.CurrentCulture));
      _formulaEngine.AssignFormulaVariable("query_cache_pct", UseQueryCache.ToString(CultureInfo.CurrentCulture));

      _formulaEngine.AssignFormulaVariable("log_out", LogOutput);
      _formulaEngine.AssignFormulaVariable("gen_query", GeneralLog);
      _formulaEngine.AssignFormulaVariable("gen_query_file", GeneralLogFile);
      _formulaEngine.AssignFormulaVariable("slow_query", SlowQueryLog);
      _formulaEngine.AssignFormulaVariable("slow_query_file", SlowQueryLogFile);
      _formulaEngine.AssignFormulaVariable("long_query_time", LongQueryTime);
      _formulaEngine.AssignFormulaVariable("log_bin", LogBin);
      _formulaEngine.AssignFormulaVariable("log_error", LogError);

      _formulaEngine.AssignFormulaVariable("server_id", ServerId.HasValue ? ServerId.ToString() : string.Empty);
      _formulaEngine.AssignFormulaVariable("lower_case_table_names", ((int)LowerCaseTableNames).ToString());
      _formulaEngine.AssignFormulaVariable("bitedness", Win32.Is64BitOs ? "0" : "1");
      _formulaEngine.AssignFormulaVariable("secure_file_priv", SecureFilePriv);
      _formulaEngine.AssignFormulaVariable("plugin_load", PluginLoad);
      _formulaEngine.AssignFormulaVariable("loose_mysqlx_port", LooseMySqlXPort.ToString());
      _formulaEngine.AssignFormulaVariable("named_pipe_full_access_group", NamedPipeFullAccessGroup);
      if (ServerVersion.ServerSupportsDefaultAuthenticationPluginVariable())
      {
        _formulaEngine.AssignFormulaVariable("default_authentication_plugin", DefaultAuthenticationPlugin.GetDescription());
      }
      else
      {
        _formulaEngine.AssignFormulaVariable("authentication_policy", AuthenticationPolicy);
      }

      // In this case we don't want the existing file to be replaced.
      if (!writeTemplate)
      {
        return;
      }

      if (File.Exists(ConfigurationFile) && !isReconfigure)
      {
        UpgradeIniFile(skipExistingValues);
        return;
      }

      var writer = new StreamWriter(ConfigurationFile);
      foreach (object obj in _output)
      {
        if (obj is IniTemplateVariable)
        {
          var itv = obj as IniTemplateVariable;
          string result;
          if (itv.Options != "NO_PARSE")
          {
            _formulaEngine.Parse(itv.Formula);
            try
            {
              result = _formulaEngine.Evaluate();
            }
            catch
            {
              itv.Disabled = true;
              result = itv.DefaultValue;
            }
          }
          else
          {
            result = itv.DefaultValue;
          }

          switch (itv.VariableName)
          {
            case "DEFAULT":
            case "STATE_CHANGE":
              // continue;
              break;

            case "CLIENT_PORT":
            case "SERVER_PORT":
              itv.Disabled = !EnableNetworking;
              break;

            case "SERVER_PIPE":
              itv.Disabled = !EnableNamedPipe;
              break;

            case "SHARED_MEMORY":
              itv.Disabled = !EnableSharedMemory;
              break;

            case "SHARED_MEMORY_BASE_NAME":
              itv.Disabled = !EnableSharedMemory;
              break;

            case "CLIENT_PIPE":
            case "CLIENT_SOCKET":
            case "SERVER_SOCKET":
              itv.Disabled = !EnableNamedPipe;
              break;

            case "SERVER_SKIP":
              itv.Disabled = EnableNetworking;
              break;

            case "QUERY_CACHE_SIZE":
              itv.Disabled = !EnableQueryCache;
              break;

            case "QUERY_CACHE_TYPE":
              itv.Disabled = !EnableQueryType;
              break;

            case "LOG_BIN":
              itv.Disabled = string.IsNullOrEmpty(LogBin);
              break;

            case "SERVER_ID":
              itv.Disabled = !ServerId.HasValue;
              break;

            case "SECURE_FILE_PRIV":
              itv.Disabled = string.IsNullOrEmpty(SecureFilePriv);
              break;

            case "PLUGIN_LOAD":
              itv.Disabled = string.IsNullOrEmpty(PluginLoad);
              break;

            case "LOOSE_MYSQLX_PORT":
              itv.Disabled = !ServerVersion.ServerSupportsXProtocol();
              break;

            case "SQL_MODE":
              itv.Disabled = true;
              break;

            case "INNODB_HOME":
              if (!string.IsNullOrEmpty(InnoDBHomeDir))
              {
                itv.Disabled = false;
                _formulaEngine.AssignFormulaVariable("innodb_home", InnoDBHomeDir);
              }
              else
              {
                itv.Disabled = true;
              }
              break;

            case "INNODB_LOG_FILE_SIZE":
              if (!string.IsNullOrEmpty(itv.DefaultValue))
              {
                result = itv.DefaultValue;
                itv.ReduceResult = false;
              }
              break;

            case "SKIP_INNODB":
              itv.Disabled = SkipInnodb;
              break;

            case "NAMED_PIPE_FULL_ACCESS_GROUP":
              itv.Disabled = !EnableNamedPipe;
              itv.SupportsEmptyResult = true;
              break;
          }

          if (itv.VariableName == "STATE_CHANGE" || !itv.Valid)
          {
            continue;
          }
          string assignment = string.Empty;
          if (result.Length > 0
              || itv.SupportsEmptyResult)
          {
            assignment = "=" + (itv.ReduceResult ? ReduceBytesToString(double.Parse(result)) : result);
          }

          writer.WriteLine((itv.Disabled ? "# " : string.Empty) + itv.OutputParameter + assignment);
        }
        else
        {
          var its = (IniTemplateStatic)obj;
          writer.WriteLine(its.LineAsRead);
        }
      }

      writer.Close();
      writer.Dispose();
    }

    /// <summary>
    /// Identifies if there are differences between the existing ini file and the template that needs to be applied during
    /// the upgrade.
    /// </summary>
    /// <param name="version">The server package version.</param>
    /// <returns><c>true</c> if the upgrade is required; otherwise, <c>false</c>.</returns>
    public bool IsUpgradeIniFileRequired(Version version)
    {
      // Load the existing ini file into memory.
      var oldIniFile = new IniFile(ConfigurationFile);

      // Replace the existing ini file with a fresh one created from the template.
      ProcessTemplate(true, false);

      // Compare both ini files and identify if changes are needed for the existing file.
      var templateBase = Path.Combine("", "my-template{0}.ini");
      string templateFile = null;
      if (version.Major >= 8
          && version.Minor > 0)
      {
        templateFile = string.Format(templateBase, $"-{version.Major}.x");
      }
      else
      {
        templateFile = string.Format(templateBase, $"-{version.Major}.{version.Minor}");
      }
      
      if (!File.Exists(templateFile))
      {
        return false;
      }

      var iniTemplateFile = new IniFile(templateFile);
      return iniTemplateFile.Lines.FindAll(s => s.IniLineType == IniLineType.Section && !oldIniFile.SectionExists(s.Section)).Count > 0
             || oldIniFile.Lines.FindAll(oldLine => DeprecatedVariablesForVersion.Any(dv => dv.Name.Equals(oldLine.Key, StringComparison.OrdinalIgnoreCase))).Count > 0
             || iniTemplateFile.Lines.FindAll(newline =>
                                                newline.IniLineType == IniLineType.KeyValuePair
                                                | newline.IniLineType == IniLineType.Flag).Count > 0;
    }

    /// <summary>
    /// Recalculates the values for keys and adds new ones from the ini template.
    /// </summary>
    public void UpgradeIniFile(bool skipExistingValues = false)
    {
      // Load the existing ini file into memory.
      IniFile oldIniFile = new IniFile(ConfigurationFile);

      // Replace the existing ini file with a fresh one created from the template.
      ProcessTemplate(true);

      // Load the refreshed file created from the template into memory.
      IniFile iniTemplateFile = new IniFile(ConfigurationFile);

      // Now, to produce a merged new ini file that contains everything we need, first we need to add new sections if any.
      var newIniFileSections = iniTemplateFile.Lines.FindAll(s => s.IniLineType == IniLineType.Section && !oldIniFile.SectionExists(s.Section));
      foreach (var newSection in newIniFileSections)
      {
        if (oldIniFile.SectionExists(newSection.Section))
        {
          continue;
        }

        oldIniFile.Lines.Add(new IniLineFullDetail(IniLineType.EmptyLine, false, oldIniFile.GetLastSection()));
        oldIniFile.Lines.Add(new IniLineFullDetail(IniLineType.Section, false, newSection.Section));
      }

      // We need to remove any deprecated Server variables, otherwise they will create conflicts with the new Server version.
      var deprecatedVariablesInSeries = DeprecatedVariablesForVersion;
      if (deprecatedVariablesInSeries != null)
      {
        var deprecatedLines = oldIniFile.Lines.FindAll(oldLine => deprecatedVariablesInSeries.Any(dv => dv.Name.Equals(oldLine.Key, StringComparison.OrdinalIgnoreCase)));
        foreach (var deprecatedLine in deprecatedLines)
        {

          // First we remove any comments related to the deprecated lines
          // and we keep inserting these comments on top of the key until we're done working with them.
          int previousLineIndex = oldIniFile.Lines.IndexOf(deprecatedLine) - 1;
          while (previousLineIndex >= 0
                 && oldIniFile.Lines[previousLineIndex].IniLineType != IniLineType.EmptyLine
                 && oldIniFile.Lines[previousLineIndex].IsCommented)
          {
            oldIniFile.Lines.RemoveAt(previousLineIndex);
            previousLineIndex--;
          }

          // Now we remove the deprecated line itself.
          oldIniFile.Lines.Remove(deprecatedLine);
        }
      }

      // Then we need to replace the old values for keys from the old ini file for the recalculated ones from the refreshed file.
      var recalculatedLines = iniTemplateFile.Lines.FindAll(newline =>
                                                              newline.IniLineType == IniLineType.KeyValuePair
                                                              | newline.IniLineType == IniLineType.Flag);
      foreach (var recalculatedLine in recalculatedLines)
      {
        // Find the index of correspondent variable from the refreshed file to old ini file.
        var index = oldIniFile.Lines.FindLastIndex(oldLine => oldLine.Key == recalculatedLine.Key && oldLine.Section == recalculatedLine.Section);

        if (index >= 0)
        {
          // Skip replacement of existing variables if flag is set to true.
          if (skipExistingValues)
          {
            continue;
          }

          // If found, replace the old value of the key with the recalculated one.
          oldIniFile.Lines[index].Value = recalculatedLine.Value;
          oldIniFile.Lines[index].IsCommented = recalculatedLine.IsCommented;
        }
        else
        {
          // If the key is new, we add it to the correspondent section on the old ini file.
          int indexForInsertOnOldFile = oldIniFile.GetLastIndexForSection(recalculatedLine.Section);
          bool collectionLimitReached = indexForInsertOnOldFile >= oldIniFile.Lines.Count;

          // We add a new line at the bottom of the section on the old file.
          if (collectionLimitReached)
          {
            //If it is found at the end of the file we need to use Add() instead of Insert() to avoid an out of bounds exception.
            oldIniFile.Lines.Add(new IniLineFullDetail(IniLineType.EmptyLine, false, oldIniFile.GetLastSection()));
            oldIniFile.Lines.Add(new IniLineFullDetail(IniLineType.KeyValuePair, recalculatedLine.IsCommented, oldIniFile.GetLastSection(), recalculatedLine.Key, recalculatedLine.Value));
          }
          else
          {
            oldIniFile.Lines.Insert(indexForInsertOnOldFile++, new IniLineFullDetail(IniLineType.EmptyLine, false, oldIniFile.GetLastSection()));
            oldIniFile.Lines.Insert(indexForInsertOnOldFile, new IniLineFullDetail(IniLineType.KeyValuePair, recalculatedLine.IsCommented, oldIniFile.GetLastSection(), recalculatedLine.Key, recalculatedLine.Value));
          }

          // Then we need to gather all previous comments in top of the key value pair and insert them after that empty space.
          int recalculatedLinePreviousIndex = iniTemplateFile.Lines.IndexOf(recalculatedLine) - 1;

          // And we keep inserting these comments on top of the key until we're done working with them.
          while (iniTemplateFile.Lines[recalculatedLinePreviousIndex].IniLineType != IniLineType.EmptyLine
                 && iniTemplateFile.Lines[recalculatedLinePreviousIndex].IsCommented)
          {
            oldIniFile.Lines.Insert(collectionLimitReached ? oldIniFile.Lines.Count - 1 : indexForInsertOnOldFile, iniTemplateFile.Lines[recalculatedLinePreviousIndex]);
            recalculatedLinePreviousIndex--;

            // We need to break the while circle if we had reached the top of the template file.
            if (recalculatedLinePreviousIndex < 0) break;
          }
        }
      }

      // Here we just write the merged line by line result into the ini file.
      var writer = new StreamWriter(ConfigurationFile);
      IniLineFullDetail previousLine = null;
      foreach (var line in oldIniFile.Lines)
      {
        if (previousLine != null
            && previousLine.IniLineType == IniLineType.EmptyLine
            && line.IniLineType == IniLineType.EmptyLine)
        {
          continue;
        }

        var sb = new StringBuilder();
        if (line.IsCommented)
        {
          sb.Append("# ");
        }

        switch (line.IniLineType)
        {
          case IniLineType.Comment:
            sb.Append(line.Key);
            break;

          case IniLineType.Section:
            sb.Append("[" + line.Section + "]");
            break;

          case IniLineType.KeyValuePair:
            sb.Append(line.Key + "=" + line.Value);
            break;

          case IniLineType.Flag:
            sb.Append(line.Key);
            break;
        }

        writer.WriteLine(sb.ToString());
        previousLine = line;
      }

      writer.Close();
      writer.Dispose();
    }

    private void BackupConfigFile(string configFile, string outputDir)
    {
      // Create a backup of the existing file(s).
      string datetime = DateTime.Now.GetDateTimeFormats('s')[0].Replace(':', '-');
      string newConfig = $"{outputDir}my_{datetime}.ini";
      int i = 0;
      while (File.Exists(newConfig))
      {
        i += 1;
        newConfig = $"{outputDir}my_{datetime}_{i}.ini";
      }

      File.Copy(configFile, newConfig);
      OutputExists = true;
    }

    /// <summary>
    /// Parses a single formula line plus another line following it where necessary.
    /// </summary>
    private void ParseFormula(string currentLine, StreamReader reader)
    {
      // Assign value to variable.
      var templateFormulaExp = new Regex(@"\[(?<var_name>[^@]+)\]=""(?<formula>[^""]+)""((?:,\s*"")(?<options>[^@]+)?(?:""))?");
      var m = templateFormulaExp.Match(currentLine);
      if (!m.Success)
      {
        return;
      }

      // Ensure element applies for the current server version.
      string variableName = m.Groups["var_name"].Value;
      if (variableName.Equals("VERSION_MAX", StringComparison.OrdinalIgnoreCase)
          || variableName.Equals("VERSION_MIN", StringComparison.OrdinalIgnoreCase))
      {
        try
        {
          var versionString = m.Groups["formula"].Value;
          var versionParts = versionString.Count(character => character == '.') + 1;
          for (; versionParts < 4; versionParts++)
          {
            versionString += ".0";
          }

          var version = new Version(versionString);
          if ((variableName == "VERSION_MAX"
              && ServerVersion > version)
              || (variableName == "VERSION_MIN"
                  && ServerVersion < version))
          {
            // Skip comments.
            do
            {
              currentLine = reader.ReadLine();
            } while (currentLine.StartsWith("#"));
          }
        }
        catch (Exception)
        {
          Logger.LogWarning(string.Format(Resources.VersionProcessingFailed, variableName));
        }

        return;
      }

      // Process formula.
      // If manual server config type is selected, we skip STATE_CHANGE variables.
      // If other server config type is selected, we skip DEFAULT variables.
      if ((_iniServerType == ServerInstallationType.Manual
           && variableName.Equals("STATE_CHANGE", StringComparison.OrdinalIgnoreCase)
           && !variableName.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase))
          || (_iniServerType != ServerInstallationType.Manual
              && variableName.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase)))
      {
        return;
      }
      else
      {
        var itv = new IniTemplateVariable(currentLine, variableName, m.Groups["formula"].Value, m.Groups["options"].Value);
        if (!variableName.Equals("STATE_CHANGE", StringComparison.OrdinalIgnoreCase))
        {
          currentLine = reader.ReadLine();
          if (variableName.Equals("DEFAULT", StringComparison.OrdinalIgnoreCase)
              && currentLine.StartsWith("# ["))
          {
            while (currentLine.StartsWith("# ["))
            {
              currentLine = reader.ReadLine();
            }
          }
          

          Regex templateOutputFormat = new Regex(@"(?<disabled>[#\s]+)?(?<output>.+)=(?<default>.+)?");
          Match m2 = templateOutputFormat.Match(currentLine);
          if (m2.Success)
          {
            itv.DefaultValue = m2.Groups["default"].Value;
            itv.Disabled = m2.Groups["disabled"].Value.Contains("#");
            itv.OutputParameter = m2.Groups["output"].Value;
          }

          // Attempt to set values from template, just the basics.
          switch (itv.VariableName)
          {
            case "BASE_DIR":
              if (itv.DefaultValue != null)
              {
                itv.Disabled = true;
                string defaultValue = itv.DefaultValue.Replace('\"', ' ').Trim();
                if (defaultValue.Length > 0)
                {
                  BaseDir = defaultValue;
                }
              }
              break;

            case "DATA_DIR":
              if (itv.DefaultValue != null)
              {
                string defaultValue = itv.DefaultValue.Replace('\"', ' ').Trim();
                if (defaultValue.Length > 0)
                {
                  defaultValue = defaultValue.Replace("\\data\\", "\\");
                  defaultValue = defaultValue.Replace("\\Data\\", "\\");
                  DataDir = defaultValue.Trim();
                }
              }
              break;

            case "SECURE_FILE_PRIV":
              if (itv.DefaultValue != null)
              {
                itv.Disabled = true;
                string defaultValue = itv.DefaultValue.Replace('\"', ' ').Trim();
                if (defaultValue.Length > 0)
                {
                  SecureFilePriv = defaultValue;
                }
              }
              break;

            case "CLIENT_PORT":
            case "SERVER_PORT":
              EnableNetworking = !itv.Disabled;
              if (EnableNetworking && itv.DefaultValue != "")
              {
                Port = uint.Parse(itv.DefaultValue);
              }
              break;

            case "CLIENT_PIPE":
            case "CLIENT_SOCKET":
              break;

            case "SERVER_PIPE":
              EnableNamedPipe = !itv.Disabled;
              break;

            case "SERVER_SOCKET":
              PipeName = itv.DefaultValue;
              break;

            case "SERVER_SKIP":
              EnableNetworking = itv.Disabled;
              break;

            case "QUERY_CACHE_SIZE":
              EnableQueryCache = !itv.Disabled;
              break;

            case "QUERY_CACHE_TYPE":
              EnableQueryType = !itv.Disabled;
              break;

            case "INNODB_HOME":
              InnoDBHomeDir = !itv.Disabled ? itv.DefaultValue : string.Empty;
              break;

            case "STATE_CHANGE":
            case "SQL_MODE":
            case "INNODB_LOG_FILE_SIZE":
            case "SKIP_INNODB":
              break;

            case "SERVER_ID":
              if (itv.Disabled)
              {
                ServerId = null;
              }
              else
              {
                uint val;
                ServerId = uint.TryParse(itv.DefaultValue, out val) ? (uint?)val : null;
              }
              break;

            case "NAMED_PIPE_FULL_ACCESS_GROUP":
              NamedPipeFullAccessGroup = itv.DefaultValue;
              break;
          }
        }

        _output.Enqueue(itv);
      }
    }

    private string ReduceBytesToString(double value)
    {
      string convertedOutput = "";
      int unitType = 0;
      char[] unitNames = new char[4] { ' ', 'K', 'M', 'G' };
      while (value / 1024 >= 1.0)
      {
        double roundUp = 0.0;
        if (value % 1024 > 0)
        {
          ++roundUp;
        }

        value /= 1024;
        value = Math.Round(value + roundUp);
        unitType++;

        // Avoid going beyond the length of the unitNames array which contains abbreviations allowed by the Server in formulas
        if (unitType >= 3)
        {
          break;
        }
      }

      value = Math.Round(value);
      convertedOutput += value.ToString(CultureInfo.CurrentCulture);
      if (unitType >= 1)
      {
        convertedOutput += unitNames[unitType];
      }

      return convertedOutput;
    }

    private void SetDefaults()
    {
      _formulaEngine = new FormulaEngine();
      EnableStrictMode = true;
      EnableNetworking = true;
      EnableQueryCache = true;
      EnableQueryType = true;
      InnoDBHomeDir = string.Empty;
      DefaultStorageEngine = "INNODB";
      Port = BaseServerSettings.DEFAULT_PORT;
      SkipInnodb = false;
      IsValid = false;
      OutputExists = false;
      ServerType = ServerInstallationType.Developer;
      LongQueryTime = "10";
      NamedPipeFullAccessGroup = string.Empty;

      // Set reasonable defaults.
      MyisamUsage = 0.05;
      NumberConnections = 20.0;
      UseQueryCache = 0.0;
      InnoDBBPSUsage = 0.50;

      // There is no way to get a proper default by Server version at this level since the version is not accessible here
      DefaultAuthenticationPlugin = MySqlAuthenticationPluginType.CachingSha2Password;
    }

    /// <summary>
    /// Initializes the list of deprecated Server variables.
    /// </summary>
    private void InitializeDeprecatedServerVariables()
    {
      _deprecatedServerVariables = new List<DeprecatedServerVariable>
      {
        new DeprecatedServerVariable("innodb_additional_mem_pool_size", ServerSeriesType.S57),
        new DeprecatedServerVariable("loose_keyring_file_data", ServerSeriesType.S57),
        new DeprecatedServerVariable("table_cache", ServerSeriesType.All),
        new DeprecatedServerVariable("query_cache_size", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("query_cache_type", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("innodbclustertype", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("innodbclustername", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("innodbclusterinstances", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("innodbclusterusername", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("innodbclusteruri", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("innodbclusterport", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("innodbclustertypeselection", ServerSeriesType.S57 | ServerSeriesType.S80),
        new DeprecatedServerVariable("sync_master_info", ServerSeriesType.S80, new Version(8,0,26)),
        new DeprecatedServerVariable("default_authentication_plugin", ServerSeriesType.S80, new Version(8,0,27))
      };
    }
  }
}