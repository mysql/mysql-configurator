/* Copyright (c) 2020, 2023, Oracle and/or its affiliates.

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

using System.Xml.Serialization;

namespace MySql.Configurator.Core.Common
{
  /// <summary>
  /// Represents general data that is used by the product catalog.
  /// </summary>
  public class GeneralSettings
  {
    #region Properties

    [XmlElement("archiveBaseUrl")]
    public string ArchiveBaseUrl { get; set; }

    [XmlElement("cdnBaseUrl")]
    public string CdnBaseUrl { get; set; }

    [XmlElement("changesBaseUrlTemplate")]
    public string ChangesBaseUrlTemplate { get; set; }

    [XmlElement("homeBaseUrl")]
    public string HomeBaseUrl { get; set; }

    [XmlElement("mosUrl")]
    public string MosUrl { get; set; }

    #endregion
  }
}
