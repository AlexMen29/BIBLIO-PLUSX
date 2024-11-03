using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class EditorialesModel
    {
        public int EditorialID { get; set; }
        public string NombreEditorial { get; set; }
        public string DireccionEditorial { get; set; }
        public string TelefonoEditorial { get; set; }

        public EditorialesModel() { }
    }
}
