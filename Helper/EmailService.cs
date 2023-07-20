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
			server.Credentials = new System.Net.NetworkCredential("suburbia.dev@gmail.com", "euitndnqfgdcynxz");

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

		public bool EnviarCorreo()
		{
			//Enviamos Correo
			try
			{
				server.Send(email);
				return true;
			}
			catch(Exception ex)
			{
				throw new Exception("Error al enviar el correo: " + ex.Message);
			}
		}

		public string GenerarToken()
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
