using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.PagePrestamos.Models
{
    public class PagoPrestamoModel
    {
        public int PagoId { get; set; }
        public string Titulo { get; set; }
        public string TipoPrestamo { get; set; }
        public DateTime Periodo { get; set; }
        public decimal Monto { get; set; }
    }
}