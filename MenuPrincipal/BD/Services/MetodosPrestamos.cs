using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using MenuPrincipal.BD.Models;

namespace MenuPrincipal.BD.Services
{
    public class MetodosPrestamos
    {
        public int RegistrarPrestamoCompleto(IngresoPrestamo prestamo)
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
                        command.CommandText = "IngresoPrestamoProcedure";

                        // Agregar parámetros al comando usando el objeto de tipo IngresoPrestamoModel
                        command.Parameters.AddWithValue("@UsuarioID", prestamo.UsuarioId);
                        command.Parameters.AddWithValue("@LibroID", prestamo.LibroId);
                        command.Parameters.AddWithValue("@FechaSolicitud", prestamo.FechaSolicitud);
                        command.Parameters.AddWithValue("@EstadoSolicitud", prestamo.EstadoSolicitud);
                        command.Parameters.AddWithValue("@TiempoEspera", prestamo.TiempoEspera);
                        command.Parameters.AddWithValue("@FechaPrestamo", prestamo.FechaPrestamo);
                        command.Parameters.AddWithValue("@FechaDevolucion", prestamo.FechaDevolucion);
                        command.Parameters.AddWithValue("@EstadoPrestamo", prestamo.EstadoPrestamo);
                        command.Parameters.AddWithValue("@TipoPrestamo", prestamo.TipoPrestamo);
                        command.Parameters.AddWithValue("@TiempoEntrega", prestamo.TiempoEntrega);
                        command.Parameters.AddWithValue("@Renovaciones", prestamo.Renovaciones);
                        command.Parameters.AddWithValue("@FechaRenovacion", prestamo.FechaRenovacion);

                        // Ejecutar el comando y obtener el resultado
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
