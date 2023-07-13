using Opciones;
using System;
using Dominio;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;


namespace Negocio
{
    public class CocinaNegocio {

        public AccesoDB acceso { get; set; }

        public CocinaNegocio() { }

        public CocinaNegocio(AccesoDB db)
        {
            acceso = db;
        }

        public void ActualizarSolicitados()
        {
            acceso = new AccesoDB();
           
            try 
            {
                acceso.setQuery($"SP_ACTUALIZARCOCINA '{DateTime.Now.ToString()}'");                   ;
                acceso.executeNonQuery();
            }
            catch
            {

            }
            finally
            {
                acceso.closeConnection();
            }
        }

        public List<Pedido> ListarPedidosenCocina()
        {
            List<Pedido> Pedidosenpreparacion = new List<Pedido>();
            acceso = new AccesoDB();
            try
            {
                acceso.setQuery($"SELECT EXP.IDPEDIDO, Max(EXP.IDESTADO), EXP.FECHAACTUALIZACION,E.Descripcion" +
                    $" FROM ESTADO_X_PEDIDO EXP JOIN PEDIDO PE ON EXP.IDPEDIDO = PE.IDPEDIDO " +
                    $"join Estadopedido E on Exp.IdEstado = E.IdEstado " +
                    $"WHERE EXP.IDESTADO = 2");
                acceso.executeReader();

                while (acceso.Reader.Read())
                {
                    Pedido aux = new Pedido();
                    aux.Id = acceso.Reader.GetInt32(0);
                    aux.Estado = acceso.Reader.GetInt32(1);
                    aux.ultimaactualizacion = acceso.Reader.GetDateTime(2);
                    aux.EstadoDescripcion = acceso.Reader.GetString(3);
                    Pedidosenpreparacion.Add(aux);
                }
            }
            catch
            { }
            finally {
                acceso.closeConnection(); }

            return Pedidosenpreparacion;
        }

    } }
