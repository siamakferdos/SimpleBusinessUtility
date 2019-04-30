Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin.Security
Imports System.Security.Claims
Imports Microsoft.Owin

Public Class SignInManager
    Inherits SignInManager(Of User, Integer)

    Sub New(UserManager As UserManager, AuthenticationManager As IAuthenticationManager)
        MyBase.New(UserManager, AuthenticationManager)
    End Sub
    Sub New(UserManager As UserManager, AuthenticationManager As IAuthenticationManager, SAMConnectionString As String)
        MyBase.New(UserManager, AuthenticationManager)
        IdentityManager.SetConnetionString(SAMConnectionString)
    End Sub

    Public Overrides Function CreateUserIdentityAsync(user As User) As Task(Of ClaimsIdentity)
        Return user.GenerateUserIdentityAsync(DirectCast(UserManager, UserManager))
    End Function

    Public Shared Function Create(options As IdentityFactoryOptions(Of SignInManager), context As IOwinContext) As SignInManager
        Return New SignInManager(context.GetUserManager(Of UserManager)(), context.Authentication)
    End Function
End Class