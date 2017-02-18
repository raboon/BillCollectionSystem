using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MtbBillCollectionLibrary.Security;
using System.Data;
using System.Text;
using System.Drawing;

namespace MtbBillCollection.Pages
{
    public partial class MarkForSpeacial : System.Web.UI.Page
    {
        BillCollectionManager _bilCollectionManager = new BillCollectionManager();
        AuthenticationManager _authenticationManger = new AuthenticationManager();
        String _collectionId = "",_clientid="-1";
       
        protected void Page_Load(object sender, EventArgs e)
        {   
            
            if ((Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] == null || Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn]) == false) ||
               (MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session["userTypeId"].ToString()), "Clearing")) == false)
            {
                Response.Redirect("/Default.aspx");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    gridCollList.Enabled = false;
                    gridCollList.Visible = false;
                    ClientList.DataSource = _bilCollectionManager.GetClientist();
                    ClientList.DataBind();

                    if (ClientList.Items.Count > 0)
                    {
                        ClientList.SelectedIndex = 0;
                        //lblClient.Text = ClientList.Text;
                        _clientid = ClientList.Items[0].Value;
                    }
                    ViewState.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.ClientId, _clientid);
                    _authenticationManger.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
                    cboBranch.DataSource = _authenticationManger.GetBranchList();
                    cboBranch.DataBind();

                    InstrumentTypeList.DataSource = _bilCollectionManager.GetInstrumentTypeist();
                    InstrumentTypeList.DataBind();
                    InstrumentTypeList.RepeatDirection = RepeatDirection.Horizontal;                    

                    ProducTypeList.DataSource = _bilCollectionManager.GetProductList(1);
                    ProducTypeList.DataBind();
                    ProducTypeList.RepeatDirection = RepeatDirection.Horizontal;

                    txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    txtConfirmation.Text = "";
                    
                }
                else
                {
                    gridCollList.Enabled = true;
                    gridCollList.Visible = true;
                    txtConfirmation.Text = "";
                }
            }
            
        }

        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (_bilCollectionManager.IsSpeacialEntryExist(txtCollectionId.Text) == false)
            {
                _bilCollectionManager = new BillCollectionManager();
                AuthenticationManager authenticationManger = new AuthenticationManager();
                EncryptionDecryptionManager encryptionDecriptionManager = new EncryptionDecryptionManager();
                authenticationManger.ConnectionString = MtbBillCollection.WebConfigManager.GetCollDBConnString();
                String userId = (String)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserId];
                string password = txtPassword.Text;
                password = encryptionDecriptionManager.ComputeHash(password, new byte[password.Length]);
                if (authenticationManger.IsActiveUser(userId, password) == true && MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session["userTypeId"].ToString()), "Clearing"))
                {
                    _collectionId = txtCollectionId.Text;
                    BillCollectionInfo billCollInfo = _bilCollectionManager.GetCollectionInformtion(_collectionId);
                    billCollInfo.ColectionId = int.Parse(txtCollectionId.Text);
                    _bilCollectionManager.MarkAsSpeacialEdit(billCollInfo.ColectionId, billCollInfo.BranchCode, userId, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), txtReason.Text);
                    billCollInfo.CollStatus = MtbBillCollection.Global.Definitions.CollectionStatus.Value.SpeacialEdit;
                    
                    ShowPopup(true,false,"");
                    //_bilCollectionManager.UpdateCollectionInformation(billCollInfo);
                }
            }
            else
            {
                ShowPopup(true, true, "Data can not save. Invalid User Id or Password or you don't have access.");
               
                //Show Messege
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

        
      

        protected void ButtonSearch_Click(object sender, ImageClickEventArgs e)
        {
            string collStatusSrchCond = "";
            string instTypeSrchCond = "";
            string productTypeSrchCond = "";
            int userTypeId=(int)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId];
            string branchCode = (cboBranchWise.Checked==true) ? cboBranch.SelectedValue : "";
            PrepareSrchCond(ref collStatusSrchCond,ref instTypeSrchCond,ref productTypeSrchCond);
            DataTable dt = _bilCollectionManager.GetCollectionList(Convert.ToInt32(ClientList.SelectedValue),
                txtFromDate.Text, txtToDate.Text, collStatusSrchCond, instTypeSrchCond, productTypeSrchCond, userTypeId, branchCode);
            //this.programmaticModalPopup.Show();
            dt.TableName = "Collection";
            if (dt.Rows.Count < 1)
            {
                gridCollList.DataSource = null;
                txtConfirmation.Text = "No data found.";
                //ShowPopup(true,true,"No data found");
            }
            else
            {

                gridCollList.DataSource = dt;
            }
            
            gridCollList.DataBind();
        }

        private void PrepareSrchCond(ref string collStatusSrchCond, ref string instTypeSrchCond, ref string productTypeSrchCond)
        {
            System.Text.StringBuilder srchItems = new StringBuilder("");

            if (cboClear.Checked==true)
            {                
                srchItems.Append(MtbBillCollection.Global.Definitions.CollectionStatus.Value.Cleared);
            }

            if (cboApprove.Checked == true)
            {
                if (srchItems.Length > 0)
                    srchItems.Append(",");
                srchItems.Append(MtbBillCollection.Global.Definitions.CollectionStatus.Value.Approved);
            }
            collStatusSrchCond = srchItems.ToString();


            srchItems = new StringBuilder("");

            for (int i = 0; i < InstrumentTypeList.Items.Count; i++)
            {
                if (InstrumentTypeList.Items[i].Selected)
                {
                    if (i > 0 && !srchItems.ToString().Equals(""))
                        srchItems.Append(",");

                    srchItems.Append(InstrumentTypeList.Items[i].Value);
                }
            }

            instTypeSrchCond = srchItems.ToString();

            srchItems = new StringBuilder("");

            for (int i = 0; i < ProducTypeList.Items.Count; i++)
            {
                if (ProducTypeList.Items[i].Selected)
                {
                    if (i > 0 && !srchItems.ToString().Equals(""))
                        srchItems.Append(",");

                    srchItems.Append(ProducTypeList.Items[i].Value);
                }
            }

            productTypeSrchCond = srchItems.ToString();
        }

        protected void cboBranchWise_CheckedChanged(object sender, EventArgs e)
        {
           cboBranch.Enabled=(cboBranchWise.Checked == true)?true:false;                
        }

        public void ShowPopup(bool IsSuccessfull, bool IsValidationMessege, string validationMessege)
        {
            if (IsSuccessfull == true)
            {
                
                lblHeade.ForeColor = Color.Green;
                txtConfirmation.ForeColor = Color.Green;
                lblHeade.Text = "Information.";
                if (IsValidationMessege == true)
                {
                    PopupMessage.Text = validationMessege;
                    txtConfirmation.Text = validationMessege;
                }
                else
                {
                    PopupMessage.Text = "Data saved successfully.";
                    txtConfirmation.Text = "Data saved successfully.";
                }
            }
            else
            {
                lblHeade.ForeColor = Color.Red;
                txtConfirmation.ForeColor = Color.Red;
                lblHeade.Text = "Error.";
                if (IsValidationMessege)
                {
                    PopupMessage.Text = validationMessege;
                    txtConfirmation.Text = validationMessege;
                }
                else
                {
                    PopupMessage.Text = "Data Canot Not Save due some error.\n Please contact system your admistrator.";
                    txtConfirmation.Text = "Data Canot Not Save due some error.\n Please contact system your admistrator."; ;
                }
            }
            //ModalPopupextender.Show();
        }

        protected void OkButton_Click(object sender, EventArgs e)
        {
            btnSubmit_Click(sender, null);
            if (gridCollList.Rows.Count >1)
                ButtonSearch_Click(sender,null);
            else
            {
                gridCollList.DataSource = null;
                gridCollList.DataBind();
            }
        }        

        protected void btnSpEdit_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridCollList.Rows)
            {
                ControlCollection controls = row.Cells[row.Cells.Count - 1].Controls;

                if ((LinkButton)controls[1] == (LinkButton)sender)
                {
                    txtCollectionId.Text = row.Cells[0].Text;
                    txtCollectionId.Columns=20;
                    txtReason.Columns = 20;
                    txtReason.Rows = 10;
                    txtPassword.Columns = 20;
                    PopupHeader.BackColor = Color.White;
                    MessegePopupExtender.Show();
                    break;
                }
                
            }
            
        }
    }
}