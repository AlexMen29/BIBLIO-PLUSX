using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.PagePrestamos.Models
{
    public class PrestamoModel
    {
        public int PrestamoId { get; set; }
        public string Titulo { get; set; }
        public string TipoPrestamo { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucion { get; set; }
    }
}