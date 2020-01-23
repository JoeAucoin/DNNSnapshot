using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DotNetNuke;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;

namespace GIBS.DNNSnapshot.Components
{
    public class DNNSnapshotController : ISearchable, IPortable
    {

        #region public method


        public List<DNNSnapshotInfo> GetDNNSnapshotModules()
        {
            return CBO.FillCollection<DNNSnapshotInfo>(DataProvider.Instance().GetDNNSnapshotModules());
        }
        
        
        /// <summary>
        /// Gets all the DNNSnapshotInfo objects for items matching the this moduleId
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public List<DNNSnapshotInfo> GetDNNSnapshots(int moduleId)
        {
            return CBO.FillCollection<DNNSnapshotInfo>(DataProvider.Instance().GetDNNSnapshots(moduleId));
        }

        /// <summary>
        /// Get an info object from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public DNNSnapshotInfo GetDNNSnapshot(int moduleId, int itemId)
        {
            return (DNNSnapshotInfo)CBO.FillObject(DataProvider.Instance().GetDNNSnapshot(moduleId, itemId), typeof(DNNSnapshotInfo));
        }


        /// <summary>
        /// Adds a new DNNSnapshotInfo object into the database
        /// </summary>
        /// <param name="info"></param>
        public void AddDNNSnapshot(DNNSnapshotInfo info)
        {
            //check we have some content to store
            if (info.Content != string.Empty)
            {
                DataProvider.Instance().AddDNNSnapshot(info.ModuleId, info.Content, info.CreatedByUser);
            }
        }

        /// <summary>
        /// update a info object already stored in the database
        /// </summary>
        /// <param name="info"></param>
        public void UpdateDNNSnapshot(DNNSnapshotInfo info)
        {
            //check we have some content to update
            if (info.Content != string.Empty)
            {
                DataProvider.Instance().UpdateDNNSnapshot(info.ModuleId, info.ItemId, info.Content, info.CreatedByUser);
            }
        }


        /// <summary>
        /// Delete a given item from the database
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="itemId"></param>
        public void DeleteDNNSnapshot(int moduleId, int itemId)
        {
            DataProvider.Instance().DeleteDNNSnapshot(moduleId, itemId);
        }


        #endregion

        #region ISearchable Members

        /// <summary>
        /// Implements the search interface required to allow DNN to index/search the content of your
        /// module
        /// </summary>
        /// <param name="modInfo"></param>
        /// <returns></returns>
        public DotNetNuke.Services.Search.SearchItemInfoCollection GetSearchItems(ModuleInfo modInfo)
        {
            SearchItemInfoCollection searchItems = new SearchItemInfoCollection();

            List<DNNSnapshotInfo> infos = GetDNNSnapshots(modInfo.ModuleID);

            foreach (DNNSnapshotInfo info in infos)
            {
                SearchItemInfo searchInfo = new SearchItemInfo(modInfo.ModuleTitle, info.Content, info.CreatedByUser, info.CreatedDate,
                                                    modInfo.ModuleID, info.ItemId.ToString(), info.Content, "Item=" + info.ItemId.ToString());
                searchItems.Add(searchInfo);
            }

            return searchItems;
        }

        #endregion

        #region IPortable Members

        /// <summary>
        /// Exports a module to xml
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <returns></returns>
        public string ExportModule(int moduleID)
        {
            StringBuilder sb = new StringBuilder();

            List<DNNSnapshotInfo> infos = GetDNNSnapshots(moduleID);

            if (infos.Count > 0)
            {
                sb.Append("<DNNSnapshots>");
                foreach (DNNSnapshotInfo info in infos)
                {
                    sb.Append("<DNNSnapshot>");
                    sb.Append("<content>");
                    sb.Append(XmlUtils.XMLEncode(info.Content));
                    sb.Append("</content>");
                    sb.Append("</DNNSnapshot>");
                }
                sb.Append("</DNNSnapshots>");
            }

            return sb.ToString();
        }

        /// <summary>
        /// imports a module from an xml file
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Content"></param>
        /// <param name="Version"></param>
        /// <param name="UserID"></param>
        public void ImportModule(int ModuleID, string Content, string Version, int UserID)
        {
            XmlNode infos = DotNetNuke.Common.Globals.GetContent(Content, "DNNSnapshots");

            foreach (XmlNode info in infos.SelectNodes("DNNSnapshot"))
            {
                DNNSnapshotInfo DNNSnapshotInfo = new DNNSnapshotInfo();
                DNNSnapshotInfo.ModuleId = ModuleID;
                DNNSnapshotInfo.Content = info.SelectSingleNode("content").InnerText;
                DNNSnapshotInfo.CreatedByUser = UserID;

                AddDNNSnapshot(DNNSnapshotInfo);
            }
        }

        #endregion
    }
}
