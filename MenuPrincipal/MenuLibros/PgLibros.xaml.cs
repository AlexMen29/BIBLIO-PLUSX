using MenuPrincipal.ActualizacionesDatos;
using MenuPrincipal.BD.Models;
using MenuPrincipal.BD.Services;
using MenuPrincipal.DatosGenerales;
using MenuPrincipal.DetallesL;
//using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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

namespace MenuPrincipal.MenuLibros
{
    /// <summary>
    /// Lógica de interacción para PgLibros.xaml
    /// </summary>
    public partial class PgLibros : Page
    {

        //Instancia para datos generales
        DatosGlobales datos = new DatosGlobales();


        private DataTable dataTable;
        private int Nivel;
        public PgLibros(int Nivel)
        {
            InitializeComponent();
            CargarDatos();
            CargarLibrosRecientes();
            CargarLibrosPoesia();
            this.Nivel = Nivel;
            datos.LlenarBoxFiltros(datos.consultaAutor, AutorComboBox, "NombreAutor");
            datos.LlenarBoxFiltros(datos.consultaEdiorial, EditorialComboBox, "NombreEditorial");
            datos.LlenarBoxFiltros(datos.consultaCategoria, CategoriaComboBox, "NombreCategoria");
            if (Nivel == 1)
            {
                tabAdministrador.Visibility = Visibility.Collapsed;
                tabAllBooks.Visibility = Visibility.Collapsed;
                TabControlLibros.SelectedIndex = 0;
            }
            else
            {

                TabControlLibros.SelectedIndex = 2;
            }
        }



        public List<DetallesLibros> ListaDataGrid;
        DetallesLibros Libros;



        SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB);


        //Funcion creada para llamar solo una vez para cargar todos los elementos

        //funcion que carga elementos segun genero
        private void CargarElementosPorGenero(string query, ItemsControl contenGrid)
        {
            try
            {
                conDB.Open();
                SqlCommand command = new SqlCommand(query, conDB);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);


                // Crear una nueva columna para las imágenes
                dataTable.Columns.Add("ImageData", typeof(BitmapImage));

                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["imagen"] != DBNull.Value)
                    {
                        byte[] imageBytes = (byte[])row["imagen"];
                        BitmapImage bitmapImage = datos.ConvertirABitmapImage(imageBytes);
                        row["ImageData"] = bitmapImage;
                    }
                }

                // Asignar los datos al ItemsControl
                contenGrid.ItemsSource = dataTable.DefaultView;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar a la base de datos: {ex.Message}");
            }
            finally
            {
                conDB.Close();
            }
        }

        private void CargarLibrosRecientes()
        {
            string query = "SELECT TOP 4 e.Titulo, e.Imagen, a.NombreAutor FROM Ediciones e INNER JOIN DetallesLibros d ON e.EdicionID = d.EdicionID INNER JOIN Categorias c ON d.CategoriaID = c.CategoriaID INNER JOIN Autores a ON d.AutorID = a.AutorID ORDER BY e.EdicionID DESC;";
            CargarElementosPorGenero(query, contentGridRecientes);
        }

        private void CargarLibrosPoesia()
        {
            string query = "SELECT TOP 4 e.Titulo, e.Imagen, a.NombreAutor FROM Ediciones e INNER JOIN DetallesLibros d ON e.EdicionID = d.EdicionID INNER JOIN Categorias c ON d.CategoriaID = c.CategoriaID INNER JOIN Autores a ON d.AutorID = a.AutorID WHERE c.CategoriaID = 2 ORDER BY e.EdicionID DESC;";
            CargarElementosPorGenero(query, contentGridPoesia);
        }

        private void CargarLibrosTodos()
        {
            string query = "SELECT e.Titulo, e.Imagen, a.NombreAutor FROM Ediciones e INNER JOIN DetallesLibros d ON e.EdicionID = d.EdicionID INNER JOIN Categorias c ON d.CategoriaID = c.CategoriaID INNER JOIN Autores a ON d.AutorID = a.AutorID ORDER BY e.Titulo ASC;";
            CargarElementosPorGenero(query, contentGridTodos);
        }

        #region ADMINISTRADOR

        public void CargarDatos()
        {
            ListaDataGrid = MetodosDetallesLibros.MostrarLibros();
            LibrosDataGrid.ItemsSource = ListaDataGrid;

        }






        public void AplicarFiltro()
        {
            // Obtenemos la lista completa de libros
            List<DetallesLibros> librosFiltrados = MetodosDetallesLibros.MostrarLibros();

            // Filtramos por autor si hay un valor seleccionado
            if (AutorComboBox.SelectedItem != null && AutorComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string autorSeleccionado = AutorComboBox.SelectedItem.ToString();
                librosFiltrados = librosFiltrados
                    .Where(libro => libro.Autor.Equals(autorSeleccionado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filtramos por editorial si hay un valor seleccionado
            if (EditorialComboBox.SelectedItem != null && EditorialComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string editorialSeleccionada = EditorialComboBox.SelectedItem.ToString();
                librosFiltrados = librosFiltrados
                    .Where(libro => libro.Editorial.Equals(editorialSeleccionada, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filtramos por categoría si hay un valor seleccionado
            if (CategoriaComboBox.SelectedItem != null && CategoriaComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string categoriaSeleccionada = CategoriaComboBox.SelectedItem.ToString();
                librosFiltrados = librosFiltrados
                    .Where(libro => libro.Categoria.Equals(categoriaSeleccionada, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (StockComboBox.SelectedItem != null && ((ComboBoxItem)StockComboBox.SelectedItem).Content.ToString() != "Ninguno")
            {


                string stockSeleccionado = ((ComboBoxItem)StockComboBox.SelectedItem).Content.ToString();

                if (stockSeleccionado == "Mayor a Menor")
                {
                    librosFiltrados = librosFiltrados
                        .OrderByDescending(libro => libro.StockActual) // Ordenar de mayor a menor
                        .ToList();
                }
                else if (stockSeleccionado == "Menor a Mayor")
                {
                    librosFiltrados = librosFiltrados
                        .OrderBy(libro => libro.StockActual) // Ordenar de menor a mayor
                        .ToList();
                }
            }


            // Asignamos los libros filtrados al DataGrid
            LibrosDataGrid.ItemsSource = librosFiltrados;


        }



        private void AutorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            AplicarFiltro();
        }
        private void EditorialComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void CategoriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void StockComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            AplicarFiltro();
        }



        #endregion

        private void btnQuitarFiltros_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFiltros();
        }
        public void LimpiarFiltros()
        {
            AutorComboBox.SelectedIndex = -1;
            EditorialComboBox.SelectedIndex = -1;
            CategoriaComboBox.SelectedIndex = -1;
            StockComboBox.SelectedIndex = -1;
        }

        private void LibrosDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            labSeleccion.Content = null;
            Libros = (DetallesLibros)LibrosDataGrid.SelectedItem;

            if (Libros == null)
            {
                return;
            }

            labSeleccion.Content += $"Elemento Seleccionado: {Libros.Titulo}";

        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ActualizacionLibros VentanaActualizacion = new ActualizacionLibros(Libros, 0);
            VentanaActualizacion.ShowDialog();
        }

        private void gpbTodos_Loaded(object sender, RoutedEventArgs e)
        {
            CargarLibrosTodos();
        }

        private void btnDetalles_Click(object sender, RoutedEventArgs e)
        {
            if (Nivel != 1)
            {
                Detalles Ventana = new Detalles();
                Ventana.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor inicie sesión para visualizar los detalles del libro", "Inicie Sesión", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}
