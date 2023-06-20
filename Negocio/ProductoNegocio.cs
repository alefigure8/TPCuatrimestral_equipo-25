using Dominio;
using Opciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Opciones.ColumnasDB;

namespace Negocio
{
    public class ProductoNegocio
    {
        public List<Dominio.Producto> ListarProductos()
        {
            AccesoDB AccesoDB = new AccesoDB();
            List<Dominio.Producto> ListaProductos = new List<Dominio.Producto>();

            try
            {
                AccesoDB.setQuery($"SELECT {ColumnasDB.Producto.Id},  {ColumnasDB.Producto.Categoria}, {ColumnasDB.Producto.Nombre},"
               + $"{ColumnasDB.Producto.Descripcion}, {ColumnasDB.Producto.Valor}, {ColumnasDB.Producto.AptoVegano},"
               + $"{ColumnasDB.Producto.AptoCeliaco}, {ColumnasDB.Producto.Alcohol}, {ColumnasDB.Producto.Stock},"
               + $"{ColumnasDB.Producto.Activo}, {ColumnasDB.Producto.TiempoCoccion} FROM {ColumnasDB.Producto.DB}");
                AccesoDB.executeReader();
                while (AccesoDB.Reader.Read())
                {
                    Dominio.Producto PAux = new Dominio.Producto();
                    PAux.Id = (Int32)AccesoDB.Reader[ColumnasDB.Producto.Id];
                    PAux.Categoria = (Int32)AccesoDB.Reader[ColumnasDB.Producto.Categoria];
                    PAux.Nombre = (string)AccesoDB.Reader[ColumnasDB.Producto.Nombre];
                    PAux.Descripcion = (string)AccesoDB.Reader[ColumnasDB.Producto.Descripcion];
                    PAux.Valor = (Decimal)AccesoDB.Reader[ColumnasDB.Producto.Valor];
                    PAux.AptoVegano = (bool)AccesoDB.Reader[ColumnasDB.Producto.AptoVegano];
                    PAux.AptoCeliaco = (bool)AccesoDB.Reader[ColumnasDB.Producto.AptoCeliaco];
                    PAux.Alcohol = (bool)AccesoDB.Reader[ColumnasDB.Producto.Alcohol];
                    PAux.Stock = (int)AccesoDB.Reader[ColumnasDB.Producto.Stock];
                    PAux.Alcohol = (bool)AccesoDB.Reader[ColumnasDB.Producto.Activo];
                    PAux.TiempoCoccion = (TimeSpan)AccesoDB.Reader[ColumnasDB.Producto.TiempoCoccion];
                    ListaProductos.Add(PAux);
                }

                return ListaProductos;
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

        public int NuevoProducto(Dominio.Producto NuevoProducto)
        {
            AccesoDB AccesoDB = new AccesoDB();
            try
            {
                AccesoDB.setQuery($"INSERT INTO " +
                    $"{ColumnasDB.Producto.DB} (" +
                    $"{ColumnasDB.Producto.Categoria}," +
                    $"{ColumnasDB.Producto.Nombre}," +
                    $"{ColumnasDB.Producto.Descripcion}, " +
                    $"{ColumnasDB.Producto.Valor}, " +
                    $"{ColumnasDB.Producto.AptoVegano}, " +
                    $"{ColumnasDB.Producto.AptoCeliaco}, " +
                    $"{ColumnasDB.Producto.Alcohol}," +
                    $"{ColumnasDB.Producto.Stock}," +
                    $"{ColumnasDB.Producto.Activo}," +
                    $"{ColumnasDB.Producto.TiempoCoccion})" +
                    $"VALUES (" +
                    $"'{NuevoProducto.Categoria}'," +
                    $"'{NuevoProducto.Nombre}'," +
                    $"'{NuevoProducto.Descripcion}'," +
                    $"{NuevoProducto.Valor}," +
                    $"'{NuevoProducto.AptoVegano}'," +
                    $"'{NuevoProducto.AptoCeliaco}'," +
                    $"'{NuevoProducto.Alcohol}'," +
                    $"{NuevoProducto.Stock}," +
                    $"'{NuevoProducto.Activo}'," +
                    $"'{NuevoProducto.TiempoCoccion}')");
                return AccesoDB.executeScalar();
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
    }
}
