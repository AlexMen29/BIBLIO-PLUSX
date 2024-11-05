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
                                    //Titulo = dr["Titulo"].ToString(),
                                    PrestamoId = Convert.ToInt32(reader["ID"].ToString()),
                                    Titulo = reader["Titulo"].ToString(),
                                    TipoPrestamo = reader["TipoPrestamo"].ToString(),
                                    FechaPrestamo = Convert.ToDateTime(reader["FechaPrestamo"].ToString()),
                                    FechaDevolucion = Convert.ToDateTime(reader["FechaDevolucion"].ToString()),
                                    EstadoPrestamo = reader["EstadoPrestamo"].ToString(),
                                    TiempoEntrega = Convert.ToInt32(reader["TiempoEntrega"].ToString()),
                                    Renovaciones = Convert.ToInt32(reader["Renovaciones"].ToString()),
                                    FechaRenovacion = Convert.ToDateTime(reader["FechaRenovacion"].ToString())
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