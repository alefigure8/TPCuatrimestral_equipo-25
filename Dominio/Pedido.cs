using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pedido
    {
        public int Id {set; get;}
        public int IdMesa { get; set; }
        public int IdMesero { get; set; }
        public List<ItemPedido> ListaItems { get; set; }
        public DateTime FechaApertura { get; set; }
        public DateTime FechaCierre { get; set; }
    }
}
