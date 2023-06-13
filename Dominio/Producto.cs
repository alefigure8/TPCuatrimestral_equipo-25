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
        public string Nombre { get; set; }
        public Categoria Categoria { get; set; }
        public string Descripcion { set; get; }
        public decimal Valor { set; get; }
        public bool AptoVegano { set; get; }
        public bool AptoCeliaco { set; get; }
        public bool Alcohol { set; get; }
        public int Stock { set; get; }
        public bool Activo { set; get; }

    }
}
