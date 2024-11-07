using MenuPrincipal.ActualizacionesDatos;
using MenuPrincipal.BD;
using MenuPrincipal.BD.Models;
using MenuPrincipal.BD.Services;
using MenuPrincipal.DatosGenerales;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MenuPrincipal.CompraDeLibros
{
    /// <summary>
    /// Lógica de interacción para CompraLibros.xaml
    /// </summary>
    public partial class CompraLibros : Page
    {
        DatosGlobales datos = new DatosGlobales();



        public CompraLibros()
        {
            InitializeComponent();

            datos.LlenarBoxFiltros(datos.consultarEdicion, SeleccionarEdiccionBox, "ISBN");
            datos.LlenarBoxFiltros(datos.consultarProveedores, SelecionarProveedorBox, "NombreProveedor");

            DateFecha.SelectedDate = DateTime.Today;
            //Dato necesario si hacemos inserccion de dato no existente en la bd
            DateFechaInexistente.SelectedDate = DateTime.Today;
            datos.LlenarBoxFiltros(datos.consultarProveedores, SelecionarProveedorInexistenteBox, "NombreProveedor");


        }

        private decimal costoTotal;

        private void calcularTotal(TextBox Cantidad, TextBox costoUnidad, TextBox respuesta)
        {
            try
            {
                costoTotal = (Convert.ToDecimal(Cantidad.Text) * Convert.ToDecimal(costoUnidad.Text));
                respuesta.Text = $"${costoTotal}";

            }
            catch (Exception e)
            {
                respuesta.Text = "...";
            }
        }
        private void cantidadLibrotxt_SelectionChanged(object sender, RoutedEventArgs e)
        {
            calcularTotal(cantidadLibrotxt, costoUnidadtxt, costoTotaltxt);
        }

        private void costoUnidadtxt_SelectionChanged(object sender, RoutedEventArgs e)
        {
            calcularTotal(cantidadLibrotxt, costoUnidadtxt, costoTotaltxt);
        }

        private void btnComprar_Click(object sender, RoutedEventArgs e)
        {
            //id 0:Edicion ; id 1:Proveerdor; id 2: detalleLibro;
            int[] id = IdProveedorEdicionDetalle(SeleccionarEdiccionBox, SelecionarProveedorBox);

            ComprasModel datosCompra = new ComprasModel();

            //datosCompra.Cantidad=

            datosCompra.Cantidad = Convert.ToInt32(cantidadLibrotxt.Text);
            datosCompra.CostoUnidad = Convert.ToDecimal(costoUnidadtxt.Text);
            datosCompra.FechaCompra = DateFecha.SelectedDate.Value;
            datosCompra.CostoTotal = costoTotal;
            datosCompra.EdicionID = id[0];
            datosCompra.ProveedorID = id[1];
            datosCompra.DetallesID = id[2];


            int res = MetodosCompras.RegistrarCompraCompleta(datosCompra);

            if (res == 0)
            {
                MessageBox.Show("Compra registrada exitosamente con ID: " + res, "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("Hubo un problema al registrar la compra", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }





            MessageBox.Show($"id Edicion {id[0]} Proveedor {id[1]}Detalle{id[2]}");
        }


        private int[] IdProveedorEdicionDetalle(ComboBox edicion, ComboBox Proveedor)
        {
            int[] ids = new int[3];

            //obtenemos id correspondiente a edicion
            string consultaEdicion = "SELECT EdicionID FROM Ediciones WHERE ISBN = @ISBN";
            ids[0] = ObtenerIdLocal(consultaEdicion, "@ISBN", edicion, 0);


            //obtenemos id correspondiente a proveedor
            string consultaProveedores = "select ProveedorID from Proveedores where NombreProveedor=@Nombre";
            ids[1] = ObtenerIdLocal(consultaProveedores, "@Nombre", Proveedor, 0);

            string consultaDetalle = "select DetallesID from DetallesLibros where EdicionID=@ediccionID";
            int ediccionID = ids[0];

            //tercer parametro no se utiliza en la implemenatcion Selecionar proveedores no se utiliza
            ids[2] = ObtenerIdLocal(consultaDetalle, "@ediccionID", SelecionarProveedorBox, ediccionID);

            return ids;
        }
        private int ObtenerIdLocal(string consulta, string clave, ComboBox valor, int Edicion)
        {
            if (Edicion > 0)
            {
                string consultaSql = consulta;
                Dictionary<string, object> parametros = new Dictionary<string, object>
                {
                    { clave, Edicion } // Reemplaza con el ISBN correspondiente
                };

                // Llamar al método y obtener el resultado
                int id = MetodosCRUD.ObtenerId(consultaSql, parametros);
                return id;
            }
            else
            {
                string consultaSql = consulta;
                Dictionary<string, object> parametros = new Dictionary<string, object>
                {
                    { clave, valor.Text } // Reemplaza con el ISBN correspondiente
                };

                // Llamar al método y obtener el resultado
                int id = MetodosCRUD.ObtenerId(consultaSql, parametros);
                return id;
            }
        }

        private void SeleccionarEdiccionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string consulta = "select Titulo from Ediciones where ISBN=@Edicion";
                datos.AsignarValorTextBox(consulta, SeleccionarEdiccionBox.SelectedItem.ToString(), NombreLibrotxt, "@Edicion");


            }
            catch (Exception)
            {
                NombreLibrotxt.Text = "...";
            }
        }


        #region Libros inexistentes


        private void btnComprarInexsitente_Click(object sender, RoutedEventArgs e)
        {


            CrearDatoLibroModel datoLibroModel = new CrearDatoLibroModel();

            datoLibroModel.StockMinimo = Convert.ToInt32(stockMinimiotxt.Text);
            datoLibroModel.StockMaximo = Convert.ToInt32(stockMaximotxt.Text);

            MessageBox.Show($"Datos: {Convert.ToInt32(cantidadLibroInexistentetxt.Text)}");


            ActualizacionLibros ventana = new ActualizacionLibros(datoLibroModel, 1);
            ventana.ShowDialog();

            string isEd = ventana.RecuperarEdicion();
            if (isEd != null)
            {
                crearRegistroCompra(isEd);
            }
            else
            {
                MessageBox.Show("Compra Cancelada", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }




        }


        public void crearRegistroCompra(string idEd)
        {
            //Datos para crear registro de compra
            ComboBox idEdicion = new ComboBox();

            //Se crea un combo box ya que el metodo que recolecta los id lo necesita
            idEdicion.Items.Add(idEd);
            idEdicion.SelectedItem = idEd;

            int[] id = IdProveedorEdicionDetalle(idEdicion, SelecionarProveedorInexistenteBox);

            ComprasModel datosCompraI = new ComprasModel();

            //El programa se detiene cuando lee estos valores de las cajas de texto

            MessageBox.Show($"Datos: {Convert.ToInt32(cantidadLibroInexistentetxt.Text)}");

            //El error comienza aqui 

            datosCompraI.Cantidad = Convert.ToInt32(cantidadLibroInexistentetxt.Text);
            datosCompraI.CostoUnidad = Convert.ToDecimal(costoUnidadInexistentetxt.Text);
            datosCompraI.FechaCompra = DateFechaInexistente.SelectedDate.Value;
            datosCompraI.CostoTotal = costoTotal;
            datosCompraI.EdicionID = id[0];
            datosCompraI.ProveedorID = id[1];
            datosCompraI.DetallesID = id[2];

            int res = MetodosCompras.RegistrarCompraCompleta(datosCompraI);

            if (res == 0)
            {
                MessageBox.Show("Compra registrada exitosamente: ", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {
                MessageBox.Show("Hubo un problema al registrar la compra", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void cantidadLibroInexistentetxt_SelectionChanged(object sender, RoutedEventArgs e)
        {
            calcularTotal(cantidadLibroInexistentetxt, costoUnidadInexistentetxt, costoTotalInexistentetxt);

        }

        private void costoUnidadInexistentetxt_SelectionChanged(object sender, RoutedEventArgs e)
        {
            calcularTotal(cantidadLibroInexistentetxt, costoUnidadInexistentetxt, costoTotalInexistentetxt);

        }



        #endregion

    }
}