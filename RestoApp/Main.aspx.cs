using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.Script.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Services;
using Negocio;
using Dominio;
using Opciones;

namespace RestoApp
{
	public partial class Main1 : System.Web.UI.Page
	{
		public Usuario usuario { get; set; }
		
		protected void Page_Load(object sender, EventArgs e)
		{

			if (!IsPostBack)
			{
				usuario = (Usuario)Session[Configuracion.Session.Usuario];
			}

		}
		
	}
}