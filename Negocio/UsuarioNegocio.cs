using Dominio;
using System;
using Opciones;
using System.Collections.Generic;

namespace Negocio
{
	public class UsuarioNegocio
	{
		//Buscamos usuarios por mail y pass
		public Usuario BuscarUsuario(string mail, string pass)
		{

			Usuario usuario = new Usuario();
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery($"SELECT {ColumnasDB.Usuario.Id}, {ColumnasDB.Usuario.Nombres}, {ColumnasDB.Usuario.Apellidos}, {ColumnasDB.Usuario.Mail}, {ColumnasDB.Usuario.Pass}, {ColumnasDB.TipoUsuario.Descripcion} " +
					$"FROM {ColumnasDB.Usuario.DB} " +
					$"INNER JOIN {ColumnasDB.TipoUsuario.DB} ON {ColumnasDB.Usuario.TipoUsuario} = {ColumnasDB.TipoUsuario.Id} " +
					$"WHERE {ColumnasDB.Usuario.Mail} = '{mail}' AND {ColumnasDB.Usuario.Pass} = '{pass}'");
				datos.executeReader();

				while (datos.Reader.Read())
				{
					//ID
					usuario.Id = (Int32)datos.Reader[ColumnasDB.Usuario.Id];

					//NOMBRES
					if (datos.Reader[ColumnasDB.Usuario.Nombres] != null)
						usuario.Nombres = (string)datos.Reader[ColumnasDB.Usuario.Nombres];

					//APELLIDOS
					if (datos.Reader[ColumnasDB.Usuario.Apellidos] != null)
						usuario.Apellidos = (string)datos.Reader[ColumnasDB.Usuario.Apellidos];

					//MAIL
					if (datos.Reader[ColumnasDB.Usuario.Mail] != null)
						usuario.Mail = (string)datos.Reader[ColumnasDB.Usuario.Mail];

					//PASS
					if (datos.Reader[ColumnasDB.Usuario.Pass] != null)
						usuario.Password = (string)datos.Reader[ColumnasDB.Usuario.Pass];					
					
					//TIPO USUARIO
					if (datos.Reader[ColumnasDB.TipoUsuario.Descripcion] != null)
						usuario.Tipo = (string)datos.Reader[ColumnasDB.TipoUsuario.Descripcion];
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

			return String.IsNullOrEmpty(usuario.Mail) ? null : usuario;
		}

		// Listar los usuarios por meseros
		public List<Usuario> ListarMeseros()
		{
			List<Usuario> meseros = new List<Usuario>();
			AccesoDB datos = new AccesoDB();

			try
			{
				int IDTIPO = 3;
				datos.setQuery($"SELECT {ColumnasDB.Usuario.Id}, {ColumnasDB.Usuario.Nombres}, {ColumnasDB.Usuario.Apellidos}, {ColumnasDB.Usuario.Mail}, {ColumnasDB.Usuario.Pass}, {ColumnasDB.TipoUsuario.Descripcion} " +
					$"FROM {ColumnasDB.Usuario.DB} " +
					$"INNER JOIN {ColumnasDB.TipoUsuario.DB} ON {ColumnasDB.Usuario.TipoUsuario} = {ColumnasDB.TipoUsuario.Id} " +
					$"WHERE {ColumnasDB.Usuario.TipoUsuario} = {IDTIPO}");
				datos.executeReader();

				while (datos.Reader.Read())
				{
					Usuario usuario = new Usuario();
					
					//ID
					usuario.Id = (Int32)datos.Reader[ColumnasDB.Usuario.Id];

					//NOMBRES
					if (datos.Reader[ColumnasDB.Usuario.Nombres] != null)
						usuario.Nombres = (string)datos.Reader[ColumnasDB.Usuario.Nombres];

					//APELLIDOS
					if (datos.Reader[ColumnasDB.Usuario.Apellidos] != null)
						usuario.Apellidos = (string)datos.Reader[ColumnasDB.Usuario.Apellidos];

					//MAIL
					if (datos.Reader[ColumnasDB.Usuario.Mail] != null)
						usuario.Mail = (string)datos.Reader[ColumnasDB.Usuario.Mail];

					//PASS
					if (datos.Reader[ColumnasDB.Usuario.Pass] != null)
						usuario.Password = (string)datos.Reader[ColumnasDB.Usuario.Pass];
					
					//TIPO USUARIO
					if (datos.Reader[ColumnasDB.TipoUsuario.Descripcion] != null)
						usuario.Tipo = (string)datos.Reader[ColumnasDB.TipoUsuario.Descripcion];

					meseros.Add(usuario);
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

			return meseros.Count == 0 ? null : meseros;

		}
	}
}
