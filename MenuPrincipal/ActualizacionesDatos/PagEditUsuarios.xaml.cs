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

namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para PagEditUsuarios.xaml
    /// </summary>
    public partial class PagEditUsuarios : Page
    {
        private int id;
        DatosGlobales datos = new DatosGlobales();
        DatosUsuariosModel datosUsuario=null;

        public PagEditUsuarios(int usuarioId)
        {
            this.id = usuarioId;
            InitializeComponent();
            ObtenerDatos();  // Llama al método para obtener y mostrar los datos al cargar la página
            CargarDatos();
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
            bool validacion = datos.VerifcarTextBox(arr);

            if (validacion==true)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Esa seguro que desea modficar?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado == MessageBoxResult.Yes)
                {
                    // Aquí agregas los datos o el procesamiento necesario para guardar la información
                    // Ejemplo: 
                    // datos.Usuario.Nombre = NombresTxt.Text;
                    // datos.Usuario.Apellidos = ApellidosTxt.Text;
                    // Otros campos...

                    // Finalmente, habilitar el botón si todo es válido
                }
            }
            else
            {
                MessageBoxResult resultado = MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void CargarDatos()
        {
            datos.LlenarCajasVDefecto(datos.consultaEstadoCivil, EstadoCivilBox, "EstadoCivil",datosUsuario.EstadoCivil );
            datos.LlenarCajasVDefecto(datos.consultaEstado, EstadoBox, "Estado", datosUsuario.Estado);
            datos.LlenarCajasVDefecto(datos.consultaTipoUsuario, TipoUsuarioBox, "Tipo", datosUsuario.TipoUsuario);
            datos.LlenarCajasVDefecto(datos.consultaCarrera, CarreraBox, "NombreCarrera", datosUsuario.Carrera);



        }
    }

}
