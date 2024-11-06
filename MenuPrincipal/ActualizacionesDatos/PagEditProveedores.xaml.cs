using MenuPrincipal.BD.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
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

using System.Data;
using System.Data.SqlClient;
using MenuPrincipal.BD.Models;
using System.Data.Common;

namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para PagEditProveedores.xaml
    /// </summary>
    public partial class PagEditProveedores : Page
    {
        public PagEditProveedores()
        {
            InitializeComponent();
            CargarDataGrid();

        }

        private ProveedoresModels proveedoresDatos;

        private void ProveedoresDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            proveedoresDatos = (ProveedoresModels)ProveedoresDataGrid.SelectedItem;

            if (proveedoresDatos != null)
            {
                txtNuevoNombre.Text = proveedoresDatos.NombreProveedor.ToString();
                txtDUI.Text = proveedoresDatos.DUIProveedor.ToString();
                txtTelefono.Text = proveedoresDatos.TelefonoProveedor.ToString();
                txtDireccion.Text = proveedoresDatos.DireccionProveedor.ToString();
                txtNuevoNombre.Focus();
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Estás seguro de que deseas agregar este elemento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (resultado == MessageBoxResult.Yes)
            {
                proveedoresDatos = new ProveedoresModels();
                proveedoresDatos.NombreProveedor = txtNuevoNombre.Text;
                proveedoresDatos.DUIProveedor = txtDUI.Text;
                proveedoresDatos.TelefonoProveedor = txtTelefono.Text;
                proveedoresDatos.DireccionProveedor = txtDireccion.Text;
                AgregarDatos(proveedoresDatos);
                Limpiartxt();
                CargarDataGrid();
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Estás seguro de que deseas modificar este elemento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (resultado == MessageBoxResult.Yes)
            {
                proveedoresDatos.NombreProveedor = txtNuevoNombre.Text;
                proveedoresDatos.DUIProveedor = txtDUI.Text;
                proveedoresDatos.TelefonoProveedor = txtTelefono.Text;
                proveedoresDatos.DireccionProveedor = txtDireccion.Text;
                ActualizarDato(proveedoresDatos);
                Limpiartxt();
                CargarDataGrid();
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Limpiartxt();
        }

        private void Limpiartxt()
        {
            txtNuevoNombre.Clear();
            txtDUI.Clear();
            txtTelefono.Clear();
            txtDireccion.Clear();

        }



        #region Metodo Personaliados
        private void CargarDataGrid()
        {
            string consulta = "select *from Proveedores";
            List<Object> lista = new List<Object>();

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandText = consulta;

                        using (DbDataReader dbRead = cmd.ExecuteReader())
                        {
                            while (dbRead.Read())
                            {
                                ProveedoresModels proveedoresDatos = new ProveedoresModels();
                                {
                                    proveedoresDatos.ProveedorID = Convert.ToInt32(dbRead["ProveedorID"].ToString());
                                    proveedoresDatos.NombreProveedor = dbRead["NombreProveedor"].ToString();
                                    proveedoresDatos.DUIProveedor = dbRead["DUIProveedor"].ToString();
                                    proveedoresDatos.TelefonoProveedor = dbRead["TelefonoProveedor"].ToString();
                                    proveedoresDatos.DireccionProveedor = dbRead["DireccionProveedor"].ToString();

                                };

                                lista.Add(proveedoresDatos);
                            }
                        }
                        ProveedoresDataGrid.ItemsSource = lista;

                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado : " + e.Message);
            }
        }

        private void ActualizarDato(ProveedoresModels proveedoresDatos)
        {

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_ActualizarProveedor";



                        cmd.Parameters.AddWithValue("@NuevoNombre", proveedoresDatos.NombreProveedor);
                        cmd.Parameters.AddWithValue("@NuevoDUI", proveedoresDatos.DUIProveedor);
                        cmd.Parameters.AddWithValue("@NuevoTelefono", proveedoresDatos.TelefonoProveedor);
                        cmd.Parameters.AddWithValue("@NuevaDireccion", proveedoresDatos.DireccionProveedor);
                        cmd.Parameters.AddWithValue("@ProveedorID", proveedoresDatos.ProveedorID);


                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Dato actualizado exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error inesperado, no se ha podido actualizar", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado : " + e.Message);
            }
        }

        private void AgregarDatos(ProveedoresModels proveedoresDatos)
        {
            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_AgregarProveedor";



                        cmd.Parameters.AddWithValue("@NombreProveedor", proveedoresDatos.NombreProveedor);
                        cmd.Parameters.AddWithValue("@DuiProveedor", proveedoresDatos.DUIProveedor);
                        cmd.Parameters.AddWithValue("@TelefonoProveedor", proveedoresDatos.TelefonoProveedor);
                        cmd.Parameters.AddWithValue("@DireccionProveedor", proveedoresDatos.DireccionProveedor);



                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Proveedor agregado exitosamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Error inesperado, no se ha podido agregar el proveedor", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado" + e.Message);
            }
            #endregion


        }
    }
}