using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opciones
{
	public class Configuracion
	{
		public struct Pagina
		{
			public const string Login = "Default.aspx";
			public const string Main = "Main.aspx";
			public const string Perfil = "Perfil.aspx";
			public const string Cocina = "Cocina.aspx";
		}

		public struct Session
		{
			public const string Usuario = "usuario";
			public const string Mesas = "Mesas";
			public const string MeseroPorDia = "MeseroPorDia";
			public const string MeserosNoAsignados = "MeserosNoAsignados";
			public const string MeserosAsignados = "MeserosAsignados";
			public const string Error = "error";
			public const string MesasAsignada = "MesasAsignadas";
			public const string NumeroMesaPedido = "NumeroMesaPedido";
			public const string Servicios = "Servicios";
			public const string Mensajes = "Mensajes";
		}

		public struct Rol
		{
			public const string Admin = "Admin";
			public const string Gerente = "Gerente";
			public const string Mesero = "Mesero";
		}
	}
}
