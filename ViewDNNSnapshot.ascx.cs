using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.DNNSnapshot.Components;
using System.Drawing;
using System.Drawing.Imaging;
using DotNetNuke.Entities.Portals;

namespace GIBS.Modules.DNNSnapshot
{
    public partial class ViewDNNSnapshot : PortalModuleBase, IActionable
    {

        public DateTime begintime;
        public DateTime stoptime;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    DNNSnapshotSettings settingsData = new DNNSnapshotSettings(this.TabModuleId);


                    if (settingsData.AutoLoad == "true" || Request.QueryString["ScheduledTask"] == "true")
                    {

                        if (settingsData.UrlToCheck != null)
                        {



                            if (settingsData.UrlToCheck.Length > 1)
                            {

                                TakeSnapshot();

                            }
                            

                        }
                        

                    }
                    else
                    {

                        
                        if (settingsData.UrlToCheck != null)

                        {
                            if (settingsData.UrlToCheck.Length > 1)
                            {

                                txtURL.Text = settingsData.UrlToCheck;

                            }
                    
                        
                        }

                        else

                        {
                            lblDebug.Text = "Click Settings to Configure Module";
                        }


                        
                    }



                    
                }
                else
                {
                    
                }

            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }


        }

        protected void TakeSnapshot()
        {

            try
            {
                DNNSnapshotSettings settingsData = new DNNSnapshotSettings(this.TabModuleId);

                if (settingsData.UrlToCheck.Length > 1)
                {
                    txtURL.Text = settingsData.UrlToCheck;

                    StartDuration();
                    
                    Bitmap bmp = ClassWSThumb.GetWebSiteThumbnail(settingsData.UrlToCheck, Int32.Parse(settingsData.BrowserWidth), Int32.Parse(settingsData.BrowserHeight), Int32.Parse(settingsData.ThumbWidth), Int32.Parse(settingsData.ThumbHeight));
                    string MyFileName = settingsData.UrlToCheck.Replace("http://", "").Replace("https://", "");
                    MyFileName = MyFileName.Replace("/", "\\");
                    bmp.Save(PortalSettings.HomeDirectoryMapPath.ToString() + settingsData.ImageFolder + MyFileName + ".jpg", ImageFormat.Jpeg);
                    ImageBox.ImageUrl = "~" + PortalSettings.HomeDirectory.ToString() + settingsData.ImageFolder + MyFileName + ".jpg";
                    ImageBox.Visible = true;
                    StopDuration();

                    string SnapURL = PortalSettings.HomeDirectory.ToString() + settingsData.ImageFolder +  MyFileName + ".jpg";
                    SendNotifications(SnapURL);
                }



            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }        
        }

        protected void Button2_Click(object sender, EventArgs e)
        {


            try
            { 
                
                StartDuration();
                DNNSnapshotSettings settingsData = new DNNSnapshotSettings(this.TabModuleId);
                Bitmap bmp = ClassWSThumb.GetWebSiteThumbnail(txtURL.Text.ToString(), Int32.Parse(settingsData.BrowserWidth), Int32.Parse(settingsData.BrowserHeight), Int32.Parse(settingsData.ThumbWidth), Int32.Parse(settingsData.ThumbHeight));
                string MyFileName = txtURL.Text.ToString().Replace("https://", "").Replace("http://", "");
                MyFileName = MyFileName.Replace("/", "\\");
                lblDebug.Text = PortalSettings.HomeDirectoryMapPath.ToString() + settingsData.ImageFolder.ToString().Replace("/", "\\").ToString() + MyFileName.ToString() + ".jpg";

                bmp.Save(PortalSettings.HomeDirectoryMapPath.ToString() + settingsData.ImageFolder + MyFileName + ".jpg", ImageFormat.Jpeg);
                ImageBox.ImageUrl = "~" + PortalSettings.HomeDirectory.ToString() + settingsData.ImageFolder.ToString().Replace("/", "\\").ToString() + MyFileName + ".jpg";
                ImageBox.Visible = true;
                StopDuration();

                string SnapURL = PortalSettings.HomeDirectory.ToString() + settingsData.ImageFolder + MyFileName + ".jpg";
            //    SendNotifications(SnapURL);            
            }

            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }   



        }

        protected void StartDuration()
        {
            begintime = DateTime.Now;
        }

        protected void StopDuration()
        {
            lblRequestTime.Visible = true;
            stoptime = DateTime.Now;
            TimeSpan duration = (stoptime - begintime);
            lblTime.Text = duration.ToString();
        }


        public void SendNotifications(string SnapshotURL)
        {
            DNNSnapshotSettings settingsData = new DNNSnapshotSettings(this.TabModuleId);

            if (settingsData.EmailAddress.Length > 1)
            {

                string vPortalAlias = "";

                string MailFrom = "";
                if (settingsData.EmailFrom.Length > 7)
                {
                    MailFrom = settingsData.EmailFrom;
                }
                else
                {
                    MailFrom = PortalSettings.Email;
                }


                PortalAliasController paController = new PortalAliasController();
                PortalAliasCollection aliasCollection = paController.GetPortalAliasByPortalID(this.PortalId);
                IDictionaryEnumerator hs = aliasCollection.GetEnumerator();
                hs.MoveNext();
                PortalAliasInfo paInfo = (PortalAliasInfo)hs.Entry.Value;

                vPortalAlias = paInfo.HTTPAlias;


                string EmailContent = "<p>URL: " +txtURL.Text.ToString() + "<br>Time Taken: " + DateTime.Now + "<br>Snapshot Request Time: " + lblTime.Text + "</p>";
                EmailContent += "<p align='center'><img src='http://" + vPortalAlias + SnapshotURL + "'></p>";
                EmailContent += "<p>" + PortalSettings.PortalName + " - http://" +  vPortalAlias +  Request.RawUrl + "<p>";
                string emailAddress = settingsData.EmailAddress;
                string[] valuePair = emailAddress.Split(new char[] { ';' });

                for (int i = 0; i <= valuePair.Length - 1; i++)
                {
                    // DotNetNuke.Services.Mail.Mail.SendEmail(PortalSettings.Email, valuePair[i].ToString().Trim(), settingsData.EmailSubject, EmailContent.ToString());
                    DotNetNuke.Services.Mail.Mail.SendMail(MailFrom, valuePair[i].ToString().Trim(), "", settingsData.EmailSubject + " - " + txtURL.Text.ToString(), EmailContent.ToString(), Server.MapPath("") +  SnapshotURL, "HTML", "", "", "", "");
                }



            }



        }



        #region IActionable Members

        public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
        {
            get
            {
                //create a new action to add an item, this will be added to the controls
                //dropdown menu
                ModuleActionCollection actions = new ModuleActionCollection();
                
                //actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
                //    ModuleActionType.EditContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                //     true, false);


                actions.Add(GetNextActionID(), Localization.GetString("Schedule", this.LocalResourceFile),
                ModuleActionType.EditContent, "", "", EditUrl("Schedule"), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
                 true, false);
                return actions;
            }
        }

        #endregion


        /// <summary>
        /// Handles the items being bound to the datalist control. In this method we merge the data with the
        /// template defined for this control to produce the result to display to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lstContent_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            //Label content = (Label)e.Item.FindControl("lblContent");
            //string contentValue = string.Empty;

            //DNNSnapshotSettings settingsData = new DNNSnapshotSettings(this.TabModuleId);

            //if (settingsData.Template != null)
            //{
            //    //apply the content to the template
            //    ArrayList propInfos = CBO.GetPropertyInfo(typeof(DNNSnapshotInfo));
            //    contentValue = settingsData.Template;

            //    if (contentValue.Length != 0)
            //    {
            //        foreach (PropertyInfo propInfo in propInfos)
            //        {
            //            object propertyValue = DataBinder.Eval(e.Item.DataItem, propInfo.Name);
            //            if (propertyValue != null)
            //            {
            //                contentValue = contentValue.Replace("[" + propInfo.Name.ToUpper() + "]",
            //                        Server.HtmlDecode(propertyValue.ToString()));
            //            }
            //        }
            //    }
            //    else
            //        //blank template so just set the content to the value
            //        contentValue = Server.HtmlDecode(DataBinder.Eval(e.Item.DataItem, "Content").ToString());
            //}
            //else
            //{
            //    //no template so just set the content to the value
            //    contentValue = Server.HtmlDecode(DataBinder.Eval(e.Item.DataItem, "Content").ToString());
            //}

            //content.Text = contentValue;
        }

    }
}