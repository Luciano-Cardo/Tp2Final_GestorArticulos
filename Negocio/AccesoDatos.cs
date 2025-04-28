using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.CodeDom.Compiler;
using System.Security.Cryptography;

namespace Negocio
{
    public class AccesoDatos //Le declaro los objetos que yo necesito para establecer una conexion.
                             //Esta clase sirve para manejar la conexión a la base de datos, hacer consultas y ejecutar acciones (insertar, actualizar, borrar, leer).
    {
        private SqlConnection conexion; //conexion: representa la conexión a tu base de datos (el "puente").
        private SqlCommand comando; //comando: representa el comando SQL que vas a ejecutar (SELECT, INSERT, UPDATE, DELETE).
        private SqlDataReader lector; //lector: sirve para leer los resultados de una consulta (SELECT).
        public SqlDataReader Lector //De esta manera tenemos la posibilidad de leer el lector desde el exterior, no de escribirlo, solo de leerlo.
                                    //Permite que otras clases puedan leer el lector, pero no modificarlo ,por ejemplo, en MarcaNegocio haciendo datos.Lector.Read()).
        {
            get { return lector; }
        }

        public AccesoDatos() //Creo una conexion mediante el uso de un constructor.El constructor esta sobrecargado
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true"); //conexion: Es el nombre de la variable que almacena la conexión a la base de datos. Es de tipo
                                                                                                                  //SqlConnection, que es una clase en ADO.NET que se utiliza para establecer una conexión con una base de datos SQL Server.
                                                                                                                  //new SqlConnection(...): Crea una nueva instancia del objeto SqlConnection. Esto es necesario para poder conectarse a la base de datos desde una aplicación .NET.

            comando = new SqlCommand(); //Esto nos permite realizar una accion contra la base de datos (consulta).
                                        //Prepara un objeto comando para cargarle consultas.
        }

        public void setearConsulta(string consulta) 
        {
            comando.CommandType = System.Data.CommandType.Text; //Configura el comando para que sea una consulta de texto(SELECT, INSERT, etc).
                                                                //comando: Es un objeto de tipo SqlCommand. Un objeto SqlCommand se utiliza para ejecutar comandos SQL en la base de datos.
                                                                //CommandType: Es una propiedad del objeto SqlCommand. Se utiliza para especificar el tipo de comando que se va a ejecutar.
                                                                //CommandType.Text se utiliza cuando se quiere ejecutar directamente una cadena de texto con una consulta SQL.
            comando.CommandText = consulta; //Guarda la consulta SQL que después vas a ejecutar.
        }

        public void ejecutarLectura() //Este metodo realiza la lectura y lo guarda en el lector. Ejecuta una consulta SELECT.
        {
            comando.Connection = conexion; // Se le asigna la conexión a la base de datos.
                                           //Conexion es un objeto de tipo SqlConnection que se usa para interactuar con la base de datos.
                                           //Comando es un objeto de tipo SqlCommand que representa la consulta que se va a ejecutar.
            try
            {
                conexion.Open(); // Este método abre la conexión a la base de datos
                lector = comando.ExecuteReader(); //Guarda el resultado en lector para que puedas leer los datos fila por fila después.
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void ejecutarAccion() //Este metodo lo que hace es ejecutar un insert, que es un executeNonQuery.
        {
            comando.Connection = conexion; //Se le asigna la conexión a la base de datos.
                                           //Conexion es un objeto de tipo SqlConnection que se usa para interactuar con la base de datos.
                                           //Comando es un objeto de tipo SqlCommand que representa la consulta que se va a ejecutar.

            try
            {
                conexion.Open(); //Este método abre la conexión a la base de datos
                comando.ExecuteNonQuery(); //Este método ejecuta el comando SQL contra la base de datos, pero no devuelve datos.
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void cerrarConexion() //Cierra la conexion a la base de datos y ademas cierra el lector si está abierto (muy importante para no bloquear la base).
        {
            if (lector != null) //Si el lector es distinto de nulo lo ciera
                lector.Close();
            conexion.Close(); //Cierra la conexion con la base de datos
        }


        
        public void setearParametros(string nombre, object valor) //Agrega un parámetro a una consulta SQL y asigna un valor, de manera que la consulta SQL pueda usar ese valor sin problemas y de forma segura.
        {
            comando.Parameters.AddWithValue(nombre, valor); //comando: Es una instancia del objeto SqlCommand, que es una clase en ADO.NET utilizada para ejecutar comandos SQL (como consultas o instrucciones de actualización). En este caso, comando representa la consulta SQL que se ejecutará en la base de datos.
                                                            //Parameters es una colección que almacena los parámetros asociados con el comando SQL.
                                                            //AddWithValue(nombre, valor): Este método agrega un parámetro con el nombre y valor proporcionados a la colección de parámetros del comando SQL.
                                                            //nombre: El nombre del parámetro en la consulta SQL. Por ejemplo, si en la consulta tienes un WHERE id = @id, entonces nombre será "@id".
                                                            //valor: El valor que se asignará al parámetro.Este valor se utilizará en lugar del parámetro en la consulta SQL.

        }

    }
}
