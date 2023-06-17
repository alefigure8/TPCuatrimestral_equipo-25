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
		public DateTime Fecha { get; set; }
		public DateTime Ingreso { get; set; }
		public DateTime Salida { get; set; }
	}
}
