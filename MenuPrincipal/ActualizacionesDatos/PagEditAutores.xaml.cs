using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data;
using System.Data.SqlClient;
using MenuPrincipal.BD.Models;
using System.Data.Common;

namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para PagEditAutores.xaml
    /// </summary>
    public partial class PagEditAutores : Page
    {
        public PagEditAutores()
        {
            InitializeComponent();
            CargarDataGrid();
        }

        private AutoresModel autoresDatos;



        private void AutoresDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            autoresDatos = (AutoresModel)AutoresDataGrid.SelectedItem;

            if (autoresDatos != null)
            {
                txtNuevoNombre.Text = autoresDatos.NombreAutor.ToString();
                txtNacionalidad.Text = autoresDatos.Nacionalidad.ToString();
                DateFecha.SelectedDate = (DateTime?)autoresDatos.FechaNacimiento;
                txtBibliografia.Text = autoresDatos.Bibliografia.ToString();
                txtNuevoNombre.Focus();
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Estás seguro de que deseas modificar este elemento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (resultado == MessageBoxResult.Yes)
            {
                autoresDatos.NombreAutor = txtNuevoNombre.Text;
                autoresDatos.Nacionalidad = txtNacionalidad.Text;
                autoresDatos.FechaNacimiento = DateFecha.SelectedDate.Value;
                autoresDatos.Bibliografia = txtBibliografia.Text;
                ActualizarDato(autoresDatos);
                Limpiartxt();
                CargarDataGrid();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Limpiartxt();
        }

        private void Limpiartxt()
        {
            txtNuevoNombre.Clear();
            txtNacionalidad.Clear();
            DateFecha.SelectedDate = null;
            txtBibliografia.Clear();

        }


        #region
        private void CargarDataGrid()
        {
            string consulta = "select *from Autores";
            List<Object> lista = new List<Object>();

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandText = consulta;

                        using (DbDataReader dbRead = cmd.ExecuteReader())
                        {
                            while (dbRead.Read())
                            {
                                AutoresModel autoresDatos = new AutoresModel();
                                {
                                    autoresDatos.AutorID = Convert.ToInt32(dbRead["AutorID"].ToString());
                                    autoresDatos.NombreAutor = dbRead["NombreAutor"].ToString();
                                    autoresDatos.Nacionalidad = dbRead["Nacionalidad"].ToString();
                                    autoresDatos.FechaNacimiento = Convert.ToDateTime(dbRead["FechaNacimiento"].ToString());
                                    autoresDatos.Bibliografia = dbRead["Bibliografia"].ToString();

                                };

                                lista.Add(autoresDatos);
                            }
                        }
                        AutoresDataGrid.ItemsSource = lista;

                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado : " + e.Message);
            }
        }

        private void ActualizarDato(AutoresModel datosAutores)
        {

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_ActualizarAutor";



                        cmd.Parameters.AddWithValue("@NuevoNombre", datosAutores.NombreAutor);
                        cmd.Parameters.AddWithValue("@NuevaNacionalidad", datosAutores.Nacionalidad);
                        cmd.Parameters.AddWithValue("@NuevaFechaNacimiento", datosAutores.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@NuevaBibliografia", datosAutores.Bibliografia);
                        cmd.Parameters.AddWithValue("@AutorID", datosAutores.AutorID);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Dato actualizado exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error inesperado, no se ha podido actualizar", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado : " + e.Message);
            }
        }


        #endregion
    }
}