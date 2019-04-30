using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Shoniz.Common.Web
{
    public class JsonForAjax : JsonResult
    {
        private readonly Dictionary<string, string> _mainDic = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _htmlDic = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _dataDic = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _funcDic = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _messageDic = new Dictionary<string, string>();
        public JsonForAjax()
        {
            Data = "";
        }

        public JsonForAjax AddJsonHtmlElement(string targetKey, string content)
        {
            _htmlDic.Add(targetKey, content);

            var html = MakeData(_htmlDic);
            if (_mainDic.ContainsKey("html"))
                _mainDic["html"] = html;
            else
                _mainDic.Add("html", html);

            Data = MakeData(_mainDic);
            return this;
        }

        public JsonForAjax AddJsonDataElement(string key, string value)
        {
            _dataDic.Add(key, value);

            var data = MakeData(_dataDic);
            if (_mainDic.ContainsKey("jsonData"))
                _mainDic["jsonData"] = data;
            else
                _mainDic.Add("jsonData", data);

            Data = MakeData(_mainDic);
            return this;
        }

        public JsonForAjax AddJsonFunction(string func)
        {
            _funcDic.Add("func_" + new Random().Next(100000), func);

            var newfunc = MakeData(_htmlDic);
            if (_mainDic.ContainsKey("func"))
                _mainDic["func"] = newfunc;
            else
                _mainDic.Add("func", newfunc);

            Data = MakeData(_mainDic);
            return this;
        }

        public JsonForAjax AddJsonMessage(string messageKey, string messageValue)
        {
            _messageDic.Add(messageKey, messageValue);
            Data = MakeData(_messageDic);
            return this;
        }

        public JsonForAjax AddJsonErrorElement(string message)
        {
            if (_mainDic.ContainsKey("error"))
                throw new Exception("This element exist in Json Data!");

            _mainDic.Add("error", message);
            Data = MakeData(_mainDic);
            return this;
        }

        public JsonForAjax AddJsonJsElement(string javaScriptStatement)
        {
            if (_mainDic.ContainsKey("js"))
                throw new Exception("This element exist in Json Data!");

            _mainDic.Add("js", javaScriptStatement);
            Data = MakeData(_mainDic);
            return this;
        }

        private string MakeData(Dictionary<string, string> keyValues)
        {
            var js = new JavaScriptSerializer();
            return js.Serialize(keyValues);
            //var d = Data.ToString();
            //if (d != "")
            //    d += ",";

            //d += str;
            //Data = d;
        }
    }
}
