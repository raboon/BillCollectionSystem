using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MtbBillCollection.Pages
{
    public partial class MenuControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] != null && Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn].Equals("True"))
            {
                //Turn on the menu control panel
                pnlMenuItems.Visible = true;

                int userType=MtbBillCollection.Global.Definitions.UserType.Value.Issuer;
                Int32.TryParse(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId].ToString(), out userType);


                //Top info part
                LitCollDate.Text = DateTime.Now.Date.ToString("dd/MMM/yyyy");
                LitUserName.Text = Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.FullName].ToString();
                LitBranchName.Text = " @ " + Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.BranchName].ToString();

                string[] pageList = (string[])Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.PageList];
                System.Collections.Hashtable hs = (System.Collections.Hashtable)Application[MtbBillCollection.Global.Definitions.SessionVariable.Value.MenuList];
                if (pageList != null)
                {
                    Menu1.Items.Clear();
                    
                    foreach (string pageName in pageList)
                    {
                        if (hs.Contains(pageName))
                        {
                            MenuItem menuItem = new MenuItem();
                            menuItem.Text = pageName;
                            menuItem.NavigateUrl = hs[pageName].ToString();                            
                            Menu1.Items.Add(menuItem);
                        }
                    }                   
                }

                LitLogMenu.Text = "<a href='Logout.aspx' style=' color:GREEN' >Logout</a>";
             
                //menu part
                if (userType == MtbBillCollection.Global.Definitions.UserType.Value.SuperAdmin)
                {
                    //LitMenu1.Text = "<a href='Register.aspx' >Register User</a>";
                    //LitMenu2.Text = "<a href='BillCollection.aspx' >Bill Collection</a>";
                    //LitMenu3.Text = "<a href='AuthorizeBillCollection.aspx' >Manage Collection</a>";
                    //LitMenuPassReset.Text = "<a href='RecoverPassword.aspx' >Recover Password</a>";
                    //LitLogMenu.Text = "<a href='Logout.aspx' >Logout</a>";
                }
                else if (userType == MtbBillCollection.Global.Definitions.UserType.Value.Manager)
                {
                    //LitMenu1.Text = "<a href='AuthorizeBillCollection.aspx' >Manage Collection</a>";
                    //LitMenu2.Text = "<a href='ViewCollections.aspx' >View Collection</a>";
                    //LitMenu3.Text = "<a href='CollectionLogReport.aspx' >Upload Log</a>";
                    //LitMenu3.Text = "<a href='Clearing.aspx' >Clearing</a>";
                    //LitLogMenu.Text = "<a href='Logout.aspx' >Logout</a>";
                }
                else if (userType == MtbBillCollection.Global.Definitions.UserType.Value.Issuer)
                {
                    //LitMenu1.Text = "<a href='BillCollection.aspx' >Bill Collection</a>";
                    //LitMenu2.Text = "<a href='ViewCollections.aspx' >View Collection</a>";                   
                    //LitLogMenu.Text = "<a href='Logout.aspx' >Logout</a>";
                }
                else if (userType == MtbBillCollection.Global.Definitions.UserType.Value.Reviewer)
                {
                    //LitMenu1.Text = "<a href='BillCollection.aspx' >Bill Collection</a>";
                    //LitMenu2.Text = "<a href='Clearing.aspx' >Clearing</a>";
                    //LitLogMenu.Text = "<a href='Logout.aspx' >Logout</a>";
                }

                //LitMenuChangePass.Text = "<a href='ChangePassword.aspx' >Change Password</a>";
            }
            else
            {
                pnlMenuItems.Visible = false;
                LitLogMenu.Text = "";
            }
        }
    }
}