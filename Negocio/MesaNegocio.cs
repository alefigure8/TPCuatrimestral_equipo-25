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

					//NOMBRES
					if (datos.Reader[ColumnasDB.Mesa.Capacidad] != null)
						auxMesa.Capacidad = (Int32)datos.Reader[ColumnasDB.Mesa.Capacidad];

					//APELLIDOS
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
					$"ON {ColumnasDB.MeseroPorDia.IdMesero} = {ColumnasDB.Usuario.Id}");
				datos.executeReader();

				while (datos.Reader.Read())
				{
					MeseroPorDia auxMesero = new MeseroPorDia();
					//ID
					auxMesero.Id = (Int32)datos.Reader[ColumnasDB.MeseroPorDia.Id];

					//MESERO
					if (datos.Reader[ColumnasDB.MeseroPorDia.IdMesero] != null)
						auxMesero.IdMesero = (Int32)datos.Reader[ColumnasDB.MeseroPorDia.IdMesero];

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
	}
}
	