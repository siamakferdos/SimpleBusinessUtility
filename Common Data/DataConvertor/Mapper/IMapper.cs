namespace Shoniz.Common.Data.DataConvertor.Mapper
{
    /// <summary>
    /// This interface is used to create new instance of convertor with uniqe performance
    /// </summary>
    /// <typeparam name="TSource">The type of the ource.</typeparam>
    /// <typeparam name="TTarget"></typeparam>
    public interface IMapper<in TSource, out TTarget>
    {
        //This method will convert an object type to other one. 
        TTarget Convert(TSource s, object extraData = null);
    }
}
