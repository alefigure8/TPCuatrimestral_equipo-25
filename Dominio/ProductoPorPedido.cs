using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
   public class ProductoPorPedido
    {
        public ProductoPorPedido()
              {
            Productodeldia = new ProductoDelDia();
            Valor = 0;
        }

        public ProductoDelDia Productodeldia { get; set; }
        public int Cantidad { get; set; }
        
        public DateTime HoraListo { get; set; }

        public decimal Valor { get; set; }
    
    }
}
