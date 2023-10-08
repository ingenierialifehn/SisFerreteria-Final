Imports Microsoft.VisualBasic
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports System.Runtime.InteropServices
Imports Microsoft.Office.Interop.Excel

Module Conexion
    Public con As SqlConnection
    Public obtenerIdCliente As Integer = 1
    Public obtenerIdProducto As Integer
    Public obtenerTelefonoCliente As String
    Public unidad As Integer
    Public descripcion As String
    Public id_producto As Integer
    Public precio As Decimal
    Public nombreUnidad As String
    Sub AbrirConexion()

        Try
            con = New SqlConnection("Data Source=LAPTOP-RKM2ENQM\SQLEXPRESS;Initial Catalog=BDferreteria;Integrated Security=True")

            con.Open()

        Catch ex As Exception
            MsgBox("No se pudo conectar" + ex.ToString)
        End Try
    End Sub
    Sub ExportarDatosAExcel()
        ' Cadena de conexión a la base de datos SQL Server
        Dim connectionString As String = "Data Source=LAPTOP-RKM2ENQM\SQLEXPRESS;Initial Catalog=BDferreteria;Integrated Security=True"

        ' Consulta SQL para seleccionar los datos de la tabla
        Dim query As String = "SELECT * FROM detalle_factura"

        ' Crear una conexión SQL
        Using connection As New SqlConnection(connectionString)
            connection.Open()

            ' Crear un comando SQL
            Using cmd As New SqlCommand(query, connection)
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    ' Crear una instancia de Excel
                    Dim excelApp As New Application()
                    Dim workbook As Workbook = excelApp.Workbooks.Add()
                    Dim worksheet As Worksheet = workbook.Sheets(1)

                    ' Copiar los datos de SQL a Excel
                    Dim row As Integer = 1
                    Dim col As Integer = 1

                    For i As Integer = 0 To reader.FieldCount - 1
                        worksheet.Cells(row, col) = reader.GetName(i)
                        col += 1
                    Next

                    While reader.Read()
                        row += 1
                        col = 1

                        For i As Integer = 0 To reader.FieldCount - 1
                            worksheet.Cells(row, col) = reader(i)
                            col += 1
                        Next
                    End While

                    ' Guardar el archivo de Excel
                    workbook.SaveAs("C:\Users\Annonymous\Documents\Fernando\DetalleFactura.xlsx")

                    ' Cerrar y liberar recursos
                    workbook.Close()
                    excelApp.Quit()
                    Marshal.ReleaseComObject(workbook)
                    Marshal.ReleaseComObject(excelApp)

                    MsgBox("Datos exportados exitosamente a Excel.")
                End Using
            End Using
        End Using
    End Sub

End Module
