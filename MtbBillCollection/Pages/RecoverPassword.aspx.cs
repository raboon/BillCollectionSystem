using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MtbBillCollectionLibrary.Security;
using System.Data;
using MtbBillCollectionLibrary.DAL;

namespace MtbBillCollection.Pages
{
    public partial class RecoverPassword : System.Web.UI.Page
    {
        AuthenticationManager _authenticationManager = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] == null || Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn]) == false ||
               (MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId].ToString()), "RecoverPassword")) == false)
            {
                Response.Redirect("/Default.aspx");
            }
            if (!Page.IsPostBack)
            {
                _authenticationManager = new AuthenticationManager();
                _authenticationManager.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString(); ;
                txtLogInName.Text = "All";
                LoadMachingCriteriaCombo();
                LoadUserType();
                LoadBranchList();
                SearchUserInfo();
            }
            lblStatus.Text = "";
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            SearchUserInfo();
        }

        private void SearchUserInfo()
        {
            _authenticationManager = new AuthenticationManager();
            _authenticationManager.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
            List<User> listUser = _authenticationManager.GetUserDetails(LogInName.Text, listBranch.SelectedValue, listUserType.SelectedValue, listMachingCriteria.SelectedValue);
            GridView1.DataSource = listUser;
            Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.UserList, listUser);
            GridView1.DataBind();
        }

        public void LoadMachingCriteriaCombo()
        {
            listMachingCriteria.Items.Add(new ListItem("=",MtbBillCollection.Global.Definitions.MatchingCriteria.Value.Equal.ToString()));
            listMachingCriteria.Items.Add(new ListItem("Starts With", MtbBillCollection.Global.Definitions.MatchingCriteria.Value.StartsWith.ToString()));
            listMachingCriteria.Items.Add(new ListItem("Ends With", MtbBillCollection.Global.Definitions.MatchingCriteria.Value.EndsWith.ToString()));
            listMachingCriteria.Items.Add(new ListItem("Contains", MtbBillCollection.Global.Definitions.MatchingCriteria.Value.Contains.ToString()));
            listMachingCriteria.DataBind();
        }

        public void LoadUserType()
        {      
            listUserType.DataSource = _authenticationManager.GetUserTypeList();            
            listUserType.DataBind();
            listUserType.Items.Add(new ListItem("All", "ALL"));
            listUserType.SelectedIndex = listUserType.Items.Count - 1;
        }

        public void LoadBranchList()
        {     
            listBranch.DataSource = _authenticationManager.GetBranchList();            
            listBranch.DataBind();
            listBranch.Items.Add(new ListItem("All", "ALL"));
            listBranch.SelectedIndex = listBranch.Items.Count - 1;
        }        

        protected void btnForReset_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "";
            txtNewPass.Text = "";
            int index = GetGridSelecttedIndex((Button)sender);           
            if (index > -1)
            {
                List<User> listUser = (List<User>)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserList];
                PanelBody.Visible = true;
                ResetPass.Enabled = true;
                txtLogInName.Text = listUser[index].LoginName;                
                this.programmaticModalPopup.Show();
            }
            
        }

        public int GetGridSelecttedIndex(Button reset)
        {
            int i = 0;
            foreach (GridViewRow row in GridView1.Rows)
            {
                Button button = (Button)row.FindControl("btnForReset");
                if (reset == button)
                {
                    return i; 
                }
                i++;
            }

            return -1;
        }

        public void ResetPass_Click(object sender, EventArgs e)
        {
            _authenticationManager=new AuthenticationManager();
            _authenticationManager.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString(); ;
             
            if (_authenticationManager.ChangeUserpassword(txtLogInName.Text,txtNewPass.Text)==true)
            {               
                lblStatus.Text="Password Successfully changed.";
            }
            else
               lblStatus.Text="Password changed Unsuccessfull.";
        }

    }
}