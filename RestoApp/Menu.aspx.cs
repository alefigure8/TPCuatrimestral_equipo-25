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
    public partial class Menu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProductoNegocio productoNegocio = new ProductoNegocio();
            Session.Add("ProductosDisponibles", productoNegocio.ListarProductosDisponibles());
            ProductoRepetidor.DataSource = Session["ProductosDisponibles"];
            ProductoRepetidor.DataBind();



        }
    }
}