Module ModuloDeTrabajo

#Region "Variables Globales"

    Public idUsuario As Integer
    Public NombreDeUsuario As String
    Public RolDelUsuario As String

#End Region

    ''Primero crearemos una funcion que nos permita 
    ''traer la variables que creamos al inicio del proyecto y con ellas 
    ''logearnos.

    ''' <summary>
    ''' Permite crear la cadena conexion correspondiente con sql
    ''' y nos debuelve una cadena
    ''' </summary>
    ''' <returns>string</returns>
    Public Function TraerCadenaDeConexion() As String

        Dim CadenaDeConexion As String
        CadenaDeConexion = String.Format("Data Source='{0}';Initial Catalog='{1}';Persist Security Info=True;User ID= '{2}';Password='{3}'", My.Settings.Servidor, My.Settings.BaseDeDatos, My.Settings.Usuario, My.Settings.Contrasena)
        Return CadenaDeConexion

    End Function
    ''' <summary>
    ''' Nos permite ocultar la columna
    ''' </summary>
    ''' <param name="ColumnasAOcultar">string</param>
    ''' <param name="DGV">DataGridView</param>
    Public Sub OcultarColumnasEnElDGV(ByVal ColumnasAOcultar As String, ByVal DGV As DataGridView)

        Try
            ''Evaluamos que el dgv tenga columnas asignadas.. y que tambien existan columnas a ocultar
            If DGV.Columns.Count > 0 And ColumnasAOcultar.Trim().Length > 0 Then
                ''creamos una arreglo apartir de las columnas que se dieron separadas por coma.
                Dim ArrayColumnas() As String = ColumnasAOcultar.Split(",")
                ''Creamos un foreach para recorrer el arreglo de tipo string.
                For Each c As String In ArrayColumnas
                    ''creamos un for each que nos permite recorrer las columnas que se hayan creado en el dgv.
                    For Each c1 As DataGridViewColumn In DGV.Columns
                        If c1.Name.Trim().ToUpper() = c.Trim().ToUpper() Then
                            c1.Visible = False
                        End If
                    Next
                Next

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Ocultar columnas en el datagrdiview", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

End Module
