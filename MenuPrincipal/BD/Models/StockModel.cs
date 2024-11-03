using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public  class StockModel
    {
        public int StockID { get; set; }
        public int StockMinimo { get; set; }
        public int StockMaximo { get; set; }
        public int StockActual { get; set; }
        public int EdicionID { get; set; }
        public StockModel() { }
    }
}
