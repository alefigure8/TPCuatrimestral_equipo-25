
using Dominio;
using Opciones;

namespace Helper
{
	public class AutentificacionUsuario
	{
		static public bool esUser(Usuario user)
		{
			if (user != null)
				return true;

			return false;
		}

		static public bool esAdmin(Usuario user)
		{
			if (user?.Tipo == ColumnasDB.TipoUsuario.Admin)
				return true;

			return false;
		}

		static public bool esGerente(Usuario user)
		{
			if (user?.Tipo == ColumnasDB.TipoUsuario.Gerente)
				return true;

			return false;
		}

		static public bool esMesero(Usuario user)
		{
			if (user?.Tipo == ColumnasDB.TipoUsuario.Mesero)
				return true;

			return false;
		}
	}
}
