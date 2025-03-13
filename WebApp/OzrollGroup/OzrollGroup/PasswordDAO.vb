Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Security.Cryptography
Imports Microsoft.VisualBasic

Public Class PasswordDAO
    Inherits System.Web.UI.Page
    Implements System.IDisposable
    Private Shared ReadOnly RandomNumberGenerator As RandomNumberGenerator = RandomNumberGenerator.Create()
    Private Const Interations As Integer = 10000
    Private Const FixedSalt As String = "3CA4988C0E421123"
    Function Hash(value As String, salt As String) As String
        Dim pbkdf2 = New Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(value), StringToByteArray(FixedSalt + salt), Interations)
        Dim key = pbkdf2.GetBytes(24)

        Return Convert.ToBase64String(key)
    End Function

    Function Hash(value As String, salt As Integer) As String
        Return Hash(value, salt.ToString(CultureInfo.InvariantCulture))
    End Function

    Function GenerateSalt(Optional size As Integer = 8) As String
        Dim salt = New Byte(size - 1) {}
        RandomNumberGenerator.GetNonZeroBytes(salt)
        Return ByteToHexBitFiddle(salt)
    End Function
    Function GenerateToken(Optional size As Integer = 16) As String
        Dim salt = New Byte(size - 1) {}
        RandomNumberGenerator.GetNonZeroBytes(salt)
        Return ByteToHexBitFiddle(salt)
    End Function
    Function ByteToHexBitFiddle(bytes As Byte()) As String
        Dim c As Char() = New Char(bytes.Length * 2 - 1) {}
        For i = 0 To bytes.Length - 1
            Dim b = bytes(i) >> 4
            c(i * 2) = ChrW(55 + b + (((b - 10) >> 31) And -7))
            b = bytes(i) And &HF
            c(i * 2 + 1) = ChrW(55 + b + (((b - 10) >> 31) And -7))
        Next
        Return New String(c)
    End Function

    Function StringToByteArray(hex As [String]) As Byte()
        Dim numberChars = hex.Length / 2
        Dim bytes As Byte() = New Byte(numberChars - 1) {}
        Using sr = New StringReader(hex)
            For i = 0 To numberChars - 1
                bytes(i) = Convert.ToByte(New String(New Char(1) {ChrW(sr.Read()), ChrW(sr.Read())}), 16)
            Next
        End Using
        Return bytes
    End Function
    'Function getUserSalt(UserName As String) As String
    '    Dim dbConn As New DBConnection
    '    Dim cnn As New SqlConnection(dbConn.getConnection_To_Smartbase())
    '    Dim cmd As New SqlCommand, strSalt As String = ""
    '    Dim Login_Type As String = ""

    '    Try
    '        cnn.Open()
    '        cmd.Connection = cnn
    '        cmd.CommandText = "select salt, Login_Type from OneLoginDetails where username = '" + UserName + "'"
    '        cmd.CommandType = CommandType.Text

    '        strSalt = cmd.ExecuteScalar()
    '        cnn.Close()

    '        Return strSalt
    '    Catch ex As Exception
    '        Return strSalt
    '    Finally
    '        cmd.Dispose()
    '        cmd = Nothing
    '        cnn.Dispose()
    '        cnn = Nothing
    '    End Try

    'End Function

End Class
