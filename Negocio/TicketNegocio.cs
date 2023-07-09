using Dominio;
using Opciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Opciones.ColumnasDB;

namespace Negocio
{
	public class TicketNegocio
	{

		//Retornarmos todos los tickets que no estén cobrados, pero que hayan cerrado
		public List<Ticket> Listar()
		{
			List<Ticket> tickets = new List<Ticket>();
			AccesoDB datos = new AccesoDB();

			string query =
				$"SELECT S.{ColumnasDB.Servicio.Id}, S.{ColumnasDB.Servicio.Fecha}, S.{ColumnasDB.Servicio.Cierre}, S.{ColumnasDB.Servicio.Cobrado}, M.{ColumnasDB.Mesa.Numero}, M.{ColumnasDB.MesasPorDia.IdMesero}" +
				$" FROM {ColumnasDB.Servicio.DB} S" +
				$" INNER JOIN {ColumnasDB.MesasPorDia.DB} M" +
				$" ON S.{ColumnasDB.MesasPorDia.Id} = M.{ColumnasDB.MesasPorDia.Id}" +
				$" WHERE S.{ColumnasDB.Servicio.Cobrado} = 0" +
				$" AND S.{ColumnasDB.Servicio.Cierre} IS NOT NULL";

			try
			{
				datos.setQuery(query);
				datos.executeReader();
				
				while (datos.Reader.Read())
				{
					Ticket auxTicket = new Ticket();
					//ID
					auxTicket.Id = (Int32)datos.Reader[ColumnasDB.Servicio.Id];

					//FECHA
					if (datos.Reader[ColumnasDB.Servicio.Fecha] != null)
						auxTicket.Fecha = (DateTime)datos.Reader[ColumnasDB.Servicio.Fecha];

					//CIERRE
					if (datos.Reader[ColumnasDB.Servicio.Cierre] != null)
						auxTicket.Cierre = (TimeSpan)datos.Reader[ColumnasDB.Servicio.Cierre];
					
					//COBRADO
					if (datos.Reader[ColumnasDB.Servicio.Cobrado] != null)
						auxTicket.Cobrado = (bool)datos.Reader[ColumnasDB.Servicio.Cobrado];

					//MESA
					if (datos.Reader[ColumnasDB.Mesa.Numero] != null)
						auxTicket.Mesa = (int)datos.Reader[ColumnasDB.Mesa.Numero];

					//MESARO ID
					if (datos.Reader[ColumnasDB.MesasPorDia.IdMesero] != null)
						auxTicket.IdMesero = (int)datos.Reader[ColumnasDB.MesasPorDia.IdMesero];

					//PEDIDOS
					List<TicketDetalle> detalles = this.ListarDetalle(auxTicket.Id);
					auxTicket.Detalle = detalles;
					
					tickets.Add(auxTicket);
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
			finally
			{
				datos.closeConnection();
			}

			return tickets;
		}

		//Retornamos el detalle de productos según el servicio
		public List<TicketDetalle> ListarDetalle(int servicio)
		{
			List<TicketDetalle> detalles = new List<TicketDetalle>();
			AccesoDB datos = new AccesoDB();
			
			//Buscamos el listado de pedidos por servicio
			string query = 
				$"SELECT P.{ColumnasDB.Pedido.Id}, PXP.{ColumnasDB.ProductoPorPedido.Cantidad} , PM.{ColumnasDB.ProductoDD.Valor}, {ColumnasDB.ProductoDD.Nombre}" +
				$" FROM {ColumnasDB.Pedido.DB} P" +
				$" INNER JOIN {ColumnasDB.ProductoPorPedido.DB} PXP" +
				$" ON P.{ColumnasDB.Pedido.Id} = PXP.{ColumnasDB.Pedido.Id}" +
				$" INNER JOIN {ColumnasDB.ProductoDD.DB} PM" +
				$" ON PXP.{ColumnasDB.ProductoPorPedido.Id}= PM.{ColumnasDB.Producto.Id}" +
				$" AND PXP.{ColumnasDB.ProductoPorPedido.Fecha} = PM.{ColumnasDB.ProductoDD.Fecha}" +
				$" WHERE P.{ColumnasDB.Pedido.IdServicio} = {servicio}";

			try
			{
				datos.setQuery(query);
				datos.executeReader();
				
				while (datos.Reader.Read())
				{
					TicketDetalle auxDetalleTicket = new TicketDetalle();
					//ID
					auxDetalleTicket.Id = (int)datos.Reader[ColumnasDB.Pedido.Id];

					//FECHA
					if (datos.Reader[ColumnasDB.ProductoDD.Nombre] != null)
						auxDetalleTicket.Descripcion = (string)datos.Reader[ColumnasDB.ProductoDD.Nombre];

					//CIERRE
					if (datos.Reader[ColumnasDB.ProductoDD.Valor] != null)
						auxDetalleTicket.Precio = (Decimal)datos.Reader[ColumnasDB.ProductoDD.Valor];

					//COBRADO
					if (datos.Reader[ColumnasDB.ProductoPorPedido.Cantidad] != null)
						auxDetalleTicket.Cantidad = (int)datos.Reader[ColumnasDB.ProductoPorPedido.Cantidad];

					detalles.Add(auxDetalleTicket);
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
			finally
			{
				datos.closeConnection();
			}

			return detalles;
		}

		//Retornamos el Ticket del servicio
		public Ticket TicketPorServicio(int servicio)
		{
			Ticket ticket = new Ticket();
			
			AccesoDB datos = new AccesoDB();

			string query =
				$"SELECT S.{ColumnasDB.Servicio.Id}, S.{ColumnasDB.Servicio.Fecha}, S.{ColumnasDB.Servicio.Cierre}, S.{ColumnasDB.Servicio.Cobrado}, M.{ColumnasDB.Mesa.Numero}, M.{ColumnasDB.MesasPorDia.IdMesero}" +
				$" FROM {ColumnasDB.Servicio.DB} S" +
				$" INNER JOIN {ColumnasDB.MesasPorDia.DB} M" +
				$" ON S.{ColumnasDB.MesasPorDia.Id} = M.{ColumnasDB.MesasPorDia.Id}" +
				$" INNER JOIN {ColumnasDB.Pedido.DB} P" +
				$" ON S.{ColumnasDB.Servicio.Id} = P.{ColumnasDB.Servicio.Id}" +
				$" WHERE S.{ColumnasDB.Servicio.Cobrado} = 0" +
				$" AND S.{ColumnasDB.Servicio.Cierre} IS NOT NULL " +
				$" AND S.{ColumnasDB.Servicio.Id} = {servicio}";

			try
			{
				datos.setQuery(query);
				datos.executeReader();

				while (datos.Reader.Read())
				{
					//ID
					ticket.Id = (Int32)datos.Reader[ColumnasDB.Mesa.Numero];

					//FECHA
					if (datos.Reader[ColumnasDB.Servicio.Fecha] != null)
						ticket.Fecha = (DateTime)datos.Reader[ColumnasDB.Servicio.Fecha];

					//CIERRE
					if (datos.Reader[ColumnasDB.Servicio.Cierre] != null)
						ticket.Cierre = (TimeSpan)datos.Reader[ColumnasDB.Servicio.Cierre];

					//COBRADO
					if (datos.Reader[ColumnasDB.Servicio.Cobrado] != null)
						ticket.Cobrado = (bool)datos.Reader[ColumnasDB.Servicio.Cobrado];

					//MESA
					if (datos.Reader[ColumnasDB.Mesa.Numero] != null)
						ticket.Mesa = (int)datos.Reader[ColumnasDB.Mesa.Numero];

					//MESARO ID
					if (datos.Reader[ColumnasDB.MesasPorDia.IdMesero] != null)
						ticket.IdMesero = (int)datos.Reader[ColumnasDB.MesasPorDia.IdMesero];

					//PEDIDOS
					List<TicketDetalle> detalles = this.ListarDetalle(ticket.Id);
					ticket.Detalle = detalles;
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
			finally
			{
				datos.closeConnection();
			}

			return ticket;
		}

		//Retornamos un listado de ticket según el mesero
		public List<Ticket> ListarPorMesero(int mesero)
		{
			List<Ticket> tickets = new List<Ticket>();
			AccesoDB datos = new AccesoDB();

			string query =
				$"SELECT S.{ColumnasDB.Servicio.Id}, S.{ColumnasDB.Servicio.Fecha}, S.{ColumnasDB.Servicio.Cierre}, S.{ColumnasDB.Servicio.Cobrado}, M.{ColumnasDB.Mesa.Numero}, M.{ColumnasDB.MesasPorDia.IdMesero}" +
				$" FROM {ColumnasDB.Servicio.DB} S" +
				$" INNER JOIN {ColumnasDB.MesasPorDia.DB} M" +
				$" ON S.{ColumnasDB.MesasPorDia.Id} = M.{ColumnasDB.MesasPorDia.Id}" +
				$" INNER JOIN {ColumnasDB.Pedido.DB} P" +
				$" ON S.{ColumnasDB.Servicio.Id} = P.{ColumnasDB.Servicio.Id}" +
				$" WHERE S.{ColumnasDB.Servicio.Cobrado} = 0" +
				$" AND S.{ColumnasDB.Servicio.Cierre} IS NOT NULL " +
				$" AND M.{ColumnasDB.MesasPorDia.IdMesero} = {mesero}";

			try
			{
				datos.setQuery(query);
				datos.executeReader();

				while (datos.Reader.Read())
				{
					Ticket auxTicket = new Ticket();
					//ID
					auxTicket.Id = (Int32)datos.Reader[ColumnasDB.Mesa.Numero];

					//FECHA
					if (datos.Reader[ColumnasDB.Servicio.Fecha] != null)
						auxTicket.Fecha = (DateTime)datos.Reader[ColumnasDB.Servicio.Fecha];

					//CIERRE
					if (datos.Reader[ColumnasDB.Servicio.Cierre] != null)
						auxTicket.Cierre = (TimeSpan)datos.Reader[ColumnasDB.Servicio.Cierre];

					//COBRADO
					if (datos.Reader[ColumnasDB.Servicio.Cobrado] != null)
						auxTicket.Cobrado = (bool)datos.Reader[ColumnasDB.Servicio.Cobrado];

					//MESA
					if (datos.Reader[ColumnasDB.Mesa.Numero] != null)
						auxTicket.Mesa = (int)datos.Reader[ColumnasDB.Mesa.Numero];

					//MESARO ID
					if (datos.Reader[ColumnasDB.MesasPorDia.IdMesero] != null)
						auxTicket.IdMesero = (int)datos.Reader[ColumnasDB.MesasPorDia.IdMesero];

					//PEDIDOS
					List<TicketDetalle> detalles = this.ListarDetalle(auxTicket.Id);
					auxTicket.Detalle = detalles;

					tickets.Add(auxTicket);
				}
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
			finally
			{
				datos.closeConnection();
			}

			return tickets;
		}

		//TODO: Retornar ticket por dia o periodo.
	}
}
