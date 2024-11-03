using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class PrestamosModel
    {
        public int PrestamoID { get; set; }
        public DateTime FechaPrestamo {  get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string EstadoPrestamo { get; set; }
        public string TipoPrestamo { get; set; }
        public int TiempoEntrega { get; set; }
        public int Renovaciones { get; set; }
        public DateTime FechaRenovacion { get; set; }
        public int SolicitudID { get; set; }
        public PrestamosModel() { }
    }
}
