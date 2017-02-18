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
    public partial class AuthorizeBillCollection : System.Web.UI.Page
    {
        private BillCollectionManager _bilCollectionManager = new BillCollectionManager();
        private DataTable _dt = new DataTable();

        public bool CompareDateValid
        {
            get { return (Convert.ToDateTime(txtFromDate.Text)>Convert.ToDateTime(txtToDate.Text))?false:true;}
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if ((Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] == null || Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn]) == false)||
                (MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session["userTypeId"].ToString()), "AuthorizeBillCollection"))==false)
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
                    GetAllBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), MtbBillCollection.Global.Definitions.CollectionStatus.Value.Received, txtFromDate.Text, txtToDate.Text);
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
            if (rdoNormal.Checked == true)
                GetAllBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue),MtbBillCollection.Global.Definitions.CollectionStatus.Value.Received,txtFromDate.Text,txtToDate.Text);
            else
                GetAllBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), MtbBillCollection.Global.Definitions.CollectionStatus.Value.SpeacialEdit, txtFromDate.Text, txtToDate.Text);
        }

        private void GetAllBillColeectionInfoByClientId(int clientId,int collStatus,string fromDate,string toDate)
        {
            gridCollList.AutoGenerateColumns = false;
            int userType = (int)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId];
            string branchCode=(String) Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.BranchCode];

            _dt = _bilCollectionManager.GetCollectionListByStatus(clientId, collStatus, txtFromDate.Text, txtToDate.Text, userType, branchCode);
        
            if (_dt.Rows.Count > 0)
            {                
                ViewState.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.CollectionInfo, _dt);
                gridCollList.DataSource = _dt;
                gridCollList.DataBind();
                //RowCounter.Text = gridCollList.Rows.Count.ToString();
                //programmaticModalPopup.Show();
                btnApprove.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                btnApprove.Visible = false;
                btnCancel.Visible = false;
                gridCollList.DataSource = null;
                gridCollList.DataBind();
                ShowPopup(true, true, "No Data Found.");
            }
            
            
        }

        protected void gridCollList_DataBound(object sender, EventArgs e)
        {
            // Get the header row.
            GridViewRow headerRow = gridCollList.HeaderRow;

            if (headerRow != null)
            {
                headerRow.ForeColor = System.Drawing.Color.Gray;
            }
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

        protected void gridCollList_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //_dt = (DataTable)ViewState[MtbBillCollection.Global.Definitions.SessionVariable.Value.CollectionInfo];
            string queryString="ClientId=" + Server.HtmlEncode(ClientList.SelectedValue);
            queryString += "&CollectionId=" + Server.HtmlEncode(gridCollList.DataKeys[e.NewEditIndex].Value.ToString());
            queryString+="&PreviousPage=AuthorizeBillCollection.aspx";
          
            Response.Redirect("BillCollection.aspx?" + queryString);
        }
                
        protected void btnApprove_Click(object sender, ImageClickEventArgs e)
        {
            string collectioinId = "";
            int rowIndex = -1;
             try
                {
                    foreach (GridViewRow row in gridCollList.Rows)
                    {
                        CheckBox cb = (CheckBox)row.FindControl("cboApprove");

                        if (cb.Checked)
                        {
                            rowIndex = row.RowIndex;
                            collectioinId = gridCollList.DataKeys[row.RowIndex].Value.ToString();
                            int approveBy = int.Parse((String)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserId]);
                            String approveDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");//DateTime.Now.ToString("yyyy-MM-dd");
                            if (rdoNormal.Checked == true)
                            {
                                _bilCollectionManager.ApproveCollection(collectioinId, approveBy, approveDate);
                            }
                            else
                            {
                                _bilCollectionManager.ApproveSpeacialColection(collectioinId, approveBy, approveDate);
                            }

                        }
                    }
                    if (rowIndex==-1)
                        ShowPopup(true,true, "No Item Selected.");
                    else
                      ShowPopup(true, false, "");
                }
                catch (Exception exception)
                {
                    ShowPopup(false, false, "");
                }

             //if (rowIndex != -1)
             //   GetAllBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), 1, txtFromDate.Text, txtToDate.Text);
            
        }        

        protected void ClientList_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnApprove.Visible = false;
            btnCancel.Visible = false;
            gridCollList.DataSource = null;
            gridCollList.DataBind();

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

        public bool IsAnyItemSelected()
        {
            foreach (GridViewRow row in gridCollList.Rows)
            {
                CheckBox cb = (CheckBox)row.FindControl("cboApprove");

                if (cb.Checked)
                {
                    return true;
                }
            }
            return false;
        }

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            if (btnApprove.Visible == true)
            {
                if(rdoNormal.Checked==true)
                    GetAllBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), MtbBillCollection.Global.Definitions.CollectionStatus.Value.Received, txtFromDate.Text, txtToDate.Text);
                else
                    GetAllBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), MtbBillCollection.Global.Definitions.CollectionStatus.Value.SpeacialEdit, txtFromDate.Text, txtToDate.Text);
            }
            else
            {
                gridCollList.DataSource = null;
                gridCollList.DataBind();
            }
        }

        protected void btnCancel_Click1(object sender, ImageClickEventArgs e)
        {
            string collectioinId = "";
            int rowIndex = -1;
            try
            {
                foreach (GridViewRow row in gridCollList.Rows)
                {
                    CheckBox cb = (CheckBox)row.FindControl("cboApprove");

                    if (cb.Checked)
                    {
                        rowIndex = row.RowIndex;
                        collectioinId = gridCollList.DataKeys[row.RowIndex].Value.ToString();
                        int approveBy = int.Parse((String)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserId]);
                        String approveDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        _bilCollectionManager.CancelCollection(collectioinId, approveBy, approveDate);

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

        protected void rdoNormal_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoNormal.Checked == true)
                GetAllBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), MtbBillCollection.Global.Definitions.CollectionStatus.Value.Received, txtFromDate.Text, txtToDate.Text);
            else
                GetAllBillColeectionInfoByClientId(Convert.ToInt32(ClientList.SelectedValue), MtbBillCollection.Global.Definitions.CollectionStatus.Value.SpeacialEdit, txtFromDate.Text, txtToDate.Text);
        }
    }
}