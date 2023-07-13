using Dominio;
using Helper;
using Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestoApp
{
	public partial class Tickets : System.Web.UI.Page
	{
		private Usuario usuario;
		public Decimal precio = 0;

			protected void Page_Load(object sender, EventArgs e)
		{
			//Verificar que sea usuario
			if (AutentificacionUsuario.esUser(Helper.Session.GetUsuario()))
				usuario = Helper.Session.GetUsuario();

			//Verificar Query del número de servicio
			int servicio = Convert.ToInt32(Request.QueryString["servicio"]);
			//int mesero = Convert.ToInt32(Request.QueryString["mesero"]);

			// CONTENIDO GERENTE
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				if (servicio > 0)
				{
					//Guardamos el número en session
					Session["ServicioTicket"] = servicio;

					try
					{
						BuscarTicket();
					}
					catch
					{
						throw new Exception("Error al cargar el ticket");
					}
				}	
				else
				{
					//Render de todos los tickets del mesero
					try
					{
						CargarTicketsAbiertos();
					}
					catch
					{
						throw new Exception("Error al cargar el ticket");
					}
				}
			}

			// CONTENIDO MESERO
			if (!IsPostBack && AutentificacionUsuario.esMesero(usuario))
			{

				if (servicio > 0)
				{
					//Guardamos el número en session
					Session["ServicioTicket"] = servicio;
					try
					{
						BuscarTicket();
					}
					catch
					{
						throw new Exception("Error al cargar el ticket");
					}
				}
				else
				{
					//Render de todos los tickets del mesero
					try
					{
						CargarTicketPorMesero();
					}
					catch
					{
						throw new Exception("Error al cargar el ticket");
					}
				}

			}

			//Cargamos el repeater
			CargarRepeaterTicker();

		}

		private List<Ticket> BuscarTicket()
		{
			//Iniciamos Ticket Negocio
			TicketNegocio ticketNegocio = new TicketNegocio();

			List<Ticket> tickets = new List<Ticket>();

			//Obtenemos el ticket según el número de servcio
			Ticket ticket = ticketNegocio.TicketPorServicio((int)Session["ServicioTicket"]);
			
			tickets.Add(ticket);
			
			//Guardamos ticket en session
			Session["Tickets"] = tickets;

			//Validamos que el mesero sea el mismo que el del ticket
			int idMesero = Helper.Session.GetUsuario().Id;
			
			if(ticket.IdMesero == idMesero)
			{
				lbTituloTicket.Text = "Ticket del servicio #" + ticket.Id;
				return tickets;
			}

			return null;
		}

		//Retornamos todos los ticket abiertos
		private List<Ticket> CargarTicketsAbiertos()
		{
			//Iniciamos Ticket Negocio
			TicketNegocio ticketNegocio = new TicketNegocio();

			//Listamos tickets
			List<Ticket> tickets = ticketNegocio.Listar();

			//Guardamos Tickets en Session
			Session["Tickets"] = tickets;

			//Label
			lbTituloTicket.Text = "Tickets abiertos";

			return tickets;
		}

		//Retornamos el listado de tickets según el mesero
		private List<Ticket> CargarTicketPorMesero()
		{
			//Buscamos el id del mesero
			int idMesero = Helper.Session.GetUsuario().Id;

			//Iniciamos Ticket Negocio
			TicketNegocio ticketNegocio = new TicketNegocio();

			//Listamos tickets
			List<Ticket> tickets = ticketNegocio.ListarPorMesero(idMesero);

			//Guardamos Tickets en Session
			Session["Tickets"] = tickets;

			//Label
			lbTituloTicket.Text = "Tickets abiertos";

			return tickets;
		}

		private void CargarRepeaterTicker()
		{
			repeaterTickets.DataSource = (List<Ticket>)Session["Tickets"];
			repeaterTickets.DataBind();
		}

		protected string RenderDetalles(object detallesObj)
		{
			precio = 0;

			if (detallesObj is List<TicketDetalle> detalles)
			{

				StringBuilder sb = new StringBuilder();

				foreach (TicketDetalle item in detalles)
				{
					precio += item.Precio * item.Cantidad;

					sb.AppendFormat(
						@"<tr>
						<th scope=""row"">1</th>
						<td>{0}</td>
						<td>{1}</td>
						<td>{2}</td>
						</tr>",
						item.Descripcion,
						item.Cantidad,
						item.Precio
					);
				}

				return sb.ToString();
			}

			return string.Empty;
		}

		protected decimal CalcularPrecio()
		{
			return precio;
		}

		protected void btnCobrar_Click(object sender, EventArgs args)
		{
			//Recuperamos argumetno del botón
			Button btn = (Button)sender;
			int numeroServicio = Convert.ToInt32(btn.CommandArgument);

			//Iniciamos negocio de Servicios
			ServicioNegocio servicioNegocio = new ServicioNegocio();

			//Si se cierra el servicio, sacamos del Session el servicio
			if (servicioNegocio.CobrarServicio(numeroServicio))
			{
				//Si se cobró correctamente, sacamos el sercisio de la sessión de Servicios
				List<Servicio> servicio = Helper.Session.GetServicios().FindAll(item => item.Id != numeroServicio);
				Helper.Session.SetServicios(servicio);

				//Mensaje de exito
				object msg = new { msg = $"El servicio número {numeroServicio} fue cobrado con éxito", tipo = "success" };
				Helper.Session.SetMensajeModal(msg);

				//Redirigir a Main
				Response.Redirect("Main.aspx");
			}
			else
			{
				//Mensaje error
				object msg = new { msg = $"El servicio número {numeroServicio} no pudo ser cobrado", tipo = "error" };
				Helper.Session.SetMensajeModal(msg);
			}
		}
	}
}