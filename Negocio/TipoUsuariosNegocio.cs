using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;
using Negocio;
using Opciones;

namespace Negocio
{
	public class TipoUsuariosNegocio
	{

		public TipoUsuarios ListarPorId(int id)
		{
			TipoUsuarios tipoUsuario = new TipoUsuarios();
			AccesoDB datos = new AccesoDB();

			try
			{
			datos.setQuery($"SELECT {ColumnasDB.TipoUsuario.Id}, {ColumnasDB.TipoUsuario.Descripcion} FROM {ColumnasDB.TipoUsuario.DB} WHERE {ColumnasDB.TipoUsuario.Id} = '{id}'");
			datos.executeReader();

				if (datos.Reader.Read())
				{
					//ID
					tipoUsuario.Id = (Int32)datos.Reader[ColumnasDB.TipoUsuario.Id];

					//Descripcion
					if (datos.Reader[ColumnasDB.TipoUsuario.Descripcion] != null)
						tipoUsuario.Descripcion = (string)datos.Reader[ColumnasDB.TipoUsuario.Descripcion];
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

			return tipoUsuario;
		}
	}
}
