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

namespace MenuPrincipal.DetallesL
{
    /// <summary>
    /// Interaction logic for Detalles.xaml
    /// </summary>
    public partial class Detalles : Window
    {
        private DetallesLibros Libros;
        DatosGlobales datos = new DatosGlobales();
        public Detalles()
        {
            InitializeComponent();
            //    this.Libros = Libros;
            //    CargarDatos();
        }

        //public void CargarDatos()
        //{
        //    CargarImgDes();
        //    lblTitulo.Content = Libros.Titulo;
        //    txbEdicion.Text = Libros.Edicion;

        //    LlenarCajas(datos.consultaAutor, EditAutorComboBox, "NombreAutor", Libros.Autor);
        //    LlenarCajas(datos.consultaEdiorial, EditEditorialComboBox, "NombreEditorial", Libros.Editorial);
        //    LlenarCajas(datos.consultaCategoria, EditCategoriaComboBox, "NombreCategoria", Libros.Categoria);

        //    // Mostrar el panel de edición
        //}
    }
}