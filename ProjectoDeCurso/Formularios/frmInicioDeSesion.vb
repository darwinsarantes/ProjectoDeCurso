''1. utilizaremos una importacion..
''las realizaremos para poder utilizar los comando apropiados para
''viajar a sql y traer la informacion..

Imports System.Data
Imports System.Data.SqlClient

Public Class frmInicioDeSesion

#Region "Código del programador"

    ''Variables....
    Dim SqlConexion As SqlConnection
    Dim SqlComando As SqlCommand
    Dim SqlAdaptador As SqlDataAdapter
    Dim DTabla As DataTable

    ''' <summary>
    ''' Verificaremos la información del usuario dentro de nuestra
    ''' base de datos, para ello lo aremos utilizando el 
    ''' nombre del usaurio y la contrasena y
    ''' luego traeremos la informacion y la guardaremos en las
    ''' variables correspondientes....
    ''' 
    ''' </summary>
    Private Sub VerificarInformacionDelUsuario()

        Try

            ''1. realizaremos la conexion con sql.
            SqlConexion = New SqlConnection(TraerCadenaDeConexion())
            SqlConexion.Open()

            ''2. Utilizaremos el comando para poder realizar la consulta
            ''Correspondiente.
            SqlComando = New SqlCommand()
            ''Establecemos la conexion..
            SqlComando.Connection = SqlConexion
            ''Definimos como va a trabajar el comando...
            ''para este caso una consulta.
            SqlComando.CommandType = CommandType.Text

            ''Vamos a crear una variable que nos permita 
            ''crear la consulta... de manera limpia...
            Dim SQLConsulta As String
            SQLConsulta = String.Format("Select CodigoUsusario, NombreUsuario, r.Nombre as 'Rol' " &
                                        " from Usuario as u " &
                                        " inner join Rol as r on r.CodigoRol = u.CodigoRol " &
                                        " Where upper(trim( u.Login)) = '{0}' and Contrasena = '{1}'",
                                        txtUsuario.Text.Trim().ToUpper(), txtContrasena.Text)

            ''una ves que tenemos la consulta devemos, asignarla al
            ''comando. lo realizaremos atreves la propiedad commandtext
            SqlComando.CommandText = SQLConsulta

            ''Ejecutaremos el comando utilizando el adaptador
            ''ya que este devolvera datos...

            SqlAdaptador = New SqlDataAdapter()
            SqlAdaptador.SelectCommand = SqlComando

            ''ahora iniciaremos nuestra tabla.. para trabar con ella..
            DTabla = New DataTable()
            ''ahora la llenaremos asiendo uso del adaptador.
            SqlAdaptador.Fill(DTabla)

            ''luego devemos realizar una verificacion de la información
            ''es decir que se hayan devuelto datos...
            If DTabla.Rows.Count > 0 Then
                ''Presten algo de atencion para una mejor compresion
                ''los datos de la tabla los pasare a una sola fila.
                Dim Fila As DataRow = DTabla.Rows(0)

                Integer.TryParse(Fila("CodigoUsusario"), idUsuario)
                NombreDeUsuario = Fila("NombreUsuario").ToString()
                RolDelUsuario = Fila("Rol").ToString()

                ''En caso que si exista el usuario
                ''procederemos a cargar el mdi...
                MDIPrincipal.WindowState = FormWindowState.Maximized
                MDIPrincipal.Show()

                Me.Close()


            Else
                Throw New ArgumentException("Usuario ingresado no válido")
            End If


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Verificando informacion del usuario", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

#End Region

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click

        Try

            If String.IsNullOrEmpty(txtUsuario.Text) Or txtUsuario.Text.Trim().Length = 0 Then
                txtUsuario.Focus()
                Throw New ArgumentException("Se debe ingresar un valor de usuario valido")
            End If

            If String.IsNullOrEmpty(txtContrasena.Text) Or txtContrasena.Text.Trim().Length = 0 Then
                txtContrasena.Focus()
                Throw New ArgumentException("Se debe de ingresar una contraseña válida")
            End If

            VerificarInformacionDelUsuario()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Verificar información del usaurio", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Me.Close()
    End Sub

    Private Sub frmInicioDeSesion_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown

        If e.KeyCode = Keys.F2 Then

            Me.Hide()
            Dim ofrmSQLConexion As New frmSQLConexion()
            ofrmSQLConexion.StartPosition = FormStartPosition.CenterScreen
            ofrmSQLConexion.ShowDialog()

        End If

    End Sub
End Class
