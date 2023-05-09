/* Copyright (c) 2018, Oracle and/or its affiliates.

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
