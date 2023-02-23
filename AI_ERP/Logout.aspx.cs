using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using AI_ERP.Application_Libs;

namespace AI_ERP
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            Response.Redirect(ResolveUrl(AI_ERP.Application_Libs.Routing.URL.LOGIN.ROUTE));
        }
    }
}