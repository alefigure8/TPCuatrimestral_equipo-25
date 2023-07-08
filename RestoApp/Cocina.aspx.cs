using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Opciones;
using System.Web.DynamicData;
using System.Globalization;
using Helper;
using System.Diagnostics.Eventing.Reader;



namespace RestoApp
{
    public partial class Cocina : System.Web.UI.Page
    {
        public DateTime Reloj { get; set; }
        public Usuario usuario { get; set; }
        public List<Pedido> Pedidossolicitados { get; set; }
        public List<Pedido> Pedidosenpreparacion { get; set; }
        public List<Pedido> Estadopedidos { get; set; }
        public List<Pedido> Estadopedidosprevia { get; set; }
        public List<HelperCocina> Helpercocina { get; set; }



        public List<ProductoPorPedido> Productosenpreparacion { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];


            if (!IsPostBack)
            {
               
                Reloj = DateTime.Now;
                Session.Add("Reloj", Reloj);
                CrearDatatableCocina();
                CrearDataTableProductosEnPreparacion();
                CrearDataTableEstadoPedidos();

            }

            ActualizarSolicitadosenDB();
            Listarpedidosenpreparacion();
            ActualizarDGVCocina();
            ActualizarDGVEstadoPedidos();
            ActualizarDGVProductosenPreparacion();


        }

        public void ActualizarSolicitadosenDB()
        {
            Pedidosenpreparacion = new List<Pedido>();
            PedidoNegocio pedidoNegocio = new PedidoNegocio();
            Pedidossolicitados = pedidoNegocio.ListarPedidos(Estados.Solicitado);

            // VALIDAR QUE SEAN DE HOY Y ESTEN EN ESTADO SOLICITADO
            foreach (Pedido pedido in Pedidossolicitados.ToList())
            {
             
                if (InvertirFecha(pedido.ultimaactualizacion) != DateTime.Now.ToString("d"))
                {
                    Pedidossolicitados.Remove(pedido);
                }
                // VALIDAR QUE SEAN DE COCINA
                foreach (ProductoPorPedido productossolicitados in pedido.Productossolicitados)
                {

                    if (productossolicitados.Productodeldia.Categoria != 3)
                    {
                        Pedidossolicitados.Remove(pedido);
                    }

                }


            }
            // CAMBIAR ESTADO A EN PREPARACION
            foreach (Pedido pedido in Pedidossolicitados.ToList())
            {
                pedidoNegocio.CambiarEstadoPedido(pedido.Id, Estados.EnPreparacion);
            }

        }



        public void Listarpedidosenpreparacion()
        {

            PedidoNegocio pedidoNegocio = new PedidoNegocio();
            Pedidosenpreparacion = pedidoNegocio.ListarPedidos(Estados.EnPreparacion);
            Pedidosenpreparacion.AddRange(pedidoNegocio.ListarPedidos(Estados.DemoradoEnCocina));

            // VALIDAR QUE SEAN DE HOY Y ESTEN EN ESTADO SOLICITADO
            foreach (Pedido pedido in Pedidosenpreparacion.ToList())
            {
                if (InvertirFecha(pedido.ultimaactualizacion) != DateTime.Now.ToString("d"))
                {
                    Pedidosenpreparacion.Remove(pedido);
                }

            }
            // VALIDAR QUE SEAN PEDIDOS DE COCINA

            foreach (Pedido pedido in Pedidosenpreparacion.ToList())
            {

                foreach (ProductoPorPedido productossolicitados in pedido.Productossolicitados)
                {

                    if (productossolicitados.Productodeldia.Categoria != 3)
                    {
                        Pedidosenpreparacion.Remove(pedido);
                    }
                }

            }

            Session.Add("Pedidosenpreparacion", Pedidosenpreparacion);


        }


        public DataTable CrearDataTableProductosEnPreparacion()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Producto", typeof(string));
            dataTable.Columns.Add("EnPreparacion", typeof(int));
            Session.Add("DTProductosenpreparacion", dataTable);
            return dataTable;

        }

        public DataTable CrearDataTableEstadoPedidos()
        {
            DataTable DTEstadopedidos = new DataTable();
            DTEstadopedidos.Columns.Add("Pedido", typeof(string));
            DTEstadopedidos.Columns.Add("Estado", typeof(string));
            DTEstadopedidos.Columns.Add("HoraFin", typeof(string));
            Session.Add("DTEstadoPedidos", DTEstadopedidos);
            return DTEstadopedidos;
        }




        public void ActualizarDGVEstadoPedidos()
        {

            Helpercocina = Session["Helpercocina"] as List<HelperCocina>;
            DataTable DTEstadopedidos = (DataTable)Session["DTEstadoPedidos"];
            Estadopedidos = Session["Pedidosenpreparacion"] as List<Pedido>;
       
           
           
                TimeSpan? Tiempomax = TimeSpan.Zero;
                DTEstadopedidos.Rows.Clear();
                
                foreach (Pedido pedido in Estadopedidos)
                {                  
                    foreach (ProductoPorPedido producto in pedido.Productossolicitados)
                    {
                        if (producto.Productodeldia.TiempoCoccion > Tiempomax)
                        {
                            Tiempomax = producto.Productodeldia.TiempoCoccion;
                        }
                    }

                    DataRow row = DTEstadopedidos.NewRow();
                    row["Pedido"] = "#" + pedido.Id;
                    row["Estado"] = pedido.EstadoDescripcion;
                    row["HoraFin"] = pedido.ultimaactualizacion.Add((TimeSpan)Tiempomax).ToString("HH:mm");
                    DTEstadopedidos.Rows.Add(row);
                    
                    if(Helpercocina != null) { 
                    Helpercocina.Find(x => x.idPedido == pedido.Id).horafin = pedido.ultimaactualizacion.Add((TimeSpan)Tiempomax).ToString("HH:mm");
                }


            }

                            
   
            Session.Add("DTEstadoPedidos", DTEstadopedidos);
            GVDEstadopedidos.DataSource = DTEstadopedidos;
            GVDEstadopedidos.DataBind();


        }


        public void ActualizacionAutomatica ()
        {
            Reloj = (DateTime)Session["Reloj"];
            TimeSpan diferencia = DateTime.Now - Reloj;
            if (diferencia.Seconds >= 10)
            {
                ActualizarSolicitadosenDB();
                Listarpedidosenpreparacion();
                ActualizarDGVCocina();
                ActualizarDGVEstadoPedidos();
                ActualizarDGVProductosenPreparacion();
                Reloj = DateTime.Now;
                Session.Add("Reloj", Reloj);
            }
        }



        public void ActualizarDGVProductosenPreparacion()
        {
            // RECUPERA DATATABLE CREADA
            DataTable dataTable = (DataTable)Session["DTProductosenpreparacion"];

            //CARGAMOS ALGO EN SESSION PARA QUE NO SE NULO LA LISTA EN EL PRIMER LOAD
            if (Session["Productosenpreparacion"] == null)
            {
                Productosenpreparacion = new List<ProductoPorPedido>();
                Session.Add("Productosenpreparacion", Productosenpreparacion);
            }

            PedidoNegocio pedidoNegocio = new PedidoNegocio();
            Productosenpreparacion = pedidoNegocio.Listarproductosenpreparacion();

         
                dataTable.Rows.Clear();
                foreach (ProductoPorPedido pxp in Productosenpreparacion)
                {
                    DataRow filaNueva = dataTable.NewRow();
                    filaNueva[0] = pxp.Productodeldia.Nombre;
                    filaNueva[1] = pxp.Cantidad;
                    dataTable.Rows.Add(filaNueva);
                }
                Session.Add("Productosenpreparacion", Productosenpreparacion);
         

            Session.Add("DTProductosenpreparacion", dataTable);
            GVDProductosenprep.DataSource = dataTable;
            GVDProductosenprep.DataBind();

        }


        public DataTable CrearDatatableCocina()
        {
            DataTable DTCocina = new DataTable();
            foreach (string horario in horarios())
            {
                DTCocina.Columns.Add(horario, typeof(string));
            }
            Session.Add("DTCocina", DTCocina);
            return DTCocina;

        }





        public void ActualizarDGVCocina()
        {
            // RECUPERA DATATABLE CREADA DE SESION
            DataTable DTCocina = (DataTable)Session["DTCocina"];
            // RECUPERA LISTA DE PEDIDOS EN PREPARACION
            Pedidosenpreparacion = Session["Pedidosenpreparacion"] as List<Pedido>;

            // RECUPERA LISTA DE FILAS POR COLUMNA POR TIEMPO DE COCCION
            List<HelperCocina> Helpercocina = Session["Helpercocina"] as List<HelperCocina>;

            PedidoNegocio pedidoNegocio = new PedidoNegocio();

            DTCocina.Rows.Clear();
            // RECORRE LISTA DE PEDIDOS EN PREPARACION
            foreach (var pedido in Pedidosenpreparacion.ToList())
            {
       
                // VALIDA QUE SE ENCUENTRE EN EL HORARIO ACTUAL
                if (horarios().Contains(HoraToString(pedido.ultimaactualizacion)))
                {
                                     
                        // BUSCA EL PRODUCTO DENTRO DEL PEDIDO QUE TENGA MAYOR TIEMPO DE COCCION
                        int CasillerosMaxTiempoCoccion = 0;
                        foreach (ProductoPorPedido producto in pedido.Productossolicitados)
                        {
                            if (CasillerosMaxTiempoCoccion < CantidadCasilleros(producto.Productodeldia.TiempoCoccion))
                            {
                                CasillerosMaxTiempoCoccion = CantidadCasilleros(producto.Productodeldia.TiempoCoccion);
                            }
                        }

                        foreach (ProductoPorPedido producto in pedido.Productossolicitados)
                        {
                            // CALCULA EL AJUSTE DE COLUMNA SEGUN EL TIEMPO DE COCCION DEL PRODUCTO
                            int TiempoCoccion = CantidadCasilleros(producto.Productodeldia.TiempoCoccion);
                            int Ajuste = ajusteportiempomaximopedido(TiempoCoccion, CasillerosMaxTiempoCoccion);

                            // AGREGA FILA POR CADA PRODUCTO DEL PEDIDO
                            DataRow filaNueva = DTCocina.NewRow();
                            filaNueva[Ajuste + IndiceColumna(HoraToString(pedido.ultimaactualizacion))] = "#" + pedido.Id;
                            filaNueva[Ajuste + IndiceColumna(HoraToString(pedido.ultimaactualizacion)) + 1] = producto.Productodeldia.Nombre;
                            filaNueva[Ajuste + IndiceColumna(HoraToString(pedido.ultimaactualizacion)) + 2] = "C:" + producto.Cantidad;
                            DTCocina.Rows.Add(filaNueva);


                            // GUARDA EN DATOS LA FILA POR COLUMNA POR TIEMPO DE COCCION PARA LUEGO PINTARLA
                            HelperCocina helpercocina = new HelperCocina();
                            helpercocina.Fila = DTCocina.Rows.IndexOf(filaNueva);
                            helpercocina.Columna = Ajuste + IndiceColumna(HoraToString(pedido.ultimaactualizacion));
                            helpercocina.TiempoCoccion = CantidadCasilleros(producto.Productodeldia.TiempoCoccion);
                            helpercocina.idPedido = pedido.Id;


                            // GUARDA EN SESION LA FILA POR COLUMNA POR TIEMPO DE COCCION
                            if (Session["Helpercocina"] == null)
                            {
                                Helpercocina = new List<HelperCocina>();
                                Helpercocina.Add(helpercocina);
                                Session.Add("Helpercocina", Helpercocina);
                            }
                            else
                            {
                                Helpercocina = Session["Helpercocina"] as List<HelperCocina>;
                                Helpercocina.Add(helpercocina);
                                Session.Add("Helpercocina", Helpercocina);
                            }
                        }
                   
                }
            }
            // ACTUALIZA DATAGRIDVIEW COCINA
            Session.Add("DTCocina", DTCocina);
            GVDCocina.DataSource = DTCocina;
            GVDCocina.DataBind();
        }


        protected void GVDEstadopedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {


            if (e.CommandName == "ListoparaEntregar")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                Estadopedidos = Session["Pedidosenpreparacion"] as List<Pedido>;
              PedidoNegocio    PedidoNegocio = new PedidoNegocio();

                PedidoNegocio.CambiarEstadoPedido(Estadopedidos[rowIndex].Id, Estados.ListoParaEntregar);
                               



            }
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
            Reloj = (DateTime)Session["Reloj"];
            int Indicecolumnahora = IndiceColumna(HoraToString(Reloj));
            // AGREGO BORDE DERECHO A CADA CELDA PARA GENERA LINEAS VERTICALES
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.Style["font-size"] = "10px";
                    cell.Style["text-align"] = "center";
                    cell.Style["border-right"] = "none";
                    cell.Style["border-right"] = "1px solid #565d63";
                }
            }


            // PINTO DE COLOR AZUL LAS CELDAS QUE CORRESPONDEN A UN PEDIDO
            Helpercocina = Session["Helpercocina"] as List<HelperCocina>;
            if (Session["Helpercocina"] != null)
            {
                foreach (var item in Helpercocina)
                {
                    if (e.Row.RowIndex == item.Fila)
                    {
                        for (int i = item.Columna; i < item.Columna + item.TiempoCoccion; i++)
                        {
                            if (i > Indicecolumnahora)
                            {

                                e.Row.Cells[i].BackColor = Color.LightBlue;
                            }
                            else if (i <= Indicecolumnahora)
                            {
                                e.Row.Cells[i].BackColor = Color.LightPink;


                            }
                        }



                    }
                }
            }


        }





        public string InvertirFecha(DateTime fechaactualizacion)
        {
            string fecha = fechaactualizacion.Month.ToString("00") + "/" + fechaactualizacion.Day.ToString("00") + "/" + fechaactualizacion.Year.ToString("0000");

            return fecha;
        }



        public List<String> horarios()
        {

            if (DateTime.Now.Hour >= 8 && DateTime.Now.Hour <= 11)
            {
                return horarios7a12();
            }
            else if (DateTime.Now.Hour >= 11 && DateTime.Now.Hour <= 16)
            {
                return horarios11a16();
            }
            else if (DateTime.Now.Hour >= 16 && DateTime.Now.Hour < 19)
            {
                return horarios16a19();
            }
            else if (DateTime.Now.Hour >= 19 && DateTime.Now.Hour < 24)
            {
                return horarios19a00();
            }
            else
            {
                return horarios00a06();
            }


        }







        public List<string> horarios16a19()
        {
            string[] horariosdia = {
                "16:00", "16:05", "16:10", "16:15", "16:20", "16:25", "16:30", "16:35", "16:40", "16:45",
                "16:50", "16:55", "17:00", "17:05", "17:10", "17:15", "17:20", "17:25", "17:30", "17:35",
                "17:40", "17:45", "17:50", "17:55", "18:00", "18:05", "18:10", "18:15", "18:20", "18:25",
                "18:30", "18:35", "18:40", "18:45", "18:50", "18:55", "19:00", "19:05", "19:10", "19:15",
            };

            List<string> listaHorarios = new List<string>(horariosdia);
            return listaHorarios;
        }
        public List<string> horarios7a12()
        {
            string[] horariosdia = {
            "07:00", "07:05", "07:10", "07:15", "07:20", "07:25", "07:30", "07:35", "07:40", "07:45",
            "07:50", "07:55", "08:00", "08:05", "08:10", "08:15", "08:20", "08:25", "08:30", "08:35",
            "08:40", "08:45", "08:50", "08:55", "09:00", "09:05", "09:10", "09:15", "09:20", "09:25",
            "09:30", "09:35", "09:40", "09:45", "09:50", "09:55", "10:00", "10:05", "10:10", "10:15",
            "10:20", "10:25", "10:30", "10:35", "10:40", "10:45", "10:50", "10:55", "11:00", "11:05",
            "11:10", "11:15", "11:20", "11:25", "11:30", "11:35", "11:40", "11:45", "11:50", "11:55",
            "12:00" };

            List<string> listaHorarios = new List<string>(horariosdia);
            return listaHorarios;
        }
        public List<string> horarios11a16()
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

        public List<string> horarios19a00()
        {
            string[] horariosnoche = {
             "19:00", "19:05", "19:10", "19:15", "19:20", "19:25", "19:30", "19:35", "19:40", "19:45",
             "19:50", "19:55", "20:00", "20:05", "20:10", "20:15", "20:20", "20:25", "20:30", "20:35",
             "20:40", "20:45", "20:50", "20:55", "21:00", "21:05", "21:10", "21:15", "21:20", "21:25",
             "21:30", "21:35", "21:40", "21:45", "21:50", "21:55", "22:00", "22:05", "22:10", "22:15",
             "22:20", "22:25", "22:30", "22:35", "22:40", "22:45", "22:50", "22:55", "23:00", "23:05",
             "23:10", "23:15", "23:20", "23:25", "23:30", "23:35", "23:40", "23:45", "23:50", "23:55",
             "00:00", "00:05", "00:10", "00:15", "00:20", "00:25", "00:30", "00:35", "00:40", "00:45",
             "00:50", "00:55", "01:00" };
            List<string> horarionoche = new List<string>(horariosnoche);
            return horarionoche;
        }

        public List<string> horarios00a06()
        {
            string[] horariosMadrugada = {
                   "00:00", "00:05", "00:10", "00:15", "00:20", "00:25", "00:30", "00:35", "00:40", "00:45",
                 "00:50", "00:55","01:00", "01:05", "01:10", "01:15", "01:20", "01:25", "01:30", "01:35", "01:40", "01:45",
                  "01:50", "01:55", "02:00", "02:05", "02:10", "02:15", "02:20", "02:25", "02:30", "02:35",
                 "02:40", "02:45", "02:50", "02:55", "03:00", "03:05", "03:10", "03:15", "03:20", "03:25",
                  "03:30", "03:35", "03:40", "03:45", "03:50", "03:55", "04:00", "04:05", "04:10", "04:15",
                     "04:20", "04:25", "04:30", "04:35", "04:40", "04:45", "04:50", "04:55", "05:00", "05:05",
                  "05:10", "05:15", "05:20", "05:25", "05:30", "05:35", "05:40", "05:45", "05:50", "05:55",
                  "06:00"
                                 };
            List<string> horariomadrugada = new List<string>(horariosMadrugada);
            return horariomadrugada;
        }

        public string HoraToString(DateTime fechaactualizacion)
        {
            TimeSpan TimeSpan = new TimeSpan(fechaactualizacion.Hour, fechaactualizacion.Minute, fechaactualizacion.Second);
            int Redondeo = fechaactualizacion.Minute - fechaactualizacion.Minute % 5;
            string hora = fechaactualizacion.Hour.ToString("00") + ":" + Redondeo.ToString("00");
            return hora;
        }

        public int IndiceColumna(string hora)
        {
            //int columnaIndice = dataTable.Columns.IndexOf("Nombre");
            int indice = 0;
            List<string> listaHorarios = new List<string>(horarios());
            indice = listaHorarios.IndexOf(hora);
            return indice;
        }

        public int CantidadCasilleros(TimeSpan? TiempoCoccion)
        {

            TimeSpan timeSpan = (TimeSpan)TiempoCoccion;

            int casilleros = 0;
            casilleros = timeSpan.Minutes / 5;
            if (timeSpan.Minutes % 5 != 0)
            {
                casilleros++;
            }
            return casilleros;
        }

        public int ajusteportiempomaximopedido(int Tiempococcion, int Tiempomaximo)
        {
            if (Tiempococcion == Tiempomaximo)
            {
                return 0;
            }
            else
            {

                return Tiempomaximo - Tiempococcion;


            }


        }

      

        
    }
}





