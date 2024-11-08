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
using MenuPrincipal.DatosGenerales;

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

        DatosGlobales datos= new DatosGlobales();

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
                btnAgregar.IsEnabled = false;

                txtNuevoNombre.Focus();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            txtNuevoNombre.Clear();
            btnAgregar.IsEnabled = true;

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

        private void AgregarDatos(CarreraModel datosCarrera)
        {
            string consulta = "INSERT INTO Carrera (NombreCarrera) VALUES (@NuevoNombre)";

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandText = consulta;
                        cmd.Parameters.AddWithValue("@NuevoNombre", datosCarrera.NombreCarrera);
                        


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Carrera agregada exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error inesperado, no se ha podido agregar la carrera", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
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
            TextBox[] arr = new TextBox[1];

            // Asignar un TextBox al array
            arr[0] = txtNuevoNombre;
        


            bool validacion = datos.VerifcarTextBox(arr);
            if (validacion == true)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Estás seguro de que deseas modificar este elemento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado == MessageBoxResult.Yes)
                {
                    datosCarrera.NombreCarrera = txtNuevoNombre.Text;
                    ActualizarDato(datosCarrera);
                    CargarDataGrid();
                }
            }
            else 
            {
                MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }


        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            // Declarar un array de TextBox
            TextBox[] arr = new TextBox[1];

            // Asignar un TextBox al array
            arr[0] = txtNuevoNombre;

            bool validacion = datos.VerifcarTextBox(arr);

            if (validacion == true)
            {

                MessageBoxResult resultado = MessageBox.Show("¿Estás seguro de que deseas agregar este elemento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado == MessageBoxResult.Yes)
                {
                    datosCarrera = new CarreraModel();
                    datosCarrera.NombreCarrera = txtNuevoNombre.Text;
                    btnAgregar.IsEnabled = true;

                    AgregarDatos(datosCarrera);
                    CargarDataGrid();
                }
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}