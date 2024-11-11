using System.Text;
using System.Collections.ObjectModel;
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
using MenuPrincipal.PageReport;
using Microsoft.Data.SqlClient;
using MenuPrincipal.MenuLibros;
using MenuPrincipal.PagePrestamos;
using MenuPrincipal.PageUsuarios;
using MenuPrincipal.ActualizacionesDatos;
using MenuPrincipal.CompraDeLibros;
using MenuPrincipal.DatosGenerales;


namespace MenuPrincipal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    

        public MainWindow()
        {
            InitializeComponent();

            HabilitarDeshabilitar();
            
        }

        private void Minimizar_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizarRestaurar_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximizarIcono.Text = "⬜";  // Cambiar el ícono a maximizar
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaximizarIcono.Text = "❐";  // Cambiar el ícono a restaurar
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void HabilitarDeshabilitar()
        {
            if (DatosGlobales.NivelUsuario==2)
            {
                btnActualizar.Visibility = Visibility.Collapsed;
                btnComprarLibros.Visibility = Visibility.Collapsed;
            }
        
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool Maximizado=false;
        private void Border_MouseLeftButtondDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (Maximizado)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    Maximizado = false;

                }

                else
                {
                    this.WindowState = WindowState.Maximized;
                 

                    Maximizado = true;

                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            CargarLibros(0);

        }

      

        public void CargarLibros(int Cerrar)
        {
            if (Cerrar == 1)
            {
                frContenido.NavigationService.Navigate(null);
            }
            else
            {
                PgLibros Page1 = new PgLibros(0);
                frContenido.NavigationService.Navigate(Page1);


                ImgLogo.Visibility = Visibility.Hidden;
            }
        }

        private void btnreport_Click(object sender, RoutedEventArgs e)
        {
            Report page2 = new Report();
            frContenido.NavigationService.Navigate(page2);
            ImgLogo.Visibility = Visibility.Hidden;
        }

        private void btnPrestamo_Click(object sender, RoutedEventArgs e)
        {
            Prestamos page3 = new Prestamos();
            frContenido.NavigationService.Navigate(page3);
            ImgLogo.Visibility = Visibility.Hidden;
        }

        private void btnUsuarios_Click(object sender, RoutedEventArgs e)
        {
            Usuarios Page4 = new Usuarios();
            frContenido.NavigationService.Navigate(Page4);
            ImgLogo.Visibility = Visibility.Hidden;
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            ModDatosGenerales MenuMod = new ModDatosGenerales();
            MenuMod.Owner = this;
            ImgLogo.Visibility = Visibility.Hidden;
            MenuMod.ShowDialog();
        }

        public void NavegarAContenido(Page pagina)
        {
             frContenido.Navigate(pagina);
        }

        private void btnComprarLibros_Click(object sender, RoutedEventArgs e)
        {
            CompraLibros pageCompraLibros = new CompraLibros();
            frContenido.NavigationService.Navigate(pageCompraLibros);
            ImgLogo.Visibility = Visibility.Hidden;
        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar cualquier contenido anterior en el Frame
             frContenido.Content = null;
            // Mostrar el Frame y la Image
            frContenido.Visibility = Visibility.Visible;
            ImgLogo.Visibility = Visibility.Visible;
        }
    }
}