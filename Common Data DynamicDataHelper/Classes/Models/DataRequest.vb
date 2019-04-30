Imports System.Data.SqlClient

Friend Class DataRequest

    Private Sub Initialize()
        _parameters = New DataTable
        _parameters.Columns.Add("Name", GetType(String))
        _parameters.Columns.Add("Type", GetType(String))
        _parameters.Columns.Add("Value", GetType(String))
    End Sub

#Region "Public Methods"
    Public Sub New()
        Initialize()
    End Sub

    Public Sub SqlDependencyNotify()
        OnChange.Invoke(Result)
    End Sub

    Public Function GetSqlCommand() As SqlCommand
        Dim sqlCommand As New SqlCommand(Me.SpName)
        sqlCommand.CommandType = CommandType.StoredProcedure
        sqlCommand.Parameters.AddWithValue("@params", _parameters)

        Return sqlCommand
    End Function

    Public Sub AddParameter(Name As String, Value As String)
        If HasParameter(Name) Then
            Throw New InvalidOperationException("This parameter already exists in parameters list")
        Else
            Dim tmp = _parameters.NewRow
            tmp("Name") = Name
            tmp("Type") = Nothing
            tmp("Value") = Value
            _parameters.Rows.Add(tmp)
        End If
    End Sub

    Public Sub AddParameter(Name As String, Type As String, Value As String)
        If HasParameter(Name) Then
            Throw New InvalidOperationException("This parameter already exists in parameters list")
        Else
            Dim tmp = _parameters.NewRow
            tmp("Name") = Name
            tmp("Type") = Type
            tmp("Value") = If(Value, "")
            _parameters.Rows.Add(tmp)
        End If
    End Sub

    Public Function HasParameter(Name As String) As Boolean
        Return (From obj In _parameters.Rows Where obj("Name") = Name).Count > 0
    End Function

    Public Sub RemoveParameter(Name As String)
        If (From obj In _parameters.Rows Where obj("Name") = Name).Count = 0 Then
            Throw New InvalidOperationException(String.Format("There is no parameter with name {0} in parameters list", Name))
        Else
            _parameters.Rows.Remove((From obj In _parameters.Rows Where obj("Name") = Name).Single)
        End If
    End Sub

#End Region

#Region "Properties"

    Public Property ConnectionString As String

    Public ReadOnly Property Text As String
        Get
            Return (From param As DataRow In Parameters.Rows Where param("Name") = "_SpName" Select Value = param("Value").ToString).First
        End Get
    End Property

    Public ReadOnly Property ListenQueue As String
        Get
            Return String.Format("[DynamicDataHelperQueue_{0}]", RequestId.ToString)
        End Get
    End Property

    Public Property OnChange As Action(Of DataSet)

    Dim _parameters As DataTable
    Public Property Parameters As DataTable
        Get
            Return _parameters
        End Get
        Set(value As DataTable)
            _parameters = value
        End Set
    End Property

    Public Property RequestId As Guid = Guid.NewGuid

    '<Newtonsoft.Json.JsonConverter(GetType(DatasetConverter))>
    Public Property Result As DataSet

    Public Property SpName As String

    Public Property IsSubscribedToChanges As Boolean

#End Region

#Region "Operators"
    Public Shared Operator =(x As DataRequest, y As DataRequest) As Boolean
        Return x.RequestId = y.RequestId
    End Operator

    Public Shared Operator <>(x As DataRequest, y As DataRequest) As Boolean
        Return x.RequestId <> y.RequestId
    End Operator
#End Region
End Class
