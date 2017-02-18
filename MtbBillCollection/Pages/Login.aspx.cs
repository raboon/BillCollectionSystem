using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MtbBillCollection.Utility;
using MtbBillCollectionLibrary.Security;


namespace MtbBillCollection.Pages
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string value=(string)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn];
            bool isLogin =(value!=null && value!="")?true:false;
            if(isLogin==true)
                Session.Abandon();
        }

        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {

            AuthenticationManager authenticationManger = new AuthenticationManager();
            authenticationManger.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
            bool IsAdmin = false;
            bool IsActiveUser = false;
            string branchCode = "", branchName = "", userName = "", userId="";
            int userTypeId = 0;

            if (authenticationManger.IsValidUser(ref userId,ref userName, ref IsActiveUser, ref IsAdmin, ref userTypeId, ref branchCode, ref branchName, txtLoginName.Text, txtPassword.Text))
            {
                LogManager loagManager = new LogManager();
                loagManager.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
                loagManager.InsertLog(userId);

                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.UserId, userId);
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn, "True");
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.UserName, txtLoginName.Text);
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.FullName, userName);
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.IsAdmin, IsAdmin);
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.BranchCode, branchCode);
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.BranchName, branchName);
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId, userTypeId);
                string[] pageList = Screen.GetPageList(userTypeId);
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.PageList, pageList);
                //System.Collections.Hashtable hs = (System.Collections.Hashtable)Application[MtbBillCollection.Global.Definitions.SessionVariable.Value.MenuList];
                //Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.MenuList, Screen.GetAllLinkAddress(hs,pageList));
                if (IsActiveUser)
                {
                    if (userTypeId == MtbBillCollection.Global.Definitions.UserType.Value.SuperAdmin)
                        Response.Redirect("Register.aspx");
                    else if (userTypeId == MtbBillCollection.Global.Definitions.UserType.Value.Issuer)
                        Response.Redirect("BillCollection.aspx");
                    else if (userTypeId == MtbBillCollection.Global.Definitions.UserType.Value.Manager)
                        Response.Redirect("AuthorizeBillCollection.aspx");
                    else if (userTypeId == MtbBillCollection.Global.Definitions.UserType.Value.Reviewer)
                        Response.Redirect("ViewCollections.aspx");
                }
                else
                {
                    Response.Redirect("AccountActivation.aspx");
                }
            }
        }
    }
}