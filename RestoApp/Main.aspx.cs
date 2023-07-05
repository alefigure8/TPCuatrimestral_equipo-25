﻿using System.Runtime.Remoting.Messaging;
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
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.DynamicData;


namespace RestoApp
{
	public partial class Main1 : System.Web.UI.Page
	{
		//Public
		public Usuario usuario { get; set; }
		public MeseroPorDia meseroPorDia { get; set; }
		public int MesasActivas { get; set; }
        public int MesasAsignadas { get; set; }
        public int MeserosPresentes { get; set; }

		public string tipoUsuario;

		public List<CategoriaProducto> ListaCategoriasProducto;
 
        //Private
        private List<MeseroPorDia> meserosPorDiaNoAsignados = new List<MeseroPorDia>();
		private List<MeseroPorDia> meserosPorDiaAsignados = new List<MeseroPorDia>();
		private List<Mesa> mesas;
		private List<MesaPorDia> mesasPorDia;

		//Javascript atributos
		private string datosMesasJSON;
		private string mesasActivasJSON;
		private string numeroMesasJSON;

		protected void Page_Load(object sender, EventArgs e)
		{
			//Verificar que sea usuario
			if (AutentificacionUsuario.esUser(Helper.Session.GetUsuario()))
					usuario = Helper.Session.GetUsuario();

			// CONTENIDO GERENTE
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				tipoUsuario = Configuracion.Rol.Gerente;
				CargarMeseros();
				CargarDatosMesas();
				CargarServicios();
				CargarEstadoMesas();
				CargarPedido();
			}

			//Si es postback, recargamos funciones de script en Gerente
			if (IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				ScriptsDataGerente();
				string script = "obtenerDatosMesasGerente().then(({ datosMesas, numeroMesas }) => {renderMesaGerente(datosMesas, numeroMesas); })";
				ScriptManager.RegisterStartupScript(this, GetType(), "scriptMain", script, true);
			}


			//CONTENIDO MESERO
			if (!IsPostBack && AutentificacionUsuario.esMesero(usuario))
			{
				//Verificamos que si ya está en memoria el meseropordia
				if ((MeseroPorDia)Session[Configuracion.Session.MeseroPorDia] != null)
					meseroPorDia = (MeseroPorDia)Session[Configuracion.Session.MeseroPorDia];

				//Verificamos si hay un número de mesa guardado para hacer pedido
				if(Helper.Session.GetNumeroMesaPedido() != null)
					lbNumeroMesa.Text = "Numero de Mesa: " + Helper.Session.GetNumeroMesaPedido().ToString();

				tipoUsuario = Configuracion.Rol.Mesero;
				CargarMenuDisponible();
				CargarMeseroPorDia();
				CargarMesasAsignadas();
				CargarServicios();

			}

			//Si es postback, recargamos funciones de script en Mesero
			if(IsPostBack &&  AutentificacionUsuario.esMesero(usuario))
			{
				ScriptsDataMesero();
				string script = " obtenerDatosMesasMesero().then(({ numeroMesas }) => {renderMesaMesero(numeroMesas); });";
				ScriptManager.RegisterStartupScript(this, GetType(), "scriptMain", script, true);
			}

			ListarCategoriasProducto();
		}

		private void CargarServicios()
		{
			ServicioNegocio servicioNegocio = new ServicioNegocio();

			//Listamos servicios que no cerrarron ticket
			List<Servicio> servicios = servicioNegocio.Listar().FindAll( serv => serv.Cobrado == false);

			List<Dictionary<string, int>> numerosServicios = new List<Dictionary<string, int>>();

			foreach(Servicio item in servicios)
			{
				Dictionary<string, int> aux = new Dictionary<string, int>();
				aux.Add("servicio", item.Id);
				aux.Add("mesa", item.Mesa);
				numerosServicios.Add(aux);
			}

			//Guardamos lista de mesa y servicios en sesión
			//TODO Llevar a helper MEJORAR!!
			Session["Servicios"] = numerosServicios;
			Session["ServiciosDB"] = servicios;

			//Mandamos datos a JS
			ScriptDataServicios();
		}

		private void CargarDatosMesas()
		{
			{
				//Llamdados a DB
				MesaNegocio mesaNegocio = new MesaNegocio();
				mesas = mesaNegocio.Listar();
				mesasPorDia = mesaNegocio.ListarMesaPorDia();

				//Sessiones
				Helper.Session.SetMesas(mesas);
				Helper.Session.SetMesasAsignadas(mesasPorDia);

				//Guardamos cantidad de mesas activas para mostrar en ASPX
				MesasActivas = mesas.FindAll(m => m.Activo == true).Count();
					
				List<MeseroPorDia> meserosAsignados = Helper.Session.GetMeserosAsignados();

				//Guardamos dato de cada mesa en una lista de Objects
				List<object> datosMesas = new List<object>();

				foreach(MesaPorDia mesa in Helper.Session.GetMesasAsignadas())
				{
					//Guardamos datos en datosMesas de cuyas mesas el cierre sea Null
					if(mesa.Cierre == null)
					{

						datosMesas.Add(new { mesa = mesa.Mesa, mesero = mesa.Mesero, nombre = meserosAsignados.Find(el => el.IdMesero == mesa.Mesero)?.Nombres, apellido = meserosAsignados.Find(el => el.IdMesero == mesa.Mesero)?.Apellidos });
					}
				}

				//Buscamos las mesas activas
				List<Mesa> mesasActivasNumeros = mesas.FindAll(m => m.Activo == true);

				// Convierte la lista en una cadena JSON
				datosMesasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(datosMesas);
				mesasActivasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(mesasActivasNumeros);

				//Guardamos datos en session para JS
				Session["datosMesasJSON"] = datosMesasJSON;
				Session["mesasActivasJSON"] = mesasActivasJSON;
				
				//Guardamos en session los datos de las mesas (mesa, idMesero, nombre y apellido)
				Session["infoMesas"] = datosMesas;

				//Mandamos datos a JS
				ScriptsDataGerente();
			}
		}

		//Metodo que enviado datos del Gerente a script
		private void ScriptsDataGerente()
		{
			//Recuperamos datos por si es postback
			mesasActivasJSON = (string)Session["mesasActivasJSON"];
			datosMesasJSON = (string)Session["datosMesasJSON"];

			//Enviamos datos a JS
			ClientScript.RegisterStartupScript(this.GetType(), "datosMesasArray", $"var datosMesasArray = '{datosMesasJSON}';", true);
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasActivasArray", $"var numeroMesasActivasArray = '{mesasActivasJSON}';", true);
		}

		private void CargarEstadoMesas()
		{

			//TODO: LLAMADO A DB PARA OBTENER ESTADO DE MESAS
			//Tenemos en servicio la mesa y el idDelServicio para saber cuales están abierta
			//Hay que buscar según la mesa al mesero
			//List<Dictionary<string, int>> datosServicios = (List<Dictionary<string, int>>)Session["Servicios"];

			//TODO Crear un List<object> donde guardar todos los datos de la mesa
			List<object> datosMesa = (List<object>)Session["infoMesas"];
			List<Servicio> serviciosDB = (List<Servicio>)Session["ServiciosDB"];

			List<object> datosMesas = new List<object>();
			
			foreach (Servicio itemServicio in serviciosDB)
			{
				foreach(object itemMesa in datosMesa)
				{
					//Si el id de la mesa es igual al id de la mesa del servicio
					if ((int)itemMesa.GetType().GetProperty("mesa").GetValue(itemMesa) == itemServicio.Mesa)
					{
						//Guardamos datos de la mesa en datosMesas
						datosMesas.Add(new { mesa = itemServicio.Mesa, servicio = itemServicio.Id, nombre = itemMesa.GetType().GetProperty("nombre").GetValue(itemMesa), apellido = itemMesa.GetType().GetProperty("apellido").GetValue(itemMesa), apertura = itemServicio.Fecha + itemServicio.Apertura, cierre = itemServicio.Fecha + itemServicio.Cierre });
					}
				}
			}

			Console.WriteLine(datosMesas);

			var dataTable = new DataTable();
			dataTable.Columns.Add("Numero", typeof(int));
			dataTable.Columns.Add("Mesero", typeof(string));
			dataTable.Columns.Add("Apertura", typeof(DateTime));
			dataTable.Columns.Add("Cierre", typeof(DateTime));
			
			// Agregar filas al DataTable de forma individual
			foreach (object item in datosMesas)
			{
				dataTable.Rows.Add(
					(int)item.GetType().GetProperty("mesa").GetValue(item), 
					(string)item.GetType().GetProperty("nombre").GetValue(item) + " " + (string)item.GetType().GetProperty("apellido").GetValue(item), 
					(DateTime)item.GetType().GetProperty("apertura").GetValue(item),
					(DateTime?)item.GetType().GetProperty("cierre").GetValue(item)
				);
			}

			foreach (DataRow row in dataTable.Rows)
			{
				int numero = (int)row["Numero"];
				string mesero = (string)row["Mesero"];
				DateTime apertura = (DateTime)row["Apertura"];
				DateTime? cierre = row["Cierre"] == DBNull.Value ? null : (DateTime?)row["Cierre"];
			}

			// Enlazar el DataTable al DataGrid
			datagrid.DataSource = dataTable;
			datagrid.DataBind();
		}

		private void CargarPedido()
		{
			//TODO: LLAMADO A DB PARA OBTENER ESTADO DE PEDIDOS

			var dataTable = new DataTable();
			dataTable.Columns.Add("Mesa", typeof(int));
			dataTable.Columns.Add("PedidoComida", typeof(string));
			dataTable.Columns.Add("Apertura", typeof(DateTime));
			dataTable.Columns.Add("Cierre", typeof(DateTime));

			// Agregar filas al DataTable de forma individual
			dataTable.Rows.Add(1, "Hamburguesa", DateTime.Now, DateTime.Now);
			dataTable.Rows.Add(1, "Pizza", DateTime.Now, DateTime.Now);
			dataTable.Rows.Add(2, "Sushi", DateTime.Now, System.DBNull.Value);
			dataTable.Rows.Add(3, "Ensalada", DateTime.Now, DateTime.Now);
			dataTable.Rows.Add(5, "Pasta", DateTime.Now, DateTime.Now);
			dataTable.Rows.Add(6, "Tacos", DateTime.Now, System.DBNull.Value);

			foreach (DataRow row in dataTable.Rows)
			{
				int numero = (int)row["Mesa"];
				string mesero = (string)row["PedidoComida"];
				DateTime apertura = (DateTime)row["Apertura"];
				DateTime? cierre = row["Cierre"] == System.DBNull.Value ? null : (DateTime?)row["Cierre"];
			}

			// Enlazar el DataTable al DataGrid
			datagridPedidos.DataSource = dataTable;
			datagridPedidos.DataBind();
		}

		private void CargarMeseros()
		{
			MesaNegocio mesaNegocio = new MesaNegocio();

			//Lista de IDs de Meseros con mesas asignadas
			List<int> IdMeserosConMesasAbiertas = mesaNegocio.ListaIdMeserosActivosConMesasAbiertas();

			//Meseros Presentes
			List<MeseroPorDia> meseroPorDia = mesaNegocio.ListaMeseroPorDia();

			//Meseros Ausentes
			List<Usuario> meserosAusentes = mesaNegocio.ListaMeserosAusentes();
			
			//Meseros no asignados
			meserosPorDiaNoAsignados = meseroPorDia.Where(usuario => !IdMeserosConMesasAbiertas.Contains(usuario.Id)).ToList();

			//Meseros asignados
			meserosPorDiaAsignados = meseroPorDia.Where(usuario => IdMeserosConMesasAbiertas.Contains(usuario.Id)).ToList();

			//Colocar cantidad de mesas asignadas
			meserosPorDiaAsignados.ForEach(mesero => mesero.MesasAsignadas = IdMeserosConMesasAbiertas
			.FindAll(id => id == mesero.Id).Count);

			//Sessions
			Helper.Session.SetMeserosAsignados(meserosPorDiaAsignados);
			Helper.Session.SetMeserosNoAsignados(meserosPorDiaNoAsignados);
		
			//Cantidad de Mesas Asignadas
			foreach(var meseros in meserosPorDiaAsignados)
			{
				MesasAsignadas += meseros.MesasAsignadas;
			}

			//Meseros presentes
			MeserosPresentes = meseroPorDia.Count();

			//Render Meseros Presentes
			repeaterMeserosPresentes.DataSource = meseroPorDia;
			repeaterMeserosPresentes.DataBind();

			//Render Meseros Ausentes
			repeaterMeserosAusentes.DataSource = meserosAusentes;
			repeaterMeserosAusentes.DataBind();
		}

		
		// VISTA MESERO
		private void CargarMenuDisponible()
		{
            Session["ListaMenu"] = null;
            ProductoNegocio ProductoNegocio = new ProductoNegocio();
            Session.Add("ListaMenu", ProductoNegocio.ListarProductosDelDia());
			CargarBebidasDelDia();
			CargarPlatosDelDia();
        }




		private void CargarPlatosDelDia()
		{
			ListarCategoriasProducto();
			List<ProductoDelDia> ListaProductosDisponibles =
				((List<ProductoDelDia>)Session["ListaMenu"]).FindAll(x => x.Activo == true && x.Categoria == ListaCategoriasProducto.Find(y => y.Descripcion == "Platos").Id); 
            PlatosDelDia.DataSource = ListaProductosDisponibles;
            PlatosDelDia.DataBind();
        }

        private void CargarBebidasDelDia()
        {
            ListarCategoriasProducto();
            List<ProductoDelDia> ListaProductosDisponibles =
                ((List<ProductoDelDia>)Session["ListaMenu"]).FindAll(x => x.Activo == true && x.Categoria == ListaCategoriasProducto.Find(y => y.Descripcion == "Bebidas").Id);
            BebidasDelDia.DataSource = ListaProductosDisponibles;
            BebidasDelDia.DataBind();
        }

        private void CargarMeseroPorDia()
		{
			List<MeseroPorDia> meserosPorDia = new List<MeseroPorDia>();
			MesaNegocio mesaNegocio = new MesaNegocio();

			meserosPorDia = mesaNegocio.ListaMeseroPorDia();

			//Verificamos si el mesero está dado de alta y no de baja
			meseroPorDia = meserosPorDia.Find(mesero => mesero.IdMesero == usuario.Id && mesero.Salida == new TimeSpan(0,0,0));

			if(meseroPorDia != null)
			{
				// Session
				Helper.Session.SetMeseroPorDia(meseroPorDia);

				if (meseroPorDia.Id > 0)
				{
					btnMeseroAlta.Text = "Darse de Baja";
					btnMeseroAlta.CssClass = "btn btn-sm btn-light";
				}
			}
			else
			{
				btnMeseroAlta.Text = "Darse de Alta";
				btnMeseroAlta.CssClass = "btn btn-sm btn-warning";
			}
		}
	
		protected void btnMeseroAlta_Click(object sender, EventArgs e)
        {
			// Session
			usuario = Helper.Session.GetUsuario();
			meseroPorDia = Helper.Session.GetMeseroPorDia();

			//BTN
			Session["BotonID"] = btnMeseroAlta.ClientID;

			if (meseroPorDia == null)
			{
				//Darse de Alta
				try
				{
					meseroPorDia = new MeseroPorDia();
					meseroPorDia.IdMesero = usuario.Id;
					meseroPorDia.Nombres = usuario.Nombres;
					meseroPorDia.Apellidos = usuario.Apellidos;
					meseroPorDia.Fecha = DateTime.Now;
					meseroPorDia.Ingreso = DateTime.Now.TimeOfDay;

					MesaNegocio mesaNegocio = new MesaNegocio();
					meseroPorDia.Id = mesaNegocio.CrearMeseroPorDia(meseroPorDia);

				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}

				//Si se crea correctamente cambiamos el texto del botón
				if (meseroPorDia.Id > 0)
				{
					btnMeseroAlta.Text = "Darse de Baja";
					btnMeseroAlta.CssClass = "btn btn-sm btn-light";
					Helper.Session.SetMeseroPorDia(meseroPorDia);
				}
			}
			else
			{
				//Darse de baja
				try
				{
					MesaNegocio mesaNegocio = new MesaNegocio();

					List<MesaPorDia> mesasAsignadas = Helper.Session.GetMesasAsignadas();

					//Si tiene mesas asigndas, se cierran
					//TODO: si tiene pedidos abierto, primero los tiene que cerrar
					if (mesasAsignadas != null)
					{
						foreach(MesaPorDia item in mesasAsignadas)
						{

							mesaNegocio.ModificarMesaPorDia(item.Id, item.Mesa, (int)item.Mesero);
						}
					}

					bool esBaja = mesaNegocio.ModificarMeseroPorDia(meseroPorDia.Id);

					//Si la baja es correcta cambiamos el texto del botón y borramos sessión
					if(esBaja)
					{
						btnMeseroAlta.Text = "Darse de Alta";
						btnMeseroAlta.CssClass = "btn btn-sm btn-warning";
						meseroPorDia = null;
						Helper.Session.SetMeseroPorDia(null);
						
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		private void CargarMesasAsignadas()
		{
			// Session
			usuario = Helper.Session.GetUsuario();
			meseroPorDia = Helper.Session.GetMeseroPorDia();

			if (meseroPorDia != null)
			{
				MesaNegocio mesaNegocio = new MesaNegocio();
				List<MesaPorDia> mesasAsignadas = mesaNegocio.ListarMesaPorDia()
					.FindAll(x => x.Mesero == meseroPorDia.IdMesero && x.Cierre == null)
					.OrderBy(x => x.Mesa).ToList();

				Helper.Session.SetMesasAsignadas(mesasAsignadas);

				if (mesasAsignadas != null)
				{
					//Creamos Objetos con los números de mesas asignadas
					List<object> datosMesas = new List<object>();

					foreach (MesaPorDia mesa in Helper.Session.GetMesasAsignadas())
					{
						//Guardamos datos en datosMesas de cuyas mesas el cierre sea Null
						if (mesa.Cierre == null)
						{

							datosMesas.Add(new { mesa = mesa.Mesa });
						}
					}

					//Serializamos datos
					numeroMesasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(datosMesas);

					//Guardamos datos en session para recuperar en caso de postback
					Session["numeroMesaJSON"] = numeroMesasJSON;

					//Mandamos datos a script
					ScriptsDataMesero();

					if (mesasAsignadas.Count > 0)
					{
						lbSinMesasAsignadas.Text = String.Empty;
						return;
					}
						
				}
			}

			lbSinMesasAsignadas.Text = "No cuenta con mesas asignadas. Comuníquese con el Gerente.";
		}

		//Enviamos datos a script para recuperar en un postback
		private void ScriptsDataMesero()
		{
			//Recuperamos datos de session
			numeroMesasJSON = (string)Session["numeroMesaJSON"];

			//Mandamos a script de javascript
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasArray", $"var numeroMesasArray = '{numeroMesasJSON}';", true);
		}

		//Enviamos datos a script para recuperar en un postback
		private void ScriptDataServicios()
		{
			//Recuperamos datos de session
			List<Dictionary<string, int>> servicios = (List<Dictionary<string, int>>)Session["Servicios"];
	
			string seviciosJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(servicios);
			
			//Mandamos a script de javascript
			ClientScript.RegisterStartupScript(this.GetType(), "seviciosJSON", $"var seviciosJSON = '{seviciosJSON}';", true);
		}
		
		
		//WEBMETHOD

		//Obtenemos número de mesa y le abrimos un servicio
		[WebMethod]
		public static string AbrirServicio(List<Dictionary<string, int>> data)
		{
			ServicioNegocio servicioNegocio = new ServicioNegocio();
			
			string response = String.Empty;

			foreach(var diccionario in data)
			{
				var numeroMesa = diccionario["mesa"];
				
				int idServicio = servicioNegocio.AbrirServicio(numeroMesa);

				//Guardamos en session el idServicio y idMesa
				if(idServicio > 0)
				{
					//TODO Llevar session a helper
					if ((List<Dictionary<string, int>>)HttpContext.Current.Session["Servicios"] != null)
					{
						//TODO Llevar session a helper
						List<Dictionary<string, int>> numerosServicios = (List<Dictionary<string, int>>)HttpContext.Current.Session["Servicios"];
						Dictionary<string, int> aux = new Dictionary<string, int>();
						aux.Add("servicio", idServicio);
						aux.Add("mesa", numeroMesa);
						numerosServicios.Add(aux);

						//TODO Llevar a helper
						HttpContext.Current.Session["Servicios"] = numerosServicios;
					}
					else
					{
						List<Dictionary<string, int>> numerosServicios = new List<Dictionary<string, int>>();
						Dictionary<string, int> aux = new Dictionary<string, int>();
						aux.Add("servicio", idServicio);
						aux.Add("mesa", numeroMesa);
						numerosServicios.Add(aux);
						
						//TODO Llevar a helper
						HttpContext.Current.Session["Servicios"] = numerosServicios;
					}
				}
				
				response = numeroMesa.ToString();
			}

			return response;
		}

		//Guardamos número de mesa en pedido
		[WebMethod]
		public static string AbrirPedido(List<Dictionary<string, int>> data)
		{

			string response = String.Empty;
			foreach (var diccionario in data)
			{
				var numeroMesa = diccionario["mesa"];
				Helper.Session.SetNumeroMesaPedido(numeroMesa);
				response = numeroMesa.ToString();
			}

			return response;
		}

        protected void ListarCategoriasProducto()
        {
			CategoriaProductoNegocio CPNAux = new CategoriaProductoNegocio();
			ListaCategoriasProducto = CPNAux.Listar();
        }

        protected void AgregarAPedido_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            RepeaterItem repeaterItem = button.NamingContainer as RepeaterItem;
            TextBox tbCantidad = repeaterItem.FindControl("tbCantidad") as TextBox;
            Button btnCancelar = repeaterItem.FindControl("btnCancelarAgregarA") as Button;


            if (button.Text.ToLower() == "+")
            {
                tbCantidad.Visible = true;
                button.Text = "✔";
				btnCancelar.Visible = true;
            }
            else
            {
                tbCantidad.Visible = false;
                button.Text = "+";
                
                btnCancelar.Visible = false;

            }
        }

        protected void BtnCancelarAgregarA_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            RepeaterItem repeaterItem = button.NamingContainer as RepeaterItem;
            TextBox tbCantidad = repeaterItem.FindControl("tbCantidad") as TextBox;
            Button btnCancelar = repeaterItem.FindControl("btnCancelarAgregarA") as Button;
            tbCantidad.Visible = false;
            btnCancelar.Visible = false;
        }
    }
}