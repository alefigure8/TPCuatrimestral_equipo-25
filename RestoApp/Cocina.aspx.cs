using Dominio;
using Helper;
using Negocio;
using Opciones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Reflection;
using System.Xml.Linq;
using System.Web.UI;
using System.Media;
using System.Net;
using System.IO;


namespace RestoApp
{
    public partial class Cocina : System.Web.UI.Page
    {
        public DateTime Reloj { get; set; }
        public Usuario usuario { get; set; }
        public CocinaNegocio cocinaNegocio { get; set; }
        public List<Pedido> Pedidosencocina { get; set; }
        public List<Pedido> Pedidosnuevos { get; set; }
        public List<ProductoPorPedido> Productosenpreparacion { get; set; }
        public int PedidosIngresados { get; set; }

        public bool sinproductos { get; set; }

        public bool PedidosnuevosOn { get; set; }

        public bool PlatosMarchandoOn { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];

            //if (AutentificacionUsuario.esCocinero(usuario))
            //{
                if (!IsPostBack)
                {
                    InstanciarObjetos();
                    CrearDatatableCocina();
                    CrearDataTableProductosEnPreparacion();
                    CrearDataTableEstadoPedidos();
                    CrearDataTablePedidosnuevos();
              
                    }




            ActualizarDGVPedidosnuevos();
            ActualizarCocina();
             ActualizarDGVCocina();
                ActualizarDGVEstadoPedidos();
                ActualizarDGVProductosenPreparacion();
                ActualizarDemorados();
     
           // }
        }


       


        public void ActualizarCocina()
        {
            cocinaNegocio = (CocinaNegocio)Session["CocinaNegocio"];
            ActualizarSolicitados(cocinaNegocio);
            Pedidosencocina = ListarPedidosenCocina(cocinaNegocio);

        }

        public void InstanciarObjetos()
        {
            
            PedidosnuevosOn = new bool ();
            PedidosnuevosOn = false;
            Session.Add("PedidosnuevosOn", PedidosnuevosOn);

            PlatosMarchandoOn = new bool();
            PlatosMarchandoOn = false;
            Session.Add("PlatosMarchandoOn", PlatosMarchandoOn);

      
            Reloj = DateTime.Now;
            Session.Add("Reloj", Reloj);

            cocinaNegocio = new CocinaNegocio();
            Session.Add("CocinaNegocio", cocinaNegocio);

            Pedidosencocina = new List<Pedido>();
            Session.Add("Pedidosencocina", Pedidosencocina);

            Pedidosnuevos = new List<Pedido>();
            Session.Add("Pedidosnuevos", Pedidosnuevos);

            PedidosIngresados = new int();
            Session.Add("PedidosIngresados", PedidosIngresados);
          

          


        }

        public void ActualizarSolicitados(CocinaNegocio cocinaNegocio)
        {
            cocinaNegocio.ActualizarSolicitados();
        }

        public List<Pedido> ListarPedidosenCocina(CocinaNegocio cocinaNegocio)
        {
         


           
            Pedidosencocina = (List<Pedido>)Session["Pedidosencocina"];
            List<Pedido> Auxiliarpedidos = new List<Pedido>();
            List<Pedido> pedidosnuevos = new List<Pedido>();

            Auxiliarpedidos = cocinaNegocio.ListarPedidosenCocina();
            Auxiliarpedidos = Auxiliarpedidos.OrderBy(Pedidosencocina => Pedidosencocina.Id).ToList();

            if(Auxiliarpedidos.Count()  == 0)
            {
             PedidosIngresados = 0;
              TxtPedidosIngresados.Text = PedidosIngresados.ToString();            
              Session.Add("PedidosIngresados",PedidosIngresados);  
            }

            TxtPedidosIngresados.BackColor = Color.White;
            PedidosIngresados = (int)Session["PedidosIngresados"];

         

            if ((Auxiliarpedidos.ToList().Count() - Pedidosencocina.Count()) > 0)
            {
                if (PedidosIngresados < PedidosIngresados + (Auxiliarpedidos.ToList().Count() - Pedidosencocina.Count()))
                {
                   
                    PedidosIngresados += Auxiliarpedidos.ToList().Count() - Pedidosencocina.Count();
                    SonarCampana();
                 

                }
            }
            TxtPedidosIngresados.Text = PedidosIngresados.ToString();
            Session.Add("PedidosIngresados", PedidosIngresados);                  

            

            foreach (Pedido pedido in Auxiliarpedidos)
            {

          
                
                // POR CADA PEDIDO LISTAMOS SUS PRODUCTOS
                pedido.Productossolicitados = cocinaNegocio.ListarProductosPorPedido(pedido.Id);
                

                // DETERMINAMOS CUAL DE LOS PRODUCTOS TIENE MAYOR TIEMPO DE COCCION
                TimeSpan? Tiempomax = TimeSpan.Zero;
                foreach (ProductoPorPedido producto in pedido.Productossolicitados)
                {
                    if (producto.Productodeldia.TiempoCoccion > Tiempomax)
                    {
                        Tiempomax = producto.Productodeldia.TiempoCoccion;
                    }           

                }
           
                
                // SI EL ESTADO ESTA EN PREPARACION LA HORA INGRESO ES LA ULTIMA ACTUALIZACION
                if (pedido.Estado == Estados.EnPreparacion)
                {
                    pedido.Horaingresococina = pedido.ultimaactualizacion;
                }
                else
                {

                    pedido.Horaingresococina = cocinaNegocio.BuscarhoraingresoCocina(pedido.Id);

                }
               // else
               // {
                    // SI ESTA DEMORADO ES LA RESTA DE SU ULTIMA ACTUALIZACION Y EL TIEMPO MAXIMO DE COCCION 
                 //   pedido.Horaingresococina = pedido.ultimaactualizacion - (TimeSpan)Tiempomax;
                //}

                // DETERMINAMOS LA CANTIDAD DE CASILLEROS QUE OCUPA EL PRODUCTO DE MAS COCCION
                int casillerostiempomax = CantidadCasilleros(Tiempomax.Value);


                foreach (ProductoPorPedido producto in pedido.Productossolicitados)
                
                {
                    producto.HoraListo = pedido.Horaingresococina.Add((TimeSpan)producto.Productodeldia.TiempoCoccion);
                                                      
                }
       
                pedido.Horalisto = pedido.Horaingresococina.Add(Tiempomax.Value);
              
            }

            /*
            if (Auxiliarproductosenpreparacion.ToList() != Productosenpreparacion.ToList())
            {
                Session.Add("Productosenpreparacion", Auxiliarproductosenpreparacion);
            }
            */


            Session.Add("Pedidosencocina", Auxiliarpedidos);
            return Pedidosencocina;
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
        public DataTable CrearDataTableProductosEnPreparacion()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Producto", typeof(string));
            dataTable.Columns.Add("Cantidad", typeof(int));
            dataTable.Columns.Add("Proximo", typeof(string));
            Session.Add("DTProductosenpreparacion", dataTable);
            return dataTable;

        }

        public DataTable CrearDataTableEstadoPedidos()
        {
            DataTable DTEstadopedidos = new DataTable();
            DTEstadopedidos.Columns.Add("Pedido", typeof(string));
            DTEstadopedidos.Columns.Add("Estado", typeof(string));
            DTEstadopedidos.Columns.Add("Listo", typeof(string));
            Session.Add("DTEstadoPedidos", DTEstadopedidos);
            return DTEstadopedidos;
        }

        public DataTable CrearDataTablePedidosnuevos() 
        {
            DataTable DTPedidosnuevos = new DataTable();
            DTPedidosnuevos.Columns.Add("#ID", typeof(int));
            DTPedidosnuevos.Columns.Add("Nombre", typeof(string));
                DTPedidosnuevos.Columns.Add("Cantidad", typeof(string));
                DTPedidosnuevos.Columns.Add("Coccion", typeof(string));    

            Session.Add("DTPedidosnuevos", DTPedidosnuevos);
            return DTPedidosnuevos;

        }

        public void ActualizarDGVPedidosnuevos()
        {
     
            DataTable DTPedidosnuevos = (DataTable)Session["DTPedidosnuevos"];
            Pedidosencocina = Session["Pedidosencocina"] as List<Pedido>;
            PedidosIngresados = (int)Session["PedidosIngresados"];
            PedidosnuevosOn = (bool)Session["PedidosnuevosOn"];



            DTPedidosnuevos.Clear();            

            if (PedidosIngresados > 0) { 

            Pedidosencocina.ToList().OrderByDescending(Pedidosencocina => Pedidosencocina.Id).ToList().Take(PedidosIngresados);
        

            foreach (Pedido p in Pedidosencocina) {

               
            foreach (ProductoPorPedido pxp in p.Productossolicitados)
            {
                 DataRow row = DTPedidosnuevos.NewRow();

                   row["#ID"] = p.Id;
                row["Nombre"] = pxp.Productodeldia.Nombre;
                row["Cantidad"] = pxp.Cantidad;
                row["Coccion"] = pxp.Productodeldia.TiempoCoccion.Value.Minutes.ToString() + " Min.";

                DTPedidosnuevos.Rows.Add(row);
            }
               
            }
                     
            }
            
            DGVPedidosnuevos.DataSource = DTPedidosnuevos;
            DGVPedidosnuevos.DataBind();
        }
              
        public void ActualizarDGVEstadoPedidos()
        {
            DataTable DTEstadopedidos = (DataTable)Session["DTEstadoPedidos"];
            Pedidosencocina = Session["Pedidosencocina"] as List<Pedido>;
            Pedidosencocina.OrderByDescending(Pedidosencocina => Pedidosencocina.Estado);
            DTEstadopedidos.Rows.Clear();

            foreach (Pedido pedido in Pedidosencocina)
            {
                DataRow row = DTEstadopedidos.NewRow();
                row["Pedido"] = "#" + pedido.Id;
                row["Estado"] = pedido.EstadoDescripcion;
                row["Listo"] = pedido.Horalisto.TimeOfDay;             
                
                    
                          
                DTEstadopedidos.Rows.Add(row);
            }
            GVDEstadopedidos.DataSource = DTEstadopedidos;
            GVDEstadopedidos.DataBind();           

        }

        public void SonarCampana()
        {
            string urlArchivoSonido = "http://djoolien.free.fr/conneries/sons/deskbell.wav";

            using (WebClient webClient = new WebClient())
            {
                byte[] archivoSonidoBytes = webClient.DownloadData(urlArchivoSonido);

                using (MemoryStream memoryStream = new MemoryStream(archivoSonidoBytes))
                {
                    SoundPlayer player = new SoundPlayer(memoryStream);
                    player.Play();
                }
            }
        }

        public void ActualizarDemorados()
        {
           
            Reloj = (DateTime)Session["Reloj"];
            Pedidosencocina = Session["Pedidosencocina"] as List<Pedido>;
            PedidoNegocio pedidoNegocio = new PedidoNegocio();

            foreach (Pedido p in Pedidosencocina.ToList())
            {
                if (p.Horalisto < Reloj)
                    pedidoNegocio.CambiarEstadoPedido(p.Id, Estados.DemoradoEnCocina, Reloj);
            }

        }
        public int CantidadCasilleros(TimeSpan? TiempoCoccion)
        {

            TimeSpan timeSpan = (TimeSpan)TiempoCoccion;

            int casilleros = 0;
            casilleros = timeSpan.Minutes / 5;
            if (timeSpan.Minutes % 5 > 1)
            {
                casilleros++;
            }
            return casilleros;
        }


        public void ActualizarDGVProductosenPreparacion()
        {
          


            // RECUPERA DATATABLE CREADA
            DataTable dataTable = (DataTable)Session["DTProductosenpreparacion"];
            Pedidosencocina = Session["Pedidosencocina"] as List<Pedido>;
            PlatosMarchandoOn = (bool)Session["PlatosMarchandoOn"];
          
            dataTable.Rows.Clear();
            
            if(PlatosMarchandoOn == true) { 
            Dictionary<string, int> productosAgrupados = new Dictionary<string, int>();

        
            foreach (Pedido p in Pedidosencocina)
            {

            // Recorrer la lista productosenpreparacion
            foreach (ProductoPorPedido pxp in p.Productossolicitados)
            {
                 string nombreProducto = pxp.Productodeldia.Nombre;
               

                  
                    // Verificar si el producto ya existe en el diccionario
                    if (productosAgrupados.ContainsKey(nombreProducto))
                    {
                    // Si existe, incrementar la cantidad
                  
                     productosAgrupados[nombreProducto] += pxp.Cantidad;

                    }
                    else
                    {
                        
                    // Si no existe, agregar el producto al diccionario con la cantidad inicial
                    productosAgrupados.Add(nombreProducto, pxp.Cantidad);
                    }

            
            }



            }
            foreach (KeyValuePair<string, int> producto in productosAgrupados)
            {
                string nombreProducto = producto.Key;
                int cantidad = producto.Value;

                DateTime minhorafin = new DateTime();
                string Horafin;
                bool bandera = false;
                foreach (Pedido p in Pedidosencocina)
                {
                    foreach (ProductoPorPedido pxp in p.Productossolicitados)
                    {
                        if (pxp.Productodeldia.Nombre == nombreProducto)
                        {
                            if (!bandera)
                            {
                                minhorafin = pxp.HoraListo;
                                bandera = true;
                            }
                            else {
                                if (pxp.HoraListo < minhorafin)
                            {
                                    minhorafin = pxp.HoraListo;
                                }
                            }

                        }
                    }
                }


               
                Horafin=  minhorafin.TimeOfDay.ToString();


                DataRow filaNueva = dataTable.NewRow();
                filaNueva[0] = nombreProducto;
                filaNueva[1] = cantidad;
                filaNueva[2] = Horafin;
                dataTable.Rows.Add(filaNueva);         
            }
            }

            GVDProductosenprep.DataSource = dataTable;
            GVDProductosenprep.DataBind();

        }



        public void ActualizarDGVCocina()
        {

            DataTable DTCocina = (DataTable)Session["DTCocina"];
            Pedidosencocina = Session["Pedidosencocina"] as List<Pedido>;

            DTCocina.Rows.Clear();
            GVDCocina.DataSource = DTCocina;
            GVDCocina.DataBind();

            DTCocina.Rows.Clear();

            foreach (var pedido in Pedidosencocina)
            {
                if (pedido.Estado != Estados.ListoParaEntregar) { 
                if (horarios().Contains(HoraToString(pedido.Horaingresococina)))
                {
                    // BUSCA EL PRODUCTO DENTRO DEL PEDIDO QUE TENGA MAYOR TIEMPO DE COCCION
                    int CasillerosMaxTiempoCoccion = CantidadCasilleros(pedido.Horalisto - pedido.Horaingresococina);

                    for (int i = 0; i < pedido.Productossolicitados.Count; i++)
                    {
                        ProductoPorPedido ProductossolicitadopsProductodeldia = pedido.Productossolicitados[i];

                        int Ajuste = ajusteportiempomaximopedido(CantidadCasilleros(pedido.Productossolicitados[i].Productodeldia.TiempoCoccion), CasillerosMaxTiempoCoccion);

                        DataRow filaNueva = DTCocina.NewRow();
                        filaNueva[Ajuste + IndiceColumna(HoraToString(pedido.Horaingresococina))] = "#" + pedido.Id;
                        filaNueva[IndiceColumna(HoraToString(pedido.Horaingresococina)) + CasillerosMaxTiempoCoccion - 2] = ProductossolicitadopsProductodeldia.Productodeldia.Nombre;
                        filaNueva[IndiceColumna(HoraToString(pedido.Horaingresococina)) + CasillerosMaxTiempoCoccion - 1] = "C:" + ProductossolicitadopsProductodeldia.Cantidad;
                        DTCocina.Rows.Add(filaNueva);

                    }
                }
                }
                GVDCocina.DataSource = DTCocina;
                GVDCocina.DataBind();
            }
        }

       public void GVDEstadopedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            if (e.CommandName == "ListoparaEntregar")
            {
            
                Pedidosencocina = Session["Pedidosencocina"] as List<Pedido>;
                Reloj = (DateTime)Session["Reloj"];
                    
                int rowIndex = Convert.ToInt32(e.CommandArgument);

                Pedidosencocina[rowIndex].Estado = Estados.ListoParaEntregar;

                if (rowIndex==0 && Pedidosencocina.ToList().Count()==1)
                {
                    GVDCocina.DataSource = null;
                    GVDCocina.DataBind();      
                }
           
            
                Session.Add("Pedidosencocina", Pedidosencocina);
                PedidoNegocio PedidoNegocio = new PedidoNegocio();
                PedidoNegocio.CambiarEstadoPedido(Pedidosencocina[rowIndex].Id, Estados.ListoParaEntregar, Reloj);

             
            }
        }
        
    public void Timer1_Tick(object sender, EventArgs e)
    {

        Pedidosencocina = Session["Pedidosencocina"] as List<Pedido>;
        
       if (Pedidosencocina.Count == 0)
       
            { 

                Page_Load(sender, e);
         }


            Reloj = (DateTime)Session["Reloj"];
        Reloj = Reloj.AddSeconds(120);
        Txtreloj.Text = Reloj.ToString("HH:mm");
        Session.Add("Reloj", Reloj);

        
            /*
            int contador = 0;
            if (Session["Contador"] != null)
            {
                    contador = (int)Session["Contador"];
           }

           contador++;
           Session["Contador"] = contador;

           if (contador % 5 == 0)
                {*/
            //ActualizarCocina();
            //ActualizarDGVCocina();
            //ActualizarDGVEstadoPedidos();
            //ActualizarDGVProductosenPreparacion();
            //ActualizarDemorados();
            // }



        }


        protected void GVDCocina_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Reloj = (DateTime)Session["Reloj"];
             int? Indicecolumnahora = IndiceColumna(HoraToString(Reloj));


            // AGREGO BORDE DERECHO A CADA CELDA PARA GENERA LINEAS VERTICALES
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell cell in e.Row.Cells)

                {
                    cell.Style["font-type"] = "bold";
                    cell.Style["font-color"] = "white";
                    cell.Style["font-size"] = "12px";
                    cell.Style["text-align"] = "center";
                    cell.Style["border-right"] = "none";
                    cell.Style["border-right"] = "1px solid #565d63";
                }
            }


          

            Pedidosencocina = (List<Pedido>)Session["Pedidosencocina"];
            int rowindex = 0;

            foreach (var pedido in Pedidosencocina)
            {
                if (horarios().Contains(HoraToString(pedido.Horaingresococina)))
                {
                    // BUSCA EL PRODUCTO DENTRO DEL PEDIDO QUE TENGA MAYOR TIEMPO DE COCCION
                    int CasillerosMaxTiempoCoccion = CantidadCasilleros(pedido.Horalisto - pedido.Horaingresococina);

                    foreach (ProductoPorPedido pxp in pedido.Productossolicitados)
                    {

                        if (e.Row.RowIndex == rowindex)
                        {
                            int Ajuste = ajusteportiempomaximopedido(CantidadCasilleros(pxp.Productodeldia.TiempoCoccion), CasillerosMaxTiempoCoccion);
                            int seed = pedido.Id.GetHashCode();
                            Random random = new Random(seed);
                            Color randomColor = Color.FromArgb(50, random.Next(100, 200), random.Next(100, 200), random.Next(100, 200));
                            double factor = 0.8;
                            int red = (int)(randomColor.R * factor);
                            int green = (int)(randomColor.G * factor);
                            int blue = (int)(randomColor.B * factor);
                            Color darkColor = Color.FromArgb(red, green, blue);
                            for (int i = 0; i < CantidadCasilleros(pxp.Productodeldia.TiempoCoccion); i++)
                            {
                                if ((Ajuste + IndiceColumna(HoraToString(pedido.Horaingresococina)) + i) >= Indicecolumnahora)
                                {
                                    e.Row.Cells[Ajuste + IndiceColumna(HoraToString(pedido.Horaingresococina)) + i].BackColor = darkColor;
                                }
                                else
                                {
                                    e.Row.Cells[Ajuste + IndiceColumna(HoraToString(pedido.Horaingresococina)) + i].BackColor = Color.Gray;

                                }
                                /*
                                if (Indicecolumnahora == null)
                                {
                                    e.Row.Cells[Ajuste + IndiceColumna(HoraToString(pedido.Horaingresococina)) + i].BackColor = Color.Gray;
                                }
                                */
                            }
                           
                        }
                        rowindex++;

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
            Reloj = (DateTime)Session["Reloj"];

            /*if (Reloj.Hour > 0 && Reloj.Hour < 01)
            {
                if (Reloj.Minute > 0)
                {
                    return horarios0a1A();
                }
                else if (Reloj.Minute > 20)
                {

                    return horarios0a1B();
                }
                else
                {

                    return horarios0a1C();
                }
            }
            */
           
                if (Reloj.Minute > 40)
                {
                    return horarios1a2C();
                }
               else
                {

                    return horarios1a2B();
                }
              
                                
              

                  
                

          
           /* else if (Reloj.Hour >= 4 && Reloj.Hour < 8)
            {
                return horarios4a8();
            }
            else if (Reloj.Hour >= 8 && Reloj.Hour < 16)
            {
                return horarios8a16();
            }
            else if (Reloj.Hour >= 22 && Reloj.Hour < 23)
            {


                if (Reloj.Minute > 40)
                {
                    return horarios22a23C();
                }
                else if (Reloj.Minute > 20)
                {
                    return horarios22a23B();
                }
                else;
                {
                    return horarios0a4();
                }

            }
            else if (Reloj.Hour >= 23 && Reloj.Hour < 24)
            {
                if (Reloj.Minute > 40)
                {
                    return horarios23a24C();
                }
                else if (Reloj.Minute > 20)
                {
                    return horarios23a24B();
                }
                else;
                {
                    return horarios23a24A();
                }

            }
           else
            {
                return horarios16a24();
            }
            
            
            */

        }





        public List<string> horarios22a23B()
        {
            string[] horarios = {
   
    
        "21:20", "21:25", "21:30", "21:35", "21:40", "21:45", "21:50", "21:55", "22:00", "22:05", "22:10", "22:15", "22:20", "22:25", "22:30", "22:35",
        "22:40", "22:45", "22:50", "22:55", "23:00", "23:05", "23:10", "23:15", "23:20",
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios22a23C()
        {
            string[] horarios = {

         "21:20", "21:25", "21:30", "21:35","21:40", "21:45", "21:50", "21:55", "22:00", "22:05", "22:10", "22:15", "22:20", "22:25", "22:30", "22:35",
        "22:40", "22:45", "22:50", "22:55", "23:00", "23:05", "23:10", "23:15", "23:20" , "23:25", "23:30", "23:35", "23:40",  "23:45",
                "23:50", "23:55", "00:00", "00:05", "00:10", "00:15", "00:20"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios23a24A()
        {
            string[] horarios = {


       "21:40", "21:45", "21:50", "21:55", "22:00", "22:05", "22:10", "22:15", "22:20", "22:25", "22:30", "22:35",
        "22:40", "22:45", "22:50", "22:55", "23:00", "23:05", "23:10", "23:15", "23:20", "23:25", "23:30", "23:35", "23:40",  "23:45",
                "23:50", "23:55", "00:00", "00:05", "00:10", "00:15", "00:20","00:25", "00:30", "00:35",
                "00:40"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios23a24B()
        {
            string[] horarios = {
    "22:00", "22:05", "22:10", "22:15", "22:20", "22:25", "22:30", "22:35",
        "22:40", "22:45", "22:50", "22:55", "23:00", "23:05", "23:10", "23:15", "23:20", "23:25", "23:30", "23:35", "23:40",  "23:45",
                "23:50", "23:55", "00:00", "00:05", "00:10", "00:15", "00:20","00:25", "00:30", "00:35",
                "00:40","00:45", "00:50", "00:55", "01:00"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }
        public List<string> horarios23a24C()
        {
            string[] horarios = {
  
        "22:40", "22:45", "22:50", "22:55", "23:00", "23:05", "23:10", "23:15", "23:20", "23:25", "23:30", "23:35", "23:40",  "23:45",
                "23:50", "23:55", "00:00", "00:05", "00:10", "00:15", "00:20","00:25", "00:30", "00:35",
                "00:40","00:45", "00:50", "00:55", "01:00", "01:05", "01:10", "01:15", "01:20", "01:25", "01:30", "01:35", "01:40"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios0a1A()
        {
            string[] horarios = {
        "23:40", "23:45", "23:50", "23:55", "00:00", "00:05", "00:10", "00:15", "00:20", "00:25", "00:30", "00:35",
        "00:40", "00:45", "00:50", "00:55", "01:00", "01:05", "01:10", "01:15", "01:20", "01:25", "01:30", "01:35",
        "01:40", "01:45", "01:50", "01:55", "02:00", "02:05", "02:10", "02:15", "02:20", "02:25", "02:30", "02:35"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios0a1B()
        {
            string[] horarios = {
        "23:55", "00:00", "00:05", "00:10", "00:15", "00:20", "00:25", "00:30", "00:35", "00:40", "00:45", "00:50",
        "00:55", "01:00", "01:05", "01:10", "01:15", "01:20", "01:25", "01:30", "01:35", "01:40", "01:45", "01:50",
        "01:55", "02:00", "02:05", "02:10", "02:15", "02:20", "02:25", "02:30", "02:35", "02:40", "02:45", "02:50"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios0a1C()
        {
            string[] horarios = {
        "00:10", "00:15", "00:20", "00:25", "00:30", "00:35", "00:40", "00:45", "00:50", "00:55", "01:00", "01:05",
        "01:10", "01:15", "01:20", "01:25", "01:30", "01:35", "01:40", "01:45", "01:50", "01:55", "02:00", "02:05",
        "02:10", "02:15", "02:20", "02:25", "02:30", "02:35", "02:40", "02:45", "02:50", "02:55", "03:00", "03:05"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios1a2A()
        {
            string[] horarios = {
        "00:40", "00:45", "00:50", "00:55", "01:00", "01:05", "01:10", "01:15", "01:20", "01:25", "01:30", "01:35",
        "01:40", "01:45", "01:50", "01:55", "02:00", "02:05", "02:10", "02:15", "02:20", "02:25", "02:30", "02:35",
        "02:40", "02:45", "02:50", "02:55", "03:00", "03:05", "03:10", "03:15", "03:20", "03:25", "03:30", "03:35"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios1a2B()
        {
            string[] horarios = {
        "01:00", "01:05", "01:10", "01:15", "01:20", "01:25", "01:30", "01:35", "01:40", "01:45", "01:50", "01:55",
        "02:00", "02:05", "02:10", "02:15", "02:20", "02:25", "02:30", "02:35", "02:40", "02:45", "02:50", "02:55",
        "03:00", "03:05", "03:10", "03:15", "03:20", "03:25", "03:30", "03:35", "03:40", "03:45", "03:50", "03:55"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios1a2C()
        {
            string[] horarios = {
        "01:20", "01:25", "01:30", "01:35", "01:40", "01:45", "01:50", "01:55", "02:00", "02:05", "02:10", "02:15",
        "02:20", "02:25", "02:30", "02:35", "02:40", "02:45", "02:50", "02:55", "03:00", "03:05", "03:10", "03:15",
        "03:20", "03:25", "03:30", "03:35", "03:40", "03:45", "03:50", "03:55", "04:00", "04:05", "04:10", "04:15"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }




        public List<string> horarios0a4()
        {
            string[] horariosNoche = {
                "23:00", "23:05", "23:10", "23:15", "23:20", "23:25", "23:30", "23:35", "23:40", "23:45",
                "23:50", "23:55", "00:00", "00:05", "00:10", "00:15", "00:20", "00:25", "00:30", "00:35",
                "00:40", "00:45", "00:50", "00:55", "01:00",
                "01:05", "01:10", "01:15", "01:20", "01:25", "01:30", "01:35", "01:40", "01:45", "01:50", "01:55", "02:00",
                  "02:05", "02:10", "02:15", "02:20", "02:25", "02:30", "02:35",
                    "02:40", "02:45", "02:50", "02:55", "03:00", "03:05", "03:10", "03:15", "03:20", "03:25", "03:30", "03:35",
                    "03:40", "03:45", "03:50", "03:55", "04:00", "04:05", "04:10", "04:15", "04:20",   };
          
                List<string> listaHorarios = new List<string>(horariosNoche);
            return listaHorarios;

        }




        public List<string> horarios4a8()
        {
            string[] horarios = {
        "04:00", "04:05", "04:10", "04:15", "04:20", "04:25", "04:30", "04:35",
        "04:40", "04:45", "04:50", "04:55", "05:00", "05:05", "05:10", "05:15", "05:20", "05:25", "05:30", "05:35",
        "05:40", "05:45", "05:50", "05:55", "06:00","06:05", "06:10", "06:15", "06:20", "06:25", "06:30", "06:35",
        "06:40", "06:45", "06:50", "06:55", "07:00",  "07:05", "07:10", "07:15", "07:20", "07:25", "07:30", "07:35",
        "07:40", "07:45", "07:50", "07:55", "08:00"
         };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }
         
        public List<string> horarios8a16()
        {
            string[] horarios = {
         "08:00", "08:05", "08:10", "08:15", "08:20", "08:25", "08:30", "08:35",
        "08:40", "08:45", "08:50", "08:55", "09:00","09:05", "09:10", "09:15", "09:20", "09:25", "09:30", "09:35",
        "09:40", "09:45", "09:50", "09:55", "10:00","10:05", "10:10", "10:15", "10:20", "10:25", "10:30", "10:35",
        "10:40", "10:45", "10:50", "10:55", "11:00", "11:05", "11:10", "11:15", "11:20", "11:25", "11:30", "11:35",
        "11:40", "11:45", "11:50", "11:55", "12:00", "12:05", "12:10", "12:15", "12:20", "12:25", "12:30", "12:35",
        "12:40", "12:45", "12:50", "12:55", "13:00", "13:05", "13:10", "13:15", "13:20", "13:25", "13:30", "13:35",
        "13:40", "13:45", "13:50", "13:55", "14:00",  "14:05", "14:10", "14:15", "14:20", "14:25", "14:30", "14:35",
        "14:40", "14:45", "14:50", "14:55", "15:00", "15:05", "15:10", "15:15", "15:20", "15:25", "15:30", "15:35",
        "15:40", "15:45", "15:50", "15:55", "16:00"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

        public List<string> horarios16a24()
        {
            string[] horarios = {
        "14:00", "14:05", "14:10", "14:15", "14:20", "14:25", "14:30", "14:35", "14:40", "14:45",
        "14:50", "14:55", "15:00", "15:05", "15:10", "15:15", "15:20", "15:25", "15:30", "15:35",
        "15:40", "15:45", "15:50", "15:55", "16:00",  "16:05", "16:10", "16:15", "16:20", "16:25", "16:30", "16:35",
        "16:40", "16:45", "16:50", "16:55", "17:00","17:05", "17:10", "17:15", "17:20", "17:25", "17:30", "17:35",
        "17:40", "17:45", "17:50", "17:55", "18:00", "18:05", "18:10", "18:15", "18:20", "18:25", "18:30", "18:35",
        "18:40", "18:45", "18:50", "18:55", "19:00", "19:05", "19:10", "19:15", "19:20", "19:25", "19:30", "19:35",
        "19:40", "19:45", "19:50", "19:55", "20:00", "20:05", "20:10", "20:15", "20:20", "20:25", "20:30", "20:35",
        "20:40", "20:45", "20:50", "20:55", "21:00", "21:05", "21:10", "21:15", "21:20", "21:25", "21:30", "21:35",
        "21:40", "21:45", "21:50", "21:55", "22:00", "22:05", "22:10", "22:15", "22:20", "22:25", "22:30", "22:35",
        "22:40", "22:45", "22:50", "22:55", "23:00", "23:05", "23:10", "23:15", "23:20", "23:25", "23:30", "23:35",
        "23:40", "23:45", "23:50", "23:55", "00:00"
    };

            List<string> listaHorarios = new List<string>(horarios);
            return listaHorarios;
        }

       


        public string HoraToString(DateTime fechaactualizacion)
        {
            int Redondeo;
            TimeSpan TimeSpan = new TimeSpan(fechaactualizacion.Hour, fechaactualizacion.Minute, fechaactualizacion.Second);
            if (TimeSpan.Minutes % 5 >= 3)
            {
                Redondeo = fechaactualizacion.Minute - fechaactualizacion.Minute % 5 + 5;
            }
            else
            {
                Redondeo = fechaactualizacion.Minute - fechaactualizacion.Minute % 5;

            }

            string hora = fechaactualizacion.Hour.ToString("00") + ":" + Redondeo.ToString("00");
            return hora;
        }


        public TimeSpan HoraRedondeada(DateTime fechaactualizacion)
        {

            int minutos;


            minutos = fechaactualizacion.Minute - fechaactualizacion.Minute % 5;


            TimeSpan Timespan = new TimeSpan(fechaactualizacion.Hour, minutos, fechaactualizacion.Second);

            return Timespan;
        }

        public int IndiceColumna(string hora)
        {
            //int columnaIndice = dataTable.Columns.IndexOf("Nombre");
            int indice;
            List<string> listaHorarios = new List<string>(horarios());
            indice = listaHorarios.IndexOf(hora);
            if(indice == -1)
            {
                return listaHorarios.Count();
            }
            else { 
            return indice+1;
            }
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



        public void GVDEstadopedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Pedidosencocina = Session["Pedidosencocina"] as List<Pedido>;
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    if (Pedidosencocina  [e.Row.RowIndex].Estado == Estados.EnPreparacion)
                    {
                        cell.BackColor = System.Drawing.Color.LightGreen;

                    }
                    else
                    {

                       // LinkButton btnInformarDemora = (LinkButton)e.Row.FindControl("InformarDemora");
                        //btnInformarDemora.Visible = true;
                        //btnInformarDemora.Enabled = true;
                        cell.BackColor = System.Drawing.Color.PaleVioletRed;

                    }



                    cell.Style["font-size"] = "12px";
                    cell.Style["text-align"] = "center";

                }
            }
        }
        public void GVDProductosenprep_RowDataBound(object sender, GridViewRowEventArgs e)
        {
         
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.Style["font-size"] = "12px";
                    cell.Style["text-align"] = "center";

                }
            }
        }


        protected void BtnDetalle_Click(object sender, EventArgs e)
        {

            if ((bool)Session["PedidosnuevosOn"])
            {
                PedidosnuevosOn = false;
                Session.Add("PedidosnuevosOn", PedidosnuevosOn);
                DGVPedidosnuevos.DataSource = null;
                DGVPedidosnuevos.DataBind();
            }
            else
            {
                PedidosnuevosOn = true;
                Session.Add("PedidosnuevosOn", PedidosnuevosOn);
                ActualizarDGVPedidosnuevos();
            }
                                  

            
        }
             

        protected void BtnConfirmarIngresos_Click(object sender, EventArgs e)
        {
            PedidosIngresados = 0;
            Session.Add("PedidosIngresados",PedidosIngresados);

            
             if ((bool)Session["PlatosMarchandoOn"]) {
                PedidosnuevosOn = false;
                Session.Add("PlatosMarchandoOn", PedidosnuevosOn);

            }

        }

        protected void BtnDisplaygvdPenP_Click(object sender, EventArgs e)
        {
            if ((bool)Session["PlatosMarchandoOn"])
            {
                PedidosnuevosOn = false;
                Session.Add("PlatosMarchandoOn", PedidosnuevosOn);

            }
            else
            {
                PedidosnuevosOn = true;
                Session.Add("PlatosMarchandoOn", PedidosnuevosOn);

            }
        }

        protected void DGVPedidosnuevos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.Style["font-size"] = "12px";
                    cell.Style["text-align"] = "center";

                }
            }

        }
    }
}





