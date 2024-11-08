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
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using MenuPrincipal.BD.Models;
using MenuPrincipal.MenuLibros;
using MenuPrincipal.BD.Services;
using MenuPrincipal.DatosGenerales;
using System.Collections;

namespace MenuPrincipal.PageUsuarios
{
    /// <summary>
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class Usuarios : Page
    {

        DatosGlobales datos = new DatosGlobales();

        public List<DetallesUsuarios> ListaDataGrid;
        DetallesUsuarios UsuariosData;

        public Usuarios()
        {
            InitializeComponent();
            CargarDatos();


            LlenarBoxFiltros(datos.consultaTipoUsuario, TipoUsuarioComboBox, "Tipo");
            LlenarBoxFiltros(datos.consultaCarrera, CarreraComboBox, "NombreCarrera");
        }


        public void CargarDatos()
        {
            ListaDataGrid = MetodoDetallesUsuarios.MostrarUsuarios();
            UsuariosDataGrid.ItemsSource = ListaDataGrid;

        }

        public void LlenarBoxFiltros(string consulta, ComboBox elementoBox, string columna)
        {
            try
            {
                //Lista con valores correspondientes a ComboBox
                List<string> Lista = new List<string>();
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = new SqlCommand(consulta, conn))
                    {
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Lista.Add(dr[columna].ToString());
                            }
                        }
                    }
                }


                elementoBox.ItemsSource = Lista; // Asigna la lista al ComboBox



            }
            catch (Exception e)
            {
                MessageBox.Show($"Error inesperado: {e.Message}");
            }
        }

        public void AplicarFiltro()
        {
            List<DetallesUsuarios> usuariosFiltrados = MetodoDetallesUsuarios.MostrarUsuarios();

            // Filtramos por tipo de usuario si hay un valor seleccionado
            if (TipoUsuarioComboBox.SelectedItem != null && TipoUsuarioComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string TipoUsuarioSeleccionado = TipoUsuarioComboBox.SelectedItem.ToString();
                usuariosFiltrados = usuariosFiltrados
                    .Where(usuario => usuario.TipoUsuario.Equals(TipoUsuarioSeleccionado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }



            // Filtramos por carrera si hay un valor seleccionado
            if (CarreraComboBox.SelectedItem != null && CarreraComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string CarreraSeleccionada = CarreraComboBox.SelectedItem.ToString();
                usuariosFiltrados = usuariosFiltrados
                    .Where(usuario => usuario.Carrera.Equals(CarreraSeleccionada, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (FechaComboBox.SelectedItem != null && ((ComboBoxItem)FechaComboBox.SelectedItem).Content.ToString() != "Ninguno")
            {


                string FechaSeleccionada = ((ComboBoxItem)FechaComboBox.SelectedItem).Content.ToString();

                if (FechaSeleccionada == "Mas Reciente")
                {
                    usuariosFiltrados = usuariosFiltrados
                        .OrderByDescending(usuario => usuario.FechaRegistro) // Ordenar de mayor a menor
                        .ToList();
                }
                else if (FechaSeleccionada == "Mas Antiguo")
                {
                    usuariosFiltrados = usuariosFiltrados
                        .OrderBy(usuario => usuario.FechaRegistro) // Ordenar de menor a mayor
                        .ToList();
                }
            }

            // Asignamos los libros filtrados al DataGrid
            if (usuariosFiltrados.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios que coincidan con los criterios de búsqueda.");
                CargarDatos();
                LimpiarCajas();
                
            }
            else
            {
                UsuariosDataGrid.ItemsSource = usuariosFiltrados;
            }




            

        }



        private void UsuariosDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            labSeleccion.Content = null;
            UsuariosData = (DetallesUsuarios)UsuariosDataGrid.SelectedItem;

            if (UsuariosData == null)
            {
                return;
            }

            labSeleccion.Content += $"Elemento Seleccionado: {UsuariosData.Nombres}";
        }

        private void TipoUsuarioComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void CarreraComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void FechaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void BuscarUsuarios()
        {
            // Obtenemos la lista completa de usuarios
            List<DetallesUsuarios> usuariosFiltrados = MetodoDetallesUsuarios.MostrarUsuarios();

            // Filtramos la lista por el carnet ingresado en el TextBox
            string carnet = CarnetTextBox.Text;
            if (!string.IsNullOrEmpty(carnet))
            {
                usuariosFiltrados = usuariosFiltrados
                    .Where(usuario => usuario.Carnet.IndexOf(carnet, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // Asignamos los usuarios filtrados al DataGrid
            if (usuariosFiltrados.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios que coincidan con el criterio de búsqueda.");
                LimpiarCajas();
                UsuariosDataGrid.ItemsSource = MetodoDetallesUsuarios.MostrarUsuarios();

            }
            else
            {

                UsuariosDataGrid.ItemsSource = usuariosFiltrados;
            }
        }


        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

            // Declarar un array de TextBox
            TextBox[] arr = new TextBox[1];

            // Asignar un TextBox al array
            arr[0] = CarnetTextBox;
          

            bool validacion = datos.VerifcarTextBox(arr);

            if (validacion == true)
            {
                BuscarUsuarios();
            }
            else

            {
                MessageBoxResult resultado = MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }


        }

        private void btnQuitarFiltros_Click(object sender, RoutedEventArgs e)
        {

            LimpiarCajas();
            CargarDatos();

        }

        private void LimpiarCajas()
        {
            // Limpiar las selecciones de los ComboBox
            TipoUsuarioComboBox.SelectedItem = null;
            CarreraComboBox.SelectedItem = null;
            FechaComboBox.SelectedItem = null;
            CarnetTextBox.Text = null;
        }





    }
}