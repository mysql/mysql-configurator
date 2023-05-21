﻿// Copyright (c) 2023, Oracle and/or its affiliates.
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License as
// published by the Free Software Foundation; version 2 of the
// License.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA
// 02110-1301  USA

using MySql.Configurator.Core.Classes.Forms;
using MySql.Configurator.Core.Classes.Logging;
using MySql.Configurator.Properties;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace MySql.Configurator.Core.Classes
{
  /// <summary>
  /// Defines functionality to store information in a XML file.
  /// </summary>
  public abstract class XmlRepository
  {
    #region Properties

    /// <summary>
    /// Gets or sets the XPath expression used to find a single repository element.
    /// </summary>
    public string ChildrenXPathExpression { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the connections file is automatically created if it is not found during loading connections.
    /// </summary>
    public bool CreateFileUponLoadIfNotFound { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the connections file is automatically created if it is not found during saving connections.
    /// </summary>
    public bool CreateFileUponSaveIfNotFound { get; set; }

    /// <summary>
    /// Gets a value indicating whether the repository file is maintained by MySQL Workbench.
    /// </summary>
    public bool IsWorkbenchFile { get; protected set; }

    /// <summary>
    /// Gets or sets the XPath expression used to find the parent node for repository elements.
    /// </summary>
    public string ParentXPathExpression { get; set; }

    /// <summary>
    /// Gets or sets the file path of the repository file to be used.
    /// </summary>
    public string RepositoryFilePath { get; set; }

    /// <summary>
    /// Gets a value indicating whether the repository file exists.
    /// </summary>
    public bool RepositoryFileExists => !string.IsNullOrEmpty(RepositoryFilePath)
                                        && File.Exists(RepositoryFilePath);

    #endregion Properties

    /// <summary>
    /// Adds a property value to the corresponding serialized representation of this object.
    /// </summary>
    /// <param name="document">XML document.</param>
    /// <param name="element">XML element representing this serialized object.</param>
    /// <param name="key">Name of the XML node representing the property.</param>
    /// <param name="type">Data type of the serialized value.</param>
    /// <param name="value">Property value.</param>
    public static void AppendPropertyValueToElement(XmlDocument document, XmlElement element, string key, string type, object value)
    {
      InsertPropertyValueToElementAfter(document, element, null, key, type, value);
    }

    /// <summary>
    /// Adds a property value to the corresponding serialized representation of this object.
    /// </summary>
    /// <param name="document">XML document.</param>
    /// <param name="element">XML element representing this serialized object.</param>
    /// <param name="afterElement">The <see cref="XmlElement"/> after which the new element is going to be inserted. If <c>null</c> then the new element is appended after the last one.</param>
    /// <param name="key">Name of the XML node representing the property.</param>
    /// <param name="type">Data type of the serialized value.</param>
    /// <param name="value">Property value.</param>
    public static void InsertPropertyValueToElementAfter(XmlDocument document, XmlElement element, XmlElement afterElement, string key, string type, object value)
    {
      var val = document.CreateElement("value");
      val.SetAttribute("type", type);
      val.SetAttribute("key", key);
      if (type.ToLower() == "string" && value == null)
      {
        value = string.Empty;
      }

      val.InnerText = value.ToString();
      val.IsEmpty = val.InnerText.Trim().Length == 0;
      if (afterElement == null)
      {
        element.AppendChild(val);
      }
      else
      {
        element.InsertAfter(val, afterElement);
      }
    }

    /// <summary>
    /// Loads the information from the repository file specified in <see cref="RepositoryFilePath"/>.
    /// </summary>
    /// <param name="retryOrRecreate">Flag indicating whether the load operation should retry if it fails or recreate the file if fails on the retries, otherwise an error is shown.</param>
    /// <param name="retryTimes">The number of times the load operation is retried.</param>
    /// <returns><c>true</c> if the load could be done successfully, <c>false</c> otherwise.</returns>
    public virtual bool Load(bool retryOrRecreate, byte retryTimes = 3)
    {
      bool retry;
      bool success = false;
      int retryRound = 1;

      do
      {
        retry = retryOrRecreate
                && retryRound++ <= retryTimes;
        try
        {
          var xmlDocument = LoadXmlFile(false, !retryOrRecreate, !retryOrRecreate);
          var rootElement = xmlDocument?.DocumentElement;
          if (rootElement != null)
          {
            LoadChildElements(rootElement);
            success = true;
          }
        }
        catch (Exception ex)
        {
          success = false;
          if (retry)
          {
            var infoProperties = InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning,
              Resources.RepositoryFileRetryTitle, Resources.RepositoryFileLoadingErrorDetail,
              Resources.RepositoryFileRetrySubDetail, string.Format(Resources.RepositoryFileRetryMoreInfo, retryTimes, retryTimes > 1 ? "s" : string.Empty, ex.Message));
            infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button2;
            infoProperties.CommandAreaProperties.DefaultButtonTimeout = 30;
            var infoResult = InfoDialog.ShowDialog(infoProperties);
            if (infoResult.DialogResult == DialogResult.No)
            {
              retry = false;
            }
          }
          else
          {
            var infoProperties = InfoDialogProperties.GetErrorDialogProperties(Resources.GenericErrorTitle,
              Resources.RepositoryFileLoadingErrorDetail, null, ex.Message);
            infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button1;
            infoProperties.CommandAreaProperties.DefaultButtonTimeout = 30;
            InfoDialog.ShowDialog(infoProperties);
          }

          Logger.LogException(ex);
        }
      }
      while (retry && !success);

      if (retryOrRecreate
          && !success)
      {
        do
        {
          var infoProperties = IsWorkbenchFile
            ? InfoDialogProperties.GetWarningDialogProperties(Resources.WarningsText, Resources.RepositoryFileRecreateDetail, Resources.RepositoryFileMaintainedByWorkbench, Resources.RepositoryFileLoadingErrorMoreInfo)
            : InfoDialogProperties.GetYesNoDialogProperties(InfoDialog.InfoType.Warning, Resources.RepositoryFileRecreateTitle, Resources.RepositoryFileRecreateDetail, Resources.RepositoryFileRecreateSubDetail);
          infoProperties.FitTextStrategy = InfoDialog.FitTextsAction.IncreaseDialogWidth;
          infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button1;
          infoProperties.CommandAreaProperties.DefaultButtonTimeout = 30;
          var infoResult = InfoDialog.ShowDialog(infoProperties);
          if (infoResult.DialogResult == DialogResult.Yes)
          {
            retry = !RecreateXmlFile();
          }
          else
          {
            retry = false;
          }

        } while (retry);
      }

      return success;
    }

    /// <summary>
    /// Saves the information to the repository file specified in <see cref="RepositoryFilePath"/>.
    /// </summary>
    /// <param name="throwException">Flag indicating whether the exception is thrown up and no error message displayed at this point, or caught and an error message displayed to users.</param>
    /// <returns><c>true</c> if the saving operation was successful, <c>false</c> otherwise.</returns>
    public virtual bool Save(bool throwException)
    {
      var success = true;
      try
      {
        var xmlDocument = LoadXmlFile(true);
        var rootNode = GetRepositoryParent(xmlDocument);
        if (rootNode == null)
        {
          return false;
        }

        SaveChildElements(ref rootNode);

        // Now save it out
        var settings = new XmlWriterSettings
        {
          Indent = true
        };
        using (var writer = XmlWriter.Create(RepositoryFilePath, settings))
        {
          xmlDocument.Save(writer);
        }
      }
      catch (Exception ex)
      {
        if (throwException)
        {
          throw;
        }

        success = false;
        Logger.LogException(ex, true, Resources.RepositoryFileSavingErrorDetail, Resources.GenericErrorTitle);
      }

      return success;
    }

    /// <summary>
    /// Creates custom elements in the XML file being created.
    /// </summary>
    /// <param name="doc">The <see cref="XmlDocument"/> that contains repository information.</param>
    protected virtual void CreateCustomXmlElements(ref XmlDocument doc)
    {
      // Meant to be overriden and implemented in inherited classes.
    }

    /// <summary>
    /// Creates the repository XML file.
    /// </summary>
    /// <param name="encoding">The value of the encoding attribute for the XML declaration. If <c>null</c> a default UTF-8 encoding is used.</param>
    /// <param name="standalone">The value must be either <c>"yes"</c> or <c>"no"</c>. If this is <c>null</c> or <c>strig.Empty</c>, the XML declaration will not have a standalone attribute.</param>
    /// <returns><c>true</c> if the file was created and information saved successfully, <c>false</c> otherwise.</returns>
    protected virtual bool CreateXmlFile(string encoding = null, string standalone = null)
    {
      if (string.IsNullOrEmpty(RepositoryFilePath))
      {
        return false;
      }

      var directoryName = Path.GetDirectoryName(RepositoryFilePath);
      if (string.IsNullOrEmpty(directoryName))
      {
        return false;
      }

      var success = true;
      try
      {
        if (!Directory.Exists(directoryName))
        {
          Directory.CreateDirectory(directoryName);
        }

        // Create repository file with mandatory XML nodes
        var xmlDocument = new XmlDocument();
        var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", encoding, standalone);
        xmlDocument.AppendChild(xmlDeclaration);

        // Create other custom elements
        CreateCustomXmlElements(ref xmlDocument);

        // Save XML file
        xmlDocument.Save(RepositoryFilePath);
      }
      catch (Exception ex)
      {
        Logger.LogException(ex, true, Resources.RepositoryFileSavingErrorDetail, Resources.GenericErrorTitle);
        success = false;
      }

      return success;
    }

    /// <summary>
    /// Removes a specific repository node with the given id from the file in <see cref="RepositoryFilePath"/>.
    /// </summary>
    /// <param name="id">Id of the node to remove.</param>
    protected virtual void DeleteRepositoryElement(string id)
    {
      var doc = LoadXmlFile(false);
      if (doc == null)
      {
        return;
      }

      var itemNode = GetRepositoryElement(doc, id);
      if (itemNode?.ParentNode == null)
      {
        return;
      }

      itemNode.ParentNode.RemoveChild(itemNode);
      doc.Save(RepositoryFilePath);
    }

    /// <summary>
    /// Gets a specific repository node with the given ID from the file in <see cref="RepositoryFilePath"/>.
    /// </summary>
    /// <param name="parentNode">A <see cref="XmlNode"/> of an already loaded repository file.</param>
    /// <param name="id">ID of the node to find.</param>
    /// <returns>The <see cref="XmlNode"/> of the element with the given ID.</returns>
    protected virtual XmlNode GetRepositoryElement(XmlNode parentNode, string id)
    {
      if (parentNode == null
          || !parentNode.HasChildNodes)
      {
        return null;
      }

      return parentNode.SelectSingleNode(string.Format(ChildrenXPathExpression, id));
    }

    /// <summary>
    /// Gets a specific repository node with the given ID from the file in <see cref="RepositoryFilePath"/>.
    /// </summary>
    /// <param name="doc">An already loaded <see cref="XmlDocument"/> for the repository file in <see cref="RepositoryFilePath"/>.</param>
    /// <param name="id">ID of the node to find.</param>
    /// <returns>The <see cref="XmlNode"/> of the element with the given ID.</returns>
    protected virtual XmlNode GetRepositoryElement(XmlDocument doc, string id)
    {
      return GetRepositoryElement(GetRepositoryParent(doc), id);
    }

    /// <summary>
    /// Gets a specific repository node with the given ID from the file in <see cref="RepositoryFilePath"/>.
    /// </summary>
    /// <param name="id">ID of the node to find.</param>
    /// <returns>The <see cref="XmlNode"/> of the element with the given ID.</returns>
    protected virtual XmlNode GetRepositoryElement(string id)
    {
      if (!RepositoryFileExists)
      {
        return null;
      }

      var doc = LoadXmlFile(false);
      return doc == null ? null : GetRepositoryElement(doc, id);
    }

    /// <summary>
    /// Gets the parent node for repository elements.
    /// </summary>
    /// <param name="documentElement">The root <see cref="XmlElement"/> of an already loaded repository file.</param>
    /// <returns>The <see cref="XmlNode"/> of the parent element.</returns>
    protected virtual XmlNode GetRepositoryParent(XmlElement documentElement)
    {
      if (documentElement == null
          || !documentElement.HasChildNodes)
      {
        return null;
      }

      return documentElement.SelectSingleNode(ParentXPathExpression);
    }

    /// <summary>
    /// Gets the parent node for repository elements.
    /// </summary>
    /// <param name="doc">An already loaded <see cref="XmlDocument"/> for the repository file in <see cref="RepositoryFilePath"/>.</param>
    /// <returns>The <see cref="XmlNode"/> of the parent element.</returns>
    protected virtual XmlNode GetRepositoryParent(XmlDocument doc)
    {
      return doc == null
        ? null
        : GetRepositoryParent(doc.DocumentElement);
    }

    /// <summary>
    /// Gets the parent node for repository elements.
    /// </summary>
    /// <returns>The <see cref="XmlNode"/> of the parent element.</returns>
    protected virtual XmlNode GetRepositoryParent()
    {
      if (!RepositoryFileExists)
      {
        return null;
      }

      var doc = LoadXmlFile(false);
      return doc == null
        ? null
        : GetRepositoryParent(doc);
    }

    /// <summary>
    /// Loads the child elements contained in the given root element.
    /// </summary>
    /// <param name="root">The root <see cref="XmlElement"/> from which to process child elements.</param>
    protected virtual void LoadChildElements(XmlElement root)
    {
      // Meant to be overriden and implemented in inherited classes.
    }

    /// <summary>
    /// Loads the XML file specified in <seealso cref="RepositoryFilePath"/>.
    /// </summary>
    /// <param name="saving">Flag indicating whether the loading of the XML file is done from a saving operation.</param>
    /// <param name="showInfoDialogOnError">Flag indicating whether an information dialog is shown to users if the file can't be loaded.</param>
    /// <param name="attemptToRecreateIfNotWorkbenchOwned">Flag indicating if the file must be re-created if MySQL Workbench does not maintain the file.</param>
    /// <returns>A <see cref="XmlDocument"/> object with the contents of the XML file.</returns>
    protected virtual XmlDocument LoadXmlFile(bool saving, bool showInfoDialogOnError = true, bool attemptToRecreateIfNotWorkbenchOwned = true)
    {
      var createFlag = saving
        ? CreateFileUponSaveIfNotFound
        : CreateFileUponLoadIfNotFound;
      if (!RepositoryFileExists
          && (createFlag && !CreateXmlFile()
              || !createFlag))
      {
        return null;
      }

      // Attempt to load the file, since we ensured a file to load exists (it was created if it did not exist)
      XmlDocument xmlDocument;
      try
      {
        xmlDocument = new XmlDocument();
        xmlDocument.Load(RepositoryFilePath);
      }
      catch (Exception ex)
      {
        // The file might be empty or malformed.
        xmlDocument = null;
        Logger.LogError(string.Format(Resources.RepositoryFileMaybeEmptyOrMalformed, RepositoryFilePath));
        Logger.LogException(ex);
        if (!IsWorkbenchFile
            && attemptToRecreateIfNotWorkbenchOwned)
        {
          // Attempt to re-create the file ourselves
          Logger.LogInformation(string.Format(Resources.RepositoryFileAttemptingToRecreate, RepositoryFilePath));
           if (RecreateXmlFile())
           {
             Logger.LogInformation(string.Format(Resources.RepositoryFileRecreatedNowReloading, RepositoryFilePath));
             return LoadXmlFile(saving, showInfoDialogOnError, false);
           }
        }

        if (showInfoDialogOnError)
        {
          // Show a message to users explaining how to let MySQL Workbench repair or re-create the file
          Logger.LogInformation(string.Format(Resources.RepositoryFileMaintainedByWorkbenchSuggestion, RepositoryFilePath));
          var infoProperties = InfoDialogProperties.GetErrorDialogProperties(Resources.GenericErrorTitle,
            Resources.RepositoryFileLoadingErrorDetail, ex.Message, IsWorkbenchFile ? Resources.RepositoryFileLoadingErrorMoreInfo : null);
          infoProperties.CommandAreaProperties.DefaultButton = InfoDialog.DefaultButtonType.Button1;
          infoProperties.CommandAreaProperties.DefaultButtonTimeout = 60;
          InfoDialog.ShowDialog(infoProperties);
        }
      }

      return xmlDocument;
    }

    /// <summary>
    /// Attempts to delete and create again the repository XML file.
    /// </summary>
    /// <param name="encoding">The value of the encoding attribute for the XML declaration. If <c>null</c> a default UTF-8 encoding is used.</param>
    /// <param name="standalone">The value must be either <c>"yes"</c> or <c>"no"</c>. If this is <c>null</c> or <c>strig.Empty</c>, the XML declaration will not have a standalone attribute.</param>
    /// <returns><c>true</c> if the file was deleted and created again successfully, <c>false</c> otherwise.</returns>
    protected virtual bool RecreateXmlFile(string encoding = null, string standalone = null)
    {
      if (string.IsNullOrEmpty(RepositoryFilePath))
      {
        return false;
      }

      if (RepositoryFileExists
          && !Utilities.DeleteFile(RepositoryFilePath, 3, 100, 0, out var error))
      {
        Logger.LogException(new Exception(error), true, Resources.RepositoryFileRecreatingErrorDetail, Resources.GenericErrorTitle);
        return false;
      }

      return CreateXmlFile(encoding, standalone);
    }

    /// <summary>
    /// Processes the child elements contained in the given root element for saving.
    /// </summary>
    /// <param name="root">The root <see cref="XmlNode"/> from which to process child elements.</param>
    protected virtual void SaveChildElements(ref XmlNode root)
    {
      // Meant to be overriden and implemented in inherited classes.
      // Append nodes for NEW data
      // Replace nodes for UPDATED data
    }
  }
}