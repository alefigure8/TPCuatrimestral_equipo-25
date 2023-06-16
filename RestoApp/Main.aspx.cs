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
				CargarEstadoMesas();
				CargarPedido();
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

		private void CargarEstadoMesas()
		{

			//TODO: LLAMADO A DB PARA OBTENER ESTADO DE MESAS

			var dataTable = new DataTable();
			dataTable.Columns.Add("Numero", typeof(int));
			dataTable.Columns.Add("Mesero", typeof(string));
			dataTable.Columns.Add("Apertura", typeof(DateTime));
			dataTable.Columns.Add("Cierre", typeof(DateTime));

			// Agregar filas al DataTable de forma individual
			AgregarFila(dataTable, 1, "Juan", DateTime.Now, DateTime.Now);
			AgregarFila(dataTable, 2, "Maria", DateTime.Now, DateTime.Now);
			AgregarFila(dataTable, 3, "Pedro", DateTime.Now, DateTime.Now);
			AgregarFila(dataTable, 4, "Laura", DateTime.Now, DateTime.Now);
			AgregarFila(dataTable, 5, "Carlos", DateTime.Now, DateTime.Now);
			AgregarFila(dataTable, 6, "Ana", DateTime.Now, DateTime.Now);

			// Enlazar el DataTable al DataGrid
			datagrid.DataSource = dataTable;
			datagrid.DataBind();
		}

		private void AgregarFila(DataTable dataTable, int numero, string mesero, DateTime apertura, DateTime cierre)
		{
			var fila = dataTable.NewRow();
			fila["Numero"] = numero;
			fila["Mesero"] = mesero;
			fila["Apertura"] = apertura;
			fila["Cierre"] = cierre;
			dataTable.Rows.Add(fila);
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
			AgregarFilaPedidos(dataTable, 1, "Hamburguesa", DateTime.Now, DateTime.Now);
			AgregarFilaPedidos(dataTable, 2, "Pizza", DateTime.Now, DateTime.Now);
			AgregarFilaPedidos(dataTable, 1, "Sushi", DateTime.Now, DateTime.Now);
			AgregarFilaPedidos(dataTable, 3, "Ensalada", DateTime.Now, DateTime.Now);
			AgregarFilaPedidos(dataTable, 2, "Pasta", DateTime.Now, DateTime.Now);
			AgregarFilaPedidos(dataTable, 1, "Tacos", DateTime.Now, DateTime.Now);


			// Enlazar los datos al DataGrid
			datagridPedidos.DataSource = dataTable;
			datagridPedidos.DataBind();
		}

		private void AgregarFilaPedidos(DataTable dataTable, int mesa, string pedido, DateTime apertura, DateTime cierre)
		{
			var fila = dataTable.NewRow();
			fila["Mesa"] = mesa;
			fila["PedidoComida"] = pedido;
			fila["Apertura"] = apertura;
			fila["Cierre"] = cierre;
			dataTable.Rows.Add(fila);
		}
	}
}