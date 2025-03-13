'Imports System.Data.SqlClient
'Imports OzrollPSLVSchedulingModel.SharedEnums

'Public Class AppService

'    Private _Permissions As New PermissionsService
'    Private _Mail As New MailService
'    Private _Log As New LogService

'#Region "Services"

'    ReadOnly Property Permissions() As PermissionsService
'        Get
'            Return _Permissions
'        End Get
'    End Property

'    ReadOnly Property Mail() As MailService
'        Get
'            Return _Mail
'        End Get
'    End Property

'    ReadOnly Property Log() As LogService
'        Get
'            Return _Log
'        End Get
'    End Property

'#End Region

'    Function runSQLHive(ByVal intSiteID As Integer, ByVal strSQL As String) As DataTable

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.runSQLHive(intSiteID, strSQL)

'    End Function

'    Function runSQLOzrollSybiz(ByVal strSQL As String) As DataTable

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.runSQLOzrollSybiz(strSQL)

'    End Function

'    Function runSQLOSCDatabase(ByVal strSQL As String, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.runSQLOSCDatabase(strSQL, cnn, trans)

'    End Function

'    Function runSQLScheduling(ByVal strSQL As String, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.runSQLScheduling(strSQL, cnn, trans)

'    End Function

'    Function runSQLOZOTS(ByVal strSQL As String) As DataTable

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.runSQLOZOTS(strSQL)

'    End Function

'    Function runSQLOzrollTracking(ByVal strSQL As String) As DataTable

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.runSQLOzrollTracking(strSQL)

'    End Function

'    Function executeSQLScheduling(ByVal strSQL As String, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.executeSQLScheduling(strSQL, cnn, trans)

'    End Function

'    Function updateScheduledDate(ByVal intSiteID As Integer, ByVal intJobNumber As Integer, ByVal dteScheduledDate As Date) As Boolean

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.updateScheduledDate(intSiteID, intJobNumber, dteScheduledDate)

'    End Function

'    Function getPlantationOrderDetail(ByVal intSiteID As Integer, ByVal intJobNumber As Integer) As DataTable

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.getPlantationOrderDetail(intSiteID, intJobNumber)

'    End Function

'    Function getPlantationOrderExtras(ByVal intSiteID As Integer, ByVal intJobNumber As Integer) As DataTable

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.getPlantationOrderExtras(intSiteID, intJobNumber)

'    End Function

'#Region "Plantation Job Details"

'    Function getPlantationJobDetailsRecord(intPSDetailID As Integer) As PlantationJobDetails

'        Dim serviceDAO As New PlantationJobDetailsDAO
'        Return serviceDAO.getPlantationJobDetailsRecord(intPSDetailID)

'    End Function

'    Function getPlantationJobDetailsRecordsByPlantationScheduleID(intPlantationScheduleID As Integer) As DataTable

'        Dim serviceDAO As New PlantationJobDetailsDAO
'        Return serviceDAO.getPlantationJobDetailsRecordsByPlantationScheduleID(intPlantationScheduleID)

'    End Function

'    Function addPlantationJobDetailsRecord(cPlantationJobDetails As PlantationJobDetails, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim serviceDAO As New PlantationJobDetailsDAO
'        Return serviceDAO.addPlantationJobDetailsRecord(cPlantationJobDetails, cnn, trans)

'    End Function

'    Function updatePlantationJobDetailsRecord(cPlantationJobDetails As PlantationJobDetails, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim serviceDAO As New PlantationJobDetailsDAO
'        Return serviceDAO.updatePlantationJobDetailsRecord(cPlantationJobDetails, cnn, trans)

'    End Function

'    Function GetPlantationJobDetailsByPlantationScheduleID(intPlantationScheduleID As Integer) As PlantationJobDetailsCollection

'        Dim serviceDAO As New PlantationJobDetailsDAO
'        Return serviceDAO.GetPlantationJobDetailsByPlantationScheduleID(intPlantationScheduleID)

'    End Function

'#End Region


'#Region "Production Schedule Section"

'    Function createOrderStatusTBL(intProductTypeID As ProductType) As DataTable

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.createOrderStatusTBL(intProductTypeID)

'    End Function

'    Function getSchedulingMonths(ByRef dtAllMonths As DataTable, Optional ByVal intYearID As Integer = Nothing) As DataTable

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.getSchedulingMonths(dtAllMonths, intYearID)

'    End Function

'    Function getProductionScheduleByID(ByVal intProdScheduleId As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.getProductionScheduleByID(intProdScheduleId)

'    End Function

'    Function getProdScheduleClsByID(ByVal intProdScheduleId As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As ProductionSchedule

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.getProdScheduleClsByID(intProdScheduleId)

'    End Function

'    Function addProductionScheduleRecord(ByVal clsProdSchedule As ProductionSchedule, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Integer

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.addProductionScheduleRecord(clsProdSchedule, cnn, trans)

'    End Function

'    Function updateProductionScheduleByID(ByVal clsProdSchedule As ProductionSchedule, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.updateProductionScheduleByID(clsProdSchedule, cnn, trans)


'    End Function

'    Function addProdScheduleHistoryRcd(ByVal intChangedID As Integer, ByVal clsProdSchedule As ProductionSchedule, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.addProdScheduleHistoryRcd(intChangedID, clsProdSchedule, cnn, trans)

'    End Function

'    Function getProductScheduleHistoryNewChgID() As Integer

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.getProductScheduleHistoryNewChgID

'    End Function

'    Function ConvertRcdToProdScheduleCls(ByVal dwProdScheduleTBL As DataRow) As ProductionSchedule

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.ConvertRcdToProdScheduleCls(dwProdScheduleTBL)

'    End Function

'    Function addProdScheduleNoteRcd(ByVal clsProdScheduleNote As ProdScheduleNote, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.addProdScheduleNoteRcd(clsProdScheduleNote, cnn, trans)

'    End Function

'    Function GetProdScheduleNotesByProductionScheduleID(cProdScheduleID As Integer) As List(Of ProdScheduleNote)

'        Dim ProdScheduleDAO As New ProductionScheduleDAO
'        Return ProdScheduleDAO.getProdScheduleNotesByProductionScheduleID(cProdScheduleID)

'    End Function

'    Function addTempProdScheduleNotes(cNote As ProdScheduleNote, strGUID As String, bolNewNote As Boolean, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim serviceDAO As New TempProdScheduleNotesDAO
'        Return serviceDAO.addTempProdScheduleNotes(cNote, strGUID, bolNewNote, cnn, trans)

'    End Function

'#End Region

'#Region "Plantation GRA Photos"

'    Function addPlatationGRAPhotosRecord(ByVal cPlantationGRAPhotos As PlantationGRAPhotos, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Integer

'        Dim ServiceDAO As New PlantationGRAPhotosDAO
'        Return ServiceDAO.addPlatationGRAPhotosRecord(cPlantationGRAPhotos, cnn, trans)

'    End Function

'    Function updatePlatationGRAPhotosRecord(ByVal cPlantationGRAPhotos As PlantationGRAPhotos, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ServiceDAO As New PlantationGRAPhotosDAO
'        Return ServiceDAO.updatePlatationGRAPhotosRecord(cPlantationGRAPhotos, cnn, trans)

'    End Function

'    Function getPlantationGRAPhotosDetailsByID(ByVal intPlantGRAID As Integer) As DataTable

'        Dim ServiceDAO As New PlantationGRAPhotosDAO
'        Return ServiceDAO.getPlantationGRAPhotosDetailsByID(intPlantGRAID)

'    End Function

'    Function deletePlantationGRAPhotoByID(ByVal intID As Integer) As Boolean

'        Dim ServiceDAO As New PlantationGRAPhotosDAO
'        Return ServiceDAO.deletePlantationGRAPhotoByID(intID)

'    End Function


'#End Region


'#Region "Job Stages"

'    Function addJobStages(cJobStages As JobStages, cnn As SqlConnection, ByRef trans As SqlTransaction) As Integer

'        Dim ServiceDAO As New JobStagesDAO
'        Return ServiceDAO.addJobStages(cJobStages, cnn, trans)

'    End Function

'    Function updateJobStages(cJobStages As JobStages, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ServiceDAO As New JobStagesDAO
'        Return ServiceDAO.updateJobStages(cJobStages, cnn, trans)

'    End Function

'    Function addJobStagesHistoryRecord(cJobStages As JobStages, intUserID As Integer, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ServiceDAO As New JobStagesDAO
'        Return ServiceDAO.addJobStagesHistoryRecord(cJobStages, intUserID, cnn, trans)

'    End Function

'    Function GetJobStagesByProductionScheduleID(intProductionScheduleID As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As List(Of JobStages)
'        Dim ServiceDAO As New JobStagesDAO

'        Return ServiceDAO.GetJobStagesByProductionScheduleID(intProductionScheduleID, cnn, trans)
'    End Function

'#End Region

'#Region "Plantation GRA"

'    Function addPlantationGRARecord(clsPlantationGRA As PlantationGRAObj, cnn As SqlConnection, trans As SqlTransaction) As Integer

'        Dim ServiceDAO As New PlantationGRADAO
'        Return ServiceDAO.addPlantationGRARecord(clsPlantationGRA, cnn, trans)

'    End Function

'    Function updatePlantationGRAByID(ByVal clsPlantationGRA As PlantationGRAObj, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ServiceDAO As New PlantationGRADAO
'        Return ServiceDAO.updatePlantationGRAByID(clsPlantationGRA, cnn, trans)

'    End Function

'    Function getPlantationGRAClassByID(ByVal intPlantationGRADID As Integer) As PlantationGRAObj

'        Dim ServiceDAO As New PlantationGRADAO
'        Return ServiceDAO.getPlantationGRAClassByID(intPlantationGRADID)

'    End Function

'#End Region

'    Function getDailyProductionSchedule(dteScheduleDate As Date, intProductTypeID As ProductType) As DataTable

'        Dim ServiceDAO As New ProductionScheduleDAO
'        Return ServiceDAO.getDailyProductionSchedule(dteScheduleDate, intProductTypeID)

'    End Function

'    Function getToBeDespatchedList(intProductTypeID As ProductType) As DataTable

'        Dim ServiceDAO As New ProductionScheduleDAO
'        Return ServiceDAO.getToBeDespatchedList(intProductTypeID)

'    End Function

'    Function getToBeCollectedFromFactory(intProductTypeID As ProductType) As DataTable

'        Dim ServiceDAO As New ProductionScheduleDAO
'        Return ServiceDAO.getToBeCollectedFromFactory(intProductTypeID)

'    End Function

'    Function getToBeInvoiced(intProductTypeID As ProductType) As DataTable

'        Dim ServiceDAO As New ProductionScheduleDAO
'        Return ServiceDAO.getToBeInvoiced(intProductTypeID)

'    End Function

'    Function getUsersRecords() As DataTable

'        Dim ServiceDAO As New UsersDAO
'        Return ServiceDAO.getUsersRecords

'    End Function

'    Function getUsers() As List(Of User)

'        Dim ServiceDAO As New UsersDAO
'        Return ServiceDAO.GetUsers

'    End Function

'    Function getUserByID(intID As Integer) As User

'        Dim ServiceDAO As New UsersDAO
'        Return ServiceDAO.GetUserByID(intID)

'    End Function

'    Public Function addWebsitePageAccess(ByVal strWebsite As String, ByVal intUserID As Integer, ByVal strUserName As String, ByVal strPageName As String, ByVal strReference As String, ByVal dteHitDate As Date) As Boolean

'        Dim serviceDAO As New DataDAO
'        Return serviceDAO.addWebsitePageAccess(strWebsite, intUserID, strUserName, strPageName, strReference, dteHitDate)

'    End Function

'    Public Sub GetSybizDataCustomerRecordsByCode(intSybizCustomerCode As String)
'        Dim serviceDAO As New CustomerDAO
'        serviceDAO.GetSybizCustomerDataRecordsByCode(intSybizCustomerCode)
'    End Sub

'    Public Function GetSybizCustomerDataByCode(intSybizCustomerCode As String) As SybizCustomer
'        Dim serviceDAO As New CustomerDAO
'        Return serviceDAO.GetSybizCustomerDataByCode(intSybizCustomerCode)
'    End Function

'    Function getCustomerByCustomerID(ByVal intCustomerID As Integer) As DataTable

'        Dim serviceDAO As New CustomerDAO
'        Return serviceDAO.getCustomerRecordByCustomerID(intCustomerID)

'    End Function

'    Function GetCustomerByID(intID As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Customer
'        Dim ServiceDAO As New CustomerDAO
'        Return ServiceDAO.GetCustomerByCustomerID(intID, cnn, trans)
'    End Function

'    Function GetCustomers(Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As List(Of Customer)

'        Dim serviceDAO As New CustomerDAO
'        Return serviceDAO.GetCustomers(cnn, trans)

'    End Function

'    Function addCustomerRecord(
'            cCustomer As Customer,
'            Optional cnn As SqlConnection = Nothing,
'            Optional ByRef trans As SqlTransaction = Nothing
'        ) As Integer

'        Dim serviceDAO As New CustomerDAO
'        Return serviceDAO.addCustomerRecord(cCustomer, cnn, trans)

'    End Function

'    Function updateCustomerRecord(cCustomer As Customer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New CustomerDAO
'        Return serviceDAO.updateCustomerRecord(cCustomer, cnn, trans)

'    End Function

'    Function getAddressesByCustomerID(intCustomerID As Integer) As DataTable

'        Dim serviceDAO As New AddressDAO
'        Return serviceDAO.getAddressesByCustomerID(intCustomerID)

'    End Function

'    ''' <summary>
'    ''' Converts the given address row into a single address string.
'    ''' </summary>
'    ''' <param name="drAddressRow">The <see cref="Datarow"/> of the address to convert.</param>
'    ''' <param name="strFieldSeparator">The string to place in between address fields.</param>
'    ''' <returns>A formatted address string.> </returns>
'    Function convertAddressDataRowToString(drAddressRow As DataRow, strFieldSeparator As String) As String
'        Dim serviceDAO As New AddressDAO
'        Return serviceDAO.convertAddressDataRowToString(drAddressRow, strFieldSeparator)
'    End Function

'    Function ConvertAddressRecordToObject(drAddressRow As DataRow) As Address
'        Dim serviceDAO As New AddressDAO
'        Return serviceDAO.convertAddressRecordToObject(drAddressRow)
'    End Function

'    Function getAddressRecordsByCustomerIDAndAddressType(intCustomerID As Integer, enumAddressType As SharedEnums.AddressType) As DataTable

'        Dim serviceDAO As New AddressDAO
'        Return serviceDAO.getAddressRecordsByCustomerIDAndAddressType(intCustomerID, enumAddressType)

'    End Function

'    Function getAddressesByCustomerIDAndAddressType(intCustomerID As Integer, enumAddressType As SharedEnums.AddressType) As List(Of Address)

'        Dim serviceDAO As New AddressDAO
'        Return serviceDAO.getAddressesByCustomerIDAndAddressType(intCustomerID, enumAddressType)

'    End Function

'    Function getAddressByID(intID As Integer) As Address

'        Dim serviceDAO As New AddressDAO
'        Return serviceDAO.getAddressByID(intID)

'    End Function

'    Function addAddressRecord(aAddress As Address, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New AddressDAO
'        Return serviceDAO.addAddressRecord(aAddress, cnn, trans)

'    End Function

'    Function updateAddressRecord(aAddress As Address, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New AddressDAO
'        Return serviceDAO.updateAddressRecord(aAddress, cnn, trans)

'    End Function

'    Function getDeliveryInstructionsByAddressID(intAddressID As Integer) As DataTable

'        Dim serviceDAO As New DeliveryInstructionsDAO
'        Return serviceDAO.getDeliveryInstructionsByAddressID(intAddressID)

'    End Function

'    Function getDeliveryInstructionsListByAddressID(intAddressID As Integer) As List(Of DeliveryInstruction)

'        Dim serviceDAO As New DeliveryInstructionsDAO
'        Return serviceDAO.getDeliveryInstructionsListByAddressID(intAddressID)

'    End Function

'    Function addDeliveryInstructionRecord(aDeliveryInstruction As DeliveryInstruction, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New DeliveryInstructionsDAO
'        Return serviceDAO.addDeliveryInstructionRecord(aDeliveryInstruction, cnn, trans)

'    End Function

'    Function getNotesByCustomerID(intCustomerID As Integer) As DataTable

'        Dim serviceDAO As New NotesDAO
'        Return serviceDAO.getNotesByCustomerID(intCustomerID)

'    End Function

'    Function addNoteRecord(nNote As Note, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New NotesDAO
'        Return serviceDAO.addNoteRecord(nNote, cnn, trans)

'    End Function

'    Function addAdditionalRequirementsRecord(cRequirement As AdditionalRequirements, cnn As SqlConnection, ByRef trans As SqlTransaction) As Integer

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.addAdditionalRequirementsRecord(cRequirement, cnn, trans)

'    End Function

'    Function updateAdditionalRequirementsRecord(cRequirement As AdditionalRequirements, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.updateAdditionalRequirementsRecord(cRequirement, cnn, trans)

'    End Function

'    Function getAdditionalRequirementsRecord(intAdditonalRequirementID As Integer) As DataTable

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.getAdditionalRequirementsRecord(intAdditonalRequirementID)

'    End Function

'    Function getAdditionalRequirementsByProductionScheduleID(intProductionScheduleID As Integer) As DataTable

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.getAdditionalRequirementsByProductionScheduleID(intProductionScheduleID)

'    End Function

'    Function GetAdditionalRequirementsListByProductionScheduleID(intProductionScheduleID As Integer) As List(Of AdditionalRequirements)

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.GetAdditionalRequirementsListByProductionSheduleID(intProductionScheduleID)

'    End Function

'    Function DeleteAdditonalRequirementsByProductionScheduleID(intProductionScheduleID As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.DeleteAdditonalRequirementsByProductionScheduleID(intProductionScheduleID, cnn, trans)

'    End Function

'    Function GetAdditionalRequirementTypeByID(intID As Integer) As AdditionalRequirementType

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.GetAdditionalRequirementTypeByID(intID)

'    End Function

'    Function GetAdditionalRequirementTypes() As List(Of AdditionalRequirementType)

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.GetAdditionalRequirementTypes()

'    End Function

'    Function addTempAddRequirementsRecord(cRequirement As AdditionalRequirements, cnn As SqlConnection, ByRef trans As SqlTransaction) As Integer

'        Dim serviceDAO As New TempAddRequirementsDAO
'        Return serviceDAO.addTempAddRequirementsRecord(cRequirement, cnn, trans)

'    End Function

'    Function updateTempAddRequirementsRecord(cRequirement As AdditionalRequirements, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim serviceDAO As New TempAddRequirementsDAO
'        Return serviceDAO.updateTempAddRequirementsRecord(cRequirement, cnn, trans)

'    End Function

'    Function getTempAddRequirementsRecord(intAdditonalRequirementID As Integer) As DataTable

'        Dim serviceDAO As New TempAddRequirementsDAO
'        Return serviceDAO.getTempAddRequirementsRecord(intAdditonalRequirementID)

'    End Function

'    Function GetPowderCoaterByID(intID As Integer) As PowderCoater

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.GetPowderCoaterByID(intID)

'    End Function

'    Function GetPowderCoaters() As List(Of PowderCoater)

'        Dim serviceDAO As New AdditionalRequirementsDAO
'        Return serviceDAO.GetPowderCoaters()

'    End Function

'#Region "Temp Plantation Job Details"

'    Function getTempPlantationJobDetailsRecord(intPSDetailID As Integer) As PlantationJobDetails

'        Dim ServiceDAO As New TempPlantationJobDetailsDAO
'        Return ServiceDAO.getTempPlantationJobDetailsRecord(intPSDetailID)

'    End Function

'    Function getTempPlantationJobDetailsRecordDatatable(intPSDetailID As Integer) As DataTable

'        Dim ServiceDAO As New TempPlantationJobDetailsDAO
'        Return ServiceDAO.getTempPlantationJobDetailsRecordDatatable(intPSDetailID)

'    End Function

'    Function getTempPlantationJobDetailsRecordsByTempGUID(strTempGUID As String, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim ServiceDAO As New TempPlantationJobDetailsDAO
'        Return ServiceDAO.getTempPlantationJobDetailsRecordsByTempGUID(strTempGUID, cnn, trans)

'    End Function

'    Function addTempPlantationJobDetailsRecord(cPlantationJobDetails As PlantationJobDetails, cnn As SqlConnection, ByRef trans As SqlTransaction) As Integer

'        Dim ServiceDAO As New TempPlantationJobDetailsDAO
'        Return ServiceDAO.addTempPlantationJobDetailsRecord(cPlantationJobDetails, cnn, trans)

'    End Function

'    Function updateTempPlantationJobDetailsRecord(cPlantationJobDetails As PlantationJobDetails, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ServiceDAO As New TempPlantationJobDetailsDAO
'        Return ServiceDAO.updateTempPlantationJobDetailsRecord(cPlantationJobDetails, cnn, trans)

'    End Function

'#End Region

'#Region "PS Lookups"

'    Public Function getPSMaterial(ByVal intManufactureLocation As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSMaterial(intManufactureLocation)

'    End Function


'    Public Function getPSRoomLocation() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSRoomLocation()

'    End Function

'    Public Function getPSInstallationMethod() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSInstallationMethod()

'    End Function

'    Public Function getPSColour(ByVal intMaterialID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSColour(intMaterialID)

'    End Function

'    Public Function getPSHingeColour(ByVal intMaterialID As Integer, ByVal intConcealed As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSHingeColour(intMaterialID, intConcealed)

'    End Function

'    Public Function getPSModuleColour() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSModuleColour()

'    End Function

'    Public Function getPSHingeType() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSHingeType()

'    End Function

'    Public Function getPSLouvre(ByVal intMaterialID As Integer, ByVal intMotorized As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSLouvre(intMaterialID, intMotorized)

'    End Function

'    Public Function getPSLouvreTypeWB() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSLouvreTypeWB()

'    End Function

'    Public Function getPSLouvreFixed() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSLouvreFixed()

'    End Function

'    Public Function getPSTiltRODPosition(ByVal intMotorized As Integer, ByVal intMaterialID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSTiltRODPosition(intMotorized, intMaterialID)

'    End Function

'    Public Function getPSLayout(ByVal intInstallationMethodID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSLayout(intInstallationMethodID)

'    End Function

'    Public Function getPSStile(ByVal intMotorized As Integer, ByVal intMaterialID As Integer, ByVal intLayoutLen As Integer, ByVal intConceledHinge As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSStile(intMotorized, intMaterialID, intLayoutLen, intConceledHinge)

'    End Function

'    Public Function getPSFrameType(ByVal intInstallationMethodID As Integer, ByVal intMaterialID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSFrameType(intInstallationMethodID, intMaterialID)

'    End Function

'    Public Function getPSSillPlate(ByVal intMaterialID As Integer, ByVal intFrameID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSSillPlate(intMaterialID, intFrameID)

'    End Function

'    Public Function getPSInOut(ByVal intMaterialID As Integer, ByVal intInstallationMethodID As Integer, ByVal intHingeTypeID As Integer, ByVal intFrameTypeID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSInOut(intMaterialID, intInstallationMethodID, intHingeTypeID, intFrameTypeID)

'    End Function

'    Public Function getPSHangingStrip(ByVal intInstallationMethodID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSHangingStrip(intInstallationMethodID)

'    End Function

'    Public Function getPSLightBlock(ByVal intInstallationMethodID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSLightBlock(intInstallationMethodID)

'    End Function

'    Public Function getPSTPos() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSTPos()

'    End Function

'    Public Function getPSSplitTiltRodLocation(ByVal strSplitLocation As String) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSSplitTiltRodLocation(strSplitLocation)

'    End Function

'    Public Function getPSHandCarveStyle() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSHandCarveStyle()

'    End Function

'    Public Function getPSHandCarveLocation() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSHandCarveLocation()

'    End Function

'    Public Function getPSRingPullLocation() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSRingPullLocation()

'    End Function


'#End Region

'#Region "PS Lookups 2"

'    Function getPSAngleBay() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSAngleBay()

'    End Function

'    Function getPSBladeSize() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSBladeSize()

'    End Function

'    Function getPSColour() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSColour()

'    End Function

'    Function getPSControlType() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSControlType()

'    End Function

'    Function getPSFrameType(ByVal intMountID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSFrameType(intMountID)

'    End Function

'    Function getPSHangStrip() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSHangStrip()

'    End Function

'    Function getPSHingeColour(ByVal intInstallatinAreaID As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSHingeColour(intInstallatinAreaID)

'    End Function

'    Function getPSInstallationArea() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSInstallationArea()

'    End Function

'    Function getPSLayoutByMountAndPanelQty(ByVal intMountID As Integer, ByVal intPanelQty As Integer) As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSLayoutByMountAndPanelQty(intMountID, intPanelQty)

'    End Function

'    Function getPSLightBlock() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSLightBlock()

'    End Function

'    Function getPSMountConfig() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSMountConfig()

'    End Function

'    Function getPSMountMethod() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSMountMethod()

'    End Function

'    Function getPSMountStyle() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSMountStyle()

'    End Function

'    Function getPSPanelQty() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSPanelQty()

'    End Function

'    Function getPSSides() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSSides()

'    End Function

'    Function getPSSlidingGuide() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSSlidingGuide()

'    End Function

'    Function getPSSlidingOpenClose() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSSlidingOpenClose()

'    End Function

'    Function getPSSplitBlade() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSSplitBlade()

'    End Function

'    Function getPSTPostQty() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSTPostQty()

'    End Function

'    Function getPSTrack() As DataTable

'        Dim serviceDAO As New PSLookupsDAO
'        Return serviceDAO.getPSTrack()

'    End Function
'#End Region

'#Region "OSC Louvre Functions"

'    Function addJobRegisterRecord(cJobRegister As OSCJobRegister, cnn As SqlConnection, ByRef trans As SqlTransaction) As Integer

'        Dim serviceDAO As New OSCJobRegisterDAO
'        Return serviceDAO.addJobRegisterRecord(cJobRegister, cnn, trans)

'    End Function

'    Function updateJobRegisterRecord(cJobRegister As OSCJobRegister, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim serviceDAO As New OSCJobRegisterDAO
'        Return serviceDAO.updateJobRegisterRecord(cJobRegister, cnn, trans)

'    End Function

'    Function getJobRegisterRecord(intJobRegisterID As Integer) As DataTable

'        Dim serviceDAO As New OSCJobRegisterDAO
'        Return serviceDAO.getJobRegisterRecord(intJobRegisterID)

'    End Function

'    Function getJobRegisterByJobRegisterID(intJobRegisterID As Integer) As OSCJobRegister

'        Dim serviceDAO As New OSCJobRegisterDAO
'        Return serviceDAO.getJobRegisterByJobRegisterID(intJobRegisterID)

'    End Function

'    Function getNextWholesaleContractNumber(cnn As SqlConnection, ByRef trans As SqlTransaction) As DataTable

'        Dim serviceDAO As New OSCJobRegisterDAO
'        Return serviceDAO.getNextWholesaleContractNumber(cnn, trans)

'    End Function

'    Function getAllJobRegisteredList() As DataTable

'        Dim serviceDAO As New OSCJobRegisterDAO
'        Return serviceDAO.getAllJobRegisteredList

'    End Function


'#End Region

'#Region "Products List Details"

'    Function ProdScheduleSQLget(ByVal strSQL As String) As DataTable

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.ProdScheduleSQLget(strSQL)

'    End Function

'    Function getAllPlantationSpecs() As DataTable

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.getAllPlantationSpecs()

'    End Function

'    Function getPlantSpecsByProdSchID(ByVal intProdScheduleID As Integer) As DataTable

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.getPlantSpecsByProdSchID(intProdScheduleID)

'    End Function

'    Function getAllLouvreSpecs() As DataTable

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.getAllLouvreSpecs()

'    End Function

'    Function getLouvreSpecsByProdSchID(ByVal intProdScheduleID As Integer) As DataTable

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.getLouvreSpecsByProdSchID(intProdScheduleID)

'    End Function

'    Function getLouvreSpecsByProductionScheduleID(ByVal intProductionScheduleID As Integer) As LouvreSpecs

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.getLouvreSpecsByProductionScheduleID(intProductionScheduleID)

'    End Function

'    Function AddPlantationSpecs(ByVal clsPlantationSpecs As PlantationSpecs, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.AddPlantationSpecs(clsPlantationSpecs, cnn, trans)

'    End Function

'    Function updatePlantationSpecs(ByVal clsPlantationSpecs As PlantationSpecs, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.updatePlantationSpecs(clsPlantationSpecs, cnn, trans)

'    End Function

'    Function deletePlantationSpecs(ByVal intProdScheduleID As Integer, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.deletePlantationSpecs(intProdScheduleID, cnn, trans)

'    End Function

'    Function AddLouvreSpecs(ByVal clsLouvreSpecs As LouvreSpecs, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.AddLouvreSpecs(clsLouvreSpecs, cnn, trans)

'    End Function

'    Function updateLouvreSpecs(ByVal clsLouvreSpecs As LouvreSpecs, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.updateLouvreSpecs(clsLouvreSpecs, cnn, trans)

'    End Function

'    Function deleteLouvreSpecs(ByVal intProdScheduleID As Integer, ByVal cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.deleteLouvreSpecs(intProdScheduleID, cnn, trans)

'    End Function

'    Function convertRowPlantSpecToCLS(ByVal drPlantSpec As DataRow) As PlantationSpecs

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.convertRowPlantSpecToCLS(drPlantSpec)

'    End Function

'    Function getLouvreSpecsClassFromDatarow(ByVal dr As DataRow) As LouvreSpecs

'        Dim ProdSTDAO As New ProductsListDAO()
'        Return ProdSTDAO.getLouvreSpecsClassFromDatarow(dr)

'    End Function

'#End Region


'#Region "Louvre Details DAO"

'    Public Function getLouvreColours() As DataTable

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.getLouvreColours

'    End Function

'    Public Function getLouvreLocations() As DataTable

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.getLouvreLocations

'    End Function

'    Public Function getLouvreDetailsByProductionScheduleID(intProductionScheduleID As Integer, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.getLouvreDetailsByProductionScheduleID(intProductionScheduleID, cnn, trans)

'    End Function

'    Public Function getLouvreDetailsCollectionByProductionScheduleID(intProductionScheduleID As Integer, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As LouvreDetailsCollection

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.getLouvreDetailsCollectionByProductionScheduleID(intProductionScheduleID, cnn, trans)

'    End Function

'    Public Function addLouvreDetails(ByVal cLouvreDetails As LouvreDetails, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.addLouvreDetails(cLouvreDetails, cnn, trans)

'    End Function

'    Public Function updateLouvreDetails(ByVal cLouvreDetails As LouvreDetails, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.updateLouvreDetails(cLouvreDetails, cnn, trans)

'    End Function

'    Public Function DeleteLouvreDetailByID(intLouvreDetailID As Integer, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.DeleteLouvreDetailByID(intLouvreDetailID, cnn, trans)

'    End Function

'    Public Function getLouvreDetailsRecord(ByVal intLouvreDetailID As Integer, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As LouvreDetails

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.getLouvreDetailsRecord(intLouvreDetailID, cnn, trans)

'    End Function

'    Public Function addTempLouvreDetails(ByVal cLouvreDetails As LouvreDetails, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New TempLouvreDetailsDAO
'        Return serviceDAO.addTempLouvreDetails(cLouvreDetails, cnn, trans)

'    End Function

'    Public Function updateTempLouvreDetails(ByVal cLouvreDetails As LouvreDetails, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New TempLouvreDetailsDAO
'        Return serviceDAO.updateTempLouvreDetails(cLouvreDetails, cnn, trans)

'    End Function

'    Public Function getTempLouvreDetailsRecord(ByVal intLouvreDetailID As Integer, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As LouvreDetails

'        Dim serviceDAO As New TempLouvreDetailsDAO
'        Return serviceDAO.getTempLouvreDetailsRecord(intLouvreDetailID, cnn, trans)

'    End Function

'    Public Function getTempLouvreDetailsByLouvreDetailID(ByVal intLouvreDetailID As Integer, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim serviceDAO As New TempLouvreDetailsDAO
'        Return serviceDAO.getTempLouvreDetailsByLouvreDetailID(intLouvreDetailID, cnn, trans)

'    End Function

'    Public Function setLouvreDetailsObjectFromDataRow(ByVal drow As DataRow) As LouvreDetails

'        Dim serviceDAO As New LouvreDetailsDAO
'        Return serviceDAO.setLouvreDetailsObjectFromDataRow(drow)

'    End Function

'    Function getTempLouvreJobDetailsRecordsByTempGUID(strTempGUID As String, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim serviceDAO As New TempLouvreDetailsDAO
'        Return serviceDAO.getTempLouvreJobDetailsRecordsByTempGUID(strTempGUID, cnn, trans)

'    End Function

'#End Region

'#Region "PS Pricing DAO"

'    Function getPSPrice(ByVal intMaterialTypeID As Integer, ByVal intInstallationAreaID As Integer, ByVal intMountMethodID As Integer, ByVal intManufactureLocationID As Integer, ByVal intControlTypeID As Integer, ByVal dteEffectiveDate As Date, ByVal intWidth As Integer, ByVal intBladeSizeID As Integer) As DataTable

'        Dim serviceDAO As New PSPricingDAO
'        Return serviceDAO.getPSPrice(intMaterialTypeID, intInstallationAreaID, intMountMethodID, intManufactureLocationID, intControlTypeID, dteEffectiveDate, intWidth, intBladeSizeID)
'        serviceDAO = Nothing

'    End Function

'    Function getPSPriceMount(ByVal intInstallationAreaID As Integer, ByVal intManufactureLocationID As Integer, ByVal intMountMethodID As Integer, ByVal intTrackID As Integer, ByVal dteEffectiveDate As Date, ByVal intSideboards As Integer, ByVal intBottomboards As Integer, ByVal intMaterialTypeID As Integer) As DataTable

'        Dim serviceDAO As New PSPricingDAO
'        Return serviceDAO.getPSPriceMount(intInstallationAreaID, intManufactureLocationID, intMountMethodID, intTrackID, dteEffectiveDate, intSideboards, intBottomboards, intMaterialTypeID)
'        serviceDAO = Nothing

'    End Function

'    Function getPSPriceFrame(ByVal intManufactureLocationID As Integer, ByVal intFrameTypeID As Integer, ByVal dteEffectiveDate As Date, ByVal intBayPost As Integer, ByVal intCornerPost As Integer, ByVal intMaterialTypeID As Integer) As DataTable

'        Dim serviceDAO As New PSPricingDAO
'        Return serviceDAO.getPSPriceFrame(intManufactureLocationID, intFrameTypeID, dteEffectiveDate, intBayPost, intCornerPost, intMaterialTypeID)
'        serviceDAO = Nothing

'    End Function



'#End Region

'    Function getUploadedFiles(ByVal RefID As String, RefType As String) As DataTable

'        Dim serviceDAO As New UploadFilesDAO
'        Return serviceDAO.getUploadedFiles(RefID, RefType)

'    End Function

'    Function addUploadedFiles(ByVal DataArray As String()) As Boolean

'        Dim serviceDAO As New UploadFilesDAO
'        Return serviceDAO.addUploadedFiles(DataArray)

'    End Function

'    Function deleteUploadedFiles(ByVal RefID As String, UserID As String, Fname As String) As Boolean

'        Dim serviceDAO As New UploadFilesDAO
'        Return serviceDAO.deleteUploadedFiles(RefID, UserID, Fname)

'    End Function

'    Function addLouvreSpecDesign(louvreDesign As LouvreSpecDesign, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New LouvreSpecDesignDAO
'        Return serviceDAO.addLouvreSpecDesign(louvreDesign, cnn, trans)
'        serviceDAO = Nothing

'    End Function

'    Function updateLouvreSpecDesign(louvreDesign As LouvreSpecDesign, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean

'        Dim serviceDAO As New LouvreSpecDesignDAO
'        Return serviceDAO.updateLouvreSpecDesign(louvreDesign, cnn, trans)
'        serviceDAO = Nothing

'    End Function

'    Function GetLouvreSpecControllerRecord(ByVal id As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim serviceDAO As New LouvreSpecsControllerDAO
'        Return serviceDAO.GetLouvreSpecControllerRecord(id, cnn, trans)
'        serviceDAO = Nothing

'    End Function

'    Function getLouvreSpecDesignRecordDetails(dt As DataTable) As LouvreSpecDesign

'        Dim serviceDAO As New LouvreSpecDesignDAO
'        Return serviceDAO.getLouvreSpecDesignRecordDetails(dt)
'        serviceDAO = Nothing

'    End Function

'    Function GetLouvreSpecDesignRecordByLouvreDetailsID(intLouvreDetailsID As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As DataTable

'        Dim serviceDAO As New LouvreSpecDesignDAO
'        Return serviceDAO.GetLouvreSpecDesignRecordByLouvreDetailsID(intLouvreDetailsID, cnn, trans)
'        serviceDAO = Nothing

'    End Function

'    Function GetLouvreSpecDesignByLouvreDetailsID(intLouvreDetailsID As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As LouvreSpecDesign

'        Dim serviceDAO As New LouvreSpecDesignDAO
'        Return serviceDAO.GetLouvreSpecDesignByLouvreDetailsID(intLouvreDetailsID, cnn, trans)
'        serviceDAO = Nothing

'    End Function

'    Public Function updateProductionLeadDays(prodLead As ProductionLeadDays, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer
'        Dim serviceDAO As New ProductionLeadDaysDAO
'        Return serviceDAO.updateProductionLeadDays(prodLead)
'    End Function

'    Public Function getJobStockUsageByID(ByVal ScheduleID As Integer, ByVal StockArticleTypeID As Integer) As DataTable
'        Dim serviceDAO As New StockUsageDAO
'        Return serviceDAO.getJobStockUsageByID(ScheduleID, StockArticleTypeID)
'    End Function

'    Function GetJobStockUsageRecordByReqID(ByVal AdditionalRequirementsID As Integer) As DataTable
'        Dim serviceDAO As New StockUsageDAO
'        Return serviceDAO.GetJobStockUsageRecordByReqID(AdditionalRequirementsID)
'    End Function

'    Function GetJobStockUsageByReqID(ByVal AdditionalRequirementsID As Integer) As List(Of StockUsage)
'        Dim serviceDAO As New StockUsageDAO
'        Return serviceDAO.GetJobStockUsageByReqID(AdditionalRequirementsID)
'    End Function

'    Public Function updateStkRecord(stockUsage As StockUsage, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer
'        Dim stockUsageDAO As New StockUsageDAO
'        Return stockUsageDAO.updateStkRecord(stockUsage)
'    End Function

'    Function AddStkRecord(Stk As StockUsage, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer
'        Dim stockUsageDAO As New StockUsageDAO
'        Return stockUsageDAO.AddStkRecord(Stk, cnn, trans)
'    End Function

'    Function getJobTypeByID(ByVal intID As Integer) As OzrollPSLVSchedulingModel.JobType
'        Dim jobTypeDAO As New JobTypeDAO
'        Return jobTypeDAO.getJobTypeByID(intID)
'    End Function

'    Function getLouvreStyleByID(ByVal intID As Integer) As LouvreStyle
'        Dim louvreStyleDAO As New LouvreStyleDAO
'        Return louvreStyleDAO.getLouvreStyleByID(intID)
'    End Function

'    Function getLouvreStyles() As List(Of LouvreStyle)
'        Dim louvreStyleDAO As New LouvreStyleDAO
'        Return louvreStyleDAO.getLouvreStyles()
'    End Function

'    Function getOrderTypeByID(ByVal intID As Integer) As OrderType
'        Dim orderTypeDAO As New OrderTypeDAO
'        Return orderTypeDAO.getOrderTypeByID(intID)
'    End Function

'#Region "LouvreJobOptimise"

'Function addLouvreJobOptimiseRecord(intScheduleID As Integer, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'    Dim serviceDAO As New LouvreJobOptimiseDAO
'    Return serviceDAO.addLouvreJobOptimiseRecord(intScheduleID, cnn, trans)

'End Function

'Function getLouvreJobOptimiseByScheduleID(intScheduleID As Integer) As DataTable

'    Dim serviceDAO As New LouvreJobOptimiseDAO
'    Return serviceDAO.getLouvreJobOptimiseByScheduleID(intScheduleID)

'End Function

'#End Region

'    Function addLouvreExtraProductRecord(cLouvreExtra As LouvreExtraProduct, cnn As SqlConnection, ByRef trans As SqlTransaction) As Boolean

'        Dim serviceDAO As New LouvreExtraProductDAO
'        Return serviceDAO.AddLouvreExtraProduct(cLouvreExtra, cnn, trans)

'    End Function

'    Function AddLouvreExtraProduct(cLouvreExtraProduct As LouvreExtraProduct, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New LouvreExtraProductDAO
'        Return serviceDAO.AddLouvreExtraProduct(cLouvreExtraProduct, cnn, trans)

'    End Function

'    Function DeleteLouvreExtraProductsByLouvreDetailsID(cLouvreDetailsID As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New LouvreExtraProductDAO
'        Return serviceDAO.DeleteLouvreExtraProductsByLouvreDetailsID(cLouvreDetailsID, cnn, trans)

'    End Function


'#Region "Costing"

'    ''' <summary>
'    ''' Gets the quotes from the database for the given parameters. 
'    ''' Missing optional parameters and nothings are ignored by the filter.
'    ''' </summary>
'    ''' <param name="intCustomerID">The optional customer ID to get the records for.</param>
'    ''' <returns>A <see cref="List(Of Quote)"/> containing the matching quotes in the database.</returns>
'    Function GetQuotesByParameters(Optional intCustomerID As Integer? = Nothing) As List(Of Quote)
'        Dim quoteDAO As New QuoteDAO
'        Return quoteDAO.GetQuotesByParameters(intCustomerID)
'    End Function

'    ''' <summary>
'    ''' Adds or Updates the given <see cref="Quote"/> record in the database. ID of 0 or less is an add, 1+ is an attempted update.
'    ''' </summary>
'    ''' <param name="cQuote">The <see cref="Quote"/> to add or update in the database.</param>
'    ''' <returns>The ID of the added/updated record. 0 returned if failed.</returns>
'    Function AddOrUpdateQuoteRecord(cQuote As Quote) As Integer
'        Dim quoteDAO As New QuoteDAO
'        Return quoteDAO.AddOrUpdateQuoteRecord(cQuote)
'    End Function

'    ''' <summary>
'    ''' Gets the address zones from the database.
'    ''' </summary>
'    ''' <returns>A <see cref="List(Of AddressZone)"/> containing the address zones in the database.</returns>
'    Function GetAddressZones() As List(Of AddressZone)
'        Dim addressZoneDAO As New AddressZoneDAO
'        Return addressZoneDAO.GetAddressZones()
'    End Function

'    ''' <summary>
'    ''' Adds or Updates the given <see cref="AddressZone"/> record in the database. ID of 0 or less is an add, 1+ is an attempted update.
'    ''' </summary>
'    ''' <param name="cAddressZone">The <see cref="AddressZone"/> to add or update in the database.</param>
'    ''' <returns>The ID of the added/updated record. 0 returned if failed.</returns>
'    Function AddOrUpdateAddressZone(cAddressZone As AddressZone) As Integer
'        Dim addressZoneDAO As New AddressZoneDAO
'        Return addressZoneDAO.AddOrUpdateAddressZoneRecord(cAddressZone)
'    End Function

'    ''' <summary>
'    ''' Deletes the Address Zone record in the database for the given ID.
'    ''' </summary>
'    ''' <param name="intID">The ID of the address zone to remove from the database.</param>
'    ''' <returns>The number of address zone rows removed.</returns>
'    Function DeleteAddressZoneByID(intID As Integer) As Integer
'        Dim addressZoneDAO As New AddressZoneDAO
'        Return addressZoneDAO.DeleteAddressZoneByID(intID)
'    End Function

'    ''' <summary>
'    ''' Deletes the Address Zone record in the database for the given ID.
'    ''' </summary>
'    ''' <param name="intID">The ID of the address zone to remove from the database.</param>
'    ''' <returns>The number of address zone rows removed.</returns>
'    Function DeleteAddressZoneRangeByID(intID As Integer) As Integer
'        Dim addressZoneDAO As New AddressZoneDAO
'        Return addressZoneDAO.DeleteAddressZoneRangeByID(intID)
'    End Function

'    ''' <summary>
'    ''' Gets the louvre prices from the database for the given parameters.
'    ''' Missing optional parameters and nothings are ignored by the filter.
'    ''' </summary>
'    ''' <param name="intCategoryID">The optional Category ID to get the Louvre Prices for.</param>
'    ''' <param name="intLouvreStyleID">The optional Louvre Style ID to get the Louvre Prices for.</param>
'    ''' <param name="intLouvreTypeID">The optional Louvre Type ID to get the Louvre Prices for.</param>
'    ''' <param name="intCoatingTypeID">The optional Coating Type ID to get the Louvre Prices for.</param>
'    ''' <param name="dteEffectiveDateTime">The optional Effective Date to get the Louvre Extra Prices for.</param>
'    ''' <param name="intHeight">The optional Height Date to get the Louvre Extra Prices for.</param>
'    ''' <param name="intWidth">The optional Width Date to get the Louvre Extra Prices for.</param>
'    ''' <returns>A <see cref="List(Of LouvrePrice)"/> containing the matching Louvre Prices in the database.</returns>
'    Function GetLouvrePricesByParameters(
'            Optional intCategoryID As Integer? = Nothing,
'            Optional intLouvreStyleID As Integer? = Nothing,
'            Optional intLouvreTypeID As Integer? = Nothing,
'            Optional intCoatingTypeID As Integer? = Nothing,
'            Optional dteEffectiveDateTime As Date? = Nothing,
'            Optional intHeight As Integer? = Nothing,
'            Optional intWidth As Integer? = Nothing
'        ) As List(Of LouvrePrice)

'        Dim louvrePricesDAO As New LouvrePricesDAO
'        Return louvrePricesDAO.getLouvrePricesByParameters(intCategoryID,
'                                                           intLouvreStyleID,
'                                                           intLouvreTypeID,
'                                                           intCoatingTypeID,
'                                                           dteEffectiveDateTime,
'                                                           intHeight,
'                                                           intWidth)
'    End Function

'    ''' <summary>
'    ''' Adds or Updates the given <see cref="LouvrePrice"/> record in the database. ID of 0 or less is an add, 1+ is an attempted update.
'    ''' </summary>
'    ''' <param name="cLouvrePrice">The <see cref="LouvrePrice"/> to add or update in the database.</param>
'    ''' <returns>The ID of the added/updated record. 0 returned if failed.</returns>
'    Function AddOrUpdateLouvrePrice(cLouvrePrice As LouvrePrice) As Integer
'        Dim louvrePricesDAO As New LouvrePricesDAO
'        Return louvrePricesDAO.addOrUpdateLouvrePriceRecord(cLouvrePrice)
'    End Function

'    ''' <summary>
'    ''' Gets the ACTIVE Louvre Extra Prices from the database for the given parameters.
'    ''' Missing optional parameters and nothings are ignored by the filter.
'    ''' </summary>
'    ''' <param name="intCustomerID">The optional customer ID to get the Louvre Extra Prices for.</param>
'    ''' <param name="intEffectiveDateTime">The optional Effective Date Time to get the Louvre Extra Prices for.</param>
'    ''' <returns>A <see cref="List(Of LouvreExtraPrice)"/> containing the matching Louvre Prices in the database.</returns>
'    Function GetLouvreExtraPricesByParameters(Optional intCustomerID As Integer? = Nothing,
'                                              Optional intEffectiveDateTime As Date? = Nothing) As List(Of LouvreExtraPrice)

'        Dim louvreExtraPricesDAO As New LouvreExtraPricesDAO
'        Return louvreExtraPricesDAO.getLouvreExtraPricesByParameters(intCustomerID, intEffectiveDateTime)
'    End Function

'    ''' <summary>
'    ''' Gets the ACTIVE AND INACTIVE louvre prices for the given parameters from the tblLouvrePrices database table.
'    ''' </summary>
'    ''' <param name="intCategoryID">The optional Category ID to get the Louvre Extra Prices for.</param>
'    ''' <param name="intExtraProductID">The optional Extra Product ID to get the Louvre Extra Prices for.</param>
'    ''' <param name="intEffectiveDateTimeStart">The optional Effective Dat Time Range Start to get the Louvre Extra Prices for.</param>
'    ''' <param name="intEffectiveDateTimeEnd">The optional Effective Date Time Range End to get the Louvre Extra Prices for.</param>
'    ''' <returns>A <see cref="List(Of LouvreExtraPrice)"/> representing the database table containing ACTIVE AND INACTIVE prices.</returns>
'    Function GetAllLouvreExtraPricesByParameters(Optional intCategoryID As Integer? = Nothing,
'                                                    Optional intExtraProductID As Integer? = Nothing,
'                                                    Optional intEffectiveDateTimeStart As Date? = Nothing,
'                                                    Optional intEffectiveDateTimeEnd As Date? = Nothing) As List(Of LouvreExtraPrice)

'        Dim louvreExtraPricesDAO As New LouvreExtraPricesDAO
'        Return louvreExtraPricesDAO.getAllLouvreExtraPricesByParameters(intCategoryID,
'                                                                        intExtraProductID,
'                                                                        intEffectiveDateTimeStart,
'                                                                        intEffectiveDateTimeEnd)
'    End Function

'    ''' <summary>
'    ''' Adds or Updates the given <see cref="LouvreExtraPrice"/> record in the database. ID of 0 or less is an add, 1+ is an attempted update.
'    ''' </summary>
'    ''' <param name="cLouvreExtraPrice">The <see cref="LouvreExtraPrice"/> to add or update in the database.</param>
'    ''' <returns>The ID of the added/updated record. 0 returned if failed.</returns>
'    Function AddOrUpdateLouvreExtraPrice(cLouvreExtraPrice As LouvreExtraPrice) As Integer
'        Dim louvreExtraPricesDAO As New LouvreExtraPricesDAO
'        Return louvreExtraPricesDAO.addOrUpdateLouvreExtraPriceRecord(cLouvreExtraPrice)
'    End Function

'    ''' <summary>
'    ''' Gets the louvre categories from the database.
'    ''' </summary>
'    ''' <returns>A <see cref="List(Of LouvreCategory)"/> containing the matching LouvreCategories in the database.</returns>
'    Function GetLouvreCategories() As List(Of LouvreCategory)
'        Dim louvreCategoriesDAO As New LouvreCategoriesDAO
'        Return louvreCategoriesDAO.getLouvreCategories()
'    End Function

'    ''' <summary>
'    ''' Adds or Updates the given <see cref="LouvreCategory"/> record in the database. ID of 0 or less is an add, 1+ is an attempted update.
'    ''' </summary>
'    ''' <param name="cLouvreCategory">The <see cref="LouvreCategory"/> to add or update in the database.</param>
'    ''' <returns>The ID of the added/updated record. 0 returned if failed.</returns>
'    Function AddOrUpdateLouvreCategory(cLouvreCategory As LouvreCategory) As Integer
'        Dim louvreCategoriesDAO As New LouvreCategoriesDAO
'        Return louvreCategoriesDAO.AddOrUpdateLouvreCategoryRecord(cLouvreCategory)
'    End Function

'#End Region

'#Region "CostingPlantations"

'    ''' <summary>
'    ''' Gets the Plantation prices from the database for the given parameters.
'    ''' Missing optional parameters and nothings are ignored by the filter.
'    ''' </summary>
'    ''' <param name="intCategoryID">The optional Category ID to get the Plantation Prices for.</param>
'    ''' <param name="dteEffectiveDateTime">The optional Effective Date to get the Plantation Extra Prices for.</param>
'    ''' <returns>A <see cref="List(Of PlantationPrice)"/> containing the matching Plantation Prices in the database.</returns>
'    Function GetPlantationPricesByParameters(
'            Optional intCategoryID As Integer? = Nothing,
'            Optional dteEffectiveDateTime As Date? = Nothing
'        ) As List(Of PlantationPrice)

'        Dim plantationPricesDAO As New PlantationPricesDAO
'        Return plantationPricesDAO.getPlantationPricesByParameters(intCategoryID, dteEffectiveDateTime)
'    End Function

'    ''' <summary>
'    ''' Adds or Updates the given <see cref="PlantationPrice"/> record in the database. ID of 0 or less is an add, 1+ is an attempted update.
'    ''' </summary>
'    ''' <param name="cPlantationPrice">The <see cref="PlantationPrice"/> to add or update in the database.</param>
'    ''' <returns>The ID of the added/updated record. 0 returned if failed.</returns>
'    Function AddOrUpdatePlantationPrice(cPlantationPrice As PlantationPrice) As Integer
'        Dim plantationPricesDAO As New PlantationPricesDAO
'        Return plantationPricesDAO.addOrUpdatePlantationPriceRecord(cPlantationPrice)
'    End Function

'    ''' <summary>
'    ''' Gets the Plantation Extra Price from the database for the given ID.
'    ''' </summary>
'    ''' <param name="intID">The ID to get the Plantation Extra Price for.</param>
'    ''' <returns>A <see cref=PlantationExtraPrice"/> matching Plantation Price in the database.</returns>
'    Function GetPlantationExtraPriceByID(intID As Integer) As PlantationExtraPrice
'        Dim plantationExtraPricesDAO As New PlantationExtraPricesDAO
'        Return plantationExtraPricesDAO.GetPlantationExtraPriceByID(intID)
'    End Function

'    ''' <summary>
'    ''' Gets the Plantation Extra Prices from the database for the given parameters.
'    ''' Missing optional parameters and nothings are ignored by the filter.
'    ''' </summary>
'    ''' <param name="intCustomerID">The optional customer ID to get the Plantation Extra Prices for.</param>
'    ''' <param name="intEffectiveDateTime">The optional Effective Date Time to get the Plantation Extra Prices for.</param>
'    ''' <returns>A <see cref="List(Of PlantationExtraPrice)"/> containing the matching Plantation Prices in the database.</returns>
'    Function GetPlantationExtraPricesByParameters(Optional intCustomerID As Integer? = Nothing,
'                                                  Optional intEffectiveDateTime As Date? = Nothing) As List(Of PlantationExtraPrice)
'        Dim plantationExtraPricesDAO As New PlantationExtraPricesDAO
'        Return plantationExtraPricesDAO.GetPlantationExtraPricesByParameters(intCustomerID, intEffectiveDateTime)
'    End Function

'    ''' <summary>
'    ''' Adds or Updates the given <see cref="PlantationExtraPrice"/> record in the database. ID of 0 or less is an add, 1+ is an attempted update.
'    ''' </summary>
'    ''' <param name="cPlantationExtraPrice">The <see cref="PlantationExtraPrice"/> to add or update in the database.</param>
'    ''' <returns>The ID of the added/updated record. 0 returned if failed.</returns>
'    Function AddOrUpdatePlantationExtraPrice(cPlantationExtraPrice As PlantationExtraPrice) As Integer
'        Dim plantationExtraPricesDAO As New PlantationExtraPricesDAO
'        Return plantationExtraPricesDAO.AddOrUpdatePlantationExtraPriceRecord(cPlantationExtraPrice)
'    End Function

'    ''' <summary>
'    ''' Gets the Plantation categories from the database.
'    ''' </summary>
'    ''' <returns>A <see cref="List(Of PlantationCategory)"/> containing the matching PlantationCategories in the database.</returns>
'    Function GetPlantationCategories() As List(Of PlantationCategory)
'        Dim plantationCategoriesDAO As New PlantationCategoriesDAO
'        Return plantationCategoriesDAO.getPlantationCategories()
'    End Function

'    ''' <summary>
'    ''' Adds or Updates the given <see cref="PlantationCategory"/> record in the database. ID of 0 or less is an add, 1+ is an attempted update.
'    ''' </summary>
'    ''' <param name="cPlantationCategory">The <see cref="PlantationCategory"/> to add or update in the database.</param>
'    ''' <returns>The ID of the added/updated record. 0 returned if failed.</returns>
'    Function AddOrUpdatePlantationCategory(cPlantationCategory As PlantationCategory) As Integer
'        Dim plantationCategoriesDAO As New PlantationCategoriesDAO
'        Return plantationCategoriesDAO.AddOrUpdatePlantationCategoryRecord(cPlantationCategory)
'    End Function

'#End Region

'#Region "Plantation Extra Products"

'    Function GetPlantationExtraProductsByPlantationJobDetailsID(intPlantationJobDetailsID As Integer) As List(Of PlantationExtraProduct)

'        Dim serviceDAO As New PlantationExtraProductDAO
'        Return serviceDAO.GetPlantationExtraProductsListByPlantationJobDetailsID(intPlantationJobDetailsID)

'    End Function

'    Function AddPlantationExtraProduct(cPlantationExtraProduct As PlantationExtraProduct, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New PlantationExtraProductDAO
'        Return serviceDAO.AddPlantationExtraProduct(cPlantationExtraProduct, cnn, trans)

'    End Function

'    Function DeletePlantationExtraProductsByPlantationJobDetailsID(cPlantationDetailsID As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer

'        Dim serviceDAO As New PlantationExtraProductDAO
'        Return serviceDAO.DeletePlantationExtraProductsByPlantationJobDetailsID(cPlantationDetailsID, cnn, trans)

'    End Function

'#End Region

'    ''' <summary>
'    ''' Gets the louvre types from the database.
'    ''' </summary>
'    ''' <returns>A <see cref="List(Of LouvreType)"/> containing the matching Louvre Types in the database.</returns>
'    Function GetLouvreTypes() As List(Of LouvreType)
'        Dim louvreTypesDAO As New LouvreTypesDAO
'        Return louvreTypesDAO.GetLouvreTypes()
'    End Function

'    Function getUnitDescriptions() As List(Of UnitDescription)
'        Dim unitDescriptionDAO As New UnitDescriptionDAO
'        Return unitDescriptionDAO.GetUnitDescriptions()
'    End Function

'    Function getColours() As List(Of Colour)
'        Dim colourDAO As New ColourDAO
'        Return colourDAO.GetColours()
'    End Function

'    Public Function GetLouvreExtraProductsListByLouvreDetailsID(intLouverDetailsID As Integer) As List(Of LouvreExtraProduct)

'        Dim serviceDAO As New LouvreExtraProductDAO
'        Return serviceDAO.GetLouvreExtraProductsListByLouvreDetailsID(intLouverDetailsID)

'    End Function

'    Public Function GetExtraProductLouvresList() As List(Of ExtraProductLouvres)

'        Dim serviceDAO As New ExtraProductLouvresDAO
'        Return serviceDAO.GetExtraProductsList()

'    End Function

'    ''' <summary>
'    ''' Adds or Updates the Extra Product Louvres List record in the database. ID > 0 is update, otherwise add.
'    ''' </summary>
'    ''' <returns>The ID of the record in the database.</returns>
'    Public Function AddOrUpdateExtraProductLouvres(ByVal cExtraProduct As ExtraProductLouvres, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer
'        Dim cExtraProductDAO As New ExtraProductLouvresDAO
'        Return cExtraProductDAO.AddOrUpdateExtraProduct(cExtraProduct, cnn, trans)
'    End Function

'    Function UpdateLoginDetailsLouvres(cLoginDetails As LoginDetails, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Boolean
'        Dim ServiceDAO As New LoginDetailsDAO
'        Return ServiceDAO.UpdateLoginDetailsLouvres(cLoginDetails)
'    End Function

'    Function AddLoginDetailsLouvres(cLoginDetails As LoginDetails, Optional ByVal cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer
'        Dim ServiceDAO As New LoginDetailsDAO
'        Return ServiceDAO.AddLoginDetailsLouvres(cLoginDetails)
'    End Function

'    Function GetLoginDetails() As List(Of LoginDetails)
'        Dim ServiceDAO As New LoginDetailsDAO
'        Return ServiceDAO.GetLoginDetails()
'    End Function

'    Function GetLoginDetailsByID(intID As Integer) As LoginDetails
'        Dim ServiceDAO As New LoginDetailsDAO
'        Return ServiceDAO.GetLoginDetailsByID(intID)
'    End Function

'    Function GetLoginDetailsByParameters(Optional intCustomerID As Integer? = Nothing,
'                                         Optional strLoginName As String = Nothing) As List(Of LoginDetails)
'        Dim ServiceDAO As New LoginDetailsDAO
'        Return ServiceDAO.GetLoginDetailsByParameters(intCustomerID, strLoginName)
'    End Function

'    ''' <summary>
'    ''' Returns a <see cref="List(Of LouvreProductionSheetSelect)"/> containing records with ONLY -1 (dynamic) number of panels.
'    ''' </summary>
'    ''' <returns>Returns a <see cref="List(Of LouvreProductionSheetSelect)"/> containing records with ONLY -1 (dynamic) number of panels.</returns>
'    Function GetLouvreProductionSheetSelects() As List(Of LouvreProductionSheetSelect)
'        Dim ServiceDAO As New LouvreProductionSheetSelectDAO
'        Return ServiceDAO.GetLouvreProductionSheetSelects()
'    End Function

'#Region "ProductionScheduleFile"

'    Public Function GetProductionScheduleFileByID(boolLoadBinary As Boolean, intID As Integer) As ProductionScheduleFile
'        Dim ServiceDAO As New ProductionScheduleFileDAO
'        Return ServiceDAO.GetProductionScheduleFileByID(boolLoadBinary, intID)
'    End Function

'    Public Function GetProductionScheduleFilesByParameters(boolLoadBinary As Boolean,
'                                                           Optional intProdScheduleID As Integer? = Nothing,
'                                                           Optional intLouvreDetailID As Integer? = Nothing,
'                                                           Optional intLouvreExtraID As Integer? = Nothing,
'                                                           Optional enumFileType As SharedEnums.ProductionScheduleFileType? = Nothing) As List(Of ProductionScheduleFile)

'        Dim ServiceDAO As New ProductionScheduleFileDAO
'        Return ServiceDAO.GetProductionScheduleFilesByParameters(boolLoadBinary,
'                                                                 intProdScheduleID,
'                                                                 intLouvreDetailID,
'                                                                 intLouvreExtraID,
'                                                                 enumFileType)
'    End Function

'    Public Function GetProductionScheduleFileBinaryByID(intID As Integer) As Byte()
'        Dim ServiceDAO As New ProductionScheduleFileDAO
'        Return ServiceDAO.GetProductionScheduleFileBinaryByID(intID)
'    End Function

'    Function AddOrUpdateProductionScheduleFile(cProductionScheduleFile As ProductionScheduleFile, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer
'        Dim ServiceDAO As New ProductionScheduleFileDAO
'        Return ServiceDAO.AddOrUpdateProductionScheduleFile(cProductionScheduleFile)
'    End Function

'    Function DeleteProductionScheduleFileByID(intID As Integer, Optional cnn As SqlConnection = Nothing, Optional ByRef trans As SqlTransaction = Nothing) As Integer
'        Dim ServiceDAO As New ProductionScheduleFileDAO
'        Return ServiceDAO.DeleteProductionScheduleFileByID(intID)
'    End Function

'#End Region

'End Class
