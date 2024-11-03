using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MenuPrincipal.BD.Models
{
    public class LibrosModel
    {
        public int LibrosID { get; set; }
        public string Titulo { get; set; }
        public byte[] ImageData { get; set; }
        public string Descripcion { get; set; }
        public string EstadoLibro { get; set; }
        public int AutorID { get; set; }
        public int EditorialID { get; set; }
        public int CategoriaID { get; set; }
        public int EdicionID { get; set; }


        public LibrosModel()
        { }
    }
}
