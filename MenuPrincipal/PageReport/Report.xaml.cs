using System;

using System.IO;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Documents;
using MenuPrincipal.PageReport.visualReports;
using MenuPrincipal.PageReport.subpagereport;

namespace MenuPrincipal.PageReport
{
    public partial class Report : Page
    {
        private DataTable comprasTable;
        private DataTable librosTable;
        private DataTable proveedoresEmpleadosTable;
        private DataTable librosMasPrestadosTable;

        public Report()
        {
            InitializeComponent();
            CargarDatosComboBox();
            CargarLibros();
            CargarDatosCompras();
            CargarDatosLibrosMasPrestados();

            CargarDatosProveedoresEmpleados();
        }

        #region carga de datos 
        private void CargarDatosCompras()
        {
            string consultaSQL = "SELECT c.CompraID AS ID, e.Titulo AS Articulo, c.FechaCompra, c.Cantidad, c.CostoTotal AS PrecioTotal " +
                                 "FROM Compras c " +
                                 "INNER JOIN Provisiones p ON c.CompraID = p.CompraID " +
                                 "INNER JOIN Ediciones e ON p.EdicionID = e.EdicionID";

            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    comprasTable = new DataTable();
                    adapter.Fill(comprasTable);
                    dataGridCompras.ItemsSource = comprasTable.DefaultView;

                    // Agregar periodos al ComboBox
                    comboBoxPeriodoCompras.Items.Add("3 Meses");
                    comboBoxPeriodoCompras.Items.Add("6 Meses");
                    comboBoxPeriodoCompras.Items.Add("12 Meses");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos de compras: " + ex.Message);
                }
            }
        }
        private void CargarDatosProveedoresEmpleados()
        {
            string consultaSQL = "SELECT ProveedorID AS ID, NombreProveedor AS Nombre, 'Proveedor' AS Tipo " +
                         "FROM Proveedores " +
                         "UNION " +
                         "SELECT u.UsuarioID AS ID, u.Nombres AS Nombre, 'Profesor' AS Tipo " +
                         "FROM Usuarios u " +
                         "INNER JOIN InfoUsuarios iu ON u.InfoID = iu.InfoID " +
                         "INNER JOIN TipoUsuario tu ON iu.TipoUsuarioID = tu.TipoUsuarioID " +
                         "WHERE tu.Tipo = 'Profesor'";

            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable proveedoresEmpleadosTable = new DataTable();
                    adapter.Fill(proveedoresEmpleadosTable);
                    dataGridProveedoresEmpleados.ItemsSource = proveedoresEmpleadosTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos de Proveedores y Empleados: " + ex.Message);
                }
            }
        }

        private void CargarDatosLibrosMasPrestados()
        {
            string consultaSQL = "SELECT e.EdicionID AS ID, e.Titulo, c.NombreCategoria AS Tema, a.NombreAutor AS Autor, " +
                     "COUNT(p.PrestamoID) AS CantidadPrestamos " +
                     "FROM Ediciones e " +
                     "INNER JOIN DetallesLibros d ON e.EdicionID = d.EdicionID " +
                     "INNER JOIN Categorias c ON d.CategoriaID = c.CategoriaID " +
                     "INNER JOIN Autores a ON d.AutorID = a.AutorID " +
                     "LEFT JOIN RefSolicitudes rs ON d.DetallesID = rs.LibroID " +
                     "LEFT JOIN Prestamos p ON rs.ReferenciaID = p.SolicitudID " +
                     "GROUP BY e.EdicionID, e.Titulo, c.NombreCategoria, a.NombreAutor " +
                     "ORDER BY CantidadPrestamos DESC";


            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable librosMasPrestadosTable = new DataTable();
                    adapter.Fill(librosMasPrestadosTable);
                    dataGridLibrosMasPrestados.ItemsSource = librosMasPrestadosTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar los datos de Libros Más Prestados: " + ex.Message);
                }
            }
        }

        private void CargarDatosComboBox()
        {
            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    conDB.Open();

                    // Cargar temas (categorías)
                    SqlCommand cmdTema = new SqlCommand("SELECT NombreCategoria FROM Categorias", conDB);
                    SqlDataReader readerTema = cmdTema.ExecuteReader();
                    while (readerTema.Read())
                    {
                        comboBoxCategoria.Items.Add(readerTema["NombreCategoria"].ToString());
                    }
                    readerTema.Close();

                    // Cargar autores
                    SqlCommand cmdAutor = new SqlCommand("SELECT NombreAutor FROM Autores", conDB);
                    SqlDataReader readerAutor = cmdAutor.ExecuteReader();
                    while (readerAutor.Read())
                    {
                        comboBoxAutor.Items.Add(readerAutor["NombreAutor"].ToString());
                    }
                    readerAutor.Close();

                    // Cargar proveedores
                    SqlCommand cmdProveedor = new SqlCommand("SELECT NombreProveedor FROM Proveedores", conDB);
                    SqlDataReader readerProveedor = cmdProveedor.ExecuteReader();
                    while (readerProveedor.Read())
                    {
                        comboBoxProveedores.Items.Add(readerProveedor["NombreProveedor"].ToString());
                    }
                    readerProveedor.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos en los ComboBox: " + ex.Message);
                }
            }
        }

        // Método para cargar todos los libros al iniciar
        private void CargarLibros()
        {
            string consultaSQL = "SELECT e.EdicionID AS ID, e.Titulo, c.NombreCategoria AS Categoria, a.NombreAutor AS Autor, s.StockActual AS Stock " +
                      "FROM Ediciones e " +
                      "INNER JOIN DetallesLibros d ON e.EdicionID = d.EdicionID " +
                      "INNER JOIN Categorias c ON d.CategoriaID = c.CategoriaID " +
                      "INNER JOIN Autores a ON d.AutorID = a.AutorID " +
                      "INNER JOIN Stock s ON e.EdicionID = s.EdicionID";


            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    librosTable = new DataTable();
                    adapter.Fill(librosTable);
                    dataGridLibros.ItemsSource = librosTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar libros: " + ex.Message);
                }
            }
        } 
        #endregion
        #region filtros
        // Método para aplicar filtros a los libros
        private void FiltrarLibros()
        {
            string categoria = comboBoxCategoria.SelectedItem?.ToString();
            string autor = comboBoxAutor.SelectedItem?.ToString();
            string especialidad = comboBoxEspecialidad.SelectedItem?.ToString();

            DataView dv = new DataView(librosTable);
            string filter = "";

            if (!string.IsNullOrEmpty(categoria))
            {
                filter += $"Categoria = '{categoria}'";
            }
            if (!string.IsNullOrEmpty(autor))
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += $"Autor = '{autor}'";
            }
            if (!string.IsNullOrEmpty(especialidad))
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += $"Especialidad = '{especialidad}'";
            }

            dv.RowFilter = filter;
            dataGridLibros.ItemsSource = dv;
        }

        // Método para aplicar filtros a proveedores y empleados
        private void FiltrarProveedoresEmpleados()
        {
            string proveedor = comboBoxProveedores.SelectedItem?.ToString();
            string empleado = comboBoxEmpleados.SelectedItem?.ToString();

            DataView dv = new DataView(proveedoresEmpleadosTable);
            string filter = "";

            if (!string.IsNullOrEmpty(proveedor))
            {
                filter += $"Proveedor = '{proveedor}'";
            }
            if (!string.IsNullOrEmpty(empleado))
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += $"Empleado = '{empleado}'";
            }

            dv.RowFilter = filter;
            dataGridProveedoresEmpleados.ItemsSource = dv;
        }

        // Método para aplicar filtros a análisis de compras
        private void FiltrarComprasPorPeriodo()
        {
            if (comboBoxPeriodoCompras.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un periodo.");
                return;
            }

            string periodo = ((ComboBoxItem)comboBoxPeriodoCompras.SelectedItem).Content.ToString();
            int meses = periodo.Contains("3 Meses") ? 3 : periodo.Contains("6 Meses") ? 6 : 12;

            DataView view = new DataView(comprasTable);
            view.RowFilter = $"DATEDIFF(MONTH, FechaCompra, GETDATE()) <= {meses}";
            dataGridCompras.ItemsSource = view;
        }

        // Método para aplicar filtros a libros más prestados
        private void FiltrarLibrosMasPrestados()
        {
            string tema = comboBoxCategoria.SelectedItem?.ToString(); // Asumiendo que Tema está en comboBoxCategoria
            string autor = comboBoxAutor.SelectedItem?.ToString();

            DataView dv = new DataView(librosMasPrestadosTable);
            string filter = "";

            if (!string.IsNullOrEmpty(tema))
            {
                filter += $"Tema = '{tema}'";
            }
            if (!string.IsNullOrEmpty(autor))
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += $"Autor = '{autor}'";
            }

            dv.RowFilter = filter;
            dataGridLibrosMasPrestados.ItemsSource = dv;
        }
        #endregion

        // Evento para actualizar los filtros en cada ComboBox
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Identificar el ComboBox que activó el evento
            if (sender == comboBoxCategoria || sender == comboBoxAutor || sender == comboBoxEspecialidad)
            {
                FiltrarLibros();
                
            }
            else if (sender == comboBoxProveedores || sender == comboBoxEmpleados)
            {
                FiltrarProveedoresEmpleados();
            }
            else if (sender == comboBoxPeriodoCompras)
            {
                FiltrarComprasPorPeriodo();
            }
        }


        // Método para generar reporte de libros
        private void GenerarReporteLibros(object sender, RoutedEventArgs e)
        {
            // Verificar si el DataGrid tiene datos
            if (dataGridLibros.ItemsSource == null)
            {
                MessageBox.Show("No hay datos para generar el reporte.");
                return;
            }

            // Instanciar el reporte de Crystal Report
            reportLibros rpt = new reportLibros();

            // Crear instancia del DataSet 'libros' y su DataTable
            libros dsLibros = new libros();
            DataTable dataTableLibros = dsLibros.Tables["DataTable1"]; // Usar el nombre correcto de la tabla

            // Verificar que la tabla no sea nula
            if (dataTableLibros == null)
            {
                MessageBox.Show("La tabla 'DataTable1' no existe en el DataSet.");
                return;
            }

            // Obtener los valores seleccionados en los ComboBox (si hay alguno seleccionado)
            string categoria = comboBoxCategoria.SelectedItem?.ToString();
            string autor = comboBoxAutor.SelectedItem?.ToString();
            string especialidad = comboBoxEspecialidad.SelectedItem?.ToString();

            // Limpiar el DataTable en el DataSet
            dataTableLibros.Clear();

            // Verificar si hay algún filtro activo
            bool filtroActivo = !string.IsNullOrEmpty(categoria) || !string.IsNullOrEmpty(autor) || !string.IsNullOrEmpty(especialidad);

            // Contador para verificar si se agregan filas al DataTable
            int contadorFilas = 0;

            // Llenar el DataTable con los datos del DataGrid, aplicando filtros si están activos
            foreach (DataRowView rowView in dataGridLibros.ItemsSource)
            {
                DataRow row = rowView.Row;

                // Verificar si la fila coincide con los filtros seleccionados
                if (!filtroActivo ||
                    ((string.IsNullOrEmpty(categoria) || row["Categoria"].ToString() == categoria) &&
                     (string.IsNullOrEmpty(autor) || row["Autor"].ToString() == autor) &&
                     (string.IsNullOrEmpty(especialidad) || row["Especialidad"].ToString() == especialidad)))
                {
                    // Crear una nueva fila en el DataTable con las columnas ajustadas
                    DataRow newRow = dataTableLibros.NewRow();
                    newRow["Columna1"] = row["Titulo"];
                    newRow["Columna2"] = row["Categoria"];
                    newRow["Columna3"] = row["Autor"];
                    newRow["Columna4"] = row["Stock"];

                    // Agregar la fila al DataTable del DataSet
                    dataTableLibros.Rows.Add(newRow);
                    contadorFilas++;
                }
            }

            // Verificar si se agregaron filas al DataTable
            if (contadorFilas == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            // Establecer el DataSet como la fuente de datos del reporte
            rpt.SetDataSource(dsLibros);

            // Instanciar el formulario de visor de reporte
            Window1 visor = new Window1();

            // Verificar si el visor y su CrystalReportViewer están correctamente inicializados
            if (visor == null)
            {
                MessageBox.Show("El visor no se pudo inicializar.");
                return;
            }

            // Verificar si el 'reportelibros' está correctamente configurado en el visor
            if (visor.reportelibros == null)
            {
                MessageBox.Show("El 'reportelibros' no está inicializado.");
                return;
            }

            // Verificar si el 'ViewerCore' está correctamente inicializado en el CrystalReportViewer
            if (visor.reportelibros.ViewerCore == null)
            {
                MessageBox.Show("El 'ViewerCore' del reporte no está inicializado.");
                return;
            }

            // Configurar el CrystalReportViewer con el reporte
            visor.reportelibros.ViewerCore.ReportSource = rpt;

           

            // Mostrar el visor del reporte
            visor.ShowDialog();
        }









        // Método para generar reporte de proveedores y empleados
        private void GenerarReporteProveedoresEmpleados()
        {
            // Verificar si los elementos seleccionados son nulos
            if (comboBoxProveedores.SelectedItem == null || comboBoxEmpleados.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un proveedor y un empleado.");
                return;
            }

            // Si los items son cadenas de texto (string), no es necesario convertirlos a ComboBoxItem
            string proveedor = comboBoxProveedores.SelectedItem.ToString();
            string empleado = comboBoxEmpleados.SelectedItem.ToString();

            string consultaSQL = "SELECT p.ProveedorID AS ID, p.NombreProveedor AS Nombre, 'Proveedor' AS Tipo " +
                                 "FROM Proveedores p WHERE p.NombreProveedor = @Proveedor " +
                                 "UNION ALL " +
                                 "SELECT e.EmpleadoID AS ID, e.NombreCompleto AS Nombre, 'Empleado' AS Tipo " +
                                 "FROM Empleados e WHERE e.NombreCompleto = @Empleado";

            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);
                    cmd.Parameters.AddWithValue("@Proveedor", proveedor);
                    cmd.Parameters.AddWithValue("@Empleado", empleado);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable proveedoresEmpleadosTable = new DataTable();
                    adapter.Fill(proveedoresEmpleadosTable);
                    dataGridProveedoresEmpleados.ItemsSource = proveedoresEmpleadosTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el reporte de proveedores y empleados: " + ex.Message);
                }
            }
        }
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar las selecciones de los ComboBox
           // comboBoxTema.SelectedItem = null;
            comboBoxAutor.SelectedItem = null;
            comboBoxEspecialidad.SelectedItem = null;
            comboBoxProveedores.SelectedItem = null;
            comboBoxEmpleados.SelectedItem = null;
            comboBoxPeriodoCompras.SelectedItem = null;

            // Volver a cargar todos los libros
            CargarLibros();
        }

        // Método para generar reporte de análisis de compras
        private void GenerarReporteCompras(object sender, RoutedEventArgs e)
        {
           
        }
        private void btnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
           
        }
        // Método para generar reporte de libros más prestados
        private void GenerarReporteLibrosMasPrestados(object sender, RoutedEventArgs e)
        {
           
        }

        // Método para generar la designación del lector y libro del mes
        private void GenerarLectorYLibroDelMes(object sender, RoutedEventArgs e)
        {
            string consultaSQLLector = "SELECT TOP 1 u.NombreCompleto, COUNT(p.PrestamoID) AS CantidadPrestamos " +
                                       "FROM Prestamos p " +
                                       "INNER JOIN Usuarios u ON p.UsuarioID = u.UsuarioID " +
                                       "WHERE MONTH(p.FechaPrestamo) = MONTH(GETDATE()) " +
                                       "GROUP BY u.NombreCompleto " +
                                       "ORDER BY CantidadPrestamos DESC";

            string consultaSQLLibro = "SELECT TOP 1 l.Titulo, COUNT(p.PrestamoID) AS CantidadPrestamos " +
                                      "FROM Prestamos p " +
                                      "INNER JOIN Libros l ON p.LibroID = l.LibroID " +
                                      "WHERE MONTH(p.FechaPrestamo) = MONTH(GETDATE()) " +
                                      "GROUP BY l.Titulo " +
                                      "ORDER BY CantidadPrestamos DESC";

            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmdLector = new SqlCommand(consultaSQLLector, conDB);
                    SqlCommand cmdLibro = new SqlCommand(consultaSQLLibro, conDB);

                    conDB.Open();

                    // Lector del Mes
                    SqlDataReader lectorReader = cmdLector.ExecuteReader();
                    if (lectorReader.Read())
                    {
                        textBlockLectorDelMes.Text = "Lector del Mes: " + lectorReader["NombreCompleto"]?.ToString();
                    }
                    lectorReader.Close();

                    // Libro del Mes
                    SqlDataReader libroReader = cmdLibro.ExecuteReader();
                    if (libroReader.Read())
                    {
                        textBlockLibroDelMes.Text = "Libro del Mes: " + libroReader["Titulo"]?.ToString();
                    }
                    libroReader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar la designación del lector y libro del mes: " + ex.Message);
                }
                finally
                {
                    conDB.Close();
                }
            }
        }

        private void GenerarReporteProveedoresEmpleados(object sender, RoutedEventArgs e)
        {
            GenerarReporteProveedoresEmpleados();
        }

  
    }
}