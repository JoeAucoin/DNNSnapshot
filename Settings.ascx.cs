using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;

using GIBS.DNNSnapshot.Components;
using System.Collections;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.FileSystem;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Localization;

namespace GIBS.Modules.DNNSnapshot
{
    public partial class Settings : ModuleSettingsBase
    {

        /// <summary>
        /// handles the loading of the module setting for this
        /// control
        /// </summary>
        public override void LoadSettings()
        {
            try
            {
                if (!IsPostBack)
                {
                    BindFolders();

                    DNNSnapshotSettings settingsData = new DNNSnapshotSettings(this.TabModuleId);

                    if (settingsData.UrlToCheck != null)
                    {
                        txtUrlToCheck.Text = settingsData.UrlToCheck;
                    }

                    if (settingsData.EmailAddress != null)
                    {
                        txtEmailAddress.Text = settingsData.EmailAddress;
                    }
                    if (settingsData.EmailFrom != null)
                    {
                        txtEmailFrom.Text = settingsData.EmailFrom;
                    }
                    if (settingsData.EmailSubject != null)
                    {
                        txtEmailSubject.Text = settingsData.EmailSubject;
                    }
                    if (settingsData.BrowserHeight != null)
                    {
                        txtBrowserHeight.Text = settingsData.BrowserHeight;
                    }

                    if (settingsData.BrowserWidth != null)
                    {
                        txtBrowserWidth.Text = settingsData.BrowserWidth;
                    }
                    if (settingsData.ThumbHeight != null)
                    {
                        txtThumbHeight.Text = settingsData.ThumbHeight;
                    }
                    if (settingsData.ThumbWidth != null)
                    {
                        txtThumbWidth.Text = settingsData.ThumbWidth;
                    }

                    if (settingsData.AutoLoad != null)
                    {
                        rblAutoLoad.SelectedValue = settingsData.AutoLoad;
                    }
                    if (settingsData.ImageFolder != null)
                    {
                        drpDefaultImageFolder.SelectedValue = settingsData.ImageFolder;
                    }

                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        private void BindFolders()
        {
            ArrayList folders = FileSystemUtils.GetFolders(PortalId);
            foreach (FolderInfo folder in folders)
            {
                ListItem FolderItem = new ListItem();
                if (folder.FolderPath == Null.NullString)
                {
                    FolderItem.Text = Localization.GetString("Root", this.LocalResourceFile);
                }
                else
                {
                    FolderItem.Text = folder.FolderPath;
                }
                FolderItem.Value = folder.FolderPath.ToString();
                //FolderItem.Value = folder.FolderID.ToString();
                drpDefaultImageFolder.Items.Add(FolderItem);
            //    drpDefaultFileFolder.Items.Add(new ListItem(FolderItem.Text, FolderItem.Value));
            }

        }



        /// <summary>
        /// handles updating the module settings for this control
        /// </summary>
        public override void UpdateSettings()
        {
            try
            {
                DNNSnapshotSettings settingsData = new DNNSnapshotSettings(this.TabModuleId);
                settingsData.BrowserHeight = txtBrowserHeight.Text;
                settingsData.BrowserWidth = txtBrowserWidth.Text;
                settingsData.ThumbHeight = txtThumbHeight.Text;
                settingsData.ThumbWidth = txtThumbWidth.Text;
                settingsData.EmailSubject = txtEmailSubject.Text;
                settingsData.EmailAddress = txtEmailAddress.Text;
                settingsData.UrlToCheck = txtUrlToCheck.Text;
                settingsData.AutoLoad = rblAutoLoad.SelectedValue;
                settingsData.ImageFolder = drpDefaultImageFolder.SelectedValue;
                settingsData.EmailFrom = txtEmailFrom.Text;
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
    }
}