using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Globalization;

namespace MtbBillCollection
{
    public partial class BillCollection : System.Web.UI.Page
    {
        BillCollectionManager _bilCollectionManager = new BillCollectionManager();
        string _clientid = "-1";
        string _collectionId = "-1";
        bool isPrepaid;
        BillCollectionInfo _billCollectionInfo = new BillCollectionInfo();
        Page _previousPage = null;
        bool _saveMode = true;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] == null || Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn]) == false ||
                (MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId].ToString()), "BillCollection")) == false)
            {
                Response.Redirect("/Default.aspx");
            }

            if (!Page.IsPostBack)
            {
                ClientList.DataSource = _bilCollectionManager.GetClientist();
                ClientList.DataBind();

                if (Request.QueryString.Count > 0)
                {
                    _clientid = Server.HtmlDecode(Request.QueryString[MtbBillCollection.Global.Definitions.SessionVariable.Value.ClientId].ToString());
                    ClientList.SelectedValue = _clientid;
                    //lblClient.Text = ClientList.SelectedItem.Text;
                    ViewState.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.ClientId, _clientid);
                }
                else
                {
                    if (ClientList.Items.Count > 0)
                    {
                        ClientList.SelectedIndex = 0;
                        //lblClient.Text = ClientList.Text;
                        _clientid = ClientList.Items[0].Value;
                    }
                }

                InstrumentDate.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
                //lblCollectionDateValue.Text = DateTime.Now.Date.ToString("dd/MMM/yyyy");
                //Date.Text = DateTime.Now.ToString("dd-MMM-yyyy");
                if (_clientid != "-1")
                {
                    ProductList.DataSource = _bilCollectionManager.GetProductList(Convert.ToInt32(_clientid));
                    ProductList.DataBind();
                    CollectionTypeList.DataSource = _bilCollectionManager.GetCollectionTypeList(Convert.ToInt32(_clientid));
                    CollectionTypeList.DataBind();
                    TranssactionTypeList.DataSource = _bilCollectionManager.GetTransactionTypeList(Convert.ToInt32(_clientid), Convert.ToInt32(ProductList.Items[0].Value));
                    TranssactionTypeList.DataBind();

                }
                //else
                //{
                //    ProductList.DataSource = _bilCollectionManager.GetProductList(Convert.ToInt32(ClientList.Items[0].Value));
                //    ProductList.DataBind();
                //    CollectionTypeList.DataSource = _bilCollectionManager.GetCollectionTypeList(Convert.ToInt32(ClientList.Items[0].Value));
                //    CollectionTypeList.DataBind();
                //    TranssactionTypeList.DataSource = _bilCollectionManager.GetTransactionTypeList(Convert.ToInt32(ClientList.Items[0].Value), Convert.ToInt32(ProductList.Items[0].Value));
                //    TranssactionTypeList.DataBind();
                //    TranssactionTypeList.SelectedIndex = (TranssactionTypeList.Items.Count > 0) ? 0 : -1;
                //}

                InstrumentBankList.DataSource = _bilCollectionManager.GetBankList();
                InstrumentBankList.DataBind();


                ProductTypeSelectedIndexChanged();
                if (Request.QueryString.Count > 1)
                {
                    _collectionId = Server.HtmlDecode(Request.QueryString[MtbBillCollection.Global.Definitions.SessionVariable.Value.CollectionId].ToString());
                    _billCollectionInfo = _bilCollectionManager.GetCollectionInformtion(_collectionId);
                    ProductList.SelectedValue = _billCollectionInfo.ProductId.ToString();
                    ProductTypeSelectedIndexChanged();
                    CollectionTypeList.SelectedValue = _billCollectionInfo.CollectionTypeId.ToString();
                    CollectionFrom.Text = _billCollectionInfo.CollFrom;
                    TranssactionTypeList.SelectedValue = _billCollectionInfo.InstTypeId.ToString();
                    InstrumentNo.Text = _billCollectionInfo.InstNumber;
                    InstrumentDate.Text = _billCollectionInfo.InstDate;
                    string[] amnt=null;
                    amnt = _billCollectionInfo.CollAmount.ToString().Split('.');
                    if (amnt.Length > 0)
                    {
                        InstrumentAmount.Text = amnt[0];
                        if (amnt.Length == 2)
                        {
                            InstrumentAmount.Text += "."+amnt[1].Substring(0,2);
                        }
                    }
                    
                    InstrumentBankList.SelectedValue = _billCollectionInfo.InstBankCode.ToString();
                    txtRemarks.Text = _billCollectionInfo.Remarks;
                    //SaveButton.Text = "Update";
                    _previousPage = this.PreviousPage;
                    _saveMode = false;
                    ViewState.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.SaveMode, _saveMode);
                    ViewState.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.CollectionId, _collectionId);
                }
                else
                {
                    CollectionFrom.Focus();
                    initializeFields();
                }
                //TranssactionTypeList.Attributes.Add("onchange", "ShowData()");

            }
            else
            {
                ProductList.Enabled = true;
                TranssactionTypeList.Enabled = true;             
                
                InstrumentAmount.Enabled = true;
                InstrumentBankList.Enabled = true;
            }
            EnableOrDisableInstrumentFields();            
        }        

        protected void initializeFields()
        {
            CollectionFrom.Text = "";
            InstrumentDate.Text = "";
            InstrumentNo.Text = "";
            InstrumentAmount.Text = "";
            txtRemarks.Text = "";
            CollectionFrom.Focus();
            //lblClient.Text = ClientList.SelectedItem.Text;
            TranssactionTypeList.SelectedIndex = (TranssactionTypeList.Items.Count > 0) ? 0 : -1;
        }
       
        private void ProductTypeSelectedIndexChanged()
        {
            string pType;
            //lblStatus.Text = "";
            if (ProductList.SelectedItem.Text== MtbBillCollection.Global.Definitions.ProductType.Name.Prepaid)
            {
                isPrepaid = true;
                pType = MtbBillCollection.Global.Definitions.ProductType.Value.Prepaid.ToString();
                lblCollectionFromID.Text = "POS Code : ";
                InstrumentNo.Text = "";
                InstrumentNo.Enabled = false;
                InstrumentDate.Text = "";
                InstrumentDate.Enabled = false;
                InstrumentBankList.SelectedIndex = 0;
                InstrumentBankList.Enabled = false;
                CollectionTypeList.SelectedValue = Convert.ToString(MtbBillCollection.Global.Definitions.CollectionType.Value.Bill);
                CollectionTypeList.Enabled = false;
                ImgBntCalc.Enabled = false;
            }
            else
            {
                isPrepaid = false;
                pType = ProductList.SelectedValue;
                if (ProductList.SelectedItem.Text == MtbBillCollection.Global.Definitions.ProductType.Name.Fees)
                    lblCollectionFromID.Text = "POS Code : ";
                else
                    lblCollectionFromID.Text = "Mobile No : ";
                InstrumentNo.Enabled = true;
                InstrumentDate.Enabled = true;
                InstrumentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                InstrumentBankList.SelectedIndex = 0; //make it MTB by default
                InstrumentBankList.Enabled = true;
                CollectionTypeList.SelectedIndex = 0;
                CollectionTypeList.Enabled = true;
                ImgBntCalc.Enabled = true;
            }
            TranssactionTypeList.DataSource = _bilCollectionManager.GetTransactionTypeList(Convert.ToInt32(ClientList.SelectedValue), Int32.Parse("0"+pType));
            TranssactionTypeList.DataBind();
        }
       
        protected void ClientList_SelectedIndexChanged(object sender, EventArgs e)
        {            
            //lblClient.Text = ClientList.SelectedItem.Text;
            ProductList.DataSource = _bilCollectionManager.GetProductList(Convert.ToInt32(ClientList.SelectedValue));
            ProductList.DataBind();
            ProductList.SelectedIndex = (ProductList.Items.Count > 0) ? 0 : -1;
            CollectionTypeList.DataSource = _bilCollectionManager.GetCollectionTypeList(Convert.ToInt32("0" + ClientList.SelectedValue));
            CollectionTypeList.DataBind();
            TranssactionTypeList.DataSource = _bilCollectionManager.GetTransactionTypeList(Convert.ToInt32(ClientList.SelectedValue), Convert.ToInt32("0" + ProductList.SelectedValue.ToString()));
            TranssactionTypeList.DataBind();
            if (TranssactionTypeList.Items.Count > 0)
                TranssactionTypeList.SelectedIndex = 0;
            ProductTypeSelectedIndexChanged();
            EnableOrDisableInstrumentFields();
            //if (TranssactionTypeList.SelectedIndex == MtbBillCollection.Global.Definitions.InstType.Value.CASH)
            //{
            //    InstrumentDate.Enabled = false;
            //    InstrumentNo.Enabled = false;
            //}
            //else
            //{
            //    InstrumentDate.Enabled = true;
            //    InstrumentNo.Enabled = true;
            //}
            //InstrumentAmount.Enabled = true;
        }

        protected void ProductList_SelectedIndexChanged(object sender, EventArgs e)
        {           
            CollectionTypeList.DataSource = _bilCollectionManager.GetCollectionTypeList(Convert.ToInt32(ClientList.SelectedValue));
            CollectionTypeList.DataBind();
            TranssactionTypeList.DataSource = _bilCollectionManager.GetTransactionTypeList(Convert.ToInt32(ClientList.SelectedValue), Convert.ToInt32(ProductList.SelectedValue));
            TranssactionTypeList.DataBind();
            ProductTypeSelectedIndexChanged();
            EnableOrDisableInstrumentFields();
        }

        private void EnableOrDisableInstrumentFields()
        {
            
            if (TranssactionTypeList.SelectedItem.Text.ToUpper() != MtbBillCollection.Global.Definitions.InstType.Name.CASH)
            {
                InstrumentNo.Enabled = true;
                InstrumentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                InstrumentDate.Enabled = true;
                InstrumentBankList.Enabled = true;
                ImgBntCalc.Enabled = true;
            }
            else
            {
                InstrumentNo.Enabled = false;
                InstrumentDate.Text = "";
                InstrumentDate.Enabled = false;
                InstrumentBankList.Enabled = false;
                ImgBntCalc.Enabled = false;
            }            
        }       

        protected void TranssactionTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {            
            EnableOrDisableInstrumentFields();
        }

        protected void SaveButton_Click(object sender, ImageClickEventArgs e)
        {
            if (IsValidEntry() == true)
            {
                if (ViewState[MtbBillCollection.Global.Definitions.SessionVariable.Value.SaveMode] != null)
                    _saveMode = Convert.ToBoolean(ViewState[MtbBillCollection.Global.Definitions.SessionVariable.Value.SaveMode].ToString());
                BillCollectionInfo bilCollectioninfo = new BillCollectionInfo();
                bilCollectioninfo.ClientId = Convert.ToInt32(ClientList.SelectedValue) ; //get it from session
                bilCollectioninfo.CollDate = DateTime.Now.ToString();
                bilCollectioninfo.ProductId = Convert.ToInt32(ProductList.SelectedValue);                
               
                InstrumentDate.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
                 DateTime creationTime = DateTime.Now.Date;

                 if (!DateTime.TryParseExact(InstrumentDate.Text, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,DateTimeStyles.None,out creationTime))
                {
                    // Parse failed
                    creationTime = DateTime.Now.Date;
                }


                bilCollectioninfo.BranchCode = Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.BranchCode].ToString();
                bilCollectioninfo.CollectionTypeId = Convert.ToInt32(CollectionTypeList.SelectedValue);
                bilCollectioninfo.CollFrom = CollectionFrom.Text;
                bilCollectioninfo.InstTypeId = Convert.ToInt32(TranssactionTypeList.SelectedValue);
                bilCollectioninfo.InstNumber = InstrumentNo.Text;
                bilCollectioninfo.InstDate = creationTime.ToString("MM/dd/yyyy");
                bilCollectioninfo.CollAmount = Convert.ToDecimal(InstrumentAmount.Text);
                bilCollectioninfo.CollStatus = MtbBillCollection.Global.Definitions.CollectionStatus.Value.Received;

                if (bilCollectioninfo.InstTypeId != MtbBillCollection.Global.Definitions.InstType.Value.CASH)
                {
                    bilCollectioninfo.InstBankCode = Convert.ToInt32(InstrumentBankList.SelectedValue);
                }
                else
                    bilCollectioninfo.InstBankCode = 0;


                
                bilCollectioninfo.Remarks = txtRemarks.Text;
                
                
                
                try
                {
                    if (_saveMode == true)
                    {
                        bilCollectioninfo.InstRecvdBy = Convert.ToInt32(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserId].ToString());
                        _bilCollectionManager.SaveCollectionInformation(bilCollectioninfo);
                        ShowPopup(true, false, "");
                    }
                    else
                    {
                        bilCollectioninfo.ColectionId = Convert.ToInt32(ViewState[MtbBillCollection.Global.Definitions.SessionVariable.Value.CollectionId].ToString());
                        _bilCollectionManager.UpdateCollectionInformation(bilCollectioninfo);
                        ShowPopup(true, false, "");
                        Response.Redirect(Request.QueryString[MtbBillCollection.Global.Definitions.SessionVariable.Value.PreviousPage].ToString() + "?ClientId=" + ViewState[MtbBillCollection.Global.Definitions.SessionVariable.Value.ClientId].ToString());
                    }
                                        
                    initializeFields();                    
                }
                catch (Exception exception)
                {
                    ShowPopup(false,false,"");                    
                }
            }
        }

        private bool IsValidEntry()
        {
            if (InstrumentAmount.Text == "") {               
                ShowPopup(false,true,"please entry Amount.");
                return false;
            }
            else if (TranssactionTypeList.SelectedValue != Convert.ToString(MtbBillCollection.Global.Definitions.InstType.Value.CASH)) {
                if (InstrumentNo.Text == "") {                    
                    ShowPopup(false,true,"please entry Instrument No");
                    return false;
                }
                else if (InstrumentDate.Text == "") {                    
                    ShowPopup(false,true,"please entry Instrument Date");
                    return false;
                }
            }

            if (CollectionFrom.Text=="") {                
                if (ProductList.SelectedValue == Convert.ToString(MtbBillCollection.Global.Definitions.ProductType.Value.Prepaid)) {                    
                    ShowPopup(false,true,"please entry Pos Code");
                }
                else if (ProductList.SelectedValue == Convert.ToString(MtbBillCollection.Global.Definitions.ProductType.Value.Prepaid))
                {                                      
                    ShowPopup(false,true,"please entry Mob. No");
                }
                return false;
            }
            return true;
        }

        public void ShowPopup(bool IsSuccessfull, bool IsValidationMessege, string validationMessege)
        {
            if (IsSuccessfull == true)
            {
                PopupHeader.ForeColor = Color.Green;
                PopupHeader.Text = "Information.";
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
            this.programmaticModalPopup.Show();
        }


        public void DummyMethod()
        {
            //
        }

    }
}