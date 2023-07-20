using System;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using Dominio;
using Helper;
using Negocio;
using Opciones;

namespace RestoApp
{
	public partial class Default : System.Web.UI.Page
	{
		public string mail;
		private string pass;
		private string passConfirm;
		public bool esRecuperarPass;
		public string token;

		protected void Page_Load(object sender, EventArgs e)
		{
			//Querys
			esRecuperarPass =  Convert.ToBoolean(Request.QueryString["recuperar"]);
			token =  Request.QueryString["Token"];

			//Mostrar Toast si hay mensajes guardados en sessíon
			if(Helper.Session.GetMensajeModal() != null)
			{
				dynamic msgModal = (dynamic)Helper.Session.GetMensajeModal();
				UIMostrarToast(msgModal.msg, msgModal.tipo);
			}

			if(token != null)
			{
				UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

				//Validamos token y buscamos usario
				mail = usuarioNegocio.RecuperarMailConToken(token);

				if (!String.IsNullOrEmpty(mail))
				{
					//Guardamos email en session
					Session["MailToken"] = mail;
				}
				else
				{
					//Mensaje negativo de que Token No existe 
					object mensajeToast = new { msg = "El token ingresado es incorrecto", tipo = "error" };
					Helper.Session.SetMensajeModal(mensajeToast);

					//Redirigimos
					Response.Redirect("Default.aspx");
				}
			}

			if(esRecuperarPass)
			{
				btnRecuperarPassword.Text = "< Regresar";
				btnForm.Text = "Enviar";
			}
		}

		//Toast
		private void UIMostrarToast(string mensaje, string tipoMensaje = "error")
		{
			string scriptModal = $"alertaToast(\"{mensaje}\", \"{tipoMensaje}\");";
			ScriptManager.RegisterStartupScript(this, GetType(), "scriptDefault", scriptModal, true);

			//Borramos mensaje del modal
			Helper.Session.SetMensajeModal(null);
		}

		//Botón principal
		protected void EnviarDatos_Click(object sender, EventArgs e)
		{
			Usuario usuario;
			mail = txb_Usuario.Text;
			pass = txb_Password.Text;
			passConfirm = txb_PasswordConfirm.Text;

			//Validamos datos
			if (esRecuperarPass && token == null)
			{
				//DB
				UsuarioNegocio usuariosNegocio = new UsuarioNegocio();
				EmailService emailService = new EmailService();

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
					string tokenLink = emailService.GenerarToken();
									
					//Enviamos mail
					try
					{
						string mensaje = "<h2> Recuperacion de Contraseña</h2> <br> <p>Link: <a href='https://localhost:44342/Default.aspx?recuperar=True&token=" + tokenLink + "'>Recuperar Contraseña</a></p>";
						emailService.ArmarCorreo(mail, "Recuperacion Contraseña", mensaje);
						bool seEnvioMail = emailService.EnviarCorreo();

						//Guardamos Token
						if(seEnvioMail)
						{
							UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
							bool seGuardoToken = usuariosNegocio.AgregarToken(mail, tokenLink);

							if (seGuardoToken)
							{

								//Mensaje Positivo
								object mensajeToast = new { msg = "Revise su correo para recuperar la contraseña", tipo = "success" };
								Helper.Session.SetMensajeModal(mensajeToast);
								
								//Redirigimos
								Response.Redirect("Default.aspx");
							}
							else
							{
								//Toast Negativo
								object mensajeToast = new { msg = "Error al generar Token. Intente de nuevo más tarde", tipo = "error" };
								Helper.Session.SetMensajeModal(mensajeToast);
								Response.Redirect("Default.aspx?recuperar=True");
							}
						}
					}
					catch (Exception ex)
					{
						//Mostramos error
						object mensajeToast = new { msg = "El email no pudo ser enviado. Pruebe nuevamente más tarde", tipo = "error" };
						Helper.Session.SetMensajeModal(mensajeToast);

						//Redirigimos
						Response.Redirect("Default.aspx?recuperar=True");
					}
				}
				else
				{
					//Mostramos error
					object mensajeToast = new { msg = "El Email no pertenece a un usuario activo o es incorrecto", tipo = "error" };
					Helper.Session.SetMensajeModal(mensajeToast);

					//Redirigimos
					Response.Redirect("Default.aspx?recuperar=True");
				}
			}
			else if(esRecuperarPass && token != null)
			{
				UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
				
				//Guardamos email en session
				if (Session["MailToken"] != null)
				{
					mail = (string)Session["MailToken"];

					if (!String.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(passConfirm))
					{
						if ((bool)Session["sonIgualesLosPass"])
						{
							//Guardar contraseña nueva
							bool seModificaPass = usuarioNegocio.ModificarPass(mail, pass);

							if(seModificaPass)
							{
								//Metodo para modificar password con email
								bool seEliminoToekn = usuarioNegocio.EliminarToken(mail);

								//Redirigimos
								if (seEliminoToekn)
								{
									//Mostramos error
									object mensajeToast = new { msg = "La contraseña se guardó correctamente", tipo = "success" };
									Helper.Session.SetMensajeModal(mensajeToast);

									//Redirigimos
									Response.Redirect("Default.aspx");
								}
								else
								{
									//Mostramos error
									object mensajeToast = new { msg = "Error al elimninar token. Intente nuevamente más tarde", tipo = "error" };
									Helper.Session.SetMensajeModal(mensajeToast);

									//Redirigimos
									Response.Redirect("Default.aspx");
								}
							}
							else
							{
								//Mostramos error
								object mensajeToast = new { msg = "Error al cambiar contraseña. Intente nuevamente más tarde", tipo = "error" };
								Helper.Session.SetMensajeModal(mensajeToast);

								//Redirigimos
								Response.Redirect("Default.aspx");
							}
						}
					}

				}
				
			}
			else if(ValidarDatos(mail, pass) && !esRecuperarPass)
			{
				//Buscamos usuario en la base de datos
				UsuarioNegocio usuariosNegocio = new UsuarioNegocio();
				usuario = usuariosNegocio.BuscarUsuario(mail, pass);

				if (AutentificacionUsuario.esUser(usuario))
				{
					//Guardamos usuario en session
					Session[Configuracion.Session.Usuario] = usuario;

					//Redirigimos a Panel
					Response.Redirect(Configuracion.Pagina.Main, false);
					Session.Add("IdUsuario", usuario.Id);

				}
				else
				{
					//Error con la información
					object mensajeToast = new { msg = Mensajes.Login.DatosIncorrectos, tipo = "error" };
					Helper.Session.SetMensajeModal(mensajeToast);

					//Redirigimos
					Response.Redirect("Default.aspx");
				}
			}
			else
			{
				//Error con formatos de datos
				object mensajeToast = new { msg = Mensajes.Login.FormatosIncorrectos, tipo = "error" };
				Helper.Session.SetMensajeModal(mensajeToast);

				//Redirigimos
				Response.Redirect("Default.aspx");
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

		//Validamos mail
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

		//Evento para ir a recuperar pass
		protected void RecuperarPass_Click(object sender, EventArgs e)
		{
			esRecuperarPass = !esRecuperarPass;
			Response.Redirect($"Default.aspx?recuperar={esRecuperarPass}", false);
		}

		//Evento de cambio en textBox de confirmar pass
		protected void txb_PasswordConfirm_TextChanged(object sender, EventArgs e)
		{
			pass = txb_Password.Text;
			passConfirm = txb_PasswordConfirm.Text;

			if (pass == passConfirm)
			{
				Session["sonIgualesLosPass"] = true;
			} 
			else
			{
				Session["sonIgualesLosPass"] = false;
				lbl_error.Text = "Las contraseñas no coinciden";
			}
		}
	}
}