using Dominio;
using Negocio;
using Opciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.DynamicData;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace RestoApp
{
    public partial class prueba : System.Web.UI.Page
    {
        public DateTime Reloj { get; set; }
      
        public List<ProductoDelDia> productoDelDias { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
             
            if (!IsPostBack)
            {
                Cargarpedidosprueba();
                Reloj = DateTime.Now;
                Session.Add("Reloj", Reloj);

            }
            Session.Add("datatable", creardatatable());
            GVDCocina.DataSource = creardatatable();
            GVDCocina.DataBind();
        }

        public void Cargarpedidosprueba()
        {
            /*
            ProductoNegocio productoNegocio = new ProductoNegocio();
            productoDelDias = productoNegocio.ListarProductosDelDia();
            productoDelDias = productoDelDias.FindAll(x => x.Categoria == 1).ToList();
            foreach (ProductoDelDia aux in productoDelDias)
            {
             aux.Stock = 2;

            }
            */
                      
                  
            Pedido Pedido = new Pedido();
            Pedido.Productosdeldia = new List<ProductoDelDia>();
            Pedido.Estadopedido = new Estadopedido();

            ProductoDelDia productodelpedido0 = new ProductoDelDia();

            productodelpedido0.Id = 1;
            productodelpedido0.TiempoCoccion = TimeSpan.Parse("00:15:00");

            Pedido.Productosdeldia.Add(productodelpedido0);

            ProductoDelDia productodelpedido1 = new ProductoDelDia();

            productodelpedido1.Id = 2;
            productodelpedido1.TiempoCoccion = TimeSpan.Parse("00:20:00");

            Pedido.Productosdeldia.Add(productodelpedido1);

            Pedido.Id = 1;
            Pedido.Estadopedido.Id = 1;
            Pedido.Estadopedido.Descripcion = "Solicitado";
            Pedido.Estadopedido.fechaactualizacion = DateTime.Now;

            Session.Add("Pedido", Pedido);


        }

        public List<string> horariosmañana()
        {
            string[] horariosdia = {
        "07:00", "07:05", "07:10", "07:15", "07:20", "07:25", "07:30", "07:35", "07:40", "07:45",
        "07:50", "07:55", "08:00", "08:05", "08:10", "08:15", "08:20", "08:25", "08:30", "08:35",
        "08:40", "08:45", "08:50", "08:55", "09:00", "09:05", "09:10", "09:15", "09:20", "09:25",
        "09:30", "09:35", "09:40", "09:45", "09:50", "09:55", "10:00", "10:05", "10:10", "10:15",
        "10:20", "10:25", "10:30", "10:35", "10:40", "10:45", "10:50", "10:55", "11:00", "11:05",
        "11:10", "11:15", "11:20", "11:25", "11:30", "11:35", "11:40", "11:45", "11:50", "11:55",
        "12:00"
    };

            List<string> listaHorarios = new List<string>(horariosdia);
            return listaHorarios;
        }


        public List<string> horariosdia()
        {
            string[] horariosdia = {
             "11:00", "11:05", "11:10", "11:15", "11:20", "11:25", "11:30", "11:35", "11:40", "11:45",
                 "11:50", "11:55", "12:00", "12:05", "12:10", "12:15", "12:20", "12:25", "12:30", "12:35",
                 "12:40", "12:45", "12:50", "12:55", "13:00", "13:05", "13:10", "13:15", "13:20", "13:25",
                "13:30", "13:35", "13:40", "13:45", "13:50", "13:55", "14:00", "14:05", "14:10", "14:15",
                 "14:20", "14:25", "14:30", "14:35", "14:40", "14:45", "14:50", "14:55", "15:00", "15:05",
                  "15:10", "15:15", "15:20", "15:25", "15:30", "15:35", "15:40", "15:45", "15:50", "15:55",
             "16:00"
            };

            List<string> listaHorarios = new List<string>(horariosdia);
            return listaHorarios;
        }

        public string[] horariosnoche()
        {
            string[] horariosnoche = {
             "19:00", "19:05", "19:10", "19:15", "19:20", "19:25", "19:30", "19:35", "19:40", "19:45",
                "19:50", "19:55", "20:00", "20:05", "20:10", "20:15", "20:20", "20:25", "20:30", "20:35",
                 "20:40", "20:45", "20:50", "20:55", "21:00", "21:05", "21:10", "21:15", "21:20", "21:25",
                    "21:30", "21:35", "21:40", "21:45", "21:50", "21:55", "22:00", "22:05", "22:10", "22:15",
                 "22:20", "22:25", "22:30", "22:35", "22:40", "22:45", "22:50", "22:55", "23:00", "23:05",
             "23:10", "23:15", "23:20", "23:25", "23:30", "23:35", "23:40", "23:45", "23:50", "23:55",
             "00:00", "00:05", "00:10", "00:15", "00:20", "00:25", "00:30", "00:35", "00:40", "00:45",
                 "00:50", "00:55", "01:00"
                };

            return horariosnoche;
        }

      
        public DataTable creardatatable()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Nombre", typeof(string));
            
            foreach (string horario in horariosmañana())
            {
                dataTable.Columns.Add(horario, typeof(string));

            }

            Pedido Pedido = new Pedido();
            Pedido.Productosdeldia = new List<ProductoDelDia>();
            Pedido.Estadopedido = new Estadopedido();
            Pedido = (Pedido)Session["Pedido"];
            
             

            
            foreach (ProductoDelDia aux in Pedido.Productosdeldia)
            {
                dataTable.Rows.Add(aux.Id);
            }
            

            // Obtén el índice de la columna en la que deseas aplicar el estilo

            

            return dataTable;
        }

        public void Timer1_Tick(object sender, EventArgs e)
        {
            Reloj = (DateTime)Session["Reloj"];
            Reloj = Reloj.AddSeconds(60);
            Txtreloj.Text = Reloj.ToString("HH:mm");
            Session.Add("Reloj", Reloj);

    

        }


        protected void GVDCocina_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //recuperamos pedido de session
            Pedido Pedido = (Pedido)Session["Pedido"];

            //recuperamos datatable de session
            
            DataTable dataTable = (DataTable)Session["datatable"];



            int columnaIndice = dataTable.Columns.IndexOf("Nombre");
            // calculamos cuantos casilleros ocupa el tiempo de coccion de cada producto
            int casillerospedido1 = (int)Pedido.Productosdeldia[0].TiempoCoccion.TotalMinutes / 5;
            int casillerospedido2 = (int)Pedido.Productosdeldia[1].TiempoCoccion.TotalMinutes / 5;

            // creamos variable datetime y variable timespan
            // a Timespan asignamos el horario del pedido
            DateTime datetime = new DateTime();
            datetime = Pedido.Estadopedido.fechaactualizacion;
            TimeSpan TimeSpan = new TimeSpan(datetime.Hour, datetime.Minute, datetime.Second);
            
            
            // redondeamos los minutos a multiplos de 5 y lo pasamos a string para comparar
            // con columnas de datatable
            int redondeomins = TimeSpan.Minutes - TimeSpan.Minutes % 5;
            string hora = TimeSpan.Hours.ToString("00") + ":" + redondeomins.ToString("00");
            
            // obtenemos el indice de la columna en la que se encuentra el horario del pedido
            int casillero = dataTable.Columns.IndexOf(hora);
            // calculamos la cantidad de casilleros que ocupa el tiempo de coccion del producto
            casillerospedido1 = casillerospedido1 + casillero;
            casillerospedido2 = casillerospedido2 + casillero;

            // recuperamos el horario actual simulado y lo asignamos a un timespan
            Reloj = (DateTime)Session["Reloj"];
            TimeSpan Horaactual = new TimeSpan(Reloj.Hour, Reloj.Minute, Reloj.Second);
            int redondeomins2 = Horaactual.Minutes - Horaactual.Minutes % 5;
            string horaactual = Horaactual.Hours.ToString("00") + ":" + redondeomins2.ToString("00");
            int casilleroevolucion1 = dataTable.Columns.IndexOf(horaactual);



            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Obtén el valor de la celda en la columna deseada
                //  string valorCelda = DataBinder.Eval(e.Row.DataItem, "Nombre").ToString();

                // foreach 
                

                if (e.Row.RowIndex == 0)
                {
                    for (int i = casillero; i <= casillerospedido1; i++)
                    {

                        if (i > casilleroevolucion1)
                        {
                            e.Row.Cells[i].BackColor = Color.LightBlue;
                        }                     

                    }
                    for (int j = casillero; j <= casilleroevolucion1; j++)
                    {

                        e.Row.Cells[j].BackColor = Color.LightPink;
                    }
                }




            }
            if (e.Row.RowIndex == 1)
            {
                for (int i = casillero; i <= casillerospedido2; i++)
                {

                    // Cambia el color de fondo de la celda
                    e.Row.Cells[i].BackColor = Color.LightBlue; // Cambia "Color.Red" por el color que desees
                    {
                        e.Row.Cells[i].BackColor = Color.LightPink;
                    }

                }

                
            }
        }
            
        
        protected void GVDCocina_DataBound(object sender, EventArgs e)
        {

        }
    }
}
