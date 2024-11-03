using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class CredencialesModel
    {
        public int CredencialID { get; set; }
        public BinaryData Clave { get; set; }
        public int UsuarioID { get; set; }
        public CredencialesModel() { }
    }
}
