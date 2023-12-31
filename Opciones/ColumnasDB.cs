﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
            public const string Cocinero = "Cocinero";
        }

		public struct Usuario
		{
			public const string Id = "IdUsuario";
			public const string Nombres = "Nombres";
			public const string Apellidos = "Apellidos";
			public const string Mail = "Mail";
			public const string Pass = "Pass";
			public const string TipoUsuario = "TipoUsuario";
			public const string Activo = "Activo";
			public const string DB = "USUARIO";
			public const string Fechaalta = "Fechaalta";
			public const string Token = "token";

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

		public struct ProductoDD
		{
			public const string Fecha = "Fecha";
			public const string StockInicial = "StockInicial";
			public const string StockCierre = "StockCierre";
			public const string Valor = "Valor";
			public const string Nombre = "Nombre";
			public const string DB = "ProductosPorDia_Menu";
        }

		public struct MesasPorDia
		{
			public const string Id = "IdMesaPorDia";
			public const string IdMesa = "IdMesa";
			public const string IdMesero = "IdMesero";
			public const string IdMeseroPorDia = "IdMeseroPorDia";
			public const string Fecha = "Fecha";
			public const string Apertura = "Apertura";
			public const string Cierre = "Cierre";
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

        public struct Servicio
        {
            public const string Id = "IdServicio";
            public const string Apertura = "Apertura";
            public const string Cierre = "Cierre";
            public const string Fecha = "Fecha";
            public const string Cobrado = "Cobrado";
            public const string IdMesero = "IdMesero";
            public const string IdMesa = "IdMesaPorDia";
            public const string DB = "Servicio";
        }

        public struct Pedido
		{
            public const string DB = "Pedido";
            public const string Id = "IdPedido";
            public const string IdServicio = "IdServicio";
            public const string IdEstado = "IdEstado";
        }

        public struct ProductoPorPedido
		{
            public const string DB = "Producto_X_Pedido";
            public const string Id = "IdProductoPorPedido";
            public const string IdPedido = "IdPedido";
            public const string IdProductoPorDia = "IdProductoPorDia";
            public const string Fecha = "Fecha";
            public const string Cantidad = "Cantidad";
            public const string Valor = "Valor";
        }

        public struct EstadosxPedido
        {
            public const string DB = "ESTADO_X_PEDIDO";
			public const string IdPedido = "IdPedido";
			public const string IdEstado = "IdEstado";
			public const string FechaActualizacion = "FechaActualizacion";

        }

        public struct Estados
        {
            public const string DB = "Estadopedido";
			public const string Id = "IdEstado";
			public const string Descripcion = "Descripcion";
		
            
		}

	}
}
