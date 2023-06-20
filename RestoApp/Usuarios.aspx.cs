using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];

            if (!IsPostBack)
            {
                CargarDgv();
            }

           
        }

        protected void CargarDgv()
        {

            UsuarioNegocio listausuarios = new UsuarioNegocio();
            List<Usuario> Listausuarios = listausuarios.Listar();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Nombres", typeof(string));
            dataTable.Columns.Add("Apellidos", typeof(string));
            dataTable.Columns.Add("Mail", typeof(string));
            dataTable.Columns.Add("Password", typeof(string));
            dataTable.Columns.Add("Fecha de alta", typeof(DateTime));
            dataTable.Columns.Add("Fecha de baja", typeof(DateTime));

            foreach (var usuario in Listausuarios)
            {
                dataTable.Rows.Add(usuario.Id, usuario.Nombres, usuario.Apellidos, usuario.Mail, usuario.Password);
            }
            GDVEmpleados.DataSource = dataTable;
            GDVEmpleados.DataBind();

        }
          
    }
}