using MenuPrincipal.PagePrestamos.Models;
using MenuPrincipal.PagePrestamos.service;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using MenuPrincipal.DatosGenerales;
using MenuPrincipal.BD.Models;
using MenuPrincipal.BD.Services;
using System.Collections.Generic;


namespace MenuPrincipal.PagePrestamos
{
    public partial class Prestamos : Page
    {

        DatosGlobales datos= new DatosGlobales();

        public Prestamos()
        {
            InitializeComponent();
            CargarClasificacionPrestamos();
            CargarControlPagos();
            CargarDatosComboBox();
        }

        private void CargarClasificacionPrestamos()
        {
            dataGridPrestamos.ItemsSource = DatoPrestamo.CargarClasificacionPrestamos();
        }

        private void CargarControlPagos()
        {
            dataGridPagos.ItemsSource = DatoPago.CargarControlPagos();
        }
        private void CargarDatosComboBox()
        {
            datos.LlenarBoxFiltros(datos.consultaTiposPrestamos, TipoPrestamoComboBox, "TipoPrestamo");
            datos.LlenarBoxFiltros(datos.consultaEstadoPrestamos, EstadoComboBox, "EstadoPrestamo");
        }
   

        private void BtnCalcularPago_Click(object sender, RoutedEventArgs e)
        {
            string periodoPago = ((ComboBoxItem)comboBoxPeriodoPago.SelectedItem)?.Content?.ToString();
            string monto = textBoxMonto.Text;

            if (string.IsNullOrEmpty(periodoPago) || string.IsNullOrEmpty(monto) || !decimal.TryParse(monto, out decimal montoDecimal))
            {
                MessageBox.Show("Por favor, completa el período de pago y el monto correctamente.");
                return;
            }

            MessageBox.Show($"Pago calculado para el período: {periodoPago}, con un monto de {montoDecimal:C}");
        }


        //Metodo para filtrar

        public void AplicarFiltro()
        {
            // Obtenemos la lista completa de libros
            List<PrestamoModel> librosFiltrados = DatoPrestamo.CargarClasificacionPrestamos();

            // Filtramos por autor si hay un valor seleccionado
            if (TipoPrestamoComboBox.SelectedItem != null && TipoPrestamoComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string TipoPrestamo = TipoPrestamoComboBox.SelectedItem.ToString();
                librosFiltrados = librosFiltrados
                    .Where(libro => libro.TipoPrestamo.Equals(TipoPrestamo, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filtramos por editorial si hay un valor seleccionado
            if (EstadoComboBox.SelectedItem != null && EstadoComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string EstadoSeleccionado = EstadoComboBox.SelectedItem.ToString();
                librosFiltrados = librosFiltrados
                    .Where(libro => libro.EstadoPrestamo.Equals(EstadoSeleccionado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            

            if (FechaDevolucionComboBox.SelectedItem != null && ((ComboBoxItem)FechaDevolucionComboBox.SelectedItem).Content.ToString() != "Ninguno")
            {


                string Fecha = ((ComboBoxItem)FechaDevolucionComboBox.SelectedItem).Content.ToString();

                if (Fecha == "Proximo")
                {
                    librosFiltrados = librosFiltrados
                        .OrderByDescending(libro => libro.FechaDevolucion) // Ordenar de mayor a menor
                        .ToList();
                }
                else if (Fecha == "Entregados")
                {
                    librosFiltrados = librosFiltrados
                        .OrderBy(libro => libro.FechaDevolucion) // Ordenar de menor a mayor
                        .ToList();
                }
            }


            // Asignamos los libros filtrados al DataGrid
            dataGridPrestamos.ItemsSource = librosFiltrados;


        }

        private void TipoPrestamoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void EstadoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void FechaDevolucionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void btnQuitarFiltros_Click(object sender, RoutedEventArgs e)
        {
            LimpiarFiltros();
        }

        public void LimpiarFiltros()
        {
            TipoPrestamoComboBox.SelectedIndex = -1;
            EstadoComboBox.SelectedIndex = -1;
            FechaDevolucionComboBox.SelectedIndex = -1;
        }


        private void btnPagar_Click_1(object sender, RoutedEventArgs e)
        {
            TextBox[] arr = new TextBox[1];

            // Asignar un TextBox al array
            arr[0] = txtIdPago;


            bool validacion = datos.VerifcarTextBox(arr);

            if (validacion == true)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Desea continuar con la finalizacion del prestanmo?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado == MessageBoxResult.Yes)
                {
                    
                }
            }
            else
            {
                MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}