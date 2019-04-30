Imports System.Runtime.CompilerServices
Imports System.Data
Imports System.Dynamic
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text

Friend Module Extensions

    <Extension>
    Public Function ToList(inp As DataTable, Optional Schema As DataTable = Nothing) As List(Of Object)
        Dim list As New List(Of Object)
        If inp Is Nothing Then Return list
        For Each row As DataRow In inp.Rows
            Dim obj As Object = New ExpandoObject

            If Schema Is Nothing Then
                For Each col As DataColumn In inp.Columns
                    If IsDBNull(row(col)) Then
                        CType(obj, IDictionary(Of String, Object)).Add(col.ColumnName, Nothing)
                    Else
                        Dim typeC As TypeCode = [Enum].Parse(GetType(System.TypeCode), col.DataType.Name)
                        Dim value = row(col)
                        CType(obj, IDictionary(Of String, Object)).Add(col.ColumnName, Convert.ChangeType(value, typeC))
                    End If
                Next

            Else
                For Each col As DataColumn In inp.Columns
                    Dim peer = (From sch In Schema Where sch("ColumnName").ToString.ToLower = col.ColumnName.ToLower).First
                    Dim value = row(col)
                    Dim typeC As TypeCode = [Enum].Parse(GetType(System.TypeCode), peer("Type").ToString)

                    CType(obj, IDictionary(Of String, Object)).Add(col.ColumnName, Convert.ChangeType(value, typeC))
                Next

            End If

            list.Add(obj)
        Next

        Return list
    End Function

    <Extension>
    Public Function ToList(Of T As New)(inp As DataTable, Optional Schema As DataTable = Nothing, Optional IsGenericMaster As Boolean = False) As List(Of T)
        Dim list As New List(Of T)
        If inp Is Nothing Then Return list
        For Each row As DataRow In inp.Rows
            Dim obj As Object = New ExpandoObject

            If Schema Is Nothing Then
                For Each col As DataColumn In inp.Columns
                    If IsDBNull(row(col)) Then
                        CType(obj, IDictionary(Of String, Object)).Add(col.ColumnName, Nothing)
                    Else
                        Try
                            Dim typeC As TypeCode = [Enum].Parse(GetType(System.TypeCode), col.DataType.Name)
                            Dim value = row(col)
                            CType(obj, IDictionary(Of String, Object)).Add(col.ColumnName, Convert.ChangeType(value, typeC))
                        Catch ex As Exception
                            Dim value = row(col)
                            CType(obj, IDictionary(Of String, Object)).Add(col.ColumnName, value)
                        End Try
                    End If
                Next

            Else
                For Each col As DataColumn In inp.Columns
                    Dim peer = (From sch In Schema Where sch("ColumnName").ToString.ToLower = col.ColumnName.ToLower).First
                    Dim value = row(col)
                    Dim typeC As TypeCode = [Enum].Parse(GetType(System.TypeCode), peer("Type").ToString)

                    CType(obj, IDictionary(Of String, Object)).Add(col.ColumnName, Convert.ChangeType(value, typeC))
                Next

            End If

            Dim tmp As New T
            If Not IsGenericMaster Then
                Dim tnp = DirectCast(obj, IDictionary(Of String, Object))
                For Each prop In tnp
                    If prop.Value Is Nothing Then
                        tmp.GetType.GetProperty(prop.Key).SetValue(tmp, Nothing)
                    Else
                        tmp.GetType.GetProperty(prop.Key).SetValue(tmp, prop.Value)
                    End If
                Next
            Else
                For Each prop In tmp.GetType.GetProperties
                    tmp.GetType.GetProperty(prop.Name).SetValue(tmp, CType(obj, IDictionary(Of String, Object))(prop.Name))
                Next
            End If

            list.Add(tmp)

        Next

        Return list
    End Function

    <Extension>
    Public Function ToBytes(str As String) As Byte()
        'Dim bytes(str.Length * Marshal.SizeOf(GetType(Char))) As Byte
        'System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length)
        'Return bytes
        Return Encoding.Unicode.GetBytes(str)
    End Function

    <Extension>
    Public Function ToStringValue(bytes As Byte()) As String
        'Dim chars(bytes.Length / Marshal.SizeOf(GetType(Char))) As Char
        'System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length)
        'Return New String(chars)
        Dim j = Convert.ToBase64String(bytes)
        Return Encoding.Unicode.GetString(bytes)
    End Function
End Module
