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
using Helper;

namespace RestoApp
{
	public partial class Main1 : System.Web.UI.Page
	{
		public Usuario usuario { get; set; }
        public int MesasActivas { get; set; }
        public int MesasAsignadas { get; set; }
		private List<Mesa> mesas;
		
        protected void Page_Load(object sender, EventArgs e)
		{
			//Verificar que sea usuario
			if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
				usuario = (Usuario)Session[Configuracion.Session.Usuario];

			// CONTENIDO GERENTE
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				CargarDatosMesas();
			}

			//CONTENIDO MESERO
			if (!IsPostBack && AutentificacionUsuario.esMesero(usuario))
			{

			}
		}

		private void CargarDatosMesas()
		{
			{
				MesaNegocio mesaNegocio = new MesaNegocio();
				mesas = mesaNegocio.Listar();
				MesasActivas = mesas.FindAll(m => m.Activo == true).Count();
				MesasAsignadas = 0; //TODO DB DE MESA POR DIA
				//MesasAsignadas = mesas.FindAll(m => m.Activo == true && m.Mesero != null).Count();
			}
		}


	}
}