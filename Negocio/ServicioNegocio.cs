using Dominio;
using Opciones;
using System;
using System.Collections.Generic;

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
				datos.setQuery($"SELECT S.{ColumnasDB.Servicio.Id}, MPD.{ColumnasDB.MesasPorDia.IdMesa}, U.{ColumnasDB.Usuario.Id}, U.{ColumnasDB.Usuario.Apellidos}, U.{ColumnasDB.Usuario.Nombres} ,S.{ColumnasDB.Servicio.Fecha}, S.{ColumnasDB.Servicio.Apertura}, S.{ColumnasDB.Servicio.Cierre}, {ColumnasDB.Servicio.Cobrado} " +
					$"FROM {ColumnasDB.Servicio.DB} S " +
					$"INNER JOIN {ColumnasDB.MesasPorDia.DB} MPD " +
					$"ON S.{ColumnasDB.MesasPorDia.Id} = MPD.{ColumnasDB.MesasPorDia.Id} " +
					$" INNER JOIN {ColumnasDB.Mesa.DB} M " +
					$"ON MPD.{ColumnasDB.MesasPorDia.IdMesa} = M.{ColumnasDB.Mesa.Numero} " +
					$" INNER JOIN {ColumnasDB.MeseroPorDia.DB} MP " +
					$"ON MPD.{ColumnasDB.MeseroPorDia.Id} = MP.{ColumnasDB.MeseroPorDia.Id} " +
					$"INNER JOIN {ColumnasDB.Usuario.DB} U " +
					$"ON MP.{ColumnasDB.Servicio.IdMesero} = U.{ColumnasDB.Usuario.Id} " +
					$"WHERE S.{ColumnasDB.Servicio.Cierre} IS NULL " +
					$"ORDER BY S.{ColumnasDB.Servicio.Fecha} DESC, S.{ColumnasDB.Servicio.Apertura} DESC");

				datos.executeReader();

				
				while (datos.Reader.Read())
				{
					Servicio auxServicio = new Servicio();

					//ID
					auxServicio.Id = (Int32)datos.Reader[ColumnasDB.Servicio.Id];

					//MESA
					if (datos.Reader[ColumnasDB.MesasPorDia.IdMesa] != null)
						auxServicio.Mesa = (int)datos.Reader[ColumnasDB.MesasPorDia.IdMesa];

					//IDMESERO
					if (datos.Reader[ColumnasDB.Usuario.Id] != null)
						auxServicio.IdMesero = (int)datos.Reader[ColumnasDB.Usuario.Id];

					//Fecha
					if (datos.Reader[ColumnasDB.Usuario.Nombres] != null && datos.Reader[ColumnasDB.Usuario.Apellidos] != null)
						auxServicio.Mesero = (string)datos.Reader[ColumnasDB.Usuario.Nombres] + " " + (string)datos.Reader[ColumnasDB.Usuario.Apellidos];

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
					datos.setQuery(
						//Buscamos si tiene servicio abierto
						$"DECLARE @TIENESERVICIO INT = ( " +
							$"SELECT count(*) " +
							$"FROM {ColumnasDB.Servicio.DB} S " +
							$"INNER JOIN {ColumnasDB.MesasPorDia.DB} M " +
							$"ON S.{ColumnasDB.MesasPorDia.Id} = M.{ColumnasDB.MesasPorDia.Id} " +
							$"WHERE S.{ColumnasDB.Servicio.Cierre} is null " +
							$"And M.{ColumnasDB.Mesa.Numero} = {mesa} " +
						$") " +

						//Buscamos que la mesa tenga mesa por dia
						$"DECLARE @IDMESAPORDIA BIGINT " +
						$"SET @IDMESAPORDIA = ISNULL((SELECT {ColumnasDB.MesasPorDia.Id} " +
						$"FROM {ColumnasDB.MesasPorDia.DB} " +
						$"WHERE {ColumnasDB.MesasPorDia.IdMesa} = {mesa} " +
						$"AND CIERRE IS NULL ), 0); " +

						//Si tiene idmesapordia y la cantidad de servicios es 0, abrimos servicio
						$"IF @IDMESAPORDIA  > 0 AND @TIENESERVICIO = 0" +
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
			int cantidadPedidosAbiertos = 0;
			

			string queryCantidadPedidosAbiertos = $"DECLARE @IDSERVICIO BIGINT = (" +
					$" SELECT S.{ColumnasDB.Servicio.Id}" +
					$" FROM {ColumnasDB.Servicio.DB} S" +
					$" INNER JOIN {ColumnasDB.MesasPorDia.DB} MPD" +
					$" ON MPD.{ColumnasDB.MesasPorDia.Id} = S.{ColumnasDB.MesasPorDia.Id}" +
					$" WHERE MPD.{ColumnasDB.MesasPorDia.IdMesa} = {mesa}" +
					$" AND S.{ColumnasDB.MesasPorDia.Cierre} IS NULL" +
					$" )" +
					$"SELECT COUNT(*) AS CANTIDAD" +
					$" FROM {ColumnasDB.Servicio.DB} S" +
					$" INNER JOIN {ColumnasDB.Pedido.DB} P" +
					$" ON S.{ColumnasDB.Servicio.Id} = P.{ColumnasDB.Pedido.IdServicio}" +
					$" INNER JOIN {ColumnasDB.EstadosxPedido.DB} EXP" +
					$" ON EXP.{ColumnasDB.EstadosxPedido.IdPedido}= P.{ColumnasDB.Pedido.Id}" +
					$" INNER JOIN {ColumnasDB.Estados.DB} EP" +
					$" ON EP.{ColumnasDB.Estados.Id} = EXP.{ColumnasDB.EstadosxPedido.IdEstado}" +
					$" WHERE S.{ColumnasDB.Servicio.Id} = @IDSERVICIO" +
					$" AND (EP.{ColumnasDB.Estados.Descripcion} <> '{estadoPedido}'" +
					$" OR (EP.{ColumnasDB.Estados.Descripcion} = '{estadoPedido}' AND S.{ColumnasDB.Servicio.Cierre} IS NOT NULL))"
					;

			try
			{
				//Buscamos cantidad de pedidos abiertos para el sistema que contiene la mesa que nos entregan por parámetro
				datos.setQuery(queryCantidadPedidosAbiertos);

				datos.executeReader();

				while (datos.Reader.Read())
				{
					cantidadPedidosAbiertos = Convert.ToInt32(datos.Reader["CANTIDAD"]);
				}

			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				datos.closeConnection();
			}
		

			//Valida que el servicio no se pueda cerrar si hay pedidos abiertos
			if(cantidadPedidosAbiertos == 0)
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
		public bool CobrarServicio(int idServicio)
		{
			AccesoDB datos = new AccesoDB();

			//Validar que el servicio esté cerrado con la fecha de cierre.
			//Validar que cobrado esté en false
			try
			{
				datos.setQuery($"UPDATE SERVICIO SET {ColumnasDB.Servicio.Cobrado} = 1 " +
					$"WHERE {ColumnasDB.Servicio.Id} = {idServicio} " +
					$"AND {ColumnasDB.Servicio.Cierre} IS NOT NULL " +
					$"AND {ColumnasDB.Servicio.Cobrado} = 0 ");

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
