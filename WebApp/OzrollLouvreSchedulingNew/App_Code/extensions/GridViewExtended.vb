Imports System.Linq
Imports System.ComponentModel

Namespace Extensions
    Public Class GridViewExtended
        Inherits GridView

        Private _FooterRow As GridViewRow

        <DefaultValue(False), Category("Appearance"), Description("Include the footer when no rows exist in the table.")>
        Public Property ShowFooterWhenEmpty As Boolean

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)>
        Public Overrides ReadOnly Property FooterRow As GridViewRow
            Get

                If (_FooterRow Is Nothing) Then
                    EnsureChildControls()
                End If

                If (_FooterRow Is Nothing) Then
                    _FooterRow = MyBase.FooterRow
                End If

                Return _FooterRow
            End Get
        End Property

        Protected Overrides Function CreateChildControls(ByVal dataSource As System.Collections.IEnumerable, ByVal dataBinding As Boolean) As Integer
            Dim returnVal As Integer = MyBase.CreateChildControls(dataSource, dataBinding)

            If returnVal = 0 AndAlso ShowFooterWhenEmpty Then
                Dim table As Table = Controls.OfType(Of Table)().First()
                Dim dcf As DataControlField() = New DataControlField(Columns.Count - 1) {}

                Columns.CopyTo(dcf, 0)

                _FooterRow = CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal, dataBinding, Nothing, dcf, table.Rows, Nothing)

                If Not ShowFooter Then
                    _FooterRow.Visible = False
                End If
            End If

            Return returnVal
        End Function

        Private Overloads Function CreateRow(ByVal rowIndex As Integer,
                                             ByVal dataSourceIndex As Integer,
                                             ByVal rowType As DataControlRowType,
                                             ByVal rowState As DataControlRowState,
                                             ByVal dataBind As Boolean,
                                             ByVal dataItem As Object,
                                             ByVal fields As DataControlField(),
                                             ByVal rows As TableRowCollection,
                                             ByVal pagedDataSource As PagedDataSource) As GridViewRow

            Dim row As GridViewRow = MyBase.CreateRow(rowIndex, dataSourceIndex, rowType, rowState)
            Dim e As GridViewRowEventArgs = New GridViewRowEventArgs(row)

            If (rowType <> DataControlRowType.Pager) Then
                InitializeRow(row, fields)
            Else
                InitializePager(row, fields.Length, pagedDataSource)
            End If

            If dataBind Then
                row.DataItem = dataItem
            End If

            OnRowCreated(e)
            rows.Add(row)

            If dataBind Then
                row.DataBind()
                OnRowDataBound(e)
                row.DataItem = Nothing
            End If

            Return row
        End Function
    End Class
End Namespace
