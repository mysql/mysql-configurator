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
using System.Windows.Forms;
using MySql.Configurator.Core.Classes;
using MySql.Configurator.Core.Forms;

namespace MySql.Configurator.Dialogs
{
  /// <summary>
  /// Shows a warning about upgrading the MySQL Server from a milestone release.
  /// </summary>
  public partial class UpgradeFromServerPreGaWarningDialog : AutoStyleableBaseDialog
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="UpgradeFromServerPreGaWarningDialog"/> class.
    /// </summary>
    /// <param name="newVersion">The version the MySQL Server is being upgraded to.</param>
    /// <param name="newReleaseNotesUrl">The URL of the release notes.</param>
    public UpgradeFromServerPreGaWarningDialog(Version newVersion, string newReleaseNotesUrl)
    {
      InitializeComponent();
      ReleaseNotesLinkLabel.Text = $"MySQL Server {newVersion.ToString(3)} Release Notes";
      ReleaseNotesUrl = newReleaseNotesUrl;
    }

    /// <summary>
    /// Gets the URL of the release notes.
    /// </summary>
    public string ReleaseNotesUrl { get; }

    /// <summary>
    /// Shows the <see cref="UpgradeFromServerPreGaWarningDialog"/>.
    /// </summary>
    /// <param name="newVersion">The version the MySQL Server is being upgraded to.</param>
    /// <param name="newReleaseNotesUrl">The URL of the release notes.</param>
    /// <returns></returns>
    public static DialogResult ShowDialog(Version newVersion, string newReleaseNotesUrl)
    {
      DialogResult result;
      using (var dialog = new UpgradeFromServerPreGaWarningDialog(newVersion, newReleaseNotesUrl))
      {
        result = dialog.ShowDialog();
      }

      return result;
    }

    /// <summary>
    /// Event delegate method fired when the <see cref="ReleaseNotesLinkLabel"/> is clicked.
    /// </summary>
    /// <param name="sender">Sender object.</param>
    /// <param name="e">Event arguments.</param>
    private void ReleaseNotesLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      Utilities.OpenBrowser(ReleaseNotesUrl);
    }
  }
}
