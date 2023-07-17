using System;
using System.Net.Mail;
using System.Text;
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
		public bool esRecuperarPass;
		string token;

		protected void Page_Load(object sender, EventArgs e)
		{
			//Querys
			esRecuperarPass =  Convert.ToBoolean(Request.QueryString["recuperar"]);
			token =  Request.QueryString["Token"];

			if (Session[Configuracion.Session.Error] != null)
			{
				//Mostramos el error
				lbl_error.Text = (string)Session[Configuracion.Session.Error];
			}

			if(token != null)
			{
				//Buscamos token en el base de datos
				//Si existe redirigimos a cambio de password
			}

			if(esRecuperarPass)
			{
				btnRecuperarPassword.Text = "< Regresar";
				btnForm.Text = "Enviar";
			}
		}

		protected void EnviarDatos_Click(object sender, EventArgs e)
		{
			Usuario usuario;
			mail = txb_Usuario.Text;
			pass = txb_Password.Text;
			
			//Validamos datos
			if (esRecuperarPass && token == null)
			{
				//DB
				UsuarioNegocio usuariosNegocio = new UsuarioNegocio();

				//CorroborarMail
				if (!ValidarMail(mail))
				{
					//Si los datos no son validos
					lbl_error.Text = Mensajes.Login.FormatosIncorrectos;
					return;
				}

				//Verificar que mail exista
				if (usuariosNegocio.BuscarUsuarioPorMail(mail) == 1)
				{
					//Generamos Token
					string tokenLink = GenerarToken();
					
					//Guardamos Token en el usuario

					
					//Enviamos mail
					try
					{
						string mensaje = "<h2> Recuperacion de Contraseña</h2> <br> <p>Link: </p><a href='http://localhost:5000/Default.aspx?recuperar=true&token=" + tokenLink + "'>Recuperar Contraseña</a>";
						EmailService emailService = new EmailService();
						emailService.ArmarCorreo(mail, "Recuperacion Contraseña", mensaje);
						emailService.EnviarCorreo();
					}
					catch (Exception ex)
					{
						//Mostramos error
						lbl_error.Text = "El email no pudo ser enviado. Pruebe nuevamente más tarde";
						return;
					}

					//Redirigimos
					Response.Redirect("Default.aspx");
				}
				else
				{
					//Mensaje de error
					lbl_error.Text = "El Email no pertenece a un usuario activo o es incorrecto";
				}
			}
			else if(esRecuperarPass && token != null)
			{
				//Validamos token y buscamos usario

				//Guardamos email en session

				//Metodo para modificar password con email

				//Redirigimos
				lbl_error.Text = "Token";
			}
			else if(ValidarDatos(mail, pass) && !esRecuperarPass)
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
					Response.Redirect(Configuracion.Pagina.Main, false);
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

		private bool ValidarMail(string mail)
		{
			if(!String.IsNullOrEmpty(mail))
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

		protected void RecuperarPass_Click(object sender, EventArgs e)
		{
			esRecuperarPass = !esRecuperarPass;
			Response.Redirect($"Default.aspx?recuperar={esRecuperarPass}", false);
		}

		private string GenerarToken()
		{
			int length = 10;
			const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			Random random = new Random();

			StringBuilder tokenBuilder = new StringBuilder(length);
			int charactersLength = Characters.Length;

			for (int i = 0; i < length; i++)
			{
				int randomIndex = random.Next(charactersLength);
				char randomCharacter = Characters[randomIndex];
				tokenBuilder.Append(randomCharacter);
			}

			return tokenBuilder.ToString();
		}
	}
}