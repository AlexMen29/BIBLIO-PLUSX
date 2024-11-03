using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class PagosPrestamosModel
    {
        public int PagoID { get; set; }
        public decimal PrecioPrestamo { get; set; }
        public decimal ValorPagar {  get; set; }
        public string Estado {  get; set; }
        public DateTime FechaPago { get; set; }
        public int PrestamoID { get; set; }
        public int PenalizacionID { get; set; }
        public PagosPrestamosModel() { }
    }
}
