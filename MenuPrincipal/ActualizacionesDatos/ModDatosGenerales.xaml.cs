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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para ModDatosGenerales.xaml
    /// </summary>
    public partial class ModDatosGenerales : Window
    {
        public ModDatosGenerales()
        {
            InitializeComponent();

        }

        private void btnCarrera_Click(object sender, RoutedEventArgs e)
        {
            //frContenido
            PagEditCarrera pag = new PagEditCarrera();

            MainWindow mainWindow = (MainWindow)this.Owner;

            // Usa el método NavegarAContenido para mostrar la Page en el Frame de la ventana principal
            mainWindow.NavegarAContenido(pag);

            this.Close();
        }

        private void btnCategorias_Click(object sender, RoutedEventArgs e)
        {
            PagEditCategorias pag = new PagEditCategorias();

            MainWindow mainWindow = (MainWindow)this.Owner;

            // Usa el método NavegarAContenido para mostrar la Page en el Frame de la ventana principal
            mainWindow.NavegarAContenido(pag);

            this.Close();
        }

        private void btnEditoriales_Click(object sender, RoutedEventArgs e)
        {
            PagEditEditoriales pag = new PagEditEditoriales();

            MainWindow mainWindow = (MainWindow)this.Owner;

            // Usa el método NavegarAContenido para mostrar la Page en el Frame de la ventana principal
            mainWindow.NavegarAContenido(pag);

            this.Close();
        }

        private void btnAutores_Click(object sender, RoutedEventArgs e)
        {
            PagEditAutores pag = new PagEditAutores();

            MainWindow mainWindow = (MainWindow)this.Owner;

            // Usa el método NavegarAContenido para mostrar la Page en el Frame de la ventana principal
            mainWindow.NavegarAContenido(pag);

            this.Close();
        }

        private void btnProveedores_Click(object sender, RoutedEventArgs e)
        {
            PagEditProveedores pag = new PagEditProveedores();

            MainWindow mainWindow = (MainWindow)this.Owner;

            // Usa el método NavegarAContenido para mostrar la Page en el Frame de la ventana principal
            mainWindow.NavegarAContenido(pag);

            this.Close();
        }
    }
}