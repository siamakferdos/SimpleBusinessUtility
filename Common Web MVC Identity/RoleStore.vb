Imports Microsoft.AspNet.Identity
Imports System.Data.SqlClient

Public Class RoleStore
    Implements IRoleStore(Of Role, Integer)

    Public Function CreateAsync(role As Role) As Threading.Tasks.Task Implements IRoleStore(Of Role, Integer).CreateAsync
        Throw New InvalidOperationException("Creating Role is not allowed. Use Shoniz Application Manager console instead.")
    End Function

    Public Function DeleteAsync(role As Role) As Threading.Tasks.Task Implements IRoleStore(Of Role, Integer).DeleteAsync
        Throw New InvalidOperationException("Deleting Role is not allowed. Use Shoniz Application Manager console instead.")
    End Function

    Public Function FindByIdAsync(roleId As Integer) As Threading.Tasks.Task(Of Role) Implements IRoleStore(Of Role, Integer).FindByIdAsync
        Return New Task(Function()
                            Dim dt = TableFunction("GetRoleById", New SqlParameter("@Id", roleId))
                            If dt.Rows.Count > 0 Then
                                Return dt.ToList(Of Role)()(0)
                            Else
                                Return Nothing
                            End If
                        End Function)
    End Function

    Public Function FindByNameAsync(roleName As String) As Threading.Tasks.Task(Of Role) Implements IRoleStore(Of Role, Integer).FindByNameAsync
        Return New Task(Function()
                            Dim dt = TableFunction("GetRoleById", New SqlParameter("@Name", roleName))
                            If dt.Rows.Count > 0 Then
                                Return dt.ToList(Of Role)()(0)
                            Else
                                Return Nothing
                            End If
                        End Function)
    End Function

    Public Function UpdateAsync(role As Role) As Threading.Tasks.Task Implements IRoleStore(Of Role, Integer).UpdateAsync
        Throw New InvalidOperationException("Updating/Changing Role is not allowed. Use Shoniz Application Manager console instead.")
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




End Class