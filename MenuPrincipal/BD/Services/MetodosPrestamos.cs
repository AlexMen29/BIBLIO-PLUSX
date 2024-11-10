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
                        command.CommandText = "SP_NuevoPrestamo";

                        // Agregar parámetros al comando usando el objeto de tipo IngresoPrestamoModel
                        command.Parameters.AddWithValue("@UsuarioID", prestamo.UsuarioId);
                        command.Parameters.AddWithValue("@LibroID", prestamo.LibroId);
                        command.Parameters.AddWithValue("@FechaSolicitud", prestamo.FechaSolicitud);
                        command.Parameters.AddWithValue("@EstadoSolicitud", prestamo.EstadoSolicitud);
                        command.Parameters.AddWithValue("@TiempoEspera", prestamo.TiempoEspera);
                        command.Parameters.AddWithValue("@FechaPrestamo", prestamo.FechaPrestamo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@FechaDevolucion", prestamo.FechaDevolucion ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@EstadoPrestamo", prestamo.EstadoPrestamo);
                        command.Parameters.AddWithValue("@TipoPrestamo", prestamo.TipoPrestamo);
                        command.Parameters.AddWithValue("@TiempoEntrega", prestamo.TiempoEntrega);
                        command.Parameters.AddWithValue("@Renovaciones", prestamo.Renovaciones);
                        command.Parameters.AddWithValue("@FechaRenovacion", prestamo.FechaRenovacion ?? (object)DBNull.Value);

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

        public int CrearRegistroPrestamo(double precioPrestamo)
        {
            int resultado = 0; // Variable para almacenar el resultado de la ejecución

            try
            {
                // Crear y abrir una conexión a la base de datos
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    // Crear un comando para ejecutar el procedimiento almacenado
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "InsertarPrestamoCompleto"; // Nombre del procedimiento almacenado

                        // Agregar el parámetro necesario para el procedimiento almacenado
                        command.Parameters.AddWithValue("@PrecioPrestamo", precioPrestamo);

                        // Ejecutar el comando y obtener el resultado
                        resultado = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                // Mostrar un mensaje en caso de error
                MessageBox.Show("Ocurrió un error: " + e.Message, "Error de consulta", MessageBoxButton.OK, MessageBoxImage.Error);
                resultado = -1; // Retornar un valor específico en caso de error
            }

            return resultado; // Devolver el resultado de la ejecución
        }

        public decimal CalcularCostoPrestamo(int prestamoID)
        {
            decimal resultado = 0; // Variable para almacenar el valor calculado del costo del préstamo

            try
            {
                // Crear y abrir una conexión a la base de datos
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    // Crear un comando para ejecutar el procedimiento almacenado
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "CalcularCostoPrestamo"; // Nombre del procedimiento almacenado

                        // Agregar el parámetro necesario para el procedimiento almacenado
                        command.Parameters.AddWithValue("@PrestamoID", prestamoID);

                        // Definir el parámetro de salida para el valor calculado
                        SqlParameter outputParam = new SqlParameter("@ValorPagar", SqlDbType.Decimal)
                        {
                            Precision = 10,
                            Scale = 2,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        // Ejecutar el comando
                        command.ExecuteNonQuery();

                        // Obtener el valor de salida (ValorPagar calculado)
                        resultado = (decimal)outputParam.Value;
                    }
                }
            }
            catch (Exception e)
            {
                // Mostrar un mensaje en caso de error
                MessageBox.Show("Ocurrió un error: " + e.Message, "Error de consulta", MessageBoxButton.OK, MessageBoxImage.Error);
                resultado = -1; // Retornar un valor específico en caso de error
            }

            return resultado; // Devolver el valor calculado del costo del préstamo
        }

        public bool ActualizarEstadoPago(int prestamoID)
        {
            bool actualizado = false; // Variable para indicar si la actualización fue exitosa

            try
            {
                // Crear y abrir una conexión a la base de datos
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    // Crear un comando para ejecutar el procedimiento almacenado
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_ActualizarEstadoPago"; // Nombre del procedimiento almacenado

                        // Agregar el parámetro necesario para el procedimiento almacenado
                        command.Parameters.AddWithValue("@PrestamoID", prestamoID);

                        // Ejecutar el comando y verificar si se afectó alguna fila
                        int filasAfectadas = command.ExecuteNonQuery();
                        actualizado = filasAfectadas > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Mostrar un mensaje en caso de error
                MessageBox.Show("Ocurrió un error: " + ex.Message, "Error de actualización", MessageBoxButton.OK, MessageBoxImage.Error);
                actualizado = false; // Si hay error, retorna falso
            }

            return actualizado; // Retornar verdadero si se realizó la actualización correctamente
        }



    }
}