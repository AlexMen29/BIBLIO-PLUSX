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

namespace MenuPrincipal.PageUsuarios
{
    /// <summary>
    /// Lógica de interacción para Usuarios.xaml
    /// </summary>
    public partial class Usuarios : Page
    {

        DatosGlobales datos = new DatosGlobales();

        public List<DetallesUsuarios> ListaDataGrid;
        DetallesUsuarios UsuariosData;

        public Usuarios()
        {
            InitializeComponent();
            CargarDatos();


            LlenarBoxFiltros(datos.consultaTipoUsuario, TipoUsuarioComboBox, "Tipo");
            LlenarBoxFiltros(datos.consultaCarrera, CarreraComboBox, "NombreCarrera");
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
                            Lista.Add("Ninguno");
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



            UsuariosDataGrid.ItemsSource = usuariosFiltrados;

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

        private void BuscarUsuarios()
        {
            string carnet = CarnetTextBox.Text;

            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.conexionDB))
            {
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;

                    // Consulta SQL dinámica
                    string query = "SELECT * FROM Usuarios WHERE 1=1"; // 1=1 para no tener que revisar si es el primer filtro

                    if (!string.IsNullOrEmpty(carnet))
                    {
                        query += " AND Carnet LIKE @Carnet";
                        cmd.Parameters.AddWithValue("@Carnet", "%" + carnet + "%");
                    }

                    cmd.CommandText = query;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    UsuariosDataGrid.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al buscar usuarios: " + ex.Message);
                }

            }
        }

        private void btnBuscar_Click(object sender, RoutedEventArgs e)
        {
            BuscarUsuarios();
        }

        private void btnQuitarFiltros_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar las selecciones de los ComboBox
            TipoUsuarioComboBox.SelectedItem = null;
            CarreraComboBox.SelectedItem = null;
            FechaComboBox.SelectedItem = null;

            CargarDatos();

        }




        //    public Usuarios()
        //    {
        //        InitializeComponent();
        //    }
        //    //CONEXION  A LA BASE DE DATOS 
        //    SqlConnection condb = new SqlConnection(Properties.Settings.Default.conexionDB);

        //    //Variable consulata sql 
        //    String consultaSQL = null;

        //    void MostrarUsuario()
        //    {
        //        //consulta sql 
        //        consultaSQL = null;
        //        consultaSQL = "SELECT Carnet,NombreCompleto,Carrera,Correo,Telefono,FechaRegistro FROM USUARIOS";
        //        condb.Open();
        //        //creando elemento sqladapter
        //        SqlDataAdapter da = new SqlDataAdapter(consultaSQL, condb);
        //        // crear data table
        //        DataTable dt = new DataTable();
        //        // llenar table 
        //        da.Fill(dt);
        //        // proceder a llenado de datagrid
        //        dataGridUsuarios.ItemsSource = dt.DefaultView;
        //        //cerrar 
        //        condb.Close();

        //    }

        //    private void BuscarUsuarios()
        //    {
        //        string carnet = txtCarnet.Text;
        //        string nombreCompleto = txtNombre.Text;
        //        string carrera = txbCarrera.Text;
        //        string correo = txtCorreo.Text;

        //        using (SqlConnection con = new SqlConnection(Properties.Settings.Default.conexionDB))
        //        {
        //            try
        //            {
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand();
        //                cmd.Connection = con;

        //                // Consulta SQL dinámica
        //                string query = "SELECT * FROM USUARIOS WHERE 1=1"; // 1=1 para no tener que revisar si es el primer filtro

        //                if (!string.IsNullOrEmpty(carnet))
        //                {
        //                    query += " AND Carnet LIKE @Carnet";
        //                    cmd.Parameters.AddWithValue("@Carnet", "%" + carnet + "%");
        //                }
        //                if (!string.IsNullOrEmpty(nombreCompleto))
        //                {
        //                    query += " AND NombreCompleto LIKE @NombreCompleto";
        //                    cmd.Parameters.AddWithValue("@NombreCompleto", "%" + nombreCompleto + "%");
        //                }
        //                if (!string.IsNullOrEmpty(carrera))
        //                {
        //                    query += " AND Carrera LIKE @Carrera";
        //                    cmd.Parameters.AddWithValue("@Carrera", "%" + carrera + "%");
        //                }
        //                if (!string.IsNullOrEmpty(correo))
        //                {
        //                    query += " AND Correo LIKE @Correo";
        //                    cmd.Parameters.AddWithValue("@Correo", "%" + correo + "%");
        //                }

        //                cmd.CommandText = query;
        //                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        //                DataTable dt = new DataTable();
        //                adapter.Fill(dt);
        //                dataGridUsuarios.ItemsSource = dt.DefaultView; // Mostrar resultados en el DataGrid
        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show("Error al buscar usuarios: " + ex.Message);
        //            }
        //        }
        //    }
        //    private void Window_Loaded(object sender, RoutedEventArgs e)
        //    {
        //        MostrarUsuario();
        //    }

        //    private void btnFiltrar_Click(object sender, RoutedEventArgs e)
        //    {
        //        BuscarUsuarios();
        //    }
        //}


    }
}