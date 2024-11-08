using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

using System.Data;
using System.Data.SqlClient;
using System.Windows.Documents;

namespace MenuPrincipal.DatosGenerales
{
    public class DatosGlobales
    {

        public DatosGlobales() { }


        public string consultaAutor = "select NombreAutor from Autores";
        public string consultaCategoria = "select NombreCategoria from Categorias";
        public string consultaEdiorial = "select NombreEditorial from Editoriales";
        public string consultaTipoUsuario = "select DISTINCT Tipo from TipoUsuario";
        public string consultaCarrera = "select NombreCarrera from Carrera";
        public string consultarEdicion = "select ISBN from Ediciones";
        public string consultarProveedores = "select NombreProveedor from Proveedores";
        public string consultaTiposPrestamos = "Select DISTINCT TipoPrestamo from Prestamos";
        public string consultaEstadoPrestamos = "Select DISTINCT EstadoPrestamo from Prestamos";


        public BitmapImage ConvertirABitmapImage(byte[] imageBytes)
        {
            using (var ms = new System.IO.MemoryStream(imageBytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public void LlenarBoxFiltros(string consulta, ComboBox elementoBox, string columna)
        {
            try
            {
                //Lista con valores correspondientes a ComboBox
                List<string> Lista = new List<string>();
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = new SqlCommand(consulta, conn))
                    {
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Lista.Add(dr[columna].ToString());
                            }
                            //Lista.Add("Ninguno");
                        }
                    }
                }

                elementoBox.ItemsSource = Lista; // Asigna la lista al ComboBox
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error inesperado: {e.Message}");
            }
        }

        //Inicio ,metodo:

        public void AsignarValorTextBox(string consulta, string valorWhere, TextBox textBoxDestino, string parametro)
        {
            try
            {
                //// Eliminación de espacios para asegurar formato correcto
                //valorWhere = valorWhere.Replace(" ", "");

                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = new SqlCommand(consulta, conn))
                    {
                        // Agregar el parámetro WHERE a la consulta
                        command.Parameters.AddWithValue(parametro, valorWhere);

                        using (var dr = command.ExecuteReader())
                        {
                            if (dr.Read()) // Si hay un resultado, asignarlo al TextBox
                            {
                                textBoxDestino.Text = dr[0].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No se encontró ningún valor que coincida con el filtro especificado.",
                                                "Resultado no encontrado", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error inesperado: {e.Message}", "Error de consulta", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public List<object> ObtenerImgDescripcion(string edicion)
        {
            // Lista para almacenar la descripción y la imagen
            List<object> listaDatos = new List<object>();

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select Imagen, Descripcion from Ediciones where ISBN = @edicion";

                        // Usamos un parámetro para evitar SQL Injection
                        cmd.Parameters.AddWithValue("@edicion", edicion);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] imagen = reader["Imagen"] as byte[];

                                // Obtiene la descripción
                                string descripcion = reader["Descripcion"].ToString();

                                listaDatos.Add(imagen);
                                listaDatos.Add(descripcion);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado: " + e.Message, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return listaDatos;  // Devolver la lista con la imagen y la descripción
        }

        public List<object> ObtenerImgDescripcionPorTitulo(string titulo) // CAMBIO CLAVE
        {
            List<object> listaDatos = new List<object>();

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "SELECT Imagen FROM Ediciones WHERE Titulo = @titulo";

                        // Parámetro para evitar SQL Injection
                        cmd.Parameters.AddWithValue("@titulo", titulo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                byte[] imagen = reader["Imagen"] as byte[];

                                listaDatos.Add(imagen);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado: " + e.Message, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            return listaDatos;
        }

        //Metodo Verificar Cajas


        public bool VerifcarTextBox(TextBox[] cajas)
        {
            try
            {


                for (int x = 0; x < cajas.Length; x++)
                {
                    if (string.IsNullOrWhiteSpace(cajas[x].Text))
                    {
                        return false; // Retorna false tan pronto como encuentre un TextBox vacío.
                    }
                }

                return true; // Si no se encontró ningún TextBox vacío, retorna true.
            }
            catch (Exception e)
            {
                return false; 
            }
        }



    }
}