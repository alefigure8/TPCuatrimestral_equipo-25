using System.Collections.Generic;
using System.Web;
using Dominio;
using Opciones;

namespace Helper
{
	public class Session
	{

		//MESAS

		//Listado de Mesas
		public static List<Mesa> GetMesas()
		{
			return (List<Mesa>)HttpContext.Current.Session[Configuracion.Session.Mesas];
		}

		public static void SetMesas(List<Mesa> mesas)
		{
			HttpContext.Current.Session[Configuracion.Session.Mesas] = mesas;
		}


		//Listado de Mesas Asignadas
		public static List<MesaPorDia> GetMesasAsignadas()
		{
			return (List<MesaPorDia>)HttpContext.Current.Session[Configuracion.Session.MesasAsignada];
		}

		public static void SetMesasAsignadas(List<MesaPorDia> mesasAsignadas)
		{
			HttpContext.Current.Session[Configuracion.Session.MesasAsignada] = mesasAsignadas;
		}


		//MESEROS
		
		//Mesero
		public static MeseroPorDia GetMeseroPorDia()
		{
			return (MeseroPorDia)HttpContext.Current.Session[Configuracion.Session.MeseroPorDia];
		}

		public static void SetMeseroPorDia(MeseroPorDia meseroPorDia)
		{
			HttpContext.Current.Session[Configuracion.Session.MeseroPorDia] = meseroPorDia;
		}


		//Meseros Asignados
		public static List<MeseroPorDia> GetMeserosAsignados()
		{
			return (List<MeseroPorDia>)HttpContext.Current.Session[Configuracion.Session.MeserosAsignados];
		}

		public static void SetMeserosAsignados(List<MeseroPorDia> meserosPorDias)
		{
			HttpContext.Current.Session[Configuracion.Session.MeserosAsignados] = meserosPorDias;
		}


		//Meseros no asignados
		public static List<MeseroPorDia> GetMeserosNoAsignados()
		{
			return (List<MeseroPorDia>)HttpContext.Current.Session[Configuracion.Session.MeserosNoAsignados];
		}

		public static void SetMeserosNoAsignados(List<MeseroPorDia> meseroPorDias)
		{
			HttpContext.Current.Session[Configuracion.Session.MeserosNoAsignados] = meseroPorDias;
		}
		

		//USUARIOS
		public static Usuario GetUsuario()
		{
			return (Usuario)HttpContext.Current.Session[Configuracion.Session.Usuario];
		}
	}
}
