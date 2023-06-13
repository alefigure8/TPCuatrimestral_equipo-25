﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opciones
{
	public static class ColumnasDB
	{
		public struct TipoUsuario
		{
			public const string Id = "IdTipoUsuario";
			public const string Descripcion = "Descripcion";
			public const string DB = "TIPOUSUARIO";
			public const string Admin = "Admin";
			public const string Gerente = "Gerente";
			public const string Mesero = "Mesero";

		}

		public struct Usuario
		{
			public const string Id = "IdUsuario";
			public const string Nombres = "Nombres";
			public const string Apellidos = "Apellidos";
			public const string Mail = "Mail";
			public const string Pass = "Pass";
			public const string TipoUsuario = "TipoUsuario";
			public const string DB = "USUARIO";
		}
	}
}