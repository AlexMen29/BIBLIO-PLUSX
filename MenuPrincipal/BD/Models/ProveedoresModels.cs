using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class ProveedoresModels
    {
        public int ProveedorID { get; set; }
        public string NombreProveedor {  get; set; }
        public string DUIProveedor { get; set; }
        public string TelefonoProveedor { get; set; }
        public string DireccionProveedor { get; set; }
        public ProveedoresModels() { }
    }
}
