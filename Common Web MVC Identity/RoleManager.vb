Imports Microsoft.AspNet.Identity

Public Class RoleManager
    Inherits RoleManager(Of Role, Integer)

    Sub New(RoleStore As IRoleStore(Of Role, Integer))
        MyBase.New(RoleStore)
    End Sub

    Sub New(RoleStore As IRoleStore(Of Role, Integer), SAMConnectionString As String)
        MyBase.New(RoleStore)
        IdentityManager.SetConnetionString(SAMConnectionString)
    End Sub
End Class