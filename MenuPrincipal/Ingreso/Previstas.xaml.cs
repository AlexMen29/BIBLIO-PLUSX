using MenuPrincipal.BD.Services;
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


using System.Data.SqlClient;
using System.Data;
using MenuPrincipal.MenuLibros;
using MenuPrincipal.DatosGenerales;

namespace MenuPrincipal.Ingreso
{
    /// <summary>
    /// Lógica de interacción para Previstas.xaml
    /// </summary>
    public partial class Previstas : Window
    {
        public Previstas()
        {
            InitializeComponent();
            Inicializar();
        }
         
        DatosGlobales datos= new DatosGlobales();


        private void Inicializar()
        {
            PgLibros Page1 = new PgLibros(1);
            frContenido.NavigationService.Navigate(Page1);
        }



        #region DECLARACION DE VARIABLES LOCALES
        //Conexion a la DB
        SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB);

        //variables para SQL Querys
        string consultaSQL = null;

        #endregion

        #region METODOS PERSONALIZADOS
        void EncontrarUsuario()
        {
            //Nos aseguramos que el txtContraseña tenga asignado el valor correspondiente
            if (txtPassword.Visibility == Visibility.Visible)
            {
                txtPassword.Password = txtPassword.Password.Trim();
            }
            else
            {
                txtPassword.Password = txtMostrarContraseña.Text;
            }


            if (MetodosCredenciales.EncontrarUsuario(txtCorreo, txtPassword) > 0)
            {
                //Enviar a id
                DatosGlobales.NivelUsuario = datos.ObtenerID(datos.consultaTipoUsuarioPorCarnet, txtCorreo.Text);


                MainWindow mainWindow = new MainWindow();
                Application.Current.MainWindow = mainWindow; // Asignar como la ventana principal
                mainWindow.Show();


                this.Close();

            }
            else
            {
                MessageBox.Show("Usuario no encontrado", "error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        #endregion

        private void Mostrar(object sender, RoutedEventArgs e)
        {
            txtMostrarContraseña.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtMostrarContraseña.Visibility = Visibility.Visible;
        }

        private void Ocultar(object sender, RoutedEventArgs e)
        {
            txtPassword.Password = txtMostrarContraseña.Text;
            txtPassword.Visibility = Visibility.Visible;
            txtMostrarContraseña.Visibility = Visibility.Collapsed;
        }

        private void BtnIngresar_Click(object sender, RoutedEventArgs e)
        {
            EncontrarUsuario();
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

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                EncontrarUsuario();
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private bool Maximizado = false;
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

        private void txtCorreo_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Verificar si se excede el límite de 6 caracteres
            if (textBox.Text.Length >= 6 && e.Key != Key.Back)
            {
                e.Handled = true;
            }
        }
    }
}