using MenuPrincipal.PagePrestamos.Models;
using MenuPrincipal.PagePrestamos.service;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Linq;


namespace MenuPrincipal.PagePrestamos
{
    public partial class Prestamos : Page
    {
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
            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    conDB.Open();

                    // Cargar temas
                    SqlCommand cmdTema = new SqlCommand("SELECT TipoPrestamo FROM Prestamos", conDB);
                    SqlDataReader readerTema = cmdTema.ExecuteReader();
                    while (readerTema.Read())
                    {
                        comboBoxTipoPrestamo.Items.Add(readerTema["TipoPrestamo"].ToString());
                    }
                    readerTema.Close();

                    // Cargar autores
                    SqlCommand cmdAutor = new SqlCommand("SELECT Estado FROM PagosPrestamos", conDB);
                    SqlDataReader readerAutor = cmdAutor.ExecuteReader();
                    while (readerAutor.Read())
                    {
                        comboBoxPeriodoPago.Items.Add(readerAutor["Estado"].ToString());
                    }
                    readerAutor.Close();


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos en los ComboBox: " + ex.Message);
                }
            }
        }
        private void BtnGenerarClasificacion_Click(object sender, RoutedEventArgs e)
        {
            string tipoPrestamo = ((ComboBoxItem)comboBoxTipoPrestamo.SelectedItem)?.Content?.ToString();
            if (string.IsNullOrEmpty(tipoPrestamo))
            {
                MessageBox.Show("Por favor, selecciona un tipo de préstamo.");
                return;
            }
            // Código para filtrar préstamos según el tipo seleccionado
            var prestamosFiltrados = DatoPrestamo.CargarClasificacionPrestamos()
                .Where(p => p.TipoPrestamo == tipoPrestamo).ToList();
            dataGridPrestamos.ItemsSource = prestamosFiltrados;
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
    }
}