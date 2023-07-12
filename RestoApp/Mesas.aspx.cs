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
using System.Collections;
using System.Diagnostics;
using System.Web.Services.Description;

namespace RestoApp
{
	public partial class Mesas : System.Web.UI.Page
	{
		public List<Mesa> mesas;
		public List<Usuario> meseros = new List<Usuario>();
		public List<MeseroPorDia> meserosPorDiaNoAsignados = new List<MeseroPorDia>();
		public List<MeseroPorDia> meserosPorDiaAsignados = new List<MeseroPorDia>();
		public List<MesaPorDia> mesasPorDia;
		public Usuario usuario { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			if ( AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
				usuario = (Usuario)Session[Configuracion.Session.Usuario];

			// CONTENIDO GERENTE
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				try
				{
					CargarMesas();
					CargarMesasGuardadas();
					CargarMesasPorDiaGuardadas();
					CargarMeseros();

				}
				catch(Exception error)
				{
					UIMostrarAlerta(error.Message);
				}
			}
		}

		//Modal Alerta
		private void UIMostrarAlerta(string mensaje, string tipoMensaje = "error")
		{
			string scriptModal = $"alertaModal(\"{mensaje}\", \"{tipoMensaje}\");";
			ScriptManager.RegisterStartupScript(this, GetType(), "ScriptMesa", scriptModal, true);

			//Borramos mensaje del modal
			Helper.Session.SetMensajeModal(null);
		}

		private void CargarMesas()
		{
			if(Helper.Session.GetMesas() != null)
			{
				//Session
				mesas = Helper.Session.GetMesas();
			}
			else
			{
				//DB
				MesaNegocio mesaNegocio = new MesaNegocio();
				mesas = mesaNegocio.Listar();
			}
		}

		//Cargamos las mesas Activas
		private void CargarMesasGuardadas()
		{
			//Guardamos número de mesa activas
			List<int> numeroMesasGuardas = new List<int>();
			numeroMesasGuardas = mesas.Select(m => m.Activo == true ? m.Numero : 0).ToList();

			// Convierte la lista en una cadena JSON
			var numeroMesasGuardasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(numeroMesasGuardas);

			//Mandamos el dato a main.js
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasGuardas", $"var numeroMesasGuardasJSON = '{numeroMesasGuardasJSON}';", true);
		}

		private void CargarMesasPorDiaGuardadas()
		{
			List<MesaPorDia> mesasPorDia = new List<MesaPorDia>();

			if(Helper.Session.GetMesasAsignadas() != null )
			{
				//Session
				mesasPorDia = Helper.Session.GetMesasAsignadas().FindAll(mesa => mesa.Cierre == null);
			}
			else
			{
				//DB
				MesaNegocio mesaNegocio = new MesaNegocio();
				mesasPorDia = mesaNegocio.ListarMesaPorDia().FindAll(mesa => mesa.Cierre == null);
				Helper.Session.SetMesasAsignadas(mesasPorDia);
			}
	
			//Creamos objeto con mesa y mesero
			List<object> objetos = new List<object>();

			foreach (var item in mesasPorDia)
			{
				//Crear objeto para javascript
				objetos.Add(new { mesa = item.Mesa, mesero = item.Mesero, idmeseropordia = item.IDMeseroPorDia, abierta = true });
			}

			// Convierte la lista en una cadena JSON
			var numeroMesasPorDiaJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(objetos);
			
			//Mandamos el dato a main.js
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasPorDia", $"var numeroMesasPorDiaJSON = '{numeroMesasPorDiaJSON}';", true);
		}
		
		private void CargarMeseros()
		{
			//DB
			MesaNegocio mesaNegocio = new MesaNegocio();
			List<int> IdMeserosConMesasAbiertas;

			if (Helper.Session.GetMesasAsignadas() != null )
			{
				//Session
				mesasPorDia = Helper.Session.GetMesasAsignadas();
				IdMeserosConMesasAbiertas = mesasPorDia.Select(mesa => (int)mesa?.IDMeseroPorDia).ToList();
			}
			else
			{
				//DB
				IdMeserosConMesasAbiertas = mesaNegocio.ListaIdMeserosActivosConMesasAbiertas();
			}

			if(Helper.Session.GetMeserosAsignados() != null && Helper.Session.GetMeserosNoAsignados() != null)
			{
				//Session
				meserosPorDiaAsignados = Helper.Session.GetMeserosAsignados();
				meserosPorDiaNoAsignados = Helper.Session.GetMeserosNoAsignados();
			}
			else
			{
				//DB
				List<MeseroPorDia> meseroPorDia = mesaNegocio.ListaMeseroPorDia();
			
				//Meseros no asignados
				meserosPorDiaNoAsignados = meseroPorDia.Where(usuario => !IdMeserosConMesasAbiertas.Contains(usuario.Id)).ToList();

				//Meseros asignados
				meserosPorDiaAsignados = meseroPorDia.Where(usuario => IdMeserosConMesasAbiertas.Contains(usuario.Id)).ToList();

				//Colocar cantidad de mesas asignadas
				meserosPorDiaAsignados.ForEach(mesero => mesero.MesasAsignadas = IdMeserosConMesasAbiertas.FindAll(id => id == mesero.Id).Count);

				Helper.Session.SetMeserosAsignados(meserosPorDiaAsignados);
				Helper.Session.SetMeserosNoAsignados(meserosPorDiaNoAsignados);
			}


			//Repeater Meseros No Asignados
			repeaterMeserosNoAsignados.DataSource = meserosPorDiaNoAsignados;
			repeaterMeserosNoAsignados.DataBind();

			//Repeater Meseros Asignados
			repeaterMeserosAsignados.DataSource = meserosPorDiaAsignados;
			repeaterMeserosAsignados.DataBind();
		}

		//Obtenemos los datos desde Main.js
		[WebMethod]
		public static void GuardarMesas(Dictionary<string, int>[] array)
		{
			//DB
			MesaNegocio mesaNegocio = new MesaNegocio();
			List<MesaPorDia> mesasPorDiaAbierta = new List<MesaPorDia>();

			if (Helper.Session.GetMesasAsignadas() != null)
			{
				//Session
				mesasPorDiaAbierta = Helper.Session.GetMesasAsignadas();
			}
			else
			{
				//DB
				mesasPorDiaAbierta = mesaNegocio.ListarMesaPorDia().FindAll(mesa => mesa.Cierre == null);
			}
			
			foreach (var diccionario in array)
			{

				var numeroMesa = diccionario["mesa"];
				var numeroMesero = diccionario["mesero"];
				var numeromeseropordia = diccionario["idmeseropordia"];
				var estaAbierta = diccionario["abierta"];

				//Abrimos mesa
				if (!mesasPorDiaAbierta.Exists(el => el.Mesa == numeroMesa))
				{
					mesaNegocio.CrearMesaPorDia(numeroMesero, numeroMesa, numeromeseropordia);
				}

				//Cerramos mesa
				if (mesasPorDiaAbierta.Exists(el => el.Mesa == numeroMesa && el.Mesero == numeroMesero && estaAbierta == 0))
				{
					mesaNegocio.ModificarMesaPorDia(mesasPorDiaAbierta.Find(el => el.Mesa == numeroMesa).Id, numeroMesa, numeroMesero);
				}

			}
		}
	}
}