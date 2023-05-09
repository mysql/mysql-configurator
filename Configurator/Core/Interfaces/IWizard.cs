/* Copyright (c) 2014, 2019, Oracle and/or its affiliates.

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

using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Configurator.Core.Wizard;

namespace MySql.Configurator.Core.Interfaces
{
  public interface IWizard
  {
    #region Properties

    Button BackButton { get; }

    bool CanCancel { get; }

    Button CancelButton { get; }

    bool CanGoBack { get; }

    bool CanGoNext { get; }

    Button ExecuteButton { get; }

    Button FinishButton { get; }

    string Log { get; set; }

    Button NextButton { get; }

    int PageCount { get; }

    List<WizardPage> Pages { get; }

    #endregion Properties

    void EnablePage(int index, bool state);

    int GetIndex(WizardPage p);

    void UpdateButtons();
  }
}
