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

namespace RestoApp
{
    public partial class Menu : System.Web.UI.Page
    {
        public Usuario usuario { get; set; }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                this.Page.MasterPageFile = "~/Masters/Default.Master";
            else
                this.Page.MasterPageFile = "~/Masters/Main.master";

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];

            if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
            {
                CheckListaProductos();
            }
            else if (!IsPostBack)
            {
                ListarProductosDelDia();
            }

        }

        //Lista de productos que se pueden agregar al menu del dia porque estan activos
        protected void CheckListaProductos()
        {
            if (Session["ListaProductos"] == null)
            {
                ProductoNegocio productoNegocio = new ProductoNegocio();
                Session.Add("ListaProductos", productoNegocio.ListarProductos());
            }
            List<Producto> ListaProductosDisponibles = (List<Producto>)Session["ListaProductos"];
            ListaProductosDisponibles.RemoveAll(x => x.Activo == false);          
            ProductoRepetidor.DataSource = ListaProductosDisponibles;
            ProductoRepetidor.DataBind();
        }

        // Lista de productos del día completa
        protected void ListarProductosDelDia()
        {
            ProductoNegocio ProductoNegocio = new ProductoNegocio();
            Session.Add("ListaProductosDelDia", ProductoNegocio.ListarProductosDelDia());
            ProductoDelDiaRepetidor.DataSource = Session["ListaProductosDelDia"];
            ProductoDelDiaRepetidor.DataBind();
        }



    }
}