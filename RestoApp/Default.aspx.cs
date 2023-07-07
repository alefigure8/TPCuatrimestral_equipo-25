using System;
using System.Net.Mail;
using Dominio;
using Helper;
using Negocio;
using Opciones;

namespace RestoApp
{
	public partial class Default : System.Web.UI.Page
	{
		private string mail;
		private string pass;

		protected void Page_Load(object sender, EventArgs e)
		{
			if (Session[Configuracion.Session.Error] != null)
			{
				//Mostramos el error
				lbl_error.Text = (string)Session[Configuracion.Session.Error];
			}
		}

		protected void EnviarDatos_Click(object sender, EventArgs e)
		{
			Usuario usuario;
			mail = txb_Usuario.Text;
			pass = txb_Password.Text;
			
			//Validamos datos
			if (ValidarDatos(mail, pass))
			{
				//Buscamos usuario en la base de datos
				UsuarioNegocio usuariosNegocio = new UsuarioNegocio();
				usuario = usuariosNegocio.BuscarUsuario(mail, pass);

				if (AutentificacionUsuario.esUser(usuario))
				{
					//Borramos password
					//usuario.Password = null;
					//Guardamos usuario en session
					Session[Configuracion.Session.Usuario] = usuario;
					//Redirigimos a Panel
					if (usuario.Tipo == ColumnasDB.TipoUsuario.Cocinero)
					{
						Response.Redirect(Configuracion.Pagina.Cocina, false);

					}
					else
					{
						Response.Redirect(Configuracion.Pagina.Main, false);
					}


					Session.Add("IdUsuario", usuario.Id);
				
				}
				else
				{
					lbl_error.Text = Mensajes.Login.DatosIncorrectos;
				}
			}
			else
			{
				//Si los datos no son validos
				lbl_error.Text = Mensajes.Login.FormatosIncorrectos;
			}

		}

		//Validamos Mails y Password
		private bool ValidarDatos(string mail, string pass)
		{
			if (!String.IsNullOrEmpty(mail) && !String.IsNullOrEmpty(pass))
			{
				try
				{
					MailAddress m = new MailAddress(mail);

					return true;
				}
				catch (FormatException)
				{
					return false;
				}
			}

			return false;
		}
	}
}