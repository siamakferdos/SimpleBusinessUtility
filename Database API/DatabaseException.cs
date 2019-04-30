using System;
using System.Collections.Generic;

namespace Shoniz.Database_API
{
    public class SqlSpException : SystemException
    {
        private readonly Dictionary<int, string> _errors = new Dictionary<int, string>();
        public void Add(int code, string message)
        {
            _errors.Add(code, message);
        }
        public SqlSpException() { }

        public Dictionary<int, string> GetErrors()
        {
            return _errors;
        }

        public string GetMessage(int code)
        {
            return _errors.ContainsKey(code) ? _errors[code] : "";
        }
    }
}