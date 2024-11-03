using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


using System.Data.SqlClient;
using MenuPrincipal.BD.Models;

namespace MenuPrincipal.BD.Services
{
    internal class MetodosCompras
    {

        public static int RegistrarCompraCompleta(ComprasModel compra)
        {
            int compraIDGenerado = 0;

            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "sp_RegistrarCompra";

                        // Agregar parámetros al comando usando el objeto de tipo ComprasModel
                        command.Parameters.AddWithValue("@Cantidad", compra.Cantidad);
                        command.Parameters.AddWithValue("@CostoUnidad", compra.CostoUnidad);
                        command.Parameters.AddWithValue("@FechaCompra", compra.FechaCompra);
                        command.Parameters.AddWithValue("@CostoTotal", compra.CostoTotal);
                        command.Parameters.AddWithValue("@EdicionID", compra.EdicionID);
                        command.Parameters.AddWithValue("@ProveedorID", compra.ProveedorID);
                        command.Parameters.AddWithValue("@DetallesID", compra.DetallesID);

                        // Ejecutar el comando y obtener el ID de compra generado
                        compraIDGenerado = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message, "Error de consulta", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show("Id de respuesta: "+compraIDGenerado);
            return compraIDGenerado;

        
        }

        //Inicio nuevo metodo

        public static int CrearDatosLibros(CrearDatoLibroModel libro)
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
                        command.CommandText = "sp_CrearDatosLibros";

                        // Agregar parámetros al comando usando el objeto de tipo LibrosModel
                        command.Parameters.AddWithValue("@ISBN", libro.ISBN);
                        command.Parameters.AddWithValue("@Descripcion", libro.Descripcion);
                        command.Parameters.AddWithValue("@Titulo", libro.Titulo);
                        command.Parameters.AddWithValue("@Imagen", libro.Imagen);
                        command.Parameters.AddWithValue("@AutorID", libro.AutorID);
                        command.Parameters.AddWithValue("@EditorialID", libro.EditorialID);
                        command.Parameters.AddWithValue("@CategoriaID", libro.CategoriaID);
                        command.Parameters.AddWithValue("@StockMinimo", libro.StockMinimo);
                        command.Parameters.AddWithValue("@StockMaximo", libro.StockMaximo);

                        // Ejecutar el comando y obtener el resultado
                        resultado = Convert.ToInt32(command.ExecuteScalar());
                        MessageBox.Show("Exito en metodo "+resultado);
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
