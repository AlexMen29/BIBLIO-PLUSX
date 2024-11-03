using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


using System.Data.SqlClient;
using System.Data;

namespace MenuPrincipal.BD.Services
{
    public class MetodosCredenciales
    {
        public static int EncontrarUsuario(TextBox carnet, PasswordBox clave)
        {
            int resp = 0;

            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SP_ENCONTRARUSUARIO";

                        // Pasar parámetros
                        cmd.Parameters.AddWithValue("@CARNET", carnet.Text);
                        cmd.Parameters.AddWithValue("@CLAVE", clave.Password);

                        // Usar ExecuteScalar para obtener el valor de retorno del procedimiento almacenado
                        object resultado = cmd.ExecuteScalar();

                     
                        if (resultado != null)
                        {
                            resp = Convert.ToInt32(resultado);
                        }
                        else
                        {
                            MessageBox.Show("No se encontró el usuario", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado\nDetalle: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return resp;
        }

    }

}
