using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DotNetNuke.Services.Scheduling;
using System.IO;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Common;
using GIBS.DNNSnapshot.Components;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Net;
using DotNetNuke.Entities.Portals;
using System.Collections;

namespace GIBS.DNNSnapshot.Components
{
    public class DNNSnapshotScheduledTask : DotNetNuke.Services.Scheduling.SchedulerClient
    {

        public DNNSnapshotScheduledTask(DotNetNuke.Services.Scheduling.ScheduleHistoryItem objScheduleHistoryItem)
            : base()
        {
            this.ScheduleHistoryItem = objScheduleHistoryItem;
        }


        public override void DoWork()
        {
            try
            {


                // perform some task     
                List<DNNSnapshotInfo> items;
                DNNSnapshotController controller = new DNNSnapshotController();

                //string vText = HttpContext.Current.Request.Url.Host.ToString();
                items = controller.GetDNNSnapshotModules();


                // items[0].ToString();
                string myList = "";
                string myMessage = "";
                
                string vPortalAlias = "";
                

                for (int i = 0; i <= items.Count - 1; i++)
                {
                    PortalAliasController paController = new PortalAliasController();
                    PortalAliasCollection aliasCollection = paController.GetPortalAliasByPortalID(items[i].PortalID);
                    IDictionaryEnumerator hs = aliasCollection.GetEnumerator();
                    hs.MoveNext();
                    PortalAliasInfo paInfo = (PortalAliasInfo)hs.Entry.Value;

                    vPortalAlias = paInfo.HTTPAlias;


                    if (CheckPage("http://" + vPortalAlias + "/Default.aspx?TabID=" + items[i].TabID.ToString() + "&ScheduledTask=true", out myMessage) == true)
                    {
                        myList += "Status " + myMessage + " - Tab " + items[i].TabID.ToString() + " Loaded!<br>";  
                    }
                    else
                    {
                        myList += myMessage + " - ERROR!!!<br>";
                    }


    
                }

                
                // report success to the scheduler framework
                ScheduleHistoryItem.Succeeded = true;
                this.ScheduleHistoryItem.AddLogNote("Processing completed at " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + "<br>" +  myList.ToString());
            }
            catch (Exception exc)
            {
                ScheduleHistoryItem.Succeeded = false;
                ScheduleHistoryItem.AddLogNote("EXCEPTION: " + exc.ToString());
                Errored(ref exc);
                Exceptions.LogException(exc);
            }
        }
       
        



        private bool CheckPage(string url, out string message)
        {
            try
            {
                //Creating the HttpWebRequest         
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; en-US)";
                //Setting the Request method HEAD, you can also use GET too.         
                request.Method = "GET";
                //Getting the Web Response.         
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TURE if the Status code == 200  
                if (response == null || response.StatusCode != HttpStatusCode.OK)
                {
                    message = response.StatusCode.ToString() + " ERROR";
                    return false;
                }
                else
                {
                    // ALL IS GOOD!
                    message = response.StatusCode.ToString();
                    return (response.StatusCode == HttpStatusCode.OK);
                }


            }
            catch (Exception ex)
            {
                //Any exception will returns false. 
                message = ex.Message + " EXCEPTION";
                return false;

            }
        }
    }
}