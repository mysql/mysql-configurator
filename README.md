# MySQL Configurator

MySQL Configurator is a server tool that provides a GUI to configure the MySQL Server in a Windows environment. It configures many aspects of their server installation through a series of wizard pages and can also migrate an existing data directory into a recent MySQL Server installation.

## Licensing

Please refer to the [LICENSE](LICENSE) file, available in this repository, and [Legal Notices in documentation](https://dev.mysql.com/doc/refman/8.1/en/preface.html) for further details.

## Download and build

MySQL Configurator is distributed as part of the MySQL Server and can be found in the bin folder located in the folder where MySQL Server was extracted to or installed.

No installation is required as the tool is distributed as a stand-alone exe which can be executed at any given moment.

## Building from sources

### Prerequisites

* Visual Studio 2022
* .NET Framework 4.6.2
* MS Build Community Tasks 1.5.0+

### Building

### Using Visual Studio

1. Open the **mysql_configurator.sln** solution file in Visual Studio 2022 with admin privileges.
2. Select the build configuration type (Debug/Release).
3. Build the solution.

### Using MSBuild

1. Open a Command Prompt app (Run as administrator).
2. Navigate to the folder containing the **mysql_configurator.sln** solution file.
3. Run the following command (replace [CONFIGURATION_TYPE] with the corresponding value (Debug/Release): 

```sh
"C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\msbuild.exe" mysql_configurator.sln /p:Configuration=[CONFIGURATION_TYPE] /p:Platform="Any CPU"
```

Finally, you will find the **mysql_configurator.exe** file in the corresponding **Configurator/bin/[CONFIGURATION_TYPE]** folder.