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
using MenuPrincipal.PageUsuarios;

namespace MenuPrincipal.PageReport
{
    public partial class Report : Page
    {
        private DataTable comprasTable;
        private DataTable librosTable;
        private DataTable proveedoresEmpleadosTable;

        private DataTable librosMasPrestadosTable;
        private DataTable usuariosTable;
        private DataTable proveedoresTable;

        public Report()
        {
            InitializeComponent();
            CargarDatosComboBox();
            CargarLibros();
            CargarDatosCompras();
            CargarDatosLibrosMasPrestados();
            CargarDatosProveedores();
            CargarDatosUsuarios();

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
        

        private void CargarDatosLibrosMasPrestados()
        {
            string consultaSQL = "SELECT e.EdicionID AS ID," +
    "e.Titulo, " +
    "c.NombreCategoria AS Tema, " +
    "a.NombreAutor AS Autor, " +
    "COUNT(pr.PrestamoID) AS CantidadPrestamos " + // Contar la cantidad de préstamos por libro
    "FROM Prestamos pr " +
    "INNER JOIN Solicitudes s ON pr.SolicitudID = s.SolicitudID " +
    "INNER JOIN RefSolicitudes rs ON s.SolicitudID = rs.ReferenciaID " +
    "INNER JOIN Libros l ON rs.LibroID = l.LibroID " +
    "INNER JOIN DetallesLibros dl ON l.DetallesID = dl.DetallesID " +
    "INNER JOIN Ediciones e ON dl.EdicionID = e.EdicionID " +
    "INNER JOIN Categorias c ON dl.CategoriaID = c.CategoriaID " + // Obtener el tema del libro
    "INNER JOIN Autores a ON dl.AutorID = a.AutorID " + // Obtener el autor del libro
    "GROUP BY e.Titulo, c.NombreCategoria, a.NombreAutor,e.EdicionID " +
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

                    /*Cargar proveedores
                    SqlCommand cmdProveedor = new SqlCommand("SELECT Tipo FROM TipoUsuario", conDB);
                    SqlDataReader readerProveedor = cmdProveedor.ExecuteReader();
                    while (readerProveedor.Read())
                    {
                        comboBoxRol.Items.Add(readerProveedor["Tipo"].ToString());
                    }
                    readerProveedor.Close();*/
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
            // Obtener el valor seleccionado del ComboBox de Stock como texto
            string stockSeleccionado = (comboBoxStock.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Construir la consulta SQL con un parámetro de filtro opcional para Stock
            string consultaSQL = "SELECT e.EdicionID AS ID, e.Titulo, c.NombreCategoria AS Categoria, a.NombreAutor AS Autor, s.StockActual AS Stock " +
                                 "FROM Ediciones e " +
                                 "INNER JOIN DetallesLibros d ON e.EdicionID = d.EdicionID " +
                                 "INNER JOIN Categorias c ON d.CategoriaID = c.CategoriaID " +
                                 "INNER JOIN Autores a ON d.AutorID = a.AutorID " +
                                 "INNER JOIN Stock s ON e.EdicionID = s.EdicionID ";

            // Si hay un valor de stock seleccionado, aplica la condición en la consulta
            if (!string.IsNullOrEmpty(stockSeleccionado))
            {
                if (stockSeleccionado == "Mayores de 15")
                {
                    consultaSQL += "WHERE s.StockActual > 15";
                }
                else
                {
                    consultaSQL += "WHERE s.StockActual = @Stock";
                }
            }

            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);

                    // Añadir el parámetro solo si es un valor específico (como "5", "10", etc.)
                    if (!string.IsNullOrEmpty(stockSeleccionado) && stockSeleccionado != "Mayores de 15")
                    {
                        // Convertir el valor seleccionado a entero antes de asignarlo al parámetro
                        int stockValue = int.Parse(stockSeleccionado);
                        cmd.Parameters.AddWithValue("@Stock", stockValue);
                    }

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
        // Método para cargar datos de usuarios
        private void textBoxFiltroUsuarios_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarUsuarios();
        }

        // Método para filtrar proveedores cuando el texto cambia en el TextBox
        private void textBoxFiltroProveedores_TextChanged(object sender, TextChangedEventArgs e)
        {
            FiltrarProveedores();
        }
        private void CargarDatosUsuarios()
        {
            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT \r\n    U.Nombres, \r\n    U.Apellidos, \r\n    U.Carnet, \r\n    CU.Correo1, \r\n    CU.Telefono1, \r\n    D.Colonia, \r\n    D.Calle, \r\n    D.Casa, \r\n    D.Municipio, \r\n    D.Departamento, \r\n    TU.Tipo AS TipoUsuario\r\nFROM \r\n    Usuarios U\r\nJOIN \r\n    InfoUsuarios IU ON U.InfoID = IU.InfoID\r\nJOIN \r\n    ContactoUsuarios CU ON IU.ContactoID = CU.ContactoID\r\nJOIN \r\n    Direcciones D ON CU.DireccionID = D.DireccionID\r\nJOIN \r\n    TipoUsuario TU ON IU.TipoUsuarioID = TU.TipoUsuarioID;", conDB);
                usuariosTable = new DataTable();
                adapter.Fill(usuariosTable);
                dataGridUsuarios.ItemsSource = usuariosTable.DefaultView;
            }
        }

        // Método para cargar datos de proveedores
        private void CargarDatosProveedores()
        {
            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT NombreProveedor, DUIProveedor, TelefonoProveedor, DireccionProveedor FROM Proveedores", conDB);
                proveedoresTable = new DataTable();
                adapter.Fill(proveedoresTable);
                dataGridProveedores.ItemsSource = proveedoresTable.DefaultView;
            }
        }


        #endregion
        #region filtros
        // Método para aplicar filtros a los libros
        private void FiltrarLibros()
        {
            string categoria = comboBoxCategoria.SelectedItem?.ToString();
            string autor = comboBoxAutor.SelectedItem?.ToString();
            string stockSeleccionado = comboBoxStock.SelectedItem?.ToString();

            DataView dv = new DataView(librosTable);
            string filter = dv.RowFilter; // Mantiene el filtro acumulado

            // Filtro por Categoría
            if (!string.IsNullOrEmpty(categoria))
            {
                if (!filter.Contains("Categoria")) // Evita duplicar el filtro
                {
                    if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                    filter += $"Categoria = '{categoria}'";
                }
            }

            // Filtro por Autor
            if (!string.IsNullOrEmpty(autor))
            {
                if (!filter.Contains("Autor")) // Evita duplicar el filtro
                {
                    if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                    filter += $"Autor = '{autor}'";
                }
            }

            // Filtro por Stock
            if (!string.IsNullOrEmpty(stockSeleccionado))
            {
                if (stockSeleccionado == "Mayores de 15")
                {
                    if (!filter.Contains("Stock > 15")) // Evita duplicar el filtro
                    {
                        if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                        filter += "Stock > 15";
                    }
                }
                else if (int.TryParse(stockSeleccionado, out int stockValue))
                {
                    if (!filter.Contains($"Stock = {stockValue}")) // Evita duplicar el filtro
                    {
                        if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                        filter += $"Stock = {stockValue}";
                    }
                }
            }

            dv.RowFilter = filter; // Aplica el filtro acumulado
            dataGridLibros.ItemsSource = dv;
        }

        private void FiltrarUsuarios()
        {
            if (comboBoxUsuarioColumnas.SelectedItem == null || string.IsNullOrEmpty(textBoxFiltroUsuarios.Text))
            {
                dataGridUsuarios.ItemsSource = usuariosTable.DefaultView;
                return;
            }

            string columna = ((ComboBoxItem)comboBoxUsuarioColumnas.SelectedItem).Content.ToString();
            string valor = textBoxFiltroUsuarios.Text;
            DataView dv = new DataView(usuariosTable);
            dv.RowFilter = $"{columna} LIKE '%{valor}%'";
            dataGridUsuarios.ItemsSource = dv;
        }

        // Método para filtrar proveedores
        private void FiltrarProveedores()
        {
            if (comboBoxProveedorColumnas.SelectedItem == null || string.IsNullOrEmpty(textBoxFiltroProveedores.Text))
            {
                dataGridProveedores.ItemsSource = proveedoresTable.DefaultView;
                return;
            }

            string columna = ((ComboBoxItem)comboBoxProveedorColumnas.SelectedItem).Content.ToString();
            string valor = textBoxFiltroProveedores.Text;
            DataView dv = new DataView(proveedoresTable);
            dv.RowFilter = $"{columna} LIKE '%{valor}%'";
            dataGridProveedores.ItemsSource = dv;
        }



        // Método para aplicar filtros a análisis de compras
        private void FiltrarComprasPorPeriodo()
        {
            if (comboBoxPeriodoCompras.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un periodo.");
                return;
            }

            string periodo = comboBoxPeriodoCompras.SelectedItem.ToString();
            int meses = periodo.Contains("3 Meses") ? 3 : periodo.Contains("6 Meses") ? 6 : 12;

            // Calcular la fecha límite para incluir compras en ese período
            DateTime fechaLimite = DateTime.Now.AddMonths(-meses);

            // Crear una nueva instancia de DataView para asegurar que se actualice el filtro correctamente
            DataView view = new DataView(comprasTable);
            view.RowFilter = $"FechaCompra >= #{fechaLimite:yyyy-MM-dd}#";

            // Asignar la nueva vista filtrada al DataGrid
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

        #region selecetionChangeds
        // Evento para actualizar los filtros en cada ComboBox
        private void comboBoxStock_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CargarLibros();
            FiltrarLibros();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Identificar el ComboBox que activó el evento
            if (sender == comboBoxCategoria || sender == comboBoxAutor || sender == comboBoxStock)
            {
                FiltrarLibros();
                
            }

            else if (sender == comboBoxPeriodoCompras)
            {
                FiltrarComprasPorPeriodo();
            }
        }
       

        #endregion
        #region Reportes
        // Método para generar reporte de libros
        private void GenerarReporteLibros(object sender, RoutedEventArgs e)
        {
            // Verificar si el DataGrid tiene datos visibles
            if (dataGridLibros.Items.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            reportLibros rpt = new reportLibros();
            libros dsLibros = new libros();
            DataTable dataTableLibros = dsLibros.Tables["DataTable1"]; // Usar el nombre correcto de la tabla

            if (dataTableLibros == null)
            {
                MessageBox.Show("La tabla 'DataTable1' no existe en el DataSet.");
                return;
            }

            dataTableLibros.Clear();

            foreach (DataRowView rowView in dataGridLibros.ItemsSource)
            {
                DataRow row = rowView.Row;
                DataRow newRow = dataTableLibros.NewRow();
                newRow["Columna1"] = row["Titulo"];
                newRow["Columna2"] = row["Categoria"];
                newRow["Columna3"] = row["Autor"];
                newRow["Columna4"] = row["Stock"];
                dataTableLibros.Rows.Add(newRow);
            }

            if (dataTableLibros.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            rpt.SetDataSource(dsLibros);

            Window1 visor = new Window1();

            if (visor == null || visor.reportelibros == null || visor.reportelibros.ViewerCore == null)
            {
                MessageBox.Show("El visor del reporte no está correctamente inicializado.");
                return;
            }

            visor.reportelibros.ViewerCore.ReportSource = rpt;
            visor.ShowDialog();
        }

        private void GenerarReporteUsuarios_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si el DataGrid tiene datos visibles
            if (dataGridUsuarios.Items.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            reportProveedoresEmpleados rptUsuarios = new reportProveedoresEmpleados();
            libros dsUsuarios = new libros();
            DataTable dataTableUsuarios = dsUsuarios.Tables["DataTable2"]; // Usar el nombre correcto de la tabla

            if (dataTableUsuarios == null)
            {
                MessageBox.Show("La tabla 'DataTableUsuarios' no existe en el DataSet.");
                return;
            }

            dataTableUsuarios.Clear();

            foreach (DataRowView rowView in dataGridUsuarios.ItemsSource)
            {
                DataRow row = rowView.Row;
                DataRow newRow = dataTableUsuarios.NewRow();
                newRow["Columna1"] = row["Nombres"];
                newRow["Columna2"] = row["Apellidos"];
                newRow["Columna3"] = row["Carnet"];
                newRow["Columna4"] = row["Correo1"];
                newRow["Columna5"] = row["Telefono1"];
                newRow["Columna6"] = row["Colonia"];
                newRow["Columna7"] = row["Calle"];
                newRow["Columna8"] = row["Casa"];
                newRow["Columna9"] = row["Municipio"];
                newRow["Columna10"] = row["Departamento"];
                newRow["Columna11"] = row["TipoUsuario"];
                dataTableUsuarios.Rows.Add(newRow);
            }

            if (dataTableUsuarios.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            rptUsuarios.SetDataSource(dsUsuarios);

            Window1 visorUsuarios = new Window1();
            visorUsuarios.reportelibros.ViewerCore.ReportSource = rptUsuarios;
            visorUsuarios.ShowDialog();
        }


        private void GenerarReporteProveedores_Click(object sender, RoutedEventArgs e)
        {
            // Verificar si el DataGrid tiene datos visibles
            if (dataGridProveedores.Items.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            reportproveedore rptProveedores = new reportproveedore();
            libros dsProveedores = new libros();
            DataTable dataTableProveedores = dsProveedores.Tables["DataTable3"]; // Usar el nombre correcto de la tabla

            if (dataTableProveedores == null)
            {
                MessageBox.Show("La tabla 'DataTableProveedores' no existe en el DataSet.");
                return;
            }

            dataTableProveedores.Clear();

            foreach (DataRowView rowView in dataGridProveedores.ItemsSource)
            {
                DataRow row = rowView.Row;
                DataRow newRow = dataTableProveedores.NewRow();
                newRow["Columna1"] = row["NombreProveedor"];
                newRow["Columna2"] = row["DUIProveedor"];
                newRow["Columna3"] = row["TelefonoProveedor"];
                newRow["Columna4"] = row["DireccionProveedor"];
                dataTableProveedores.Rows.Add(newRow);
            }

            if (dataTableProveedores.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            rptProveedores.SetDataSource(dsProveedores);

            Window1 visorProveedores = new Window1();
            visorProveedores.reportelibros.ViewerCore.ReportSource = rptProveedores;
            visorProveedores.ShowDialog();
        }

        private void GenerarReporteCompras(object sender, RoutedEventArgs e)
        {
            // Verificar si el DataGrid tiene datos visibles
            if (dataGridCompras.Items.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            reportcompras rptcompra = new reportcompras();
            libros dsCompras = new libros();
            DataTable dataTableCompras = dsCompras.Tables["DataTable4"];

            if (dataTableCompras == null)
            {
                MessageBox.Show("La tabla 'DataTable4' no existe en el DataSet.");
                return;
            }

            dataTableCompras.Clear();

            foreach (DataRowView rowView in dataGridCompras.ItemsSource)
            {
                DataRow row = rowView.Row;
                DataRow newRow = dataTableCompras.NewRow();
                newRow["Columna1"] = row["Articulo"];
                newRow["Columna2"] = row["FechaCompra"];
                newRow["Columna3"] = row["Cantidad"];
                newRow["Columna4"] = row["PrecioTotal"];
                dataTableCompras.Rows.Add(newRow);
            }

            if (dataTableCompras.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            rptcompra.SetDataSource(dsCompras);

            Window1 visorCompras = new Window1();
            visorCompras.reportelibros.ViewerCore.ReportSource = rptcompra;
            visorCompras.ShowDialog();
        }

        private void GenerarReporteLibrosMasPrestados(object sender, RoutedEventArgs e)
        {
            // Verificar si el DataGrid tiene datos visibles
            if (dataGridLibrosMasPrestados.Items.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            reportLibroMasMostrado rptLibrosMasPrestados = new reportLibroMasMostrado();
            libros dsLibrosMasPrestados = new libros();
            DataTable dataTableLibrosMasPrestados = dsLibrosMasPrestados.Tables["DataTable5"]; // Asegúrate de que "DataTable5" sea el nombre correcto de la tabla

            if (dataTableLibrosMasPrestados == null)
            {
                MessageBox.Show("La tabla 'DataTable5' no existe en el DataSet.");
                return;
            }

            dataTableLibrosMasPrestados.Clear();

            foreach (DataRowView rowView in dataGridLibrosMasPrestados.ItemsSource)
            {
                DataRow row = rowView.Row;
                DataRow newRow = dataTableLibrosMasPrestados.NewRow();
                newRow["Columna1"] = row["ID"];
                newRow["Columna2"] = row["Titulo"];
                newRow["Columna3"] = row["Tema"];
                newRow["Columna4"] = row["Autor"];
                newRow["Columna5"] = row["CantidadPrestamos"];
                dataTableLibrosMasPrestados.Rows.Add(newRow);
            }

            if (dataTableLibrosMasPrestados.Rows.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            rptLibrosMasPrestados.SetDataSource(dsLibrosMasPrestados);

            Window1 visorLibrosMasPrestados = new Window1();
            visorLibrosMasPrestados.reportelibros.ViewerCore.ReportSource = rptLibrosMasPrestados;
            visorLibrosMasPrestados.ShowDialog();
        }


        #endregion

        #region btnLimpiar
        private void btnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            // Limpiar las selecciones de los ComboBox
            comboBoxCategoria.SelectedItem = null;
            comboBoxAutor.SelectedItem = null;
            comboBoxStock.SelectedItem = null;
           

            // Volver a cargar todos los libros
            CargarLibros();
        }
        private void btnLimpiar_Copiar_Click(object sender, RoutedEventArgs e)
        {
            comboBoxUsuarioColumnas.SelectedItem = null;
            textBoxFiltroUsuarios.Text = string.Empty;

        }
        private void btnLimpiar_Copiar1_Click(object sender, RoutedEventArgs e)
        {
            comboBoxProveedorColumnas.SelectedItem = null;
            textBoxFiltroProveedores.Text = string.Empty;
        }
        private void btnLimpiar_Copiar2_Click(object sender, RoutedEventArgs e)
        {
            comboBoxPeriodoCompras.SelectedItem = null;
            CargarDatosCompras();
        }
        #endregion

        private void GenerarDesignacionLectorYLibroDelMes()
        {
            try
            {
                using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
                {
                    conDB.Open();

                    // Consulta para el Lector del Mes
                    string lectorQuery = @"
                SELECT TOP 1 u.Nombres AS Lector, COUNT(p.PrestamoID) AS TotalPrestamos
                FROM Prestamos p
                JOIN Solicitudes s ON p.SolicitudID = s.SolicitudID
                JOIN RefSolicitudes rs ON s.ReferenciaID = rs.ReferenciaID
                JOIN Usuarios u ON rs.UsuarioID = u.UsuarioID
                WHERE p.FechaPrestamo >= DATEADD(MONTH, -1, GETDATE())
                GROUP BY u.Nombres
                ORDER BY TotalPrestamos DESC";

                    using (SqlCommand cmdLector = new SqlCommand(lectorQuery, conDB))
                    using (SqlDataReader reader = cmdLector.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBlockLectorDelMes.Text = "Lector del Mes: " + reader["Lector"].ToString();
                        }
                        else
                        {
                            textBlockLectorDelMes.Text = "Lector del Mes: No disponible";
                        }
                    }

                    // Consulta para el Libro del Mes
                    string libroQuery = @"
                SELECT TOP 1 e.Titulo AS LibroTitulo, COUNT(p.PrestamoID) AS TotalPrestamos
                FROM Prestamos p
                JOIN Solicitudes s ON p.SolicitudID = s.SolicitudID
                JOIN RefSolicitudes rs ON s.ReferenciaID = rs.ReferenciaID
                JOIN Libros l ON rs.LibroID = l.LibroID
                JOIN Ediciones e ON l.DetallesID = e.EdicionID
                WHERE p.FechaPrestamo >= DATEADD(MONTH, -1, GETDATE())
                GROUP BY e.Titulo
                ORDER BY TotalPrestamos DESC";

                    using (SqlCommand cmdLibro = new SqlCommand(libroQuery, conDB))
                    using (SqlDataReader reader = cmdLibro.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBlockLibroDelMes.Text = "Libro del Mes: " + reader["LibroTitulo"].ToString();
                        }
                        else
                        {
                            textBlockLibroDelMes.Text = "Libro del Mes: No disponible";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar la designación: " + ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GenerarDesignacionLectorYLibroDelMes();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Verificar si el DataGrid tiene datos visibles
            if (dataGridLibros.Items.Count == 0)
            {
                MessageBox.Show("No se encontraron datos para el reporte con los filtros seleccionados.");
                return;
            }

            libromesyusuario rpt = new libromesyusuario();
            libros dsLibros = new libros();
            DataTable dataTableLibros = dsLibros.Tables["DataTable6"]; // Usar el nombre correcto de la tabla

            if (dataTableLibros == null)
            {
                MessageBox.Show("La tabla 'DataTable6' no existe en el DataSet.");
                return;
            }

            dataTableLibros.Clear();

            // Añadir una fila con los datos del Lector y Libro del Mes
            DataRow newRow = dataTableLibros.NewRow();
            newRow["DataColumn1"] = textBlockLectorDelMes.Text.Replace("Lector del Mes: ", "");
            newRow["DataColumn2"] = textBlockLibroDelMes.Text.Replace("Libro del Mes: ", "");
            dataTableLibros.Rows.Add(newRow);

            // Enviar el DataTable al reporte
            rpt.SetDataSource(dsLibros);

            // Mostrar el reporte en el visor
            Window1 visor = new Window1();
            visor.reportelibros.ViewerCore.ReportSource = rpt;
            visor.ShowDialog();
        }

    }
}