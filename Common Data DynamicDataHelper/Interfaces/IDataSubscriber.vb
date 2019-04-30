Public Interface IDataSubscriber
    Property PortNumber As Integer
    Sub HandleData(CallTag As String, Response As DataSet)

End Interface
