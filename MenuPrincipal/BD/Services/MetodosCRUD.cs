using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Data.Sql;
using System.Data.SqlClient;

namespace MenuPrincipal.BD.Services
{
    public class MetodosCRUD
    {


        public static int ActualizarLibro(int detallesID, int autorID, int editorialID, int categoriaID, int edicionID, string isbn, string descripcion, string titulo, byte[] imagen)
        {
            int detallesModificado = 0;

            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "ActualizarLibros";

                        // Agregar parámetros al comando
                        command.Parameters.AddWithValue("@DetallesID", detallesID);
                        command.Parameters.AddWithValue("@AutorID", autorID);
                        command.Parameters.AddWithValue("@EditorialID", editorialID);
                        command.Parameters.AddWithValue("@CategoriaID", categoriaID);
                        command.Parameters.AddWithValue("@EdicionID", edicionID);
                        command.Parameters.AddWithValue("@ISBN", isbn);
                        command.Parameters.AddWithValue("@Descripcion", descripcion);
                        command.Parameters.AddWithValue("@Titulo", titulo);
                        command.Parameters.AddWithValue("@Imagen", imagen);

                        // Ejecutar el comando y obtener el DetallesID modificado
                        detallesModificado = (int)command.ExecuteScalar();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return detallesModificado;
        }


        //Inicio metodo

        public static int ObtenerId(string consultaSql, Dictionary<string, object> parametros = null)
        {
            int idResultado = 0;

            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = consultaSql;

                        // Agregar los parámetros a la consulta si existen
                        if (parametros != null)
                        {
                            foreach (var parametro in parametros)
                            {
                                command.Parameters.AddWithValue(parametro.Key, parametro.Value);
                            }
                        }

                        // Ejecutar la consulta y obtener el resultado como entero
                        idResultado = Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message, "Error de consulta", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return idResultado;
        }


    }
}
