Imports System.Data.SqlClient

Public Class MainDashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Session("P1") Is Nothing AndAlso Session("P2") Is Nothing Then ' Check UserName And Password 
                Response.Redirect("login.aspx", False)
            Else
                'Dim connectionString As String = "Data Source=GDITLN008;Initial Catalog=RND;Integrated Security=True"
                'Dim connectionString As String = "Server=OZOTSSVR\OZROLL;Database=smartbase16March2022;User ID=smartbase;Password=smartbase;Trusted_Connection=False;Connect Timeout=360"
                Dim query As String = "SELECT *  FROM DashboardLink where Active=1"
                ' Create a DataTable to store the data from the SQL table
                Dim dataTable As New DataTable()
                Dim dbcon As New DBConnection
                Using connection As New SqlConnection(dbcon.getConnection_To_ozots())
                    Using command As New SqlCommand(query, connection)
                        connection.Open()

                        ' Read the data from the database into the DataTable
                        Using adapter As New SqlDataAdapter(command)
                            adapter.Fill(dataTable)
                        End Using
                    End Using
                End Using
                ' Bind the DataTable to the Repeater

                DataList3.DataSource = dataTable
                DataList3.DataBind()
            End If
        End If
    End Sub

    Protected Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Session.Abandon()
        Response.Redirect("Login.aspx", False)
    End Sub
    Protected Sub DataList3_ItemCommand(source As Object, e As DataListCommandEventArgs)
        If e.CommandName = "addtocart" Then
            Dim LinkButton As Button = TryCast(e.CommandSource, Button)


            Dim linkWebUrl As LinkButton = CType(e.Item.FindControl("linkWebUrl"), LinkButton)
            Dim linkWebLinkId As LinkButton = CType(e.Item.FindControl("linkWebLinkId"), LinkButton)
            Dim commandText As String = linkWebUrl.Text

            'Dim btnRollShield As Button = CType(e.Item.FindControl("btnRollShield"), Button)

            Dim strWebLinkId As String = linkWebLinkId.Text

            Dim strP1 As String = Session("P1") ' User Name 
            Dim strP2 As String = Session("P2") ' Password
            Dim strP4 As String = Session("P4") ' Passsword Type
            lblMessgaeStatus.Text = ""

            'btnRollShield.Enabled = False

            Dim query As String = ""
            If strWebLinkId = "1" Then
                query = "select * from customer_login where username='" + strP1 + "' and LouvresUserId is not null"
                'btnRollShield.Enabled = True

            End If
            If strWebLinkId = "2" Then
                query = "select * from customer_login where username='" + strP1 + "' and PlantationUserId is not null"
                'btnRollShield.Enabled = True
            End If

            If strWebLinkId = "4" Or strWebLinkId = "3" Then
                query = "select * from customer_login where username='" + strP1 + "'"
                'btnRollShield.Enabled = True
            End If

            If query IsNot Nothing Then


                ' Create a DataTable to store the data from the SQL table
                Dim dataTable As New DataTable()
                Dim dbcon As New DBConnection
                Using connection As New SqlConnection(dbcon.getConnection_To_ozots())
                    Using command As New SqlCommand(query, connection)
                        connection.Open()

                        ' Read the data from the database into the DataTable
                        Using adapter As New SqlDataAdapter(command)
                            adapter.Fill(dataTable)
                            If dataTable.Rows.Count = 0 Then
                                'MsgBox("You are not authorized to access this web page.")
                                lblMessgaeStatus.Text = "You are not authorized to access this web page."
                                Return
                            End If
                        End Using
                    End Using
                End Using
                Dim strCustomerId As String = ""
                Dim strCurrentId As String = ""

                'If strWebLinkId = "1" And strWebLinkId = "2" Then
                'strCustomerId = dataTable.Rows(0)("ProductTypeID")
                'Else
                strCustomerId = dataTable.Rows(0)("customer_id")
                '    End If
                If strWebLinkId = "1" Then
                    strCurrentId = dataTable.Rows(0)("LouvresUserId")
                End If
                If strWebLinkId = "2" Then
                    strCurrentId = dataTable.Rows(0)("PlantationUserId")
                End If

                If strWebLinkId = "4" Or strWebLinkId = "3" Then
                    strCurrentId = dataTable.Rows(0)("ID")
                    Dim str As String = Convert.ToString(dataTable.Rows(0)("IsShutterLogin"))
                    If (String.IsNullOrEmpty(str)) Then
                        lblMessgaeStatus.Text = "You are not authorized to access this web page."
                        Return
                    End If
                End If

                ' Dim wrapper1 As New Simple3Des(strP1)
                Dim cipherText1 As String = EncryptString(strP1) 'User Name

                ' Dim wrapper2 As New Simple3Des(strP2)
                Dim cipherText2 As String = EncryptString(strP2) 'Password

                Dim strDateTime = System.DateTime.Now.AddMinutes(2) ' DateTime
                ' Dim wrapper3 As New Simple3Des(strDateTime)

                Dim cipherText3 As String = EncryptString(strDateTime)

                Dim cipherText4 As String = EncryptString(strCustomerId)
                Dim cipherText5 As String = EncryptString(strCurrentId)


                Dim plainText1 As String = DecryptString(cipherText1)
                Dim plainText2 As String = DecryptString(cipherText2)
                Dim plainText3 As String = DecryptString(cipherText3)
                Dim plainText4 As String = DecryptString(cipherText4)

                Dim plainText5 As String = DecryptString(cipherText5)

                Dim finalEncryptURL = commandText + "?P1=" + cipherText1 + "&P2=" + cipherText2 + "&P3=" + cipherText3 + "&ID=" + cipherText4 + "&CID=" + cipherText5 + ""
                Dim finalDecryptURL = commandText + "?P1=" + plainText1 + "&P2=" + plainText2 + "&P3=" + plainText3 + "&ID=" + plainText4 + "&CID=" + plainText5 + ""
                ' P1=User Name
                ' P2= Password
                ' P3=Datetime+ ADD 2 Minitue
                ' P4= customer_id

                'Dim finalEncryptURL = commandText + "?P1=" + cipherText1 + "&P2=" + cipherText2 + "&P3=" + cipherText3 + "&ID=" + cipherText4 + ""
                'Dim finalDecryptURL = commandText + "?P1=" + plainText1 + "&P2=" + plainText2 + "&P3=" + plainText3 + "&ID=" + plainText4 + ""

                Try
                    ''Process.Start(finalEncryptURL)
                    Response.Redirect(finalEncryptURL)
                Catch ex As Exception
                    ''Process.Start(finalEncryptURL)
                    Response.Redirect(finalEncryptURL)
                End Try
            End If


        End If
    End Sub

    Public Function EncryptString(ByVal text As String) As String
        Dim encrypted As String = ""
        Dim bytes As Byte()
        Try
            bytes = New Byte(text.Length - 1) {}
            bytes = System.Text.Encoding.UTF8.GetBytes(text)
            encrypted = Convert.ToBase64String(bytes)
        Catch ex As Exception
            encrypted = ""
        End Try

        Return encrypted
    End Function
    Public Function DecryptString(ByVal encrString As String) As String
        Dim bytes As Byte()
        Dim decrypted As String
        Try
            bytes = Convert.FromBase64String(encrString)
            decrypted = System.Text.ASCIIEncoding.ASCII.GetString(bytes)
        Catch fe As FormatException
            decrypted = ""
        End Try

        Return decrypted
    End Function

    Protected Sub DataList3_ItemDataBound(sender As Object, e As DataListItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            ' Find the control by its ID
            Dim linkWebLinkId As LinkButton = CType(e.Item.FindControl("linkWebLinkId"), LinkButton)
            Dim btnRollShield As Button = CType(e.Item.FindControl("btnRollShield"), Button)
            Dim strWebLinkId As String = linkWebLinkId.Text
            btnRollShield.Enabled = False
            Dim strP1 As String = Session("P1") ' User Name 
            Dim query As String = ""
            'If strWebLinkId = "1" Then
            '    query = "select * from customer_login where username='" + strP1 + "' and LouvresUserId is not null"
            '    'btnRollShield.Enabled = True

            'End If
            'If strWebLinkId = "2" Then
            '    query = "select * from customer_login where username='" + strP1 + "' and PlantationUserId is not null"
            '    'btnRollShield.Enabled = True
            'End If

            ' If strWebLinkId = "4" Or strWebLinkId = "3" Then
            query = "select * from customer_login where username='" + strP1 + "'"
            'btnRollShield.Enabled = True
            ' End If

            If query IsNot Nothing Then
                ' Create a DataTable to store the data from the SQL table
                Dim dataTable As New DataTable()
                Dim dbcon As New DBConnection
                Using connection As New SqlConnection(dbcon.getConnection_To_ozots())
                    Using command As New SqlCommand(query, connection)
                        connection.Open()

                        ' Read the data from the database into the DataTable
                        Using adapter As New SqlDataAdapter(command)
                            adapter.Fill(dataTable)
                            If dataTable IsNot Nothing AndAlso dataTable.Rows.Count > 0 Then
                                'MsgBox("You are not authorized to access this web page.")
                                If strWebLinkId = "1" Then
                                    If (Convert.ToString(dataTable.Rows(0)("LouvresUserId")).Length > 0) Then
                                        ' Else
                                        btnRollShield.Enabled = True
                                    End If
                                End If

                                If strWebLinkId = "2" Then
                                    If (Convert.ToString(dataTable.Rows(0)("PlantationUserId")).Length > 0) Then
                                        'Else
                                        btnRollShield.Enabled = True
                                    End If
                                End If
                                If strWebLinkId = "4" Or strWebLinkId = "3" Then
                                    If (Convert.ToString(dataTable.Rows(0)("IsShutterLogin")).Length > 0) Then
                                        'Else
                                        btnRollShield.Enabled = True
                                    End If
                                    'If (Convert.ToString(dataTable.Rows(0)("SlideTrackUserId")).Length > 0) Then
                                    '    'Else
                                    '    btnRollShield.Enabled = True
                                    'End If
                                End If
                            Else
                                ' btnRollShield.Enabled = False

                            End If
                        End Using
                    End Using
                End Using
            End If


        End If
    End Sub


End Class