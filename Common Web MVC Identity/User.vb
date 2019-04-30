Imports Microsoft.AspNet.Identity
Imports System.Security.Claims

Public Class User
    Implements IUser(Of Integer)
    Dim _userId As Integer

    Public ReadOnly Property Id As Integer Implements IUser(Of Integer).Id
        Get
            Return _userId
        End Get
    End Property

    Public Property UserId As Integer
        Get
            Return _userId
        End Get
        Set(value As Integer)
            _userId = value
        End Set
    End Property

    Public Property Username As String Implements IUser(Of Integer).UserName
        Get
            Return _userId
        End Get
        Set(value As String)
            _userId = value
        End Set
    End Property

    Public Property Password As Byte()

    Public Property PasswordText As String

    Public Property FirstName As String
    Public Property LastName As String
    Public Property FullName
        Get
            Return String.Format("{0} {1}", FirstName, LastName)
        End Get
        Set(value)
        End Set
    End Property
    Public Property SystemUserName As String
    Public Property ComputerName As String
    Public Property Sign As Byte()

    Public Async Function GenerateUserIdentityAsync(manager As UserManager) As Task(Of ClaimsIdentity)
        ' Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        Dim userIdentity = Await manager.CreateIdentityAsync(Me, DefaultAuthenticationTypes.ApplicationCookie)
        ' Add custom user claims here
        userIdentity.AddClaim(New Claim("Organization", "Shoniz"))
        Return userIdentity
    End Function
End Class
