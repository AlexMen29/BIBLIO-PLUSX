using MaterialDesignThemes.Wpf;
using MenuPrincipal.BD.Models;
using MenuPrincipal.DatosGenerales;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
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
using System.Data.SqlClient;
using System.Windows.Media.Animation;
using System.Text.RegularExpressions;
using MenuPrincipal.PageUsuarios;
using MenuPrincipal.BD.Services;
using MenuPrincipal.MenuLibros;

namespace MenuPrincipal.PageSolicitudes
{
    /// <summary>
    /// Interaction logic for PgSolicitudes.xaml
    /// </summary>
    public partial class PgSolicitudes : Page
    {
        private ObtenerVistaCliente Libros;
        DatosGlobales datos = new DatosGlobales();
        MetodosPrestamos metodos = new MetodosPrestamos();

        public string titulo;
        public int tipoPrestamo;
        private int contador = 0;
        public DateTime? devolucion;
        public DateTime? prestamo;
        double costo = 0.0;
        public PgSolicitudes(string titulo, int tipoPrestamo)
        {
            this.titulo = titulo;
            this.tipoPrestamo = tipoPrestamo;
            InitializeComponent();
            LlenarDatos();
            CargarImgDes();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            txbTiempo.Visibility = Visibility.Collapsed;
            txblTiempo.Visibility = Visibility.Collapsed;
            txbTiempo.IsEnabled = false;
            txbFechaPrestamo.SelectedDate = DateTime.Now;
            txbFechaSolicitud.SelectedDate = DateTime.Now;
            tmPickerPrestamo.SelectedTime = DateTime.Now;
            tmPickerDevolucion.SelectedTime = DateTime.Now;
            cmbPlazo.SelectedIndex = 0;
            cmbTipoPrestamo.SelectedIndex = 0;
            cmbPlazo.SelectedIndex = 0;
            txbTiempo.Text = "Inmediata";
            devolucion = tmPickerDevolucion.SelectedTime.Value;

            if (tipoPrestamo == 1)
            {
                txbFechaSolicitud.Visibility = Visibility.Collapsed;
                txblFechaSolicitud.Visibility = Visibility.Collapsed;
                
            }
            else
            {
                txbFechaPrestamo.Visibility = Visibility.Collapsed;
                txblFechaPrestamo.Visibility = Visibility.Collapsed;
                tmPickerPrestamo.Visibility = Visibility.Collapsed;
                btnPrestamo.Content = "Solicitar";
                
            }

        }
        private void cmbTipoPrestamo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTipoPrestamo.SelectedIndex == 1)
            {
                txblTiempo.Visibility = Visibility.Visible;
                txbTiempo.Visibility = Visibility.Visible;
                txbTiempo.Text = "1 - 3 días aprox";
                
            }
            else
            {
                txbTiempo.Visibility = Visibility.Collapsed;
                txblTiempo.Visibility = Visibility.Collapsed;
                txbTiempo.Text = "Inmediata";
            }
        }

        private void LlenarDatos()
        {
            ObtenerVistaCliente datosVista = DetallesLibros(titulo);

            try
            {
                if (datosVista != null)
                {
                    lblTitulo.Content = datosVista.Titulo;
                    lblAutor.Content = datosVista.Autor;
                    Libros = datosVista; //Cambio Correcto
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado ", e.ToString());
            }
        }

        private void cmbPlazo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPlazo.SelectedIndex == 0)
            {
                tmPickerDevolucion.Visibility = Visibility.Visible;
                txbFechaPrestamo.Visibility = Visibility.Collapsed;

                // Aquí se asegura que estos elementos permanezcan ocultos si tipoPrestamo != 1
                if (tipoPrestamo == 1)
                {
                    tmPickerPrestamo.Visibility = Visibility.Visible;
                    tmPickerPrestamo.SelectedTime = DateTime.Now;
                    prestamo = tmPickerPrestamo.SelectedTime.Value;
                }
                else
                {
                    tmPickerPrestamo.Visibility = Visibility.Collapsed;
                    prestamo = null;
                }

                txbFechaDevolucionDias.Visibility = Visibility.Collapsed;
                txbFechaDevolucionSemanas.Visibility = Visibility.Collapsed;
                txblFechaPrestamo.Text = "Hora del Prestamo";
                txblFechaDevolucion.Text = "Hora de Devolución";

            }
            else if (cmbPlazo.SelectedIndex == 1)
            {
                txbFechaDevolucionDias.Visibility = Visibility.Visible;

                // Misma lógica para que permanezcan colapsados si tipoPrestamo != 1
                if (tipoPrestamo == 1)
                {
                    txbFechaPrestamo.Visibility = Visibility.Visible;
                    prestamo = txbFechaPrestamo.SelectedDate.Value;
                }
                else
                {
                    txbFechaPrestamo.Visibility = Visibility.Collapsed;
                    prestamo = null;
                }

                txbFechaDevolucionSemanas.Visibility = Visibility.Collapsed;
                tmPickerDevolucion.Visibility = Visibility.Collapsed;
                tmPickerPrestamo.Visibility = Visibility.Collapsed;
                txbFechaDevolucionDias.SelectedDate = DateTime.Today;
                txbFechaDevolucionDias.DisplayDateStart = DateTime.Today;
                txbFechaDevolucionDias.DisplayDateEnd = txbFechaDevolucionDias.DisplayDateStart.Value.AddDays(5);
                txblFechaPrestamo.Text = "Fecha del Prestamo";
                txblFechaDevolucion.Text = "Fecha de Devolución";

            }
            else
            {
                txbFechaDevolucionSemanas.Visibility = Visibility.Visible;

                // Misma lógica aquí para asegurar colapso si tipoPrestamo != 1
                if (tipoPrestamo == 1)
                {
                    txbFechaPrestamo.Visibility = Visibility.Visible;
                    prestamo = txbFechaPrestamo.SelectedDate.Value;
                }
                else
                {
                    txbFechaPrestamo.Visibility = Visibility.Collapsed;
                    prestamo = null;
                }

                tmPickerDevolucion.Visibility = Visibility.Collapsed;
                txbFechaDevolucionDias.Visibility = Visibility.Collapsed;
                tmPickerPrestamo.Visibility = Visibility.Collapsed;
                txbFechaDevolucionSemanas.SelectedDate = DateTime.Today;
                txbFechaDevolucionSemanas.DisplayDateStart = DateTime.Today;
                txbFechaDevolucionSemanas.DisplayDateEnd = txbFechaDevolucionSemanas.DisplayDateStart.Value.AddDays(28);
                txblFechaPrestamo.Text = "Fecha del Prestamo";
                txblFechaDevolucion.Text = "Fecha de Devolución";

            }
        }


        private void txbCarne_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //e.Handled = !Regex.IsMatch(e.Text, "^[0-9]$");
        }

        private void txbCarne_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Verificar si se excede el límite de 6 caracteres
            if (textBox.Text.Length >= 6 && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }

        #region METODO
        public static ObtenerVistaCliente DetallesLibros(string titulo)
        {
            ObtenerVistaCliente datosVista = null;

            try
            {
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = conn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "SP_ObtenerVistaCliente"; // Asegúrate de que este sea el nombre correcto
                        command.Parameters.AddWithValue("@Titulo", titulo);

                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                datosVista = new ObtenerVistaCliente
                                {
                                    DetalleID = dr.GetInt32(dr.GetOrdinal("DetalleID")),
                                    Titulo = dr["Titulo"].ToString(),
                                    Autor = dr["Autor"].ToString(),
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al intentar obtener los libros: " + e.Message, "Validación", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return datosVista;
        }

        #endregion

        private void CargarImgDes()
        {
            List<object> ListImgDes = datos.ObtenerImgDescripcionPorTitulo(Libros.Titulo); //Cambio Correcto

            // Asegurarse de que haya datos en la lista
            if (ListImgDes != null) //Cambio Correcto
            {

                // Manejar la imagen
                byte[] imagenBytes = ListImgDes[0] as byte[];
                if (imagenBytes != null)
                {
                    // Convertir los bytes a BitmapImage usando el método
                    BitmapImage imagen = datos.ConvertirABitmapImage(imagenBytes);
                    var viewModel = new { ImageData = imagen };

                    // Asignar el DataContext de la ventana o del control que contiene la imagen
                    this.DataContext = viewModel;

                }
                else
                {
                    MessageBox.Show("No se encontró una imagen para esta edición.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("No se encontraron datos para esta edición.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CalcularCosto() {
            if (cmbPlazo.SelectedIndex == 0)
            {
                if (tmPickerPrestamo.SelectedTime.HasValue && tmPickerDevolucion.SelectedTime.HasValue)
                {
                    TimeSpan diferencia = tmPickerDevolucion.SelectedTime.Value - tmPickerPrestamo.SelectedTime.Value;
                    int horas = (int)diferencia.TotalHours;
                    switch (horas)
                    {
                        case 1: costo = 0.05; break;
                        case 2: costo = 0.10; break;
                        case 3: costo = 0.15; break;
                        case 4: costo = 0.20; break;
                        case 5: costo = 0.25; break;
                        default:
                            MessageBox.Show("El préstamo no puede exceder las 5 horas.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                    }
                }
            }
            else if (cmbPlazo.SelectedIndex == 1) // Plazo por días
            {
                if (txbFechaPrestamo.SelectedDate.HasValue && txbFechaDevolucionDias.SelectedDate.HasValue)
                {
                    TimeSpan diferencia = txbFechaDevolucionDias.SelectedDate.Value - txbFechaPrestamo.SelectedDate.Value;
                    int dias = diferencia.Days;

                    // Calcular costo basado en la tabla de "días"
                    switch (dias)
                    {
                        case 1: costo = 0.50; break;
                        case 2: costo = 1.00; break;
                        case 3: costo = 1.50; break;
                        case 4: costo = 2.00; break;
                        case 5: costo = 2.50; break;
                        default:
                            MessageBox.Show("El préstamo no puede exceder los 5 días.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                    }
                }
            }
            else if (cmbPlazo.SelectedIndex == 2) // Plazo por semanas
            {
                if (txbFechaPrestamo.SelectedDate.HasValue && txbFechaDevolucionSemanas.SelectedDate.HasValue)
                {
                    TimeSpan diferencia = txbFechaDevolucionSemanas.SelectedDate.Value - txbFechaPrestamo.SelectedDate.Value;
                    int semanas = (int)(diferencia.TotalDays / 7);

                    // Calcular costo basado en la tabla de "semanas"
                    switch (semanas)
                    {
                        case 1: costo = 3.00; break;
                        case 2: costo = 4.00; break;
                        case 3: costo = 5.00; break;
                        case 4: costo = 6.00; break;
                        case 5: costo = 7.00; break;
                        default:
                            MessageBox.Show("El préstamo no puede exceder las 5 semanas.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                    }
                }

            }
        }

        private void btnPrestamo_Click(object sender, RoutedEventArgs e)
        {
            int minLength = 6;
            if (txbCarne.Text.Length < minLength)
            {
                // Mostrar un mensaje de error o cambiar alguna propiedad visual
                MessageBox.Show("Debe ingresar un carnet válido",
                       "Error",
                       MessageBoxButton.OK,
                       MessageBoxImage.Warning);
            }
            else
            {
                int respuesta = ObtenerID("select UsuarioID from Usuarios where Carnet=@Valor", txbCarne.Text);
                if (respuesta == -1)
                {
                    MessageBox.Show("Carnet no encontrado: " + respuesta, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else {        
                    CalcularCosto();
                    if (costo > 0)
                    {
                        MessageBoxResult boxResult = MessageBox.Show($"El costo del préstamo es: ${costo.ToString("F2")}\n¿Desea Continuar?", "Costo del Préstamo", MessageBoxButton.YesNo, MessageBoxImage.Information);

                        if (boxResult == MessageBoxResult.Yes)
                        {
                            try
                            {
                                if (cmbPlazo.SelectedIndex == 0)
                                {
                                    devolucion = tmPickerDevolucion.SelectedTime.Value;
                                }
                                else if (cmbPlazo.SelectedIndex == 1)
                                {
                                    devolucion = txbFechaDevolucionDias.SelectedDate.Value;
                                }
                                else
                                {
                                    devolucion = txbFechaDevolucionSemanas.SelectedDate.Value;
                                }

                                metodos.RegistrarPrestamoCompleto(LlenarDatosBD());
                                MessageBox.Show($"prestamo: {prestamo}\ndevolucion: {devolucion}");
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("ERROR INESPERADO: " + ex);
                            }
                        }
                        PgLibros Page1 = new PgLibros(0);
                        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                        mainWindow.NavegarAContenido(Page1);
                    }
                }
            }

        }

        private void tmPickerDevolucion_SelectedTimeChanged(object sender, RoutedPropertyChangedEventArgs<DateTime?> e)
        {
            try
            {
                devolucion = tmPickerDevolucion.SelectedTime.Value;
            }
            catch (Exception error)
            {
                //MessageBox.Show("Tu error " + error);

            }
        
        }

        private IngresoPrestamo LlenarDatosBD()
        {
            IngresoPrestamo datos = new IngresoPrestamo();

            datos.UsuarioId = ObtenerID("select UsuarioID from Usuarios where Carnet=@Valor", txbCarne.Text);
            datos.LibroId = ObtenerID("SELECT TOP 1 Libros.LibroID FROM Ediciones JOIN DetallesLibros ON Ediciones.EdicionID = DetallesLibros.EdicionID JOIN Libros ON DetallesLibros.DetallesID = Libros.DetallesID WHERE Ediciones.Titulo = @Valor;", titulo);
            datos.FechaSolicitud = txbFechaSolicitud.SelectedDate.Value;
            datos.EstadoSolicitud = "Aprobada";
            datos.TiempoEspera = txbTiempo.Text;
            datos.FechaPrestamo = prestamo;
            datos.FechaDevolucion = devolucion;
            datos.EstadoPrestamo = "Activo";
            datos.TipoPrestamo = cmbTipoPrestamo.Text;
            datos.TiempoEntrega = txbTiempo.Text;
            datos.Renovaciones = 0;
            datos.FechaRenovacion = null;

            return datos;
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

        

        private void txbFechaDevolucionDias_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            devolucion = txbFechaDevolucionDias.SelectedDate.Value;
        }

        private void txbFechaDevolucionSemanas_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            devolucion = txbFechaDevolucionSemanas.SelectedDate.Value;
        }
    }
}
