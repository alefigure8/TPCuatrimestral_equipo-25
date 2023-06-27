﻿using System;
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
            try
            {
                Button button = sender as Button;
                Producto Paux = ((List<Producto>)Session["ListaProductos"]).Find(x => x.Id == int.Parse(button.CommandArgument));
                int Cat = Paux.Categoria;
                ProductoDelDia PDDAux = new ProductoDelDia(Paux);
                ProductoNegocio PNaux = new ProductoNegocio();
                PNaux.AgregarProductoDD(PDDAux);
                ListarProductosDelDia();
            }
            catch (Exception ex)
            {

                string script = "alert('Ya se encuentra en el menú actual');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
              
            }
            finally
            {
                ListarProductosDelDia();
            }
        
        }

        protected void BtnDesactivar_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            ProductoDelDia PDDAux = ((List<ProductoDelDia>)Session["ListaProductosDelDia"]).Find(x => x.Id == int.Parse(button.CommandArgument));
            if(button.Text.ToLower() == "cerrar")
            {
                CerrarProductoDelDia(PDDAux);
            }
            else if(button.Text.ToLower() == "abrir")
            {
                AbrirProductoDelDia(PDDAux);
            }
        }

        protected void CerrarProductoDelDia(ProductoDelDia PDDAux)
        {
            PDDAux.Activo = false;
            PDDAux.StockCierre = (((List<Producto>)Session["ListaProductos"]).Find(x => x.Id == PDDAux.Id)).Stock;
            ProductoNegocio PNAux = new ProductoNegocio();
            PNAux.ModificarProductoDD(PDDAux);
            ListarProductosDelDia();
            
        }
        protected void AbrirProductoDelDia(ProductoDelDia PDDAux)
        {
            PDDAux.Activo = true;
            PDDAux.StockCierre = (((List<Producto>)Session["ListaProductos"]).Find(x => x.Id == PDDAux.Id)).Stock;
            ProductoNegocio PNAux = new ProductoNegocio();
            PNAux.ModificarProductoDD(PDDAux);
            ListarProductosDelDia();
        }

        protected void BtnAgregarStock_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            ProductoDelDia PDDAux = ((List<ProductoDelDia>)Session["ListaProductosDelDia"]).Find(x => x.Id == int.Parse(button.CommandArgument));
            
        }

        protected void BtnSumarStock_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            ProductoDelDia PDDAux = ((List<ProductoDelDia>)Session["ListaProductosDelDia"]).Find(x => x.Id == int.Parse(button.CommandArgument));
            PDDAux.Activo = true;
            PDDAux.StockCierre = (((List<Producto>)Session["ListaProductos"]).Find(x => x.Id == PDDAux.Id)).Stock;
            ProductoNegocio PNAux = new ProductoNegocio();
            PNAux.ModificarProductoDD(PDDAux);
            ListarProductosDelDia();
        }

    }
}