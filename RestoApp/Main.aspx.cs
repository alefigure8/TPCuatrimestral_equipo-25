using System.Runtime.Remoting.Messaging;
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

        protected void Page_Load(object sender, EventArgs e)
        {
                      

			//Verificar que sea usuario
			if (AutentificacionUsuario.esUser(Helper.Session.GetUsuario()))
                usuario = Helper.Session.GetUsuario();

            // CONTENIDO GERENTE
            if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
            {
                tipoUsuario = Configuracion.Rol.Gerente;
                CargarMeseros();
                CargarDatosMesas();
                CargarServicios();
                CargarEstadoMesas();
                CargarPedido();
            }

            //Si es postback, recargamos funciones de script en Gerente
            if (IsPostBack && AutentificacionUsuario.esGerente(usuario))
            {
                ScriptsDataGerente();
                ScriptDataServicios();
				string script = "obtenerDatosMesasGerente().then(({ datosMesas, numeroMesas, numeroServicios }) => {renderMesaGerente(datosMesas, numeroMesas, numeroServicios); })";
                ScriptManager.RegisterStartupScript(this, GetType(), "scriptMain", script, true);
            }


            //CONTENIDO MESERO
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


                tipoUsuario = Configuracion.Rol.Mesero;
                CargarMenuDisponible();
                CargarMeseroPorDia();
                CargarMesasAsignadas();
                CargarServicios();
                ActualizarPedidos();

            }

            //Si es postback, recargamos funciones de script en Mesero
            if (IsPostBack && AutentificacionUsuario.esMesero(usuario))
            {
                ScriptsDataMesero();
                ScriptDataServicios();
                string script = " obtenerDatosMesasMesero().then(({ numeroMesas, numeroServicios }) => {renderMesaMesero(numeroMesas, numeroServicios); });";
                ScriptManager.RegisterStartupScript(this, GetType(), "scriptMain", script, true);
            }

			ListarCategoriasProducto();
        }

		private void Tr_reloj_Tick(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		protected void ActivarBtnCancelarPedido()
        {
            btnTerminarPedido.Visible = true;

        }

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
                            itemServicioSession.Fecha = itemServicioDB.Fecha;
                            itemServicioSession.Apertura = itemServicioDB.Apertura;
                            itemServicioSession.Cierre = itemServicioDB.Cierre;
                            itemServicioSession.Cobrado = itemServicioDB.Cobrado;
                        }
                    }
                }

                if (AutentificacionUsuario.esGerente(usuario))
                {
                    //Info de la mesa desde session
                    List<object> infoMesas = (List<object>)Session["infoMesas"];

                    foreach (Servicio itemServicio in serviciosSession)
                    {

                        foreach (dynamic itemMesa in infoMesas)
                        {

                            if (itemServicio.Mesa == itemMesa.mesa)
                            {
                                itemServicio.Mesero = $"{itemMesa.nombre} {itemMesa.apellido}";
                                itemServicio.IdMesero = itemMesa.mesero;
                            }
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
                    auxServicioSession.Fecha = item.Fecha;
                    auxServicioSession.Apertura = item.Apertura;
                    auxServicioSession.Cierre = item.Cierre;
                    auxServicioSession.Cobrado = item.Cobrado;

                    servicio.Add(auxServicioSession);
                }

                if (AutentificacionUsuario.esGerente(usuario))
                {
                    //Info de la mesa desde session
                    List<object> infoMesas = (List<object>)Session["infoMesas"];

                    foreach (Servicio itemServicio in servicio)
                    {
                        foreach (dynamic itemMesa in infoMesas)
                        {
                            if (itemServicio.Mesa == itemMesa.mesa)
                            {
                                itemServicio.Mesero = $"{itemMesa.nombre} {itemMesa.apellido}";
                                itemServicio.IdMesero = itemMesa.mesero;
                            }
                        }
                    }
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
                mesasPorDia = mesaNegocio.ListarMesaPorDia();

                //Sessiones
                Helper.Session.SetMesas(mesas);
                Helper.Session.SetMesasAsignadas(mesasPorDia);

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
            dataTable.Columns.Add("Numero", typeof(int));
            dataTable.Columns.Add("Mesero", typeof(string));
            dataTable.Columns.Add("Apertura", typeof(DateTime));
            dataTable.Columns.Add("Cierre", typeof(DateTime));

            foreach (Servicio item in servicios)
            {
                dataTable.Rows.Add(
                    (int)item.Mesa,
                    (string)item.Mesero,
                    (DateTime)item.Fecha + item.Apertura,
                    (DateTime?)item.Fecha + item.Cierre
                );
            }

            foreach (DataRow row in dataTable.Rows)
            {
                int numero = (int)row["Numero"];
                string mesero = (string)row["Mesero"];
                DateTime apertura = (DateTime)row["Apertura"];
                DateTime? cierre = row["Cierre"] == DBNull.Value ? null : (DateTime?)row["Cierre"];
            }

            // Enlazar el DataTable al DataGrid
            datagrid.DataSource = dataTable;
            datagrid.DataBind();
        }

        private void CargarPedido()
        {
            //TODO: LLAMADO A DB PARA OBTENER ESTADO DE PEDIDOS

            var dataTable = new DataTable();
            dataTable.Columns.Add("Mesa", typeof(int));
            dataTable.Columns.Add("PedidoComida", typeof(string));
            dataTable.Columns.Add("Apertura", typeof(DateTime));
            dataTable.Columns.Add("Cierre", typeof(DateTime));

            // Agregar filas al DataTable de forma individual
            dataTable.Rows.Add(1, "Hamburguesa", DateTime.Now, DateTime.Now);
            dataTable.Rows.Add(1, "Pizza", DateTime.Now, DateTime.Now);
            dataTable.Rows.Add(2, "Sushi", DateTime.Now, System.DBNull.Value);
            dataTable.Rows.Add(3, "Ensalada", DateTime.Now, DateTime.Now);
            dataTable.Rows.Add(5, "Pasta", DateTime.Now, DateTime.Now);
            dataTable.Rows.Add(6, "Tacos", DateTime.Now, System.DBNull.Value);

            foreach (DataRow row in dataTable.Rows)
            {
                int numero = (int)row["Mesa"];
                string mesero = (string)row["PedidoComida"];
                DateTime apertura = (DateTime)row["Apertura"];
                DateTime? cierre = row["Cierre"] == System.DBNull.Value ? null : (DateTime?)row["Cierre"];
            }

            // Enlazar el DataTable al DataGrid
            datagridPedidos.DataSource = dataTable;
            datagridPedidos.DataBind();
        }

        private void CargarMeseros()
        {
            MesaNegocio mesaNegocio = new MesaNegocio();

            //Lista de IDs de Meseros con mesas asignadas
            List<int> IdMeserosConMesasAbiertas = mesaNegocio.ListaIdMeserosActivosConMesasAbiertas();

            //Meseros Presentes
            List<MeseroPorDia> meseroPorDia = mesaNegocio.ListaMeseroPorDia();

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


        // VISTA MESERO
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
            ((List<ProductoDelDia>)Session["ListaMenu"]).FindAll(x => x.Activo == true && x.Categoria == ListaCategoriasProducto.Find(y => y.Descripcion == "Platos").Id);
            PlatosDelDia.DataSource = ListaProductosDisponibles;
            PlatosDelDia.DataBind();
        }

        private void CargarBebidasDelDia()
        {
            ListarCategoriasProducto();
            List<ProductoDelDia> ListaProductosDisponibles =
            ((List<ProductoDelDia>)Session["ListaMenu"]).FindAll(x => x.Activo == true && x.Categoria == ListaCategoriasProducto.Find(y => y.Descripcion == "Bebidas").Id);
            BebidasDelDia.DataSource = ListaProductosDisponibles;
            BebidasDelDia.DataBind();
        }

        private void CargarMeseroPorDia()
        {
            List<MeseroPorDia> meserosPorDia = new List<MeseroPorDia>();
            MesaNegocio mesaNegocio = new MesaNegocio();

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

                    MesaNegocio mesaNegocio = new MesaNegocio();
                    meseroPorDia.Id = mesaNegocio.CrearMeseroPorDia(meseroPorDia);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                //Si se crea correctamente cambiamos el texto del botón
                if (meseroPorDia.Id > 0)
                {
                    btnMeseroAlta.Text = "Darse de Baja";
                    btnMeseroAlta.CssClass = "btn btn-sm btn-light";
                    Helper.Session.SetMeseroPorDia(meseroPorDia);
                }
            }
            else
            {
                //Darse de baja
                try
                {
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

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        private void CargarMesasAsignadas()
        {
            // Session
            usuario = Helper.Session.GetUsuario();
            meseroPorDia = Helper.Session.GetMeseroPorDia();

            if (meseroPorDia != null)
            {
                MesaNegocio mesaNegocio = new MesaNegocio();
                List<MesaPorDia> mesasAsignadas = mesaNegocio.ListarMesaPorDia()
                    .FindAll(x => x.Mesero == meseroPorDia.IdMesero && x.Cierre == null)
                    .OrderBy(x => x.Mesa).ToList();

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

            string seviciosJSON = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(serviciosJS);

            //Mandamos a script de javascript
            ClientScript.RegisterStartupScript(this.GetType(), "seviciosJSON", $"var seviciosJSON = '{seviciosJSON}';", true);
        }

        //WEBMETHOD
        //Obtenemos número de mesa y le abrimos un servicio
        [WebMethod]
        public static bool AbrirServicio(List<Dictionary<string, int>> data)
        {
            ServicioNegocio servicioNegocio = new ServicioNegocio();

            bool response = false;

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
                };
            }

            return response;
        }

		[WebMethod]
		public static bool CerrarServicio(List<Dictionary<string, int>> data)
		{
			ServicioNegocio servicioNegocio = new ServicioNegocio();

			bool response = false;

			foreach (var diccionario in data)
			{
				var numeroMesa = diccionario["mesa"];

				if(servicioNegocio.CerrarServicio(numeroMesa))
                {
					//Modificamos la sessión agregándole la hora de cierre al servicio, pero sigue abierto hasta que cobrado no sea true
					List<Servicio> servicio = Helper.Session.GetServicios();

                    foreach (var item in servicio)
                    {
                        if(item.Mesa == numeroMesa)
                        {
                            item.Cierre = DateTime.Now.TimeOfDay;
                        }
                    }

                    Helper.Session.SetServicios(servicio);

					response = true;
				}
                else
                {
					//Si no se guardó de manera correcta
                    response = false;
				}
			}

			return response;
		}

		//Guardamos número de mesa en pedido
		[WebMethod]
        public static string AbrirPedido(List<Dictionary<string, int>> data)
        {

            string response = String.Empty;
            foreach (var diccionario in data)
            {
                var numeroMesa = diccionario["mesa"];
                Helper.Session.SetNumeroMesaPedido(numeroMesa);
                response = numeroMesa.ToString();
            }

            return response;
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

                if (button.Text.ToLower() == "✚")
                {
                    tbCantidad.Visible = true;
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

                    ProductoPorPedido productoPorPedido = new ProductoPorPedido();
                    productoPorPedido.Productodeldia = ((List<ProductoDelDia>)Session["ListaMenu"]).Find(x => x.Id == int.Parse(button.CommandArgument));
                    productoPorPedido.Cantidad = int.Parse(tbCantidad.Text);
                    ((List<ProductoPorPedido>)Session["ProductosPorPedido"]).Add(productoPorPedido);

                }

                ActualizarPedidos();
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


                PedidoNegocio PNaux = new PedidoNegocio();
                PNaux.AbrirPedido(Paux);
                Session["ProductosPorPedido"] = null;
                btnGuardarPedido.Visible = false;
                btnTerminarPedido.Text = "Terminar Pedido";
            }
            else
            {
                string script = "alert('Para enviar un pedido primero agregue un producto');";
                ScriptManager.RegisterStartupScript(this, GetType(), "ServerAlert", script, true);
            }
            ActualizarPedidos();




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

                if (button.Text.ToLower() == "✚")
                {
                    tbCantidad.Visible = true;
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

                    ProductoPorPedido productoPorPedido = new ProductoPorPedido();
                    productoPorPedido.Productodeldia = ((List<ProductoDelDia>)Session["ListaMenu"]).Find(x => x.Id == int.Parse(button.CommandArgument));
                    productoPorPedido.Cantidad = int.Parse(tbCantidad.Text);
                    ((List<ProductoPorPedido>)Session["ProductosPorPedido"]).Add(productoPorPedido);

                }
                ActualizarPedidos();
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


        protected void ActualizarPedidos()
        {
            ServicioNegocio SNAux = new ServicioNegocio();
            List<Servicio> ListaServicios = (Helper.Session.GetServicios());

            PedidoNegocio PNAux = new PedidoNegocio();
                List<Pedido> Pedidos = PNAux.ListarPedidos();

            List<Pedido> PedidosAux = new List<Pedido>();

            //busco servicios abiertos
            if (ListaServicios.Count > 0) {
                // Comento validacion para trabajar en front
                //   ListaServicios = ListaServicios.FindAll(x => x.Cobrado != true && x.IdMesero == (int)Session["IdUsuario"]);

            //Aca ya están los servicios guardados
            //

            

            foreach (Pedido Pedido in Pedidos)
            {
                bool esPedidoEnCurso = false;
                
                foreach (Servicio Servicio in ListaServicios)
                {
                    if (Servicio.Id == Pedido.IdServicio)
                    {
                        esPedidoEnCurso = true;
                    }

                }

                if(esPedidoEnCurso!=false) { 
                    PedidosAux.Add(Pedido);
                }

            }
            }

            RepeaterPedidosEnCurso.DataSource = PedidosAux;
            RepeaterPedidosEnCurso.DataBind();
        }

        protected void RepeaterPedidosEnCurso_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Pedido pedido = e.Item.DataItem as Pedido;
            Label lbl = e.Item.FindControl("lbNroMesaPedido") as Label;
            
            lbl.Text = "Mesa " + ((List<Servicio>)Session["Servicios"]).Find(x => x.Id == pedido.IdServicio).Mesa.ToString();

            lbl = e.Item.FindControl("lbCantItemsPedido") as Label;
            lbl.Text = pedido.Productossolicitados.Count().ToString() + " Items Pedidos";

            lbl = e.Item.FindControl("lbEstadoPedido") as Label;
            if(pedido.Estado == 1)
            {
                lbl.Text = "🔴";
            }

        }
	}
}