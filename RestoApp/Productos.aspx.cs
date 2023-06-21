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
using System.Reflection;

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

            modalDDLEstado.Items.Add("Estado");
            modalDDLEstado.Items.Add("Activo");
            modalDDLEstado.Items.Add("Inactivo");

        }

        public void CargarDDLCategorias()
        {
            
            DDLCategorias.Items.Add("Categorias");
            modalDDLCategorias.Items.Add("Categorias");

            foreach (CategoriaProducto CPaux in ListaCategoriasProducto)
            {
                DDLCategorias.Items.Add(CPaux.Descripcion);
                modalDDLCategorias.Items.Add(CPaux.Descripcion);

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
            CheckBoxAtributos.Items.Add("Vegano");
            CheckBoxAtributos.Items.Add("Celiaco");
            CheckBoxAtributos.Items.Add("Alcohol");

            modalCheckBoxAtributos.Items.Add("Vegano");
            modalCheckBoxAtributos.Items.Add("Celiaco");
            modalCheckBoxAtributos.Items.Add("Alcohol");
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

                resultado = (bool.Parse(e.Row.Cells[7].Text)) == false ? "Inactivo" : "Activo";
                e.Row.Cells[7].Text = resultado;

            }





        }

        protected void NewListaFiltrada()
        {
            ListaFiltrada = new List<Producto>();
        }

        protected void DDLEstado_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DDLEstado.SelectedIndex != 0)
            {
                ValidarListaFiltrada();

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
            else
            {
                ListarProductos();
            }



        }

        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            ListarProductos();
        }

        protected void DDLCategorias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(DDLCategorias.SelectedIndex!=0)
            {
                ValidarListaFiltrada();
                switch (DDLCategorias.SelectedIndex)
                {
                    case 1:
                        {
                            foreach (Producto PAux in (List<Producto>)Session["ListaProductos"])
                            {
                                if (!(PAux.Categoria == 1))
                                {
                                    ListaFiltrada.Remove(PAux);
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            foreach (Producto PAux in (List<Producto>)Session["ListaProductos"])
                            {

                                if (PAux.Categoria == 2)
                                {
                                    ListaFiltrada.Remove(PAux);
                                }
                            }
                            break;
                        }
                }
                Session["ListaFiltrada"] = ListaFiltrada;
                GVProductos.DataSource = Session["ListaFiltrada"];
                GVProductos.DataBind();

            }
            else
            {
                ListarProductos();
            }

        }

        protected void ValidarListaFiltrada()
        {
            if (ListaFiltrada == null)
            {
                NewListaFiltrada();
            }

        }

        protected void AplicarFiltro(object sender, EventArgs e)
        {
       
        }

        protected void LBtnNuevoPlato_Click(object sender, EventArgs e)
        {

        }

      
        protected void GuardarNuevoProducto_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(NuevoProductoNombre.Text) &&
           !string.IsNullOrWhiteSpace(NuevoProductoDescripcion.Text) &&
           !string.IsNullOrWhiteSpace(NuevoProductoValor.Text) &&
           modalDDLCategorias.SelectedIndex != 0 &&
           modalDDLEstado.SelectedIndex != 0 &&
           !string.IsNullOrWhiteSpace(NuevoProductoStock.Text))
            {

                Producto NuevoProducto = new Producto();
                NuevoProducto.Nombre = NuevoProductoNombre.Text;
                NuevoProducto.Descripcion = NuevoProductoDescripcion.Text;
                NuevoProducto.Valor = decimal.Parse(NuevoProductoValor.Text);
                NuevoProducto.Categoria = modalDDLCategorias.SelectedIndex;
                bool estado = modalDDLEstado.SelectedIndex-1 == 0 ? false : true;
                NuevoProducto.Activo = estado;
                
                 Convert.ToBoolean(modalDDLEstado.SelectedIndex);

                if (modalCheckBoxAtributos.SelectedItem.Value == "Vegano")
                {
                    NuevoProducto.AptoVegano = true;
                }
                if (modalCheckBoxAtributos.SelectedItem.Value == "Celiaco")
                {
                    NuevoProducto.AptoCeliaco = true;
                }
                if (modalCheckBoxAtributos.SelectedItem.Value == "Alcohol")
                {
                    NuevoProducto.Alcohol = true;
                }
                NuevoProducto.TiempoCoccion =  TimeSpan.Parse(NuevoProductoTiempoCoccion.Text);
                NuevoProducto.Stock = int.Parse(NuevoProductoStock.Text);

                ProductoNegocio PNaux = new ProductoNegocio();
                PNaux.NuevoProducto(NuevoProducto);



            }
        }

        public void EliminarProducto(object sender, System.EventArgs e)
        {
            Button button = sender as Button;
            ProductoNegocio PNaux = new ProductoNegocio();
            PNaux.EliminarProducto(int.Parse(button.CommandArgument));
        }
    }
}