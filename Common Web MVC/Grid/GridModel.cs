#region Usings

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace Shoniz.Common.Web.MVC.Grid
{
    /// <summary>
    /// Create model for grid
    /// </summary>
    /// <typeparam name="T">Type of grid Datasource</typeparam>
    public class GridModel<T> : GridModel
    {
        #region Constructor
        /// <summary>
        /// T is for type of grid datasource
        /// </summary>
        /// <param name="gridDataSource">List of T</param>
        /// <param name="gridName"></param>
        /// <param name="pageRecordCount"></param>
        /// <param name="currentPageIndex"></param>
        /// <param name="controller"></param>
        /// <param name="actionName"></param>
        /// <param name="orderField"></param>
        public GridModel(List<T> gridDataSource, string gridName,
            int pageRecordCount, int currentPageIndex, string controller = "", string actionName = "", string orderField = "")
            : base()
        {
            GridDataSource = gridDataSource;
            if (!string.IsNullOrEmpty(orderField))
                try
                {
                    GridDataSource = gridDataSource.OrderBy(o => o.GetType().GetProperty(orderField).GetValue(o, null)).ToList();
                }
                catch { }

            GridDataSource = GridDataSource.ToPageX(pageRecordCount, currentPageIndex);
            GridName = gridName;
            RecordCount = gridDataSource.Count();
            PageRecordCount = pageRecordCount;
            ShowPageCounter = true;
            CurrentPageIndex = currentPageIndex;
            Controller = controller;
            ActionName = actionName;

            OrderField = orderField;

            RowCounterTitle = "ردیف";
            HasRowCounter = true;

            Styles = new GridStyle(GridName);
            ColumnsImage = new List<GridColumnImage>();
            ColumnDataSource = new Dictionary<string, Dictionary<string, object>>();

            DefaultGroupColumn = "";
        }

        public GridModel(List<T> gridDataSource, string gridName,
            int allRecordCount, int pageRecordCount, int currentPageIndex, string controller = "", string actionName = "")
            : base()
        {
            GridDataSource = gridDataSource;

            //GridDataSource = this.GridDataSource.ToPageX(pageRecordCount, currentPageIndex);
            GridName = gridName;
            RecordCount = allRecordCount;
            ShowPageCounter = true;
            PageRecordCount = pageRecordCount;
            CurrentPageIndex = currentPageIndex;
            Controller = controller;
            ActionName = actionName;
            OrderField = "";
            RowCounterTitle = "ردیف";
            HasRowCounter = true;
            Styles = new GridStyle(GridName);
            ColumnsImage = new List<GridColumnImage>();
            ColumnDataSource = new Dictionary<string, Dictionary<string, object>>();

            DefaultGroupColumn = "";
        }
        #endregion

        #region Public Fields & Properties

        public List<string> PrimaryFieldList = new List<string>();
        public List<string> HiddenFieldList = new List<string>();
        public List<string> ExcludedFieldList = new List<string>();
        public List<string> EditableFieldList = new List<string>();

        public readonly Dictionary<string, string> FieldDisplayNames = new Dictionary<string, string>();
        public void AddFieldDisplayName(string fieldName, string displayName)
        {
            FieldDisplayNames.Add(fieldName, displayName);
        }

        public bool CanGroup { get; set; }
        public string DefaultGroupColumn { get; set; }

        public List<CustomColumn> CustomColumns = new List<CustomColumn>();


        public string OnCellClick = "";
        public string OnCellDblClick = "";

        public string EditController = "";
        public string EditAction = "";
        public bool EditOnModal = false;

        public string DeleteController = "";
        public string DeleteAction = "";

        public string DeleteLinkCaption = "حذف";
        public string EditLinkCaption = "ویرایش";

        public string RowLinkText = "لینک";
        public List<string> RowLinkFields = new List<string>();

        public string ExtraParam = "";

        public GridSelectMode SelectingMode = GridSelectMode.None;
        public GridEditMode EditingMode = GridEditMode.None;

        public bool HasDetail = false;
        public string DetailHeaderName = "جزئیات";
        public string DetailColumnName = "مشاهده";
        public string DetailController = "";
        public string DetailAction = "";

        public bool CanInsert = false;

        public bool IsSortableGrid = true;
        public bool CheckableRow { get; set; }
        public Dictionary<int, string> ColumnOrdering = new Dictionary<int, string>();

        public List<string> SearchColumnList = new List<string>();
        public string SearchController = "";
        public string SearchAction = "";
        public bool SearchOnKeyPress = false;

        public bool AllowDelete = false;
        public bool HasRowCounter { get; set; }
        public string RowCounterTitle { get; set; }

        public string Filters { get; set; }
        public Dictionary<string, object> ConstantValue = new Dictionary<string, object>();
        public Dictionary<string, string> ColumnAttribute = new Dictionary<string, string>();

        public bool ClientOnlySort { get; set; }

        public List<T> GridDataSource { get; set; }
        public T GridObject = Activator.CreateInstance<T>();
        //ColumnDataSource : <column name, <option display name, value>>
        public readonly Dictionary<string, Dictionary<string, object>> ColumnDataSource =
            new Dictionary<string, Dictionary<string, object>>();
        public string GridName { get; set; }
        public int RecordCount { get; set; }
        public int PageRecordCount { get; set; }
        public bool ShowPageCounter { get; set; }
        public int CurrentPageIndex { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        private string _orderField;
        //public string TargetId { get; set; }
        public string OrderField
        {
            get { return _orderField; }
            set
            {
                if (value != "")
                    try
                    {
                        GridDataSource =
                            GridDataSource.OrderBy(o => o.GetType().GetProperty(value).GetValue(o, null)).ToList();
                    }
                    catch
                    {
                    }
                _orderField = value;
            }
        }
        public bool ActOnClient { get; set; }
        public string AjaxLoaderTargetSelector { get; set; }
        /// <summary>
        /// Gets or sets the columns image.
        /// </summary>
        /// <value>
        /// The columns image. Note: "AllValues" for key of dictionary, will show image to all values
        /// </value>
        public List<GridColumnImage> ColumnsImage { get; set; }

        /// <summary>
        /// Gets or sets the extra column attributes.
        /// </summary>
        /// <value>
        /// The extra column attributes. If You want your grid has an extra column, Just add an attribute
        /// that be in TD tag. Like this:
        /// ExtraColumnAttribute.Add("data-grid-RemoveColumn = 'remove'", "حذف سطر")
        /// It will be this tag on grid : <td data-grid-RemoveColumn='remove'>حذف سطر</td>
        /// </value> 
        public Dictionary<string, string> ExtraColumn { get; set; }

        #endregion

        #region public Method
        public void AddFilter(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(Filters))
            {
                Filters = key + ":" + value;
            }
            else
            {
                Filters += "|" + key + ":" + value;
            }

        }

        public void AddColumnDatasource(string columnName, Dictionary<string, object> nameValueDic)
        {
            if (ColumnDataSource.ContainsKey(columnName))
                ColumnDataSource[columnName] = nameValueDic;
            else
                ColumnDataSource.Add(columnName, nameValueDic);
        }

        #endregion

        public GridStyle Styles;
    }

    /// <summary>
    /// Gridmodel for Mvc grid
    /// </summary>
    /// <typeparam name="T">Datasource type of grid</typeparam>
    /// <typeparam name="TT">Type of tooltip class for grid columns. This type should be a class contains only String fiels to use as tooltip. 
    /// Additionally, each property which will used as tooltip, should be same as main datasource column name.</typeparam>
    public class GridModel<T, TT> : GridModel<T>
    {
        public List<TT> tooltipDataSource;
        public GridModel(List<T> gridDataSource, List<TT> tooltipDataSource, string gridName,
            int pageRecordCount, int currentPageIndex, string controller = "", string actionName = "", string orderField = "")
            : base(gridDataSource, gridName, pageRecordCount, currentPageIndex, controller, actionName, orderField)
        {
            this.tooltipDataSource = tooltipDataSource;
            this.tooltipDataSource = this.tooltipDataSource.ToPageX(pageRecordCount, currentPageIndex);
        }

        public GridModel(List<T> gridDataSource, List<TT> tooltipDataSource, string gridName,
            int allRecordCount, int pageRecordCount, int currentPageIndex, string controller = "", string actionName = "")
            : base(gridDataSource, gridName, allRecordCount, pageRecordCount, currentPageIndex, controller, actionName)
        {
            this.tooltipDataSource = tooltipDataSource;
            this.tooltipDataSource = this.tooltipDataSource.ToPageX(pageRecordCount, currentPageIndex);
        }
    }

    public class CustomColumn
    {
        public CustomColumn(string text, string controller, string action, bool isModalType)
        {
            Text = text;
            Controller = controller;
            Action = action;
            IsModalType = isModalType;
        }
        public string Text { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool IsModalType { get; set; }
    }


    public class GridModel
    {
        private dynamic _model;
        internal string AssemblyName;
        internal string TypeName;
        public GridModel(string assemblyName, string typeName, IList gridDataSource, string gridName,
            int pageRecordCount, int currentPageIndex, string controller = "", string actionName = "", string orderField = "")
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            //MethodInfo method = typeof(GridModel<>).GetMethods().First(m => m.Name == "RunSp" && m.GetGenericArguments().Length == 1);
            //var typeArgs = Assembly.Load(assemblyName).GetTypes().First(t => t.Name == typeName);
            ////Type[] typeArgs = { Type.GetType("Shoniz.Labroatory.Models." + typeName + ", Shoniz.Labroatory.Models") };
            //MethodInfo generic = method.MakeGenericMethod(typeArgs);
            //return (IList)generic.Invoke(this, new[] { (object)connectionNameEnum, (object)storeProcureName });

            Type elementType = Assembly.Load(assemblyName).GetTypes().First(t => t.Name == typeName);
            Type listType = typeof(GridModel<>).MakeGenericType(new Type[] { elementType });

            _model = Activator.CreateInstance(listType,
                (object)gridDataSource,
                (object)gridName,
                (object)pageRecordCount,
                (object)currentPageIndex,
                (object)controller,
                (object)actionName,
                (object)orderField
                );
        }

        public GridModel(string assemblyName, string typeName, IList gridDataSource, string gridName,
            int allRecordCount, int pageRecordCount, int currentPageIndex, string controller = "", string actionName = "")
        {
            AssemblyName = assemblyName;
            TypeName = typeName;
            //MethodInfo method = typeof(GridModel<>).GetMethods().First(m => m.Name == "RunSp" && m.GetGenericArguments().Length == 1);
            //var typeArgs = Assembly.Load(assemblyName).GetTypes().First(t => t.Name == typeName);
            ////Type[] typeArgs = { Type.GetType("Shoniz.Labroatory.Models." + typeName + ", Shoniz.Labroatory.Models") };
            //MethodInfo generic = method.MakeGenericMethod(typeArgs);
            //return (IList)generic.Invoke(this, new[] { (object)connectionNameEnum, (object)storeProcureName });

            Type elementType = Assembly.Load(assemblyName).GetTypes().First(t => t.GetProperties()[4] is int && t.Name == typeName);
            Type listType = typeof(GridModel<>).MakeGenericType(new Type[] { elementType });

            _model = (GridModel)Activator.CreateInstance(listType,
                (object)gridDataSource,
                (object)gridName,
                (object)allRecordCount,
                (object)pageRecordCount,
                (object)currentPageIndex,
                (object)controller,
                (object)actionName
                );
        }

        protected GridModel()
        {

        }

        public dynamic Model
        {
            get { return _model; }
            set { _model = value; }
        }

       
    }
}
