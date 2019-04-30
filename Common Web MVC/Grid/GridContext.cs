using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Shoniz.Common.Web.MVC.Grid
{
    public class GridContext<T> : GridContext
    {
        readonly StringBuilder _gridHtml = new StringBuilder();
        readonly GridModel<T> _model;
        private readonly List<string> _primaryKeyFields;
        private readonly List<string> _editableFields = new List<string>();
        private readonly Dictionary<string, GridCustomAttribute> _columnsAttr = new Dictionary<string, GridCustomAttribute>();
        private readonly Dictionary<string, string> _columnsName = new Dictionary<string, string>();
        readonly dynamic mainGridObject = new ExpandoObject();

        public GridContext(GridModel<T> model = null)
        {
            _model = model;
            _primaryKeyFields = new List<string>();
        }

        public new MvcHtmlString InitialGrid()
        {
            //CreateBegining();
            CreateTableHead();
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(mainGridObject);
            _gridHtml.AppendLine("<div id='" + _model.GridName + "'>خطا!!<script>makeGrid(" + json + ");</script></div>");
            return MvcHtmlString.Create(_gridHtml.ToString());

        }


        private void CreateTableHead()
        {
            var _props = typeof(T).GetProperties();
            ////header columns
            foreach (var p in _props)
            {
                
                if (!_columnsAttr.ContainsKey(p.Name) ||
                    (_columnsAttr[p.Name] != null && !_columnsAttr[p.Name].Excluded))
                {
                    _columnsAttr.Add(p.Name, p.GetGridCustomAttribute());
                    if(p.DisplayName() != null && p.DisplayName() != "")            

                        _columnsName.Add(p.Name, p.DisplayName());

                    if (_columnsAttr.ContainsKey(p.Name) && (_columnsAttr[p.Name] != null))
                    {
                        var attr = _columnsAttr[p.Name];
                        if (attr.PrimaryKey)
                        {
                            _primaryKeyFields.Add(p.Name);
                        }

                        if (attr.CanEdit)
                        {
                            _editableFields.Add(p.Name);
                        }
                    }
                }

                mainGridObject.Attributes = _columnsAttr;
                mainGridObject.GridModel = _model;
                mainGridObject.PrimaryKeyList = _primaryKeyFields;
                mainGridObject.EditableColumnList = _editableFields;
                mainGridObject.ColumnNames = _columnsName;
            }
        }
    }

    public class GridContext
    {
        private object _model;
        public GridContext(GridModel model)
        {
            //Type elementType = Assembly.Load("Shoniz.Common.Web.MVC").GetTypes().First(t => t.Name == "GridContext");
            Type listType = typeof(GridContext<>).MakeGenericType(Assembly.Load(model.AssemblyName).GetTypes().First(t => t.Name == model.TypeName));

            _model = Activator.CreateInstance(listType,
               model.Model
             );
        }


        protected GridContext()
        {

        }

        public MvcHtmlString InitialGrid()
        {
            Type magicType = _model.GetType();
            MethodInfo magicMethod = magicType.GetMethod("InitialGrid");
            return (MvcHtmlString)magicMethod.Invoke(_model, new object[] { });
        }
    }
}

