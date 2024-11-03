using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.PageUsuarios.Models
{
    public class UsuariosModel
    {
        public int UsuarioId { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }

        //Constructor vacio
        public UsuariosModel() { }
    }
}