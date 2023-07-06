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
        public int Categoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public Decimal Valor { get; set; }
        public bool AptoVegano { get; set; }
        public bool AptoCeliaco { get; set; }
        public bool Alcohol { get; set; }
        public int Stock { get; set; }
        public bool Activo { get; set; }
        public TimeSpan? TiempoCoccion { get; set; }

        public Producto()
        { }

            public Producto(ProductoDelDia aux)
        {
            this.Id = aux.Id;
            this.Categoria = aux.Categoria;
            this.Nombre = aux.Nombre;
            this.Descripcion = aux.Descripcion;
            this.Valor = aux.Valor;
            this.AptoVegano = aux.AptoVegano;
            this.AptoCeliaco = aux.AptoCeliaco;
            this.Activo = aux.Activo;
            this.Alcohol = aux.Alcohol;
            this.TiempoCoccion = aux.TiempoCoccion;
            this.Stock = aux.Stock;
        }
    }
}
