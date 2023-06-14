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
		public static List<Mesa> mesas;
		public Usuario usuario { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if(Session[Configuracion.Session.Usuario] != null)
				usuario = (Usuario)Session[Configuracion.Session.Usuario];
			
			// CONTENIDO GERENTE
			if (!IsPostBack && usuario?.Tipo.Descripcion == ColumnasDB.TipoUsuario.Gerente)
			{
				CargarMesas();
				CargarNumeroDeMesasAlDesplegable();
				CargarMesasGuardadas();
				lblTipoUsuario.Text = ColumnasDB.TipoUsuario.Admin;
			}

			//CONTENIDO MESERO
			if (!IsPostBack && usuario?.Tipo.Descripcion == ColumnasDB.TipoUsuario.Mesero)
			{
				lblTipoUsuario.Text = ColumnasDB.TipoUsuario.Mesero;
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

		//Obtenemos los datos desde Main.js
		[WebMethod]
		public static void GuardarMesas(int[] array)
		{
			
			MesaNegocio mesaNegocio = new MesaNegocio();
			for(int i = 0; i < array.Length; i++)
			{
				//Verificar cambios
				if (Main1.mesas[i].Activo != (array[i] == 1))
				mesaNegocio.ActivarMesasPorNumero(i + 1, array[i]);
			}
			
		}
	}
}