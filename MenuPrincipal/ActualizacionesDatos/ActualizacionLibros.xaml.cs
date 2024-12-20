﻿using MenuPrincipal.BD.Models;
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
using MenuPrincipal.CompraDeLibros;


namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para ActualizacionLibros.xaml
    /// </summary>
    public partial class ActualizacionLibros : Window
    {
        private DetallesLibros Libros;
        private CrearDatoLibroModel DatosCrearLibros;
        DatosGlobales datos = new DatosGlobales();
        MetodosCompras MetodosCompras = new MetodosCompras();

        public string IsbnEdicion;
        private int ModificarCrear;

        CrearDatoLibroModel datoLibroModel = new CrearDatoLibroModel();
        CompraLibros CompraLibros = new CompraLibros();


        public ActualizacionLibros(DetallesLibros Libros, int ModificarCrear)//0=Modficar y 1=Crear
        {
            InitializeComponent();


            this.ModificarCrear = ModificarCrear;

            this.Libros = Libros;
            this.IsbnEdicion = Libros.Edicion;
            LabTitulo.Content = "Editar Datos de Libro";

            CargarDatosModificar();

        }

        //Constructor para crear libros
        public ActualizacionLibros(CrearDatoLibroModel Libros, int ModificarCrear)//0=Modficar y 1=Crear
        {
            InitializeComponent();
            this.DatosCrearLibros = Libros;
            this.ModificarCrear = ModificarCrear;
            LabTitulo.Content = "Ingreso de Nuevo Libro";


            CargarDatosSeleccionar();
        }

        public void CargarDatosModificar()
        {
            CargarImgDes();
            EditTituloTextBox.Text = Libros.Titulo;
            EditEdicionTextBox.Text = Libros.Edicion;

            datos.LlenarCajasVDefecto(datos.consultaAutor, EditAutorComboBox, "NombreAutor", Libros.Autor);
            datos.LlenarCajasVDefecto(datos.consultaEdiorial, EditEditorialComboBox, "NombreEditorial", Libros.Editorial);
            datos.LlenarCajasVDefecto(datos.consultaCategoria, EditCategoriaComboBox, "NombreCategoria", Libros.Categoria);




            // Mostrar el panel de edición
        }

        public void CargarDatosSeleccionar()
        {
            datos.LlenarBoxFiltros(datos.consultaAutor, EditAutorComboBox, "NombreAutor");
            datos.LlenarBoxFiltros(datos.consultaEdiorial, EditEditorialComboBox, "NombreEditorial");
            datos.LlenarBoxFiltros(datos.consultaCategoria, EditCategoriaComboBox, "NombreCategoria");
            bntModificar.Content = "Finalizar Compra";

        }

        private void CargarImgDes()
        {
            // Llamada al método para obtener la imagen y la descripción
            List<object> ListImgDes = datos.ObtenerImgDescripcion(Libros.Edicion);

            // Asegurarse de que haya datos en la lista
            if (ListImgDes.Count >= 2)
            {
                // Asignar la descripción al TextBox
                EditDescripcionTextBox.Text = ListImgDes[1].ToString();

                // Manejar la imagen
                byte[] imagenBytes = ListImgDes[0] as byte[];
                if (imagenBytes != null)
                {
                    // Convertir los bytes a BitmapImage usando el método
                    BitmapImage imagen = datos.ConvertirABitmapImage(imagenBytes);

                    // Crear un ViewModel temporal para establecer la imagen
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

        //Cargar datos para modificacion de Autor, Editorial y categoria

        

        private void btnCargarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de imagen (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png"
            };

            // Mostrar el explorador de archivos
            if (openFileDialog.ShowDialog() == true)
            {
                // Obtener la ruta de la imagen seleccionada
                string rutaImagen = openFileDialog.FileName;

                byte[] imageBytes = File.ReadAllBytes(rutaImagen);

                // Reutilizar el método de la clase DatosGlobales para convertir los bytes a BitmapImage
                BitmapImage imagen = datos.ConvertirABitmapImage(imageBytes);

                // Asignar la imagen al control Image
                ImagePreview.Source = imagen;
            }
        }

        private void bntModificar_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] arr = new TextBox[3];

            // Asignar un TextBox al array
            arr[0] = EditTituloTextBox;
            arr[1]=EditDescripcionTextBox;
            arr[2]=EditEdicionTextBox;


            bool validacion = datos.VerifcarTextBox(arr);
            if (validacion == true && ImagePreview.Source!=null)
            {

                if (MessageBox.Show("Esta apunto de modificar ¿Desea contiuar?",
                     "Informacion",
                     MessageBoxButton.YesNo,
                     MessageBoxImage.Information) == MessageBoxResult.Yes)
                {

                    if (ModificarCrear == 0)
                    {
                        modifcar();
                    }
                    else
                    {
                        IngresarLibro();
                    }
                }

            }
            else
            {
                MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void modifcar()
        {
            ArrayList ids = ObtenerId();
            int AutorId = Convert.ToInt32(ids[0]);
            int EditorialId = Convert.ToInt32(ids[1]);
            int CategoriaId = Convert.ToInt32(ids[2]);
            int edicionId = Convert.ToInt32(ids[3]);

            byte[] imagenBytes = ConvertImageToByteArray(ImagePreview);

            int res = MetodosCRUD.ActualizarLibro(Libros.DetalleID, AutorId, EditorialId, CategoriaId, edicionId, IsbnEdicion, EditDescripcionTextBox.Text, EditTituloTextBox.Text, imagenBytes);

            if (res > 0)
            {
                MessageBox.Show("Actualizacion realizada exitosamente ", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                //Actualizacion de data grid con los nuevos datos



                //instancia donde se encuentra el elemento frame(aca mostramos todo)
                MainWindow mainWindow = (MainWindow)this.Owner;
                ((MainWindow)Application.Current.MainWindow).CargarLibros(1);
                ((MainWindow)Application.Current.MainWindow).CargarLibros(0);

                //cerramos
                //Abrimos ventana

                this.Close();


            }

        }


        private ArrayList ObtenerId() //0 Inexistente  diferen
        {

            ArrayList id = new ArrayList();
            id = MetodosDetallesLibros.ObtenerIdModLibros(EditAutorComboBox.SelectedItem.ToString(), EditEditorialComboBox.SelectedItem.ToString(), EditCategoriaComboBox.SelectedItem.ToString(), IsbnEdicion);
            if (id.Count > 0)
            {
                // Usar String.Join para concatenar los elementos de la lista en una cadena
                string valores = string.Join(", ", id.Cast<object>().Select(i => i.ToString()));

            }
            else
            {
                MessageBox.Show("No se encontraron valores.");
            }


            return id;

        }

        public static byte[] ConvertImageToByteArray(Image imageControl)
        {
            byte[] imageBytes = null;

            // Verifica si el Source de la imagen es un BitmapImage
            if (imageControl.Source is BitmapImage bitmapImage)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Crea un encoder para convertir la imagen a un arreglo de bytes
                    JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                    encoder.Save(memoryStream);

                    imageBytes = memoryStream.ToArray();
                }
            }

            return imageBytes;
        }

        #region Logica para Crear Libros

        private void IngresarLibro()
        {
            DatosCrearLibros.Titulo = EditTituloTextBox.Text;
            DatosCrearLibros.Descripcion = EditDescripcionTextBox.Text;
            DatosCrearLibros.ISBN = EditEdicionTextBox.Text;
            byte[] imagenBytes = ConvertImageToByteArray(ImagePreview);
            DatosCrearLibros.Imagen = imagenBytes;

            //obtener id correspondiente a datos seleccionado
            IsbnEdicion = DatosCrearLibros.ISBN;
            ArrayList ids = ObtenerId();
            int AutorId = Convert.ToInt32(ids[0]);
            int EditorialId = Convert.ToInt32(ids[1]);
            int CategoriaId = Convert.ToInt32(ids[2]);

            DatosCrearLibros.AutorID = AutorId;
            DatosCrearLibros.EditorialID = EditorialId;
            DatosCrearLibros.CategoriaID = CategoriaId;

            int res = MetodosCompras.CrearDatosLibros(DatosCrearLibros);

            if (res == 0)
            {
                MessageBox.Show("Creacion realizada exitosamente ", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
                
            }
        }

        public string RecuperarEdicion()
        {
            string edicion = DatosCrearLibros.ISBN;
            return edicion;
        }


        #endregion

    }
}