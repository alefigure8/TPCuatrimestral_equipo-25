using Negocio;
using Opciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;
using Helper;

namespace RestoApp
{
	public partial class Mesas : System.Web.UI.Page
	{
		public static List<Mesa> mesas;
		public List<Usuario> meseros = new List<Usuario>();
		public List<MeseroPorDia> meserosPorDia = new List<MeseroPorDia>();
		public List<MesaPorDia> mesasPorDia;
		public Usuario usuario { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if ( AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
				usuario = (Usuario)Session[Configuracion.Session.Usuario];

			// CONTENIDO GERENTE
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				CargarMesas();
				CargarNumeroDeMesasAlDesplegable();
				CargarMesasGuardadas();
				CargarMeseros();
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

		private void CargarMeseros()
		{
			MesaNegocio mesaNegocio = new MesaNegocio();
			meserosPorDia = mesaNegocio.ListaMeseroPorDia();
			
			repeaterMeseros.DataSource = meserosPorDia;
			repeaterMeseros.DataBind();
		}

		//Obtenemos los datos desde Main.js
		[WebMethod]
		public static void GuardarMesas(int[] array, int idMesero)
		{
			MesaNegocio mesaNegocio = new MesaNegocio();
			List<MesaPorDia> mesasPorDia = new List<MesaPorDia>();
			mesasPorDia = mesaNegocio.ListarMesaPorDia();

			for (int i = 0; i < array.Length; i++)
			{
				Console.WriteLine("Mesero: " + idMesero.ToString());
				Console.WriteLine("Mesas: " + array[i].ToString());
				
				//Verificar cambios
				if (mesasPorDia.Exists(me => me.Mesa == i + 1) && array[i] == 0)
				{
					//Es un cambio de mesero a nada
					if (mesasPorDia.Find(me => me.Mesa == i + 1).Mesero == idMesero)
						mesaNegocio.ModificarMesaPorDia(mesasPorDia.Find(me => me.Mesa == i + 1).Id, i + 1, 0);
				}
				else if (mesasPorDia.Exists(me => me.Mesa == i + 1) && array[i] == 1)
				{
					//Es un cambio de nada a mesero
					mesaNegocio.ModificarMesaPorDia(mesasPorDia.Find(me => me.Mesa == i + 1).Id, i + 1, idMesero);
				}
				else
				{
					//Es una nueva mesa
					if (array[i] == 1)
					{
						mesaNegocio.CrearMesaPorDia(idMesero, i + 1);
					}
				}
					
			}

		}
	}
}