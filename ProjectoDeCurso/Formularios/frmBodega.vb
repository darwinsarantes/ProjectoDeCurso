
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
            Consultas = String.Format("Select CodigoBodega, Nombre, Observacion, Descripcion from bodega where CodigoBodega > 0 {0}", WhereDinamico())
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

    Private Function WhereDinamico() As String
        Try
            Dim Where As String = ""
            If chkBodegaPrincipal.CheckState = CheckState.Checked And String.IsNullOrEmpty(txtFCodigoDeBodega.Text) = False And txtFCodigoDeBodega.Text.Trim.Length > 0 Then
                Where &= String.Format(" and CodigoBodega = {0}", txtFCodigoDeBodega.Text.Trim)
            End If

            If chkNombre.CheckState = CheckState.Checked And String.IsNullOrEmpty(txtFNombre.Text) And txtFNombre.Text.Trim.Length > 0 Then
                Where &= String.Format(" AND Nombre like '%{0}%'", txtFNombre.Text)
            End If

            If chkObservaciones.CheckState = CheckState.Checked And String.IsNullOrEmpty(txtFObservaciones.Text) = False And txtFObservaciones.Text.Trim.Length > 0 Then
                Where &= String.Format(" and Observaciones Like '%{0}%'", txtFObservaciones.Text)
            End If

            Return Where

        Catch ex As Exception
            Return ""
            MessageBox.Show(ex.Message, "Filtro de busqueda", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Function

    Private Sub FormatearDGV()

        Try
            ''lo que vamos a realizar es un formateo del datagridview...
            ''primero desactivaremos algunas operaciones.. que por defecto 
            ''bienen activadas...
            With dgvListar
                ''desctivamos la opcion que nos permite agregar rows o filas...
                .AllowUserToAddRows = False
                ''desactivamos la opcion que nos permite quitar una fila o eliminarla
                .AllowUserToDeleteRows = False
                ''esta opcion nos permite redimencionar las filas...
                .AllowUserToResizeRows = False
                ''esta opcion nos permite colocar un tipo de fuente por defecto al datagridview y un tamano.
                .DefaultCellStyle.Font = New Font("Segoe UI", 8)
                ''para que todas las columnas tenga el mismo tipo de fuente
                .ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 8)
                ''con esta opcion podemos poner un solo color al fondo del datagridview.
                .DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue
                ''aplicamos fondo al datagridview, el anterior es para cuando se seleciona..
                .BackgroundColor = System.Drawing.SystemColors.Window
                ''aqui podemos poner el borde al datagridview.
                .BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
                ''ponemos un tamano del header o encabezados en 25
                .RowHeadersWidth = 25
                ''con esta opcion nos permitimos poner todos los encabezados del datagridview en el centro.
                .ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                ''con esta opcion lo que pretendemos es seleccionar toda la fila..
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect
                ''nos permite manejar el tab en el datadrigview.
                .StandardTab = True
                ''desactivamos la opcion de escritura en la celda..
                .ReadOnly = False
                ''aplicamos un estilo de borde tridimensional.
                .CellBorderStyle = DataGridViewCellBorderStyle.Raised

                Dim columnasacultar As String = ""
                OcultarColumnasEnElDGV(columnasacultar, dgvListar)
                FormatoDeColumnasDelDGV()

            End With


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Formato del Data gridview", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub FormatoDeColumnasDelDGV()

        Try

            ''para aplicar los formatos de colunas tenemos que identificar las columnas que mostraremos..
            ''para mas o menos tener una nocion de utilidad de estas...
            ''ejemplo..  CodigoBodega, Nombre, Observacion, Descripcion
            With dgvListar
                ''con esta instruccion tenemos acceso al nombre de la columna dentro del datagridview.
                ''esto no permite cambiarle el nombre de manera visual entre otras coas..
                .Columns("CodigoBodega").HeaderText = "Código"
                .Columns("CodigoBodega").Width = "50" ''Podemos asignar un tamano a la celda.
                .Columns("CodigoBodega").ReadOnly = True ''Podemos decidir si se escribe o no en la celda.
                ''esta instruccion nos permite decidir la posicion del texto, centrado, izquier, derecha dentro del encabezado
                .Columns("CodigoBodega").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                ''esta indica la posicion dentro de la celda del texto.
                .Columns("CodigoBodega").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ''esto mismo lo aplicaremos para cada una de las columnas...
                ''presten atencion...

                .Columns("Nombre").HeaderText = "Bodega"
                .Columns("Nombre").Width = "160" ''Podemos asignar un tamano a la celda.
                .Columns("Nombre").ReadOnly = True ''Podemos decidir si se escribe o no en la celda.
                .Columns("Nombre").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("Nombre").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("Observacion").HeaderText = "Observación"
                .Columns("Observacion").Width = "200" ''Podemos asignar un tamano a la celda.
                .Columns("Observacion").ReadOnly = True ''Podemos decidir si se escribe o no en la celda.
                .Columns("Observacion").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("Observacion").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

                .Columns("Descripcion").HeaderText = "Descripción"
                .Columns("Descripcion").Width = "200" ''Podemos asignar un tamano a la celda.
                .Columns("Descripcion").ReadOnly = True ''Podemos decidir si se escribe o no en la celda.
                .Columns("Descripcion").HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Columns("Descripcion").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            End With

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Formato de columnas del datagridview", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub LlenarListado()

        Try

            If ListadoDeRegistros() Then

                dgvListar.DataSource = DTable
                FormatearDGV()

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