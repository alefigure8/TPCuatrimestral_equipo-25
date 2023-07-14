using Dominio;
using System;
using Opciones;
using System.Collections.Generic;


namespace Negocio
{
	public class UsuarioNegocio
	{
        public List<Usuario> Listar()
        {

            List<Usuario> listausuarios = new List<Usuario>();
            AccesoDB datos = new AccesoDB();

            try
            {
				datos.setQuery($"SELECT {ColumnasDB.Usuario.Id}, {ColumnasDB.Usuario.Nombres}, {ColumnasDB.Usuario.Apellidos}, {ColumnasDB.Usuario.Mail}, {ColumnasDB.Usuario.Pass}, {ColumnasDB.TipoUsuario.Descripcion} " +
					$"FROM {ColumnasDB.Usuario.DB} " +
					$"INNER JOIN {ColumnasDB.TipoUsuario.DB} ON {ColumnasDB.Usuario.TipoUsuario} = {ColumnasDB.TipoUsuario.Id} " +
					$"WHERE {ColumnasDB.Usuario.Activo} = 1 "); 
				datos.executeReader();
                    

                while (datos.Reader.Read())
                {
                    Usuario usuarioaux = new Usuario();
                    //ID
                    usuarioaux.Id = (Int32)datos.Reader[ColumnasDB.Usuario.Id];

                    //NOMBRES
                    if (datos.Reader[ColumnasDB.Usuario.Nombres] != null)
                        usuarioaux.Nombres = (string)datos.Reader[ColumnasDB.Usuario.Nombres];

                    //APELLIDOS
                    if (datos.Reader[ColumnasDB.Usuario.Apellidos] != null)
                        usuarioaux.Apellidos = (string)datos.Reader[ColumnasDB.Usuario.Apellidos];

                    //MAIL
                    if (datos.Reader[ColumnasDB.Usuario.Mail] != null)
                        usuarioaux.Mail = (string)datos.Reader[ColumnasDB.Usuario.Mail];

                    //PASS
                    if (datos.Reader[ColumnasDB.Usuario.Pass] != null)
                        usuarioaux.Password = (string)datos.Reader[ColumnasDB.Usuario.Pass];


					//TIPO USUARIO
					if (datos.Reader[ColumnasDB.TipoUsuario.Descripcion] != null)
						usuarioaux.Tipo = (string)datos.Reader[ColumnasDB.TipoUsuario.Descripcion];
					
					listausuarios.Add(usuarioaux);
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

            return listausuarios;
        }

		//Buscamos meseros por pass y mail
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

		//Buscar mail
		public int BuscarUsuarioPorMail(string mail)
		{

			int existeUsuario = 0;
			AccesoDB datos = new AccesoDB();

			try
			{
				datos.setQuery(
					$"SELECT 1 AS existeUsuario " +
					$"FROM {ColumnasDB.Usuario.DB} " +
					$"INNER JOIN {ColumnasDB.TipoUsuario.DB} ON {ColumnasDB.Usuario.TipoUsuario} = {ColumnasDB.TipoUsuario.Id} " +
					$"WHERE {ColumnasDB.Usuario.Mail} = '{mail}' " +
					$"AND {ColumnasDB.Usuario.Activo} = 'true'"
					);
				datos.executeReader();

				while (datos.Reader.Read())
				{
					//si es cero el usuario no existe, si es uno existe
					existeUsuario = (Int32)datos.Reader["existeUsuario"];
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

			return existeUsuario;
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
        
		public bool BajalogicaUsuario(int id)
		{

            Usuario usuario = new Usuario();
            AccesoDB datos = new AccesoDB();

            try
            {
                
				datos.setQuery($"UPDATE {ColumnasDB.Usuario.DB} set {ColumnasDB.Usuario.Activo} = 0 WHERE {ColumnasDB.Usuario.Id} = {id}");                   
      

                if (datos.executeNonQuery())
                {
                    datos.closeConnection();
                    return true;
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
			return false;
        }


		public int convertidordetipousuario(string tipo)
		{
            int id = 0;
            if (tipo == ColumnasDB.TipoUsuario.Mesero)
			{
                id = 3;
            }
            else if (tipo == ColumnasDB.TipoUsuario.Gerente)
			{
                id = 1;
            }
			else if (tipo == ColumnasDB.TipoUsuario.Cocinero)
			{
				id = 4;

			}
            return id;
        }

		public bool Agregarusuario(Usuario nuevousuario)
		{
            AccesoDB datos = new AccesoDB();
			int tipo=convertidordetipousuario(nuevousuario.Tipo);
			


            try
            {
				               
				datos.setQuery($"INSERT INTO " +
                   $"{ColumnasDB.Usuario.DB} (" +
                   $"{ColumnasDB.Usuario.Nombres}," +
                   $"{ColumnasDB.Usuario.Apellidos}," +
                   $"{ColumnasDB.Usuario.Mail}, " +
                   $"{ColumnasDB.Usuario.Pass}, " +
                   $"{ColumnasDB.Usuario.Activo}, " +
                   $"{ColumnasDB.Usuario.TipoUsuario}) " +
                   $"VALUES ('{nuevousuario.Nombres}', " +
                    $"'{nuevousuario.Apellidos}', " +
                    $"'{nuevousuario.Mail}', " +
                    $"'{nuevousuario.Password}', " +
      //              $"GETDATE(), " +
                    $"1 ," +
                    $"'{tipo}')");

                if (datos.executeNonQuery())
                {
                    datos.closeConnection();
                    return true;
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
            return false;



        }

		public void Modificarusuario(Usuario usuariomodificado)
		{
            AccesoDB datos = new AccesoDB();

            try            {

                datos.setQuery($"UPDATE {ColumnasDB.Usuario.DB} " +
					$"set {ColumnasDB.Usuario.Nombres} = '{usuariomodificado.Nombres}', " +
					$"{ColumnasDB.Usuario.Apellidos} = '{usuariomodificado.Apellidos}', " +
					$"{ColumnasDB.Usuario.Mail} = '{usuariomodificado.Mail}', " +
					$"{ColumnasDB.Usuario.Pass} = '{usuariomodificado.Password}', " +
					$"{ColumnasDB.Usuario.TipoUsuario} = '{convertidordetipousuario(usuariomodificado.Tipo)}' " +
					$" WHERE {ColumnasDB.Usuario.Id} = {usuariomodificado.Id}");


                if (datos.executeNonQuery())
                {
                    datos.closeConnection();
                  
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


        }
    }
}
