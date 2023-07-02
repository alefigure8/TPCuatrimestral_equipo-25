using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing.Drawing2D;
using System.Web.Services;
using System.Threading;
using Dominio;
using Negocio;
namespace RestoApp
{
    public partial class Cocina : System.Web.UI.Page
    {
        public int progreso { get; set; }
        
        public List<ProductoDelDia> productoDelDias { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            /*
                ProductoNegocio productoNegocio = new ProductoNegocio();
                productoDelDias = productoNegocio.ListarProductosDelDia();
                productoDelDias = productoDelDias.FindAll(x=> x.Categoria == 1).ToList();
                foreach (ProductoDelDia aux in productoDelDias)
                {

                    aux.Stock = 2;

                }
                Repeaterprueba.DataSource = productoDelDias;
                Repeaterprueba.DataBind();
            */
            }
        }
        /*
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if(progreso <= 100)
            {
                progreso = DateTime.Now.Second;

            }



            RepeaterItem repeateritem;
            repeateritem.
            


            /*
            foreach (Producto producto in productoDelDias)
            {
             
              //  progressbar+producto.Id.Style["width"] = progreso.ToString() + "%";
            }
            */
        }
    
    }
