using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class IngresoPrestamo
    {
        public int UsuarioId { get; set; }
        public int LibroId { get; set;}
        public DateTime FechaSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
        public string TiempoEspera { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string EstadoPrestamo { get; set; }
        public string TipoPrestamo { get; set; }
        public string TiempoEntrega { get; set; }
        public int Renovaciones {  get; set; }
        public DateTime FechaRenovacion { get; set; }
    }
}
