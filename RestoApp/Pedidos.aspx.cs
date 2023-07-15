using Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominio;

namespace RestoApp
{
	public partial class Pedidos : System.Web.UI.Page
	{
		private Usuario usuario;

		//Querys
		private bool abierto;
		private string dia;

		protected void Page_Load(object sender, EventArgs e)
		{
			//Verificar que sea usuario
			if (AutentificacionUsuario.esUser(Helper.Session.GetUsuario()))
				usuario = Helper.Session.GetUsuario();

			//Verificar Query del número de servicio
			abierto = Convert.ToBoolean(Request.QueryString["abierto"]);
			dia = Request.QueryString["dia"];

			// CONTENIDO GERENTE
			if (!IsPostBack && AutentificacionUsuario.esGerente(usuario))
			{
				try
				{
					//Mostramos los pedidos generales por Mesa y Servico
				}
				catch(Exception ex)
				{
					UIMostrarAlerta(ex.Message);
				}
			}

			// CONTENIDO MESERO
			if (!IsPostBack && AutentificacionUsuario.esMesero(usuario))
			{

				if (abierto)
				{
					//Guardamos el número en session
					Session["ServicioPedido"] = abierto;
					try
					{
						//Llamamos a los pedidos que permanezcan abiertos por Mesa y Servicio
					}
					catch (Exception ex)
					{
						UIMostrarAlerta(ex.Message);
					}
				}

				if(dia != null)
				{
					try
					{
						//Render de todos los Pedidos realizados en el día por Mesa y Servicio
					}
					catch (Exception ex)
					{
						UIMostrarAlerta(ex.Message);
					}
				}

			}

			//Cargamos el repeater
			CargarRepeaterPedidos();

		}

		//UI Alerta Modal
		private void UIMostrarAlerta(string mensaje, string tipoMensaje = "error")
		{
			string scriptModal = $"alertaModal(\"{mensaje}\", \"{tipoMensaje}\");";
			ScriptManager.RegisterStartupScript(this, GetType(), "ScriptTicket", scriptModal, true);

			//Borramos mensaje del modal
			Helper.Session.SetMensajeModal(null);
		}

		private void CargarRepeaterPedidos()
		{
			//repeaterPedidos
		}
	}
}