using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class DetallesLibros
    {
        public int DetalleID { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editorial { get; set; }
        public string Categoria { get; set; }
        public string Edicion { get; set; }
        public int StockActual { get; set; }

        public DetallesLibros() { }
    }
}
