﻿using Dominio;
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
namespace RestoApp
{
    public partial class Cocina : System.Web.UI.Page
    {
        public DateTime Reloj { get; set; }

        public List<Pedido> Pedidosencocina { get; set; }

        public List<FilasxColumnasxTiempoCoccion> ListaFxCxT { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                Reloj = DateTime.Now;
                //Session.Add("Reloj", Reloj);
                creardatatable();
                CargarPedidosaDataTable();
            }

          

            GVDCocina.DataSource = (DataTable)Session["datatable"];
            GVDCocina.DataBind();
        }


        public DataTable creardatatable()
        {

            DataTable dataTable = new DataTable();

            foreach (string horario in horariosMadrugada())
            {
                dataTable.Columns.Add(horario, typeof(string));

            }

            Session.Add("datatable", dataTable);
             return dataTable;

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
            "12:00" };

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

        public void Timer1_Tick(object sender, EventArgs e)
        {
            // Reloj = (DateTime)Session["Reloj"];
            Reloj = Reloj.AddSeconds(60);
            Txtreloj.Text = Reloj.ToString("HH:mm");
            // Session.Add("Reloj", Reloj);
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
             "00:50", "00:55", "01:00" };

            return horariosnoche;
        }

        public string[] horariosMadrugada()
        {
            string[] horariosMadrugada = {
        "22:20", "22:25", "22:30", "22:35", "22:40", "22:45", "22:50", "22:55", "23:00", "23:05",
             "23:10", "23:15", "23:20", "23:25", "23:30", "23:35", "23:40", "23:45", "23:50", "23:55",
             "00:00", "00:05", "00:10", "00:15", "00:20", "00:25", "00:30", "00:35", "00:40", "00:45",
             "00:50", "00:55", "01:00", "01:05", "01:10", "01:15", "01:20", "01:25", "01:30", "01:35", "01:40", "01:45",
        "01:50", "01:55", "02:00", "02:05", "02:10", "02:15", "02:20", "02:25", "02:30", "02:35",
        "02:40", "02:45", "02:50", "02:55", "03:00", "03:05", "03:10", "03:15", "03:20", "03:25",
        "03:30", "03:35", "03:40", "03:45", "03:50", "03:55", "04:00", "04:05", "04:10", "04:15",
        "04:20", "04:25", "04:30", "04:35", "04:40", "04:45", "04:50", "04:55", "05:00", "05:05",
        "05:10", "05:15", "05:20", "05:25", "05:30", "05:35", "05:40", "05:45", "05:50", "05:55",
        "06:00"
    };

            return horariosMadrugada;
        }
        /*
        public string convertidorfechaahora(DateTime ahora)
        {
        }
        */
        public string HoraIngresoPedido(DateTime fechaactualizacion)
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
            List<string> listaHorarios = new List<string>(horariosMadrugada());
            indice = listaHorarios.IndexOf(hora);
            return indice;
        }

        public int CantidadCasilleros(TimeSpan? TiempoCoccion)
        {
           
            TimeSpan timeSpan = (TimeSpan)TiempoCoccion;
                        
            int casilleros = 0;
            casilleros = timeSpan.Minutes / 5;
            return casilleros;
        }

        public void CargarPedidosaDataTable()
        {
            DataTable dataTable = (DataTable)Session["datatable"];
            PedidoNegocio pedidoNegocio = new PedidoNegocio();
            List<Pedido> pedidos = pedidoNegocio.ListarPedidos();

            foreach (var pedido in pedidos)
            {

                string fecha = pedido.ultimaactualizacion.Day.ToString("00") + "/" + pedido.ultimaactualizacion.Month.ToString("00") + "/" + pedido.ultimaactualizacion.Year.ToString("0000");


                if (pedido.Estado == Estados.Solicitado && fecha == DateTime.Today.ToString("d"))
                {

                    //if (ultimaactualizacion.Date == DateTime.Today)
                    foreach (ProductoPorPedido producto in pedido.Productossolicitados)
                    {
                        if (producto.Productodeldia.Categoria == CategoriasProductos.Alimentos )
                        {
                            DataRow filaNueva = dataTable.NewRow();

                            filaNueva[IndiceColumna(HoraIngresoPedido(pedido.ultimaactualizacion))] = pedido.Id;
                            filaNueva[IndiceColumna(HoraIngresoPedido(pedido.ultimaactualizacion))+1] = producto.Productodeldia.Nombre;
                            filaNueva[IndiceColumna(HoraIngresoPedido(pedido.ultimaactualizacion))+2] = producto.Cantidad;
                            dataTable.Rows.Add(filaNueva);
                            FilasxColumnasxTiempoCoccion filasxColumnasxTiempoCoccion = new FilasxColumnasxTiempoCoccion();
                            filasxColumnasxTiempoCoccion.Fila = dataTable.Rows.IndexOf(filaNueva);
                            filasxColumnasxTiempoCoccion.Columna = IndiceColumna(HoraIngresoPedido(pedido.ultimaactualizacion));
                            filasxColumnasxTiempoCoccion.TiempoCoccion = CantidadCasilleros(producto.Productodeldia.TiempoCoccion);


                            if (Session["ListaFxCxT"] == null)
                            {

                                List<FilasxColumnasxTiempoCoccion> ListaFxCxT = new List<FilasxColumnasxTiempoCoccion>();
                                    ListaFxCxT.Add(filasxColumnasxTiempoCoccion);
                                    Session.Add("ListaFxCxT", ListaFxCxT);
                                
                            }
                            else
                            {
                                ListaFxCxT = Session["ListaFxCxT"] as List<FilasxColumnasxTiempoCoccion>;
                                ListaFxCxT.Add(filasxColumnasxTiempoCoccion);
                                Session.Add("ListaFxCxT", ListaFxCxT);

                            }
                           
                          

                            //filaNueva[IndiceColumna(HoraIngresoPedido(pedido.ultimaactualizacion))]                            
                        
                            int numeroFila = dataTable.Rows.IndexOf(filaNueva);
                        }

                        

                    }
                    pedidoNegocio.CambiarEstadoPedido(pedido.Id, Estados.EnPreparacion);
                }
                
            }
            GVDCocina.DataSource = dataTable;
            Session.Add("datatable", dataTable);
            GVDCocina.DataBind();
        }




        protected void GVDCocina_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // DataTable dataTable = (DataTable)Session["datatable"];
            //PedidoNegocio pedidoNegocio = new PedidoNegocio();
            //List<Pedido> pedidos = pedidoNegocio.ListarPedidos();


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.Style["border-right"] = "1px solid #565d63";
                }
            }











            ListaFxCxT = Session["ListaFxCxT"] as List<FilasxColumnasxTiempoCoccion>;
            if (Session["ListaFxCxT"] != null)
            {
                foreach (var item in ListaFxCxT)
                {
                    if (e.Row.RowIndex == item.Fila)
                    {
                        for (int i = item.Columna; i <= item.Columna + item.TiempoCoccion; i++)
                        {
                            e.Row.Cells[i].BackColor = Color.LightBlue;
                            e.Row.Cells[i].Style["text-align"] = "center";
                            e.Row.Cells[i].Style["border-right"] = "none";
                        }

                     //   if (DateTime.Now.)
                       // {

                        //} ()

                    }
                }
            }

           
            //Session.Add("datatable", dataTable);
        }


        /*
                
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
            */

        protected void botonpedido_Click(object sender, EventArgs e)
        {

            Pedido pedido = new Pedido();
            pedido.IdServicio = 1;
            pedido.Productossolicitados = new List<ProductoPorPedido>();
            ProductoPorPedido productoporpedido = new ProductoPorPedido();
            ProductoNegocio productoNegocio = new ProductoNegocio();



            /*
            productoporpedido.Productodeldia.Id = 38;
            productoporpedido.Cantidad = 2;

            pedido.Productossolicitados.Add(productoporpedido);

            productoporpedido.Productodeldia.Id = 38;
            productoporpedido.Cantidad = 2;
            
            pedido.Productossolicitados.Add(productoporpedido);
            */
            productoporpedido.Productodeldia = productoNegocio.BuscarProductoDelDia(44, DateTime.Now);
            productoporpedido.Cantidad = 2;
        ;   pedido.Productossolicitados.Add(productoporpedido);

            productoporpedido.Productodeldia = productoNegocio.BuscarProductoDelDia(41, DateTime.Now);
            productoporpedido.Cantidad = 2;
            pedido.Productossolicitados.Add(productoporpedido);

            PedidoNegocio pedidoNegocio = new PedidoNegocio();

            pedidoNegocio.AbrirPedido(pedido);

        }

    }
}








