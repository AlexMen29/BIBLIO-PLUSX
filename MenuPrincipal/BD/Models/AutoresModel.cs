using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class AutoresModel
    {
        public int AutorID { get; set; }
        public string NombreAutor { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public string Bibliografia { get; set; }

        public AutoresModel() { }
    }
}
