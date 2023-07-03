using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
   public class Pedido
    {
        public int Id { get; set; }
        public List<ProductoDelDia> Productosdeldia;
        public Estadopedido Estadopedido { get; set;}

    }
}
