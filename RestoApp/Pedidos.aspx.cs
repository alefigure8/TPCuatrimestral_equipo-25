using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;
using System.Data;

namespace RestoApp
{
	public partial class Pedidos : System.Web.UI.Page
	{
		private Usuario usuario;

		//Querys
		private bool abierto;
		private string dia;

		protected void Page_Load(object sender, EventArgs e)
		{
			//Verificar que sea usuario
			if (AutentificacionUsuario.esUser(Helper.Session.GetUsuario()))
				usuario = Helper.Session.GetUsuario();

			//Verificar Query del número de servicio
			abierto = Convert.ToBoolean(Request.QueryString["abierto"]);
			dia = Request.QueryString["dia"];

			// CONTENIDO GERENTE
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				try
				{
					//Mostramos los pedidos generales por Mesa y Servico
					//Cargamos los dropdown
					CargarDropDownListMesa();
				}
				catch (Exception ex)
				{
					UIMostrarAlerta(ex.Message);
				}
			}

			// CONTENIDO MESERO
			if (!IsPostBack && AutentificacionUsuario.esMesero(usuario))
			{

				if (abierto)
				{
					//Guardamos el número en session
					Session["ServicioPedido"] = abierto;
					try
					{
						//Llamamos a los pedidos que permanezcan abiertos por Mesa y Servicio
					}
					catch (Exception ex)
					{
						UIMostrarAlerta(ex.Message);
					}
				}

				if (dia != null)
				{
					try
					{
						//Render de todos los Pedidos realizados en el día por Mesa y Servicio
					}
					catch (Exception ex)
					{
						UIMostrarAlerta(ex.Message);
					}
				}

			}

			//Cargamos el repeater
			CargarRepeaterPedidos();

		}

		//UI Alerta Modal
		private void UIMostrarAlerta(string mensaje, string tipoMensaje = "error")
		{
			string scriptModal = $"alertaModal(\"{mensaje}\", \"{tipoMensaje}\");";
			ScriptManager.RegisterStartupScript(this, GetType(), "ScriptTicket", scriptModal, true);

			//Borramos mensaje del modal
			Helper.Session.SetMensajeModal(null);
		}

		private void CargarDropDownListMesa()
		{
			List<Mesa> mesas = Helper.Session.GetMesas().FindAll(item => item.Activo == true).OrderBy(item => item.Numero).ToList();
			//List<Servicio> servicios = Helper.Session.GetServicios();

			//Ordenamos la lista por mesa
			List<Servicio> listaMesa = (List<Servicio>)Helper.Session.GetServicios().OrderBy(item => item.Mesa).ToList();

			foreach (Mesa item in mesas)
			{
				ddlMesaPedidos.Items.Add(new ListItem($"Mesa {item.Numero}", item.Numero.ToString()));
				ddlMesaPedidos.Attributes["class"] = "dropdown-item";
			}
		}
	
		private void CargarDropDownListServicio(int id)
		{
			ServicioNegocio servicioNegocio = new ServicioNegocio();
			List<Servicio> listaServicios = servicioNegocio.Listar().FindAll(item => item.Mesa == id);
			Session["ServicioPorMesa"] = listaServicios;

			//Limpiamos la opción anterio
			ddlServicioPedidos.Items.Clear();

			if(listaServicios.Count > 0)
			{
				ddlServicioPedidos.Items.Add(new ListItem($"Todo", "0"));
				
				foreach (Servicio item in listaServicios)
				{
					ddlServicioPedidos.Items.Add(new ListItem($"Servicio {item.Id}", item.Id.ToString()));
					ddlMesaPedidos.Attributes["class"] = "dropdown-item";
				}
			}
			else
				ddlServicioPedidos.Items.Add(new ListItem($"Sin servicio"));
		}
		
		private void CargarPedido()
		{
			//Session
			List<Pedido> pedidos = (List<Pedido>)Session["PedidosGerente"];

			//DataTable
			var dataTable = new DataTable();
			dataTable.Columns.Add("PedidoComida", typeof(string));
			dataTable.Columns.Add("Actualización", typeof(DateTime));
			dataTable.Columns.Add("Estado", typeof(string));

			foreach (Pedido itemPedido in pedidos)
			{
				foreach (ProductoPorPedido itemProducto in itemPedido.Productossolicitados)
				{
					dataTable.Rows.Add((string)itemProducto.Productodeldia.Nombre, (DateTime)itemPedido.ultimaactualizacion, (string)itemPedido.EstadoDescripcion);

				}
			}
			

			//Filas
			foreach (DataRow row in dataTable.Rows)
			{
				string pedidocomida = (string)row["PedidoComida"];
				DateTime actualizacion = (DateTime)row["Actualización"];
				string estado = (string)row["Estado"];
			}

			// Enlazar el DataTable al DataGrid
			datagridPedidosGerente.DataSource = dataTable;
			datagridPedidosGerente.DataBind();
		}

		private void CargarRepeaterPedidos()
		{
			//repeaterPedidos

		}


		/***** Events ******/
		protected void ddlMesaPedidos_SelectedIndexChanged(object sender, EventArgs e)
		{
			int idServicio = Convert.ToInt32(ddlMesaPedidos.SelectedValue);

			CargarDropDownListServicio(idServicio);
		}

		protected void BtnBuscar_Click(object sender, EventArgs e)
		{
			//DB
			PedidoNegocio pedidoNegocio = new PedidoNegocio();

			//Obtenemos los valores
			int? mesa = Convert.ToInt32(ddlMesaPedidos?.SelectedValue);
			int? servicio = Convert.ToInt32(ddlServicioPedidos?.SelectedValue);

			List<Pedido> pedidos;

			if(servicio == 0)
			{
				// Si servicio es null, se trae todos los de la mesa
				pedidos = pedidoNegocio.ListarPedidosDelDiaPorMesa((int)mesa);
			}
			else
			{
				//Si servicio tiene número, solo se trae los del servio
				pedidos = pedidoNegocio.ListarPedidosDelDiaPorServicio((int)servicio);
			}

			Session["PedidosGerente"] = pedidos.OrderBy(item => item.Estado).ToList();
			
			CargarPedido();
		}
	}
}