using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public  class ComprasModel
    {
        public int Cantidad { get; set; }
        public decimal CostoUnidad { get; set; }
        public DateTime FechaCompra {  get; set; }
        public decimal CostoTotal { get; set; }
        public int EdicionID {  get; set; }
        public int ProveedorID {  get; set; }
        public int DetallesID { get; set; }
        public ComprasModel() { }
    }
}
