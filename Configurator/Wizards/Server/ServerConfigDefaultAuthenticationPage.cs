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

using System.Linq;
using System.Text;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Wizard;

namespace MySql.Configurator.Wizards.Server
{
  /// <summary>
  /// Contains configuration options to select the default authentication method used when connecting to a MySQL Server.
  /// </summary>
  public partial class ServerConfigDefaultAuthenticationPage : ConfigWizardPage
  {
    #region Fields

    /// <summary>
    /// The <seealso cref="ServerConfigurationController"/> used to perform actions.
    /// </summary>
    private ServerConfigurationController _controller;

    /// <summary>
    /// The <see cref="MySqlServerSettings"/> holding the Server configuration settings.
    /// </summary>
    private MySqlServerSettings _settings;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of the <see cref="ServerConfigDefaultAuthenticationPage"/> class.
    /// </summary>
    /// <param name="controller">The <see cref="ServerConfigurationController"/> used to perform configuration actions.</param>
    public ServerConfigDefaultAuthenticationPage(ServerConfigurationController controller)
    {
      InitializeComponent();
      _controller = controller;
      _settings = controller.Settings;
    }

    #region Properties

    /// <summary>
    /// Gets a value indicating if Next button is available.
    /// </summary>
    public override bool NextOk => UseSha256AuthenticationRadioButton.Checked
                                   || UseNativeAuthenticationRadioButton.Checked;

    #endregion Properties

    /// <summary>
    /// Activates this instance.
    /// </summary>
    public override void Activate()
    {
      bool useNativeAuthentication = _settings.DefaultAuthenticationPlugin == MySqlAuthenticationPluginType.MysqlNativePassword;
      UseNativeAuthenticationRadioButton.Checked = useNativeAuthentication;
      UseSha256AuthenticationRadioButton.Checked = !useNativeAuthentication;
      base.Activate();
    }

    /// <summary>
    /// Performs actions to be done when moving to the next configuration page (when the Next button is clicked).
    /// </summary>
    /// <returns><c>true</c> if the wizard is allowed to the next page, <c>false</c> otherwise.</returns>
    public override bool Next()
    {
      _settings.DefaultAuthenticationPlugin = UseSha256AuthenticationRadioButton.Checked
        ? MySqlAuthenticationPluginType.CachingSha2Password
        : MySqlAuthenticationPluginType.MysqlNativePassword;
      if (!_controller.Package.Version.ServerSupportsDefaultAuthenticationPluginVariable()
          && _controller.DefaultAuthenticationPluginChanged
          && !string.IsNullOrEmpty(_settings.AuthenticationPolicy))
      {
        var builder = new StringBuilder();
        builder.Append(_settings.DefaultAuthenticationPlugin == MySqlAuthenticationPluginType.CachingSha2Password
          ? "*"
          : _settings.DefaultAuthenticationPlugin.GetDescription());
        var factors = _settings.AuthenticationPolicy.Split(',');
        for(int index=1; index<factors.Length; index++)
        {
          builder.Append($",{factors[index]}");
        }

        _settings.AuthenticationPolicy = builder.ToString();
      }

      if (_controller.ConfigurationType == Core.Enums.ConfigurationType.Upgrade)
      {
        var userAccountsPage = _controller.Pages.FirstOrDefault(page => page is ServerConfigUserAccountsPage);
        if (userAccountsPage != null)
        {
          userAccountsPage.PageVisible = _controller.IsThereServerDataFiles
                                           ? _controller.DefaultAuthenticationPluginChanged
                                           : true;
          Parent.Refresh();
        }
      }

      return base.Next();
    }
  }
}
