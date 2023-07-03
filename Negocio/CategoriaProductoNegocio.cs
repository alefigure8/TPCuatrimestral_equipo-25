using Dominio;
using Opciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negocio
{
    public class CategoriaProductoNegocio
    {


        public List<CategoriaProducto> Listar()
        {
            List<CategoriaProducto> ListaCategorias = new List<CategoriaProducto>();
            AccesoDB datos = new AccesoDB();

            try
            {
                datos.setQuery($"SELECT {ColumnasDB.CategoriaProducto.Id}, {ColumnasDB.CategoriaProducto.Descripcion} FROM {ColumnasDB.CategoriaProducto.DB}");
                datos.executeReader();

                while (datos.Reader.Read())
                {
                    CategoriaProducto CPaux = new CategoriaProducto();
                    CPaux.Id = (Int32)datos.Reader[ColumnasDB.CategoriaProducto.Id];
                    if (datos.Reader[ColumnasDB.CategoriaProducto.Descripcion] != null)
                        CPaux.Descripcion = (string)datos.Reader[ColumnasDB.CategoriaProducto.Descripcion];
                    ListaCategorias.Add(CPaux);
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                datos.closeConnection();
            }

            return ListaCategorias;

        }

        public int AgregarCategoria(CategoriaProducto NuevaCategoria)
        {
            AccesoDB AccesoDB = new AccesoDB();
            try
            {
                AccesoDB.setQuery($"INSERT INTO " +
                    $"{ColumnasDB.CategoriaProducto.DB} (" +
                    $"{ColumnasDB.CategoriaProducto.Descripcion})"  +
                    $"VALUES (" +
                     $"'{NuevaCategoria.Descripcion}')");
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
