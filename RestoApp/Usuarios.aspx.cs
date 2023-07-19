using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Helper;
using Negocio;
using Opciones;
using System.ComponentModel;
using System.Net;


namespace RestoApp
{
    public partial class Usuarios : System.Web.UI.Page
    {
        public Usuario usuario { get; set; }
     
        List<Usuario> Listausuarios;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Si esta logueado asignamos el usuario a la variable usuario
            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];
        
            if (!IsPostBack)
            {
        
                Lblbusquedafallida.Visible = false;
                Rbnmodo.SelectedIndex = 0;
                
                bool modificar = new bool();
                modificar = false;
                Session.Add("modificar", modificar);
                CargarDgv();
                Cargarestadossorting();
                CargarDdltipo();
            }
        }

        public void Cargarestadossorting()
        {
            bool idasc = true;
            bool nombreasc = true;
            bool apellidoasc = true;
            bool mailasc = true;
            bool tipoasc = true;
            Session.Add("idasc", idasc);
            Session.Add("nombreasc", nombreasc);
            Session.Add("apellidoasc", apellidoasc);
            Session.Add("mailasc", mailasc);
            Session.Add("tipoasc", tipoasc);
        }


        public void CargarDgv()
        {

            UsuarioNegocio listausuarios = new UsuarioNegocio();
            Listausuarios = listausuarios.Listar();
            Listausuarios = Filtrolistasegunusuario(usuario, Listausuarios);
            Session.Add("listaactual", Listausuarios);
            GDVEmpleados.DataSource = Convertidordatatable(Listausuarios);
            GDVEmpleados.DataBind();

        }

        public List<Usuario> Filtrolistasegunusuario(Usuario usuario, List<Usuario> lista)
        {
            if (AutentificacionUsuario.esAdmin(usuario))
            {
                lista.RemoveAll(x => x.Tipo == ColumnasDB.TipoUsuario.Admin);
            }
            if (AutentificacionUsuario.esGerente(usuario))
            {
               lista.RemoveAll(x => x.Tipo == ColumnasDB.TipoUsuario.Admin);
                lista.RemoveAll(x => x.Tipo == ColumnasDB.TipoUsuario.Gerente);
            }
            return lista;
                    }


        protected void GDVEmpleados_Sorting(object sender, GridViewSortEventArgs e)
        {
            string columnaseleccionada = e.SortExpression;
            Listausuarios = (List<Usuario>)Session["listaactual"];

            bool idasc = (bool)Session["idasc"];
            bool nombreasc = (bool)Session["nombreasc"];
            bool apellidoasc = (bool)Session["apellidoasc"];
            bool mailasc = (bool)Session["mailasc"];
            bool tipoasc = (bool)Session["tipoasc"];


            if (columnaseleccionada == ColumnasDB.Usuario.Id)
            {
                if (idasc)
                {
                    Listausuarios = Listausuarios.OrderByDescending(x => x.Id).ToList();
                    idasc = false;
                    Session.Add("idasc", idasc);
                }
                else
                {
                    Listausuarios = Listausuarios.OrderBy(x => x.Id).ToList();
                    idasc = true;
                    Session.Add("idasc", idasc);

                }

            }
            else if (columnaseleccionada == ColumnasDB.Usuario.Nombres)
            {
                if (nombreasc)
                {
                    Listausuarios = Listausuarios.OrderByDescending(x => x.Nombres).ToList();
                    nombreasc = false;
                    Session.Add("nombreasc", nombreasc);
                }
                else
                {
                    Listausuarios = Listausuarios.OrderBy(x => x.Nombres).ToList();
                    nombreasc = true;
                    Session.Add("nombreasc", nombreasc);
                }

            }
            else if (columnaseleccionada == ColumnasDB.Usuario.Apellidos)
            {
                if (apellidoasc)
                {
                    Listausuarios = Listausuarios.OrderByDescending(x => x.Apellidos).ToList();
                    apellidoasc = false;
                    Session.Add("apellidoasc", apellidoasc);
                }
                else
                {
                    Listausuarios = Listausuarios.OrderBy(x => x.Apellidos).ToList();
                    apellidoasc = true;
                    Session.Add("apellidoasc", apellidoasc);
                }
            }
            else if (columnaseleccionada == ColumnasDB.Usuario.Mail)
            {
                if (mailasc)
                {
                    Listausuarios = Listausuarios.OrderByDescending(x => x.Mail).ToList();
                    mailasc = false;
                    Session.Add("mailasc", mailasc);
                }
                else
                {
                    Listausuarios = Listausuarios.OrderBy(x => x.Mail).ToList();
                    mailasc = true;
                    Session.Add("mailasc", mailasc);

                }
            }
            else if (columnaseleccionada == ColumnasDB.Usuario.TipoUsuario)
            {
                if (tipoasc)
                {
                    Listausuarios = Listausuarios.OrderByDescending(x => x.Tipo).ToList();
                    tipoasc = false;
                    Session.Add("tipoasc", tipoasc);
                }
                else
                {
                    Listausuarios = Listausuarios.OrderBy(x => x.Tipo).ToList();
                    tipoasc = true;
                    Session.Add("tipoasc", tipoasc);

                }
            }

            Session.Add("listaactual", Listausuarios);
            GDVEmpleados.DataSource = Convertidordatatable(Listausuarios);
            GDVEmpleados.DataBind();

        }

        public DataTable Convertidordatatable(List<Usuario> listausuarios)
        {

            if (listausuarios != null)
            {

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(ColumnasDB.Usuario.Id, typeof(int));
                dataTable.Columns.Add(ColumnasDB.Usuario.Nombres, typeof(string));
                dataTable.Columns.Add(ColumnasDB.Usuario.Apellidos, typeof(string));
                dataTable.Columns.Add(ColumnasDB.Usuario.Mail, typeof(string));
                dataTable.Columns.Add(ColumnasDB.Usuario.Pass, typeof(string));
                dataTable.Columns.Add(ColumnasDB.Usuario.TipoUsuario, typeof(string));
               // dataTable.Columns.Add("Fecha de alta", typeof(DateTime));
          

                foreach (var usuario in listausuarios)
                {
                    dataTable.Rows.Add(usuario.Id, usuario.Nombres, usuario.Apellidos, usuario.Mail, usuario.Password, usuario.Tipo);
                }

                return dataTable;
            }
            else
            {
                return null;
            }
        }


        public void Limpiarcamposdetexto()
         {
            TxtApellidos.Text = "";
            TxtNombres.Text = "";
            TxtId.Text = "";
            TxtMail.Text = "";
            TxtPassword.Text = "";

        }

        public void CargarDdltipo()
        {

            DdlTipo.Items.Add(ColumnasDB.TipoUsuario.Mesero);
            DdlTipo.Items.Add(ColumnasDB.TipoUsuario.Cocinero);
            if (AutentificacionUsuario.esAdmin(usuario))
            {
                DdlTipo.Items.Add(ColumnasDB.TipoUsuario.Gerente);
            }
            

        }

        protected void GDVEmpleados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarUsuario")
            {
                if (AutentificacionUsuario.esAdmin(usuario) || AutentificacionUsuario.esGerente(usuario))
                {
                    UsuarioNegocio negocio = new UsuarioNegocio();
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    List<Usuario> listausuarios = (List<Usuario>)Session["listaactual"];

                    if (rowIndex >= 0 && rowIndex < listausuarios.Count)
                    {
                        Usuario usuarioSeleccionado = listausuarios[rowIndex];

                        negocio.BajalogicaUsuario(usuarioSeleccionado.Id);

                        CargarDgv();
                    }
                }
            }
            if(e.CommandName == "ModificarUsuario")
            {
                if ((bool)Session["modificar"])
                {
                    if (AutentificacionUsuario.esAdmin(usuario) || AutentificacionUsuario.esGerente(usuario))
                    {
                        int rowIndex = Convert.ToInt32(e.CommandArgument);
                    List<Usuario> listausuarios = (List<Usuario>)Session["listaactual"];

                    if (rowIndex >= 0 && rowIndex < listausuarios.Count)
                    {
                        Usuario usuarioSeleccionado = listausuarios[rowIndex];
                        TxtApellidos.Text = usuarioSeleccionado.Apellidos;
                        TxtNombres.Text = usuarioSeleccionado.Nombres;
                        TxtId.Text = usuarioSeleccionado.Id.ToString();
                        TxtMail.Text = usuarioSeleccionado.Mail;
                        TxtPassword.Text = usuarioSeleccionado.Password;
                        DdlTipo.SelectedValue = usuarioSeleccionado.Tipo;

                        CargarDgv();
                    }
                    }
                }
                else
                {
                    Limpiarcamposdetexto();

                }
            }
        }


        public bool camposcompletos()
        {
            if(TxtApellidos.Text.ToString() != "" && TxtNombres.Text.ToString() != "" && TxtMail.Text.ToString() != "" && TxtPassword.Text.ToString() != "" && DdlTipo.SelectedValue.ToString() != "")
            {
                return true;
            }
            else
            {
                return false;
            }            

        }

         public void BtnConfirmarcambios_Click(object sender, EventArgs e)
        {
            bool aux;
            if ((bool)Session["modificar"] == false)
            {
                if (AutentificacionUsuario.esAdmin(usuario) || AutentificacionUsuario.esGerente(usuario))
                {

                    if (emailvalido(TxtMail.Text.ToString()) && camposcompletos())
                    {
                        Usuario nuevousuario = new Usuario();
                        UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                        nuevousuario.Apellidos = TxtApellidos.Text.ToString();
                        nuevousuario.Nombres = TxtNombres.Text.ToString();
                        nuevousuario.Tipo = DdlTipo.SelectedValue.ToString();
                        nuevousuario.Mail = TxtMail.Text.ToString();
                        nuevousuario.Password = TxtPassword.Text.ToString();

                        usuarioNegocio.Agregarusuario(nuevousuario);
                        LblError.Visible = false;
                        CargarDgv();

                    

                        Limpiarcamposdetexto();
                    }
                    else if(!camposcompletos())
                    {
                        LblError.Text = "*Debe completar todos los campos.";
                        LblError.Visible = true;
                    }
                    else if (!emailvalido(TxtMail.Text.ToString()))
                    {
                        LblError.Text = "*El mail ingresado no es válido.";
                        LblError.Visible = true;
                    }
                }
               
            }
            else
            {
                if (emailvalido(TxtMail.Text.ToString()) && camposcompletos())
                {
                Usuario usuariomodificado = new Usuario();
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                usuariomodificado.Id = Convert.ToInt32(TxtId.Text);
                usuariomodificado.Nombres = TxtNombres.Text;
                usuariomodificado.Apellidos = TxtApellidos.Text;
                usuariomodificado.Mail = TxtMail.Text;
                usuariomodificado.Password = TxtPassword.Text;
                usuariomodificado.Tipo = DdlTipo.SelectedValue.ToString();
                usuarioNegocio.Modificarusuario(usuariomodificado);

                LblError.Visible = false;
                aux = false;
                Session.Add("modificar", aux);
                CargarDgv();
                Limpiarcamposdetexto();
                Rbnmodo.SelectedIndex = 0;

                }
                else if (!camposcompletos())
                {
                    LblError.Text = "*Debe completar todos los campos.";
                    LblError.Visible = true;
                }
                else if (!emailvalido(TxtMail.Text.ToString()))
                {
                    LblError.Text = "*El mail ingresado no es válido.";
                    LblError.Visible = true;
                }
            }
            
        }

        public bool emailvalido(string email)
        {
            return Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        }

        protected void BtnModificarusuario_Click(object sender, EventArgs e)
        {
            bool aux;
            if ((bool)Session["modificar"]==false)
            {
                Rbnmodo.SelectedIndex = 1;
                aux = true;
               Session.Add("modificar", aux);

            }
            else 
            {
              
                if (TxtId.Text == "")
                {
                    Rbnmodo.SelectedIndex = 0;
                    aux = false;
                    Session.Add("modificar", aux);
                    Limpiarcamposdetexto();
                }
            }
        }

     

        protected void BtnBusqueda_Click(object sender, EventArgs e)
        {
            if (TxtBusqueda.Text == "")
            {
                CargarDgv();
            }
            else
            {
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                List<Usuario> listausuarios = new List<Usuario>();
                listausuarios = usuarioNegocio.Listar();
          
                Session.Add("listaactual", listausuarios);
                listausuarios = Listafiltrada(listausuarios);
                GDVEmpleados.DataSource = Convertidordatatable(listausuarios);
                GDVEmpleados.DataBind();
            }
        }

        public List<Usuario> Listafiltrada(List<Usuario> lista)
        {
            lista.RemoveAll(x => !x.Nombres.ToUpper().Contains(TxtBusqueda.Text.ToUpper()));
            lista.RemoveAll(x => !x.Apellidos.ToUpper().Contains(TxtBusqueda.Text.ToUpper()));
            lista.RemoveAll(x => !x.Mail.ToUpper().Contains(TxtBusqueda.Text.ToUpper()));
            lista.RemoveAll(x => !x.Tipo.ToUpper().Contains(TxtBusqueda.Text.ToUpper()));

            return lista;
        }

        protected void TxtBusqueda_TextChanged(object sender, EventArgs e)
        {

            List<Usuario> Listafiltrada = new List<Usuario>();

            if (TxtBusqueda.Text.Count() > 0)
            {
                Listafiltrada = ((List<Usuario>)Session["listaactual"]).FindAll(x => x.Nombres.ToUpper().Contains(TxtBusqueda.Text.ToUpper()) || x.Apellidos.ToUpper().Contains(TxtBusqueda.Text.ToUpper()) || x.Mail.ToUpper().Contains(TxtBusqueda.Text.ToUpper()));
                if(Listafiltrada.Count == 0)
                {
                    Lblbusquedafallida.Visible = true;
                }
                else
                {
                    GDVEmpleados.DataSource = Convertidordatatable(Listafiltrada);
                    GDVEmpleados.DataBind();
                    Lblbusquedafallida.Visible = false;
                }
             
            }
            else
            {
                Lblbusquedafallida.Visible = false;
                CargarDgv();
            }
        }

        protected void Rbnmodo_SelectedIndexChanged(object sender, EventArgs e)
        {
           bool aux;
            if(((uint)Rbnmodo.SelectedIndex) == 0)
            {
                aux = false;
                Session.Add("modificar", aux);
                Limpiarcamposdetexto();
            }
        }
    }

}