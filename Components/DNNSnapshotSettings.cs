using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common;

namespace GIBS.DNNSnapshot.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class DNNSnapshotSettings
    {
        ModuleController controller;
        int tabModuleId;

        public DNNSnapshotSettings(int tabModuleId)
        {
            controller = new ModuleController();
            this.tabModuleId = tabModuleId;
        }

        protected T ReadSetting<T>(string settingName, T defaultValue)
        {
            Hashtable settings = controller.GetTabModuleSettings(this.tabModuleId);

            T ret = default(T);

            if (settings.ContainsKey(settingName))
            {
                System.ComponentModel.TypeConverter tc = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                try
                {
                    ret = (T)tc.ConvertFrom(settings[settingName]);
                }
                catch
                {
                    ret = defaultValue;
                }
            }
            else
                ret = defaultValue;

            return ret;
        }

        protected void WriteSetting(string settingName, string value)
        {
            controller.UpdateTabModuleSetting(this.tabModuleId, settingName, value);
        }

        #region public properties

        /// <summary>
        /// get/set template used to render the module content
        /// to the user
        /// </summary>

        public string EmailAddress
        {
            get { return ReadSetting<string>("emailAddress", null); }
            set { WriteSetting("emailAddress", value); }
        }

        public string EmailFrom
        {
            get { return ReadSetting<string>("emailFrom", null); }
            set { WriteSetting("emailFrom", value); }
        }
        public string EmailSubject
        {
            get { return ReadSetting<string>("emailSubject", null); }
            set { WriteSetting("emailSubject", value); }
        }
        public string UrlToCheck
        {
            get { return ReadSetting<string>("urlToCheck", null); }
            set { WriteSetting("urlToCheck", value); }
        }

        public string BrowserWidth
        {
            get { return ReadSetting<string>("browserWidth", null); }
            set { WriteSetting("browserWidth", value); }
        }
        public string BrowserHeight
        {
            get { return ReadSetting<string>("browserHeight", null); }
            set { WriteSetting("browserHeight", value); }
        }
        public string ThumbWidth
        {
            get { return ReadSetting<string>("thumbWidth", null); }
            set { WriteSetting("thumbWidth", value); }
        }
        public string ThumbHeight
        {
            get { return ReadSetting<string>("thumbHeight", null); }
            set { WriteSetting("thumbHeight", value); }
        }

        public string AutoLoad
        {
            get { return ReadSetting<string>("autoLoad", null); }
            set { WriteSetting("autoLoad", value); }
        }

        public string ImageFolder
        {
            get { return ReadSetting<string>("imageFolder", null); }
            set { WriteSetting("imageFolder", value); }
        }

        #endregion
    }
}
