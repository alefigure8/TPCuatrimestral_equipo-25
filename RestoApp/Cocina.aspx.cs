using Dominio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestoApp
{
    public partial class Cocina : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Cargarpedidosprueba()
        {
            /*
            ProductoNegocio productoNegocio = new ProductoNegocio();
            productoDelDias = productoNegocio.ListarProductosDelDia();
            productoDelDias = productoDelDias.FindAll(x => x.Categoria == 1).ToList();
            foreach (ProductoDelDia aux in productoDelDias)
            {
             aux.Stock = 2;

            }
            */


            Pedido Pedido = new Pedido();
            Pedido.Productosdeldia = new List<ProductoDelDia>();
     

            ProductoDelDia productodelpedido0 = new ProductoDelDia();

            productodelpedido0.Id = 1;
            productodelpedido0.TiempoCoccion = TimeSpan.Parse("00:15:00");

            Pedido.Productosdeldia.Add(productodelpedido0);

            ProductoDelDia productodelpedido1 = new ProductoDelDia();

            productodelpedido1.Id = 2;
            productodelpedido1.TiempoCoccion = TimeSpan.Parse("00:20:00");

            Pedido.Productosdeldia.Add(productodelpedido1);

            Pedido.Id = 1;
            Pedido.Estadopedido.Id = 1;
            Pedido.Estadopedido.Descripcion = "Solicitado";
            Pedido.Estadopedido.fechaactualizacion = DateTime.Now;

            Session.Add("Pedido", Pedido);


        }

        public DataTable creardatatable()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Nombre", typeof(string));

            foreach (string horario in horariosmañana())
            {
                dataTable.Columns.Add(horario, typeof(string));

            }

            Pedido Pedido = new Pedido();
            Pedido.Productosdeldia = new List<ProductoDelDia>();
            Pedido.Estadopedido = new EstadoPedido();
            Pedido = (Pedido)Session["Pedido"];




            foreach (ProductoDelDia aux in Pedido.Productosdeldia)
            {
                dataTable.Rows.Add(aux.Id);
            }


            // Obtén el índice de la columna en la que deseas aplicar el estilo



            return dataTable;
        }

    }
}