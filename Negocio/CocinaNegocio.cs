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
using System.Runtime.Remoting.Messaging;

namespace Negocio
{
    public class CocinaNegocio
    {

        public AccesoDB acceso { get; set; }

        public CocinaNegocio() { }

        public CocinaNegocio(AccesoDB db)
        {
            acceso = db;
        }

        public void ActualizarSolicitados()
        {
            acceso = new AccesoDB();

            string cambiarformato = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {
                acceso.setQuery(
                    $"declare @FECHAHORA datetime " +
                    $"set @FECHAHORA = CAST('{cambiarformato}' AS DATETIME) " +
                    $"EXEC SP_ACTUALIZARCOCINA @FECHAHORA"); ;
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



                acceso.setQuery($"SELECT ExP.IdPedido,ExP.IdEstado,ExP.FechaActualizacion, e.Descripcion,p.CategoriaProducto FROM ESTADO_X_PEDIDO ExP " +
                        $"join estadopedido E on ExP.IdEstado = e.IdEstado " +
                        $" join PRODUCTO_X_PEDIDO  pxp on pxp.IdPedido = exp.IdPedido " +
                        $"join PRODUCTOS P on pxp.IdProductopordia = P.IdProducto " +
                        $" WHERE ExP.IdEstado = 2 and ExP.idEstado = (Select max(IdEstado) from ESTADO_X_PEDIDO ep where ep.IdPedido = exp.IdPedido) " +
                        $" or ExP.IdEstado = 3 and ExP.idEstado = (Select max(IdEstado) from ESTADO_X_PEDIDO ep where ep.IdPedido = exp.IdPedido) " +
                        $"group by ExP.IdPedido,ExP.IdEstado,ExP.FechaActualizacion, e.Descripcion,p.CategoriaProducto "+
                        $"having CategoriaProducto = 3");
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
            finally
            {
                acceso.closeConnection();
            }

            return Pedidosenpreparacion;
        }

        public List<ProductoPorPedido> ListarProductosPorPedido(int idPedido)
        {
            AccesoDB AccesoDB = new AccesoDB();
            List<ProductoPorPedido> listaproductoporpedidos = new List<ProductoPorPedido>();

            try
            {
                AccesoDB.setQuery($"SELECT  PxP.{ColumnasDB.ProductoPorPedido.IdProductoPorDia} " +
                $", PxP.{ColumnasDB.ProductoPorPedido.Cantidad} " +
                $", P.{ColumnasDB.Producto.Nombre}, P.{ColumnasDB.Producto.TiempoCoccion} " +
                $", P.{ColumnasDB.Producto.Categoria} " +
                $" FROM {ColumnasDB.ProductoPorPedido.DB} PxP " +
                $" JOIN {ColumnasDB.Producto.DB} P ON PxP.{ColumnasDB.ProductoPorPedido.IdProductoPorDia} = P.{ColumnasDB.Producto.Id}" +
                $" WHERE {ColumnasDB.ProductoPorPedido.IdPedido} = {idPedido} and P.CategoriaProducto = 3");
                AccesoDB.executeReader();

                while (AccesoDB.Reader.Read())
                {
                    ProductoPorPedido aux = new ProductoPorPedido();
                    aux.Productodeldia.Nombre = (string)AccesoDB.Reader[ColumnasDB.Producto.Nombre];
                    aux.Productodeldia.TiempoCoccion = (TimeSpan?)AccesoDB.Reader[ColumnasDB.Producto.TiempoCoccion];
                    aux.Productodeldia.Categoria = (int)AccesoDB.Reader[ColumnasDB.Producto.Categoria];
                    aux.Productodeldia.Id = (Int32)AccesoDB.Reader[ColumnasDB.ProductoPorPedido.IdProductoPorDia];
                    aux.Cantidad = (Int32)AccesoDB.Reader[ColumnasDB.ProductoPorPedido.Cantidad];
                    listaproductoporpedidos.Add(aux);
                }

                return listaproductoporpedidos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                AccesoDB.closeConnection();
            }
        }

        public DateTime BuscarhoraingresoCocina(int idPedido)
        {
            AccesoDB AccesoDB = new AccesoDB();
            DateTime horaingreso = new DateTime();
            try
            {
                AccesoDB.setQuery($"SELECT FechaActualizacion FROM ESTADO_X_PEDIDO WHERE IdPedido = {idPedido} and IdEstado = 2");
                AccesoDB.executeReader();

                while (AccesoDB.Reader.Read())
                {
                    horaingreso = (DateTime)AccesoDB.Reader[ColumnasDB.EstadosxPedido.FechaActualizacion];
                }

                return horaingreso;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                AccesoDB.closeConnection();
            }
           
        }

       
        
    }
}