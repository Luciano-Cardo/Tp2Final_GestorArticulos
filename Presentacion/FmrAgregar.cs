using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO; // proporciona clases para trabajar con archivos y directorios. Permite leer y escribir en archivos, copiar archivos, moverlos, y otras operaciones
                 // relacionadas con el sistema de archivos.
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;
using System.Configuration; // es un espacio de nombres que contiene clases para acceder a la configuración de la aplicación, como las configuraciones en archivos
                            // app.config o web.config
                            //Se usa para leer valores de configuración de la aplicación en tiempo de ejecución, como rutas de archivos, cadenas de conexión, etc.

namespace Presentacion
{
    public partial class FmrAgregar: Form //Declaración de una clase llamada FmrAgregar que hereda de la clase Form
    {

        private Articulo articulo = null; //Declara un atributo privado llamado articulo que será de tipo Articulo.
                                          //Inicialmente, no tiene ningún valor, por lo que está configurado como null
                                          //Se inicializa en null ya que en el flujo del programa, estas variables se inicializarán correctamente cuando sea necesario.

        private OpenFileDialog archivo = null; //Declara un atributo privado llamado archivo de tipo OpenFileDialog.
                                               //Inicialmente, no tiene ningún valor, por lo que está configurado como null.
                                               //Se inicializa en null ya que en el flujo del programa, estas variables se inicializarán correctamente cuando sea necesario.
        public FmrAgregar()
        {
            InitializeComponent(); //configura la interfaz gráfica de usuario (GUI) del formulario, es decir, crea los controles (como botones, cuadros de texto, etc.).
        }

        public FmrAgregar(Articulo articulo)
        {
            InitializeComponent(); //configura la interfaz gráfica de usuario (GUI) del formulario, es decir, crea los controles (como botones, cuadros de texto, etc.),
            this.articulo = articulo; //Aquí se está asignando el valor del parámetro articulo (el objeto que se pasa al constructor) a una variable de instancia llamada articulo.
            Text = "Modificar"; //Aquí se está configurando el texto de la ventana. En este caso, el texto de la ventana será "Modificar".
        }


        private void btAceptar_Click(object sender, EventArgs e) //El metodo lo que hace es que al momento de darle al boton de aceptar, carga en la base de datos un nuevo articulo
        {
            ArticuloNegocio negocio = new ArticuloNegocio(); //Para poder llamar la funcion debo declarar una nueva instancia del objeto negocio.
            try
            {
                if (articulo == null) //se verifica que el objeto articulo sea null
                    articulo = new Articulo(); //Si es null se crea un nuevo objeto de tipo articulo.
                                               //Esto garantiza que el articulo siempre tenga un valor válido (un objeto de tipo Articulo) antes de intentar asignarle propiedades o enviarlo a la base de datos..
                if (!ValidarFormulario())
                    return;
                articulo.nombre = txNombre.Text; //La propiedad nombre del objeto articulo tomara el valor de la text box (txNombre).
                                                 //.Text obtiene lo que se escribe en la text Box
                articulo.descripcion = txDescripcion.Text; //El .Text a secas solamente sirve para los strings, si fuera un int deberia ser int.Parse(txDescripcion.Text).
                articulo.codigo = txCodigo.Text;
                articulo.url = txUrl.Text;
                decimal precio;
                if (decimal.TryParse(txPrecio.Text, NumberStyles.Any, new CultureInfo("es-ES"), out precio))
                {
                    articulo.precio = precio;
                }
                else
                {
                    return; // Salimos para que no guarde algo incorrecto
                }

                //El casting a (Categoria) y (Marca) es necesario porque el valor seleccionado en estos controles es de tipo object, pero sabemos que es un objeto de
                //tipo Categoria o Marca.
                articulo.tipo = (Categoria)cbTipo.SelectedItem;
                articulo.marca = (Marca)cbMarca.SelectedItem;

                if (articulo.id != 0) // Esto indica que el artículo ya existe en la base de datos (asumimos que los artículos existentes tienen un ID mayor que 0).
                {
                    negocio.modificar(articulo); //Llama al metodo modificar para modificar un articulo existente en la base de datos
                    MessageBox.Show("El Articulo se modifico correctamente");
                }
                else
                {
                    negocio.agregar(articulo); //Llama al metodo agregar para agregar un nuevo articulo a la base de datos
                    MessageBox.Show("El Articulo se agrego correctamente");
                }


                //Guardo imagen si la agrego localmente
                if (archivo != null && !(txUrl.Text.ToUpper().Contains("HTTP"))) //Si la variable archivo no es null y si la url no contiene http entonces entra en el if
                                                                                 //txUrl.Text.ToUpper(): convierte el texto de la URL en mayúsculas para hacer la comparación insensible a mayúsculas/minúsculas.
                                                                                 //Contains("HTTP"): verifica si la URL contiene la cadena "HTTP", lo que indica que es una URL de una imagen en línea.
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["carpeta-imagen"] + archivo.SafeFileName); //archivo.FileName: Obtiene la ruta completa del archivo seleccionado por el usuario. FileName devuelve la ruta completa al archivo en el sistema de archivos, como
                                                                                                                            //ConfigurationManager.AppSettings["carpeta-imagen"]: Aquí se obtiene el valor de una clave de configuración llamada "carpeta-imagen" que se encuentra en el archivo de configuración de la aplicación (app.config o web.config). Este valor representa la carpeta de destino donde se guardarán las imágenes.
                                                                                                                            //File.Copy:se utiliza para copiar un archivo desde una ubicación de origen a una ubicación de destino. 

                Close(); //Después de copiar la imagen, se cierra el formulario actual utilizando el método Close().
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); //Lanza una excepcion mostrando un mensaje con el error, en vez de romperse todo.
            }

        }

        private void btCancelar_Click(object sender, EventArgs e) //Cierra la ventana emergente al tocar el boton cancelar.
        {
            Close();
        }
        
        private void FmrAgregar_Load(object sender, EventArgs e) //Al cargar la ventana emergente carga los desplegables yendo a la base de datos y trayendolos
        {
            CategoriaNegocio categorianegocio = new CategoriaNegocio(); //Se crea una instacia nueva de CategoriaNegocio
            MarcaNegocio marcanegocio = new MarcaNegocio(); //Se crea una instacia nueva de MarcaNegocio
            try
            {
                cbTipo.DataSource = categorianegocio.listar(); //Trae de la base de datos los desplegables
                cbTipo.ValueMember = "id";
                cbTipo.DisplayMember = "descripcion";
                cbMarca.DataSource = marcanegocio.listar(); //Trae de la base de datos los desplegables
                cbMarca.ValueMember = "id";
                cbMarca.DisplayMember = "descripcion"; //DisplayMember para que el usuario vea la Descripcion de la categoría en lugar del Id.

                if (articulo != null) //Si el articulo es distinto de nulo, es decir que tiene datos precargados, entonces significa que es una modificacion
                                      //Si articulo es null, significa que probablemente estamos en un contexto de agregar un nuevo artículo y no es necesario cargar los valores de un artículo ya existente.
                {
                    txCodigo.Text = articulo.codigo; //Asigna el codigo del artículo a un cuadro de texto llamado txCodigo
                    txDescripcion.Text = articulo.descripcion; //Asigna la descripcion del artículo a un cuadro de texto llamado txDescripcion.
                    txNombre.Text = articulo.nombre; //Asigna el nombre del artículo al cuadro de texto llamado txNombre.
                    txPrecio.Text = articulo.precio.ToString(); //Asigna el precio del artículo (convertido a cadena con ToString()) al cuadro de texto llamado txPrecio.
                    txUrl.Text = articulo.url; //Asigna la url del artículo  al cuadro de texto llamado txUrl
                    cargarImagen(articulo.url); //Llama al método cargarImagen con la URL del artículo para mostrar la imagen correspondiente en un control de tipo PictureBox .
                    cbTipo.SelectedValue = articulo.tipo.id; //Establece el valor seleccionado en el ComboBox de categorías (cbTipo) a la id de la categoría del artículo
                                                             //El SelectedValue es una propiedad de los controles de tipo ComboBox. Esta propiedad permite obtener o establecer el valor seleccionado en un ComboBox  cuando está configurado para trabajar con un conjunto de datos.
                    cbMarca.SelectedValue = articulo.marca.id; //Establece el valor seleccionado en el ComboBox de marcas (cbMarca) a la id de la marca del artículo.
                                                               //El SelectedValue es una propiedad de los controles de tipo ComboBox. Esta propiedad permite obtener o establecer el valor seleccionado en un ComboBox  cuando está configurado para trabajar con un conjunto de datos.

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ToString());//Lanza una excepcion mostrando un mensaje con el error, en vez de romperse todo.
            }
        }

        private void FmrAgregar_Leave(object sender, EventArgs e) //Carga una imagen en un control.
        {
            cargarImagen(txUrl.Text); //txUrl.Text: Es el contenido del cuadro de texto txUrl, que debería ser una URL o una ruta local a una imagen que se quiere mostrar en el formulario.
        }



        private void cargarImagen(string imagen) //El parámetro imagen contiene la URL (o ruta local) de la imagen que se quiere cargar en el PictureBox.
        {
            try
            {
                pbAgregar.Load(imagen); //Utiliza el método Load del control PictureBox para intentar cargar la imagen desde la URL o ruta que se pasa como argumento en el parámetro imagen
                                        //.load es un evento de la picture box
            }
            catch (Exception ex)
            {

                pbAgregar.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");//Si la foto falla mostrara, lanzara una excepcion mostrando una imagen de placeholder
            }
        }


        
        private void btAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog(); //Permite generar una ventana de dialogo que se a abrir y me va a permitir abrir un archivo

            archivo.Filter = "jpg|*.jpg; |png|*.png"; //Esto hace que solamente permita archivos.jpg y .png

            if (archivo.ShowDialog() == DialogResult.OK) //Si el resultado es DialogResult.OK, significa que el usuario ha seleccionado un archivo y puede continuar con el proceso.
                                                         //archivo.ShowDialog() bloquea la ventana principal hasta que el usuario interactúe con la nueva ventana
                                                         //DialogResult.OK: Si el usuario selecciona un archivo y hace clic en "Aceptar", el resultado será DialogResult.OK, si el usuario cancela la selección, el resultado será DialogResult.Cancel
            {
                txUrl.Text = archivo.FileName; //Guarda la ruta completa de la url del archivo que estamos seleccionado
                cargarImagen(archivo.FileName); //Llama al metodo cargarImagen pasandole como parametro a ruta completa de la url para asi poder cargarla en la picture box

                File.Copy(archivo.FileName, ConfigurationManager.AppSettings["carpeta-imagen"] + archivo.SafeFileName); //la imagen seleccionada por el usuario se guarde en una carpeta específica en el sistema, definida previamente en la configuración de la aplicación. 
            }

            
        }

        private bool ValidarFormulario()
        {
            if (string.IsNullOrWhiteSpace(txNombre.Text))
            {
                MessageBox.Show("El campo Nombre es obligatorio.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txDescripcion.Text))
            {
                MessageBox.Show("El campo Descripción es obligatorio.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txCodigo.Text))
            {
                MessageBox.Show("El campo Código es obligatorio.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txPrecio.Text))
            {
                MessageBox.Show("El campo Precio es obligatorio.");
                return false;
            }
            if ((!esDecimal(txPrecio.Text))) //Usa el método esDecimal para validar si lo que escribió el usuario es un número decimal válido.
            {
                MessageBox.Show("En el campo precio solo se admiten numeros"); //muestra el mensaje: "En el campo precio solo se admiten numeros". si esta vacio
                return true;
            }

            if (cbTipo.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar un Tipo.");
                return false;
            }

            if (cbMarca.SelectedIndex < 0)
            {
                MessageBox.Show("Debe seleccionar una Marca.");
                return false;
            }

            return true;
        }


        private bool esDecimal(string texto)
        {
            decimal aux; //Declara una variable aux de tipo decimal
            return decimal.TryParse(texto, out aux); //Intenta convertir el texto a un valor decimal.
                                                     //Si el texto puede convertirse a decimal , devuelve true caso contrario false.
                                                     //El resultado de la conversión (si es exitosa) se guarda en la variable aux, aunque en esta función no lo uses más.
                                                     //Es un método seguro para intentar convertir un string a otro tipo
                                                     //Toma como string la variable texto y la convierte a decimal en la variable aux
        }
    }
}
