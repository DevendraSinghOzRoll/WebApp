
Imports System.Data.SqlClient

Partial Class CustomerSampleTracking
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not IsPostBack Then

            If Session("sessUserID") = String.Empty Then
                Response.Redirect("Logout.aspx", False)
                Exit Sub
            Else
                If Not IsNumeric(Session("sessUserID")) Then
                    Response.Redirect("Logout.aspx", False)
                    Exit Sub
                End If
            End If

            'Populate Un-Shipped Items
            PopulateCustomerSample()

            'Set Delivery Date Coloumn
            SetDeliveryDateColumn(False)

        End If

    End Sub

    Private Function getPageInfo() As String
        Dim strPageInfo As String = String.Empty

        Dim strName As String = String.Empty
        If Session.Contents.Count > 0 Then
            strPageInfo &= "Session Variables" & Environment.NewLine
            For Each strName In Session.Contents
                strPageInfo &= strName & ": " & CStr(Session.Contents(strName)) & Environment.NewLine
            Next
        Else
            strPageInfo &= "No Session Variables" & Environment.NewLine
        End If

        strPageInfo &= Environment.NewLine

        If Me.formCustomerSampleTracking.HasControls Then
            strPageInfo &= "Form Controls" & Environment.NewLine
            getPageControls(Me.formCustomerSampleTracking, strPageInfo)
        Else
            strPageInfo &= "No Form Controls" & Environment.NewLine
        End If

        Return strPageInfo
    End Function

    Private Sub getPageControls(ByVal ctrl As Control, ByRef strPageControls As String)
        If ctrl.HasControls Then
            For Each childCtrl As Control In ctrl.Controls
                getPageControls(childCtrl, strPageControls)
            Next
        Else
            Select Case ctrl.GetType.Name
                Case "TextBox"
                    Dim frmTxt As TextBox
                    frmTxt = DirectCast(ctrl, TextBox)
                    strPageControls &= frmTxt.ID & ": " & Left(frmTxt.Text, 100) & Environment.NewLine
                Case "DropDownList"
                    Dim frmCbo As DropDownList
                    frmCbo = DirectCast(ctrl, DropDownList)
                    If frmCbo.Items.Count > 0 Then
                        strPageControls &= frmCbo.ID & ": " & frmCbo.SelectedItem.Text & " (" & frmCbo.SelectedValue & ")" & Environment.NewLine
                    Else
                        strPageControls &= frmCbo.ID & ": Not Populated" & Environment.NewLine
                    End If
                Case "CheckBox"
                    Dim frmChk As CheckBox
                    frmChk = DirectCast(ctrl, CheckBox)
                    strPageControls &= frmChk.ID & ": " & frmChk.Checked & Environment.NewLine
                Case "RadioButton"
                    Dim frmRdo As RadioButton
                    frmRdo = DirectCast(ctrl, RadioButton)
                    strPageControls &= frmRdo.ID & ": " & frmRdo.Checked & Environment.NewLine
            End Select
        End If
    End Sub

    Protected Sub btnHome_Click(sender As Object, e As System.EventArgs) Handles btnHome.Click

        Response.Redirect("Home.aspx", False)

    End Sub

    Protected Sub btnLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogout.Click

        Response.Redirect("Logout.aspx", False)

    End Sub


    Private Sub PopulateCustomerSample(Optional ByVal bolShowShip As Boolean = False)

        'Service
        Dim service As New AppService

        'SQL For DataGridView
        Dim strSQL As String = "Select csam.OrderID, csam.JobNotes, csam.FirstName, csam.lastName, "
        strSQL &= "  csam.Phone as PhoneNumber, csam.Mobile As MobileNumber,csam.Email,col.ColourName As Colours, "
        strSQL &= " case when csam.ProductTypeID = 1 then 'Plantation' when csam.ProductTypeID = 2 then 'Louvre' end as Product, "
        strSQL &= " Convert(date, csam.ReceivedDate, 106) As ReceivedDate, Convert(date,csam.ShippedDate, 106) As ShippedDate, Convert(date,csam.RequestedDate, 106) As RequestedDate, "
        strSQL &= " case when csam.ShippedDate Is NULL then 0 when csam.ShippedDate Is Not NULL then 1 end as shipped, "
        strSQL &= " csam.ConsignmentNumber as ConNote, csam.Courier, csam.street, csam.suburb, csam.postcode, csam.state "
        strSQL &= " from tblCustomerSample csam "
        strSQL &= " left join tblColours col on csam.Colour=col.ColourID "
        strSQL &= " where csam.cancelled = 0 and csam.ProductTypeID = 2 "

        If Not bolShowShip Then
            strSQL &= " and csam.ShippedDate Is Null "
        Else
            strSQL &= " and csam.ShippedDate Is Not Null "
        End If

        'Populate Datatable
        Dim dtCustomerSample As DataTable = service.RunSQLScheduling(strSQL)

        'Check Row Count
        If dtCustomerSample.Rows.Count > 0 Then

            ViewState("dtCustomerSample") = dtCustomerSample

            'Bind To DataGridView
            dgvCustomerSample.DataSource = dtCustomerSample
            dgvCustomerSample.DataBind()

            If bolShowShip Then
                dgvCustomerSample.Columns(14).Visible = bolShowShip
                dgvCustomerSample.Columns(15).Visible = bolShowShip
                dgvCustomerSample.Columns(16).Visible = Not bolShowShip
                dgvCustomerSample.Columns(17).Visible = Not bolShowShip
                dgvCustomerSample.Columns(18).Visible = bolShowShip
            Else
                dgvCustomerSample.Columns(14).Visible = bolShowShip
                dgvCustomerSample.Columns(15).Visible = bolShowShip
                dgvCustomerSample.Columns(16).Visible = Not bolShowShip
                dgvCustomerSample.Columns(17).Visible = Not bolShowShip
                dgvCustomerSample.Columns(18).Visible = bolShowShip
            End If

        Else
            'Clear DataGridView
            dgvCustomerSample.DataSource = Nothing
            dgvCustomerSample.DataBind()
        End If

        'Dispose Service
        service = Nothing

    End Sub

    Protected Sub btnMarkAsShipped_Click(sender As Object, e As EventArgs) Handles btnMarkAsShipped.Click
        UpdateRecords()
    End Sub

    Private Sub UpdateRecords()

        'Connection
        Dim service As New AppService
        Dim dbConn As New DBConnection
        Dim cnn As SqlConnection = dbConn.getSQLConnection_To_OzRollLouvreScheduling
        Dim trans As SqlTransaction = Nothing
        Dim bolValid As Boolean = False

        dbConn = Nothing
        lblStatus.Text = String.Empty

        Try

            cnn.Open()

            trans = cnn.BeginTransaction

            'Iterate Each Row
            For Each Row In dgvCustomerSample.Rows

                Dim strConNote As String = Row.FindControl("txtConNote").Text
                Dim strCourier As String = Row.FindControl("txtCourier").Text

                If strConNote <> String.Empty And strCourier <> String.Empty Then

                    'Declare New Customer Sample
                    Dim clsCustomerSample As New CustomerSample

                    'Get Order ID
                    Dim intOrderID As Integer = CInt(dgvCustomerSample.DataKeys(Row.RowIndex).Values("OrderID"))

                    'Fill Class
                    FillCustomerSample(clsCustomerSample, strConNote, strCourier, intOrderID)

                    'Update
                    bolValid = service.UpdateCustomerSampleTracking(clsCustomerSample, cnn, trans)

                    'Check If Update Is Valid
                    If Not bolValid Then
                        Exit For
                    End If

                End If
            Next

            If bolValid = True Then
                trans.Commit()
            Else
                trans.Rollback()
                lblStatus.Text = "Unable to Update Customer Samples"
            End If

        Catch ex As Exception
            If Not trans Is Nothing Then
                trans.Rollback()
            End If
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message & Environment.NewLine & getPageInfo())
            bolValid = False
        Finally
            trans.Dispose()
            trans = Nothing
            If cnn.State = ConnectionState.Open Then
                cnn.Close()
            End If
            cnn.Dispose()
            cnn = Nothing
        End Try

        'Repopulate Data
        If bolValid Then
            lblStatus.Text = ""
            PopulateCustomerSample(False)
            btnMarkAsShipped.Enabled = False
            btnMarkAsShipped.CssClass = "form-button-disabled"
        End If

        service = Nothing


    End Sub

    Private Sub FillCustomerSample(ByRef clsCustomerSample As CustomerSample, strCourier As String, strConsignment As String, ByVal intOrderID As Integer)

        'Fill Customer Sample Details
        clsCustomerSample.OrderID = intOrderID
        clsCustomerSample.ShippedDate = Date.Now
        clsCustomerSample.ConsignmentNumber = strConsignment
        clsCustomerSample.Courier = strCourier

    End Sub

    Protected Sub rdoShipped_CheckedChanged(sender As Object, e As EventArgs) Handles rdoShipped.CheckedChanged

        If rdoShipped.Checked Then

            'Set Shipped
            Dim bolShipped As Boolean = True

            'Check Radio Button
            CheckRadioButton(bolShipped)

        End If

    End Sub

    Private Sub CheckRadioButton(ByVal bolShipped As Boolean)


        'Get Where Shipped Is True
        PopulateCustomerSample(bolShipped)

        'Set Delivery Date Coloumn
        SetDeliveryDateColumn(bolShipped)

        'Set Mark As Shipped Button
        SetMarkAsShipped(Not bolShipped)


    End Sub


    Protected Sub rdoUnshipped_CheckedChanged(sender As Object, e As EventArgs) Handles rdoUnshipped.CheckedChanged

        If rdoUnshipped.Checked Then

            'Set Shipped
            Dim bolShipped As Boolean = False

            'Check Radio Button
            CheckRadioButton(bolShipped)
        End If

    End Sub

    Private Sub SetDeliveryDateColumn(ByVal bolShipped As Boolean)

        'Hide/Show Coloumn
        dgvCustomerSample.Columns(13).Visible = bolShipped

    End Sub


    Private Sub SetMarkAsShipped(ByVal bolShipped As Boolean)

        'Hide/Show Button
        btnMarkAsShipped.Visible = bolShipped

        'Make Button Visible False
        btnMarkAsShipped.Enabled = False
        btnMarkAsShipped.CssClass = "form-button-disabled"

    End Sub

    Public Sub chkShipped_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        CheckActiveShipment()
    End Sub

    Private Sub CheckActiveShipment()

        'Set Button To Be Disabled
        btnMarkAsShipped.Enabled = False
        btnMarkAsShipped.CssClass = "form-button-disabled"

        Dim bolValid As Boolean = False

        'Iterate Rows
        For Each Row In dgvCustomerSample.Rows

            'Get Textbox Values
            Dim strConNote As String = Row.FindControl("txtConNote").Text
            Dim strCourier As String = Row.FindControl("txtCourier").Text

            If strConNote = String.Empty And strCourier = String.Empty Then
                Continue For
            ElseIf strConNote <> String.Empty And strCourier <> String.Empty Then
                lblStatus.Text = ""
                bolValid = True
            End If

            If strConNote = String.Empty Then
                lblStatus.Text = "Consignment Note cannot be empty!"
                bolValid = False
                Exit For
            End If

            If strCourier = String.Empty Then
                lblStatus.Text = "Courier cannot be empty!"
                bolValid = False
                Exit For
            End If

        Next

        If bolValid Then
            btnMarkAsShipped.Enabled = bolValid
            btnMarkAsShipped.Visible = bolValid
            btnMarkAsShipped.CssClass = "form-button"
        End If

    End Sub

    Protected Sub txtConNote_TextChanged(sender As Object, e As EventArgs)
        CheckActiveShipment()
    End Sub

    Protected Sub txtCourier_TextChanged(sender As Object, e As EventArgs)
        CheckActiveShipment()
    End Sub

    Private Sub dgvCustomerSample_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles dgvCustomerSample.RowCommand

        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = dgvCustomerSample.Rows(index)
        Dim strID As String = row.Cells(0).Text   ' ID
        lblNotes.Text = ""

        Dim dtCustomerSample As DataTable = ViewState("dtCustomerSample")

        Dim drowSelect() As DataRow = dtCustomerSample.Select("OrderID=" & strID)

        If drowSelect.Length > 0 Then
            lblNotes.Text = CStr(drowSelect(0)("JobNotes"))
        End If

        ModalPopupExtender.Show()

        drowSelect = Nothing

    End Sub

    Protected Sub btnCancelNotes_Click(sender As Object, e As EventArgs) Handles btnCancelNotes.Click
        ModalPopupExtender.Hide()
        lblNotes.Text = ""
    End Sub
End Class
