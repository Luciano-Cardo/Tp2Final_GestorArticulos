using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo //Debe ser public ya que tiene que poder ser una clase expuesta, debe ser vista de un proyecto a otro
    {
        public int id { get; set; } // public porque necesitan ser accesibles desde otros lugares del programa, especialmente desde otros proyectos o capas.
                                    // Si fueran private, solo la misma clase Articulo podría acceder a ellos, lo que limitaría mucho su utilidad.

        public string nombre { get; set; } // public porque necesitan ser accesibles desde otros lugares del programa, especialmente desde otros proyectos o capas.
                                           // Si fueran private, solo la misma clase Articulo podría acceder a ellos, lo que limitaría mucho su utilidad.

        public string descripcion { get; set; } // public porque necesitan ser accesibles desde otros lugares del programa, especialmente desde otros proyectos o capas.
                                                // Si fueran private, solo la misma clase Articulo podría acceder a ellos, lo que limitaría mucho su utilidad.

        public string codigo { get; set; } // public porque necesitan ser accesibles desde otros lugares del programa, especialmente desde otros proyectos o capas.
                                           // Si fueran private, solo la misma clase Articulo podría acceder a ellos, lo que limitaría mucho su utilidad.

        public string url { get; set; } // public porque necesitan ser accesibles desde otros lugares del programa, especialmente desde otros proyectos o capas.
                                        // Si fueran private, solo la misma clase Articulo podría acceder a ellos, lo que limitaría mucho su utilidad.

        public decimal precio { get; set; } // public porque necesitan ser accesibles desde otros lugares del programa, especialmente desde otros proyectos o capas.
                                            // Si fueran private, solo la misma clase Articulo podría acceder a ellos, lo que limitaría mucho su utilidad.

        public Categoria tipo { get; set; } //El tipo del articulo es un objeto de tipo cateogoria

        public Marca marca { get; set; } //El tipo del articulo es un objeto de tipo marca

    }
}
