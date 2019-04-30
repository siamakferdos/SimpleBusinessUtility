using System;
using System.Collections.Generic;


namespace Shoniz.Common.Data.DataConvertor.Mapper
{
    /// <summary>
    /// This static class keeps a dictionary of convertor classes of various type to each other which method for them is created before. At any converting
    /// at first this dictionary is looked to if exist any same convertor.
    /// </summary>
    static class NewObjectPool
    {
        public static Dictionary<string, Type> Classpool = new Dictionary<string, Type>();
    }
}
