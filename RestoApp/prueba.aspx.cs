using Dominio;
using Negocio;
using Opciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace RestoApp
{
    public partial class prueba : System.Web.UI.Page
    {

        public List<ProductoDelDia> productoDelDias        { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
                        

            GVDCocina.DataSource = creardatatable();
            GVDCocina.DataBind();

       

;        }

        public DataTable creardatatable()
        {

            ProductoNegocio productoNegocio = new ProductoNegocio();
            productoDelDias = productoNegocio.ListarProductosDelDia();
            productoDelDias = productoDelDias.FindAll(x => x.Categoria == 1).ToList();
            foreach (ProductoDelDia aux in productoDelDias)
            {

                aux.Stock = 2;

            }


            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Nombre", typeof(string));
            
            dataTable.Columns.Add("11:00", typeof(string));
            dataTable.Columns.Add("11:05", typeof(string));
            dataTable.Columns.Add("11:10", typeof(string));
            dataTable.Columns.Add("11:15", typeof(string));
            dataTable.Columns.Add("11:20", typeof(string));
            dataTable.Columns.Add("11:25", typeof(string));
            dataTable.Columns.Add("11:30", typeof(string));
            dataTable.Columns.Add("11:35", typeof(string));
            dataTable.Columns.Add("11:40", typeof(string));
            dataTable.Columns.Add("11:45", typeof(string));
            dataTable.Columns.Add("11:50", typeof(string));
            dataTable.Columns.Add("11:55", typeof(string));
            dataTable.Columns.Add("12:00", typeof(string));
            dataTable.Columns.Add("12:05", typeof(string));
            dataTable.Columns.Add("12:10", typeof(string));
            dataTable.Columns.Add("12:15", typeof(string));
            dataTable.Columns.Add("12:20", typeof(string));
            dataTable.Columns.Add("12:25", typeof(string));
            dataTable.Columns.Add("12:30", typeof(string));
            dataTable.Columns.Add("12:35", typeof(string));
            dataTable.Columns.Add("12:40", typeof(string));
            dataTable.Columns.Add("12:45", typeof(string));
            dataTable.Columns.Add("12:50", typeof(string));
            dataTable.Columns.Add("12:55", typeof(string));
            dataTable.Columns.Add("13:00", typeof(string));
            dataTable.Columns.Add("13:05", typeof(string));
            dataTable.Columns.Add("13:10", typeof(string));
            dataTable.Columns.Add("13:15", typeof(string));
            dataTable.Columns.Add("13:20", typeof(string));
            dataTable.Columns.Add("13:25", typeof(string));
            dataTable.Columns.Add("13:30", typeof(string));
            dataTable.Columns.Add("13:35", typeof(string));
            dataTable.Columns.Add("13:40", typeof(string));
            dataTable.Columns.Add("13:45", typeof(string));
            dataTable.Columns.Add("13:50", typeof(string));
            dataTable.Columns.Add("13:55", typeof(string));
            dataTable.Columns.Add("14:00", typeof(string));
            dataTable.Columns.Add("14:05", typeof(string));
            dataTable.Columns.Add("14:10", typeof(string));
            dataTable.Columns.Add("14:15", typeof(string));
            dataTable.Columns.Add("14:20", typeof(string));
            dataTable.Columns.Add("14:25", typeof(string));
            dataTable.Columns.Add("14:30", typeof(string));
            dataTable.Columns.Add("14:35", typeof(string));
            dataTable.Columns.Add("14:40", typeof(string));
            dataTable.Columns.Add("14:45", typeof(string));
            dataTable.Columns.Add("14:50", typeof(string));
            dataTable.Columns.Add("14:55", typeof(string));
            dataTable.Columns.Add("15:00", typeof(string));
            dataTable.Columns.Add("15:05", typeof(string));
            dataTable.Columns.Add("15:10", typeof(string));
            dataTable.Columns.Add("15:15", typeof(string));
            dataTable.Columns.Add("15:20", typeof(string));
            dataTable.Columns.Add("15:25", typeof(string));
            dataTable.Columns.Add("15:30", typeof(string));
            dataTable.Columns.Add("15:35", typeof(string));
            dataTable.Columns.Add("15:40", typeof(string));
            dataTable.Columns.Add("15:45", typeof(string));
            dataTable.Columns.Add("15:50", typeof(string));
            dataTable.Columns.Add("15:55", typeof(string));
            dataTable.Columns.Add("16:00", typeof(string));


            foreach (ProductoDelDia aux in productoDelDias)
            {
                dataTable.Rows.Add(aux.Nombre);
            }


            return dataTable;

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Txtreloj.Text = DateTime.Now.ToString();
          
        }
    }
}