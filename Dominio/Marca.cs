using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Marca //Debe ser public ya que tiene que poder ser una clase expuesta, debe ser vista de un proyecto a otro
    {
        public int id { get; set; } // public porque necesitan ser accesibles desde otros lugares del programa, especialmente desde otros proyectos o capas.
                                    // Si fueran private, solo la misma clase Articulo podría acceder a ellos, lo que limitaría mucho su utilidad.

        public string descripcion { get; set; } // public porque necesitan ser accesibles desde otros lugares del programa, especialmente desde otros proyectos o capas.
                                                // Si fueran private, solo la misma clase Articulo podría acceder a ellos, lo que limitaría mucho su utilidad.


        //Error de marca.dominio //Este metodo lo que hace es sobreescribir el metodo toString para que en el form me muestra en la columna la marca del articulo
        public override string ToString() //El método ToString() convierte un objeto en un string (texto).
        {
            return descripcion;

        }

    }
}
