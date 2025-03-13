Imports System.Data.SqlClient

Public Class AppService
#Region "Services"
    Function getHashPassword(UserName As String, SaltValue As String) As String
        Dim ServiceDAO As New PasswordDAO
        Return ServiceDAO.Hash(UserName, SaltValue)
    End Function

    Function GetUserSalt(UserName As String) As UserInfo
        Dim result As New UserInfo()

        Dim dbConn As New DBConnection
        ' Dim connectionString As String = "YourConnectionString"
        Dim query As String = "SELECT salt, salt as Password_Type FROM customer_login  WHERE username = '" + UserName + "'"

        Using connection As New SqlConnection(dbConn.getConnection_To_ozots())
            connection.Open()
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@username", UserName)

                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        result.Salt = Convert.ToString(reader("Salt"))
                        result.Password_Type = Convert.ToString(reader("Password_Type"))
                    End If
                End Using
            End Using
        End Using

        Return result
    End Function

#End Region

End Class
