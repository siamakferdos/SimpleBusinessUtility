''' <summary>
''' Provides a way to deal with ProcedureCaller.
''' </summary>
''' <remarks></remarks>
Public Interface IDataObserver
    ''' <summary>
    ''' Runs an StoredProcedure in SQL Server using ADO.Net and calls OnDataRecieve when request completed
    ''' </summary>
    ''' <param name="Catalog"></param>
    ''' <param name="SpName"></param>
    ''' <param name="OnDataRecieve"></param>
    ''' <param name="Parameters"></param>
    ''' <remarks></remarks>
    Sub RunSp(Catalog As String, SpName As String, OnDataRecieve As Action(Of DataSet), ParamArray Parameters As Parameter())

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Catalog"></param>
    ''' <param name="SpName"></param>
    ''' <param name="Parameters"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function RunSp(Catalog As String, SpName As String, ParamArray Parameters As Parameter()) As DataSet

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Catalog"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function ResolveCatalog(Catalog As String) As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property CurrentDate As Date

End Interface
