using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;
using Negocio;
using Opciones;


namespace RestoApp
{
    public partial class Usuarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CargarDgv();
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

            foreach (var usuario in Listausuarios)
            {
                dataTable.Rows.Add(usuario.Id, usuario.Nombres, usuario.Apellidos, usuario.Mail, usuario.Password);
            }
            GDVEmpleados.DataSource = dataTable;
            GDVEmpleados.DataBind();

        }
          
    }
}