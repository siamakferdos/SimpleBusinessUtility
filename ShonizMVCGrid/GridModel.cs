#region Usings
using System.Collections.Generic;
using System.Linq;
#endregion

namespace Shoniz.MVCGrid
{
    public class GridModel
    {
        #region Constructor
        public GridModel(List<object> gridDataSource, string gridName,
            int pageRecordCount = 10, int currentPageIndex = 1, string controller = "", string actionName = "", string orderField = "")
        {
            GridDataSource = gridDataSource;
            if (!string.IsNullOrEmpty(orderField))
                try
                {
                    this.GridDataSource = gridDataSource.OrderBy(o => o.GetType().GetProperty(orderField).GetValue(o, null)).ToList();
                }
                catch { }

            GridDataSource = this.GridDataSource.ToPageX(pageRecordCount, currentPageIndex);
            GridName = gridName;
            RecordCount = gridDataSource.Count();
            PageRecordCount = pageRecordCount;
            CurrentPageIndex = currentPageIndex;
            Controller = controller;
            ActionName = actionName;

            OrderField = orderField;

            RowCounterTitle = "ردیف";
            HasRowCounter = true;

             Styles = new GridStyle(GridName);
        }
        #endregion

        #region Public Fields & Properties
        public string EditController = "";
        public string EditAction = "";

        public string DeleteController = "";
        public string DeleteAction = "";

        public string DeleteLinkCaption = "حذف";
        public string EditLinkCaption = "ویرایش";

        public ShonizGridSelectMode SelectingMode = ShonizGridSelectMode.None;
        public ShonizGridEditMode EditingMode = ShonizGridEditMode.None;

        public bool AllowDelete = false;
        public bool HasRowCounter { get; set; }
        public string RowCounterTitle { get; set; }

        public string Filters { get; set; }
        
        public List<object> GridDataSource { get; set; }
        public string GridName { get; set; }
        public int RecordCount { get; set; }
        public int PageRecordCount { get; set; }
        public int CurrentPageIndex { get; set; }
        public string Controller { get; set; }
        public string ActionName { get; set; }
        public string TargetId { get; set; }
        public string OrderField { get; set; }
        public bool ActOnClient { get; set; }

        public List<string> HiddenFieldList = new List<string>();
        #endregion

        #region public Method
            public void AddFilter(string key, string value)
        {
            if (Filters == "")
            {
                Filters = key + ":" + value;
            }
            else
            {
                Filters += "|" + key + ":" + value;
            }

        }

        #endregion

        public GridStyle Styles;
    }
}
