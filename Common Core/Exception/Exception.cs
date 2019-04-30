using System.Collections.Generic;
using System.Data;

namespace Shoniz.Common.Core.Exception
{
    public abstract class Exception : System.Exception
    {
        public readonly Dictionary<int, string> ErrorDictionary = new Dictionary<int, string>();
        public DataTable ErrorTable = new DataTable();

        public virtual Dictionary<int, string> GetErrors()
        {
            return ErrorDictionary;
        }

        public virtual DataTable GetErrorTable()
        {
            return ErrorTable;
        }

        public virtual string GetMessage(int code)
        {
            return ErrorDictionary.ContainsKey(code) ? ErrorDictionary[code] : "";
        }

        public virtual void Add(int code, string message)
        {
            if(!ErrorDictionary.ContainsKey(code))
                ErrorDictionary.Add(code, message);
        }

        public virtual void Add(DataTable dt)
        {
            ErrorTable = dt;
        }
    }
}
