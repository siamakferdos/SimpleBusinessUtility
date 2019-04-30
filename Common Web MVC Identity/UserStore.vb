Imports Microsoft.AspNet.Identity
Imports System.Data.SqlClient

Public Class UserStore
    Implements IUserStore(Of User, Integer), 
        IUserPasswordStore(Of User, Integer), 
        IUserRoleStore(Of User, Integer), 
        IUserLockoutStore(Of User, Integer), 
        IUserTwoFactorStore(Of User, Integer), IUserLoginStore(Of User, Integer)


    Shared _accessFailedCount As New Dictionary(Of User, Integer)
    Shared _lockoutEndDate As New Dictionary(Of User, DateTimeOffset)

    Public Function GetAccessFailedCountAsync(user As User) As Threading.Tasks.Task(Of Integer) Implements IUserLockoutStore(Of User, Integer).GetAccessFailedCountAsync
        If Not _accessFailedCount.ContainsKey(user) Then
            _accessFailedCount.Add(user, 0)
        End If
        Return Task.Run(Of Integer)(Function() _accessFailedCount(user))
    End Function

    Public Function GetLockoutEnabledAsync(user As User) As Threading.Tasks.Task(Of Boolean) Implements IUserLockoutStore(Of User, Integer).GetLockoutEnabledAsync
        Return Task.Run(Of Boolean)(Function() True)
    End Function

    Public Function GetLockoutEndDateAsync(user As User) As Threading.Tasks.Task(Of DateTimeOffset) Implements IUserLockoutStore(Of User, Integer).GetLockoutEndDateAsync
        If Not _lockoutEndDate.ContainsKey(user) Then
            _lockoutEndDate.Add(user, New DateTimeOffset(Now))
        End If
        Return Task.Run(Function() _lockoutEndDate(user))
    End Function

    Public Function IncrementAccessFailedCountAsync(user As User) As Threading.Tasks.Task(Of Integer) Implements IUserLockoutStore(Of User, Integer).IncrementAccessFailedCountAsync
        If Not _accessFailedCount.ContainsKey(user) Then
            _accessFailedCount.Add(user, 0)
        End If
        Return Task.Run(Of Integer)(Function()
                                        _accessFailedCount(user) += 1
                                        Return _accessFailedCount(user)
                                    End Function)
    End Function

    Public Function ResetAccessFailedCountAsync(user As User) As Threading.Tasks.Task Implements IUserLockoutStore(Of User, Integer).ResetAccessFailedCountAsync
        If Not _accessFailedCount.ContainsKey(user) Then
            _accessFailedCount.Add(user, 0)
        End If
        Return Task.Run(Sub()
                            _accessFailedCount(user) = 0
                        End Sub)
    End Function

    Public Function SetLockoutEnabledAsync(user As User, enabled As Boolean) As Threading.Tasks.Task Implements IUserLockoutStore(Of User, Integer).SetLockoutEnabledAsync
        Return Task.Run(Sub()
                        End Sub)
    End Function

    Public Function SetLockoutEndDateAsync(user As User, lockoutEnd As DateTimeOffset) As Threading.Tasks.Task Implements IUserLockoutStore(Of User, Integer).SetLockoutEndDateAsync
        If Not _lockoutEndDate.ContainsKey(user) Then
            _lockoutEndDate.Add(user, New DateTimeOffset(Now))
        End If
        Return Task.Run(Sub() _lockoutEndDate(user) = lockoutEnd)
    End Function

    Public Function GetPasswordHashAsync(user As User) As Threading.Tasks.Task(Of String) Implements IUserPasswordStore(Of User, Integer).GetPasswordHashAsync
        Return Task.Run(Of String)(Function() As String
                                       Return user.PasswordText
                                   End Function)
    End Function

    Public Function HasPasswordAsync(user As User) As Threading.Tasks.Task(Of Boolean) Implements IUserPasswordStore(Of User, Integer).HasPasswordAsync
        Return Task.Run(Of Boolean)(Function() As Boolean
                                        Return user.Password.Length > 0
                                    End Function)
    End Function

    Public Function SetPasswordHashAsync(user As User, passwordHash As String) As Threading.Tasks.Task Implements IUserPasswordStore(Of User, Integer).SetPasswordHashAsync
        Return Task.Run(Sub()
                            user.Password = passwordHash.ToBytes
                        End Sub)
    End Function

    Public Function AddToRoleAsync(user As User, roleName As String) As Threading.Tasks.Task Implements IUserRoleStore(Of User, Integer).AddToRoleAsync
        Throw New InvalidOperationException("Adding a User to Role is not allowed. Use Shoniz Application Manager console instead.")
    End Function

    Public Function GetRolesAsync(user As User) As Threading.Tasks.Task(Of IList(Of String)) Implements IUserRoleStore(Of User, Integer).GetRolesAsync
        Return Task.Run(Of IList(Of String))(Function()
                                                 Dim dt = TableFunction("GetRolesByUserId", New SqlParameter("@UserId", user.Id))
                                                 If dt.Rows.Count > 0 Then
                                                     Return dt.ToList(Of Role)().Select(Function(x) x.Name).ToList
                                                 Else
                                                     Return Nothing
                                                 End If
                                             End Function)
    End Function

    Public Function IsInRoleAsync(user As User, roleName As String) As Threading.Tasks.Task(Of Boolean) Implements IUserRoleStore(Of User, Integer).IsInRoleAsync
        Return Task.Run(Of Boolean)(Function() As Boolean
                                        Return ScalarFunction("IsUserInRole", New SqlParameter("@UserId", user.Id), New SqlParameter("@RoleName", roleName))
                                    End Function)
    End Function

    Public Function RemoveFromRoleAsync(user As User, roleName As String) As Threading.Tasks.Task Implements IUserRoleStore(Of User, Integer).RemoveFromRoleAsync
        Throw New InvalidOperationException("Removing a User from Role is not allowed. Use Shoniz Application Manager console instead.")
    End Function

    Public Function CreateAsync(user As User) As Threading.Tasks.Task Implements IUserStore(Of User, Integer).CreateAsync
        Throw New InvalidOperationException("Adding User is not allowed. Use Shoniz Application Manager console instead.")
    End Function

    Public Function DeleteAsync(user As User) As Threading.Tasks.Task Implements IUserStore(Of User, Integer).DeleteAsync
        Throw New InvalidOperationException("Deleting User is not allowed. Use Shoniz Application Manager console instead.")
    End Function

    Public Function FindByIdAsync(userId As Integer) As Threading.Tasks.Task(Of User) Implements IUserStore(Of User, Integer).FindByIdAsync
        Return Task.Run(Of User)(Function() As User
                                     Dim tmp = TableFunction("GetUserById", New SqlParameter("@UserId", userId))
                                     If tmp.Rows.Count > 0 Then
                                         Return tmp.ToList(Of User)().First
                                     Else
                                         Return Nothing
                                     End If
                                 End Function)
    End Function

    Public Function FindByNameAsync(userName As String) As Threading.Tasks.Task(Of User) Implements IUserStore(Of User, Integer).FindByNameAsync
        Return Task.Run(Of User)(Function() As User
                                     Dim tmp = TableFunction("GetUserById", New SqlParameter("@UserId", userName))
                                     If tmp.Rows.Count > 0 Then
                                         Return tmp.ToList(Of User)().First
                                     Else
                                         Return Nothing
                                     End If
                                 End Function)
    End Function

    Public Function UpdateAsync(user As User) As Threading.Tasks.Task Implements IUserStore(Of User, Integer).UpdateAsync
        Throw New InvalidOperationException("Updating/Changing User is not allowed. Use Shoniz Application Manager console instead.")
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    Public Function GetTwoFactorEnabledAsync(user As User) As Task(Of Boolean) Implements IUserTwoFactorStore(Of User, Integer).GetTwoFactorEnabledAsync
        Return Task.Run(Of Boolean)(Function() True)
    End Function

    Public Function SetTwoFactorEnabledAsync(user As User, enabled As Boolean) As Task Implements IUserTwoFactorStore(Of User, Integer).SetTwoFactorEnabledAsync
        Return Task.Run(Sub()
                        End Sub)
    End Function

    Public Function AddLoginAsync(user As User, login As UserLoginInfo) As Task Implements IUserLoginStore(Of User, Integer).AddLoginAsync
        Return Nothing
    End Function

    Public Function FindAsync(login As UserLoginInfo) As Task(Of User) Implements IUserLoginStore(Of User, Integer).FindAsync
        Return Nothing
    End Function

    Public Function GetLoginsAsync(user As User) As Task(Of IList(Of UserLoginInfo)) Implements IUserLoginStore(Of User, Integer).GetLoginsAsync
        Return Nothing
    End Function

    Public Function RemoveLoginAsync(user As User, login As UserLoginInfo) As Task Implements IUserLoginStore(Of User, Integer).RemoveLoginAsync
        Return Nothing
    End Function
End Class

