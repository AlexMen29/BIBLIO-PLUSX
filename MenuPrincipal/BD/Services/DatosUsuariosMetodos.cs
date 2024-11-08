using MenuPrincipal.PageUsuarios;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MenuPrincipal.BD.Models;

using System.Data.SqlClient;

namespace MenuPrincipal.BD.Services
{
    public class DatosUsuariosMetodos
    {


        public DatosUsuariosModel MostrarUsuarios(int id)
        {
            DatosUsuariosModel Datos = null;
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "sp_DatosUsuariosPorID";
                        command.Parameters.AddWithValue("@UsuarioID", id);

                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                DatosUsuariosModel usuario = new DatosUsuariosModel
                                {
                                    UsuarioID = dr["UsuarioID"] != DBNull.Value ? int.Parse(dr["UsuarioID"].ToString()) : 0,
                                    Nombres = dr["Nombres"].ToString(),
                                    Apellidos = dr["Apellidos"].ToString(),
                                    EstadoCivil = dr["EstadoCivil"].ToString(),
                                    ApellidoCasada = dr["ApellidoCasada"].ToString(),
                                    Correo1 = dr["Correo1"].ToString(),
                                    Correo2 = dr["Correo2"].ToString(),
                                    Telefono1 = dr["Telefono1"].ToString(),
                                    Telefono2 = dr["Telefono2"].ToString(),
                                    TelefonoFijo = dr["TelefonoFijo"].ToString(),
                                    Carnet = dr["Carnet"].ToString(),
                                    Estado = dr["Estado"].ToString(),
                                    TipoUsuario = dr["TipoUsuario"].ToString(),
                                    Carrera = dr["Carrera"].ToString(),
                                    Colonia = dr["Colonia"].ToString(),
                                    Calle = dr["Calle"].ToString(),
                                    Casa = dr["Casa"].ToString(),
                                    Municipio = dr["Municipio"].ToString(),
                                    Departamento = dr["Departamento"].ToString(),
                                    CP = dr["CP"].ToString()
                                };

                                Datos = usuario;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener los Usuarios: " + e.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return Datos;
        }

        //Metodo de modificar usuario:

        public bool ModificarUsuario(DatosUsuariosModel usuario)
        {
            bool resultado = false;

            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "sp_ModificarUsuario";

                        // Agregar parámetros al comando usando el objeto de tipo DatosUsuariosModel
                        command.Parameters.AddWithValue("@UsuarioID", usuario.UsuarioID);
                        command.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                        command.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                        command.Parameters.AddWithValue("@EstadoCivil", usuario.EstadoCivil);
                        command.Parameters.AddWithValue("@ApellidoCasada", usuario.ApellidoCasada);
                        command.Parameters.AddWithValue("@Correo1", usuario.Correo1);
                        command.Parameters.AddWithValue("@Correo2", usuario.Correo2);
                        command.Parameters.AddWithValue("@Telefono1", usuario.Telefono1);
                        command.Parameters.AddWithValue("@Telefono2", usuario.Telefono2);
                        command.Parameters.AddWithValue("@TelefonoFijo", usuario.TelefonoFijo);
                        command.Parameters.AddWithValue("@Carnet", usuario.Carnet);
                        command.Parameters.AddWithValue("@EstadoID", usuario.EstadoID);
                        command.Parameters.AddWithValue("@TipoUsuarioID", usuario.TipoUsuarioId);
                        command.Parameters.AddWithValue("@CarreraID", usuario.CarreraID);
                        command.Parameters.AddWithValue("@Colonia", usuario.Colonia);
                        command.Parameters.AddWithValue("@Calle", usuario.Calle);
                        command.Parameters.AddWithValue("@Casa", usuario.Casa);
                        command.Parameters.AddWithValue("@Municipio", usuario.Municipio);
                        command.Parameters.AddWithValue("@Departamento", usuario.Departamento);
                        command.Parameters.AddWithValue("@CP", usuario.CP);

                        // Ejecutar el comando y verificar el resultado
                        int filasAfectadas = command.ExecuteNonQuery();
                        resultado = true;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error: " + e.Message, "Error de consulta", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return resultado;
        }




    }
}
