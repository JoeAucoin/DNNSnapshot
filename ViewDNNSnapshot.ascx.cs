using System;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using GIBS.DNNSnapshot.Components;
using System.Drawing;
using System.Drawing.Imaging;
using DotNetNuke.Entities.Portals;
using System.Linq;

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
                                txtURL.Text = settingsData.UrlToCheck.ToString();
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
                                txtURL.Text = settingsData.UrlToCheck.ToString();
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
                
                    StartDuration();

                    Bitmap bmp = ClassWSThumb.GetWebSiteThumbnail(txtURL.Text.ToString(), Int32.Parse(settingsData.BrowserWidth), Int32.Parse(settingsData.BrowserHeight), Int32.Parse(settingsData.ThumbWidth), Int32.Parse(settingsData.ThumbHeight));
                    string MyFileName = txtURL.Text.ToString().Replace("https://", "").Replace("http://", "");
                    MyFileName = MyFileName.Replace("/", "\\");
                    //     lblDebug.Text = PortalSettings.HomeDirectoryMapPath.ToString() + settingsData.ImageFolder.ToString().Replace("/", "\\").ToString() + MyFileName.ToString() + ".jpg";

                    bmp.Save(PortalSettings.HomeDirectoryMapPath.ToString() + settingsData.ImageFolder + MyFileName + ".jpg", ImageFormat.Jpeg);
                    ImageBox.ImageUrl = "~" + PortalSettings.HomeDirectory.ToString() + settingsData.ImageFolder.ToString().Replace("/", "\\").ToString() + MyFileName + ".jpg?timestamp=1";
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
                TakeSnapshot();
           
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

            try
            {

                DNNSnapshotSettings settingsData = new DNNSnapshotSettings(this.TabModuleId);

                if (settingsData.EmailAddress.Length > 1)
                {
               
                    string MailFrom = PortalSettings.Email.ToString();
                    string mySubject = settingsData.EmailSubject.ToString();
                    string SMTPUserName = DotNetNuke.Entities.Controllers.HostController.Instance.GetString("SMTPUsername");
                    string myBaseURL = this.PortalSettings.DefaultPortalAlias.ToString();

                    string EmailContent = "<p>URL: " + txtURL.Text.ToString() + "<br>Time Taken: " + DateTime.Now + "<br>Snapshot Request Time: " + lblTime.Text + "</p>";
                    EmailContent += "<p align='center'><img src='http://" + myBaseURL.ToString() + SnapshotURL + "' alt='" + txtURL.Text.ToString() + "'></p>";
                    EmailContent += "<p>" + PortalSettings.PortalName + " - http://" + myBaseURL.ToString() + Request.RawUrl + "<p>";
                    
                    string emailAddress = settingsData.EmailAddress;
                    var emailListToSend = emailAddress.Split(';').ToList();
        
                    string[] emailAttachemnts1 = new string[] { };
                      string sendToEmailAddress = "";

                    for (int i = 0; i < emailListToSend.Count; i++)
                    {
                        sendToEmailAddress = emailListToSend[i].ToString().Trim();
                        DotNetNuke.Services.Mail.Mail.SendMail(SMTPUserName.ToString(), sendToEmailAddress.ToString(), "", "", sendToEmailAddress.ToString(), DotNetNuke.Services.Mail.MailPriority.Normal, mySubject.ToString(), DotNetNuke.Services.Mail.MailFormat.Html, System.Text.ASCIIEncoding.ASCII, EmailContent.ToString(), emailAttachemnts1, string.Empty, string.Empty, string.Empty, string.Empty, true);
                                                                               //MailFrom,             MailTo,                     Cc, Bcc,     ReplyTo,                                        Priority,                          Subject,                                       BodyFormat,                         BodyEncoding,           Body,                     Attachments, SMTPServer, SMTPAuthentication, SMTPUsername, SMTPPassword, SMTPEnableSSL
                    }

                }

            }

            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
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




    }
}