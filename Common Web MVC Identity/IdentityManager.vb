Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices

<Assembly: internalsVisibleTo("Shoniz.Common.Web.MVC.Identity.Test")> 

Public Module IdentityManager
    Friend SAM_CONNECTION_STRING As String = ""

    Public Sub SetConnetionString(ConnectionString As String)
        SAM_CONNECTION_STRING = ConnectionString
    End Sub

    Public Sub SetConnetionString(ConnectionStringBuilder As SqlConnectionStringBuilder)
        SAM_CONNECTION_STRING = ConnectionStringBuilder.ToString
    End Sub

    Friend Function TableFunction(Name As String, ParamArray Parameters() As SqlParameter) As DataTable
        If SAM_CONNECTION_STRING = "" Then
            Throw New InvalidOperationException("ConnectionString is not set.")
        End If

        Dim connection As New SqlConnection(SAM_CONNECTION_STRING)
        Dim command As New SqlCommand(String.Format("SELECT * FROM dbo.{0}({1})",
                                                    Name,
                                                    String.Join(",", Parameters.Select(Function(x) x.ParameterName).ToArray)),
                                      connection)

        Parameters.ToList.ForEach(Sub(x) command.Parameters.AddWithValue(x.ParameterName, x.Value))

        Dim dataAdapter As New SqlDataAdapter(command)
        Dim dataTable As New DataTable(Name)

        connection.Open()
        dataAdapter.Fill(dataTable)
        connection.Close()

        Return dataTable
    End Function

    Friend Function ScalarFunction(Name As String, ParamArray Parameters() As SqlParameter) As Object
        If SAM_CONNECTION_STRING = "" Then
            Throw New InvalidOperationException("ConnectionString is not set.")
        End If

        Dim connection As New SqlConnection(SAM_CONNECTION_STRING)
        Dim command As New SqlCommand(String.Format("SELECT dbo.{0}({1})",
                                                    Name,
                                                    String.Join(",", Parameters.Select(Function(x) x.ParameterName).ToArray)),
                                      connection)

        Parameters.ToList.ForEach(Sub(x) command.Parameters.AddWithValue(x.ParameterName, x.Value))

        Dim result As Object = Nothing

        connection.Open()
        result = command.ExecuteScalar
        connection.Close()

        Return result
    End Function
End Module