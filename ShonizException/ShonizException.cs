using System;
using System.Collections.Generic;


namespace Shoniz.Exception
{
    public abstract class ShonizException : System.Exception
    {
        private readonly Dictionary<int, string> _errors = new Dictionary<int, string>();

        public virtual Dictionary<int, string> GetErrors()
        {
            return _errors;
        }

        public virtual string GetMessage(int code)
        {
            return _errors.ContainsKey(code) ? _errors[code] : "";
        }

        public virtual void Add(int code, string message)
        {
            _errors.Add(code, message);
        }
    }
}
