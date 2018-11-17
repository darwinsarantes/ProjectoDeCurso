
''IMPORTACIONES DE LAS LIBRERIA PARA USAR SQLSERVER 
Imports System.Data
Imports System.Data.SqlClient

Public Class frmBodega
    ''DEFINIREMOS LAS VARIABLES GLOBALES A LA CLASE...
    Dim ConexionSQL As SqlConnection
    Dim Comando As SqlCommand
    Dim Adaptador As SqlDataAdapter
    Dim DTable As DataTable
    ''VARIABLE PARA CAPTURAR ERRORES.
    Dim Errores As String
    Dim Consultas As String ''la usaremos para consultas.. 

    ''PARA TRABAJAR ORDENADOS VAMOS CREAR REGIONES...
#Region "Funciones definidas por el programador"

    Private Function ListadoDeRegistros() As Boolean

        Try
            ''Establecemos la conexion con sql. y procedemos a abrirla.
            ConexionSQL = New SqlConnection(TraerCadenaDeConexion())
            ConexionSQL.Open()

            ''procedemos conextar el comando....
            Comando = New SqlCommand()
            ''establecemos la conexion.
            Comando.Connection = ConexionSQL
            ''tipo de conexion del comando....
            Comando.CommandType = CommandType.Text

            ''voy a crear una consulta sencilla por el momento luego la modificaremos
            Consultas = "Select * from bodega where CodigoBodega > 0 "
            Comando.CommandText = Consultas

            ''inicializaremos el adaptador para ejecutar la consutla dentro del comando...
            Adaptador = New SqlDataAdapter
            Adaptador.SelectCommand = Comando
            ''inicializaremos la tabla...
            DTable = New DataTable
            DTable.Clear()

            ''ahora la asignaremos con el adaptador.
            Adaptador.Fill(DTable)

            Return True

        Catch ex As Exception
            Return False
            MessageBox.Show(ex.Message, "Llenar listado", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Finally
            ''vamos evaluar que la variable no este vacia
            ''no estarlo verificamos que el estado de la conexion
            ''sea abierta y luego procederemos a cerrala.
            If Not IsNothing(ConexionSQL) Then
                If ConexionSQL.State = ConnectionState.Open Then
                    ConexionSQL.Close()
                End If
            End If

            Comando = Nothing
            Adaptador = Nothing
        End Try

    End Function

    Private Sub LlenarListado()

        Try

            If ListadoDeRegistros() Then

                dgvListar.DataSource = DTable

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Llenar listado", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

#End Region
    Private Sub tsbFiltro_Click(sender As Object, e As EventArgs) Handles tsbFiltro.Click
        LlenarListado()
    End Sub
End Class