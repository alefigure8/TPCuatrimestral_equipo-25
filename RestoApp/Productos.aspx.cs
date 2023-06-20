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
        public List<Producto> ListaProductos;
        public List<Producto> ListaFiltrada;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];



            CategoriaProductoNegocio CategoriaProductoNegocio = new CategoriaProductoNegocio();
            ListaCategoriasProducto = CategoriaProductoNegocio.Listar();

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
            Session.Add("ListaProductos", productoNegocio.ListarProductos());
            GVProductos.DataSource = Session["ListaProductos"];
            GVProductos.DataBind();
        }

        public void NoEsGerente()
        {
            DDLEstado.Visible = false;
            PanelValor.Visible = false;
            PanelStock.Visible = false;

        }

        protected void GVProductos_DataBound(object sender, EventArgs e)
        {

        }

        protected void GVProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch ((int.Parse(e.Row.Cells[1].Text)))
                {
                    case 1:
                        {
                            e.Row.Cells[1].Text = ListaCategoriasProducto[0].Descripcion;
                            break;
                        }
                    case 2:
                        {
                            e.Row.Cells[1].Text = ListaCategoriasProducto[1].Descripcion;
                            break;
                        }
                }



                string resultado = (bool.Parse(e.Row.Cells[3].Text)) == true ? "✔" : "✖";
                e.Row.Cells[3].Text = resultado;

                resultado = (bool.Parse(e.Row.Cells[4].Text)) == true ? "✔" : "✖";
                e.Row.Cells[4].Text = resultado;

                resultado = (bool.Parse(e.Row.Cells[5].Text)) == true ? "✔" : "✖";
                e.Row.Cells[5].Text = resultado;

                resultado = (bool.Parse(e.Row.Cells[7].Text)) == true ? "Activo" : "Inactivo";
                e.Row.Cells[7].Text = resultado;

            }





        }

        protected void DDLEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListaFiltrada = new List<Producto>();

            switch (DDLEstado.SelectedIndex)
            {
               

                case 1:
                    {
                        foreach (Producto PAux in (List<Producto>)Session["ListaProductos"])
                        {
                            if (PAux.Activo == true)
                            {
                               ListaFiltrada.Add(PAux);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        foreach (Producto PAux in (List<Producto>)Session["ListaProductos"])
                        {
                            
                            if (PAux.Activo == false)
                            {
                                ListaFiltrada.Add(PAux);
                            }
                        }
                        break;
                    }
            }

            Session["ListaFiltrada"] = ListaFiltrada;
            GVProductos.DataSource = Session["ListaFiltrada"];
            GVProductos.DataBind();



        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            ListarProductos();
        }
    }
}