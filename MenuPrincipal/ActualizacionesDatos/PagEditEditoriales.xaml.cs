using MenuPrincipal.BD.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
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

namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para PagEditEditoriales.xaml
    /// </summary>
    public partial class PagEditEditoriales : Page
    {
        public PagEditEditoriales()
        {
            InitializeComponent();
            CargarDataGrid();
        }

        private EditorialesModel editorialesDatos;

        private void EditorialesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            editorialesDatos = (EditorialesModel)EditorialesDataGrid.SelectedItem;

            if (editorialesDatos != null)
            {
                txtNuevoNombre.Text = editorialesDatos.NombreEditorial.ToString();
                txtDireccion.Text = editorialesDatos.DireccionEditorial.ToString();
                txtTelefono.Text = editorialesDatos.TelefonoEditorial.ToString();
                txtNuevoNombre.Focus();
            }
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult resultado = MessageBox.Show("¿Estás seguro de que deseas modificar este elemento?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (resultado == MessageBoxResult.Yes)
            {
                editorialesDatos.NombreEditorial = txtNuevoNombre.Text;
                editorialesDatos.DireccionEditorial = txtDireccion.Text;
                editorialesDatos.TelefonoEditorial = txtTelefono.Text;
                ActualizarDato(editorialesDatos);
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
            txtDireccion.Clear();
            txtTelefono.Clear();

        }



        #region MetodosPersonalizados

        private void CargarDataGrid()
        {
            string consulta = "select *from Editoriales";
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
                                EditorialesModel editorialesDatos = new EditorialesModel();
                                {
                                    editorialesDatos.EditorialID = Convert.ToInt32(dbRead["EditorialID"].ToString());
                                    editorialesDatos.NombreEditorial = dbRead["NombreEditorial"].ToString();
                                    editorialesDatos.DireccionEditorial = dbRead["DireccionEditorial"].ToString();
                                    editorialesDatos.TelefonoEditorial = dbRead["TelefonoEditorial"].ToString();

                                };

                                lista.Add(editorialesDatos);
                            }
                        }
                        EditorialesDataGrid.ItemsSource = lista;

                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado : " + e.Message);
            }
        }

        private void ActualizarDato(EditorialesModel datosEditorial)
        {

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ActualizarEditorial";



                        cmd.Parameters.AddWithValue("@NuevoNombre", datosEditorial.NombreEditorial);
                        cmd.Parameters.AddWithValue("@NuevaDireccion", datosEditorial.DireccionEditorial);
                        cmd.Parameters.AddWithValue("@NuevoTelefono", datosEditorial.TelefonoEditorial);
                        cmd.Parameters.AddWithValue("@EditorialId", datosEditorial.EditorialID);


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



        #endregion


    }
}