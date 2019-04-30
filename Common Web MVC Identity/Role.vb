Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security

Public Class Role
    Implements IRole(Of Integer)

    Dim _Id As Integer

    Public ReadOnly Property Id As Integer Implements IRole(Of Integer).Id
        Get
            Return _Id
        End Get
    End Property

    Public Property RoleId As Integer
        Get
            Return _Id
        End Get
        Set(value As Integer)
            _Id = value
        End Set
    End Property

    Public Property Name As String Implements IRole(Of Integer).Name

    Public Property Weight As Byte

    Public Property ProgramId As Integer
End Class