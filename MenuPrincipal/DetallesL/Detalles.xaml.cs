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

using Microsoft.Win32; // Para OpenFileDialog
using System.IO;       // Para manejar streams
using System.Windows.Media.Imaging; // Para BitmapImage
using System.Data.SqlClient;
using System.Data;
using System.Collections;
using MenuPrincipal.DatosGenerales;
using System.Data.Common;
using MenuPrincipal.BD.Services;
using MenuPrincipal.MenuLibros;
using MenuPrincipal.BD.Models;
using System.Windows.Media.Media3D;
using MenuPrincipal.PageSolicitudes;

namespace MenuPrincipal.DetallesL
{
    /// <summary>
    /// Interaction logic for Detalles.xaml
    /// </summary>
    public partial class Detalles : Window
    {
        private ObtenerVistaCliente Libros;
        DatosGlobales datos = new DatosGlobales();
        public string titulo;
        int varStock;
        public Detalles(string titulo)
        {
            this.titulo = titulo;
            InitializeComponent();
            LlenarDatos();
            CargarImgDes();
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
                                    Editorial = dr["Editorial"].ToString(),
                                    Categoria = dr["Categoria"].ToString(),
                                    Edicion = dr["Edicion"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(), // Asegúrate de que este sea el tipo correcto
                                    StockActual = dr.GetInt32(dr.GetOrdinal("StockActual"))
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

        private void LlenarDatos() {
            ObtenerVistaCliente datosVista = DetallesLibros(titulo);

            try
            {
                if (datosVista != null)
                {
                    lblTitulo.Content = datosVista.Titulo;
                    txbAutor.Text = datosVista.Autor;
                    txbCategoria.Text = datosVista.Categoria;
                    txbDescripcion.Text = datosVista.Descripcion;
                    txbEdicion.Text = datosVista.Edicion;
                    txbEditorial.Text = datosVista.Editorial;
                    Libros = datosVista; //Cambio Correcto
                    lblStock.Content = ("Cantidad en Inventario: " + datosVista.StockActual);
                    varStock = datosVista.StockActual;


                }
            }
            catch(Exception e)
            {
                MessageBox.Show("Error inesperado ", e.ToString());
            }
        }

        private void bntPrestamo_Click(object sender, RoutedEventArgs e)
        {
            int tipoPrestamo;

            if (varStock > 0)
            {
                
                MessageBoxResult boxResult = MessageBox.Show("¿Deseas solicitar el libro: '" + titulo + "' ?", "Solicitud", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (boxResult == MessageBoxResult.Yes)
                {
                    tipoPrestamo = 1;
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    PgSolicitudes pg = new PgSolicitudes(titulo, tipoPrestamo);
                    mainWindow.NavegarAContenido(pg);
                    
                   
                   //MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    
                }
                this.Close();
            }
            else {
                
                MessageBoxResult boxResult = MessageBox.Show("¡Libro fuera de Stock! ¿Desea entrar a la cola de espera?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (boxResult == MessageBoxResult.Yes)
                {
                    tipoPrestamo = 2;
                    PgSolicitudes pg = new PgSolicitudes(titulo, tipoPrestamo);
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.NavegarAContenido(pg);
                }
                this.Close();
            }
        }
    }
}