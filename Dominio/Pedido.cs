using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pedido
    {
        /*Pedido()
        {
            Estadopedido = new EstadoPedido();
            Estadopedido.Id = 1;
        }
        */
        public int Id { get; set; }
        public List<ProductoDelDia> Productosdeldia;
        public EstadoPedido Estadopedido        {    get; set;        }
    }
}
