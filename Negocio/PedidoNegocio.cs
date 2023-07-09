using Opciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;



namespace Negocio
{
    public class PedidoNegocio
    {
        public int GenerarIdPedido(int Servicio)
        {
            int IdPedido = 0;

            AccesoDB datos = new AccesoDB();
            try
            {
                datos.setQuery(
                    $"INSERT INTO {ColumnasDB.Pedido.DB} ({ColumnasDB.Pedido.IdServicio}) " +
                    $"VALUES ( {Servicio})" + 
                    $"SELECT CAST(scope_identity() AS int)"
                    );
                 
                IdPedido = datos.executeScalar();
            }
            catch (Exception Ex)
            {
                return IdPedido=-1;
            }
            finally
            {
                datos.closeConnection();
            }
            return IdPedido;
        }

        public int AbrirPedido(Pedido pedido)
        {
            pedido.Id = 0;
            AccesoDB datos = new AccesoDB();
            
            try
            {
                pedido.Id= GenerarIdPedido(pedido.IdServicio);
                CargarEstadoPedido(pedido.Id);
                foreach (ProductoPorPedido producto in pedido.Productossolicitados)
                {
                    datos.setQuery(
                       
                        $"INSERT INTO {ColumnasDB.ProductoPorPedido.DB} ({ColumnasDB.ProductoPorPedido.IdPedido}, {ColumnasDB.ProductoPorPedido.IdProductoPorDia}, {ColumnasDB.ProductoPorPedido.Cantidad}, {ColumnasDB.ProductoPorPedido.Fecha}) " +
                        $"VALUES ({pedido.Id}, {producto.Productodeldia.Id}, {producto.Cantidad},'{DateTime.Now.ToString("yyyy-MM-dd")}')");

                    datos.executeNonQuery();
                    datos.closeConnection();
                }
              
            }

            catch (Exception Ex)
            {
                return pedido.Id;
            }
            finally
            {
                datos.closeConnection();
            }
            return pedido.Id;
        }

        public int CargarEstadoPedido(int idpedido)
        {
       
            AccesoDB datos = new AccesoDB();

            try
            {
                                                   
                   datos.setQuery(
                    $"INSERT INTO {ColumnasDB.EstadosxPedido.DB} ({ColumnasDB.EstadosxPedido.IdPedido}, {ColumnasDB.EstadosxPedido.IdEstado}, {ColumnasDB.EstadosxPedido.FechaActualizacion}) " +
                    $"VALUES ({idpedido}, 1 , '{DateTime.Now.ToString("G")}')");
                    datos.executeNonQuery();

            }

            catch (Exception Ex)
            {
                return idpedido = -1;
            }
            finally
            {
                datos.closeConnection();
            }
            return idpedido;
        }

      


        



            public List<ProductoPorPedido> ListarProductosPorPedido(int IdPedido)
        {
            AccesoDB AccesoDB = new AccesoDB();
            List<ProductoPorPedido> listaproductoporpedidos = new List<ProductoPorPedido>();

            try
            {
                AccesoDB.setQuery($"SELECT  PxP.{ColumnasDB.ProductoPorPedido.IdProductoPorDia} "+
                $", PxP.{ColumnasDB.ProductoPorPedido.Cantidad} " +   
                $", P.{ColumnasDB.Producto.Nombre}, P.{ColumnasDB.Producto.TiempoCoccion} " +
                $", P.{ColumnasDB.Producto.Categoria} " +
                $" FROM {ColumnasDB.ProductoPorPedido.DB} PxP " +
                $" JOIN {ColumnasDB.Producto.DB} P ON PxP.{ColumnasDB.ProductoPorPedido.IdProductoPorDia} = P.{ColumnasDB.Producto.Id}" +
                $" WHERE {ColumnasDB.ProductoPorPedido.IdPedido} = {IdPedido} ");
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


        public List<Pedido> ListarPedidos()
        {
            AccesoDB AccesoDB = new AccesoDB();
            List<Pedido> listapedidos = new List<Pedido>();
                

            try
            {
                AccesoDB.setQuery($"SELECT P.{ColumnasDB.Pedido.Id} "+
                    $", P.{ColumnasDB.Pedido.IdServicio} " +                                                    
                    $", E.{ColumnasDB.Estados.Id} " +
                    $", E.{ColumnasDB.Estados.Descripcion} " +  
                    $", ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} " +
                    $" FROM {ColumnasDB.Pedido.DB} P " +
                    $" JOIN {ColumnasDB.EstadosxPedido.DB} ExP ON P.{ColumnasDB.Pedido.Id} = ExP.{ColumnasDB.EstadosxPedido.IdPedido} " +
                    $" JOIN {ColumnasDB.Estados.DB} E on ExP.{ColumnasDB.EstadosxPedido.IdEstado} = E.{ColumnasDB.Estados.Id}" +
                    $" WHERE ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id})"
                    );

                AccesoDB.executeReader();
                
                while (AccesoDB.Reader.Read())
                {
                    Pedido aux = new Pedido();
                    aux.Id = (Int32)AccesoDB.Reader[ColumnasDB.Pedido.Id];
                    aux.IdServicio = (Int32)AccesoDB.Reader[ColumnasDB.Pedido.IdServicio];
                    aux.Estado = (Int32)AccesoDB.Reader[ColumnasDB.Estados.Id];
                    aux.EstadoDescripcion = (string)AccesoDB.Reader[ColumnasDB.Estados.Descripcion];
                    aux.ultimaactualizacion = (DateTime)AccesoDB.Reader[ColumnasDB.EstadosxPedido.FechaActualizacion];
                    aux.Productossolicitados = ListarProductosPorPedido(aux.Id);
                    listapedidos.Add(aux);
                }

                return listapedidos;
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

        /*

        public List<Pedido> ListarPedidos(int estado1,int estado2)
        {
            AccesoDB AccesoDB = new AccesoDB();
            List<Pedido> listapedidos = new List<Pedido>();
               

            try
            {
                AccesoDB.setQuery($"SELECT P.{ColumnasDB.Pedido.Id} "+
                $", P.{ColumnasDB.Pedido.IdServicio} " +                                                    
                $", E.{ColumnasDB.Estados.Id} " +
                $", E.{ColumnasDB.Estados.Descripcion} " +  
               $", ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} " +
                 $" FROM {ColumnasDB.Pedido.DB} P " +
                 $" JOIN {ColumnasDB.EstadosxPedido.DB} ExP ON P.{ColumnasDB.Pedido.Id} = ExP.{ColumnasDB.EstadosxPedido.IdPedido} " +
                $" JOIN {ColumnasDB.Estados.DB} E on ExP.{ColumnasDB.EstadosxPedido.IdEstado} = E.{ColumnasDB.Estados.Id}" +                              
                $" WHERE ExP.{ColumnasDB.EstadosxPedido.IdEstado} in (  {estado1}, {estado2})"
                                                                                                                                                                                               );

                AccesoDB.executeReader();
                
                while (AccesoDB.Reader.Read())
                {
                    Pedido aux = new Pedido();
                    aux.Id = (Int32)AccesoDB.Reader[ColumnasDB.Pedido.Id];
                    aux.IdServicio = (Int32)AccesoDB.Reader[ColumnasDB.Pedido.IdServicio];
                    aux.Estado = (Int32)AccesoDB.Reader[ColumnasDB.Estados.Id];
                    aux.EstadoDescripcion = (string)AccesoDB.Reader[ColumnasDB.Estados.Descripcion];
                    aux.ultimaactualizacion = (DateTime)AccesoDB.Reader[ColumnasDB.EstadosxPedido.FechaActualizacion];
                    aux.Productossolicitados = ListarProductosPorPedido(aux.Id);
                    listapedidos.Add(aux);
                }

                return listapedidos;
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
        */

        public List<Pedido> ListarPedidos(int estado1)
        {
            AccesoDB AccesoDB = new AccesoDB();
            List<Pedido> listapedidos = new List<Pedido>();


            try
            {
                AccesoDB.setQuery($"SELECT P.{ColumnasDB.Pedido.Id} " +
                $", P.{ColumnasDB.Pedido.IdServicio} " +
                $", E.{ColumnasDB.Estados.Id} " +
                $", E.{ColumnasDB.Estados.Descripcion} " +
               $", ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} " +
                 $" FROM {ColumnasDB.Pedido.DB} P " +
                 $" JOIN {ColumnasDB.EstadosxPedido.DB} ExP ON P.{ColumnasDB.Pedido.Id} = ExP.{ColumnasDB.EstadosxPedido.IdPedido} " +
                $" JOIN {ColumnasDB.Estados.DB} E on ExP.{ColumnasDB.EstadosxPedido.IdEstado} = E.{ColumnasDB.Estados.Id}" +
                $" WHERE ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id})"+
                $" AND ExP.{ColumnasDB.EstadosxPedido.IdEstado} =  {estado1} "
                                                                                                                                                                                               );

                AccesoDB.executeReader();

                while (AccesoDB.Reader.Read())
                {
                    Pedido aux = new Pedido();
                    aux.Id = (Int32)AccesoDB.Reader[ColumnasDB.Pedido.Id];
                    aux.IdServicio = (Int32)AccesoDB.Reader[ColumnasDB.Pedido.IdServicio];
                    aux.Estado = (Int32)AccesoDB.Reader[ColumnasDB.Estados.Id];
                    aux.EstadoDescripcion = (string)AccesoDB.Reader[ColumnasDB.Estados.Descripcion];
                    aux.ultimaactualizacion = (DateTime)AccesoDB.Reader[ColumnasDB.EstadosxPedido.FechaActualizacion];
                    aux.Productossolicitados = ListarProductosPorPedido(aux.Id);
                    listapedidos.Add(aux);
                }

                return listapedidos;
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

        public void CambiarEstadoPedido(int idpedido, int nuevoestado)
        {
            AccesoDB datos = new AccesoDB();
            try
            {
                          
                datos.setQuery(
                 $"INSERT INTO {ColumnasDB.EstadosxPedido.DB} ({ColumnasDB.EstadosxPedido.IdPedido}, {ColumnasDB.EstadosxPedido.IdEstado}, {ColumnasDB.EstadosxPedido.FechaActualizacion}) " +
                 $"VALUES ({idpedido}, {nuevoestado} , '{DateTime.Now.ToString("G")}')");
                datos.executeNonQuery();
                datos.closeConnection();
            }

            catch (Exception Ex)
            {
              
            }
            finally
            {
                datos.closeConnection();
            }
             

        }
    }
}
