using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using System.Drawing;

namespace MtbBillCollection.Pages
{
    public partial class Collections : System.Web.UI.Page
    {
        private int gData =0;

        private BillCollectionManager _bilCollectionManager = new BillCollectionManager();
        private ReportDocument _crystalReport;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn] == null || Convert.ToBoolean(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.IsLoggedIn]) == false ||
               (MtbBillCollection.Utility.Screen.IsUserPermitedToAccessScreen(Convert.ToInt32(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId].ToString()), "ViewCollections")) == false)
            {
                Response.Redirect("/Default.aspx");
            } 


            if (!IsPostBack)
            {
                gData = 100;

                txtFromDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd-MM-yyyy");


                ClientList.DataSource = _bilCollectionManager.GetClientist();
                ClientList.DataBind();

                InstrumentTypeList.DataSource = _bilCollectionManager.GetInstrumentTypeist();
                InstrumentTypeList.DataBind();
                InstrumentTypeList.RepeatDirection = RepeatDirection.Horizontal;
                

                CollectionStatusType.DataSource = _bilCollectionManager.GetCollectionStatusType();
                CollectionStatusType.DataBind();
                CollectionStatusType.RepeatDirection = RepeatDirection.Horizontal;

                ProducTypeList.DataSource = _bilCollectionManager.GetProductList(1);
                ProducTypeList.DataBind();
                ProducTypeList.RepeatDirection = RepeatDirection.Horizontal;

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            }
            else
            {
                DataTable rptTable = (DataTable)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.CollectionReport];
               
                if (rptTable != null)
                {
                    rptTable.TableName = "CollectionStatement";
                    _crystalReport = new ReportDocument();
                    _crystalReport.Load(Server.MapPath("~/Reports/rptCollectionStatment.rpt"));
                     _crystalReport.SetDataSource(rptTable);
                    CrystalReportViewer1.ReportSource = _crystalReport;
                }
            }
        }

        protected void ButtonSearch_Click(object sender, ImageClickEventArgs e)
        {
            string collStatusSrchCond = "";
            string instTypeSrchCond = "";
            string productTypeSrchCond = "";

            PrepareSrchCond(ref collStatusSrchCond, ref instTypeSrchCond, ref productTypeSrchCond);

            _crystalReport = new ReportDocument();
           
            _crystalReport.Load(Server.MapPath("~/Reports/rptCollectionStatment.rpt"));
                        int userTypeId = int.Parse(Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.UserTypeId].ToString());
            string branchCode = (String)Session[MtbBillCollection.Global.Definitions.SessionVariable.Value.BranchCode];
            DataTable rptTable = _bilCollectionManager.GetCollectionList(Convert.ToInt32(ClientList.SelectedValue),
                txtFromDate.Text, txtToDate.Text, collStatusSrchCond, instTypeSrchCond, productTypeSrchCond, userTypeId, branchCode);
            //this.programmaticModalPopup.Show();
            rptTable.TableName = "CollectionStatement";

            if (rptTable.Rows.Count == 0)
            {
                ShowPopup(true, true, "No data available.");
                CrystalReportViewer1.ReportSource = null;

                CrystalReportViewer1.RefreshReport();
                
            }
            else
            {
                _crystalReport.SetDataSource(rptTable);
                Session.Add(MtbBillCollection.Global.Definitions.SessionVariable.Value.CollectionReport, rptTable);

                CrystalReportViewer1.ReportSource = _crystalReport;
                
                CrystalReportViewer1.RefreshReport();
            }
           
        }


        private void PrepareSrchCond(ref string collStatusSrchCond, ref string instTypeSrchCond, ref string productTypeSrchCond)
        {
            StringBuilder srchItems = new StringBuilder("") ;

            for (int i = 0; i < CollectionStatusType.Items.Count; i++)
            {
                if (CollectionStatusType.Items[i].Selected)
                {
                    if (i > 0 && !srchItems.ToString().Equals(""))
                        srchItems.Append(",");
                    
                    srchItems.Append(CollectionStatusType.Items[i].Value);
                }
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


        protected void gridCollList_DataBound(object sender, EventArgs e)
        {
            // Get the header row.
            //GridViewRow headerRow = gridCollList.HeaderRow;

            //if (headerRow != null)
            //{
            //    GridViewRow footerRow = gridCollList.FooterRow;
            //    headerRow.ForeColor = System.Drawing.Color.Green;
            //    footerRow.ForeColor = System.Drawing.Color.Green;
            //    // Display the sort order in the footer row.
            //    footerRow.Cells[6].Text = "Total Collection:";
            //}
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