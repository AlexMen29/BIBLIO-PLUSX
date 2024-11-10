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
using MaterialDesignThemes.Wpf;
using MenuPrincipal.PageUsuarios;
using System.Data;
using System.Runtime.CompilerServices;


namespace MenuPrincipal.PagePrestamos
{
    public partial class Prestamos : Page
    {
        ColaModel LibroData;

        MetodosPrestamos metodoPrestamo = new MetodosPrestamos();
        DatosGlobales datos = new DatosGlobales();
        DatoCola metodoModificar = new DatoCola();

        public string estadoPrestamo = "Activo";
        public DateTime fechaPrestamo = DateTime.Now;
        private string valorSolicitudId;
        private int stockActual;

        public Prestamos()
        {
            InitializeComponent();

            //Verifca si algun pago esta atrasado y modifca su estado
            CargarClasificacionPrestamos();
            CargarDatosComboBox();
            dataGridCola.ItemsSource = DatoCola.MostrarDatosCola();
            ActualizarEstado();
        }

        private void CargarClasificacionPrestamos()
        {
            var prestamosAprobados = DatoPrestamo.CargarClasificacionPrestamos()
            .Where(p => p.EstadoSolicitud == "Aprobada")
            .ToList();

            dataGridPrestamos.ItemsSource = prestamosAprobados;

        }


        private void CargarDatosComboBox()
        {
            datos.LlenarBoxFiltros(datos.consultaTiposPrestamos, TipoPrestamoComboBox, "TipoPrestamo");
            datos.LlenarBoxFiltros(datos.consultaEstadoP, EstadoComboBox, "Estado");
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
                    .Where(libro => libro.Estado.Equals(EstadoSeleccionado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }



            if (FechaDevolucionComboBox.SelectedItem != null && ((ComboBoxItem)FechaDevolucionComboBox.SelectedItem).Content.ToString() != "Ninguno")
            {


                string Fecha = ((ComboBoxItem)FechaDevolucionComboBox.SelectedItem).Content.ToString();

                if (Fecha == "Más Próximo")
                {
                    librosFiltrados = librosFiltrados
                        .OrderByDescending(libro => libro.FechaDevolucion) // Ordenar de mayor a menor
                        .ToList();
                }
                else if (Fecha == "Más Antiguo")
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

        private ColaModel AprobarSolicitud() {

            ColaModel aprobacion = new ColaModel();

            aprobacion.SolicitudId = ObtenerID("select SolicitudID from Solicitudes WHERE SolicitudID = @valor", valorSolicitudId);
            aprobacion.PrestamoId = ObtenerID("select PrestamoID from Prestamos WHERE SolicitudID = @valor", valorSolicitudId);
            aprobacion.EstadoPrestamo = "Activo";
            aprobacion.EstadoSolicitud = "Aprobada";
            aprobacion.FechaPrestamo = DateTime.Now;
            return aprobacion;


        }



        private void ActualizarEstado()
        {

            try
            {
                // Lista para almacenar los ID de los préstamos
                List<int> idPrestamos = new List<int>();

                // Recorremos las filas del DataGrid
                foreach (var item in dataGridPrestamos.Items)
                {
                    // Hacemos un casting del item a la clase de datos que usas para enlazar el DataGrid (ajústalo al tipo de tu modelo, por ejemplo "Prestamo")
                    var prestamo = item as PrestamoModel;  // Cambia "Prestamo" por la clase que tienes

                    if (prestamo != null && prestamo.Estado != "Pagada")
                    {
                        // Si el estado no es "Pagada", agregamos el ID a la lista
                        idPrestamos.Add(prestamo.PrestamoId);
                    }
                }

                foreach (int PrestamoID in idPrestamos)
                {
                    metodoModificar.ModificarAtrasado(PrestamoID);
                }
                //Actualizar el dataGrid por si se modifico
                CargarClasificacionPrestamos();
            }
            catch (Exception e)
            {
                MessageBox.Show("Fallo en la actualizacion de estados");
            }

        }


        //Verifcar que id sea valido segun estado()
        private void HabilitarPagar()
        {
            try
            {
                // Lista para almacenar los ID de los préstamos con estado "Pagada"
                List<int> idPrestamosPagados = new List<int>();

                // Recorremos las filas del DataGrid
                foreach (var item in dataGridPrestamos.Items)
                {
                    // Hacemos un casting del item a PrestamoModel
                    var prestamo = item as PrestamoModel;

                    if (prestamo != null && prestamo.Estado == "Pagado")
                    {
                        // Si el estado es "Pagada", agregamos el ID a la lista
                        idPrestamosPagados.Add(prestamo.PrestamoId);
                    }
                }

                // Revisa si el valor de txtIdPago.Text es uno de los ID en la lista
                if (int.TryParse(txtIdPago.Text, out int idPago))
                {
                    // Deshabilitar el botón si el ID se encuentra en la lista de pagos
                    btnPagar.IsEnabled = !idPrestamosPagados.Contains(idPago);
                }
                else
                {
                    // Si el valor de txtIdPago.Text no es un número válido, habilitamos el botón
                    btnPagar.IsEnabled = true;
                }
            }
            catch (Exception e)
            {
                // Captura y muestra el error inesperado
                MessageBox.Show("Error inesperado: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                btnPagar.IsEnabled = true; // Aseguramos que el botón esté habilitado en caso de error
            }
        }




        private void btnPagar_Click_1(object sender, RoutedEventArgs e)
        {
            TextBox[] arr = new TextBox[1];

            // Asignar un TextBox al array
            arr[0] = txtIdPago;


            bool validacion = datos.VerifcarTextBox(arr);
            if (txtCosto.Text == "N/A")
            {
                MessageBox.Show("ID ingreado no aplica para esta operacion", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtIdPago.Text = null;
            }
            else
            {
                if (validacion == true)
                {
                    MessageBoxResult resultado = MessageBox.Show("¿Desea continuar con la finalizacion del prestanmo?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (resultado == MessageBoxResult.Yes)
                    {


                        if (metodoPrestamo.ActualizarEstadoPago(Convert.ToInt32(txtIdPago.Text)) == true)
                        {
                            MessageBox.Show("Pago Realizado", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                            CargarClasificacionPrestamos();

                        }
                        else
                        {
                            MessageBox.Show("Error Inesperado, no se ha podido procesar el pago", "Error", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

                }
            }

        }

        private void dataGridCola_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblLibro.Content = null;
            LibroData = (ColaModel)dataGridCola.SelectedItem;

            if (LibroData == null)
            {
                return;
            }

            lblLibro.Content += $"Solicitud Seleccionada: {LibroData.Titulo}";


            valorSolicitudId = LibroData.SolicitudId.ToString();
            stockActual = int.Parse(LibroData.StockActual.ToString());

            if (stockActual < 1)
            {
                btnAprobar.IsEnabled = false;
            }
            else {
                btnAprobar.IsEnabled = true;
            }
        }

        private void btnAprobar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult boxResult = MessageBox.Show($"¿Desea aprobar la solicitud del libro: {LibroData.Titulo}", "Aprobación", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (boxResult == MessageBoxResult.Yes)
            {
                metodoModificar.ModificarEstadoFecha(AprobarSolicitud());
            }
            dataGridCola.ItemsSource = DatoCola.MostrarDatosCola();
        }
        public int ObtenerID(string consultaSQL, string valor)
        {
            int id = -1; // Valor inicial del ID, en caso de que no se encuentre

            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.conexionDB))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(consultaSQL, connection))
                    {
                        // Agregar el parámetro @valor con el valor proporcionado
                        command.Parameters.AddWithValue("@valor", valor);

                        // Ejecutar la consulta y obtener el resultado
                        object result = command.ExecuteScalar();

                        // Si el resultado no es nulo, lo convertimos a entero
                        if (result != null && int.TryParse(result.ToString(), out int parsedID))
                        {
                            id = parsedID;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener el ID: " + ex.Message);
                }
            }

            return id;
        }

        private void txtIdPago_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {




                if (txtIdPago != null)
                {
                    decimal res = metodoPrestamo.CalcularCostoPrestamo(Convert.ToInt32(txtIdPago.Text));
                    if (res > 0)
                    {
                        txtCosto.Text = "$" + res.ToString();
                        HabilitarPagar();
                    }
                    else
                    {
                        txtCosto.Text = "N/A";
                    }
                }
            }
            catch (Exception eX)
            {
                txtCosto.Text = "N/A";
            }

        }

      


        private void dataGridPrestamos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //dataGridPrestamos

            PrestamoModel datos = (PrestamoModel)dataGridPrestamos.SelectedItem;

            if (datos == null)
            {
                return;
            }

            txtIdPago.Text = datos.PrestamoId.ToString();
            txtIdPagoR.Text = datos.PrestamoId.ToString();

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            txtIdPago.Text=null;
        }
    }
}