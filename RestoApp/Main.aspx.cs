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
		private List<Mesa> mesas;
		private List<MeseroPorDia> meserosPorDia;

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
			}

			//CONTENIDO MESERO
			if (!IsPostBack && AutentificacionUsuario.esMesero(usuario))
			{
				//Verificamos que si ya está en memoria el meseropordia
				if ((MeseroPorDia)Session[Configuracion.Session.MeseroPorDia] != null)
					meseroPorDia = (MeseroPorDia)Session[Configuracion.Session.MeseroPorDia];

				CargarMenuDisponible();
				CargarMeseroPorDia();
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

		// VISTA MESERO
		private void CargarMenuDisponible()
		{
            ProductoNegocio productoNegocio = new ProductoNegocio();
            Session.Add("ProductosDisponibles", productoNegocio.ListarProductos());
            MenuDelDia.DataSource = Session["ProductosDisponibles"];
            MenuDelDia.DataBind();
        }

		private void CargarMeseroPorDia()
		{
			List<MeseroPorDia> meserosPorDia = new List<MeseroPorDia>();
			MesaNegocio mesaNegocio = new MesaNegocio();

			meserosPorDia = mesaNegocio.ListaMeseroPorDia();

			//Verificamos si hay un mesero con el mismo id, el mismo día y con fecha de salida en 0 que ya esté dado de alta
			meseroPorDia = meserosPorDia.Find(mesero => mesero.IdMesero == usuario.Id && DateTime.Now.ToString("yyyy-MM-dd") == mesero.Fecha.ToString("yyyy-MM-dd") && mesero.Salida == new TimeSpan(0,0,0));

			if(meseroPorDia != null)
			{
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
					bool esBaja = mesaNegocio.ModificarMeseroPorDia(meseroPorDia.Id, DateTime.Now.TimeOfDay);

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
    }
}