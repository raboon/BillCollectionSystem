using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MtbBillCollection.Pages
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["IsLoggedIn"] = "False";
            Session["UserName"] = "";
            Session["FullName"] = "";
            Session["IsAdmin"] = "";

            Response.Redirect("../Default.aspx");
        }
    }
}