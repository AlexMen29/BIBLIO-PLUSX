using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class CrearDatoLibroModel
    {
        public string ISBN { get; set; }
        public string Descripcion { get; set; }
        public string Titulo { get; set; }
        public byte[] Imagen { get; set; } // VARBINARY se mapea como byte[] en C#
        public int AutorID { get; set; }
        public int EditorialID { get; set; }
        public int CategoriaID { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }

        public CrearDatoLibroModel() { }   




    }
}
