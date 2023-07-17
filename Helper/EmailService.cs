using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
	public class EmailService
	{
		private MailMessage email;
		private SmtpClient server;

        public EmailService()
        {
			server = new SmtpClient();
			server.Host = "smtp.gmail.com";
			server.Port = 587;
			server.EnableSsl = true;

			//User y pass en web.config
			server.Credentials = new System.Net.NetworkCredential("suburbiadev@gmail.com", "pago7611");

		}

        public void ArmarCorreo(string to, string subject, string body)
		{
			//Armamos correo
			email = new MailMessage();
			email.From = new MailAddress("noresponder@grupo25.com");
			email.To.Add(to);
			email.Subject = subject;
			email.IsBodyHtml = true;
			email.Body = body;
		}

		public void EnviarCorreo()
		{
			//Enviamos Correo
			try
			{
				server.Send(email);
			}
			catch(Exception ex)
			{
				throw new Exception("Error al enviar el correo: " + ex.Message);
			}
		}
	}
}
