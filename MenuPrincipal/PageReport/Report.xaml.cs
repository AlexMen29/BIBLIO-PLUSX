using System;

using System.IO;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Documents;

namespace MenuPrincipal.PageReport
{
    public partial class Report : Page
    {
        private DataTable comprasTable;
        private DataTable librosTable;

        public Report()
        {
            InitializeComponent();
            CargarDatosComboBox();
            CargarLibros();
            CargarDatosCompras();
        }

        #region carga de datos y filtros
        private void CargarDatosCompras()
        {
            string consultaSQL = "SELECT c.CompraID AS ID, l.Titulo AS Articulo, c.FechaCompra, c.Cantidad, c.TotalCompra AS PrecioTotal " +
                     "FROM Compras c " +
                     "INNER JOIN Libros l ON c.LibroID = l.LibroID ";// Relacionar con la tabla Libros




            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    comprasTable = new DataTable();
                    adapter.Fill(comprasTable);
                    dataGridCompras.ItemsSource = comprasTable.DefaultView; // Cargar datos en el DataGrid

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

        private void FiltrarComprasPorPeriodo(object sender, RoutedEventArgs e)
        {
            if (comboBoxPeriodoCompras.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un periodo.");
                return;
            }

            string periodo = ((ComboBoxItem)comboBoxPeriodoCompras.SelectedItem).Content.ToString();
            int meses = periodo.Contains("3 Meses") ? 3 : periodo.Contains("6 Meses") ? 6 : 12;

            // Filtrar los datos del DataTable en función del periodo seleccionado
            DataView view = new DataView(comprasTable);
            view.RowFilter = $"DATEDIFF(MONTH, FechaCompra, GETDATE()) <= {meses}";
            dataGridCompras.ItemsSource = view; // Actualizar los datos en el DataGrid
        }
        private void CargarDatosComboBox()
        {
            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    conDB.Open();

                    // Cargar temas
                    SqlCommand cmdTema = new SqlCommand("SELECT NombreCategoria FROM Categorias", conDB);
                    SqlDataReader readerTema = cmdTema.ExecuteReader();
                    while (readerTema.Read())
                    {
                        comboBoxTema.Items.Add(readerTema["NombreCategoria"].ToString());
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

                    // Cargar especialidades
                    SqlCommand cmdEspecialidad = new SqlCommand("SELECT NombreEspecialidad FROM Especialidades", conDB);
                    SqlDataReader readerEspecialidad = cmdEspecialidad.ExecuteReader();
                    while (readerEspecialidad.Read())
                    {
                        comboBoxEspecialidad.Items.Add(readerEspecialidad["NombreEspecialidad"].ToString());
                    }
                    readerEspecialidad.Close();

                    // Cargar proveedores
                    SqlCommand cmdProveedor = new SqlCommand("SELECT NombreProveedor FROM Proveedores", conDB);
                    SqlDataReader readerProveedor = cmdProveedor.ExecuteReader();
                    while (readerProveedor.Read())
                    {
                        comboBoxProveedores.Items.Add(readerProveedor["NombreProveedor"].ToString());
                    }
                    readerProveedor.Close();

                    // Cargar empleados
                    SqlCommand cmdEmpleado = new SqlCommand("SELECT NombreCompleto FROM Empleados", conDB);
                    SqlDataReader readerEmpleado = cmdEmpleado.ExecuteReader();
                    while (readerEmpleado.Read())
                    {
                        comboBoxEmpleados.Items.Add(readerEmpleado["NombreCompleto"].ToString());
                    }
                    readerEmpleado.Close();
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
            string consultaSQL = "SELECT l.LibroID AS ID, l.Titulo, c.NombreCategoria AS Tema, l.Autor, e.NombreEspecialidad AS Especialidad " +
                                 "FROM Libros l " +
                                 "INNER JOIN Categorias c ON l.CategoriaID = c.CategoriaID " +
                                 "INNER JOIN Especialidades e ON l.EspecialidadID = e.EspecialidadID";

            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    librosTable = new DataTable();
                    adapter.Fill(librosTable);
                    dataGridLibros.ItemsSource = librosTable.DefaultView; // Cargar los datos en el DataGrid
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar libros: " + ex.Message);
                }
            }
        }

        // Método para aplicar filtros a los libros
        private void FiltrarLibros()
        {
            string tema = comboBoxTema.SelectedItem?.ToString();
            string autor = comboBoxAutor.SelectedItem?.ToString();
            string especialidad = comboBoxEspecialidad.SelectedItem?.ToString();

            DataView dv = new DataView(librosTable);
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
            if (!string.IsNullOrEmpty(especialidad))
            {
                if (!string.IsNullOrEmpty(filter)) filter += " AND ";
                filter += $"Especialidad = '{especialidad}'";
            }

            dv.RowFilter = filter;
            dataGridLibros.ItemsSource = dv; // Aplicar el filtro en el DataGrid
        }
        #endregion
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Llamar a FiltrarLibros cada vez que se cambie un ComboBox
            FiltrarLibros();
        }


        // Método para generar reporte de libros
        private void GenerarReporteLibros(object sender, RoutedEventArgs e)
        {
            // Verificar si los elementos seleccionados son nulos
            if (comboBoxTema.SelectedItem == null || comboBoxAutor.SelectedItem == null || comboBoxEspecialidad.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecciona un tema, autor y especialidad.");
                return;
            }

            // Asumiendo que los ComboBox contienen strings en lugar de ComboBoxItem
            string tema = comboBoxTema.SelectedItem.ToString();
            string autor = comboBoxAutor.SelectedItem.ToString();
            string especialidad = comboBoxEspecialidad.SelectedItem.ToString();

            // Consulta SQL corregida, usando la columna 'Autor' en lugar de 'AutorID'
            string consultaSQL = "SELECT l.LibroID AS ID, l.Titulo, c.NombreCategoria AS Tema, l.Autor, e.NombreEspecialidad AS Especialidad " +
                                 "FROM Libros l " +
                                 "INNER JOIN Categorias c ON l.CategoriaID = c.CategoriaID " +
                                 "INNER JOIN Especialidades e ON l.EspecialidadID = e.EspecialidadID " +
                                 "WHERE c.NombreCategoria = @Tema AND l.Autor = @Autor AND e.NombreEspecialidad = @Especialidad";

            // Conexión a la base de datos
            using (SqlConnection conDB = new SqlConnection(MenuPrincipal.Properties.Settings.Default.conexionDB))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(consultaSQL, conDB);
                    cmd.Parameters.AddWithValue("@Tema", tema);
                    cmd.Parameters.AddWithValue("@Autor", autor);
                    cmd.Parameters.AddWithValue("@Especialidad", especialidad);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable librosTable = new DataTable();
                    adapter.Fill(librosTable);
                    dataGridLibros.ItemsSource = librosTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al generar el reporte de libros: " + ex.Message);
                }
            }
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
            comboBoxTema.SelectedItem = null;
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
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "PDF Document (.pdf)|.pdf";
            //saveFileDialog.FileName = "ReporteCompras.pdf";

            //if (saveFileDialog.ShowDialog() == true)
            //{
            //    // Obtener la ruta donde se guardará el archivo PDF
            //    string rutaArchivoPDF = saveFileDialog.FileName;

            //    // Crear un documento PDF
            //    iTextSharp.text.Document documento = new iTextSharp.text.Document();
            //    PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(rutaArchivoPDF, FileMode.Create));
            //    documento.Open();

            //    // Agregar título
            //    documento.Add(new iTextSharp.text.Paragraph("Reporte de Compras"));
            //    documento.Add(new iTextSharp.text.Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")));
            //    documento.Add(new iTextSharp.text.Paragraph("\n"));

            //    // Crear una tabla para agregar los datos del DataGrid
            //    PdfPTable tablaPDF = new PdfPTable(dataGridCompras.Columns.Count);

            //    // Agregar los encabezados de las columnas
            //    foreach (DataGridColumn column in dataGridCompras.Columns)
            //    {
            //        tablaPDF.AddCell(new Phrase(column.Header.ToString()));
            //    }

            //    // Agregar las filas de datos
            //    foreach (DataRowView row in dataGridCompras.ItemsSource as DataView)
            //    {
            //        foreach (var item in row.Row.ItemArray)
            //        {
            //            tablaPDF.AddCell(new Phrase(item.ToString()));
            //        }
            //    }

            //    // Agregar la tabla al documento
            //    documento.Add(tablaPDF);

            //    // Cerrar el documento PDF
            //    documento.Close();
            //    writer.Close();

            //    MessageBox.Show("Reporte generado exitosamente en: " + rutaArchivoPDF);
            //}
        }
        private void btnGenerarReporte_Click(object sender, RoutedEventArgs e)
        {
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "PDF Document (.pdf)|.pdf";
            //saveFileDialog.FileName = "ReporteBiblioteca.pdf";

            //if (saveFileDialog.ShowDialog() == true)
            //{
            //    string rutaArchivoPDF = saveFileDialog.FileName;

            //    iTextSharp.text.Document documento = new iTextSharp.text.Document();
            //    try
            //    {
            //        PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(rutaArchivoPDF, FileMode.Create));
            //        documento.Open();

            //        documento.Add(new iTextSharp.text.Paragraph("Reporte de Biblioteca"));
            //        documento.Add(new iTextSharp.text.Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")));
            //        documento.Add(new iTextSharp.text.Paragraph("\n"));

            //        // Agregar reporte de Proveedores y Empleados
            //        foreach (DataRowView row in dataGridProveedoresEmpleados.Items)
            //        {
            //            string id = row["ID"].ToString();
            //            string nombre = row["Nombre"].ToString();
            //            string tipo = row["Tipo"].ToString();

            //            documento.Add(new iTextSharp.text.Paragraph("ID: " + id));
            //            documento.Add(new iTextSharp.text.Paragraph("Nombre: " + nombre));
            //            documento.Add(new iTextSharp.text.Paragraph("Tipo: " + tipo));
            //            documento.Add(new iTextSharp.text.Paragraph("\n"));
            //        }

            //        documento.Add(new iTextSharp.text.Paragraph("Reporte de Compras:"));
            //        documento.Add(new iTextSharp.text.Paragraph("\n"));

            //        foreach (DataRowView row in dataGridCompras.Items)
            //        {
            //            string idCompra = row["ID"].ToString();
            //            string articulo = row["Articulo"].ToString();
            //            string fecha = row["FechaCompra"].ToString();
            //            string cantidad = row["Cantidad"].ToString();
            //            string precioTotal = row["PrecioTotal"].ToString();

            //            documento.Add(new iTextSharp.text.Paragraph("ID Compra: " + idCompra));
            //            documento.Add(new iTextSharp.text.Paragraph("Artículo: " + articulo));
            //            documento.Add(new iTextSharp.text.Paragraph("Fecha de Compra: " + fecha));
            //            documento.Add(new iTextSharp.text.Paragraph("Cantidad: " + cantidad));
            //            documento.Add(new iTextSharp.text.Paragraph("Precio Total: $" + precioTotal));
            //            documento.Add(new iTextSharp.text.Paragraph("\n"));
            //        }

            //        MessageBox.Show("Reporte generado exitosamente en: " + rutaArchivoPDF);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error al generar el reporte: " + ex.Message);
            //    }
            //    finally
            //    {
            //        if (documento.IsOpen()) documento.Close();
            //    }
            //}
        }
        // Método para generar reporte de libros más prestados
        private void GenerarReporteLibrosMasPrestados(object sender, RoutedEventArgs e)
        {
            //// Mostrar el diálogo para que el usuario elija dónde guardar el archivo PDF
            //SaveFileDialog saveFileDialog = new SaveFileDialog();
            //saveFileDialog.Filter = "PDF Document (.pdf)|.pdf";
            //saveFileDialog.FileName = "ReporteLibrosMasPrestados.pdf";

            //if (saveFileDialog.ShowDialog() == true)
            //{
            //    string rutaArchivoPDF = saveFileDialog.FileName;

            //    try
            //    {
            //        // Crear un documento PDF
            //        iTextSharp.text.Document documento = new iTextSharp.text.Document();
            //        PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(rutaArchivoPDF, FileMode.Create));
            //        documento.Open();

            //        // Agregar título al PDF
            //        documento.Add(new iTextSharp.text.Paragraph("Reporte de Libros Más Prestados"));
            //        documento.Add(new iTextSharp.text.Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy")));
            //        documento.Add(new iTextSharp.text.Paragraph("\n"));

            //        // Crear una tabla PDF con 3 columnas (ID, Título, Cantidad de Préstamos)
            //        PdfPTable tablaPDF = new PdfPTable(3);
            //        tablaPDF.AddCell("ID Libro");
            //        tablaPDF.AddCell("Título");
            //        tablaPDF.AddCell("Cantidad de Préstamos");

            //        // Recorrer los datos del DataGrid y agregar a la tabla
            //        foreach (DataRowView row in dataGridLibrosMasPrestados.Items)
            //        {
            //            tablaPDF.AddCell(row["ID"].ToString());
            //            tablaPDF.AddCell(row["Titulo"].ToString());
            //            tablaPDF.AddCell(row["CantidadPrestamos"].ToString());
            //        }

            //        // Agregar la tabla al documento PDF
            //        documento.Add(tablaPDF);

            //        // Cerrar el documento PDF
            //        documento.Close();
            //        writer.Close();

            //        // Notificar al usuario que el reporte se generó exitosamente
            //        MessageBox.Show("Reporte generado exitosamente en: " + rutaArchivoPDF);
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Error al generar el reporte PDF: " + ex.Message);
            //    }
            //}
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