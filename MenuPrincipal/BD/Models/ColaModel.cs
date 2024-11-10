using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    internal class ColaModel
    {
        //Mandar a Llamar
        public int PrestamoId { get; set; }
        public int SolicitudId { get; set; }
        public string Titulo { get; set; }
        public string TiempoEspera { get; set; }
        public DateTime FechaSolicitud {  get; set; }
        public DateTime FechaDevolucion { get; set; }
        public string TipoPrestamo { get; set; }

        //Modificar
        public string EstadoPrestamo { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public string EstadoSolicitud { get; set; }
        public int StockActual {  get; set; }

    }
}
