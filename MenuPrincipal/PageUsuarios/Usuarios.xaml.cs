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
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using MenuPrincipal.BD.Models;
using MenuPrincipal.MenuLibros;
using MenuPrincipal.BD.Services;
using MenuPrincipal.DatosGenerales;
using System.Collections;
using MenuPrincipal.ActualizacionesDatos;

namespace MenuPrincipal.PageUsuarios
{
    /// <summary>
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class Usuarios : Page
    {

        DatosGlobales datos = new DatosGlobales();

        public List<DetallesUsuarios> ListaDataGrid;
        int idActual;
        int AdminOrEstudiante=0;
        DetallesUsuarios UsuariosData;

        public Usuarios()
        {
            InitializeComponent();
            CargarDatos();


            LlenarBoxFiltros(datos.consultaTipoUsuario, TipoUsuarioComboBox, "Tipo");
            LlenarBoxFiltros(datos.consultaCarrera, CarreraComboBox, "NombreCarrera");
            LlenarBoxFiltros(datos.consultaEstado, estadoComboBox, "Estado");
            LlenarBoxFiltros(datos.consultaEstadoCivil, estadoCivilBox, "EstadoCivil");
            LlenarBoxFiltros(datos.consultaTipoUsuario, tipoUsuarioComboBox, "Tipo");
            LlenarBoxFiltros(datos.consultaCarrera, carreraComboBox, "NombreCarrera");
            LlenarBoxFiltros(datos.consultaEstado, estadoComboBox2, "Estado");
        }


        public void CargarDatos()
        {
            ListaDataGrid = MetodoDetallesUsuarios.MostrarUsuarios();
            UsuariosDataGrid.ItemsSource = ListaDataGrid;

        }

        public void LlenarBoxFiltros(string consulta, ComboBox elementoBox, string columna)
        {
            try
            {
                //Lista con valores correspondientes a ComboBox
                List<string> Lista = new List<string>();
                using (var conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conn.Open();

                    using (var command = new SqlCommand(consulta, conn))
                    {
                        using (DbDataReader dr = command.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                Lista.Add(dr[columna].ToString());
                            }
                        }
                    }
                }


                elementoBox.ItemsSource = Lista; // Asigna la lista al ComboBox



            }
            catch (Exception e)
            {
                MessageBox.Show($"Error inesperado: {e.Message}");
            }
        }

        public void AplicarFiltro()
        {
            List<DetallesUsuarios> usuariosFiltrados = MetodoDetallesUsuarios.MostrarUsuarios();

            // Filtramos por tipo de usuario si hay un valor seleccionado
            if (TipoUsuarioComboBox.SelectedItem != null && TipoUsuarioComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string TipoUsuarioSeleccionado = TipoUsuarioComboBox.SelectedItem.ToString();
                usuariosFiltrados = usuariosFiltrados
                    .Where(usuario => usuario.TipoUsuario.Equals(TipoUsuarioSeleccionado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }



            // Filtramos por carrera si hay un valor seleccionado
            if (CarreraComboBox.SelectedItem != null && CarreraComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string CarreraSeleccionada = CarreraComboBox.SelectedItem.ToString();
                usuariosFiltrados = usuariosFiltrados
                    .Where(usuario => usuario.Carrera.Equals(CarreraSeleccionada, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Filtramos por estado si hay un valor seleccionado
            if (estadoComboBox.SelectedItem != null && estadoComboBox.SelectedItem.ToString() != "Ninguno")
            {
                string estadoSeleccionado = estadoComboBox.SelectedItem.ToString();
                usuariosFiltrados = usuariosFiltrados
                    .Where(usuario => usuario.Estado.Equals(estadoSeleccionado, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            if (FechaComboBox.SelectedItem != null && ((ComboBoxItem)FechaComboBox.SelectedItem).Content.ToString() != "Ninguno")
            {


                string FechaSeleccionada = ((ComboBoxItem)FechaComboBox.SelectedItem).Content.ToString();

                if (FechaSeleccionada == "Mas Reciente")
                {
                    usuariosFiltrados = usuariosFiltrados
                        .OrderByDescending(usuario => usuario.FechaRegistro) // Ordenar de mayor a menor
                        .ToList();
                }
                else if (FechaSeleccionada == "Mas Antiguo")
                {
                    usuariosFiltrados = usuariosFiltrados
                        .OrderBy(usuario => usuario.FechaRegistro) // Ordenar de menor a mayor
                        .ToList();
                }
            }

            // Asignamos los libros filtrados al DataGrid
            if (usuariosFiltrados.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios que coincidan con los criterios de búsqueda.");
                CargarDatos();
                LimpiarCajas();

            }
            else
            {
                UsuariosDataGrid.ItemsSource = usuariosFiltrados;
            }






        }



        private void UsuariosDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            labSeleccion.Content = null;
            UsuariosData = (DetallesUsuarios)UsuariosDataGrid.SelectedItem;

            if (UsuariosData == null)
            {
                return;
            }

            labSeleccion.Content += $"Elemento Seleccionado: {UsuariosData.Nombres}";
            idActual = UsuariosData.UsuarioID;
            CarnetTextBox.Text = UsuariosData.Carnet;
        }

        private void TipoUsuarioComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void CarreraComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void FechaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }
        private void estadoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AplicarFiltro();
        }

        private void BuscarUsuarios()
        {
            // Obtenemos la lista completa de usuarios
            List<DetallesUsuarios> usuariosFiltrados = MetodoDetallesUsuarios.MostrarUsuarios();

            // Filtramos la lista por el carnet ingresado en el TextBox
            string carnet = CarnetTextBox.Text;
            if (!string.IsNullOrEmpty(carnet))
            {
                usuariosFiltrados = usuariosFiltrados
                    .Where(usuario => usuario.Carnet.IndexOf(carnet, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            // Asignamos los usuarios filtrados al DataGrid
            if (usuariosFiltrados.Count == 0)
            {
                MessageBox.Show("No se encontraron usuarios que coincidan con el criterio de búsqueda.");
                LimpiarCajas();
                UsuariosDataGrid.ItemsSource = MetodoDetallesUsuarios.MostrarUsuarios();

            }
            else
            {

                UsuariosDataGrid.ItemsSource = usuariosFiltrados;

                //Id de usuario encontrado
                //DetallesUsuarios obtenerID = (DetallesUsuarios)UsuariosDataGrid.SelectedItem;
                //idActual=obtenerID.UsuarioID;
                idActual = Convert.ToInt32(usuariosFiltrados[0].UsuarioID);

                PagEditUsuarios page = new PagEditUsuarios(idActual);

                frContenidoUsuarios.NavigationService.Navigate(page);
                //ImgLogo.Visibility = Visibility.Hidden;
            }
        }


        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {

            // Declarar un array de TextBox
            TextBox[] arr = new TextBox[1];

            // Asignar un TextBox al array
            arr[0] = CarnetTextBox;


            bool validacion = datos.VerifcarTextBox(arr);

            if (validacion == true)
            {
                BuscarUsuarios();

            }
            else

            {
                MessageBoxResult resultado = MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }


        }

        public void btnQuitarFiltros_Click(object sender, RoutedEventArgs e)
        {

            CargarDatosLimpiarframe();

        }

        private void LimpiarCajas()
        {
            // Limpiar las selecciones de los ComboBox
            TipoUsuarioComboBox.SelectedItem = null;
            CarreraComboBox.SelectedItem = null;
            FechaComboBox.SelectedItem = null;
            estadoComboBox.SelectedItem = null;
            CarnetTextBox.Text = null;
        }

        public void CargarDatosLimpiarframe()
        {
            // Quitar la página del Frame +
            frContenidoUsuarios.Content = null;
            LimpiarCajas();
            CargarDatos();
        }

        #region Agregar Usuarios



        private void btnAgregarUsuario_Click(object sender, RoutedEventArgs e)
        {
            // Declarar un array de TextBox
            TextBox[] arrCajas = new TextBox[11];
            ComboBox[] arrCombo = new ComboBox[3];

            // Asignar TextBox al array
            arrCajas[0] = nombrestxt;
            arrCajas[1] = apellidostxt;
            arrCajas[2] = Carnettxt;
            arrCajas[3] = correo1txt;
            arrCajas[4] = telefono1txt;
            arrCajas[5] = coloniatxt;
            arrCajas[6] = calletxt;
            arrCajas[7] = casatxt;
            arrCajas[8] = municipiotxt;
            arrCajas[9] = departamentotxt;
            arrCajas[10] = cptxt;


            // Asignar Combobox al array
            arrCombo[0] = estadoCivilBox;
            arrCombo[1] = tipoUsuarioComboBox;
            arrCombo[2] = estadoComboBox2;


            bool validacionCajas = datos.VerifcarTextBox(arrCajas);
            bool validacionCombo = datos.VerifcarComboBox(arrCombo);

            if (validacionCajas == true && validacionCombo == true)
            {
                MessageBoxResult resultado = MessageBox.Show("¿Esta seguro de agregar el usuario?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (resultado == MessageBoxResult.Yes)
                {
                    // Suponiendo que tienes controles de entrada para cada dato en tu formulario
                    string nombres = nombrestxt.Text;
                    string apellidos = apellidostxt.Text;
                    string estadoCivil = estadoCivilBox.Text;
                    string ApellidoCasada = nombreCasadatxt.Text;
                    DateTime fechaRegistro = DateFechaRegistro.SelectedDate ?? DateTime.Now;

                    string correo1 = correo1txt.Text;
                    string correo2 = correo2txt.Text;
                    string telefono1 = telefono1txt.Text;
                    string telefono2 = telefono2txt.Text;
                    string telefonoFijo = telefonoFijotxt.Text;

                    string colonia = coloniatxt.Text;
                    string calle = calletxt.Text;
                    string casa = casatxt.Text;
                    string municipio = municipiotxt.Text;
                    string departamento = departamentotxt.Text;
                    string cp = cptxt.Text;

                    string clave = contraseñatxt.Text;
                    string carnet = Carnettxt.Text;

                    ComboBox tipoUsuarioBox = tipoUsuarioComboBox;
                    ComboBox estadoBox = estadoComboBox2;
                    ComboBox carreraBox = carreraComboBox;

                    AgregarUsuario(
                        nombres, apellidos, estadoCivil, ApellidoCasada, fechaRegistro,
                        correo1, correo2, telefono1, telefono2, telefonoFijo,
                        colonia, calle, casa, municipio, departamento, cp, clave,
                        carnet, tipoUsuarioBox, estadoBox, carreraBox
                    );
                    CargarDatos();

                    MessageBox.Show("Datos guardados exitosamente.");

                }

            }
            else

            {
                MessageBoxResult resultado = MessageBox.Show("Datos Incompletos, por favor complete los campos requeridos", "Informacion", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        private void estadoCivilBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LlenarBoxFiltros(datos.consultaEstadoCivil, estadoCivilBox, "EstadoCivil");
            if (estadoCivilBox.SelectedItem?.ToString() != "Soltero")
            {
                nombreCasadatxt.IsEnabled = true;

            }
            else
            {
                nombreCasadatxt.IsEnabled = false;
            }
        }

        private void tipoUsuarioBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LlenarBoxFiltros(datos.consultaTipoUsuario, tipoUsuarioComboBox, "Tipo");
            // Obtener el valor seleccionado del ComboBox como texto.

            if (tipoUsuarioComboBox.SelectedItem?.ToString() != "Estudiante")
            {
                carreraComboBox.IsEnabled = false;
                AdminOrEstudiante = 1;
                carreraComboBox.SelectedValue = "N/A";
                contraseñatxt.IsEnabled = true;

            }
            else
            {
                AdminOrEstudiante = 0;
                carreraComboBox.IsEnabled = true;
                contraseñatxt.IsEnabled = false;
            }
        }

        private void carreraBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LlenarBoxFiltros(datos.consultaCarrera, carreraComboBox, "NombreCarrera");
        }

        private void estadoBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LlenarBoxFiltros(datos.consultaEstado, estadoComboBox2, "Estado");
        }

        private void AgregarUsuario(string nombres, string apellidos, string estadoCivil, string ApellidoCasada, DateTime fechaRegistro,
        string correo1, string correo2, string telefono1, string telefono2, string telefonoFijo,
         string colonia, string calle, string casa, string municipio, string departamento, string cp, string clave,
        string carnet, ComboBox tipoUsuarioBox, ComboBox estadoBox, ComboBox carreraBox)
        {
            try
            {
                int[] ids = new int [2];
                int tipoUsuarioID = 0;
                int carreraID = 0;
                int estadoID = 0;



                if (AdminOrEstudiante == 0)
                {
                    ids = IdTipoCarreraEstado(tipoUsuarioBox, estadoBox, carreraBox);
                    tipoUsuarioID = ids[0];
                    carreraID = ids[1];
                    estadoID = ids[2];
                }
                else
                {
                    carreraBox.SelectedValue = "N/A";
                    ids = IdTipoCarreraEstado(tipoUsuarioBox, estadoBox, carreraBox);
                    tipoUsuarioID = ids[0];
                    carreraID = ids[1];
                    estadoID = ids[2];
                }
          

                using (SqlConnection conn = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    SqlCommand cmd = new SqlCommand("sp_AgregarUsuario", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Nombres", nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", apellidos);
                    cmd.Parameters.AddWithValue("@EstadoCivil", estadoCivil);
                    cmd.Parameters.AddWithValue("@ApellidoCasada", ApellidoCasada);
                    cmd.Parameters.AddWithValue("@FechaRegistro", fechaRegistro);
                    cmd.Parameters.AddWithValue("@Correo1", correo1);
                    cmd.Parameters.AddWithValue("@Correo2", correo2);
                    cmd.Parameters.AddWithValue("@Telefono1", telefono1);
                    cmd.Parameters.AddWithValue("@Telefono2", telefono2);
                    cmd.Parameters.AddWithValue("@TelefonoFijo", telefonoFijo);
                    cmd.Parameters.AddWithValue("@Colonia", colonia);
                    cmd.Parameters.AddWithValue("@Calle", calle);
                    cmd.Parameters.AddWithValue("@Casa", casa);
                    cmd.Parameters.AddWithValue("@Municipio", municipio);
                    cmd.Parameters.AddWithValue("@Departamento", departamento);
                    cmd.Parameters.AddWithValue("@CP", cp);
                    cmd.Parameters.AddWithValue("@contrasena", clave);
                    cmd.Parameters.AddWithValue("@Carnet", carnet);
                    cmd.Parameters.AddWithValue("@TipoUsuarioID", tipoUsuarioID);
                    cmd.Parameters.AddWithValue("@EstadoID", estadoID);
                    cmd.Parameters.AddWithValue("@CarreraID", carreraID);

                    conn.Open();
                    if (cmd.ExecuteNonQuery() != 0)
                    {
                        MessageBox.Show("Dato agregado exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error inesperado, no se ha podido agregar", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado : " + e.Message);
            }
        }


        private int[] IdTipoCarreraEstado(ComboBox tipo, ComboBox carrera, ComboBox estado)
        {
            int[] ids = new int[3];

            string consultaTipoUsuario = "SELECT TipoUsuarioID FROM TipoUsuario WHERE Tipo = @Tipo";
            ids[0] = ObtenerIdLocal(consultaTipoUsuario, "@Tipo", tipoUsuarioComboBox, 0);

            string consultaCarrera = "SELECT CarreraID FROM Carrera WHERE NombreCarrera = @Carrera";
            ids[1] = ObtenerIdLocal(consultaCarrera, "@Carrera", carreraComboBox, 0);

            string consultaEstado = "SELECT EstadoID FROM Estado WHERE Estado = @Estado";
            ids[2] = ObtenerIdLocal(consultaEstado, "@Estado", estadoComboBox2, 0);

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













        #endregion
    }
}