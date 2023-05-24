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

using MySql.Configurator.Core.Enums;

namespace MySql.Configurator.Core.Wizard
{
  public partial class ConfigWizardPage : WizardPage
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigWizardPage"/> class.
    /// </summary>
    public ConfigWizardPage()
    {
      InitializeComponent();
      ValidConfigureTypes = ConfigurationType.All;
      Name = nameof(ConfigWizardPage);
    }

    #region Properties

    public ConfigurationType ValidConfigureTypes { get; set; }

    #endregion Properties

    public bool ValidForType(ConfigurationType type)
    {
      return ValidConfigureTypes.HasFlag(type);
    }
  }
}