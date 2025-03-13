<%@ WebHandler Language="VB" Class="MessageCheck" %>

Imports System
Imports System.Web

Public Class MessageCheck : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim intNewMessages As Integer = 0
        
        Try
            
            Dim intSiteID As Integer = CInt(context.Request.Params("SiteID"))
            Dim intUserID As Integer = CInt(context.Request.Params("UserID"))
            
            Dim service As New AppService
            
            Dim dtData As DataTable = service.runSQLHive(intSiteID, "SELECT COUNT(tblConversationMessageRead.MessageID) AS Count FROM tblConversationMessageRead INNER JOIN tblConversationMessage ON tblConversationMessageRead.MessageID = tblConversationMessage.MessageID INNER JOIN tblConversation ON tblConversationMessage.ConversationID = tblConversation.ConversationID INNER JOIN tblConversationPeople ON tblConversation.ConversationID = tblConversationPeople.ConversationID AND tblConversationMessageRead.UserID = tblConversationPeople.UserID WHERE (tblConversationMessageRead.UserID = " & CStr(intUserID) & ") AND (tblConversationMessageRead.DateRead IS NULL) AND (tblConversationMessage.DateDeleted IS NULL) AND (tblConversation.DateDeleted IS NULL)")
            
            If dtData.Rows.Count > 0 Then
                intNewMessages = CInt(dtData.Rows(0).Item("Count"))
            End If
            
            dtData.Dispose()
            dtData = Nothing
            service = Nothing
            
        Catch ex As Exception
            EventLog.addEventLogEmail(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name & ": " & System.Reflection.MethodBase.GetCurrentMethod().Name & " - " & ex.Message)
        End Try
        
        context.Response.Write(CStr(intNewMessages))
    End Sub
    
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property
    
End Class