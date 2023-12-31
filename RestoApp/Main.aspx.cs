﻿using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.Web.Script.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.Services;
using Negocio;
using Dominio;
using Opciones;
using Helper;
using System.Data;
using System.Web.UI.WebControls.WebParts;
using System.Web.Script.Serialization;
using System.Web.Handlers;
using System.Web.Services.Description;


namespace RestoApp
{
    public partial class Main1 : System.Web.UI.Page
    {
        //Public
        public Usuario usuario { get; set; }
        public MeseroPorDia meseroPorDia { get; set; }
        public int MesasActivas { get; set; }
        public int MesasAsignadas { get; set; }
        public int MeserosPresentes { get; set; }
        public string tipoUsuario;
        public List<CategoriaProducto> ListaCategoriasProducto;

        //Private
        private List<MeseroPorDia> meserosPorDiaNoAsignados = new List<MeseroPorDia>();
        private List<MeseroPorDia> meserosPorDiaAsignados = new List<MeseroPorDia>();
        private List<Mesa> mesas;
        private List<MesaPorDia> mesasPorDia;

		//Javascript atributos
		private string datosMesasJSON;
        private string mesasActivasJSON;
        private string numeroMesasJSON;
        private string seviciosJSON;

		protected void Page_Load(object sender, EventArgs e)
        {
            //AUTENTIFICACION USUARIO
            if (AutentificacionUsuario.esUser(Helper.Session.GetUsuario()))
                usuario = Helper.Session.GetUsuario();

            // CONTENIDO GERENTE
            if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
            {
                //Acciona mensajes de alerta en caso de que haya mensajes guardados
                if (Helper.Session.GetMensajeModal() != null)
                {
					dynamic msgModal = (dynamic)Helper.Session.GetMensajeModal();
					UIMostrarAlerta(msgModal.msg, msgModal.tipo);
				}

                try
                {
                    //Variable para front
					tipoUsuario = Configuracion.Rol.Gerente;

                    //Métodos
                    CargarMeseros();
                    CargarDatosMesas();
                    CargarServicios();
                    CargarEstadoMesas();
                    ActualizarPedidos();
                    CargarDDLPedidos();

                    //Si hubo una búsqueda anterior, se carga
                    if((List<Pedido>)Session["PedidosGerente"] != null)
					    CargarPedido();
				}
                catch (Exception error)
                {
                    UIMostrarAlerta(error.Message);
				}
            }

            //Si es postback, recargamos funciones de script en Gerente
            if (IsPostBack && AutentificacionUsuario.esGerente(usuario))
            {
                ScriptsGerentePostBack();
			}

            // CONTENIDO MESERO
			if (!IsPostBack && AutentificacionUsuario.esMesero(usuario))

            {
                //Verificamos que si ya está en memoria el meseropordia
                if ((MeseroPorDia)Session[Configuracion.Session.MeseroPorDia] != null)
                    meseroPorDia = (MeseroPorDia)Session[Configuracion.Session.MeseroPorDia];

                //Verificamos si hay un número de mesa guardado para hacer pedido
                if (Helper.Session.GetNumeroMesaPedido() != null)
                {
                    lbNumeroMesa.Text = "CREANDO PEDIDO PARA MESA  #" + Helper.Session.GetNumeroMesaPedido().ToString();
                    ActivarBtnCancelarPedido();
                }

                //Accionamos Modal de mensaje si hay mensajes guardados
                if (Helper.Session.GetMensajeModal() != null)
                {
                    dynamic msgModal = (dynamic)Helper.Session.GetMensajeModal();
                    //UIMostrarAlerta(msgModal.msg, msgModal.tipo);
                    UIMostrarToast(msgModal.msg, msgModal.tipo);
				}

				try
				{
					//Variable para front
					tipoUsuario = Configuracion.Rol.Mesero;

                    //Métodos
					CargarMeseros();
					CargarMenuDisponible();
					CargarMeseroPorDia();
					CargarMesasAsignadas();
					CargarServicios();
					ActualizarPedidos();
				}
				catch (Exception error)
				{
                    UIMostrarAlerta(error.Message);
				}
			}
			
            //Si es postback, recargamos funciones de script en Mesero
            if (IsPostBack && AutentificacionUsuario.esMesero(usuario))
            {
                ScriptsMeseroPostBack();
			}

            ListarCategoriasProducto();
        }

		protected void ActivarBtnCancelarPedido()
        {
            btnTerminarPedido.Visible = true;
        }

		// ********** VISTA GERENTE ************

		//Cargamos los servicios desde la base de datos
		private void CargarServicios()
        {
            ServicioNegocio servicioNegocio = new ServicioNegocio();
            List<Servicio> serviciosDB = servicioNegocio.Listar().FindAll(serv => serv.Cobrado == false);

            //Si la session existe completamos los datos desde la DB
            if (Helper.Session.GetServicios() != null)
            {
                List<Servicio> serviciosSession = Helper.Session.GetServicios();

                foreach (var itemServicioSession in serviciosSession)
                {
                    foreach (var itemServicioDB in serviciosDB)
                    {
                        if (itemServicioSession.Mesa == itemServicioDB.Mesa)
                        {
                            itemServicioSession.Id = itemServicioDB.Id;
                            itemServicioSession.Mesa = itemServicioDB.Mesa;
                            itemServicioSession.Mesero = itemServicioDB.Mesero;
                            itemServicioSession.IdMesero = itemServicioDB.IdMesero;
                            itemServicioSession.Fecha = itemServicioDB.Fecha;
                            itemServicioSession.Apertura = itemServicioDB.Apertura;
                            itemServicioSession.Cierre = itemServicioDB.Cierre;
                            itemServicioSession.Cobrado = itemServicioDB.Cobrado;
                        }
                    }
                }

                //Guardamos en session
                Helper.Session.SetServicios(serviciosSession);
            }
            else
            {
                List<Servicio> servicio = new List<Servicio>();

                foreach (Servicio item in serviciosDB)
                {
                    Servicio auxServicioSession = new Servicio();

                    auxServicioSession.Id = item.Id;
                    auxServicioSession.Mesa = item.Mesa;
                    auxServicioSession.Mesero = item.Mesero;
                    auxServicioSession.IdMesero = item.IdMesero;
                    auxServicioSession.Fecha = item.Fecha;
                    auxServicioSession.Apertura = item.Apertura;
                    auxServicioSession.Cierre = item.Cierre;
                    auxServicioSession.Cobrado = item.Cobrado;

                    servicio.Add(auxServicioSession);
                }

                //Guardamos en session
                Helper.Session.SetServicios(servicio);
            }

            //Mandamos datos a JS
            ScriptDataServicios();
        }
		
        private void CargarDatosMesas()
        {
            {
				//Llamdados a DB
				MesaNegocio mesaNegocio = new MesaNegocio();
				mesas = mesaNegocio.Listar();
				
				//Sessiones
				Helper.Session.SetMesas(mesas);

                //Guardamos cantidad de mesas activas para mostrar en ASPX
                MesasActivas = mesas.FindAll(m => m.Activo == true).Count();

                List<MeseroPorDia> meserosAsignados = Helper.Session.GetMeserosAsignados();

                //Guardamos dato de cada mesa en una lista de Objects
                List<object> datosMesas = new List<object>();

                foreach (MesaPorDia mesa in Helper.Session.GetMesasAsignadas())
                {
                    //Guardamos datos en datosMesas de cuyas mesas el cierre sea Null
                    if (mesa.Cierre == null)
                    {
                        datosMesas.Add(new { mesa = mesa.Mesa, mesero = mesa.Mesero, nombre = meserosAsignados.Find(el => el.IdMesero == mesa.Mesero)?.Nombres, apellido = meserosAsignados.Find(el => el.IdMesero == mesa.Mesero)?.Apellidos });
                    }
                }

                //Buscamos las mesas activas
                List<Mesa> mesasActivasNumeros = mesas.FindAll(m => m.Activo == true);

                // Convierte la lista en una cadena JSON
                datosMesasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(datosMesas);
                mesasActivasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(mesasActivasNumeros);

                //Guardamos datos en session para JS
                Session["datosMesasJSON"] = datosMesasJSON;
                Session["mesasActivasJSON"] = mesasActivasJSON;

                //Guardamos en session los datos de las mesas (mesa, idMesero, nombre y apellido)
                Session["infoMesas"] = datosMesas;

                //Mandamos datos a JS
                ScriptsDataGerente();
            }
        }

        private void CargarEstadoMesas()
        {
            List<Servicio> servicios = Helper.Session.GetServicios();

            var dataTable = new DataTable();
            dataTable.Columns.Add("Servicio", typeof(int));
            dataTable.Columns.Add("Numero", typeof(int));
			dataTable.Columns.Add("Mesero", typeof(string));
            dataTable.Columns.Add("Apertura", typeof(DateTime));
            dataTable.Columns.Add("Cierre", typeof(DateTime));

            foreach (Servicio item in servicios)
            {
                dataTable.Rows.Add(
                    (int)item.Id,
                    (int)item.Mesa,
                    (string)item.Mesero,
                    (DateTime)item.Fecha + item.Apertura,
                    (DateTime?)item.Fecha + item.Cierre); ;
            }

            foreach (DataRow row in dataTable.Rows)
            {
                int servicio = (int)row["Servicio"];
                int numero = (int)row["Numero"];
                string mesero = (string)row["Mesero"];
                DateTime apertura = (DateTime)row["Apertura"];
                DateTime? cierre = row["Cierre"] == DBNull.Value ? null : (DateTime?)row["Cierre"];
            }

            // Enlazar el DataTable al DataGrid
            datagrid.DataSource = dataTable;
            datagrid.DataBind();
        }

        private void CargarDDLPedidos()
        {
            //Ordenamos la lista por mesa
            List<Servicio> listaMesa = (List<Servicio>)Helper.Session.GetServicios().OrderBy(item => item.Mesa).ToList();

			foreach (Servicio item in listaMesa)
            {
			    ddlPedidosGerente.Items.Add(new ListItem($"Pedidos de la Mesa {item.Mesa}", item.Mesa.ToString()));
				ddlPedidosGerente.Attributes["class"] = "dropdown-item";
			}
		}

        protected void BtnBuscarPedidos_Click(object sender, EventArgs e)
        {
            int mesa = Convert.ToInt32(ddlPedidosGerente.SelectedValue);
			CargarPedidosDelDiaGerente(mesa);
			
		}
		
		private void CargarPedidosDelDiaGerente(int id)
        {
			PedidoNegocio pedidoNegocio = new PedidoNegocio();
			// List<Pedido> pedidosGerente = pedidoNegocio.ListarPedidosDelDia();
			List<Pedido> pedidosGerente = pedidoNegocio.ListarPedidosDelDiaPorMesa(id);
			Session["PedidosGerente"] = pedidosGerente;

            string mesa = $"<p><span class=\"fw-semibold\">Cantidad de Pedidos:</span> {pedidosGerente.Select(item => item.Productossolicitados.Count() > 0 ? item.Productossolicitados.Count() : 0).Sum()}</p>";
			lbCantidadPedidos.Text = HttpUtility.HtmlDecode(mesa);

			string mesero = $"<p><span class=\"fw-semibold\">Mesero:</span> {Helper.Session.GetServicios().Find(item => item.Mesa == id).Mesero}</p>";
            lbPedidoMesero.Text = HttpUtility.HtmlDecode(mesero);

			CargarPedido();
		}

        private void CargarPedido()
        {
            //Session
            List<Pedido> pedidos = (List<Pedido>)Session["PedidosGerente"];

            //Filtramos los pedidos que no tengan "Entregado"
            pedidos = pedidos.FindAll(pedido => pedido.EstadoDescripcion != "Entregado");

            //Ordenar los pedidos por mesa para que aparezcan en orden en el listado
            List<Servicio> servicios = (List<Servicio>)Helper.Session.GetServicios();

            //DataTable
            var dataTable = new DataTable();

            dataTable.Columns.Add("Mesa", typeof(int));
            dataTable.Columns.Add("PedidoComida", typeof(string));
            dataTable.Columns.Add("Actualización", typeof(DateTime));
            dataTable.Columns.Add("Estado", typeof(string));


            foreach (Servicio itemServicio in servicios)
            {
                foreach (Pedido itemPedido in pedidos)
                {
                    if (itemServicio.Id == itemPedido.IdServicio)
                    {
                        foreach (ProductoPorPedido itemProducto in itemPedido.Productossolicitados)
                        {
                            dataTable.Rows.Add((int)itemServicio.Mesa, (string)itemProducto.Productodeldia.Nombre, (DateTime)itemPedido.ultimaactualizacion, (string)itemPedido.EstadoDescripcion);

                        }
                    }
                }
            }

            //Filas
            foreach (DataRow row in dataTable.Rows)
            {
                int mesa = (int)row["Mesa"];
                string pedidocomida = (string)row["PedidoComida"];
                DateTime actualizacion = (DateTime)row["Actualización"];
                string estado = (string)row["Estado"];
            }

            // Enlazar el DataTable al DataGrid
            datagridPedidos.DataSource = dataTable;
            datagridPedidos.DataBind();
		}
		
        private void CargarMeseros()
        {
			//DB
			MesaNegocio mesaNegocio = new MesaNegocio();
			mesasPorDia = mesaNegocio.ListarMesaPorDia();

            //Session
            Helper.Session.SetMesasAsignadas(mesasPorDia);

			//Lista de IDs de Meseros con mesas asignadas
			List<int?> IdMeserosConMesasAbiertas = mesasPorDia.Select(mesa => mesa.IDMeseroPorDia).ToList();

			//Meseros Presentes
			List<MeseroPorDia> meseroPorDia = mesaNegocio.ListaMeseroPorDia();
			Session["MeserosPresentes"] = meseroPorDia;

			//Meseros Ausentes
			List<Usuario> meserosAusentes = mesaNegocio.ListaMeserosAusentes();

            //Meseros no asignados
            meserosPorDiaNoAsignados = meseroPorDia.Where(usuario => !IdMeserosConMesasAbiertas.Contains(usuario.Id)).ToList();

            //Meseros asignados
            meserosPorDiaAsignados = meseroPorDia.Where(usuario => IdMeserosConMesasAbiertas.Contains(usuario.Id)).ToList();

            //Colocar cantidad de mesas asignadas
            meserosPorDiaAsignados.ForEach(mesero => mesero.MesasAsignadas = IdMeserosConMesasAbiertas
            .FindAll(id => id == mesero.Id).Count);

            //Sessions
            Helper.Session.SetMeserosAsignados(meserosPorDiaAsignados);
            Helper.Session.SetMeserosNoAsignados(meserosPorDiaNoAsignados);

            //Cantidad de Mesas Asignadas
            foreach (var meseros in meserosPorDiaAsignados)
            {
                MesasAsignadas += meseros.MesasAsignadas;
            }

            //Meseros presentes
            MeserosPresentes = meseroPorDia.Count();

            //Render Meseros Presentes
            repeaterMeserosPresentes.DataSource = meseroPorDia;
            repeaterMeserosPresentes.DataBind();

            //Render Meseros Ausentes
            repeaterMeserosAusentes.DataSource = meserosAusentes;
            repeaterMeserosAusentes.DataBind();
        }


        // ********** VISTA MESERO ************
		
        private void CargarMenuDisponible()
        {
            Session["ListaMenu"] = null;
            ProductoNegocio ProductoNegocio = new ProductoNegocio();
            Session.Add("ListaMenu", ProductoNegocio.ListarProductosDelDia());
            CargarBebidasDelDia();
            CargarPlatosDelDia();
        }


        private void CargarPlatosDelDia()
        {
            ListarCategoriasProducto();
            List<ProductoDelDia> ListaProductosDisponibles =
            ((List<ProductoDelDia>)Session["ListaMenu"]).FindAll(x => x.Activo == true && x.Stock > 1 && x.Categoria == ListaCategoriasProducto.Find(y => y.Descripcion == "Platos").Id);
            PlatosDelDia.DataSource = ListaProductosDisponibles;
            PlatosDelDia.DataBind();
        }
		
        private void CargarBebidasDelDia()
        {
            ListarCategoriasProducto();
            List<ProductoDelDia> ListaProductosDisponibles =
            ((List<ProductoDelDia>)Session["ListaMenu"]).FindAll(x => x.Activo == true && x.Stock > 1 && x.Categoria == ListaCategoriasProducto.Find(y => y.Descripcion == "Bebidas").Id);
            BebidasDelDia.DataSource = ListaProductosDisponibles;
            BebidasDelDia.DataBind();
        }

		private void CargarMeseroPorDia()
        {
			//DB
            MesaNegocio mesaNegocio = new MesaNegocio();

            List<MeseroPorDia> meserosPorDia = new List<MeseroPorDia>();
            meserosPorDia = mesaNegocio.ListaMeseroPorDia();

            //Verificamos si el mesero está dado de alta y no de baja
            meseroPorDia = meserosPorDia.Find(mesero => mesero.IdMesero == usuario.Id && mesero.Salida == new TimeSpan(0, 0, 0));

            if (meseroPorDia != null)
            {
                // Session
                Helper.Session.SetMeseroPorDia(meseroPorDia);

                if (meseroPorDia.Id > 0)
                {
                    btnMeseroAlta.Text = "Darse de Baja";
                    btnMeseroAlta.CssClass = "btn btn-sm btn-light";
                }
            }
            else
            {
                btnMeseroAlta.Text = "Darse de Alta";
                btnMeseroAlta.CssClass = "btn btn-sm btn-warning";
            }
        }

        //Evento de botón de alta de mesero
        protected void btnMeseroAlta_Click(object sender, EventArgs e)
        {
            // Session
            usuario = Helper.Session.GetUsuario();
            meseroPorDia = Helper.Session.GetMeseroPorDia();

            //BTN
            Session["BotonID"] = btnMeseroAlta.ClientID;
			
            if (meseroPorDia == null)
            {
                //Darse de Alta
                try
                {
                    meseroPorDia = new MeseroPorDia();
                    meseroPorDia.IdMesero = usuario.Id;
                    meseroPorDia.Nombres = usuario.Nombres;
                    meseroPorDia.Apellidos = usuario.Apellidos;
                    meseroPorDia.Fecha = DateTime.Now;
                    meseroPorDia.Ingreso = DateTime.Now.TimeOfDay;

                    //DB
                    MesaNegocio mesaNegocio = new MesaNegocio();

                    //Creamos mesero y recuperamos ID
                    meseroPorDia.Id = mesaNegocio.CrearMeseroPorDia(meseroPorDia);                        
				}
                catch (Exception ex)
                {
                    //Mostramos error
                    UIMostrarAlerta(ex.Message);
				}

                //Si se crea correctamente cambiamos el texto del botón
                if (meseroPorDia.Id > 0)
                {
                    btnMeseroAlta.Text = "Darse de Baja";
                    btnMeseroAlta.CssClass = "btn btn-sm btn-light";
                    Helper.Session.SetMeseroPorDia(meseroPorDia);

                    //Mostramos resultado exitoso
					UIMostrarAlerta("Se ha dado de alta exitosamente", "success");
				}
            }
            else
            {
                //Darse de baja
                try
                {
				    //DB
                   MesaNegocio mesaNegocio = new MesaNegocio();

                    List<MesaPorDia> mesasAsignadas = Helper.Session.GetMesasAsignadas();

                    //Si tiene mesas asigndas, se cierran
                    //TODO: si tiene pedidos abierto, primero los tiene que cerrar
                    if (mesasAsignadas != null)
                    {
                        foreach (MesaPorDia item in mesasAsignadas)
                        {
                            mesaNegocio.ModificarMesaPorDia(item.Id, item.Mesa, (int)item.Mesero);
                        }
                    }

                    bool esBaja = mesaNegocio.ModificarMeseroPorDia(meseroPorDia.Id);

                    //Si la baja es correcta cambiamos el texto del botón y borramos sessión
                    if (esBaja)
                    {
                        btnMeseroAlta.Text = "Darse de Alta";
                        btnMeseroAlta.CssClass = "btn btn-sm btn-warning";
                        meseroPorDia = null;
                        Helper.Session.SetMeseroPorDia(null);

                        //Mostramos mensaje de baja confirmando
						UIMostrarAlerta("Se ha dado de baja exitosamente", "success");
					}
				}
                catch (Exception ex)
                {
					//Mostramos error
					UIMostrarAlerta(ex.Message);
				}
            }
        }
		
        private void CargarMesasAsignadas()
        {
            // Session
            usuario = Helper.Session.GetUsuario();
            meseroPorDia = Helper.Session.GetMeseroPorDia();

            //Si hay mesero por dia
            if (meseroPorDia != null)
            {
				//DB
                MesaNegocio mesaNegocio = new MesaNegocio();
				
				
                List<MesaPorDia> mesasAsignadas = mesaNegocio.ListarMesaPorDia()
                    .FindAll(x => x.Mesero == meseroPorDia.IdMesero && x.Cierre == null)
                    .OrderBy(x => x.Mesa).ToList();

                //Session - Guardamos las mesas asignadas después de filtrarlas por IdMesero y que no estén cerradas
                Helper.Session.SetMesasAsignadas(mesasAsignadas);

                if (mesasAsignadas != null)
                {
                    //Creamos Objetos con los números de mesas asignadas
                    List<object> datosMesas = new List<object>();

                    foreach (MesaPorDia mesa in Helper.Session.GetMesasAsignadas())
                    {
                        //Guardamos datos en datosMesas de cuyas mesas el cierre sea Null
                        if (mesa.Cierre == null)
                        {

                            datosMesas.Add(new { mesa = mesa.Mesa });
                        }
                    }

                    //Serializamos datos
                    numeroMesasJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(datosMesas);

                    //Guardamos datos en session para recuperar en caso de postback
                    Session["numeroMesaJSON"] = numeroMesasJSON;

                    //Mandamos datos a script
                    ScriptsDataMesero();

                    if (mesasAsignadas.Count > 0)
                    {
                        lbSinMesasAsignadas.Text = String.Empty;
                        return;
                    }

                }
            }

            lbSinMesasAsignadas.Text = "No cuenta con mesas asignadas. Comuníquese con el Gerente.";
        }

        protected void ListarCategoriasProducto()
        {
            CategoriaProductoNegocio CPNAux = new CategoriaProductoNegocio();
            ListaCategoriasProducto = CPNAux.Listar();
        }

        protected void AgregarAPedido_Click(object sender, EventArgs e)
        {
            if (Session["Servicios"] == null)
            {
                string script = "alert('Primero abra un servicio');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
            }
            else if (Helper.Session.GetNumeroMesaPedido() == null)
            {
                string script = "alert('Primero abra un Pedido');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
            }
            else
            {
                Button button = sender as Button;

                RepeaterItem repeaterItem = button.NamingContainer as RepeaterItem;
                TextBox tbCantidad = repeaterItem.FindControl("tbCantidad") as TextBox;
                Button btnCancelar = repeaterItem.FindControl("btnCancelarAgregarA") as Button;
                Button btnGuardarPedido = UPGuardarPedido.FindControl("BtnGuardarPedido") as Button;
                btnGuardarPedido.Visible = true;

                ProductoPorPedido productoPorPedido = new ProductoPorPedido();
                productoPorPedido.Productodeldia = ((List<ProductoDelDia>)Session["ListaMenu"]).Find(x => x.Id == int.Parse(button.CommandArgument));


                if (button.Text.ToLower() == "✚")
                {
                    tbCantidad.Visible = true;
                    tbCantidad.Attributes.Add("max", productoPorPedido.Productodeldia.Stock.ToString());
                    button.Text = "✔";
                    btnCancelar.Visible = true;
                }
                else
                {
                    tbCantidad.Visible = false;
                    button.Text = "✚";
                    btnCancelar.Visible = false;


                    if (Session["ProductosPorPedido"] == null)
                    {
                        List<ProductoPorPedido> list = new List<ProductoPorPedido>();
                        Session.Add("ProductosPorPedido", list);
                    }

                    if (((List<ProductoPorPedido>)Session["ProductosPorPedido"]).Count > 0){
                        if (((List<ProductoPorPedido>)Session["ProductosPorPedido"]).First<ProductoPorPedido>().Productodeldia.Categoria == (ListaCategoriasProducto.Find(x => x.Descripcion.ToLower() == "platos")).Id)
                        {
                            productoPorPedido.Cantidad = int.Parse(tbCantidad.Text);
                            ((List<ProductoPorPedido>)Session["ProductosPorPedido"]).Add(productoPorPedido);
                        }
                        else
                        {
                            string script = "alert('No puede agregar Platos al Pedido en curso (Pedido de Bebidas)');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
                        }
                    }
                    else
                    {
                        productoPorPedido.Cantidad = int.Parse(tbCantidad.Text);
                        ((List<ProductoPorPedido>)Session["ProductosPorPedido"]).Add(productoPorPedido);
                    }
                }
            }
        }

        protected void BtnCancelarAgregarA_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            RepeaterItem repeaterItem = button.NamingContainer as RepeaterItem;
            TextBox tbCantidad = repeaterItem.FindControl("tbCantidad") as TextBox;
            Button btnCancelar = repeaterItem.FindControl("btnCancelarAgregarA") as Button;
            Button BtnAgregarAPedido = repeaterItem.FindControl("AgregarAPedido") as Button;
            BtnAgregarAPedido.Text = "✚";
            tbCantidad.Visible = false;
            btnCancelar.Visible = false;
        }

        protected void btnGuardarPedido_Click(object sender, EventArgs e)
        {
            Button btnGuardarPedido = sender as Button;

            if ((Session["ProductosPorPedido"] != null) && ((List<ProductoPorPedido>)(Session["ProductosPorPedido"])).Count > 0)
            {
                Pedido Paux = new Pedido();
                Paux.IdServicio = ((List<Servicio>)Session["Servicios"]).Find(x => x.Mesa == Helper.Session.GetNumeroMesaPedido()).Id;
                Paux.Productossolicitados = ((List<ProductoPorPedido>)Session["ProductosPorPedido"]);

                foreach (ProductoPorPedido PPP in Paux.Productossolicitados)
                {
                    ((List<ProductoDelDia>)Session["ListaMenu"]).Find(x => x.Id == PPP.Productodeldia.Id).Stock -= PPP.Cantidad;
                    ((List<ProductoDelDia>)Session["ListaMenu"]).Find(x => x.Id == PPP.Productodeldia.Id).StockCierre -= PPP.Cantidad;
                    Producto ProductoAux = new Producto(PPP.Productodeldia);
                    ProductoNegocio ProductoNegocioAux = new ProductoNegocio();
                    ProductoNegocioAux.ModificarProducto(ProductoAux);
                    ProductoNegocioAux.ModificarProductoDD((Dominio.ProductoDelDia)(((List<ProductoDelDia>)Session["ListaMenu"]).Find(x => x.Id == PPP.Productodeldia.Id)));
                }

                PedidoNegocio PNaux = new PedidoNegocio();
                ((List<Pedido>)Session["Pedidos"]).Add(PNaux.BuscarPedido(PNaux.AbrirPedido(Paux)));
                Session["ProductosPorPedido"] = null;
                btnGuardarPedido.Visible = false;
                btnTerminarPedido.Text = "Terminar Pedido";
            }
            else
            {
                string script = "alert('Para enviar un pedido primero agregue un producto');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
            }
        }

        protected void btnTerminarPedido_Click(object sender, EventArgs e)
        {
            Button btnTerminarPedido = sender as Button;
            Session["NumeroMesaPedido"] = null;
            Session["ProductosPorPedido"] = null;
            btnGuardarPedido.Visible = false;
            btnTerminarPedido.Visible = false;
            lbNumeroMesa.Text = "SIN MESA SELECCIONADA";
            ActualizarPedidos();
        }

        protected void AgregarAPedido2_Click(object sender, EventArgs e)
        {
            if (Session["Servicios"] == null)
            {
                string script = "alert('Primero abra un servicio');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
            }
            else if (Helper.Session.GetNumeroMesaPedido() == null)
            {
                string script = "alert('Primero abra un Pedido');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
            }
            else
            {
                Button button = sender as Button;

                RepeaterItem repeaterItem = button.NamingContainer as RepeaterItem;
                TextBox tbCantidad = repeaterItem.FindControl("tbCantidad2") as TextBox;
                Button btnCancelar = repeaterItem.FindControl("btnCancelarAgregarA2") as Button;
                Button btnGuardarPedido = UPGuardarPedido.FindControl("BtnGuardarPedido") as Button;
                btnGuardarPedido.Visible = true;


                ProductoPorPedido productoPorPedido = new ProductoPorPedido();
                productoPorPedido.Productodeldia = ((List<ProductoDelDia>)Session["ListaMenu"]).Find(x => x.Id == int.Parse(button.CommandArgument));


                if (button.Text.ToLower() == "✚")
                {
                    tbCantidad.Visible = true;
                    tbCantidad.Attributes.Add("max", productoPorPedido.Productodeldia.Stock.ToString());
                    button.Text = "✔";
                    btnCancelar.Visible = true;
                }
                else
                {
                    tbCantidad.Visible = false;
                    button.Text = "✚";
                    btnCancelar.Visible = false;

                    if (Session["ProductosPorPedido"] == null)
                    {
                        List<ProductoPorPedido> list = new List<ProductoPorPedido>();
                        Session.Add("ProductosPorPedido", list);
                    }

                    if (((List<ProductoPorPedido>)Session["ProductosPorPedido"]).Count > 0)
                    {
                        if (((List<ProductoPorPedido>)Session["ProductosPorPedido"]).First<ProductoPorPedido>().Productodeldia.Categoria == (ListaCategoriasProducto.Find(x => x.Descripcion.ToLower() == "bebidas")).Id)
                        {
                            productoPorPedido.Cantidad = int.Parse(tbCantidad.Text);
                            ((List<ProductoPorPedido>)Session["ProductosPorPedido"]).Add(productoPorPedido);
                        }
                        else
                        {
                            string script = "alert('No puede agregar Bebidas al Pedido en curso (Pedido de Platos)');";
                            ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
                        }
                    }
                    else
                    {
                        productoPorPedido.Cantidad = int.Parse(tbCantidad.Text);
                        ((List<ProductoPorPedido>)Session["ProductosPorPedido"]).Add(productoPorPedido);
                    }
                }

            }
        }

        protected void BtnCancelarAgregarA2_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            RepeaterItem repeaterItem = button.NamingContainer as RepeaterItem;
            TextBox tbCantidad = repeaterItem.FindControl("tbCantidad2") as TextBox;
            Button btnCancelar = repeaterItem.FindControl("btnCancelarAgregarA2") as Button;
            Button BtnAgregarAPedido = repeaterItem.FindControl("AgregarAPedido2") as Button;
            BtnAgregarAPedido.Text = "✚";
            tbCantidad.Visible = false;
            btnCancelar.Visible = false;
        }

        protected void ListarPedidosDelDia()
        {
            Session["Pedidos"] = null;
            PedidoNegocio PNAux = new PedidoNegocio();
            if (AutentificacionUsuario.esMesero(usuario))
            {
                Session.Add("Pedidos", PNAux.ListarPedidosDelDia(int.Parse((Session["IdUsuario"]).ToString())));
            }
            if (AutentificacionUsuario.esGerente(usuario))
            {
                Session.Add("Pedidos", new List<Pedido>());
            }


        }

        protected void ActualizarPedidos()
        {
            //ServicioNegocio SNAux = new ServicioNegocio();
            List<Servicio> ListaServicios = (Helper.Session.GetServicios());

            if (Session["Pedidos"] == null)
            {
                ListarPedidosDelDia();
            }

            List<Pedido> Pedidos = ((List<Pedido>)Session["Pedidos"]);

            List<Pedido> PedidosAux = new List<Pedido>();

            //busco servicios abiertos
            if (ListaServicios.Count > 0)
            {
                // Comento validacion para trabajar en front
                ListaServicios = ListaServicios.FindAll(x => x.Cobrado != true && x.IdMesero == (int)Session["IdUsuario"]);
                foreach (Pedido Pedido in Pedidos)
                {
                    bool esPedidoEnCurso = false;
                    foreach (Servicio Servicio in ListaServicios)
                    {
                        if (Servicio.Id == Pedido.IdServicio && Pedido.Estado != 5)
                        {
                            esPedidoEnCurso = true;
                        }
                    }

                    if (esPedidoEnCurso != false)
                    {
                        PedidosAux.Add(Pedido);
                    }
                }
            }

            RepeaterPedidosEnCurso.DataSource = PedidosAux;
            RepeaterPedidosEnCurso.DataBind();

            //Guardo Session de PedidosAux para vista de Gerente
            // Session["Pedidos"] = PedidosAux;

            // consultar con ale. Esta lista esta filtrada por estado y idmesero. Pedidos en session no deberia mantener la lista original?
            // agrego funcion a parte
        }

        protected void RepeaterPedidosEnCurso_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Pedido pedido = e.Item.DataItem as Pedido;
            Label lbl = e.Item.FindControl("lbNroMesaPedido") as Label;
            lbl.Text = "Mesa " + ((List<Servicio>)Session["Servicios"]).Find(x => x.Id == pedido.IdServicio).Mesa.ToString();
            Estados s = new Estados();
            Panel panel = e.Item.FindControl("PanelEstadoPedido") as Panel;
            panel.Style.Add("background", s.getColorEstado(pedido.Estado));
            panel.Style.Add("cursor", "pointer");

        }

        protected string getDetallePedido(object dataItem)
        {
            var pedido = (Pedido)dataItem;

            var DetallesPedido = new Dictionary<string, object>();
            DetallesPedido.Add("PedidoId", pedido.Id);
            DetallesPedido.Add("Estado", pedido.EstadoDescripcion);

            var productos = new List<Dictionary<string, object>>();
            foreach (ProductoPorPedido productoPorPedido in pedido.Productossolicitados)
            {
                var producto = new Dictionary<string, object>();
                producto.Add("Nombre", productoPorPedido.Productodeldia.Nombre);
                producto.Add("Cantidad", productoPorPedido.Cantidad);
                productos.Add(producto);
            }
            DetallesPedido.Add("Productos", productos);
            DetallesPedido.Add("UltimaActualizacion", pedido.ultimaactualizacion);

            var jsonSerializer = new JavaScriptSerializer();
            var json = jsonSerializer.Serialize(DetallesPedido);

            return json;
        }

        protected void BtnCerrarPedido_Click(object sender, EventArgs e)
        {
            Button BtnCerrarPedido = sender as Button;
            PedidoNegocio PNaux = new PedidoNegocio();
            PNaux.CambiarEstadoPedido(int.Parse(BtnCerrarPedido.CommandArgument), 5);
            ((List<Pedido>)Session["Pedidos"]).Find(x => x.Id == int.Parse(BtnCerrarPedido.CommandArgument)).Estado = 5;
            ActualizarPedidos();
        }

		// ************** SCRIPTS ***************

		//Modal errores o éxitos: Tipo: error o success
		private void UIMostrarAlerta(string mensaje, string tipoMensaje = "error")
		{
			string scriptModal = $"alertaModal(\"{mensaje}\", \"{tipoMensaje}\");";
			ScriptManager.RegisterStartupScript(this, GetType(), "scriptMain", scriptModal, true);

			//Borramos mensaje del modal
			Helper.Session.SetMensajeModal(null);
		}

        private void UIMostrarToast(string mensaje, string tipoMensaje = "error")
        {
			string scriptModal = $"alertaToast(\"{mensaje}\", \"{tipoMensaje}\");";
			ScriptManager.RegisterStartupScript(this, GetType(), "scriptMain", scriptModal, true);

			//Borramos mensaje del modal
			Helper.Session.SetMensajeModal(null);
		}

		//Enviamos datos a script para recuperar en un postback
		private void ScriptsDataMesero()
		{
			//Recuperamos datos de session
			numeroMesasJSON = (string)Session["numeroMesaJSON"];

			//Mandamos a script de javascript
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasArray", $"var numeroMesasArray = '{numeroMesasJSON}';", true);
		}


		//Metodo que enviado datos del Gerente a script
		private void ScriptsDataGerente()
		{
			//Recuperamos datos por si es postback
			mesasActivasJSON = (string)Session["mesasActivasJSON"];
			datosMesasJSON = (string)Session["datosMesasJSON"];

			//Enviamos datos a JS
			ClientScript.RegisterStartupScript(this.GetType(), "datosMesasArray", $"var datosMesasArray = '{datosMesasJSON}';", true);
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasActivasArray", $"var numeroMesasActivasArray = '{mesasActivasJSON}';", true);
		}

		//Enviamos datos a script para recuperar en un postback
		private void ScriptDataServicios()
		{
			//Recuperamos datos de session
			List<Servicio> servicios = Helper.Session.GetServicios();
			List<object> serviciosJS = new List<object>();

			foreach (Servicio item in servicios)
			{
				serviciosJS.Add(
					new
					{
						mesa = item.Mesa,
						servicio = item.Id,
						cobrado = item.Cobrado,
						apertura = (item.Fecha + item.Apertura).ToString("HH:mm:ss"),
						cierre = item.Cierre.HasValue ? (item.Fecha + item.Apertura).ToString("HH:mm:ss") : String.Empty,
						mesero = item.Mesero
					});
			}

			seviciosJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(serviciosJS);

            //Guardamos en Session Datos para reenviar en caso de postback
            Session["servicioJSON"] = seviciosJSON;

			//Mandamos a script de javascript
			ClientScript.RegisterStartupScript(this.GetType(), "seviciosJSON", $"var seviciosJSON = '{seviciosJSON}';", true);
		}

		//Enviamos los datos del Gerente al scripr
		private void ScriptsGerentePostBack()
		{
			// Recuperamos datos por si es postback
			mesasActivasJSON = (string)Session["mesasActivasJSON"];
			datosMesasJSON = (string)Session["datosMesasJSON"];
			seviciosJSON = (string)Session["servicioJSON"];

			// Mandamos a script de JavaScript
			string script = @"
            document.addEventListener('DOMContentLoaded', function() {
            obtenerDatosMesasGerente()
                .then(({ datosMesas, numeroMesas, numeroServicios }) => {
                    renderMesaGerente(datosMesas, numeroMesas, numeroServicios);
                });
            })
            ";

            //Recuperamos el numero de mesas
			mesas = (List<Mesa>)Helper.Session.GetMesas();
			MesasActivas = mesas.FindAll(m => m.Activo == true).Count();

			//Mandamos a script de javascript
			ClientScript.RegisterStartupScript(this.GetType(), "seviciosJSON", $"var seviciosJSON = '{seviciosJSON}';", true);
			ClientScript.RegisterStartupScript(this.GetType(), "datosMesasArray", $"var datosMesasArray = '{datosMesasJSON}';", true);
			ClientScript.RegisterStartupScript(this.GetType(), "numeroMesasActivasArray", $"var numeroMesasActivasArray = '{mesasActivasJSON}';", true);
			ScriptManager.RegisterStartupScript(this, GetType(), "scriptMain", script, true);
            
		}

		//Enviamos los datos del mesero al script
		private void ScriptsMeseroPostBack()
		{
			ScriptsDataMesero();
			ScriptDataServicios();
			string script = " obtenerDatosMesasMesero().then(({ numeroMesas, numeroServicios }) => {renderMesaMesero(numeroMesas, numeroServicios); });";
			ScriptManager.RegisterStartupScript(this, GetType(), "scriptMain", script, true);

		}

		// ************ WEBMETHOD *************

		//Obtenemos número de mesa y le abrimos un servicio
		[WebMethod]
		public static bool AbrirServicio(List<Dictionary<string, int>> data)
		{
			//DB
			ServicioNegocio servicioNegocio = new ServicioNegocio();

			bool response = false;
			object msg = new { msg = "El servicio no pudo cargarse. Esto puede deberse a un error de conexión o a que la mesa ya se encuentra abierta.", tipo = "error" };

			foreach (var diccionario in data)
			{
				var numeroMesa = diccionario["mesa"];
				int idServicio = servicioNegocio.AbrirServicio(numeroMesa);

				//Si se guardó de manera correcta
				if (idServicio > 0)
				{
					//Guardamos todo en la session en caso de que exista
					if (Helper.Session.GetServicios() != null)
					{
						List<Servicio> servicio = Helper.Session.GetServicios();
						Servicio auxServicioSession = new Servicio();

						auxServicioSession.Id = idServicio;
						auxServicioSession.Mesa = numeroMesa;

						servicio.Add(auxServicioSession);

						//Guardamos en Session
						Helper.Session.SetServicios(servicio);
					}
					else
					{
						List<Servicio> servicio = new List<Servicio>();
						Servicio auxServicioSession = new Servicio();

						auxServicioSession.Id = idServicio;
						auxServicioSession.Mesa = numeroMesa;

						servicio.Add(auxServicioSession);

						//Guardamos en Session
						Helper.Session.SetServicios(servicio);
					}

					response = true;
					msg = new { msg = $"El servicio de la mesa {numeroMesa} fue abierto con éxito", tipo = "success" };
				};
			}

			//Guardamos mensaje para modal de front
			Helper.Session.SetMensajeModal(msg);

			return response;
		}

        //Cerrar servicios de la mesa
		[WebMethod]
		public static bool CerrarServicio(List<Dictionary<string, int>> data)
		{
			//DB
			ServicioNegocio servicioNegocio = new ServicioNegocio();

			//Iniciamos respuestas
			bool response = false;
			object msg = new { msg = $"El servicio no pudo cargarse. Esto puede deberse a un error de conexión o a pedidos que aún permanecen abiertos.", tipo = "error" };

			foreach (var diccionario in data)
			{
				var numeroMesa = diccionario["mesa"];

				if (servicioNegocio.CerrarServicio(numeroMesa))
				{
					//Modificamos la sessión agregándole la hora de cierre al servicio, pero sigue abierto hasta que cobrado no sea true
					List<Servicio> servicio = Helper.Session.GetServicios();

					foreach (var item in servicio)
					{
						if (item.Mesa == numeroMesa)
						{
							item.Cierre = DateTime.Now.TimeOfDay;
						}
					}

					//Guardamos servicio
					Helper.Session.SetServicios(servicio);

					//Generamos respuestas
					response = true;
					msg = new { msg = $"El servicio de la mesa {numeroMesa} fue cerrado con éxito", tipo = "success" };
				}
				else
				{
					//Generamos respuestas
					response = false;
					msg = new { msg = $"El servicio de la mesa {numeroMesa} no pudo cerrarse. Esto puede deberse a un error de conexión o a pedidos que aún permanecen abiertos", tipo = "error" };
				}
			}

			//Guardamos mensaje para modal de front
			Helper.Session.SetMensajeModal(msg);

			//Enviamos respuesta al fron
			return response;
		}

		//Guardamos número de mesa en pedido
		[WebMethod]
		public static bool AbrirPedido(List<Dictionary<string, int>> data)
		{
			//Iniciamos respuesta
			bool response = false;

			foreach (var diccionario in data)
			{
				var numeroMesa = diccionario["mesa"];

				if (numeroMesa > 0)
				{
					//Guardamos numero de mesa que se seleccionó para iniciar pedido
					Helper.Session.SetNumeroMesaPedido(numeroMesa);
					response = true;
				}
			}

			//Enviamos resupuesta al fron
			return response;
		}

	}
}