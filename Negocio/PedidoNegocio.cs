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
                return IdPedido = -1;
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
                pedido.Id = GenerarIdPedido(pedido.IdServicio);
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

            string formato = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            try
            {

                datos.setQuery(
                 $"INSERT INTO {ColumnasDB.EstadosxPedido.DB} ({ColumnasDB.EstadosxPedido.IdPedido}, {ColumnasDB.EstadosxPedido.IdEstado}, {ColumnasDB.EstadosxPedido.FechaActualizacion}) " +
                 $"VALUES ({idpedido}, 1 , CAST('{formato}' AS DATETIME))");
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
                AccesoDB.setQuery($"SELECT  PxP.{ColumnasDB.ProductoPorPedido.IdProductoPorDia} " +
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


        // Listar pedidos del dia, valida que la fecha del servicio asociado al pedido sea de la fecha actual y filtra por Id de mesero
        public List<Pedido> ListarPedidosDelDia(int IdMesero)
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
                    $" JOIN {ColumnasDB.Servicio.DB} S on S.{ColumnasDB.Servicio.Id} = P.{ColumnasDB.Pedido.IdServicio}" +
                    $" JOIN {ColumnasDB.MesasPorDia.DB} MxD on MxD.{ColumnasDB.MesasPorDia.Id} = S.{ColumnasDB.Servicio.IdMesa}" +
                    $" WHERE MxD.{ColumnasDB.MesasPorDia.IdMesero} = {IdMesero} " +
                    $" AND S.{ColumnasDB.Servicio.Fecha} = '{DateTime.Now.ToString("yyyy-MM-dd")}' " +
                    $" AND ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id})"
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


        // Lista pedidos del dia para gerente, valida que el estado sea distinto a entregado
        public List<Pedido> ListarPedidosDelDia()
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
                    $" JOIN {ColumnasDB.Servicio.DB} S on S.{ColumnasDB.Servicio.Id} = P.{ColumnasDB.Pedido.IdServicio}" +
                    $" JOIN {ColumnasDB.MesasPorDia.DB} MxD on MxD.{ColumnasDB.MesasPorDia.Id} = S.{ColumnasDB.Servicio.IdMesa}" +
                    //$" WHERE S.{ColumnasDB.Servicio.Fecha} = '{DateTime.Now.ToString("yyyy-MM-dd")}' " +
                    //$" AND ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id}" +
                    $" WHERE ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id}" +
					$" AND LOWER(E.{ColumnasDB.Estados.Descripcion}) <> 'entregado')"
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

		public List<Pedido> ListarPedidosDelDiaPorMesa(int mesa)
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
					$" JOIN {ColumnasDB.Servicio.DB} S on S.{ColumnasDB.Servicio.Id} = P.{ColumnasDB.Pedido.IdServicio}" +
					$" JOIN {ColumnasDB.MesasPorDia.DB} MxD on MxD.{ColumnasDB.MesasPorDia.Id} = S.{ColumnasDB.Servicio.IdMesa}" +
					//$" WHERE S.{ColumnasDB.Servicio.Fecha} = '{DateTime.Now.ToString("yyyy-MM-dd")}' " +
					//$" AND ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id}" +
					$" WHERE ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id})" +
					$" AND MxD.{ColumnasDB.MesasPorDia.IdMesa} = {mesa}"
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

        //Listamos pedidos por servicio
		public List<Pedido> ListarPedidosDelDiaPorServicio(int servicio)
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
					$" JOIN {ColumnasDB.Servicio.DB} S on S.{ColumnasDB.Servicio.Id} = P.{ColumnasDB.Pedido.IdServicio}" +
					$" JOIN {ColumnasDB.MesasPorDia.DB} MxD on MxD.{ColumnasDB.MesasPorDia.Id} = S.{ColumnasDB.Servicio.IdMesa}" +
					//$" WHERE S.{ColumnasDB.Servicio.Fecha} = '{DateTime.Now.ToString("yyyy-MM-dd")}' " +
					//$" AND ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id}" +
					$" WHERE ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id})" +
					$" AND P.{ColumnasDB.Pedido.IdServicio} = {servicio}"
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


		// Busca un pedido por ID
		public Pedido BuscarPedido(int IdPedido)
        {
            AccesoDB AccesoDB = new AccesoDB();
            Pedido Pedido = new Pedido();

            try
            {
                //AccesoDB.setQuery($"SELECT " +
                //  $"P.{ColumnasDB.Pedido.Id} " +
                //  $", P.{ColumnasDB.Pedido.IdServicio} " +
                //  $", E.{ColumnasDB.Estados.Id} " +
                //  $", E.{ColumnasDB.Estados.Descripcion} " +
                //  $", ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} " +
                //  $" FROM {ColumnasDB.Pedido.DB} P " +
                //  $" JOIN {ColumnasDB.EstadosxPedido.DB} ExP ON P.{ColumnasDB.Pedido.Id} = ExP.{ColumnasDB.EstadosxPedido.IdPedido} " +
                //  $" JOIN {ColumnasDB.Estados.DB} E on ExP.{ColumnasDB.EstadosxPedido.IdEstado} = E.{ColumnasDB.Estados.Id}" +
                //  $" WHERE {ColumnasDB.Pedido.Id} = {IdPedido} AND" +
                //  $" ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id})"
                //  );

                AccesoDB.setQuery($"SELECT " +
                    $" P.{ColumnasDB.Pedido.Id}, " +
                    $" P.{ColumnasDB.Pedido.IdServicio}, " +
                    $" E.{ColumnasDB.Estados.Id}, " +
                    $" E.{ColumnasDB.Estados.Descripcion}, " +
                    $" ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} " +
                    $" FROM {ColumnasDB.Pedido.DB} P " +
                    $" JOIN {ColumnasDB.EstadosxPedido.DB} ExP ON P.{ColumnasDB.Pedido.Id} = ExP.{ColumnasDB.EstadosxPedido.IdPedido} " +
                    $" JOIN {ColumnasDB.Estados.DB} E ON ExP.{ColumnasDB.EstadosxPedido.IdEstado} = E.{ColumnasDB.Estados.Id} " +
                    $" WHERE P.{ColumnasDB.Pedido.Id} = {IdPedido} " +
                    $" AND ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) " +
                    $" FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id})");

                AccesoDB.executeReader();

                while (AccesoDB.Reader.Read())
                {
                    Pedido.Id = (Int32)AccesoDB.Reader[ColumnasDB.Pedido.Id];
                    Pedido.IdServicio = (Int32)AccesoDB.Reader[ColumnasDB.Pedido.IdServicio];
                    Pedido.Estado = (Int32)AccesoDB.Reader[ColumnasDB.Estados.Id];
                    Pedido.EstadoDescripcion = (string)AccesoDB.Reader[ColumnasDB.Estados.Descripcion];
                    Pedido.ultimaactualizacion = (DateTime)AccesoDB.Reader[ColumnasDB.EstadosxPedido.FechaActualizacion];
                    Pedido.Productossolicitados = ListarProductosPorPedido(IdPedido);
                }

                return Pedido;

            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                AccesoDB.closeConnection();
            }

        }


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
                $" WHERE ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion} = (SELECT MAX(ExP.{ColumnasDB.EstadosxPedido.FechaActualizacion}) " +
                $"FROM {ColumnasDB.EstadosxPedido.DB} ExP WHERE ExP.{ColumnasDB.EstadosxPedido.IdPedido} = P.{ColumnasDB.Pedido.Id})" +
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
                 $"VALUES ({idpedido}, {nuevoestado} , '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}')");
                datos.executeNonQuery();
                datos.closeConnection();

            }

            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                datos.closeConnection();
            }


        }

     
        
        public void CambiarEstadoPedido(int idpedido, int nuevoestado, DateTime Fechahora)
        {
            AccesoDB datos = new AccesoDB();
            try
            {
                string formato = Fechahora.ToString("yyyy-MM-dd HH:mm:ss");

                datos.setQuery(
                 $"INSERT INTO {ColumnasDB.EstadosxPedido.DB} ({ColumnasDB.EstadosxPedido.IdPedido}, {ColumnasDB.EstadosxPedido.IdEstado}, {ColumnasDB.EstadosxPedido.FechaActualizacion}) " +
                 $"VALUES ({idpedido}, {nuevoestado} , CAST('{formato}' AS DATETIME))");
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
