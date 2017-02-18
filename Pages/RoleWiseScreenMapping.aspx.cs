using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MtbBillCollectionLibrary.Security.EntityClass;
using MtbBillCollectionLibrary.Security;
namespace MtbBillCollection.Pages
{
    public partial class RoleWiseScreenMapping : System.Web.UI.Page
    {
        ScreenManager _screenManager = new ScreenManager();
        IList<Screen> listOfScreen;
        bool _isAdmin = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            ReqFieldValidatorForScreen.ForeColor = System.Drawing.Color.Red;
            if (Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsAdmin] != null)
                _isAdmin = Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsAdmin].ToString());
            if (_isAdmin == true)
            {
                if (!Page.IsPostBack)
                {
                    _screenManager = new ScreenManager();
                    _screenManager.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
                    listOfScreen = _screenManager.GetAllScreenInformation();
                    Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.Screenlist, listOfScreen);
                    GridView1.DataSource = listOfScreen;
                    GridView1.DataBind();
                }
                else
                {
                    if (Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.Screenlist] != null)
                        listOfScreen = (IList<Screen>)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.Screenlist];
                }
            }
            else
            {
                Session.Clear();
                Response.Redirect("Login.aspx");
            }

        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in GridView1.Rows)
            {
                
                //LinkButton tempLink = Convert.ChangeType(row["Delete"], LinkButton);
                //if (tempLink == (LinkButton)sender)
                //{
                //    lblStatus.Text = "";
                //    _screenManager = new ScreenManager();
                //    _screenManager.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["CollDBConnectionString"];                   

                //    if (_screenManager.AddNewScreen(screen) == true)
                //    {
                //        lblStatus.ForeColor = System.Drawing.Color.Blue;
                //        lblStatus.Font.Bold = true;
                //        lblStatus.Text = "Screen Added Successfully";
                //    }
                //}
            }
        }

        protected void btnAdd_Click(object sender, ImageClickEventArgs e)
        {
            lblStatus.Text = "";
            _screenManager = new ScreenManager();
            _screenManager.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
            Screen screen = new Screen();
            screen.ScreenName = ScreenName.Text;
            screen.IsManagerPermited = cboManager.Checked;
            screen.IsIssuerPermited = cboIssuer.Checked;
            screen.IsReviewerPermited = cboReviewer.Checked;

            if (_screenManager.AddNewScreen(screen)==true)
            {
                lblStatus.ForeColor = System.Drawing.Color.Blue;
                lblStatus.Font.Bold = true;
                lblStatus.Text = "Screen Added Successfully";
                SetInitialTemplate();
            }
        }

        public void SetInitialTemplate()
        {
            ScreenName.Text = "";
            cboIssuer.Checked = false;
            cboManager.Checked=false;
            cboReviewer.Checked=false;
        }
        
    }
}