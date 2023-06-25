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
                ListaProductosDisponibles();
                ListarProductosDelDia();
            }
            else if (!IsPostBack)
            {
                ListarMenu();
            }

        }

        //Lista de productos que se pueden agregar al menu del dia porque estan activos
        protected void ListaProductosDisponibles()
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

        // Lista Productos del día
        protected void ListarProductosDelDia()
        {
            ProductoNegocio ProductoNegocio = new ProductoNegocio();
            Session.Add("ListaProductosDelDia", ProductoNegocio.ListarProductosDelDia());
            ProductoDelDiaRepetidor.DataSource = Session["ListaProductosDelDia"];
            for (int i = 0; i < ProductoDelDiaRepetidor.Items.Count; i++)
            {
                ProductoDelDia PDDaux = (ProductoDelDia)ProductoDelDiaRepetidor.Items[i].DataItem;
                //BtnDesactivar.Text = PDDaux.Activo == true ? "Cerrar" : "Abrir";
            }
            ProductoDelDiaRepetidor.DataBind();
        }

        // Lista de productos en menú actual
        protected void ListarMenu()
        {
            ProductoNegocio ProductoNegocio = new ProductoNegocio();
            Session.Add("ListaMenu", ProductoNegocio.ListarProductosDelDia());
            List<ProductoDelDia> ListaProductosDisponibles = (List<ProductoDelDia>)Session["ListaMenu"];
            ListaProductosDisponibles.RemoveAll(x => x.Activo == false);
            MenuRepetidor.DataSource = Session["ListaMenu"];
            MenuRepetidor.DataBind();
        }

        protected void BtnAgregarAPDD_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Producto Paux = ((List<Producto>)Session["ListaProductos"]).Find(x => x.Id == int.Parse(button.CommandArgument));
            ProductoDelDia PDDAux = new ProductoDelDia(Paux);
            ProductoNegocio PNaux = new ProductoNegocio();
            PNaux.AgregarProductoDD(PDDAux);
            ListarProductosDelDia();
        }

        protected void BtnDesactivar_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if(button.Text.ToLower() == "cerrar")
            {

            }
            else if(button.Text.ToLower() == "abrir")
            {

            }
        }
    }
}