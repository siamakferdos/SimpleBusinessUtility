Imports System.Data.SqlClient
Imports System.Threading.Tasks.Dataflow

Friend Class DataTransmitInterface

    Private Shared _instance As DataTransmitInterface

    Dim _requestRunner As New TransformBlock(Of DataRequest, Task(Of DataRequest))(AddressOf GetRequestTask)
    Dim _responsePublisher As New ActionBlock(Of Task(Of DataRequest))(AddressOf PublishResponse)

    Private Sub New()
        _requestRunner.LinkTo(_responsePublisher)
    End Sub

    ''' <summary>
    ''' Gets the only instance(Singleton) of ADOHandler class. Users are only able to use ADOHandler thorght this property
    ''' </summary>
    ''' <value><c>Shoniz.CMMS.Data.ADoHandler</c></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Instance As DataTransmitInterface
        Get
            If _instance Is Nothing Then
                _instance = New DataTransmitInterface
            End If
            Return _instance
        End Get
    End Property

#Region "Public Methods"

    ''' <summary>
    ''' Runs a <c>Shoniz.CMMS.Shared.Framework.DataRequest</c> object asynchronously. and publishes the result whenever it's ready on the IDataProvider object.
    ''' </summary>
    ''' <param name="Request">A <c>Shoniz.CMMS.Shared.Framework.DataRequest</c> object to run.</param>
    ''' <param name="Owner">A <c>Shoniz.CMMS.Shared.Framework.IDataProvider</c> object to publish return value from executing SQL command.</param>
    ''' <remarks></remarks>
    Public Sub RunAsyncronous(Request As DataRequest)
        If Not Request.HasParameter("_RequestId") Then
            Request.AddParameter("_RequestId", Request.RequestId.ToString)
        End If
        _requestRunner.Post(Request)
    End Sub

    ''' <summary>
    ''' Processes a <c>Shoniz.CMMS.Shared.Framework.DataRequest</c> object and returns the result immediately.
    ''' </summary>
    ''' <param name="Request">A <c>Shoniz.CMMS.Shared.Framework.DataRequest</c> object to run.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RunImmediately(Request As DataRequest) As DataRequest
        Request.AddParameter("_RequestId", Request.RequestId.ToString)

        Dim task = GetRequestTask(Request)
        task.Start()
        task.Wait()
        Dim response = task.Result

        'RenameTables(response)

        Return response
    End Function

#End Region

#Region "Private Methods"

    Private Function GetRequestTask(Request As DataRequest) As Task(Of DataRequest)
        Dim task As New Task(Of DataRequest)(Function() As DataRequest
                                                 Dim sqlConnection As New SqlClient.SqlConnection(Request.ConnectionString)
                                                 Dim sqlCommand = Request.GetSqlCommand
                                                 sqlCommand.Connection = sqlConnection

                                                 Dim dataset As New DataSet
                                                 Dim dataAdapter As New SqlDataAdapter(sqlCommand)

                                                 Select Case sqlConnection.State
                                                     Case ConnectionState.Closed
                                                         sqlConnection.Open()
                                                 End Select

                                                 dataAdapter.Fill(dataset)

                                                 sqlConnection.Close()
                                                 System.Data.SqlClient.SqlConnection.ClearAllPools()

                                                 Request.Result = dataset

                                                 Return Request
                                             End Function)

        Return task
    End Function

    Private Sub PublishResponse(response As Task(Of DataRequest))
        response.Start()
        response.Wait()
        Dim responeResult = response.Result

        'RenameTables(responeResult)

        responeResult.SqlDependencyNotify()
    End Sub

    Private Sub RenameTables(ByRef response As DataRequest)
        If response.Result IsNot Nothing Then
            Dim tableCount = response.Result.Tables.Count

            If tableCount >= 2 Then
                response.Result.Tables(tableCount - 1).TableName = "Tables"

                For cnt = 0 To response.Result.Tables.Count - 2
                    Dim tableName As String = "Unknown"
                    If response.Result.Tables(tableCount - 1).Rows.Count >= cnt Then
                        Select Case response.Result.Tables(tableCount - 1).Rows(cnt)(0)
                            Case "D"
                                If Not IsDBNull(response.Result.Tables(tableCount - 1).Rows(cnt)(1)) Then
                                    tableName = response.Result.Tables(tableCount - 1).Rows(cnt)(1)
                                End If

                            Case "S"
                                tableName = "Schema"

                            Case "E"
                                tableName = "Exceptions"

                            Case "R"
                                tableName = "Status"

                        End Select
                    End If
                    response.Result.Tables(cnt).TableName = tableName
                Next
            End If
        End If
    End Sub

#End Region

End Class