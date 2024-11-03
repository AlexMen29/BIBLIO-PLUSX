using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class DetallesUsuarios
    {
        public int UsuarioID { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        //public string Direccion { get; set; }
        public string Correo1 { get; set; }
        public int Telefono1 { get; set; }
        public int Telefono2 { get; set; }
        public int TelefonoFijo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Carnet { get; set; }
        public string Estado { get; set; }
        public string TipoUsuario { get; set; }
        public string Carrera { get; set; }

        public DetallesUsuarios() { }
    }
}
