using Helper;
using Opciones;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using System.Web.Services;

namespace RestoApp
{
	public partial class MesaHabilitar : System.Web.UI.Page
	{
		public static List<Mesa> mesas;
		public Usuario usuario { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
				usuario = (Usuario)Session[Configuracion.Session.Usuario];
			
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				CargarMesas();
				CargarNumeroDeMesasAlDesplegable();
				CargarMesasGuardadas();
			}
		}

		private void CargarMesas()
		{
			if (Helper.Session.GetMesas() != null)
			{
				//Session
				mesas = Helper.Session.GetMesas();
			}
			else
			{
				//DB
				MesaNegocio mesaNegocio = new MesaNegocio();
				mesas = mesaNegocio.Listar();
				Helper.Session.SetMesas(mesas);
			}
		}

		//Ponemos el número de mesas de la base de datos en el dropdown
		private void CargarNumeroDeMesasAlDesplegable()
		{
			int numeroMesas = Helper.Session.GetMesas().Count();
			
			//Mandamos el dato a main.js
			ClientScript.RegisterStartupScript(this.GetType(), "cantidadMesas", $"var cantidadMesas = '{numeroMesas}';", true);
		}

		//Cargamos las mesas Activas
		private void CargarMesasGuardadas()
		{
			//Buscamos mesas activas
			List<Mesa> mesasGuardadas = Helper.Session.GetMesas().FindAll(m => m.Activo == true);

			//Guardamos número de mesa activas
			List<int> numeroMesasGuardas = new List<int>();
			numeroMesasGuardas = Helper.Session.GetMesas().Select(m => m.Activo == true ? m.Numero : 0).ToList();

			// Convierte la lista en una cadena JSON
			var numeroMesasGuardasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(numeroMesasGuardas);

			//Mandamos el dato a main.js
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasGuardas", $"var numeroMesasGuardasJSON = '{numeroMesasGuardasJSON}';", true);

		}

		//Obtenemos los datos desde Main.js
		[WebMethod]
		public static void GuardarMesas(int[] array)
		{

			MesaNegocio mesaNegocio = new MesaNegocio();
			for (int i = 0; i < array.Length; i++)
			{
				//Verificar cambios
				if (mesas[i].Activo != (array[i] == 1))
				{
					//mesaNegocio.ActivarMesasPorNumero(i + 1, array[i]);
					mesaNegocio.ActivarMesasPorNumero(mesas[i].Numero, array[i]);

					//Session
					mesas[i].Activo = array[i] != 0;
					Helper.Session.SetMesas(mesas);
				}
			}

		}
	}
}