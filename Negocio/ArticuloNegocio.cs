using System;
using System.Collections.Generic; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //Utilizamos una libreria para poder declarar determinados objetos
using Dominio; //Utilizando este comando el proyecto sobre el que estamos trabajando podra utilizar las clases contenidas en el proyecto dominio

namespace Negocio
{
    public class ArticuloNegocio //Debe ser public ya que tiene que poder ser una clase expuesta, debe ser vista de un proyecto a otro
    {
        //Esta clase establece una conexion a la base de datos y realza una lectura/trae los datos.
        //En esta clase creamos los metodos de acceso a datos para los articulos.
        
        public List<Articulo> listar() //Hacemos los metodos public para que la misma puede ser accedida desde el exterior
        {
            List<Articulo> lista = new List<Articulo>();

            SqlConnection conexion = new SqlConnection(); //Con esto nos conectamos a la base de datos

            SqlCommand comando = new SqlCommand(); //Con esto podemos realizar acciones

            SqlDataReader lector; //Guardamos el set de datos en el lector(variable)

            //Utilizamos un manejo excepciones para retorner una lista si todo esta ok o retorne error si hay algo mal.
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";//Configuramos la cadena de conexion, le aclaramos a donde se va a conectar  y a que base de datos se va a conectar y como nos vamos a conectar entre las comillas dobles.
                
                comando.CommandType = System.Data.CommandType.Text;//El comando sirve para realizar la accion, en este caso la lectura mandando la sentencia sql que quiero ejecutar
                
                comando.CommandText = "select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, C.Descripcion Categoria, M.Descripcion Marca, Precio, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.IdCategoria and M.Id = A.IdMarca"; //Esta consulta primero la hacemos en sql y luego la copiamos y pegamos para evitar erores, esta consulta la mandamos desde la apliacion a la base de datos

                comando.Connection = conexion; //Ejecuta el comando configurado en esta conexion

                conexion.Open(); //Abre la conexion

                lector = comando.ExecuteReader(); //Realiza la lectura

                while (lector.Read()) //se fija si hay una lectura, si da true se posiciona el puntero en la primer fila de la base de datos e ingresa en el while con el lector apuntado a la primer fila
                {
                    //En cada vuelta del while crea una nueva instancia del articulo y lo guarda en la lista
                    Articulo aux = new Articulo(); // Creamos un articulo auxiliar

                    //Cargamos los datos del registro 
                    aux.codigo = (string)lector["Codigo"];
                    aux.nombre = (string)lector["Nombre"];
                    aux.id = (int)lector["Id"];
                    aux.descripcion = (string)lector["Descripcion"];
                    if (!(lector["ImagenUrl"] is DBNull))
                        aux.url = (string)lector["ImagenUrl"];
                    aux.tipo = new Categoria(); //Esto se hace ya que sino cuando querramos tipo.descipcion nos va a dar referencia nula porque no existe un objeto
                                                //tipo categoria, por eso se hace el new antes
                    aux.tipo.id = (int)lector["IdCategoria"];
                    aux.tipo.descripcion = (string)lector["categoria"];
                    aux.marca = new Marca();
                    aux.marca.id = (int)lector["IdMarca"];
                    aux.marca.descripcion = (string)lector["Marca"];
                    aux.precio = (decimal)lector["Precio"];
                    

                    lista.Add(aux); //Agrega el articulo a la lista

                }
                conexion.Close(); //Cierra la conexion con la base de datos
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        

        public void agregar(Articulo nuevo)  //Llama al metodo AgregarModificar y agrega un articulo nuevo
        {
            AccesoDatos datos = new AccesoDatos(); //Para conectarme a la base de datos y agregar un articulo, necesito un objeto de la clase accesodatos.
                                                   //Crea una instancia de AccesoDatos
            try
            {
                //Realizamos un consulta de inserccion en SQL.
                datos.setearConsulta("Insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, Precio, ImagenUrl) values (@Codigo, @Nombre, @Descripcion, @IdMarca, @IdCategoria, @Precio, @ImagenUrl)"); //Setea la consulta
                datos.setearParametros("@Codigo", nuevo.codigo); //Carga uno a uno los valores nuevos para cada parámetro de la consulta.
                datos.setearParametros("@Nombre", nuevo.nombre);
                datos.setearParametros("@Descripcion", nuevo.descripcion);
                datos.setearParametros("@IdMarca", nuevo.marca.id);
                datos.setearParametros("@IdCategoria", nuevo.tipo.id);
                datos.setearParametros("@Precio", nuevo.precio);
                datos.setearParametros("@ImagenUrl", nuevo.url);

                datos.ejecutarAccion(); //Ejecuta el metodo que inserta un nuevo articulo a la base de datos.
                                        //Ejecuta la consulta en la base de datos.
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally //Siempre cierra la conexión a la base de datos, haya salido bien o mal.
            {
                datos.cerrarConexion();
            }
        }
        public void modificar(Articulo modificar)  //Llama al metodo AgregarModificar y modifica un articulo ya existente
        {
            AccesoDatos datos = new AccesoDatos(); //Para conectarme a la base de datos y agregar un articulo, necesito un objeto de la clase accesodatos.
                                                   //Crea una instancia de AccesoDatos
            try
            {
                //Realizamos un consulta de inserccion en SQL.
                datos.setearConsulta("update Articulos set codigo = @Codigo, Nombre = Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria,  Precio = @Precio, ImagenUrl = @ImagenUrl Where Id = @id"); //Setea la consulta
                datos.setearParametros("@Codigo", modificar.codigo); //Carga uno a uno los valores nuevos para cada parámetro de la consulta.
                datos.setearParametros("@Nombre", modificar.nombre);
                datos.setearParametros("@Descripcion", modificar.descripcion);
                datos.setearParametros("@IdMarca", modificar.marca.id);
                datos.setearParametros("@IdCategoria", modificar.tipo.id);
                datos.setearParametros("@Precio", modificar.precio);
                datos.setearParametros("@ImagenUrl", modificar.url);
                datos.setearParametros("@Id", modificar.id);

                datos.ejecutarAccion(); //Ejecuta el metodo que inserta un nuevo articulo a la base de datos.
                                        //Ejecuta la consulta en la base de datos.
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally //Siempre cierra la conexión a la base de datos, haya salido bien o mal.
            {
                datos.cerrarConexion();
            }
        }


        //Recibe un id por parametro y lo elimina de la lista
        public void Eliminar (int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos(); //Creamos una clase de acceso a datos

                
                datos.setearConsulta("delete from ARTICULOS where id = @id");//Le seteamos la consulta.NUNCA HAY QUE OLVIDARSE DEL WHERE CUANDO HACEMOS LA CONSULTA!!!!!!!!!!!!

                datos.setearParametros("@id",id); ////Prepara una consulta SQL

                datos.ejecutarAccion(); //Ejecuta la consulta
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {

            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {

                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, C.Descripcion Categoria, M.Descripcion Marca, Precio, A.IdMarca, A.IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M Where C.Id = A.IdCategoria and M.Id = A.IdMarca And ";
                
                if (campo == "Precio")
                {
                    filtro = filtro.Replace(",", ".");
                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio > " + filtro;
                            break;
                        case "Menor a":
                            consulta += "Precio < " + filtro;
                            break;
                        default:
                            consulta += "Precio = " + filtro;
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "M.Descripcion  like '" + filtro + "%' "; // k %, busca palabras que empiezen con k
                            break;
                        case "Termina con":
                            consulta += "M.Descripcion  like '%" + filtro + "'"; // % k, busca palabras que terminen con k
                            break;
                        default:
                            consulta += "M.Descripcion  like '%" + filtro + "%'"; // % k %, busca con lo que tenga en el medio entre los porcentajes 
                            break;
                    }
                }
                else 
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "C.Descripcion  like '" + filtro + "%' "; 
                            break;
                        case "Termina con":
                            consulta += "C.Descripcion  like '%" + filtro + "'";
                            break;
                        default:
                            consulta += "C.Descripcion  like '%" + filtro + "%'";
                            break;
                    }
                }


                datos.setearConsulta(consulta); //Setea la consulta
                datos.ejecutarLectura(); //Ejecuta la lectura

                while (datos.Lector.Read())   //.Lector: proviene de la clase AccesoDatos, es un objeto de tipo SqlDataReader que permite leer los datos obtenidos de una consulta SQL de la base de datos.
                {
                   
                    Articulo aux = new Articulo(); // Creamos una nueva instancia de articulo

                    aux.codigo = (string)datos.Lector["Codigo"];
                    aux.nombre = (string)datos.Lector["Nombre"];
                    aux.id = (int)datos.Lector["Id"];
                    aux.descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.url = (string)datos.Lector["ImagenUrl"];
                    aux.tipo = new Categoria();
                    aux.tipo.id = (int)datos.Lector["IdCategoria"];
                    aux.tipo.descripcion = (string)datos.Lector["categoria"];
                    aux.marca = new Marca();
                    aux.marca.id = (int)datos.Lector["IdMarca"];
                    aux.marca.descripcion = (string)datos.Lector["Marca"];
                    aux.precio = (decimal)datos.Lector["Precio"];


                    lista.Add(aux); 

                }

                return lista;
            }
            catch (Exception ex )
            {

                throw ex;
            }
        }
    }
}
