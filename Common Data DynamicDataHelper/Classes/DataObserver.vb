Imports System.Reflection

Public Class DataObserver
    Implements IDataObserver

    Dim _isUsingTimeMachine As Boolean = True
    Dim _catalogResolver As Func(Of String, String)

#Region "Config Methods"
    Sub New(CatalogResolver As Func(Of String, String), Optional CurrentDate As Date = Nothing)
        If CurrentDate = Nothing Then
            Me.CurrentDate = Now
            _isUsingTimeMachine = False
        Else
            Me.CurrentDate = CurrentDate
            _isUsingTimeMachine = True
        End If

        _catalogResolver = CatalogResolver
    End Sub
#End Region

#Region "Data Specific"

    Public Sub TurnTimeMachineOff()
        _isUsingTimeMachine = False
    End Sub

    Dim _currentDate As Date = Now

    Public Property CurrentDate As Date Implements IDataObserver.CurrentDate
        Get
            If _isUsingTimeMachine Then
                Return _currentDate
            Else
                Return Now
            End If
        End Get
        Set(value As Date)
            _currentDate = value
            _isUsingTimeMachine = True
        End Set
    End Property

    Public Function RunSp(Catalog As String, SpName As String, ParamArray Parameters() As Parameter) As DataSet Implements IDataObserver.RunSp
        Dim request As New DataRequest With {.ConnectionString = ResolveCatalog(Catalog), .SpName = SpName}

        request.AddParameter("_CD", CurrentDate.ToStandardString)

        Dim stackTrace = New StackTrace
        Dim stackFrame = stackTrace.GetFrame(1)
        Dim methodName = stackFrame.GetMethod.Name
        Dim className = stackFrame.GetMethod.DeclaringType.Name

        request.AddParameter("_Class", className)
        request.AddParameter("_Method", methodName)

        AddOtherParameters(request)

        Parameters.ToList.ForEach(Sub(x) request.AddParameter(x.Key, x.Value))

        Return DataTransmitInterface.Instance.RunImmediately(request).Result
    End Function

    Public Sub RunSp(Catalog As String, SpName As String, OnDataRecieve As Action(Of DataSet), ParamArray Parameters() As Parameter) Implements IDataObserver.RunSp

        Dim request As New DataRequest With {.ConnectionString = ResolveCatalog(Catalog), .OnChange = OnDataRecieve, .SpName = SpName}

        request.AddParameter("_SpName", SpName)
        request.AddParameter("_CD", CurrentDate.ToStandardString)

        Dim stackTrace = New StackTrace
        Dim stackFrame = stackTrace.GetFrame(1)
        Dim methodName = stackFrame.GetMethod.Name
        Dim className = stackFrame.GetMethod.DeclaringType.Name

        request.AddParameter("_Class", className)
        request.AddParameter("_Method", methodName)

        AddOtherParameters(request)

        Parameters.ToList.ForEach(Sub(x) request.AddParameter(x.Key, x.Value))

        DataTransmitInterface.Instance.RunAsyncronous(request)
    End Sub

    Friend Function CallRunSp(Catalog As String, SpName As String, CallerSpName As String, ParamArray Parameters() As Parameter) As DataSet
        Dim request As New DataRequest With {.ConnectionString = ResolveCatalog(Catalog), .SpName = CallerSpName}

        request.AddParameter("_SpName", SpName)
        request.AddParameter("_CD", CurrentDate.ToStandardString)

        Dim stackTrace = New StackTrace
        Dim stackFrame = stackTrace.GetFrame(1)
        Dim methodName = stackFrame.GetMethod.Name
        Dim className = stackFrame.GetMethod.DeclaringType.Name

        request.AddParameter("_Class", className)
        request.AddParameter("_Method", methodName)

        AddOtherParameters(request)

        Parameters.ToList.ForEach(Sub(x) request.AddParameter(x.Key, x.Value))

        Dim result = DataTransmitInterface.Instance.RunImmediately(request)

        Return result.Result
    End Function

    Friend Sub CallRunSp(Catalog As String, SpName As String, CallerSpName As String, OnDataRecieve As Action(Of DataSet), ParamArray Parameters() As Parameter)

        Dim request As New DataRequest With {.ConnectionString = ResolveCatalog(Catalog), .OnChange = OnDataRecieve, .SpName = CallerSpName}

        request.AddParameter("_SpName", SpName)
        request.AddParameter("_CD", CurrentDate.ToStandardString)

        Dim stackTrace = New StackTrace
        Dim stackFrame = stackTrace.GetFrame(1)
        Dim methodName = stackFrame.GetMethod.Name
        Dim className = stackFrame.GetMethod.DeclaringType.Name

        request.AddParameter("_Class", className)
        request.AddParameter("_Method", methodName)

        AddOtherParameters(request)

        Parameters.ToList.ForEach(Sub(x) request.AddParameter(x.Key, x.Value))

        DataTransmitInterface.Instance.RunAsyncronous(request)
    End Sub

    Public Function ResolveCatalog(Catalog As String) As String Implements IDataObserver.ResolveCatalog
        Return _catalogResolver(Catalog)
    End Function

    Private Sub AddOtherParameters(Request As DataRequest)
        Try
            Request.AddParameter("_Ip", System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList(1).ToString())
            Request.AddParameter("_ExecutingVersion", Assembly.GetExecutingAssembly.GetName.Version.ToString)
            Request.AddParameter("_EntryVersion", Assembly.GetEntryAssembly.GetName.Version.ToString)
        Catch ex As Exception
        End Try
    End Sub
#End Region

End Class