using MenuPrincipal.PagePrestamos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MenuPrincipal.PagePrestamos.service
{
    public class DatoPrestamo
    {
        public static List<PrestamoModel> CargarClasificacionPrestamos()
        {
            List<PrestamoModel> listaPrestamos = new List<PrestamoModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARCLASIFICACIONPRESTAMOS";

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var prestamo = new PrestamoModel
                                {
                                    PrestamoId = reader.GetInt32(0),
                                    Titulo = reader.GetString(1),
                                    TipoPrestamo = reader.GetString(2),
                                    FechaPrestamo = reader.GetDateTime(3),
                                    FechaDevolucion = reader.GetDateTime(4)
                                };
                                listaPrestamos.Add(prestamo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los préstamos: " + ex.Message);
            }

            return listaPrestamos;
        }
    }
}