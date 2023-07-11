using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Pedido
    {
        public Pedido()
        {
           
        }
        
        public int Id { get; set; }
        public int IdServicio { get; set; }

        public List<ProductoPorPedido> Productossolicitados;
        public int Estado { get; set;}
        public string EstadoDescripcion { get; set; }

        public DateTime ultimaactualizacion;

        public DateTime ingresococina;
    }
}
