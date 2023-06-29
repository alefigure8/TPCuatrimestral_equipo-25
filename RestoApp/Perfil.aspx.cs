using Dominio;
using Helper;
using Opciones;
using System;
using System.Web.UI.WebControls;
using Negocio;
using System.Timers;
using System.EnterpriseServices;

namespace RestoApp
{
    public partial class Perfil : System.Web.UI.Page
    {
        public Usuario usuario { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AutentificacionUsuario.esUser((Usuario)Session[Configuracion.Session.Usuario]))
                usuario = (Usuario)Session[Configuracion.Session.Usuario];

           


            if (!IsPostBack)
            {


                bool Aux = false;
                   Session.Add("Passvisible", Aux);
                Session.Add("Modificarpass", Aux);
                       Cargartextboxs();

            }
        }






        public void Cargartextboxs()
        {
            if (usuario != null)
            {

                TxtId.Text = usuario.Id.ToString();
                TxtNombre.Text = usuario.Nombres.ToString();
                TxtApellido.Text = usuario.Apellidos.ToString();
                txtEmail.Text = usuario.Mail.ToString();
                TxtPassword.Enabled = false;
                TxtPassword.TextMode = TextBoxMode.Password;
                TxtPassword.Attributes["value"] = usuario.Password.ToString();            

            }



        }

        protected void btnVerPassword_Click(object sender, EventArgs e)
        {

            bool aux;
            if ((bool)Session["Passvisible"] == false)
            {   
                aux = true;
                Session.Add("Passvisible", aux);
                TxtPassword.TextMode = TextBoxMode.SingleLine;
                LblAvisopass.Visible = false;
            }
            else
            {
                if ((bool)Session["Modificarpass"] == false)
                {
                    aux = false;
                    Session.Add("Passvisible", aux);
                    TxtPassword.TextMode = TextBoxMode.Password;
                    LblAvisopass.Visible = false;
                }
                else
                {
                    aux = true;
                    Session.Add("Passvisible", aux);
                    TxtPassword.TextMode = TextBoxMode.SingleLine;
                    LblAvisopass.Visible = false;
                }
   

            }

        }



        protected void BtnModificarPass_Click(object sender, EventArgs e)
        {

            bool aux;
            if ((bool)Session["Modificarpass"] == false)
            {
                aux = true;
                Session.Add("Modificarpass", aux);
                Session.Add("Passvisible", aux);
                TxtPassword.TextMode = TextBoxMode.SingleLine;
                TxtPassword.Enabled = aux;
                LblAvisopass.Visible = false;

            }
            else
            {

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                usuario.Password = TxtPassword.Text.ToString();
                usuarioNegocio.Modificarusuario(usuario);
                Cargartextboxs();

                aux = false;
                Session.Add("Modificarpass", aux);
                Session.Add("Passvisible", aux);
                TxtPassword.Enabled = aux;
                              
          
               LblAvisopass.Text = "Contraseña modificada con exito!";
                LblAvisopass.Visible = true;

                 

            }

        }
    }
}