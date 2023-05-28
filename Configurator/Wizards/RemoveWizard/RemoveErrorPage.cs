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
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Controllers;
using MySql.Configurator.Core.Controls;
using MySql.Configurator.Core.Enums;
using MySql.Configurator.Core.Interfaces;
using MySql.Configurator.Core.Package;
using MySql.Configurator.Core.Product;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Properties;
using MySql.Configurator.Wizards.Common;

namespace MySql.Configurator.Wizards.RemoveWizard
{
  public partial class RemoveErrorPage : WizardPage
  {
    /// <summary>
    /// The main constructor.
    /// </summary>
    public RemoveErrorPage()
    {
      InitializeComponent();
    }
  }
}
