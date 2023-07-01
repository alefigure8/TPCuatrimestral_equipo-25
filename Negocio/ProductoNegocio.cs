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
        // PRODUCTOS BASE
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
                    PAux.Activo = (bool)AccesoDB.Reader[ColumnasDB.Producto.Activo];
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

        public bool EliminarProducto(int Id)
        {
            AccesoDB AccesoDB = new AccesoDB();
            try
            {
                AccesoDB.setQuery("DELETE FROM PRODUCTOS WHERE IdProducto = " + Id);
                //AccesoDB.setQuery($"DELETE " +
                //    $"{ColumnasDB.Producto.DB}" +
                //    $"Where" +
                //    $"{ColumnasDB.Producto.Id} =" + Id);
                //AccesoDB.setParameter("@id", Id);
                if (AccesoDB.executeNonQuery())
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                AccesoDB.closeConnection();
            }
            return false;
        }

        public int ModificarProducto(Dominio.Producto ProdAModificar)
        {
            AccesoDB AccesoDB = new AccesoDB();
            try
            {
                //AccesoDB.setQuery("UPDATE " +
                //    $"{ColumnasDB.Producto.DB}" +
                //    "SET " +
                //    $"{ColumnasDB.Producto.Categoria} = " + ProdAModificar.Categoria + ", " +
                //    $"{ColumnasDB.Producto.Nombre} = '" + ProdAModificar.Nombre + "', " +
                //    $"{ColumnasDB.Producto.Descripcion} = '" + ProdAModificar.Descripcion + "', " +
                //    $"{ColumnasDB.Producto.Valor} = " + ProdAModificar.Valor + "," +
                //    $"{ColumnasDB.Producto.AptoVegano} = '" + ProdAModificar.AptoVegano + "', " +
                //    $"{ColumnasDB.Producto.AptoCeliaco} = '" + ProdAModificar.AptoCeliaco + "', " +
                //    $"{ColumnasDB.Producto.Alcohol} = '" + ProdAModificar.Alcohol + "', " +
                //    $"{ColumnasDB.Producto.Stock} = " + ProdAModificar.Stock + "," +
                //    $"{ColumnasDB.Producto.Activo} = '" + ProdAModificar.Activo + "', " +
                //    $"{ColumnasDB.Producto.TiempoCoccion} = '" + ProdAModificar.TiempoCoccion + "' " +
                //    "WHERE" + $"{ColumnasDB.Producto.Id} = " + ProdAModificar.Id );

                AccesoDB.setQuery("UPDATE PRODUCTOS SET CategoriaProducto = " + ProdAModificar.Categoria + ", Nombre = '" +
                    ProdAModificar.Nombre + "', Descripcion = '" + ProdAModificar.Descripcion + "', Valor = " + ProdAModificar.Valor +
                    ", AptoVegano = '" + ProdAModificar.AptoVegano + "', AptoCeliaco = '" + ProdAModificar.AptoCeliaco + "', Alcohol = '" +
                    ProdAModificar.Alcohol + "', Stock = " + ProdAModificar.Stock + ", Activo = '" + ProdAModificar.Activo + "', TiempoCoccion = '" +
                    ProdAModificar.TiempoCoccion + "' WHERE IdProducto = " + ProdAModificar.Id);
                   
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


        // PRODUCTOS DEL DÍA

        public List<ProductoDelDia> ListarProductosDelDia()
        {
            AccesoDB AccesoDB = new AccesoDB();
            List<ProductoDelDia> ListaProductosDelDia = new List<ProductoDelDia>();

            try
            {
                AccesoDB.setQuery($"SELECT {ColumnasDB.Producto.Id},  {ColumnasDB.Producto.Categoria}, {ColumnasDB.Producto.Nombre},"
               + $"{ColumnasDB.Producto.Descripcion}, {ColumnasDB.Producto.Valor}, {ColumnasDB.Producto.AptoVegano},"
               + $"{ColumnasDB.Producto.AptoCeliaco}, {ColumnasDB.Producto.Alcohol}, {ColumnasDB.Producto.Stock},"
               + $"{ColumnasDB.Producto.Activo}, {ColumnasDB.Producto.TiempoCoccion}, {ColumnasDB.ProductoDD.Fecha}, {ColumnasDB.ProductoDD.StockInicial}, {ColumnasDB.ProductoDD.StockCierre}  " 
               + $" FROM {ColumnasDB.ProductoDD.DB} WHERE {ColumnasDB.ProductoDD.Fecha} =  TRY_CONVERT(DATE,GETDATE())");
                AccesoDB.executeReader();
                while (AccesoDB.Reader.Read())
                {
                    ProductoDelDia PDDAux = new ProductoDelDia();
                    PDDAux.Id = (Int32)AccesoDB.Reader[ColumnasDB.Producto.Id];
                    PDDAux.Categoria = (Int32)AccesoDB.Reader[ColumnasDB.Producto.Categoria];
                    PDDAux.Nombre = (string)AccesoDB.Reader[ColumnasDB.Producto.Nombre];
                    PDDAux.Descripcion = (string)AccesoDB.Reader[ColumnasDB.Producto.Descripcion];
                    PDDAux.Valor = (Decimal)AccesoDB.Reader[ColumnasDB.Producto.Valor];
                    PDDAux.AptoVegano = (bool)AccesoDB.Reader[ColumnasDB.Producto.AptoVegano];
                    PDDAux.AptoCeliaco = (bool)AccesoDB.Reader[ColumnasDB.Producto.AptoCeliaco];
                    PDDAux.Alcohol = (bool)AccesoDB.Reader[ColumnasDB.Producto.Alcohol];
                    PDDAux.Stock = (int)AccesoDB.Reader[ColumnasDB.Producto.Stock];
                    PDDAux.Activo = (bool)AccesoDB.Reader[ColumnasDB.Producto.Activo];
                    PDDAux.TiempoCoccion = (TimeSpan)AccesoDB.Reader[ColumnasDB.Producto.TiempoCoccion];
                    PDDAux.Fecha = (DateTime)AccesoDB.Reader[ColumnasDB.ProductoDD.Fecha];
                    PDDAux.StockInicio = AccesoDB.Reader[ColumnasDB.ProductoDD.StockInicial] is DBNull ? 0 : (int)AccesoDB.Reader[ColumnasDB.ProductoDD.StockInicial];
                    PDDAux.StockCierre = AccesoDB.Reader[ColumnasDB.ProductoDD.StockCierre] is DBNull ? 0 : (int)AccesoDB.Reader[ColumnasDB.ProductoDD.StockCierre];

                    ListaProductosDelDia.Add(PDDAux);
                }

                return ListaProductosDelDia;
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

        public int AgregarProductoDD(ProductoDelDia ProductoDelDia)
        {
            AccesoDB AccesoDB = new AccesoDB();
            try
            {
                AccesoDB.setQuery($"INSERT INTO " +
                    $"{ColumnasDB.ProductoDD.DB} (" +
                    $"{ColumnasDB.Producto.Id}," +
                    $"{ColumnasDB.Producto.Categoria}," +
                    $"{ColumnasDB.Producto.Nombre}," +
                    $"{ColumnasDB.Producto.Descripcion}, " +
                    $"{ColumnasDB.Producto.Valor}, " +
                    $"{ColumnasDB.Producto.AptoVegano}, " +
                    $"{ColumnasDB.Producto.AptoCeliaco}, " +
                    $"{ColumnasDB.Producto.Alcohol}," +
                    $"{ColumnasDB.Producto.Stock}," +
                    $"{ColumnasDB.Producto.Activo}," +
                    $"{ColumnasDB.Producto.TiempoCoccion}, " +
                    $"{ColumnasDB.ProductoDD.Fecha}, " +
                    $"{ColumnasDB.ProductoDD.StockInicial}, " +
                    $"{ColumnasDB.ProductoDD.StockCierre})" +
                    $"VALUES (" +
                     $"{ProductoDelDia.Id}," +
                    $"'{ProductoDelDia.Categoria}'," +
                    $"'{ProductoDelDia.Nombre}'," +
                    $"'{ProductoDelDia.Descripcion}'," +
                    //$" {ProductoDelDia.Valor.ToString().Replace(",", ".")}," +
                    $" {ProductoDelDia.Valor}," +
					$"'{ProductoDelDia.AptoVegano}'," +
                    $"'{ProductoDelDia.AptoCeliaco}'," +
                    $"'{ProductoDelDia.Alcohol}'," +
                    $" {ProductoDelDia.Stock}," +
                    $"'{ProductoDelDia.Activo}'," +
                    $"'{ProductoDelDia.TiempoCoccion}'," +
                    //$"'{ProductoDelDia.Fecha.ToString("yyyy-MM-dd")}'," +
                    $"'{ProductoDelDia.Fecha}'," +
					$"{ProductoDelDia.StockInicio}," +
                    $"{ProductoDelDia.StockCierre})");
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

        public int ModificarProductoDD(ProductoDelDia ProdAModificar)
        {
            AccesoDB AccesoDB = new AccesoDB();
            try
            {
                //AccesoDB.setQuery("UPDATE " +
                //    $"{ColumnasDB.ProductoDD.DB}" +
                //    " SET " +
                //    $"{ColumnasDB.Producto.Categoria} = " + ProdAModificar.Categoria + ", " +
                //    $"{ColumnasDB.Producto.Nombre} = '" + ProdAModificar.Nombre + "', " +
                //    $"{ColumnasDB.Producto.Descripcion} = '" + ProdAModificar.Descripcion + "', " +
                //    $"{ColumnasDB.Producto.Valor} = " + ProdAModificar.Valor + "," +
                //    $"{ColumnasDB.Producto.AptoVegano} = '" + ProdAModificar.AptoVegano + "', " +
                //    $"{ColumnasDB.Producto.AptoCeliaco} = '" + ProdAModificar.AptoCeliaco + "', " +
                //    $"{ColumnasDB.Producto.Alcohol} = '" + ProdAModificar.Alcohol + "', " +
                //    $"{ColumnasDB.Producto.Stock} = " + ProdAModificar.Stock + "," +
                //    $"{ColumnasDB.Producto.Activo} = '" + ProdAModificar.Activo + "', " +
                //    $"{ColumnasDB.Producto.TiempoCoccion} = '" + ProdAModificar.TiempoCoccion + "' " +
                //    $"{ColumnasDB.ProductoDD.Fecha} = " + ProdAModificar.Fecha + ", " +
                //    $"{ColumnasDB.ProductoDD.StockInicial} = " + ProdAModificar.StockInicio + ", " +
                //    $"{ColumnasDB.ProductoDD.StockCierre} = " + ProdAModificar.StockCierre +
                //    " WHERE " + $"{ColumnasDB.Producto.Id} = " + ProdAModificar.Id + 
                //    " AND " + $"{ColumnasDB.ProductoDD.Fecha} = '" + ProdAModificar.Fecha + "' ");

                AccesoDB.setQuery("UPDATE ProductosPorDia_Menu SET CategoriaProducto = " + ProdAModificar.Categoria + ", Nombre = '" +
                    ProdAModificar.Nombre + "', Descripcion = '" + ProdAModificar.Descripcion + "', Valor = " + ProdAModificar.Valor +
                    ", AptoVegano = '" + ProdAModificar.AptoVegano + "', AptoCeliaco = '" + ProdAModificar.AptoCeliaco + "', Alcohol = '" +
                    ProdAModificar.Alcohol + "', Stock = " + ProdAModificar.Stock + ", Activo = '" + ProdAModificar.Activo + "', TiempoCoccion = '" +
                    ProdAModificar.TiempoCoccion + "', Fecha = '" + ProdAModificar.Fecha + "', StockInicial = " + ProdAModificar.StockInicio +
                    ", StockCierre = " + ProdAModificar.StockCierre +" WHERE IdProducto = " + ProdAModificar.Id + " AND " + "Fecha = '" + ProdAModificar.Fecha + "'");

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
