using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
	public class MesaPorDia
	{
        public int Id { get; set; }
        public int? Mesero { get; set; }
        public int Mesa { get; set; }
		public DateTime Fecha { get; set; }
	}
}
