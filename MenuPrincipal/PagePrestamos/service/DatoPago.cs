using MenuPrincipal.PagePrestamos.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MenuPrincipal.PagePrestamos.service
{
    public class DatoPago
    {
        public static List<PagoPrestamoModel> CargarControlPagos()
        {
            List<PagoPrestamoModel> listaPagos = new List<PagoPrestamoModel>();

            try
            {
                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SPMOSTRARCONTROLPAGOS";

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var pago = new PagoPrestamoModel
                                {
                                    PagoId = reader.GetInt32(0),
                                    Titulo = reader.GetString(1),
                                    TipoPrestamo = reader.GetString(2),
                                    Periodo = reader.GetDateTime(3),
                                    Monto = reader.GetDecimal(4)
                                };
                                listaPagos.Add(pago);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los pagos: " + ex.Message);
            }

            return listaPagos;
        }
    }
}