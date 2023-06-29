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
using System.Data;

namespace RestoApp
{
	public partial class Main1 : System.Web.UI.Page
	{
		public Usuario usuario { get; set; }
		public MeseroPorDia meseroPorDia { get; set; }
		public int MesasActivas { get; set; }
        public int MesasAsignadas { get; set; }
        public int MeserosPresentes { get; set; }
		private List<Mesa> mesas;
		private List<MeseroPorDia> meserosPorDia;
		public List<MeseroPorDia> meserosPorDiaNoAsignados = new List<MeseroPorDia>();
		public List<MeseroPorDia> meserosPorDiaAsignados = new List<MeseroPorDia>();

		protected void Page_Load(object sender, EventArgs e)
		{
			//Verificar que sea usuario
			if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
				usuario = (Usuario)Session[Configuracion.Session.Usuario];



			// CONTENIDO GERENTE
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				CargarDatosMesas();
				CargarEstadoMesas();
				CargarPedido();
				CargarMeseros();
			}

			//CONTENIDO MESERO
			if (!IsPostBack && AutentificacionUsuario.esMesero(usuario))
			{
				//Verificamos que si ya está en memoria el meseropordia
				if ((MeseroPorDia)Session[Configuracion.Session.MeseroPorDia] != null)
					meseroPorDia = (MeseroPorDia)Session[Configuracion.Session.MeseroPorDia];

				CargarMenuDisponible();
				CargarMeseroPorDia();
				CargarMesasAsignadas();
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

		private void CargarEstadoMesas()
		{

			//TODO: LLAMADO A DB PARA OBTENER ESTADO DE MESAS

			var dataTable = new DataTable();
			dataTable.Columns.Add("Numero", typeof(int));
			dataTable.Columns.Add("Mesero", typeof(string));
			dataTable.Columns.Add("Apertura", typeof(DateTime));
			dataTable.Columns.Add("Cierre", typeof(DateTime));

			// Agregar filas al DataTable de forma individual
			dataTable.Rows.Add(1, "Juan", DateTime.Now, System.DBNull.Value);
			dataTable.Rows.Add(2, "Maria", DateTime.Now, DateTime.Now);
			dataTable.Rows.Add(3, "Pedro", DateTime.Now, System.DBNull.Value);
			dataTable.Rows.Add(4, "Laura", DateTime.Now, DateTime.Now);
			dataTable.Rows.Add(5, "Carlos", DateTime.Now, System.DBNull.Value);
			dataTable.Rows.Add(6, "Ana", DateTime.Now, System.DBNull.Value);
			
			foreach (DataRow row in dataTable.Rows)
			{
				int numero = (int)row["Numero"];
				string mesero = (string)row["Mesero"];
				DateTime apertura = (DateTime)row["Apertura"];
				DateTime? cierre = row["Cierre"] == System.DBNull.Value ? null : (DateTime?)row["Cierre"];
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
			meserosPorDiaAsignados.ForEach(mesero => mesero.MesasAsignadas = IdMeserosConMesasAbiertas.FindAll(id => id == mesero.Id).Count);

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

			Session[Configuracion.Session.MeserosAsignados] = meserosPorDiaAsignados;
			Session[Configuracion.Session.MeserosNoAsignados] = meserosPorDiaNoAsignados;

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
			List<ProductoDelDia> ListaProductosDisponibles = ((List<ProductoDelDia>)Session["ListaMenu"]).FindAll(x => x.Activo == true && x.Categoria == 1);
            PlatosDelDia.DataSource = ListaProductosDisponibles;
            PlatosDelDia.DataBind();
        }

        private void CargarBebidasDelDia()
        {
            List<ProductoDelDia> ListaProductosDisponibles = ((List<ProductoDelDia>)Session["ListaMenu"]).FindAll(x => x.Activo == true && x.Categoria == 2);
            BebidasDelDia.DataSource = ListaProductosDisponibles;
            BebidasDelDia.DataBind();
        }




        private void CargarMeseroPorDia()
		{
			List<MeseroPorDia> meserosPorDia = new List<MeseroPorDia>();
			MesaNegocio mesaNegocio = new MesaNegocio();

			meserosPorDia = mesaNegocio.ListaMeseroPorDia();

			//Verificamos si el mesero está dado de alta y no de baja
			meseroPorDia = meserosPorDia.Find(mesero => mesero.IdMesero == usuario.Id && DateTime.Now.ToString("yyyy-MM-dd") == mesero.Fecha.ToString("yyyy-MM-dd") && mesero.Salida == new TimeSpan(0,0,0));

			if(meseroPorDia != null)
			{
				//Si hay un mesero dado de alta, lo guardamos en sesión
				Session[Configuracion.Session.MeseroPorDia] = meseroPorDia;

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
			usuario = (Usuario)Session[Configuracion.Session.Usuario];
			meseroPorDia = (MeseroPorDia)Session[Configuracion.Session.MeseroPorDia];

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
					Session[Configuracion.Session.MeseroPorDia] = meseroPorDia;
				}
			}
			else
			{
				//Darse de baja
				try
				{
					MesaNegocio mesaNegocio = new MesaNegocio();
					List<MesaPorDia> mesasAsignadas = (List<MesaPorDia>)Session[Configuracion.Session.MesasAsignada];

					//Si tiene mesas asigndas, se cierran
					//TODO: si tiene pedidos abierto, primero los tiene que cerrar
					if(mesasAsignadas != null)
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
						Session[Configuracion.Session.MeseroPorDia] = null;
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
			usuario = (Usuario)Session[Configuracion.Session.Usuario];
			meseroPorDia = (MeseroPorDia)Session[Configuracion.Session.MeseroPorDia];

			if (meseroPorDia != null)
			{
				MesaNegocio mesaNegocio = new MesaNegocio();
				List<MesaPorDia> mesasAsignadas = mesaNegocio.ListarMesaPorDia()
					.FindAll(x => x.Mesero == meseroPorDia.IdMesero && x.Cierre == null)
					.OrderBy(x => x.Mesa).ToList();

				Session[Configuracion.Session.MesasAsignada] = mesasAsignadas;

				if (mesasAsignadas != null)
				{
					repeaterMesasAsigndas.DataSource = mesasAsignadas;
					repeaterMesasAsigndas.DataBind();

					if(mesasAsignadas.Count > 0)
					{
						lbSinMesasAsignadas.Text = String.Empty;
						return;
					}
						
				}
			}

			lbSinMesasAsignadas.Text = "No cuenta con mesas asignadas. Comuníquese con el Gerente.";
		}
	}
}