using System;
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

		public struct Mesa
		{
			public const string Numero = "IdMesa";
			public const string Capacidad = "Capacidad";
			public const string Activo = "Activo";
			public const string DB = "Mesa";

		}

        public struct CategoriaProducto
        {
            public const string Id = "IdCategoriaProducto";
            public const string Descripcion = "Descripcion";
			public const string DB = "CategoriaProducto";
        }

		public struct Producto
		{
			public const string Id = "IdProducto";
            public const string Categoria = "CategoriaProducto";
            public const string Nombre = "Nombre";
			public const string Descripcion = "Descripcion";
			public const string Valor = "Valor";
			public const string AptoVegano = "AptoVegano";
			public const string AptoCeliaco = "AptoCeliaco";
			public const string Alcohol = "Alcohol";
			public const string Stock = "Stock";
			public const string Activo = "Activo";
			public const string TiempoCoccion = "TiempoCoccion";
			public const string DB = "Productos";
        }

		public struct MesasPorDia
		{
			public const string Id = "IdMesaPorDia";
			public const string IdMesa = "IdMesa";
			public const string IdMesero = "IdMesero";
			public const string Fecha = "Fecha";
			public const string DB = "MesasPorDia";
		}

		public struct MeseroPorDia
		{
			public const string Id = "IdMeseroPorDia";
			public const string IdMesero = "IdMesero";
			public const string Nombres = "Nombres";
			public const string Apellidos = "Apellidos";
			public const string Fecha = "Fecha";
			public const string Ingreso = "Ingreso";
			public const string Salida = "Salida";
			public const string DB = "MeseroPorDia";
		}
	}
}
