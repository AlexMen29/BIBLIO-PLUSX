using MenuPrincipal.BD.Models;
using MenuPrincipal.PagePrestamos.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.SqlClient;

namespace MenuPrincipal.PagePrestamos.service
{
    internal class DatoCola
    {
        public static List<ColaModel> MostrarDatosCola()
        {
            List<ColaModel> lstSolicitudes = new List<ColaModel>();
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "sp_MostrarDatosCola"; // Asegúrate de que este sea el nombre correcto

                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                ColaModel cola = new ColaModel
                                {
                                    PrestamoId = int.Parse(dr["PrestamoId"].ToString()),
                                    SolicitudId = int.Parse(dr["SolicitudId"].ToString()),
                                    Titulo = dr["Titulo"].ToString(),
                                    TiempoEspera = dr["TiempoEspera"].ToString(),
                                    FechaSolicitud = Convert.ToDateTime(dr["FechaSolicitud"].ToString()),
                                    FechaDevolucion = Convert.ToDateTime(dr["FechaDevolucion"].ToString()),
                                    TipoPrestamo = dr["TipoPrestamo"].ToString(),
                                    EstadoPrestamo = dr["EstadoPrestamo"].ToString(),
                                    StockActual = int.Parse(dr["StockActual"].ToString())
                                };
                                lstSolicitudes.Add(cola);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener los libros: " + e.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstSolicitudes;
        }
        public int ModificarEstadoFecha(ColaModel modificacion)
        {
            int resultado = 0; // Variable para almacenar el resultado de la ejecución

            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_ModificarEstadoFecha";

                        command.Parameters.AddWithValue("@PrestamoId", modificacion.PrestamoId);
                        command.Parameters.AddWithValue("@SolicitudId", modificacion.SolicitudId);
                        command.Parameters.AddWithValue("@EstadoPrestamo", modificacion.EstadoPrestamo);
                        command.Parameters.AddWithValue("@EstadoSolicitud", modificacion.EstadoSolicitud);
                        command.Parameters.AddWithValue("@FechaPrestamo", modificacion.FechaPrestamo);

                        resultado = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message, "Error de consulta", MessageBoxButton.OK, MessageBoxImage.Error);
                resultado = -1; // Retornar un valor específico en caso de error
            }

            return resultado; // Devolver el resultado de la ejecución
        }

    }
}
