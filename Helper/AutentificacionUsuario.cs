
using Dominio;
using Opciones;

namespace Helper
{
	public class AutentificacionUsuario
	{
		static public bool esUser(Usuario user)
		{
			if (user == null)
				return false;

			return true;
		}

		static public bool esAdmin(Usuario user)
		{
			if (user.Tipo == ColumnasDB.TipoUsuario.Admin)
				return true;

			return false;
		}
	}
}
