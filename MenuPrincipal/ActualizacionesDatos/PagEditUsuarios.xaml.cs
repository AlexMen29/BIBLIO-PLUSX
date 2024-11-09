using MenuPrincipal.BD.Models;
using MenuPrincipal.BD.Services;
using MenuPrincipal.DatosGenerales;
using MenuPrincipal.MenuLibros;
using MenuPrincipal.PageUsuarios;
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
using System.Data.SqlClient;
using System.Drawing;


namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para PagEditUsuarios.xaml
    /// </summary>
    public partial class PagEditUsuarios : Page
    {
        private int id;
        DatosGlobales datosG = new DatosGlobales();
        DatosUsuariosModel datosUsuario = null;

        public PagEditUsuarios(int usuarioId)
        {
            this.id = usuarioId;
            InitializeComponent();
            ObtenerDatos();  // Llama al método para obtener y mostrar los datos al cargar la página
            CargarDatos();
            OcultarCarrera();
            DeshabilitarObjetos();
        }

        Usuarios metodosusuarios = new Usuarios();

        private void ObtenerDatos()
        {
            // Llama al método MostrarUsuarios con el ID de usuario y obtiene los datos
            DatosUsuariosMetodos metodo = new DatosUsuariosMetodos();

            DatosUsuariosModel usuario = metodo.MostrarUsuarios(id);

            // Verifica si se encontraron datos
            if (usuario != null)
            {
                // Asigna los datos a los campos correspondientes en la interfaz de usuario
                UsuarioIDtxt.Text = usuario.UsuarioID.ToString();
                NombresTxt.Text = usuario.Nombres;
                ApellidosTxt.Text = usuario.Apellidos;
                ApellidoCasadaTxt.Text = usuario.ApellidoCasada;
                CarnetTxt.Text = usuario.Carnet;
                Correo1Txt.Text = usuario.Correo1;
                Correo2Txt.Text = usuario.Correo2;
                Telefono1Txt.Text = usuario.Telefono1;
                Telefono2Txt.Text = usuario.Telefono2;
                TelefonoFijoTxt.Text = usuario.TelefonoFijo;

                ColoniaTxt.Text = usuario.Colonia;
                CalleTxt.Text = usuario.Calle;
                CasaTxt.Text = usuario.Casa;
                MunicipioTxt.Text = usuario.Municipio;
                DepartamentoTxt.Text = usuario.Departamento;
                CPTxt.Text = usuario.CP;

                datosUsuario = usuario;
            }
            else
            {
                MessageBox.Show("No se encontraron datos para el usuario especificado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void HabilitarObjetos()
        {
            NombresTxt.IsEnabled = true;
            ApellidosTxt.IsEnabled = true;
            EstadoCivilBox.IsEnabled = true;
            ApellidoCasadaTxt.IsEnabled = true;
            EstadoBox.IsEnabled = true;
            TipoUsuarioBox.IsEnabled = true;
            CarreraBox.IsEnabled = true;

            Correo1Txt.IsEnabled = true;
            Correo2Txt.IsEnabled = true;
            Telefono1Txt.IsEnabled = true;
            Telefono2Txt.IsEnabled = true;
            TelefonoFijoTxt.IsEnabled = true;

            ColoniaTxt.IsEnabled = true;
            CalleTxt.IsEnabled = true;
            CasaTxt.IsEnabled = true;
            MunicipioTxt.IsEnabled = true;
            DepartamentoTxt.IsEnabled = true;
            CPTxt.IsEnabled = true;
            btnGuardarUsuario.IsEnabled = true;

        }

        private void DeshabilitarObjetos()
        {
            UsuarioIDtxt.IsEnabled = false;
            NombresTxt.IsEnabled = false;
            ApellidosTxt.IsEnabled = false;
            EstadoCivilBox.IsEnabled = false;
            ApellidoCasadaTxt.IsEnabled = false;
            CarnetTxt.IsEnabled = false;
            EstadoBox.IsEnabled = false;
            TipoUsuarioBox.IsEnabled = false;
            CarreraBox.IsEnabled = false;

            Correo1Txt.IsEnabled = false;
            Correo2Txt.IsEnabled = false;
            Telefono1Txt.IsEnabled = false;
            Telefono2Txt.IsEnabled = false;
            TelefonoFijoTxt.IsEnabled = false;

            ColoniaTxt.IsEnabled = false;
            CalleTxt.IsEnabled = false;
            CasaTxt.IsEnabled = false;
            MunicipioTxt.IsEnabled = false;
            DepartamentoTxt.IsEnabled = false;
            CPTxt.IsEnabled = false;
            btnGuardarUsuario.IsEnabled = false;
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            HabilitarObjetos();
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DeshabilitarObjetos();
        }

        private void btnGuardarUsuario_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] arr = new TextBox[]
            {
                NombresTxt, ApellidosTxt,
                Correo1Txt, Telefono1Txt, ColoniaTxt, CalleTxt, CasaTxt, MunicipioTxt, DepartamentoTxt, CPTxt
            };

            // Verificar si todos los TextBox tienen texto
            bool validacion = datosG.VerifcarTextBox(arr);

            if (validacion == true)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Esa seguro que desea modficar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado == MessageBoxResult.Yes)
                {
                    DatosUsuariosMetodos metodos = new DatosUsuariosMetodos();
                    if (metodos.ModificarUsuario(datosEnviar()) == true)
                    {
                        MessageBox.Show("Modificacion exitosa", "Confirmación", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error inesperado, no se ha podido modficar", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);
                    }




                }
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void CargarDatos()
        {
            datosG.LlenarCajasVDefecto(datosG.consultaEstadoCivil, EstadoCivilBox, "EstadoCivil", datosUsuario.EstadoCivil);
            datosG.LlenarCajasVDefecto(datosG.consultaEstado, EstadoBox, "Estado", datosUsuario.Estado);
            datosG.LlenarCajasVDefecto(datosG.consultaTipoUsuario, TipoUsuarioBox, "Tipo", datosUsuario.TipoUsuario);
            datosG.LlenarCajasVDefecto(datosG.consultaCarrera, CarreraBox, "NombreCarrera", datosUsuario.Carrera);
        }

        private void OcultarCarrera()
        {
            if (TipoUsuarioBox.SelectedItem.ToString() == "Estudiante")
            {
                PanelCarrera.Visibility = Visibility.Visible;
            }
            else
            {
                PanelCarrera.Visibility = Visibility.Collapsed;
            }
        }

        private void TipoUsuarioBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OcultarCarrera();
        }

       


        private DatosUsuariosModel datosEnviar()

        {
            DatosUsuariosModel datos = new DatosUsuariosModel();

            datos.UsuarioID = datosUsuario.UsuarioID;
            datos.Nombres = NombresTxt.Text;
            datos.Apellidos = ApellidosTxt.Text;
            datos.EstadoCivil = EstadoCivilBox.SelectedItem.ToString();
            datos.ApellidoCasada = ApellidoCasadaTxt.Text;
            datos.Correo1 = Correo1Txt.Text;
            datos.Correo2 = Correo2Txt.Text;
            datos.Telefono1 = Telefono1Txt.Text;
            datos.Telefono2 = Telefono2Txt.Text;
            datos.TelefonoFijo = TelefonoFijoTxt.Text;
            datos.Carnet = CarnetTxt.Text;
            datos.EstadoID = datosG.ObtenerID("select EstadoID from Estado where Estado=@Valor", EstadoBox.SelectedItem.ToString());
            datos.TipoUsuarioId = datosG.ObtenerID("select TipoUsuarioID from TipoUsuario where Tipo = @Valor", TipoUsuarioBox.SelectedItem.ToString());
            //MessageBox.Show($"dato id {ObtenerID("select TipoUsuarioID from TipoUsuario where Tipo = @Valor", TipoUsuarioBox.SelectedItem.ToString())}");

            if (TipoUsuarioBox.SelectedItem.ToString() == "Estudiante" || TipoUsuarioBox.SelectedItem == null)
            {
                datos.CarreraID = datosG.ObtenerID("select CarreraID from Carrera where NombreCarrera=@Valor", CarreraBox.SelectedItem.ToString());

            }
            else
            {
                datos.CarreraID = 0;

            }
            datos.Colonia = ColoniaTxt.Text;
            datos.Calle = CalleTxt.Text;
            datos.Casa = CasaTxt.Text;
            datos.Municipio = MunicipioTxt.Text;
            datos.Departamento = DepartamentoTxt.Text;
            datos.CP = CPTxt.Text;

            MessageBox.Show($"EstadoID: {datos.EstadoID}, TipoUsuarioId: {datos.TipoUsuarioId}, CarreraID: {datos.CarreraID}");

            return datos;

        }

    }

}
