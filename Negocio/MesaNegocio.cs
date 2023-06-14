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

		public void ActivarMesasPorNumero(int numero)
		{
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery($"UPDATE {ColumnasDB.Mesa.DB} SET {ColumnasDB.Mesa.Activo} = 1 WHERE {ColumnasDB.Mesa.Numero} = {numero}");
				datos.executeNonQuery();
			}
			catch(Exception Ex)
			{
				throw Ex;
			}
			finally
			{
				datos.closeConnection();
			}
		}
	}
}
