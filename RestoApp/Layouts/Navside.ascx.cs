using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Helper;
using Opciones;

namespace RestoApp.Layouts
{
	public partial class Navside : System.Web.UI.UserControl
	{
		public Usuario usuario { get; set; }
		public MeseroPorDia meseroPorDia {get; set;}
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
			{
				Response.Redirect(Configuracion.Pagina.Login, false);
				Session[Configuracion.Session.Error] = Mensajes.Autentificacion.Error;
			}
			else
			{
				//Verificamos que si ya está en memoria el meseropordia
				if ((MeseroPorDia)Session[Configuracion.Session.MeseroPorDia] != null)
					meseroPorDia = (MeseroPorDia)Session[Configuracion.Session.MeseroPorDia];

				if (!IsPostBack)
				{
					usuario = (Usuario)Session[Configuracion.Session.Usuario];
				}
			}
		}

		protected void salir_click(object sender, EventArgs e)
		{
			Session[Configuracion.Session.Usuario] = null;
			Session[Configuracion.Session.Error] = null;
			Response.Redirect(Configuracion.Pagina.Login, false);
		}
	}
}