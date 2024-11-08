using MenuPrincipal.BD.Models;
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

        public PagEditUsuarios(int usuarioId)
        {
            this.id = usuarioId;
            InitializeComponent();
            ObtenerDatos();  // Llama al método para obtener y mostrar los datos al cargar la página
            DeshabilitarObjetos();
        }

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
                EstadoCivilTxt.Text = usuario.EstadoCivil;
                ApellidoCasadaTxt.Text = usuario.ApellidoCasada;
                CarnetTxt.Text = usuario.Carnet;
                EstadoTxt.Text = usuario.Estado;
                TipoUsuarioTxt.Text = usuario.TipoUsuario;
                CarreraTxt.Text = usuario.Carrera;

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
            }
            else
            {
                MessageBox.Show("No se encontraron datos para el usuario especificado.", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        private void HabilitarObjetos()
        {
            UsuarioIDtxt.IsEnabled = true;
            NombresTxt.IsEnabled = true;
            ApellidosTxt.IsEnabled = true;
            EstadoCivilTxt.IsEnabled = true;
            ApellidoCasadaTxt.IsEnabled = true;
            CarnetTxt.IsEnabled = true;
            EstadoTxt.IsEnabled = true;
            TipoUsuarioTxt.IsEnabled = true;
            CarreraTxt.IsEnabled = true;

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
        }

        private void DeshabilitarObjetos()
        {
            UsuarioIDtxt.IsEnabled = false;
            NombresTxt.IsEnabled = false;
            ApellidosTxt.IsEnabled = false;
            EstadoCivilTxt.IsEnabled = false;
            ApellidoCasadaTxt.IsEnabled = false;
            CarnetTxt.IsEnabled = false;
            EstadoTxt.IsEnabled = false;
            TipoUsuarioTxt.IsEnabled = false;
            CarreraTxt.IsEnabled = false;

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
        }


    }

}
