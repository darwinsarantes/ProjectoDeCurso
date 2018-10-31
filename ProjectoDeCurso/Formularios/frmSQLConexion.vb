Public Class frmSQLConexion

    ''primero definiremos regiones de trabajo.
#Region "Funciones del programador"

    ''Crearemos una funcion que nos permita cargar los datos guardados
    ''en las variables del sistemas.
    Public Sub traerInformacionDelServidor()

        Try ''utilizaremos un try catch para eviatar errores.

            txtServidor.Text = My.Settings.Servidor
            txtUsuario.Text = My.Settings.Usuario
            txtContrasena.Text = My.Settings.Contrasena
            txtBaseDeDatos.Text = My.Settings.BaseDeDatos

        Catch ex As Exception
            ''en caso de erro mandamos un mensaje al usuario.
            MessageBox.Show(ex.Message, "Traer información del usuario", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Public Sub GuardarInformacionDelServidor()
        Try ''ahora realizaremos el proceso inverso guardar la informacion
            ''dentro del sistema.

            My.Settings.Servidor = txtServidor.Text.Trim()
            My.Settings.Usuario = txtUsuario.Text.Trim()
            My.Settings.Contrasena = txtContrasena.Text.Trim()
            My.Settings.BaseDeDatos = txtBaseDeDatos.Text.Trim()
            My.Settings.Save()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Traer informacion del servidor", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function GenerarCadenaDeConexion() As String
        Dim CadenaDeConexion As String = String.Format("Data Soucer='{0}';Initial Catalog='{1}';Persist Security Info=True;User ID= '{2}';Password='{3}'", txtServidor.Text.Trim(), txtBaseDeDatos.Text.Trim(), txtUsuario.Text.Trim(), txtContrasena.Text.Trim())
        Return CadenaDeConexion
    End Function

    ''ahora debemos de crear una funcion que nos permita realizar un test
    ''y darnos cuentas si se logro conextar o no.
    Private Function TestDeConexionConELServidor() As Boolean

        Try
            ''aqui nos falta la cadena de conexion. ahora ya pusimos la cadena de conexion
            Dim SqlConexion As New System.Data.SqlClient.SqlConnection(GenerarCadenaDeConexion())
            SqlConexion.Open()

            Return True

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Test de conexion", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try

    End Function

    ''Procederemos a crear una funcion que nos permita evaluar la conexion y guardarla.
    Private Sub ConexionSQL()

        Try

            If TestDeConexionConELServidor() Then
                GuardarInformacionDelServidor()
                Me.Close()
                frmInicioDeSesion.Show()
            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Conexion a SQL", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

#End Region

    Private Sub frmSQLConexion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        traerInformacionDelServidor()
    End Sub

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click
        ConexionSQL()
    End Sub
End Class
