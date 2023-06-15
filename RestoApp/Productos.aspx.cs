using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;


namespace RestoApp
{
    public partial class Productos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarDropDownLists();
                ListarProductos();
            }
        }

        public void CargarDropDownLists()
        {
            CategoriaProductoNegocio CategoriaProductoNegocio = new CategoriaProductoNegocio();
            List<CategoriaProducto> ListaCategoriasProducto = new List<CategoriaProducto>();
            ListaCategoriasProducto = CategoriaProductoNegocio.Listar();

            DDPCategorias.Items.Add("Categorias");

            foreach (CategoriaProducto CPaux in ListaCategoriasProducto)
            {
                DDPCategorias.Items.Add(CPaux.Descripcion);
            }
        }

        public void ListarProductos() {
        ProductoNegocio productoNegocio = new ProductoNegocio();
        Session.Add("ProductosDisponibles", productoNegocio.ListarProductosDisponibles());
            ProductoRepetidor.DataSource = Session["ProductosDisponibles"];
            ProductoRepetidor.DataBind();
        }
    }
}