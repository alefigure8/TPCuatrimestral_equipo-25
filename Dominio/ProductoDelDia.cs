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

        public ProductoDelDia() { }

        public ProductoDelDia(Producto aux) {
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
            this.Fecha = DateTime.Today;
            this.StockInicio = aux.Stock;
            this.StockCierre = aux.Stock;
        }
    }
}
