using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Producto
    {
        public int Id { get; set; }
        public CategoriaProducto Categoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Decimal Valor { get; set; }
        public bool AptoVegano { get; set; }
        public bool AptoCeliaco { get; set; }
        public bool Alcohol { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public DateTime TiempoCoccion { get; set; }
    }
}
