Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Linq

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://modernonline.com.au/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class AutoComplete
    Inherits System.Web.Services.WebService

    <WebMethod()>
    <ScriptMethodAttribute()>
    Public Function GetMatchingColourList(ByVal prefixText As String, ByVal count As Integer) As List(Of String)
        Dim service As New AppService
        Dim lReturnValues As New List(Of String)

        If HttpRuntime.Cache("Colours") Is Nothing Then
            ' Get the colours and cache them for 1 minute.
            Dim lAllColours As List(Of Colour) = service.getColours().FindAll(Function(x) Not x.Discontinued AndAlso x.ProductTypeID = 2)

            ' Append colour tags to the names.
            For Each c As Colour In lAllColours
                If c.CoatingTypeID = SharedEnums.CoatingType.StandardPowderCoat Then
                    c.Name = c.Name & SharedConstants.STR_STANDARD_COLOUR_TAG
                ElseIf c.CoatingTypeID = SharedEnums.CoatingType.PremiumPowderCoat Then
                    c.Name = c.Name & SharedConstants.STR_PREMIUM_COLOUR_TAG
                ElseIf c.CoatingTypeID = SharedEnums.CoatingType.PrestigePowderCoat Then
                    c.Name = c.Name & SharedConstants.STR_PRESTIGE_COLOUR_TAG
                End If
            Next c

            HttpRuntime.Cache.Insert("Colours", lAllColours, Nothing, Date.Now.AddMinutes(1), TimeSpan.Zero)
        End If

        Dim lCachedColours As List(Of Colour) = HttpRuntime.Cache("Colours")

        If lCachedColours IsNot Nothing Then

            Dim lMatch As List(Of Colour) = lCachedColours.FindAll(Function(x) x.Name.ToLower.Contains(prefixText.ToLower))

            If lMatch IsNot Nothing Then

                lMatch = lMatch.OrderBy(Function(x) x.CoatingTypeID).ThenBy(Function(x) x.Name).ToList()

                For Each c As Colour In lMatch
                    lReturnValues.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(c.Name, c.ID))
                Next c
            End If
        End If

        Return lReturnValues
    End Function

End Class