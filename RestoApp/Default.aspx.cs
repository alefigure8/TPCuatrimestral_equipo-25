using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestoApp
{
	public partial class Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session["Error"] != null)
			{
				lbl_error.Text = (string)Session["Error"];
			}
		}

		protected void EnviarDatos_Click(object sender, EventArgs e)
		{
			
			Response.Redirect("Main.aspx", false);
		}

		private bool ValidarDatos(string mail, string pass)
		{
			if (!String.IsNullOrEmpty(mail) && !String.IsNullOrEmpty(pass))
			{
				try
				{
					MailAddress m = new MailAddress(mail);

					return true;
				}
				catch (FormatException)
				{
					return false;
				}
			}

			return false;
		}
	}
}