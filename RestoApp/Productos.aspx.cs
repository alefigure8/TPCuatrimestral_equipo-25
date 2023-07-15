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
        protected void Page_Load(object sender, EventArgs e)
        {

            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];

         
            if (!IsPostBack)
            {
                IniciarDDL();
                ListarProductos();
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

        public void ListarCategoriasProducto()
        {
            CategoriaProductoNegocio CategoriaProductoNegocio = new CategoriaProductoNegocio();
            ListaCategoriasProducto = CategoriaProductoNegocio.Listar();
        }

        public void CargarDDLEstado()
        {
            DDLEstado.Items.Add("Estado");
            DDLEstado.Items.Add("Activo");
            DDLEstado.Items.Add("Inactivo");

            modalDDLEstado.Items.Add("Estado");
            modalDDLEstado.Items.Add("Activo");
            modalDDLEstado.Items.Add("Inactivo");

            MPDDLEstado.Items.Add("Estado");
            MPDDLEstado.Items.Add("Activo");
            MPDDLEstado.Items.Add("Inactivo");

        }

        public void CargarDDLCategorias()
        {
            ListarCategoriasProducto();

            DDLCategorias.Items.Clear();
            modalDDLCategorias.Items.Clear();
            MPDDLCategoria.Items.Clear();

            DDLCategorias.Items.Add("Categorias");
            modalDDLCategorias.Items.Add("Categorias");
            MPDDLCategoria.Items.Add("Categorias");
            foreach (CategoriaProducto CPaux in ListaCategoriasProducto)
            {
                DDLCategorias.Items.Add(CPaux.Descripcion);
                modalDDLCategorias.Items.Add(CPaux.Descripcion);
                MPDDLCategoria.Items.Add(CPaux.Descripcion);

            }

            CategoriasRepetidor.DataSource = ListaCategoriasProducto;
            CategoriasRepetidor.DataBind();

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

            MPCheckBoxAtributos.Items.Add("Vegano");
            MPCheckBoxAtributos.Items.Add("Celiaco");
            MPCheckBoxAtributos.Items.Add("Alcohol");
        }




        public void ListarProductos()
        {
            if (((List<Producto>)Session["ListaProductos"]) == null)
            {
                ProductoNegocio productoNegocio = new ProductoNegocio();
                Session.Add("ListaProductos", productoNegocio.ListarProductos());
            }
            GVProductos.DataSource = Session["ListaProductos"];
            GVProductos.DataBind();
        }

        public void ActualizarGV(List<Producto> ListaFiltrada)
        {
            
            GVProductos.DataSource = ListaFiltrada;
            GVProductos.DataBind();
        }

        protected void GVProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ListarCategoriasProducto();
                e.Row.Cells[1].Text = ListaCategoriasProducto.Find(x => x.Id == int.Parse(e.Row.Cells[1].Text)).Descripcion;

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


        protected void btnLimpiarFiltro_Click(object sender, EventArgs e)
        {
            ListarProductos();
            LimpiarListaFiltrada();
            LimpiarBotonesFiltros();
        }

        protected void LimpiarListaFiltrada()
        {

            Session["ListaFiltrada"] = null;
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
                ListarCategoriasProducto();
                NuevoProducto.Categoria = ListaCategoriasProducto.Find(x => x.Descripcion.ToLower() == modalDDLCategorias.SelectedValue.ToLower()).Id;
                bool estado = modalDDLEstado.SelectedIndex == 2 ? false : true;
                NuevoProducto.Activo = estado;

                Convert.ToBoolean(modalDDLEstado.SelectedIndex);

                if (modalCheckBoxAtributos.SelectedItem != null)
                {
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
                }

                NuevoProducto.TiempoCoccion = TimeSpan.Parse(NuevoProductoTiempoCoccion.Text);
                NuevoProducto.Stock = int.Parse(NuevoProductoStock.Text);

                ProductoNegocio PNaux = new ProductoNegocio();
                try
                {
                    PNaux.NuevoProducto(NuevoProducto);
                    ((List<Producto>)Session["ListaProductos"]).Add(NuevoProducto);

                }
                catch (Exception ex)
                {
                    
                    string script = $"alert('Error al guardar el registro. {ex.ToString()} ');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
                }
              
                NuevoProductoNombre.Text = null;
                NuevoProductoDescripcion.Text = null;
                NuevoProductoValor.Text = null;

                modalDDLCategorias.SelectedIndex = 0;
                modalDDLEstado.SelectedIndex = 0;
                NuevoProductoStock = null;


                for (int i = 0; i < modalCheckBoxAtributos.Items.Count; i++)
                {
                    modalCheckBoxAtributos.Items[i].Selected = false;
                }

                NuevoProductoTiempoCoccion.Text = null;
                ListarProductos();
             
            }
            else
            {


                string script = "alert('ERROR. Todos los campos deben ser completados. Intentelo nuevamente.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);

            }


        }

        public void EliminarProducto(object sender, System.EventArgs e)
        {
            Button button = sender as Button;
            ProductoNegocio PNaux = new ProductoNegocio();

            if (!ValidarProducto(int.Parse(button.CommandArgument)))
            {
                try
                {
                    PNaux.EliminarProducto(int.Parse(button.CommandArgument));
                    string script = "alert('Eliminado correctamente');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
                    ListarProductos();
                }

                catch (Exception ex)
                {

                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                string script = "alert('Registro en uso. No se puede eliminar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
            }


        }

        protected void btnModificarProducto_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            Producto Paux = ((List<Producto>)Session["ListaProductos"]).Find(x => x.Id == int.Parse(button.CommandArgument));
            Session.Add("ProductoAModificar", Paux);


        }

        protected void MPBtnModificarProducto_Click(object sender, EventArgs e)
        {
            Producto Paux = ((Producto)(Session["ProductoAModificar"]));
            Paux.Nombre = string.IsNullOrEmpty(MPnombre1.Text) ? Paux.Nombre : MPnombre1.Text;
            Paux.Descripcion = string.IsNullOrEmpty(MPDescripcion.Text) ? Paux.Descripcion : MPDescripcion.Text;
            Paux.Valor = string.IsNullOrEmpty(MPvalor.Text) ? Paux.Valor : decimal.Parse(MPvalor.Text);
            Paux.Categoria = MPDDLCategoria.SelectedIndex != 0 && MPDDLCategoria != null ? MPDDLCategoria.SelectedIndex : Paux.Categoria;
            bool estado = MPDDLEstado.SelectedIndex == 2 ? false : true;
            Paux.Activo = estado;
            Paux.AptoCeliaco = MPCheckBoxAtributos.Items.FindByText("Celiaco").Selected == true ? true : false;
            Paux.Alcohol = MPCheckBoxAtributos.Items.FindByText("Alcohol").Selected == true ? true : false;
            Paux.TiempoCoccion = string.IsNullOrEmpty(MPtiempococcion.Text) ? Paux.TiempoCoccion : TimeSpan.Parse(MPtiempococcion.Text);
            Paux.AptoVegano = MPCheckBoxAtributos.Items.FindByText("Vegano").Selected == true ? true : false;
            Paux.Stock = string.IsNullOrEmpty(MPStock.Text) ? Paux.Stock : int.Parse(MPStock.Text);

            ProductoNegocio PNaux = new ProductoNegocio();
            try
            {
                PNaux.ModificarProducto(Paux);

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {

                ListarProductos();
                MPnombre1.Text = null;
                MPDescripcion.Text = null;
                MPvalor.Text = null;
                MPDDLCategoria.SelectedIndex = 0;
                MPDDLEstado.SelectedIndex = 0;
                MPStock = null;


                for (int i = 0; i < MPCheckBoxAtributos.Items.Count; i++)
                {
                    modalCheckBoxAtributos.Items[i].Selected = false;
                }

                MPtiempococcion.Text = null;
            }





        }


        protected void BtnAplicarFiltros_Click(object sender, EventArgs e)
        {
            LimpiarListaFiltrada();
            List<Producto> ListaFiltrada;
            ListaFiltrada = ((List<Producto>)Session["ListaProductos"]);
            bool SinCamposVacios = false;
            //check estado
            if (DDLEstado.SelectedIndex > 0)
            {
                bool estado = DDLEstado.SelectedIndex == 1 ? true : false;
                ListaFiltrada.RemoveAll(x => !x.Activo == estado);
                SinCamposVacios = true;
            }
            // check categorias
            if (DDLCategorias.SelectedIndex > 0)
            {
                ListarCategoriasProducto();
                ListaFiltrada.RemoveAll(x => x.Categoria != ListaCategoriasProducto.Find(y => y.Descripcion == DDLCategorias.SelectedItem.ToString()).Id);
                SinCamposVacios = true;
            }
            // check rango precios
            if (tbPrecioMenor.Text != string.Empty && tbPrecioMayor.Text != string.Empty)
            {
                decimal precioMaximo;
                decimal precioMinimo;
                if (decimal.TryParse(tbPrecioMenor.Text, out precioMinimo) && decimal.TryParse(tbPrecioMayor.Text, out precioMaximo))
                {
                    ListaFiltrada.RemoveAll(x => x.Valor < precioMinimo || x.Valor > precioMaximo);
                    SinCamposVacios = true;
                }
            }
            else if (tbPrecioMayor.Text != string.Empty)
            {
                decimal precioMaximo;
                if (decimal.TryParse(tbPrecioMayor.Text, out precioMaximo))
                {
                    ListaFiltrada.RemoveAll(x => x.Valor > precioMaximo);
                    SinCamposVacios = true;
                }
            }
            else if (tbPrecioMenor.Text != string.Empty)
            {
                decimal precioMinimo;
                if (decimal.TryParse(tbPrecioMenor.Text, out precioMinimo))
                {
                    ListaFiltrada.RemoveAll(x => x.Valor < precioMinimo);
                    SinCamposVacios = true;
                }
            }
            // check rango stock


            if (tbStockMenor.Text != string.Empty && tbStockMayor.Text != string.Empty)
            {
                int stockMayor;
                int stockMenor;
                if (int.TryParse(tbStockMenor.Text, out stockMenor) && int.TryParse(tbStockMayor.Text, out stockMayor))
                {
                    ListaFiltrada.RemoveAll(x => x.Stock < stockMenor || x.Stock > stockMayor);
                    SinCamposVacios = true;
                }
            }
            else if (tbStockMayor.Text != string.Empty)
            {
                int stockMayor;
                if (int.TryParse(tbStockMayor.Text, out stockMayor))
                {
                    ListaFiltrada.RemoveAll(x => x.Stock > stockMayor);
                    SinCamposVacios = true;
                }
            }
            else if (tbStockMenor.Text != string.Empty)
            {
                int stockMinimo;
                if (int.TryParse(tbStockMenor.Text, out stockMinimo))
                {
                    ListaFiltrada.RemoveAll(x => x.Stock < stockMinimo);
                    SinCamposVacios = true;
                }
            }

            // check atributos
            if (CheckBoxAtributos.Items[0].Selected == true)
            {
                ListaFiltrada.RemoveAll(x => !x.AptoVegano == true);
                SinCamposVacios = true;
            }
            if (CheckBoxAtributos.Items[1].Selected == true)
            {
                ListaFiltrada.RemoveAll(x => !x.AptoCeliaco == true);
                SinCamposVacios = true;
            }
            if (CheckBoxAtributos.Items[2].Selected)
            {
                ListaFiltrada.RemoveAll(x => !x.Alcohol == true);
                SinCamposVacios = true;
            }


            // check orden
            if (DDLPrecios.SelectedIndex != 0)
            {
                ListaFiltrada = OrdenarPorPrecio(ListaFiltrada);
                SinCamposVacios = true;
            }
            if (DDLStock.SelectedIndex != 0)
            {
                ListaFiltrada = OrdenarPorStock(ListaFiltrada);
                SinCamposVacios = true;
            }


            if (SinCamposVacios == true)
            {
                Session.Add("ListaFiltrada", ListaFiltrada);
                ActualizarGV(ListaFiltrada);
            }


        }


        // Limpiar Botones Filtros
        public void LimpiarBotonesFiltros()
        {
            DDLCategorias.SelectedIndex = 0;
            DDLEstado.SelectedIndex = 0;
            DDLPrecios.SelectedIndex = 0;
            DDLStock.SelectedIndex = 0;
            tbPrecioMayor.Text = string.Empty;
            tbPrecioMenor.Text = string.Empty;
            tbStockMenor.Text = string.Empty;
            tbStockMayor.Text = string.Empty;
            for (int i = 0; i < CheckBoxAtributos.Items.Count; i++)
            {
                CheckBoxAtributos.Items[i].Selected = false;
            }
            TxtBuscar.Text = "Ingrese nombre o descripción";
        }

        // Ordenar por precio
        public List<Producto> OrdenarPorPrecio(List<Producto> ListaFiltrada)
        {

            if (DDLPrecios.SelectedIndex == 1)
            {
                ListaFiltrada = ListaFiltrada.OrderBy(x => x.Valor).ToList();
            }
            if (DDLPrecios.SelectedIndex == 2)
            {
                ListaFiltrada = ListaFiltrada.OrderByDescending(x => x.Valor).ToList();
            }
            return ListaFiltrada;
        }

        // Ordenar por stock
        public List<Producto> OrdenarPorStock(List<Producto> ListaFiltrada)
        {
            if (DDLStock.SelectedIndex == 1)
            {
                ListaFiltrada = ListaFiltrada.OrderBy(x => x.Stock).ToList();
            }
            if (DDLStock.SelectedIndex == 2)
            {
                ListaFiltrada = ListaFiltrada.OrderByDescending(x => x.Stock).ToList();
            }
            return ListaFiltrada;
        }

        // Buscar
        protected void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (TxtBuscar.Text.Count() > 0 && TxtBuscar.Text != "Ingrese nombre o descripción")
            {
                LimpiarListaFiltrada();
                List<Producto> ListaFiltrada = ((List<Producto>)Session["ListaProductos"]).FindAll(x => x.Nombre.ToUpper().Contains(TxtBuscar.Text.ToUpper()) || x.Descripcion.ToUpper().Contains(TxtBuscar.Text.ToUpper()));
                ActualizarGV(ListaFiltrada);
            }
        }



        protected void BtnAgregarStock_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            GridViewRow row = (GridViewRow)button.NamingContainer;
            TextBox tbAgregarStock = (TextBox)row.FindControl("tbAgregarStock");

            if (!string.IsNullOrEmpty(tbAgregarStock.Text))
            {
                if (!ValidarProducto(int.Parse(button.CommandArgument)))
                {

                    Producto PAux = ((List<Producto>)Session["ListaProductos"]).Find(x => x.Id == int.Parse(button.CommandArgument));
                    PAux.Stock += int.Parse(tbAgregarStock.Text);
                    ProductoNegocio PNAux = new ProductoNegocio();
                    PNAux.ModificarProducto(PAux);
                    ListarProductos();

                }
                else
                {
                    string script = "alert('ERROR. Producto en uso. Modificar desde Productos Del Dia');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);

                }

            }
            else
            {
                string script = "alert('Ingrese un Valor');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);

            }

        }

        protected void BtnQuitarStock_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            GridViewRow row = (GridViewRow)button.NamingContainer;
            TextBox tbAgregarStock = (TextBox)row.FindControl("tbAgregarStock");

            if (!string.IsNullOrEmpty(tbAgregarStock.Text))
            {
                if (!ValidarProducto(int.Parse(button.CommandArgument)))
                {

                    Producto PAux = ((List<Producto>)Session["ListaProductos"]).Find(x => x.Id == int.Parse(button.CommandArgument));
                    PAux.Stock -= int.Parse(tbAgregarStock.Text);
                    ProductoNegocio PNAux = new ProductoNegocio();
                    PNAux.ModificarProducto(PAux);
                    ListarProductos();

                }
                else
                {
                    string script = "alert('ERROR. Producto en uso. Modificar desde Productos Del Dia');";
                    ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);

                }
            }
            else
            {
                string script = "alert('Ingrese un Valor');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);

            }

        }

        protected bool ValidarProducto(int id)
        {
            ProductoNegocio PNAux = new ProductoNegocio();

            DateTime Faux = DateTime.Now;


            if ((PNAux.BuscarProductoDelDia(id, DateTime.Now)).Id != -1 && (PNAux.BuscarProductoDelDia(id, DateTime.Now)) != null)
            {
                return true;
            }
            else { return false; }
        }

        // GUARDAR CATEGORIA MODIFICADA
        protected void btnModificarCategoria_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            RepeaterItem repeaterItem = button.NamingContainer as RepeaterItem;
            TextBox tbCategoriaNombre = repeaterItem.FindControl("tbCategoriaNombre") as TextBox;
            if (tbCategoriaNombre.Text != string.Empty)
            {
                CategoriaProducto CPAux = new CategoriaProducto();
                CPAux.Id = int.Parse(button.CommandArgument);
                CPAux.Descripcion = tbCategoriaNombre.Text;
                CategoriaProductoNegocio CNPAux = new CategoriaProductoNegocio();
                CNPAux.ModificarCategoria(CPAux);
                CargarDDLCategorias();
                ListarProductos();
            }
        }

        // GUARDAR NUEVA CATEGORIA
        protected void btnGuardarCategoria_Click(object sender, EventArgs e)
        {

            if ((tbNuevaCategoria.Text != string.Empty) && (tbNuevaCategoria.Text != "Ingresar Descripción"))
            {
                CategoriaProducto CPAux = new CategoriaProducto();
                CPAux.Descripcion = tbNuevaCategoria.Text;
                CategoriaProductoNegocio CNPAux = new CategoriaProductoNegocio();
                CNPAux.AgregarCategoria(CPAux);
                CargarDDLCategorias();
                
            }
            else
            {
                string script = "alert('Ingrese un Valor');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);

            }
        }



        protected void btnCancelarCategoria_Click(object sender, EventArgs e)
        {

        }

       
        protected void btnActivarCategoria_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            try
            {
                CategoriaProductoNegocio CNPAux = new CategoriaProductoNegocio();
                CNPAux.CategoriaBajaFisica(int.Parse(button.CommandArgument));
                CargarDDLCategorias();
            }
            catch (Exception)
            {
                string script = "alert('Categoria en uso, no se puede eliminar.');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
               
            }
        }

    
        protected void BtnEliminarLote_Click(object sender, EventArgs e)
        {
            if ((List<Producto>)Session["ListaFiltrada"] != null)
            {
                List<Producto> ListaFiltrada = ((List<Producto>)Session["ListaFiltrada"]);
                List<Producto> ProductosActivos = new List<Producto>();

                foreach(Producto Producto in ListaFiltrada)
                {
                    if (!ValidarProducto(Producto.Id))
                    {
                        ProductoNegocio PNAux = new ProductoNegocio();
                        PNAux.EliminarProducto(Producto.Id);
                    }
                }
                ListarProductos();
                    

            }
            else
            {
                string script = "alert('Primero filtre una selección con los controles de filtro');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);

            }

        }

        protected void GVProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
        }
    }
    }





