using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Services.Scheduling;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Exceptions;
using System.Collections;
using GIBS.DNNSnapshot.Components;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;

namespace GIBS.Modules.DNNSnapshot
{
    public partial class Scheduler : PortalModuleBase   //, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindHistory();

            phScheduler.Visible = this.UserInfo.IsSuperUser;



            if ((IsPostBack == false))
            {
                

                if ((phScheduler.Visible))
                {
                    BindSchedulerSettings();
                    BindHistory();
                }
                else
                {
                    string typeName = "GIBS.DNNSnapshot.Components.DNNSnapshotScheduledTask, GIBS.Modules.DNNSnapshot";
                    ScheduleItem objSchedule = SchedulingProvider.Instance().GetSchedule(typeName, Null.NullString);

                    if ((objSchedule != null))
                    {
                        if ((objSchedule.Enabled))
                        {
                            lblScheduler.Visible = false;
                        }
                        else
                        {
                            lblScheduler.Text = Localization.GetString("SchedulerNotEnabled", this.LocalResourceFile);
                            lblScheduler.Visible = true;
                        }
                    }
                    else
                    {
                        lblScheduler.Text = Localization.GetString("SchedulerNotEnabled", this.LocalResourceFile);
                        lblScheduler.Visible = true;
                    }
                }

            }


        }


        private void BindHistory()
        {
            string typeName = "GIBS.DNNSnapshot.Components.DNNSnapshotScheduledTask, GIBS.Modules.DNNSnapshot";
            ScheduleItem objSchedule = SchedulingProvider.Instance().GetSchedule(typeName, Null.NullString);


            if ((objSchedule != null))
            {
                ArrayList arrSchedule = SchedulingProvider.Instance().GetScheduleHistory(objSchedule.ScheduleID);
             //   ArrayList arrSchedule = SchedulingProvider.Instance.GetScheduleHistory(objSchedule.ScheduleID);


                if ((arrSchedule.Count > 0))
                {
                    arrSchedule.Sort(new ScheduleHistorySortStartDate());

                    //Localize Grid
                   //JOE Localization.LocalizeDataGrid(dgScheduleHistory, this.LocalResourceFile);

                    dgScheduleHistory.DataSource = arrSchedule;
                    dgScheduleHistory.DataBind();

                    lblNoHistory.Visible = false;
                    dgScheduleHistory.Visible = true;
                }
                else
                {
                    lblNoHistory.Visible = true;
                    dgScheduleHistory.Visible = false;
                }


            }
            else
            {
                lblNoHistory.Visible = true;
                dgScheduleHistory.Visible = false;

            }

        }


        protected void cmdUpdate_Click(object sender, EventArgs e)
        {

            try
            {
                string typeName = "GIBS.DNNSnapshot.Components.DNNSnapshotScheduledTask, GIBS.Modules.DNNSnapshot";

                ScheduleItem objSchedule = SchedulingProvider.Instance().GetSchedule(typeName, Null.NullString);


                if ((objSchedule != null))
                {
                    objSchedule.Enabled = chkEnabled.Checked;


                    double Num;
                    bool isNum = double.TryParse(txtTimeLapse.Text, out Num);

                    if (isNum)
                    {
                        objSchedule.TimeLapse = Convert.ToInt32(txtTimeLapse.Text);
                    }
                    else
                    {
                        objSchedule.TimeLapse = 30;
                    }
                    objSchedule.TimeLapseMeasurement = drpTimeLapseMeasurement.SelectedValue;

                    if (isNum)
                    {
                        objSchedule.RetryTimeLapse = Convert.ToInt32(txtRetryTimeLapse.Text);
                    }
                    else
                    {
                        objSchedule.RetryTimeLapse = 60;
                    }
                    objSchedule.RetryTimeLapseMeasurement = drpRetryTimeLapseMeasurement.SelectedValue;

                    SchedulingProvider.Instance().UpdateSchedule(objSchedule);


                }
                else
                {
                    objSchedule = new ScheduleItem();

                    objSchedule.TypeFullName = typeName;
                    objSchedule.Enabled = chkEnabled.Checked;

                    double Num;
                    bool isNum = double.TryParse(txtTimeLapse.Text, out Num);

                    if (isNum)
                    {
                        objSchedule.TimeLapse = Convert.ToInt32(txtTimeLapse.Text);
                    }
                    else
                    {
                        objSchedule.TimeLapse = 30;
                    }
                    objSchedule.TimeLapseMeasurement = drpTimeLapseMeasurement.SelectedValue;

                    if (isNum)
                    {
                        objSchedule.RetryTimeLapse = Convert.ToInt32(txtRetryTimeLapse.Text);
                    }
                    else
                    {
                        objSchedule.RetryTimeLapse = 60;
                    }
                    objSchedule.RetryTimeLapseMeasurement = drpRetryTimeLapseMeasurement.SelectedValue;
                    objSchedule.FriendlyName = "DNNSnapshot";
                    objSchedule.RetainHistoryNum = 10;
                    objSchedule.AttachToEvent = "";
                    objSchedule.CatchUpEnabled = false;
                    objSchedule.Enabled = chkEnabled.Checked;
                    objSchedule.ObjectDependencies = "";
                    objSchedule.Servers = "";

                    SchedulingProvider.Instance().AddSchedule(objSchedule);

                }
                Response.Redirect(Globals.NavigateURL(), true);
                
                //Module failed to load
            }
            catch (Exception ex)
            {
                
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }


        protected void cmdCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Globals.NavigateURL(), true);
        }

        private void BindSchedulerSettings()
        {
            string typeName = "GIBS.DNNSnapshot.Components.DNNSnapshotScheduledTask, GIBS.Modules.DNNSnapshot";

            ScheduleItem objSchedule = SchedulingProvider.Instance().GetSchedule(typeName, Null.NullString);

            if ((objSchedule != null))
            {
                chkEnabled.Checked = objSchedule.Enabled;

                txtTimeLapse.Text = objSchedule.TimeLapse.ToString();
                if ((drpTimeLapseMeasurement.Items.FindByValue(objSchedule.TimeLapseMeasurement) != null))
                {
                    drpTimeLapseMeasurement.SelectedValue = objSchedule.TimeLapseMeasurement;
                }
                else
                {
                    drpTimeLapseMeasurement.SelectedValue = "h";
                }

                txtRetryTimeLapse.Text = objSchedule.RetryTimeLapse.ToString();
                if ((drpRetryTimeLapseMeasurement.Items.FindByValue(objSchedule.RetryTimeLapseMeasurement) != null))
                {
                    drpRetryTimeLapseMeasurement.SelectedValue = objSchedule.RetryTimeLapseMeasurement;
                }
                else
                {
                    drpRetryTimeLapseMeasurement.SelectedValue = "m";
                }
            }
            else
            {
                txtTimeLapse.Text = "8";
                drpTimeLapseMeasurement.SelectedValue = "h";

                txtRetryTimeLapse.Text = "60";
                drpRetryTimeLapseMeasurement.SelectedValue = "m";
            }

        }




    //    #region IActionable Members

    //    public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
    //    {
    //        get
    //        {
    //            //create a new action to add an item, this will be added to the controls
    //            //dropdown menu
    //            ModuleActionCollection actions = new ModuleActionCollection();



    //            actions.Add(GetNextActionID(), Localization.GetString(ModuleActionType.AddContent, this.LocalResourceFile),
    //ModuleActionType.EditContent, "", "", EditUrl(), false, DotNetNuke.Security.SecurityAccessLevel.Edit,
    // true, false);


    //            return actions;
    //        }
    //    }

    //    #endregion

 



    }
}