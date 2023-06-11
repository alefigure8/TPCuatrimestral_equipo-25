using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestoApp
{
	public partial class Main1 : System.Web.UI.Page
	{
	
		protected void Page_Load(object sender, EventArgs e)
		{
			

		}
		protected void salir_click(object sender, EventArgs e)
		{
			Session["usuario"] = null;
			Session["error"] = null;
			Response.Redirect("Default.aspx", false);
		}
	}
}