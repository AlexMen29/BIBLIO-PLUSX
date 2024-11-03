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

using System.Data.SqlClient;
using System.Data;

namespace MenuPrincipal.ActualizacionesDatos
{
    /// <summary>
    /// Lógica de interacción para PagEditCategorias.xaml
    /// </summary>
    public partial class PagEditCategorias : Page
    {
        public PagEditCategorias()
        {
            InitializeComponent();
            CargarDataGrid();
        }

        private CategoriasModel datosCategoria;

        private void CategoriaDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            datosCategoria = (CategoriasModel)CategoriaDataGrid.SelectedItem;

            if (datosCategoria != null)
            {
                txtNuevoNombre.Text = datosCategoria.NombreCategoria.ToString();
                txtNuevoNombre.Focus();
            }

        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            txtNuevoNombre.Clear();
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            datosCategoria.NombreCategoria = txtNuevoNombre.Text;
            ActualizarDato(datosCategoria);
            CargarDataGrid();
        }



        #region metodo personalizados
        private void CargarDataGrid()
        {
            string consulta = "select *from Categorias";
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
                                CategoriasModel categoriaDatos = new CategoriasModel();
                                {
                                    categoriaDatos.CategoriaID = Convert.ToInt32(dbRead["CategoriaID"].ToString());
                                    categoriaDatos.NombreCategoria = dbRead["NombreCategoria"].ToString();
                                };

                                lista.Add(categoriaDatos);
                            }
                        }
                        CategoriaDataGrid.ItemsSource = lista;

                    }
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("Error inesperado : " + e.Message);
            }
        }

        private void ActualizarDato(CategoriasModel datosCategoria)
        {
            // Quitar las comillas en @NuevoNombre
            string consulta = "UPDATE Categorias SET NombreCategoria = @NuevoNombre WHERE CategoriaID = @CategoriaId";

            try
            {
                using (var conDb = new SqlConnection(Properties.Settings.Default.conexionDB))
                {
                    conDb.Open();

                    using (var cmd = conDb.CreateCommand())
                    {
                        cmd.CommandText = consulta;
                        cmd.Parameters.AddWithValue("@NuevoNombre", datosCategoria.NombreCategoria);
                        cmd.Parameters.AddWithValue("@CategoriaId", datosCategoria.CategoriaID);


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