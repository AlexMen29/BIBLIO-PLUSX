using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPrincipal.BD.Models
{
    public class SolicitudesModel
    {
        public int SolicitudID { get; set; }
        public DateTime FechaSolicitud {  get; set; }
        public string EstadoSolicitud { get; set; }
        public int TiempoEspera {  get; set; }
        public int UsuarioID { get; set; }
        public int LibroID { get; set; }
        public SolicitudesModel() { }
    }
}
