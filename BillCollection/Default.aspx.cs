using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MtbBillCollectionLibrary.Security;

namespace MtbBillCollection
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["isLoggedIn"] == null || Convert.ToBoolean(Session["isLoggedIn"]) == false)
            {
                Response.Redirect("pages/Login.aspx");
            }
        }
    }
}