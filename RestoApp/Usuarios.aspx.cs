using System;
using System.Collections.Generic;
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


namespace RestoApp
{
    public partial class Usuarios : System.Web.UI.Page
    {
        public Usuario usuario { get; set; }

        List<Usuario> Listausuarios;

      

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];

            if (!IsPostBack)
            {
                CargarDgv();
                Cargarestadossorting();


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

            if (AutentificacionUsuario.esAdmin(usuario))
            {
                Listausuarios.RemoveAll(x => x.Tipo == ColumnasDB.TipoUsuario.Admin);
            }
            if (AutentificacionUsuario.esGerente(usuario))
            {
                Listausuarios.RemoveAll(x => x.Tipo == ColumnasDB.TipoUsuario.Admin);
                Listausuarios.RemoveAll(x => x.Tipo == ColumnasDB.TipoUsuario.Gerente);
            }

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(ColumnasDB.Usuario.Id, typeof(int));
            dataTable.Columns.Add(ColumnasDB.Usuario.Nombres, typeof(string));
            dataTable.Columns.Add(ColumnasDB.Usuario.Apellidos, typeof(string));
            dataTable.Columns.Add(ColumnasDB.Usuario.Mail, typeof(string));
            dataTable.Columns.Add(ColumnasDB.Usuario.Pass, typeof(string));
            dataTable.Columns.Add(ColumnasDB.Usuario.TipoUsuario, typeof(string));
            dataTable.Columns.Add("Fecha de alta", typeof(DateTime));
            dataTable.Columns.Add("Fecha de baja", typeof(DateTime));

            foreach (var usuario in Listausuarios)
            {
                dataTable.Rows.Add(usuario.Id, usuario.Nombres, usuario.Apellidos, usuario.Mail, usuario.Password, usuario.Tipo);
            }

            Session.Add("listaactual", Listausuarios);
            GDVEmpleados.DataSource = Listausuarios;
            GDVEmpleados.DataBind();

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



            if (columnaseleccionada == "Id")
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
            else if (columnaseleccionada == "Tipo")
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
            GDVEmpleados.DataSource = Listausuarios;
            GDVEmpleados.DataBind();

        }
    }
}