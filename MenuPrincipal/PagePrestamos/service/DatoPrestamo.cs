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
                            PrestamoId = reader["ID"] != DBNull.Value && int.TryParse(reader["ID"].ToString(), out int prestamoId) ? prestamoId : 0,
                            Titulo = reader["Titulo"]?.ToString() ?? "N/A",
                            TipoPrestamo = reader["TipoPrestamo"]?.ToString() ?? "N/A",
                            FechaPrestamo = DateTime.TryParse(reader["FechaPrestamo"]?.ToString(), out DateTime fechaPrestamo) ? fechaPrestamo : DateTime.MinValue,
                            FechaDevolucion = DateTime.TryParse(reader["FechaDevolucion"]?.ToString(), out DateTime fechaDevolucion) ? fechaDevolucion : DateTime.MinValue,
                            EstadoPrestamo = reader["EstadoPrestamo"]?.ToString() ?? "N/A",
                            TiempoEntrega = reader["TiempoEntrega"] != DBNull.Value && int.TryParse(reader["TiempoEntrega"].ToString(), out int tiempoEntrega) ? tiempoEntrega : 0,
                            Renovaciones = reader["Renovaciones"] != DBNull.Value && int.TryParse(reader["Renovaciones"].ToString(), out int renovaciones) ? renovaciones : 0,
                            FechaRenovacion = reader["FechaRenovacion"] != DBNull.Value && DateTime.TryParse(reader["FechaRenovacion"].ToString(), out DateTime fechaRenovacion) ? (DateTime?)fechaRenovacion : null
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