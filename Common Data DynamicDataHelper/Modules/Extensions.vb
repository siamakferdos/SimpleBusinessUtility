Imports System.Runtime.CompilerServices
Imports System.Dynamic

Module Extensions
    <Extension>
    Public Function ToStandardString(inp As Date) As String
        Return inp.ToString("yyyy-MM-dd HH:mm:ss.fff", New Globalization.CultureInfo("en"))
    End Function


End Module

Public Module DataObserverRunSpExtensions
    <Extension>
    Public Function CallRunSP(DataObsever As DataObserver, Catalog As String, SpName As String, ParamArray Parameters() As KeyValuePair(Of String, Object)) As DataSet
        Return DataObsever.CallRunSP(Catalog, SpName, Parameters)
    End Function

    <Extension>
    Public Function CallRunSP(DataObsever As DataObserver, Catalog As String, CallerSpName As String, SpName As String, ParamArray Parameters() As KeyValuePair(Of String, Object)) As DataSet
        Return DataObsever.CallRunSP(Catalog, CallerSpName, SpName, Parameters)
    End Function

    <Extension>
    Public Sub CallRunSP(DataObsever As DataObserver, Catalog As String, SpName As String, OnDataRecieve As Action(Of DataSet), ParamArray Parameters() As KeyValuePair(Of String, Object))
        DataObsever.CallRunSP(Catalog, SpName, Parameters)
    End Sub

    <Extension>
    Public Sub CallRunSP(DataObsever As DataObserver, Catalog As String, SpName As String, CallerSpName As String, OnDataRecieve As Action(Of DataSet), ParamArray Parameters() As KeyValuePair(Of String, Object))
        DataObsever.CallRunSP(Catalog, SpName, CallerSpName, OnDataRecieve, Parameters)

    End Sub

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

            Dim tmp As New T
            If Not IsGenericMaster Then
                Dim tnp = DirectCast(obj, IDictionary(Of String, Object))
                For Each prop In tnp
                    tmp.GetType.GetProperty(prop.Key).SetValue(tmp, prop.Value)
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
    Public Function Table(DataSet As DataSet, Name As String) As DataTable
        Dim result = (From obj As DataTable In DataSet.Tables
                      Where obj.TableName.ToLower = Name.ToLower)

        If result.Any Then
            Return result.First
        Else
            Return Nothing
        End If
    End Function
End Module
