using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
	public class Ticket
	{
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
		public TimeSpan Cierre { get; set; }
		public int Mesa { get; set; }
		public int IdMesero { get; set; }
		public bool Cobrado { get; set; }
		public List<TicketDetalle> Detalle { get; set; }
	}

	public class TicketDetalle
	{
        public int Id { get; set; }
		public string Descripcion { get; set; }
		public int Cantidad { get; set; }
		public decimal Precio { get; set; }
	}
}
