/* Copyright (c) 2010, 2019, Oracle and/or its affiliates.

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
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Configurator.Controls;
using MySql.Configurator.Core.Interfaces;
using MySql.Configurator.Core.Wizard;
using MySql.Configurator.Dialogs;

namespace MySql.Configurator.Wizards
{
  /// <summary>
  /// The control used to perform Install and Upgrade (MSI) actions.
  /// </summary>
  public partial class Wizard : UserControl, IWizard
  {
    #region Fields

    private int _firstPageIndex;
    private int _lastPageIndex;

    #endregion Fields

    public Wizard()
    {
      InitializeComponent();
      FromMsi = false;
      Pages = new List<WizardPage>();
      _firstPageIndex = _lastPageIndex = -1;
    }

    /// <summary>
    /// Thread-safe delegate used to set the enabled status of the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="enabled">The value for the Enabled property.</param>
    private delegate void SetControlEnableStatusCallback(Control control, bool enabled);

    /// <summary>
    /// Thread-safe delegate used to set the visible status of the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="visible">The value for the Visible property.</param>
    private delegate void SetControlVisibleStatusCallback(Control control, bool visible);

    #region Events

    public event EventHandler WizardCanceled;
    public event EventHandler WizardClosed;

    #endregion Events

    #region Properties

    public Button BackButton => WizardBackButton;

    public virtual bool CanCancel => true;

    public Button CancelButton => WizardCancelButton;

    public bool CanGoBack
    {
      get
      {
        int index = CurrentIndex;
        while (index > 0)
        {
          index--;
          if (Pages[index].PageVisible)
          {
            return true;
          }
        }

        return false;
      }
    }

    public bool CanGoNext
    {
      get
      {
        int index = CurrentIndex;
        while (index < Pages.Count - 1)
        {
          index++;
          if (Pages[index].PageVisible)
          {
            return true;
          }
        }

        return false;
      }
    }

    public int CurrentIndex { get; private set; }

    public WizardPage CurrentPage { get; private set; }

    public Button ExecuteButton => WizardExecuteButton;

    public Button FinishButton => WizardFinishButton;

    public bool FromMsi { get; set; }

    public string Log { get; set; }

    public Button NextButton => WizardNextButton;

    public int PageCount => Pages.Count(p => p.PageVisible);

    public List<WizardPage> Pages { get; }

    #endregion Properties

    public void AddPage(WizardPage page)
    {
      if (Pages.Contains(page))
      {
        return;
      }

      page.Visible = false;
      Pages.Add(page);
      Controls.Add(page);
      page.Location = new Point(220, 0);
      page.Wizard = this;
      AddSideBarTabForPage(page);
    }

    public void ClearPages()
    {
      WizardSideBar.Tabs.Clear();
      Pages.ForEach(p => p.Dispose());
      Pages.Clear();
    }

    public void EnablePage(int index, bool state)
    {
      if (index < 0 || index >= Pages.Count)
      {
        throw new IndexOutOfRangeException();
      }

      Pages[index].PageVisible = state;
      FindFirstAndLastPage();
      Refresh();
      WizardSideBar.Refresh();
    }

    public int GetIndex(WizardPage page)
    {
      return Pages.TakeWhile(p => p != page).Count();
    }

    public void RefreshSideBar()
    {
      WizardSideBar.Refresh();
    }

    public virtual void ShowWizard(MainForm parentMainForm)
    {
      ShowWizard(parentMainForm, true);
    }

    public void ShowWizard(MainForm parentMainForm, bool initialShow)
    {
      if (parentMainForm == null)
      {
        throw new ArgumentNullException(nameof(parentMainForm));
      }

      if (initialShow)
      {
        foreach (var wizardPage in Pages)
        {
          wizardPage.WizardShowing();
        }

        FindFirstAndLastPage();
        CurrentIndex = _firstPageIndex;
        CurrentPage = Pages[CurrentIndex];
        ActivatePage();
      }

      parentMainForm.ShowContainer(this);
    }

    public void UpdateButtons()
    {
      SetControlVisibleStatus(WizardBackButton, CurrentIndex != _firstPageIndex);
      SetControlEnabledStatus(WizardBackButton, CurrentPage.BackOk);
      SetControlVisibleStatus(WizardNextButton, CurrentIndex != _lastPageIndex);
      SetControlEnabledStatus(WizardNextButton, CurrentPage.NextOk);
      SetControlVisibleStatus(WizardFinishButton, (CurrentIndex == _lastPageIndex) && !WizardExecuteButton.Visible);
      SetControlEnabledStatus(WizardFinishButton, CurrentPage.FinishOk);
      SetControlEnabledStatus(WizardCancelButton, CurrentPage.CancelOk);
      SetControlEnabledStatus(WizardExecuteButton, CurrentPage.ExecuteOk);
    }

    protected override void OnLoad(EventArgs e)
    {
      AutoScaleMode = AutoScaleMode.None;
      Core.Classes.Utilities.NormalizeFont(this);
      base.OnLoad(e);
    }

    protected virtual void UpdateWizardPage(int newPage)
    {
      MoveToNewPage(newPage);
      ActivatePage();
    }

    private void ActivatePage()
    {
      foreach (var otherPage in Pages.Where(p => !p.Equals(CurrentPage)))
      {
        otherPage.Visible = false;
      }

      CurrentPage.Visible = true;
      WizardSideBar.SelectTab(CurrentPage);
      ExecuteButton.Visible = false;
      CurrentPage.Activate();
      UpdateButtons();
    }

    private void AddSideBarTabForPage(WizardPage p)
    {
      if (WizardSideBar.Tabs.Exists(tab => tab.Page.Equals(p)))
      {
        return;
      }

      var t = new SideBarTab(p.TabTitle, false);
      WizardSideBar.Tabs.Add(t);
      t.Page = p;
    }

    private void FindFirstAndLastPage()
    {
      _firstPageIndex = -1;
      int index = -1;
      while (++index < Pages.Count)
      {
        if (!Pages[index].PageVisible)
        {
          continue;
        }

        _firstPageIndex = index;
        break;
      }

      _lastPageIndex = -1;
      index = Pages.Count;
      while (--index >= 0)
      {
        if (!Pages[index].PageVisible)
        {
          continue;
        }

        _lastPageIndex = index;
        break;
      }

      if (_firstPageIndex == -1)
      {
        throw new Exception("No start page found");
      }
    }

    private void MoveToNewPage(int newPage)
    {
      while (true)
      {
        CurrentIndex += newPage;
        CurrentPage = Pages[CurrentIndex];
        if (CurrentPage.PageVisible)
        {
          return;
        }
      }
    }

    /// <summary>
    /// Thread-safe delegate used to set the enabled status of the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="enabled">The value for the Enabled property.</param>
    private void SetControlEnabledStatus(Control control, bool enabled)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (control.InvokeRequired)
      {
        SetControlEnableStatusCallback callbackMethod = SetControlEnabledStatus;
        Invoke(callbackMethod, control, enabled);
      }
      else
      {
        control.Enabled = enabled;
      }
    }

    /// <summary>
    /// Thread-safe delegate used to set the visible status of the specified control.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="visible">The value for the Visible property.</param>
    private void SetControlVisibleStatus(Control control, bool visible)
    {
      // InvokeRequired required compares the thread ID of the
      // calling thread to the thread ID of the creating thread.
      // If these threads are different, it returns true.
      if (control.InvokeRequired)
      {
        SetControlVisibleStatusCallback callbackMethod = SetControlVisibleStatus;
        Invoke(callbackMethod, control, visible);
      }
      else
      {
        control.Visible = visible;
      }
    }

    /// <summary>
    /// Back button press event handler.
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The parameter</param>
    private void WizardBackButton_Click(object sender, EventArgs e)
    {
      if (!CurrentPage.Back())
      {
        return;
      }

      UpdateWizardPage(-1);
    }

    /// <summary>
    /// Cancel button press event handler.
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The parameter</param>
    private void WizardCancelButton_Click(object sender, EventArgs e)
    {
      if (!CanCancel
          || !CurrentPage.Cancel())
      {
        return;
      }

      WizardCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void WizardExecuteButton_Click(object sender, EventArgs e)
    {
      CurrentPage.Execute();
    }

    private void WizardFinishButton_Click(object sender, EventArgs e)
    {
      if (!CurrentPage.Finish())
      {
        return;
      }

      WizardClosed?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Help button press event handler.
    /// </summary>
    /// <param name="sender">The sender</param>
    /// <param name="e">The parameter</param>
    private void WizardHelpButton_Click(object sender, EventArgs e)
    {
    }

    private void WizardNextButton_Click(object sender, EventArgs e)
    {
      if (!CurrentPage.Next())
      {
        return;
      }

      UpdateWizardPage(1);
    }
  }
}
