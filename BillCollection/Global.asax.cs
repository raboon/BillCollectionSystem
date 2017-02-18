using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using MtbBillCollection.Utility;
using MtbBillCollectionLibrary.Security;

namespace MtbBillCollection
{
    public class GlobalApp : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            System.Collections.Hashtable hs=Screen.LoadAllMenu();
            Application.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.MenuList, hs);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            LogManager loagManager = new LogManager();
            loagManager.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
            string userId =(string) Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserId];
            if(userId!=null && userId!="")
                loagManager.UpdateLog(userId);
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}