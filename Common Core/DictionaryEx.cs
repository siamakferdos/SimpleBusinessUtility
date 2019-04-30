using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoniz.Common.Core
{
    //class DictionaryEx<TKey, TValue, TData>
    //{


//        private Dictionary<TKey, Dictionary<TValue, TData>> _pDictionary =
//            new Dictionary<TKey, Dictionary<TValue, TData>>();

//    public Item( TKey key) 
//    {
//        get { return _pDictionary.Item(key)
//        End Get
//        Set(ByVal value As ValueDataPair(Of TValue, TData))
//            p_dictionary.Item(key) = value
//        End Set
//    End Property

//    Public ReadOnly Property Keys() As TKey()
//        Get
//            Return p_dictionary.Keys.ToArray()
//        End Get
//    End Property

//    Public Sub New()
//        p_dictionary = New Dictionary(Of TKey, ValueDataPair(Of TValue, TData))
//    End Sub

//    Public Sub Add(ByVal key As TKey, ByVal value As TValue, ByVal data As TData)
//        p_dictionary.Add(key, New ValueDataPair(Of TValue, TData)(value, data))
//    End Sub

//    Public Sub Clear()
//        p_dictionary.Clear()
//    End Sub

//    Public Function ContainsKey(ByVal key As TKey) As Boolean
//        Return p_dictionary.ContainsKey(key)
//    End Function

//    Public ReadOnly Property Count() As Integer
//        Get
//            Return p_dictionary.Count
//        End Get
//    End Property

//    Public Function Remove(ByVal key As TKey) As Boolean
//        Return p_dictionary.Remove(key)
//    End Function

//    Public Function TryGetValue(ByVal key As TKey, ByRef value As TValue, ByVal data As TData) As Boolean
//        Try
//            Dim vdp = Item(key)

//            value = vdp.Value
//            data = vdp.Data

//            Return True

//        Catch ex As Exception
//            Return False
//        End Try
//    End Function

//End Class

//    }

//    public struct ValueDataPair<TValue, TData>
//    {
//        private readonly TValue _pValue;
//        private readonly TData _pData;
	
//        public ValueDataPair(TValue value, TData data)
//        {
//            _pValue = value;
//            _pData = data;
//        }
	
//        public TValue Value
//        {
//            get
//            {
//                return _pValue;
//            }
//        }
	
//        public TData Data
//        {
//            get
//            {
//                return _pData;
//            }
//        }
	
//    }
}
