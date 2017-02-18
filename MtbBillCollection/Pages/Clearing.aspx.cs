using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

namespace MtbBillCollection.Pages
{
    public partial class Clearing : System.Web.UI.Page
    {
        private BillCollectionManager _bilCollectionManager = new BillCollectionManager();
        private DataTable _dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] == null || Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn]) == false) ||
               (MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session["userTypeId"].ToString()), "Clearing")) == false)
            {
                Response.Redirect("/Default.aspx");
            }

            if (!IsPostBack)
            {
                ClientList.DataSource = _bilCollectionManager.GetClientist();
                ClientList.DataBind();
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                if (Request.QueryString.Count > 0)
                {
                    ClientList.SelectedValue = Request.QueryString[MtbBillCollection.Global.Definitions.SessionVariable.Value.ClientId].ToString();
                    GetAllNonCashBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), false, MtbBillCollection.Global.Definitions.CollectionStatus.Value.Approved, "", "");
                }
                else
                {
                    btnApprove.Visible = false;
                    btnCancel.Visible = false;
                }
            }
            
        }

        protected void ButtonSearch_Click(object sender, ImageClickEventArgs e)
        {
            GetAllNonCashBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue),false, MtbBillCollection.Global.Definitions.CollectionStatus.Value.Approved, txtFromDate.Text, txtToDate.Text);
        }

        protected void ClientList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnApprove.Visible = false;
            btnCancel.Visible = false;
            gridCollList.DataSource = null;
            gridCollList.DataBind();
        }      

        private void GetAllNonCashBillColeectionInfoByClientId(int clientId,bool IsClearColl, int collStatus, string fromDate, string toDate)
        {
            gridCollList.AutoGenerateColumns = false;
            int userType = (int)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId];
            string branchCode = (String)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.BranchCode];
            _dt = _bilCollectionManager.GetNonCashColllectionList(collStatus, clientId, IsClearColl, userType, branchCode, fromDate,toDate);
            
            if (_dt.Rows.Count > 0)
            {
                gridCollList.DataSource = _dt;
                gridCollList.DataBind();
                ViewState.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.CollectionInfo, _dt);
                btnApprove.Visible = true;
                btnCancel.Visible = true;
                //RowCounter.Text = gridCollList.Rows.Count.ToString();
                //programmaticModalPopup.Show();
            }
            else
            {
                gridCollList.DataSource = null;
                gridCollList.DataBind();
                ShowPopup(true, true, "No Data Found.");
                btnApprove.Visible = false;
                btnCancel.Visible = false;
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


        decimal TotalUnitPrice;
        public decimal GetUnitPrice(decimal Price)
        {
            TotalUnitPrice += Price;
            return Price;
        }

        public decimal GetTotal()
        {
            return TotalUnitPrice;
        }          

        protected void btnApprove_Click(object sender, ImageClickEventArgs e)
        {
            string collectioid = "";
            int rowIndex = -1;
            if (gridCollList.Rows.Count == 0)
                ShowPopup(false, false, "No Data Found.");
            else
            {
                try
                {
                    foreach (GridViewRow row in gridCollList.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("cboApprove");

                        if (cb.Checked)
                        {
                            rowIndex = row.RowIndex;
                            collectioid = gridCollList.DataKeys[row.RowIndex].Value.ToString();
                            int clearedBy = int.Parse((String)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserId]);
                            string clearingDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");// DateTime.Now.ToString("yyyy-MM-dd");
                            _bilCollectionManager.ClearCollection(collectioid, clearedBy, clearingDate);
                        }
                    }
                    if (rowIndex == -1)
                        ShowPopup(true, true, "No Item Selected.");
                    else
                        ShowPopup(true, false, "");
                }
                catch (Exception exception)
                {
                    ShowPopup(false, false, "");
                }
            }
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            if (btnApprove.Visible == true)
            {
                GetAllNonCashBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), false, MtbBillCollection.Global.Definitions.CollectionStatus.Value.Approved, txtFromDate.Text, txtToDate.Text);
            }
            else
            {
                gridCollList.DataSource = null;
                gridCollList.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            string collectioid = "";
            int rowIndex = -1;
            if (gridCollList.Rows.Count == 0)
                ShowPopup(false, false, "No Data Found.");
            else
            {
                try
                {
                    foreach (GridViewRow row in gridCollList.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("cboApprove");

                        if (cb.Checked)
                        {
                            rowIndex = row.RowIndex;
                            collectioid = gridCollList.DataKeys[row.RowIndex].Value.ToString();
                            int clearedBy = int.Parse((String)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserId]);
                            string clearingDate = DateTime.Now.ToString();// DateTime.Now.ToString("yyyy-MM-dd");
                            _bilCollectionManager.CancelCollection(collectioid, clearedBy, clearingDate);
                        }
                    }
                    if (rowIndex == -1)
                        ShowPopup(true, true, "No Item Selected.");
                    else
                        ShowPopup(true, false, "");
                }
                catch (Exception exception)
                {
                    ShowPopup(false, false, "");
                }
            }
        }
    }
}