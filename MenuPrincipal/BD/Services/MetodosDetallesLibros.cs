using MenuPrincipal.BD.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using System.Data.SqlClient;
using System.Collections;

namespace MenuPrincipal.BD.Services
{
    public class MetodosDetallesLibros
    {


        public static List<DetallesLibros> MostrarLibros()
        {
            List<DetallesLibros> lstLibros = new List<DetallesLibros>();
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_ObtenerDetallesLibros"; // Asegúrate de que este sea el nombre correcto

                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                DetallesLibros libro = new DetallesLibros
                                {
                                    Titulo = dr["Titulo"].ToString(),
                                    Autor = dr["Autor"].ToString(),
                                    Editorial = dr["Editorial"].ToString(),
                                    Categoria = dr["Categoria"].ToString(),
                                    Edicion = dr["Edicion"].ToString(), // Asegúrate de que este sea el tipo correcto
                                    StockActual = int.Parse(dr["StockActual"].ToString()),
                                    DetalleID = int.Parse(dr["DetalleID"].ToString()),
                                    StockMaximo = int.Parse(dr["StockMaximo"].ToString()),
                                    StockMinimo = int.Parse(dr["StockMinimo"].ToString())

                                };

                                lstLibros.Add(libro);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener los libros: " + e.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstLibros;
        }
        //fin de metodo detalles libros


        public static ArrayList ObtenerIdModLibros(string Autor, string Editorial, string Categoria,string ISBN)
        {
            ArrayList list = new ArrayList();

            try
            {

                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();


                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "ObtenerIdModLibros"; // Asegúrate de que este sea el nombre correcto
                                                                    // Agregar los parámetros al comando
                        command.Parameters.AddWithValue("@NombreAutor", Autor);
                        command.Parameters.AddWithValue("@NombreEditorial", Editorial);
                        command.Parameters.AddWithValue("@NombreCategoria", Categoria);
                        command.Parameters.AddWithValue("@ISBN", ISBN);

                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read()) 
                            {
                                list.Add(dr["Resultado"]);
                            }
                        }

                    }

                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            return list;
        }


       


    }
}
