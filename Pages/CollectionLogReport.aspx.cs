using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Globalization;
using MtbBillCollectionLibrary.Security;
using System.Drawing;

namespace MtbBillCollection.Pages
{
    public partial class CollectionLogReport : System.Web.UI.Page
    {
        //System.Configuration.Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] == null || Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn]) == false ||
                (MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId].ToString()), "CollectionLogReport")) == false)
            {
                Response.Redirect("/Default.aspx");
            } 
            if (!Page.IsPostBack)
            {
                fileTypeList.Items.Add(new ListItem("All", ""+MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.All));
                fileTypeList.Items.Add(new ListItem("Prepaid", "" + MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.Prepaid));
                fileTypeList.Items.Add(new ListItem("PostPaid", "" + MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.PostPaid));
                fileTypeList.Items.Add(new ListItem("DayEnd", "" + MtbBillCollectionLibrary.Security.EntityClass.FileType.Value.DayEnd));
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                fileTypeList.SelectedIndex = 0;
            }
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            MtbBillCollectionLibrary.Security.LogManager logManager=new MtbBillCollectionLibrary.Security.LogManager();
            //cfg.AppSettings.Settings[""]
            DateTime CollFromDate = DateTime.Now.Date, CollToDate = DateTime.Now.Date;
            if (!txtFromDate.Text.Equals("") && !txtToDate.Text.Equals(""))
            {
                

                if (!DateTime.TryParseExact(txtFromDate.Text, "dd/MM/yyyy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out CollFromDate))
                {
                    // Parse failed
                }
                if (!DateTime.TryParseExact(txtToDate.Text, "dd/MM/yyyy",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.None,
                                           out CollToDate))
                {
                    // Parse failed
                }
               
            }
            List<MtbBillCollectionLibrary.Security.EntityClass.FileInfo> listOfFileInfo = logManager.GetLogInformation("C:/", CollFromDate, CollToDate, Convert.ToInt32(fileTypeList.SelectedValue));
            if (listOfFileInfo.Count < 1)
            {
                ShowPopup(true, true, "No data found.");
                collectiomInfo.DataSource = null;
                collectiomInfo.DataBind();
            }
            else
            {
                collectiomInfo.DataSource = listOfFileInfo;
                collectiomInfo.DataBind();
            }
            
           
        }

        public void ShowPopup(bool IsSuccessfull, bool IsValidationMessege, string validationMessege)
        {
            if (IsSuccessfull == true)
            {
                PopupHeader.ForeColor = Color.Green;
                PopupHeader.Text = "Information.";
                if (IsValidationMessege == true)
                    PopupMessage.Text = validationMessege;
                else
                    PopupMessage.Text = "Data saved successfully.";
            }
            else
            {
                PopupHeader.ForeColor = Color.Red;
                PopupHeader.Text = "Error.";
                if (IsValidationMessege)
                {
                    PopupMessage.Text = validationMessege;
                }
                else
                {
                    PopupMessage.Text = "Data Canot Not Save due some error.\n Please contact system your admistrator.";
                }
            }
            MessegePopupExtender.Show();
        }
    }
}