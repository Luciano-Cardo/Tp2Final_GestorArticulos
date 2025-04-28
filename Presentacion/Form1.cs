using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio; //Utilizando este comando el proyecto sobre el que estamos trabajando podra utilizar las clases contenidas en el proyecto dominio
using Negocio; //Utilizando este comando el proyecto sobre el que estamos trabajando podra utilizar las clases contenidas en el proyecto negocio




//En los comboBox el dropDownLista lo que hace es que solamente te permite seleccionar uno de los campos del desplegable, es decir no te deja escribir dentro de la caja de texto.
namespace Presentacion
{
    public partial class Form1: Form
    {
        private List<Articulo> listaArticulo; //listaArticulo es una lista de objetos Articulo que se va a usar para almacenar los artículos que se cargan
                                              //desde la base de datos.



        //Form1() es el constructor de la clase del formulario.
        public Form1()
        {
            InitializeComponent(); //InitializeComponent() es el método que inicializa todos los controles del formulario(botones,
                                   //cajas de texto, etc.), configurándolos correctamente.
        }


        //Form1_Load es el manejador del evento que se ejecuta cuando el formulario se carga.
        private void Form1_Load(object sender, EventArgs e)
        {
            cargar(); //Llama a la función que carga los artículos de la base de datos.
            cbCampo.Items.Add("Precio"); //Agrega opciones a un ComboBox (cbCampo) para que el usuario pueda elegir un campo de filtrado: "Precio", "Marca" o "Categoria".
            cbCampo.Items.Add("Marca");
            cbCampo.Items.Add("Categoria");
            dgvArticulo.CellFormatting += dgvArticulo_CellFormatting; //dgvArticulo.CellFormatting es un evento que se usa para personalizar cómo se formatean
                                                                      //las celdas del dgvArticulo , en este caso, para el precio.

        }


        
        private void cargar() //Es el método que obtiene la lista de artículos desde la base de datos.
        {
            ArticuloNegocio negocio = new ArticuloNegocio(); //Crea una instancia de la clase ArticuloNegocio.
            try
            {
                listaArticulo = negocio.listar(); //Llama al método listar() de ArticuloNegocio que obtiene la lista de artículos desde la base de datos.
                                                  //La lista obtenida se almacena en la variable listaArticulo.
                                                  //Esta lista contiene todos los artículos que luego se mostrarán en el formulario.

                dgvArticulo.DataSource = listaArticulo; //Asigna la lista de artículos (listaArticulo) como fuente de datos del DataGridView llamado dgvArticulo.
                                                        //Esto hace que los datos de la lista se muestren automáticamente en el DataGridView.

                dgvArticulo.Columns["url"].Visible = false; //Oculta la columna de la url de la imagen
                dgvArticulo.Columns["Id"].Visible = false;  //Oculta la columna del id del articulo


                pbArticulo.Load(listaArticulo[0].url); //Carga la imagen del primer artículo en el PictureBox llamado pbArticulo.
                                                       //Es un método del PictureBox que se usa para cargar una imagen en el control.
                                                       //Load() recibe como parámetro una cadena (string) que representa la URL o la ubicación del archivo de imagen
                                                       //que deseas mostrar en el PictureBox.
                                                       //listaArticulo[0]: Aquí estamos accediendo al primer elemento de la lista listaArticulo.
                                                       //Las listas en C# son de índice cero, por lo que listaArticulo[0] es el primer Articulo de la lista que ha sido
                                                       //cargada desde la base de datos
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); 
                //MessageBox es una clase estática en el espacio de nombres System.Windows.Forms que se utiliza para mostrar ventanas emergentes (diálogos) con
                //mensajes al usuario.
                //Show() es un método estático de la clase MessageBox.
                //En general muestra una ventana emergente (diálogo) que contiene una descripción completa de la excepción que ocurrió. Esta descripción está contenida
                //en el resultado de ex.ToString(), que incluye detalles como el tipo de error, el mensaje de la excepción y la pila de llamadas, lo que ayuda a
                //diagnosticar el problema que ocurrió en la aplicación.
            }


        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e) //Este evento lo que hace es que al hacer click en otro articulo cambia la imagen al que
                                                                              //estamos seleccionado
        {
            Articulo seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem; 
            cargarImagen(seleccionado.url); //Llama a la funcion cargarImagen pasandole por parametro la url que seleccionamos
        }

        private void cargarImagen(string imagen) //Intenta cargar una imagen en el PictureBox (pbArticulo).
                                                 //Si ocurre un error(por ejemplo, si la URL no es válida), muestra una imagen de "placeholder" en lugar de la original.
        {
            try
            {
                pbArticulo.Load(imagen);
            }
            catch (Exception ex)
            {

                pbArticulo.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");//Si la foto falla mostrara, lanzara una excepcion mostrando una imagen de placeholder
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            FmrAgregar agregar = new FmrAgregar();
            agregar.ShowDialog(); //Este lo que hace es que no nos permita salir de la aplicacion hasta no cerrar la ventana emergente.
            cargar();//Actualizo la carga asi no tengo que salir y volver a entrar para ver el nuevo articulo agregado.
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem; //CurrentRow es una propiedad de DataGridView que hace referencia a la fila actualmente seleccionada en el control
                                                                           //DataGridView.
                                                                           //CurrentRow devuelve un objeto de tipo DataGridViewRow, que representa la fila que el usuario ha
                                                                           //seleccionado en la tabla.
                                                                           //Este es un cast explícito, que convierte el objeto que se obtiene de DataBoundItem a un tipo específico.
                                                                           //En este caso, el objeto es de tipo object y se convierte a tipo Articulo, ya que sabemos que la lista
                                                                           //de datos que se está mostrando en el DataGridView contiene objetos de la clase Articulo.

            FmrAgregar modificar = new FmrAgregar(seleccionado); //Le paso por parametro el objeto articulo que quiero modificar 

            modificar.ShowDialog(); //Este lo que hace es que no nos permita salir de la aplicacion hasta no cerrar la ventana emergente.

            cargar();//Actualizo la carga asi no tengo que salir y volver a entrar para ver el nuevo articulo agregado.
        }

        private void btEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            Articulo seleccionado;//Como necesitamos el id del pokemon hacemos lo siguiente.

            try
            {
                DialogResult respuesta = MessageBox.Show("¿Estas seguro que quieres eliminar el articulo?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning); //El messagebox nos va a mostrar una ventana emergente con la pregunta asignada, y si la respuesta es si o no, la variable respuesta va a tomar el valor seleccionado.

                if (respuesta == DialogResult.Yes) //Si la opcion seleccionada es si, entonces se eliminar el articulo seleccionado, si es no, no se eliminara.
                {
                    seleccionado = (Articulo)dgvArticulo.CurrentRow.DataBoundItem; //Esto lo que hace es que seleccionado toma el valor del articulo sobre el que estamos seleccionado

                    negocio.Eliminar(seleccionado.id); //Este metodo eliminar recibe el id por parametro y lo elimina de la base de datos con el metodo eliminar 

                    cargar(); //El metodo cargar lo que hace es que al momento de eliminar, el articulo eliminado deja de verse en la lista
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); //Muestra la excepcion en pantalla
            }
        }

        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion = cbCampo.SelectedItem.ToString(); //Guarda el elemento seleccionado en la variable opcion.
            if (opcion == "Precio")
            {
                cbCriterio.Items.Clear(); // Si se selecciona la opcion de precio en el comboBox limpia todos los ítems(opciones) que actualmente están en el ComboBox
                cbCriterio.Items.Add("Mayor a "); //Lo que esta () son los criterios de busqueda
                cbCriterio.Items.Add("Menor a "); //Agrega la opción "Mayor a" al ComboBox cbCriterio.Esta será una de las opciones para que el usuario elija, en este
                                                  //caso, para filtrar artículos por precio.
                cbCriterio.Items.Add("Igual a ");
            }
            else
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Comienza con ");
                cbCriterio.Items.Add("Termina con ");
                cbCriterio.Items.Add("Contiene ");
            }
        }

        private bool validarFiltro()
        {
            if (cbCampo.SelectedIndex < 0) //Verifica si en el ComboBox de campos(cbCampo) no se seleccionó nada.
            {
                MessageBox.Show("Por favor, seleccione el campo para filtrar");
                return true;
            }

            if (cbCriterio.SelectedIndex < 0) //Verifica si en el ComboBox de criterios(cbCriterio) no se seleccionó nada.
            {
                MessageBox.Show("Por favor, seleccione el criterio para filtrar");
                return true;
            }
            if (cbCampo.SelectedItem.ToString() == "Precio") //Si el campo seleccionado es "Precio", hace validaciones especiales porque un precio debe ser numérico.
            {
                if (string.IsNullOrEmpty(txFiltro.Text)) //Verifica si el TextBox del filtro (txFiltro) está vacío. 
                                                         //Es un método estático de la clase String en C#.Sirve para verificar dos cosas a la vez:
                                                         //Si una cadena(string) es null , o si está vacía("")
                {
                    MessageBox.Show("Debes completar el campo con numeros"); //muestra el mensaje: "Debes completar el campo con números". si esta vacio
                    return true;
                }
                if((!esDecimal(txFiltro.Text))) //Usa el método esDecimal para validar si lo que escribió el usuario es un número decimal válido.
                                                //Si no es un número:
                {
                    MessageBox.Show("Solo se pueden numeros"); //muestra el mensaje: "Solo se pueden numeros". si esta vacio
                    return true;
                }
            }

            return false; //Si todo pasa correctamente (es decir, si no hay errores), entonces devuelve false, indicando que la validación fue exitosa.
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


        private void btBuscar_Click(object sender, EventArgs e)
        {

            ArticuloNegocio negocio = new ArticuloNegocio(); //Crea una nueva instancia de ArticuloNegocio
            try
            {
                if (validarFiltro()) //Si validarFiltro devuelve true, se sale del método (return) y no hace la búsqueda.
                    return;

                string campo = cbCampo.SelectedItem.ToString(); //Toma el elemento seleccionado (el campo sobre el que quieres buscar).
                                                                //SelectedItem:Es la propiedad que devuelve el elemento actualmente seleccionado en el ComboBox
                                                                //ToString():Convierte el SelectedItem a texto
                string criterio = cbCriterio.SelectedItem.ToString();
                string filtro = txFiltro.Text; //Toma el texto que el usuario escribió como filtro
                dgvArticulo.DataSource = negocio.filtrar(campo, criterio, filtro); // .DataSource: Es la propiedad donde defines de dónde va a sacar los datos el DataGridView
                                                                                   //negocio.filtrar: llama al metodo filtrar de la instancia negocio.
                                                                                   //El metodo recibe 3 parametros
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString()); //Muestra la excepcion en pantalla
            }

        }


        private void dgvArticulo_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvArticulo.Columns[e.ColumnIndex].Name == "precio" && e.Value != null) //Verifica que la columna que se está formateando se llama "precio" y que el
                                                                                        //valor de la celda no sea nulo.


            {
                if (decimal.TryParse(e.Value.ToString(), out decimal precio)) //Intenta convertir el valor de la celda a un número decimal.
                                                                              //Usa TryParse para evitar errores si el contenido no fuera numérico.
                {
                    e.Value = precio.ToString("N3", new System.Globalization.CultureInfo("es-AR")); //Formatea el numero al sistema argentino (con 3 decimales)
                    e.FormattingApplied = true; //Indica al DataGridView que ya aplicaste el formato manualmente.
                }
            }
        }

    }

}
