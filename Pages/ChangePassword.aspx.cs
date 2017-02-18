using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MtbBillCollectionLibrary.Security;
namespace MtbBillCollection.Pages
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            lblError.Text = "";
            lblMessage.Text="";
            try
            {
                AuthenticationManager authenticationmanger = new AuthenticationManager();
                authenticationmanger.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString(); ;
                authenticationmanger.ChangePassword(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserName].ToString(), txtOldPassword.Text, txtPassword.Text);
                lblMessage.Text = "Password change successfull.";
            }
            catch (Exception ee)
            {
                lblError.Text = "Password change unsuccessfull.";
            }
        }
    }
}