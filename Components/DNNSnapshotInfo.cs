using System;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace GIBS.DNNSnapshot.Components
{
    public class DNNSnapshotInfo
    {
        //private vars exposed thro the
        //properties
        private int tabID;
        private int moduleId;
        private int itemId;
        private int portalID;
        private string content;
        private int createdByUser;
        private DateTime createdDate;
        private string createdByUserName = null;


        /// <summary>
        /// empty cstor
        /// </summary>
        public DNNSnapshotInfo()
        {
        }


        #region properties

        public int TabID
        {
            get { return tabID; }
            set { tabID = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public int ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        public int PortalID
        {
            get { return portalID; }
            set { portalID = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public int CreatedByUser
        {
            get { return createdByUser; }
            set { createdByUser = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string CreatedByUserName
        {
            get
            {
                if (createdByUserName == null)
                {
                    int portalId = PortalController.GetCurrentPortalSettings().PortalId;
                    UserInfo user = UserController.GetUser(portalId, createdByUser, false);
                    createdByUserName = user.DisplayName;
                }

                return createdByUserName;
            }
        }

        #endregion
    }
}
