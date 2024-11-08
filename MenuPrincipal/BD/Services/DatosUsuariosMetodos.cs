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


        public  DatosUsuariosModel MostrarUsuarios(int id)
        {
            DatosUsuariosModel Datos=null;
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




    }
}
