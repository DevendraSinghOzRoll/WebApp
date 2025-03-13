Public Class DBConnection
    Public Function getConnection_To_ozots() As String
        Dim w_Server As String
        Dim w_Database As String
        Dim w_Username As String = "Modernro"
        Dim w_Password As String = "EZGnVMP}#(X58df"

        'If Constants.LIVE_SITE Then
        '    w_Server = "OZOTSSVR\OZROLL"
        '    w_Database = "smartbase"
        'Else
        w_Server = "OZsasql2"
        'w_Database = "smartbasetest"
        'w_Database = "ozots_7Oct2021"
        w_Database = "ozots_12Oct2023"


        Dim connectionString As String = "Server=" & w_Server & ";Database=" & w_Database & ";User ID=" & w_Username & ";Password=" & w_Password & ";Trusted_Connection=False;Connect Timeout=60"
        Return connectionString
    End Function
End Class
