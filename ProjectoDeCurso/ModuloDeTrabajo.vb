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

End Module
