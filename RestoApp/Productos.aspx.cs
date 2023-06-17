using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Helper;
using Negocio;
using Opciones;
using System.Data;
using System.Web.UI.HtmlControls;

namespace RestoApp
{
    public partial class Productos : System.Web.UI.Page
    {
        public Usuario usuario { get; set; }
        public List<CategoriaProducto> ListaCategoriasProducto;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];

            if (!IsPostBack)
            {
                IniciarDDL();
                ListarProductos();
            }

            if (AutentificacionUsuario.esGerente(usuario))
            {
               
            }
            if (AutentificacionUsuario.esMesero(usuario))
            {
                NoEsGerente();
            }



            }

            protected void IniciarDDL()
        {
            CargarDDLEstado();
            CargarDDLCategorias();
            CargarDDLValor();
            CargarDDLStock();

            CargarAtributos();
        }

        public void CargarDDLEstado()
        {
            DDLEstado.Items.Add("Estado");
            DDLEstado.Items.Add("Activo");
            DDLEstado.Items.Add("Inactivo");
        }

        public void CargarDDLCategorias()
        {
            CategoriaProductoNegocio CategoriaProductoNegocio = new CategoriaProductoNegocio();
            ListaCategoriasProducto = CategoriaProductoNegocio.Listar();
            DDLCategorias.Items.Add("Categorias");
            foreach (CategoriaProducto CPaux in ListaCategoriasProducto)
            {
                DDLCategorias.Items.Add(CPaux.Descripcion);
            }
        }

        public void CargarDDLValor()
        {
            DDLPrecios.Items.Add("Ordernar por Valor");
            DDLPrecios.Items.Add("Valor ascendiente");
            DDLPrecios.Items.Add("Valor descendiente");
        }

        public void CargarDDLStock()
        {
            DDLStock.Items.Add("Ordernar por Stock");
            DDLStock.Items.Add("Stock ascendiente");
            DDLStock.Items.Add("Stock descendiente");
        }

        public void CargarAtributos()
        {
            CheckBoxAtributos.Items.Add(ColumnasDB.Producto.AptoVegano);
            CheckBoxAtributos.Items.Add(ColumnasDB.Producto.AptoCeliaco);
            CheckBoxAtributos.Items.Add(ColumnasDB.Producto.Alcohol);
        }

        public void ListarProductos()
        {
            ProductoNegocio productoNegocio = new ProductoNegocio();
            Session.Add("ProductosDisponibles", productoNegocio.ListarProductos());
            GVProductos.DataSource = Session["ProductosDisponibles"];
            GVProductos.DataBind();
        }

        public void NoEsGerente()
        {
            DDLEstado.Visible = false;
            PanelValor.Visible = false;
            PanelStock.Visible = false;

        }


    }
}