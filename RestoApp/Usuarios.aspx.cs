using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RestoApp
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDgv();
        }

        protected void CargarDgv()
        {
            if (Session["carrito"] != null)
            {


                ListaCarrito = (List<Carrito>)Session["carrito"];
                dgvCarrito.DataSource = ListaCarrito;
                dgvCarrito.DataBind();

                CargarTotales();
            }
        }
    }
}