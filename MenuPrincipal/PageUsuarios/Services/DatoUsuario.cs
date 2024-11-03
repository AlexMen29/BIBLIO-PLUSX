using MenuPrincipal.PageUsuarios.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows;

namespace MenuPrincipal.PageUsuarios.Services
{
    public class DatoUsuario
    {
        public DatoUsuario()
        {

        }


        //METODO PARA CARGAR DATOS AL DATAGRID
        public static List<UsuariosModel> MuestraUsuario()
        {
            List<UsuariosModel> lsUsuarios = new List<UsuariosModel>();

            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();
                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARUSUARIO";
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            //recorrer dataGrid
                            while (dr.Read())
                            {
                                UsuariosModel user = new UsuariosModel();
                                user.UsuarioId = int.Parse(dr["USUARIOID"].ToString());
                                user.NombreCompleto = dr["NOMBRECOMPLETO"].ToString();
                                user.Correo = dr["CORREO"].ToString();
                                user.Clave = dr["CLAVE"].ToString();

                                //Agregar a la lista inicial
                                lsUsuarios.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrio un error al intentar mostrar el registro: " + e.Message, "Validacion",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return lsUsuarios;
        }
    }
}