using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class ProductoNegocio
    {
        public List<ProductoDelDia> ListarProductosDisponibles()
        {
            AccesoDB AccesoDB = new AccesoDB();
            List<ProductoDelDia> ListaProductoDelDia = new List<ProductoDelDia>();

            try
            {
                //AccesoDB.setQuery("P.Nombre AS 'Nombre', P.Descripcion AS 'Descripcion', P.Valor AS 'Valor'," 
                //    + "P.AptoVegano AS 'AptoVegano', P.AptoCeliaco AS 'AptoCeliaco', P.Alcohol AS 'Alcohol',"
                //    + "P.Stock AS 'Stock', P.Activo AS 'Activo', P.TiempoCoccion AS 'TiempoCoccion'"
                //    + "FROM PRODUCTOSPORDIA_MENU M INNER JOIN PRODUCTOS P ON M.IdProducto = P.IdProducto"
                //    + "INNER JOIN CATEGORIAPRODUCTO C  ON P.CategoriaProducto = C.IdCategoriaProducto");

                AccesoDB.setQuery("SELECT IdProducto, Nombre, Descripcion, Valor FROM PRODUCTOS GO");
                AccesoDB.executeReader();
                while(AccesoDB.Reader.Read())
                {
                    ProductoDelDia PDDAux = new ProductoDelDia();
                    PDDAux.Id = (int)AccesoDB.Reader["IdProducto"];
                    //PDDAux.Fecha = (DateTime)AccesoDB.Reader["Fecha"];
                    //PDDAux.StockInicio = (int)AccesoDB.Reader["M.StockInicial"];
                    //PDDAux.StockCierre = (int)AccesoDB.Reader["M.StockCierre"];
                    
                    //CategoriaProducto CPaux = new CategoriaProducto();
                    //CPaux.Id = (int)AccesoDB.Reader["CategoriaProducto"];
                    //CPaux.Descripcion = (String)AccesoDB.Reader["C.Descripcion"];
                    //PDDAux.Categoria = CPaux;

                    PDDAux.Nombre = (String)AccesoDB.Reader["Nombre"];
                    PDDAux.Descripcion = (string)AccesoDB.Reader["Descripcion"];
                    PDDAux.Valor = (Decimal)AccesoDB.Reader["Valor"];
                    //PDDAux.AptoVegano = (bool)AccesoDB.Reader["P.AptoVegano"];
                    //PDDAux.AptoCeliaco = (bool)AccesoDB.Reader["P.AptoCeliaco"];
                    //PDDAux.Alcohol = (bool)AccesoDB.Reader["P.Alcohol"];
                    //PDDAux.Stock = (int)AccesoDB.Reader["P.Stock"];
                    //PDDAux.Activo = (bool)AccesoDB.Reader["P.Activo"];
                    //PDDAux.TiempoCoccion = (DateTime)AccesoDB.Reader["P.TiempoCoccion"];

                    ListaProductoDelDia.Add(PDDAux);



                }

                return ListaProductoDelDia;
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
