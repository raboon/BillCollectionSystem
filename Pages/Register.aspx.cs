using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MtbBillCollectionLibrary.Security;
using MtbBillCollectionLibrary.DAL;

namespace MtbBillCollection.Pages
{
    public partial class Register : System.Web.UI.Page
    {
        AuthenticationManager _authenticationManger = new AuthenticationManager();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] == null || Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn]) == false ||
               (MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId].ToString()), "Register")) == false)
            {
                Response.Redirect("/Default.aspx");
            } 
            if (!IsPostBack)
            {
                _authenticationManger.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
                BranchList.DataSource = _authenticationManger.GetBranchList();
                BranchList.DataBind();
                _authenticationManger.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
                UserTypeList.DataSource = _authenticationManger.GetUserTypeList();
                UserTypeList.DataBind();
            }
        }

        protected void btnCreateNewUser_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                User user = new User();

                user.LoginName = txtLoginName.Text;

                if (txtPassword.Text.Equals(txtPasswordRe.Text))
                {
                    user.Password = txtPasswordRe.Text;
                }
                else
                {
                    //write exception handling code
                }

                user.UserDetails = txtUserDetails.Text;
                user.UserName = txtUserName.Text;
                user.BranchCode = BranchList.SelectedValue;
                user.UserTypeId = byte.Parse(UserTypeList.SelectedValue);
                user.isActive = true;
                user.CreationDate = DateTime.Now;
                MtbBillCollectionLibrary.Security.AuthenticationManager _authenticationManger = new MtbBillCollectionLibrary.Security.AuthenticationManager();
                _authenticationManger.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
                _authenticationManger.CreteNewUser(user);

                InitRegisterFields();

            }
            catch
            {

            }
        }

        protected void InitRegisterFields()
        {
            txtUserName.Text = "";
            txtLoginName.Text = "";
            txtUserDetails.Text = "";
            txtPassword.Text = "";
            txtPasswordRe.Text = "";

            txtLoginName.Focus();
        }
    }
}