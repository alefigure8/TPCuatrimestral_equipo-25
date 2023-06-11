using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
	static public class User
	{
		static public bool esUser(Usuarios user)
		{
			if (user == null)
				return false;

			return true;
		}

		static public bool esAdmin(Usuarios user)
		{
			if (user.perfil.perfil == "Admin")
				return true;

			return false;
		}
	}
}
