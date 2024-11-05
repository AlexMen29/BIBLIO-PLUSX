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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Common;
using MenuPrincipal.BD.Models;
using MenuPrincipal.PageUsuarios;

namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para PagEditCarrera.xaml
    /// </summary>
    public partial class PagEditCarrera : Page
    {
        public PagEditCarrera()
        {
            InitializeComponent();
            CargarDataGrid();
        }
        private CarreraModel datosCarrera;

        private void CargarDataGrid()
        {
            string consulta = "select* from Carrera";
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
                                CarreraModel carreraDatos = new CarreraModel();
                                {
                                    carreraDatos.CarreraID = Convert.ToInt32(dbRead["CarreraID"].ToString());
                                    carreraDatos.NombreCarrera = dbRead["NombreCarrera"].ToString();
                                };

                                lista.Add(carreraDatos);
                            }
                        }
                        CarrerasDataGrid.ItemsSource = lista;

                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado : " + e.Message);
            }
        }

        private void CarrerasDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datosCarrera = (CarreraModel)CarrerasDataGrid.SelectedItem;

            if (datosCarrera != null)
            {
                txtNuevoNombre.Text = datosCarrera.NombreCarrera.ToString();
                txtNuevoNombre.Focus();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            txtNuevoNombre.Clear();
        }

        private void ActualizarDato(CarreraModel datosCarrera)
        {
            // Quitar las comillas en @NuevoNombre
            string consulta = "UPDATE Carrera SET NombreCarrera = @NuevoNombre WHERE CarreraID = @CarreraId";

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandText = consulta;
                        cmd.Parameters.AddWithValue("@NuevoNombre", datosCarrera.NombreCarrera);
                        cmd.Parameters.AddWithValue("@CarreraId", datosCarrera.CarreraID);


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


        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Estás seguro de que deseas modificar este elemento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (resultado == MessageBoxResult.Yes)
            {
                datosCarrera.NombreCarrera = txtNuevoNombre.Text;
                ActualizarDato(datosCarrera);
                CargarDataGrid();
            }
        }
    }
}