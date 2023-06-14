using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Negocio;
using Dominio;
using System.Collections;
using System.Web.Services;
using System.Web.Script.Services;

namespace RestoApp
{
	public partial class Main1 : System.Web.UI.Page
	{
		private List<Mesa> mesas;
		
		protected void Page_Load(object sender, EventArgs e)
		{
			
			if (!IsPostBack)
			{
				CargarMesas();
				CargarNumeroDeMesasAlDesplegable();
				CargarMesasGuardadas();
			}
		}
		
		private void CargarMesas()
		{
			MesaNegocio mesaNegocio = new MesaNegocio();
			mesas = mesaNegocio.Listar();
		}

		//Ponemos el número de mesas de la base de datos en el dropdown
		private void CargarNumeroDeMesasAlDesplegable()
		{
			int numeroMesas = mesas.Count();
			//Mandamos el dato a main.js
			ClientScript.RegisterStartupScript(this.GetType(), "cantidadMesas", $"var cantidadMesas = '{numeroMesas}';", true);
		}

		//Cargamos las mesas Activas
		private void CargarMesasGuardadas()
		{
			//Buscamos mesas activas
			List<Mesa> mesasGuardadas = mesas.FindAll(m => m.Activo == true);

			//Guardamos número de mesa activas
			List<int> numeroMesasGuardas = new List<int>();
			numeroMesasGuardas = mesas.Select(m => m.Activo == true ? m.Numero : 0).ToList();

			// Convierte la lista en una cadena JSON
			var numeroMesasGuardasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(numeroMesasGuardas);

			//Mandamos el dato a main.js
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasGuardas", $"var numeroMesasGuardasJSON = '{numeroMesasGuardasJSON}';", true);

		}


		[WebMethod]
		public static void GuardarMesas(int[] array)
		{
			
			MesaNegocio mesaNegocio = new MesaNegocio();
			//Verificar cambios
			for(int i = 0; i < array.Length; i++)
			{
				int activo = array[i] == 0 ? 0 : 1;
				mesaNegocio.ActivarMesasPorNumero(i + 1, activo);
			}
			
		}

	}
}