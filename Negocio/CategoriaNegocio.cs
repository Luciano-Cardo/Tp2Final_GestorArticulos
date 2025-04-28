using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominio;

namespace Negocio
{
    public class CategoriaNegocio
    {
        public List<Categoria> listar()   //Trae un listado/lista de  objetos (categoria)
        {
            List<Categoria> lista = new List<Categoria>(); //Crea una lista vacía para guardar las categorias.
            AccesoDatos datos = new AccesoDatos();         ////Crea un objeto que maneja la conexión y lectura de la base de datos(Una clase de helper..)
            try
            {
                datos.setearConsulta("select Id, descripcion from categorias"); // Le dice a AccesoDatos qué consulta SQL tiene que ejecutar
                                                                                //(traer los Id y Descripcion de la tabla marcas).

                datos.ejecutarLectura(); //Ejecuta la consulta en la base de datos y abre un lector para leer los datos que trae.

                while (datos.Lector.Read()) //Cada vez que encuentra un registro en la tabla: Crea un objeto Categoria nuevo (aux).
                                            //Llena sus propiedades id y descripcion leyendo del resultado de la consulta.
                                            //El método .Read() avanza el lector a la siguiente fila y devuelve true si hay datos o false si ya no quedan más.
                {
                    Categoria aux = new Categoria(); //Crea un objeto categoria nuevo (aux).

                    aux.id = (int)datos.Lector["Id"]; // datos.Lector["Id"] → trae el valor de la columna Id de la fila en la que está parado
                                                      //(int) → fuerza (hace un cast) ese valor para que sea un número entero (int).
                                                      //aux.id = ... → guarda ese número en el objeto aux.

                    aux.descripcion = (string)datos.Lector["Descripcion"]; 

                    lista.Add(aux); // Agrega esa marca (aux) a la lista.

                }
                return lista; //Si todo salió bien, devuelve la lista de marcas.
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion(); //cierra la conexión a la base de datos para liberar recursos.
            }
        } 

    }
}
