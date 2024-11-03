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
    public class MetodoDetallesUsuarios
    {
        public static List<DetallesUsuarios> MostrarUsuarios()
        {
            List<DetallesUsuarios> lstUsuarios = new List<DetallesUsuarios>();
            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_ObtenerDetallesUsuarios"; // Asegúrate de que este sea el nombre correcto

                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                DetallesUsuarios usuario = new DetallesUsuarios();
                                {
                                    usuario.Nombres = dr["Nombres"].ToString();
                                    usuario.Apellidos = dr["Apellidos"].ToString();
                                    //usuario.Direccion = dr["Direccion"].ToString();
                                    usuario.Telefono1 = !string.IsNullOrEmpty(dr["Telefono1"].ToString()) ? int.Parse(dr["Telefono1"].ToString()) : 0;
                                    usuario.Telefono2 = !string.IsNullOrEmpty(dr["Telefono2"].ToString()) ? int.Parse(dr["Telefono2"].ToString()) : 0;
                                    usuario.TelefonoFijo = !string.IsNullOrEmpty(dr["TelefonoFijo"].ToString()) ? int.Parse(dr["TelefonoFijo"].ToString()) : 0;
                                    usuario.FechaRegistro = DateTime.Parse(dr["FechaRegistro"].ToString());
                                    usuario.Carnet = dr["Carnet"].ToString();
                                    usuario.UsuarioID = int.Parse(dr["UsuarioID"].ToString());
                                    usuario.Estado = dr["Estado"].ToString();
                                    usuario.TipoUsuario = dr["TipoUsuario"].ToString();
                                    usuario.Carrera = dr["Carrera"].ToString();

                                };

                                lstUsuarios.Add(usuario);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener los Usuarios: " + e.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return lstUsuarios;
        }
        //Fin metodo
    }
}
