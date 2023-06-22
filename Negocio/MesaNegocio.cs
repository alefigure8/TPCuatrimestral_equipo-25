using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Opciones;

namespace Negocio
{
	public class MesaNegocio
	{
		public List<Mesa> Listar()
		{
			List<Mesa> mesas = new List<Mesa>();
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery($"SELECT {ColumnasDB.Mesa.Numero}, {ColumnasDB.Mesa.Capacidad}, {ColumnasDB.Mesa.Activo} FROM {ColumnasDB.Mesa.DB}");
				datos.executeReader();

				while (datos.Reader.Read())
				{
					Mesa auxMesa = new Mesa();
					//ID
					auxMesa.Numero = (Int32)datos.Reader[ColumnasDB.Mesa.Numero];

					//CAPACIDAD
					if (datos.Reader[ColumnasDB.Mesa.Capacidad] != null)
						auxMesa.Capacidad = (Int32)datos.Reader[ColumnasDB.Mesa.Capacidad];

					//ACTIVO
					if (datos.Reader[ColumnasDB.Mesa.Activo] != null)
						auxMesa.Activo = (bool)datos.Reader[ColumnasDB.Mesa.Activo];

					mesas.Add(auxMesa);
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

			return mesas;

		}

		public void ActivarMesasPorNumero(int numero, int activo)
		{
			AccesoDB datos = new AccesoDB();

			//En caso de false, verificar que la mesa no esté asignada

			try
			{
				datos.setQuery($"UPDATE {ColumnasDB.Mesa.DB} SET {ColumnasDB.Mesa.Activo} = {activo} WHERE {ColumnasDB.Mesa.Numero} = {numero}");
				datos.executeNonQuery();
			}
			catch (Exception Ex)
			{
				throw Ex;
			}
			finally
			{
				datos.closeConnection();
			}
		}

		public List<MeseroPorDia> ListaMeseroPorDia()
		{
			List<MeseroPorDia> mesas = new List<MeseroPorDia>();
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery($"SELECT {ColumnasDB.MeseroPorDia.Id}, {ColumnasDB.MeseroPorDia.IdMesero}, {ColumnasDB.MeseroPorDia.Fecha}, {ColumnasDB.MeseroPorDia.Ingreso}, {ColumnasDB.MeseroPorDia.Salida}, {ColumnasDB.Usuario.Nombres},{ColumnasDB.Usuario.Apellidos} " +
					$"FROM {ColumnasDB.MeseroPorDia.DB} " +
					$"INNER JOIN {ColumnasDB.Usuario.DB} " +
					$"ON {ColumnasDB.MeseroPorDia.IdMesero} = {ColumnasDB.Usuario.Id} " +
					$"WHERE {ColumnasDB.MeseroPorDia.Salida} = '00:00:00'");
				datos.executeReader();

				while (datos.Reader.Read())
				{
					MeseroPorDia auxMesero = new MeseroPorDia();
					//ID
					auxMesero.Id = (Int32)datos.Reader[ColumnasDB.MeseroPorDia.Id];

					//MESERO
					if (datos.Reader[ColumnasDB.MeseroPorDia.IdMesero] != null)
						auxMesero.IdMesero = (Int32)datos.Reader[ColumnasDB.MeseroPorDia.IdMesero];

					//NOMBRE
					if (datos.Reader[ColumnasDB.MeseroPorDia.Nombres] != null)
						auxMesero.Nombres = (string)datos.Reader[ColumnasDB.MeseroPorDia.Nombres];

					//APELLIDOS
					if (datos.Reader[ColumnasDB.MeseroPorDia.Apellidos] != null)
						auxMesero.Apellidos = (string)datos.Reader[ColumnasDB.MeseroPorDia.Apellidos];

					//FECHA
					if (datos.Reader[ColumnasDB.MeseroPorDia.Fecha] != null)
						auxMesero.Fecha = (DateTime)datos.Reader[ColumnasDB.MeseroPorDia.Fecha];

					//INGRESO
					if (datos.Reader[ColumnasDB.MeseroPorDia.Ingreso] != null)
						auxMesero.Ingreso  = (TimeSpan)datos.Reader[ColumnasDB.MeseroPorDia.Ingreso];

					//SALIDA
					if (datos.Reader[ColumnasDB.MeseroPorDia.Salida] != null)
						auxMesero.Salida = (TimeSpan)datos.Reader[ColumnasDB.MeseroPorDia.Salida];

					mesas.Add(auxMesero);
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

			return mesas;
		}

		public int CrearMeseroPorDia(MeseroPorDia meseroPorDia)
		{
			AccesoDB datos = new AccesoDB();
			int id = 0;
			
			if (meseroPorDia.Id == 0)
			{
				try
				{
					datos.setQuery($"INSERT INTO {ColumnasDB.MeseroPorDia.DB} ({ColumnasDB.MeseroPorDia.IdMesero}, {ColumnasDB.MeseroPorDia.Fecha}, {ColumnasDB.MeseroPorDia.Ingreso}, {ColumnasDB.MeseroPorDia.Salida}) " +
						$"VALUES ({meseroPorDia.IdMesero}, '{meseroPorDia.Fecha.ToString("yyyy-MM-dd")}', '{meseroPorDia.Ingreso}', '{meseroPorDia.Salida}')"
					+ "SELECT CAST(scope_identity() AS int)");
					id = datos.executeScalar();

				}
				catch (Exception Ex)
				{
					throw Ex;
				}
				finally
				{
					datos.closeConnection();
				}
			}

			return id;
		}

		public bool ModificarMeseroPorDia(int id, TimeSpan? salida = null)
		{
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery($"UPDATE {ColumnasDB.MeseroPorDia.DB} SET {ColumnasDB.MeseroPorDia.Salida} = '{salida}' WHERE {ColumnasDB.MeseroPorDia.Id} = {id}");
				return datos.executeNonQuery();
			}
			catch (Exception Ex)
			{
				return false;
				throw Ex;
			}
			finally
			{
				datos.closeConnection();
			}

		}

		//Listamos las mesas por dia
		public List<MesaPorDia> ListarMesaPorDia()
		{
			List<MesaPorDia> mesas = new List<MesaPorDia>();
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery($"SELECT MesaPD.{ColumnasDB.MesasPorDia.Id}, MeseroPD.{ColumnasDB.MeseroPorDia.IdMesero}, MesaPD.{ColumnasDB.MesasPorDia.IdMesa}, MesaPD.{ColumnasDB.MesasPorDia.IdMeseroPorDia} ,MesaPD.{ColumnasDB.MesasPorDia.Fecha}, MesaPD.{ColumnasDB.MesasPorDia.Apertura}, MesaPD.{ColumnasDB.MesasPorDia.Cierre} " +
					$"FROM {ColumnasDB.MesasPorDia.DB} MesaPD " +
					$"INNER JOIN {ColumnasDB.MeseroPorDia.DB} MeseroPD " +
					$" ON MesaPD.{ColumnasDB.MesasPorDia.IdMeseroPorDia} = MeseroPD.{ColumnasDB.MeseroPorDia.Id}");

				datos.executeReader();

				while (datos.Reader.Read())
				{
					MesaPorDia auxMesero = new MesaPorDia();
					//ID
					auxMesero.Id = (Int32)datos.Reader[ColumnasDB.MesasPorDia.Id];

					//MESERO
					object valorMesero = datos.Reader[ColumnasDB.MeseroPorDia.IdMesero];
					auxMesero.Mesero = DBNull.Value.Equals(valorMesero) ? (int?)null : Convert.ToInt32(valorMesero);

					//MESA
					if (datos.Reader[ColumnasDB.MesasPorDia.IdMesa] != null)
						auxMesero.Mesa = (Int32)datos.Reader[ColumnasDB.MesasPorDia.IdMesa];

					//MESERO POR DIA
					object valorMeseroPorDia = datos.Reader[ColumnasDB.MesasPorDia.IdMeseroPorDia];
					auxMesero.IDMeseroPorDia = DBNull.Value.Equals(valorMesero) ? (int?)null : Convert.ToInt32(valorMeseroPorDia);

					//FECHA
					if (datos.Reader[ColumnasDB.MesasPorDia.Fecha] != null)
						auxMesero.Fecha = (DateTime)datos.Reader[ColumnasDB.MesasPorDia.Fecha];

					//APERTURA
					if (datos.Reader[ColumnasDB.MesasPorDia.Apertura] != null)
						auxMesero.Apertura = (TimeSpan)datos.Reader[ColumnasDB.MesasPorDia.Apertura];

					//SALIDA
					object valorCierre = datos.Reader[ColumnasDB.MesasPorDia.Cierre];
					auxMesero.Cierre = DBNull.Value.Equals(valorCierre) ? (TimeSpan?)null : (TimeSpan)valorCierre; ;

					mesas.Add(auxMesero);
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

			return mesas;
		}

		public int CrearMesaPorDia(int mesero, int mesa, int idmeseropordia)
		{
			AccesoDB datos = new AccesoDB();
			
			int id = 0;
				
			bool estaCargadaLaMesa = false;
			bool estaActivaLaMesa = false;

			//Validar si el mismo servicio de mesero tiene la mesa
			List<MesaPorDia> mesasPorDia = this.ListarMesaPorDia().FindAll(m => m.Cierre == null);
			foreach (MesaPorDia item in mesasPorDia)
			{
				//Si el mesero con el mismo servicio tiene la mesa asignada y abierta no se puede crear
				if (item.Mesero == mesero && item.Mesa == mesa)
					estaCargadaLaMesa = true;

				//Si es la misma mesa pero su servicio está abierto no se puede crear
				else if(item.Mesa == mesa)
					estaCargadaLaMesa = true;
			}
			
			//Validar que la mesa este Activa
			List<Mesa> mesas = this.Listar();
			foreach(Mesa item in mesas)
			{
				if (item.Numero == mesa)
				{
					estaActivaLaMesa = item.Activo;
				}
			}

			if(!estaCargadaLaMesa && estaActivaLaMesa)
			{
				try
				{
					datos.setQuery($"INSERT INTO {ColumnasDB.MesasPorDia.DB} ({ColumnasDB.MesasPorDia.IdMesa}, {ColumnasDB.MesasPorDia.IdMesero}, {ColumnasDB.MesasPorDia.IdMeseroPorDia}, {ColumnasDB.MesasPorDia.Fecha}, {ColumnasDB.MesasPorDia.Apertura}) " +
					$"VALUES ({mesa}, {mesero}, {idmeseropordia}, '{DateTime.Now.ToString("yyyy - MM - dd")}', '{DateTime.Now.ToString("HH:mm:ss")}') "
					+ "SELECT CAST(scope_identity() AS int)");
					id = datos.executeScalar();
				}
				catch (Exception Ex)
				{
					throw Ex;
				}
				finally
				{
					datos.closeConnection();
				}

				return id;
			}

			return 0;
		}

		//Modificar MesaPorDia
		public bool ModificarMesaPorDia(int idMesaPorDia,int mesa, int mesero)
		{
			AccesoDB datos = new AccesoDB();

			bool estaCargadaLaMesa = false;

			//Validar que el mesero no tenga la misma mesa ya asignada
			List<MesaPorDia> mesasPorDia = this.ListarMesaPorDia().FindAll(m => m.Cierre == null);
			
			foreach (MesaPorDia item in mesasPorDia)
			{
				if (item.Mesero == mesero && item.Mesa == mesa)
					estaCargadaLaMesa = true;
			}

			try
			{
				if (estaCargadaLaMesa)
				{
					//Si no está cargado el mesero, lo cargamos
					datos.setQuery($"UPDATE {ColumnasDB.MesasPorDia.DB} SET {ColumnasDB.MesasPorDia.Cierre} = '{DateTime.Now.ToString("HH:mm:ss")}' WHERE {ColumnasDB.MesasPorDia.Id} = {idMesaPorDia}");
					return datos.executeNonQuery();
				}

				return false;
			}
			catch(Exception ex)
			{
				return false;
				throw ex;
			}
			finally
			{
				datos.closeConnection();
			}
		}

		//Listamos los ids de los meseros activos que tienen mesas abiertas
		public List<int> ListaIdMeserosActivosConMesasAbiertas()
		{
			AccesoDB datos = new AccesoDB();
			
			List<int> IDMeseros = new List<int>();

			try
			{
				datos.setQuery($"SELECT MeseroPD.{ColumnasDB.MeseroPorDia.Id} " +
					$"FROM {ColumnasDB.MeseroPorDia.DB} MeseroPD " +
					$"INNER JOIN {ColumnasDB.MesasPorDia.DB} MesaPD " +
					$" ON MesaPD.{ColumnasDB.MesasPorDia.IdMeseroPorDia} = MeseroPD.{ColumnasDB.MeseroPorDia.Id} " +
					$"WHERE MesaPD.{ColumnasDB.MesasPorDia.Cierre} IS NULL " +
					$"AND MeseroPD.{ColumnasDB.MeseroPorDia.Salida} = '00:00:00' ");

				datos.executeReader();
				
				while (datos.Reader.Read())
				{
					IDMeseros.Add(Convert.ToInt32(datos.Reader[ColumnasDB.MeseroPorDia.Id]));
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
			

			return IDMeseros;
			
		}
	}
}
	