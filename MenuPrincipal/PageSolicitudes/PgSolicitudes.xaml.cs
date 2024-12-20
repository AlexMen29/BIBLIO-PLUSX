﻿using MaterialDesignThemes.Wpf;
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
using MenuPrincipal.PageReport.subpagereport;
using MenuPrincipal.PageReport.visualReports;

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
        int tipoPrestamoS;
        private int contador = 0;
        public DateTime? devolucion;
        public DateTime? prestamo;
        double costo = 0.0;
        public PgSolicitudes(string titulo, int tipoPrestamo)
        {
            this.titulo = titulo;
            this.tipoPrestamoS = tipoPrestamo;
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

            if (tipoPrestamoS == 1)
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
                if (tipoPrestamoS == 1)
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
                if (tipoPrestamoS == 1)
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
                if (tipoPrestamoS == 1)
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
                    int horas = (int)diferencia.TotalHours + 1;
                    switch (horas)
                    {
                        case 1: costo = 0.05; break;
                        case 2: costo = 0.10; break;
                        case 3: costo = 0.15; break;
                        case 4: costo = 0.20; break;
                        case 5: costo = 0.25; break;
                        default:
                            MessageBox.Show("El préstamo debe estar entre un rango de 1 - 5 horas a partir de ahora.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                    }
                }
            }
            else if (cmbPlazo.SelectedIndex == 1) // Plazo por días
            {
                if (txbFechaPrestamo.SelectedDate.HasValue && txbFechaDevolucionDias.SelectedDate.HasValue)
                {
                    TimeSpan diferencia = txbFechaDevolucionDias.SelectedDate.Value - txbFechaPrestamo.SelectedDate.Value;
                    int dias = diferencia.Days + 1;

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
                    int semanas = (int)(diferencia.TotalDays / 7 + 1);

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
            // Verificar longitud mínima del carnet
            int minLength = 6;
            if (txbCarne.Text.Length < minLength)
            {
                MessageBox.Show("Debe ingresar un carnet válido", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Obtener ID de usuario
            int respuesta = ObtenerID("select UsuarioID from Usuarios where Carnet=@Valor", txbCarne.Text);
            if (respuesta == -1)
            {
                MessageBox.Show("Carnet no encontrado", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Calcular el costo del préstamo
            CalcularCosto();
            if (costo <= 0)
            {
                MessageBox.Show("No se ha calculado el costo del préstamo.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Confirmar costo con el usuario
            MessageBoxResult boxResult = MessageBox.Show($"El costo del préstamo es: ${costo.ToString("F2")}\n¿Desea Continuar?", "Costo del Préstamo", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (boxResult != MessageBoxResult.Yes)
                return;

            // Variables para almacenar los datos del estudiante
            string nombre = "", apellido = "", carrera = "";

            // Conectar a la base de datos y obtener nombre, apellido y carrera según el Carnet
            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                string query = @"
            SELECT u.Nombres, u.Apellidos, c.NombreCarrera
            FROM Usuarios u
            JOIN InfoUsuarios iu ON u.UsuarioID = iu.TipoUsuarioID
            JOIN Carrera c ON iu.CarreraID = c.CarreraID
            WHERE u.Carnet = @Carne";

                SqlCommand command = new SqlCommand(query, conDB);
                command.Parameters.AddWithValue("@Carne", txbCarne.Text);

                try
                {
                    conDB.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        // Asignación de datos sin prefijos
                        nombre = reader["Nombres"].ToString();
                        apellido = reader["Apellidos"].ToString();
                        carrera = reader["NombreCarrera"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al obtener datos del estudiante: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            // Configurar datos de préstamo
            DateTime devolucion;
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

                // Registrar el préstamo
                metodos.RegistrarPrestamoCompleto(LlenarDatosBD());
                metodos.CrearRegistroPrestamo(costo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR INESPERADO: " + ex.Message);
                return;
            }

            // Crear el reporte y asignar los valores obtenidos
            SolicitudPrestamo rpt = new SolicitudPrestamo();
            libros dsLibros = new libros();
            DataTable dataTableLibros = dsLibros.Tables["DataTable7"];

            if (dataTableLibros == null)
            {
                MessageBox.Show("La tabla 'DataTable7' no existe en el DataSet.");
                return;
            }

            dataTableLibros.Clear();

            // Crear una nueva fila y asignar los valores de los controles, incluyendo el costo y datos del estudiante
            DataRow newRow = dataTableLibros.NewRow();
            newRow["Carne"] = txbCarne.Text;
            newRow["TipoPrestamo"] = cmbTipoPrestamo.SelectedItem != null ? ((ComboBoxItem)cmbTipoPrestamo.SelectedItem).Content.ToString() : "";
            newRow["Plazo"] = cmbPlazo.SelectedItem != null ? ((ComboBoxItem)cmbPlazo.SelectedItem).Content.ToString() : "";
            newRow["FechaSolicitud"] = txbFechaSolicitud.SelectedDate.HasValue ? txbFechaSolicitud.SelectedDate.Value.ToString("yyyy-MM-dd") : "";
            newRow["TituloLibro"] = lblTitulo.Content != null ? lblTitulo.Content.ToString() : "";
            newRow["AutorLibro"] = lblAutor.Content != null ? lblAutor.Content.ToString() : "";
            newRow["Costo"] = costo;

            // Asignar la fecha de devolución según el tipo de plazo
            if (cmbPlazo.SelectedIndex == 0)
            {
                newRow["HoraDevolucion"] = tmPickerDevolucion.SelectedTime.HasValue ? tmPickerDevolucion.SelectedTime.Value.ToString("HH:mm") : "";
            }
            else if (cmbPlazo.SelectedIndex == 1)
            {
                newRow["FechaDevolucionDias"] = txbFechaDevolucionDias.SelectedDate.HasValue ? txbFechaDevolucionDias.SelectedDate.Value.ToString("yyyy-MM-dd") : "";
            }
            else if (cmbPlazo.SelectedIndex == 2)
            {
                newRow["FechaDevolucionSemanas"] = txbFechaDevolucionSemanas.SelectedDate.HasValue ? txbFechaDevolucionSemanas.SelectedDate.Value.ToString("yyyy-MM-dd") : "";
            }

            // Asignar datos adicionales del estudiante: Nombre, Apellido y Carrera
            newRow["Nombre"] = nombre;
            newRow["Apellido"] = apellido;
            newRow["Carrera"] = carrera;

            // Agregar la nueva fila al DataTable
            dataTableLibros.Rows.Add(newRow);

            // Configurar y mostrar el reporte
            rpt.SetDataSource(dsLibros);
            rpt.Refresh();
            Window1 visor = new Window1();
            visor.reportelibros.ViewerCore.ReportSource = rpt;
            visor.ShowDialog();

            // Navegar a la página principal después del préstamo
            PgLibros Page1 = new PgLibros(0);
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.NavegarAContenido(Page1);
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
            datos.TiempoEspera = txbTiempo.Text;
            datos.FechaPrestamo = prestamo;
            datos.FechaDevolucion = devolucion;
            datos.TipoPrestamo = cmbTipoPrestamo.Text;
            datos.TiempoEntrega = txbTiempo.Text;
            datos.Renovaciones = 0;
            datos.FechaRenovacion = null;

            if (tipoPrestamoS == 2)
            {
                datos.EstadoSolicitud = "Pendiente";
                datos.EstadoPrestamo = "Pendiente";

            }
            else
            {
                datos.EstadoSolicitud = "Aprobada";
                datos.EstadoPrestamo = "Activo";
            }

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

        private void btnInformacion_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tarifas por préstamo\r\n\r\n" +
                "Por Hora: $0.05 por cada hora (hasta un máximo de 5 horas).\r\n\r\n" +
                "Por Día: $0.50 por cada día (hasta un máximo de 5 días).\r\n\r\n" +
                "Por Semana: $3.00 por semana, con un incremento de $1.00 " +
                "por cada semana adicional (hasta un máximo de 5 semanas). ", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
