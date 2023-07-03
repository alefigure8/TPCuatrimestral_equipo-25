using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
	public class Servicio
	{
        public int Id { get; set; }
        public int Mesa { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Apertura { get; set; }
        public TimeSpan? Cierre { get; set; }
        public bool Cobrado { get; set; }

        public Servicio()
        {
            Cierre = null;
			Cobrado = false;
		}
    }
}
