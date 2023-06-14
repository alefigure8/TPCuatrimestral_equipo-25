using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ProductoDelDia : Producto
    { 
        public DateTime Fecha {  get; set; }
        public int StockInicio { get; set; }
        public int StockCierre { get; set; }
    }
}
