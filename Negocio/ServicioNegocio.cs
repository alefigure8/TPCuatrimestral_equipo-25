using Dominio;
using Opciones;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Negocio
{
	public class ServicioNegocio
	{
		//Listamos todos los servicios Abiertos
		public List<Servicio> Listar()
		{
			List<Servicio> servicios = new List<Servicio>();
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery($"SELECT {ColumnasDB.Servicio.Id}, {ColumnasDB.MesasPorDia.IdMesa}, S.{ColumnasDB.Servicio.Fecha}, S.{ColumnasDB.Servicio.Apertura}, S.{ColumnasDB.Servicio.Cierre}, {ColumnasDB.Servicio.Cobrado} " +
					$"FROM {ColumnasDB.Servicio.DB} S " +
					$"INNER JOIN {ColumnasDB.MesasPorDia.DB} MPD " +
					$"ON S.{ColumnasDB.MesasPorDia.Id} = MPD.{ColumnasDB.MesasPorDia.Id}");

				datos.executeReader();

				
				while (datos.Reader.Read())
				{
					Servicio auxServicio = new Servicio();

					//ID
					auxServicio.Id = (Int32)datos.Reader[ColumnasDB.Servicio.Id];

					//Mesa
					if (datos.Reader[ColumnasDB.MesasPorDia.IdMesa] != null)
						auxServicio.Mesa = (int)datos.Reader[ColumnasDB.MesasPorDia.IdMesa]; ;

					//Fecha
					if (datos.Reader[ColumnasDB.Servicio.Fecha] != null)
						auxServicio.Fecha = (DateTime)datos.Reader[ColumnasDB.Servicio.Fecha];

					//Apertura
					if (datos.Reader[ColumnasDB.Servicio.Apertura] != null)
						auxServicio.Apertura = (TimeSpan)datos.Reader[ColumnasDB.Servicio.Apertura];
					
					//Cierre
					object valorCierre = datos.Reader[ColumnasDB.Servicio.Cierre];
					auxServicio.Cierre = DBNull.Value.Equals(valorCierre) ? (TimeSpan?)null : (TimeSpan?)valorCierre;

					//Cobrado
					if (datos.Reader[ColumnasDB.Servicio.Cobrado] != null)
						auxServicio.Cobrado = (bool)datos.Reader[ColumnasDB.Servicio.Cobrado];

					servicios.Add(auxServicio);
				}
			}
			catch(Exception ex)
			{
				throw ex;
			}
			finally
			{
				datos.closeConnection();
			}

			return servicios;
		}

		//Abrimos servicio y retornamos el ID del servicio creado
		public int AbrirServicio(int mesa)
		{
			int id = 0;
			AccesoDB datos = new AccesoDB();

			//Validar que no se pueda crear servicios si para la mesa el cobro está como false
			Servicio servicio = this.Listar().Find(item => item.Cobrado == false && item.Mesa == mesa);

			//Si no existe servicio con el numero de mesa y el cobro en false, creamos servicio
			if(servicio == null)
			{
				try
				{
					//Buscamos ID de MesaPorDia con el número de Mesa y creamos el servicio
					datos.setQuery($"DECLARE @IDMESAPORDIA BIGINT " +
						$"SET @IDMESAPORDIA = ISNULL((SELECT {ColumnasDB.MesasPorDia.Id} FROM {ColumnasDB.MesasPorDia.DB} WHERE {ColumnasDB.MesasPorDia.IdMesa} = {mesa} AND CIERRE IS NULL ), 0); " +
						$"IF @IDMESAPORDIA  > 0 " +
						$"BEGIN " +
						$"INSERT INTO SERVICIO ({ColumnasDB.MesasPorDia.Id}, {ColumnasDB.Servicio.Fecha}, {ColumnasDB.Servicio.Apertura}) " +
						$"VALUES (@IDMESAPORDIA, '{DateTime.Now.ToString("yyyy - MM - dd")}', '{DateTime.Now.ToString("HH:mm:ss")}'); " +
						$"SELECT CAST(scope_identity() AS int) " +
						$"END");

					return datos.executeScalar();
				}
				catch (Exception Ex)
				{
					return -1;
				}
				finally
				{
					datos.closeConnection();
				}
			}
			else
			{
				return -1;
			}
		}

		//Cerramos el servicio y retornamos true o false dependiendo la operación
		public bool CerrarServicio(int mesa)
		{
			AccesoDB datos = new AccesoDB();

			string estadoPedido = "Entregado";

			string queryCantidadPedidosAbiertos = $"DECLARE @IDSERVICIO BIGINT = (" +
				$" SELECT S.{ColumnasDB.Servicio.Id}" +
				$" FROM {ColumnasDB.Servicio.DB} S" +
				$" INNER JOIN {ColumnasDB.MesasPorDia.DB} MPD" +
				$" ON MPD.{ColumnasDB.MesasPorDia.Id} = S.{ColumnasDB.MesasPorDia.Id}" +
				$" WHERE MPD.IDMESA = {mesa}" +
				$" AND MPD.{ColumnasDB.MesasPorDia.Cierre} IS NULL" +
			$" )" +
			$" DECLARE @PEDIDOSABIERTOS INT = (" +
				$"SELECT COUNT(*) AS CANTIDAD" +
				$" FROM {ColumnasDB.Servicio.DB} S" +
				$" INNER JOIN {ColumnasDB.Pedido.DB} P" +
				$" ON S.{ColumnasDB.Servicio.Id} = P.{ColumnasDB.Pedido.IdServicio}" +
				$" INNER JOIN {ColumnasDB.EstadosxPedido.DB} EXP" +
				$" ON EXP.{ColumnasDB.EstadosxPedido.IdPedido}= P.{ColumnasDB.Pedido.Id}" +
				$" INNER JOIN {ColumnasDB.Estados.DB} EP" +
				$" ON EP.{ColumnasDB.Estados.Id} = EXP.{ColumnasDB.EstadosxPedido.IdEstado}" +
				$" WHERE S.{ColumnasDB.Servicio.Id} = @IDSERVICIO" +
				$" AND EP.{ColumnasDB.Estados.Descripcion} <> '{estadoPedido}'" +
				$")";


			//Buscamos cantidad de pedidos abiertos para el sistema que contiene la mesa que nos entregan por parámetro
			datos.setQuery(queryCantidadPedidosAbiertos);

			datos.executeReader();

			int cantidadPedidosAbiertos = 0;

			while (datos.Reader.Read()) { 
				cantidadPedidosAbiertos = (int)datos.Reader["CANTIDAD"];
			}


			//Valida que el servicio no se pueda cerrar si hay pedidos abiertos
			if(cantidadPedidosAbiertos > 0)
			{
				try
				{
					datos.setQuery(
						$" DECLARE @IDMESAPORDIA BIGINT" +
						$" SET @IDMESAPORDIA = ISNULL((SELECT {ColumnasDB.MesasPorDia.Id} FROM {ColumnasDB.MesasPorDia.DB} WHERE {ColumnasDB.MesasPorDia.IdMesa} = {mesa} AND CIERRE IS NULL ), 0);" +
						$" IF @IDMESAPORDIA  > 0" +
						$" BEGIN" +
							$" UPDATE SERVICIO SET {ColumnasDB.Servicio.Cierre} = '{DateTime.Now.ToString("HH:mm:ss")}' WHERE {ColumnasDB.MesasPorDia.Id} = @IDMESAPORDIA AND CIERRE IS NULL" +
						$" END");

					return datos.executeNonQuery();
				}
				catch (Exception Ex)
				{
					return false;
				}
				finally
				{
					datos.closeConnection();
				}
			}

			return false;
		}
		
		//Cobrar servicio. Modificamos cobrado  a true
		public bool ModificarServicio(int idServicio)
		{
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery($"UPDATE SERVICIO SET {ColumnasDB.Servicio.Cobrado} = 1 WHERE {ColumnasDB.Servicio.Id} = {idServicio}");

				return datos.executeNonQuery();
			}
			catch (Exception Ex)
			{
				return false;
			}
			finally
			{
				datos.closeConnection();
			}

		}
	}
}
