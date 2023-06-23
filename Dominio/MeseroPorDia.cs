using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
	public class MeseroPorDia
	{
        public int Id { get; set; }
        public int IdMesero { get; set; }
		public string Nombres { get; set; }
		public string Apellidos { get; set; }
		public int MesasAsignadas { get; set; }
		public int MesasConPedidos { get; set; }
		public DateTime Fecha { get; set; }
		public TimeSpan Ingreso { get; set; }
		public TimeSpan Salida { get; set; }

        public MeseroPorDia()
        {
			MesasAsignadas = 0;
			MesasConPedidos = 0;
		}
    }
}
