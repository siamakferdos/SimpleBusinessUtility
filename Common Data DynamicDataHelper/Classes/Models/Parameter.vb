Public Class Parameter
    Public Property Key() As String
    Public Property Value() As Object

    Sub New(key As String, value As Object)
        Me.Key = key
        Me.Value = value
    End Sub
End Class
