Imports Microsoft.AspNet.Identity
Imports Microsoft.AspNet.Identity.Owin
Imports Microsoft.Owin

Public Class UserManager
    Inherits UserManager(Of User, Integer)

    Sub New(UserStore As IUserStore(Of User, Integer))
        MyBase.New(UserStore)
    End Sub

    Sub New(UserStore As IUserStore(Of User, Integer), SAMConnectionString As String)
        MyBase.New(UserStore)
        IdentityManager.SetConnetionString(SAMConnectionString)
    End Sub

    Public Shared Function Create(options As IdentityFactoryOptions(Of UserManager), context As IOwinContext)
        Dim manager = New UserManager(New UserStore)

        ' Configure validation logic for usernames
        manager.UserValidator = New UserValidator(Of User, Integer)(manager) With {.AllowOnlyAlphanumericUserNames = True,
                                                                                   .RequireUniqueEmail = False}

        ' Configure validation logic for passwords
        manager.PasswordValidator = New PasswordValidator With {.RequiredLength = 3,
                                                                .RequireNonLetterOrDigit = False,
                                                                .RequireDigit = False,
                                                                .RequireLowercase = False,
                                                                .RequireUppercase = False}

        ' Configure user lockout defaults
        manager.UserLockoutEnabledByDefault = True
        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5)
        manager.MaxFailedAccessAttemptsBeforeLockout = 5
        manager.PasswordHasher = New PasswordHasher

        Dim dataProtectionProvider = options.DataProtectionProvider
        If (dataProtectionProvider IsNot Nothing) Then
            manager.UserTokenProvider = New DataProtectorTokenProvider(Of User, Integer)(dataProtectionProvider.Create("ASP.NET Identity"))
        End If

        Return manager
    End Function
End Class