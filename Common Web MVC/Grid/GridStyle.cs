using System.Collections.Generic;
using System.Linq;

namespace Shoniz.Common.Web.MVC.Grid
{
    public class GridStyle
    {
        //property of row that has special value give to row a special style
        public readonly Dictionary<KeyValuePair<string, string>, string> CustomPropertyRowStyle
            = new Dictionary<KeyValuePair<string, string>, string>();

        public readonly Dictionary<KeyValuePair<string, string>, string> CustomPropertyCellStyle
            = new Dictionary<KeyValuePair<string, string>, string>();

        private readonly string _gridName;

        public GridStyle(string gridName)
        {
            _gridName = gridName;
        }

        public readonly Dictionary<string, string> GridStyles =
            new Dictionary<string, string>();

        public readonly Dictionary<string, string> ColumnsStyle =
            new Dictionary<string, string>();

        /// <summary>
        /// Add aditional class names separate by space
        /// </summary>
        /// <example>
        /// class1 class2 class3
        /// </example>
        public string TableAdditionalClasses { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shonizGridClass">Chose yor class from GridModel.GridClass Enum</param>
        /// <param name="cssStyle">Enter full css attribute and values in a string format. Something like this :
        /// "color:red; font-size:10px;"
        /// </param>
        public void SetGridStyle(GridClasses shonizGridClass, string cssStyle)
        {
            if (GridStyles.ContainsKey(shonizGridClass.ToString()))
            {
                GridStyles[shonizGridClass.ToString()] = cssStyle;
            }
            else
            {
                GridStyles.Add(shonizGridClass.ToString(), cssStyle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Return: The CSS style of getted class in CSS originall format.</returns>
        internal string GetGridStyle(GridClasses shonizGridClass)
        {
            if (GridStyles.ContainsKey(shonizGridClass.ToString()))
            {
                return " GridContainer" + _gridName + " ." + shonizGridClass.ToString() + "{" + GridStyles[shonizGridClass.ToString()] + "}";
            }
            return "";
        }

        /// <summary>GetColumnStyle
        /// 
        /// </summary>
        /// <returns>Return: The CSS style of all classes of grid in CSS originall format.</returns>
        internal string GetGridStyle()
        {
            var css = new System.Text.StringBuilder();
            foreach (var style in GridStyles)
                if (style.Key == GridClasses.AlternativeRow.ToString())
                    css.Append(" ." + _gridName + style.Key + " tr:nth-child(even){" + style.Value + "} ");
                else if (style.Key == GridClasses.Pager.ToString())
                    css.Append(" #GridContainer" + _gridName + " ." + _gridName + style.Key + " span{" + style.Value + "} ");
                else
                    css.Append(" #GridContainer" + _gridName + " ." + _gridName + style.Key + "{" + style.Value + "} ");
            css.Append(GetColumnStyle());
            return css.ToString();
        }

        public void SetColumnsStyle(string columnName, string cssStyle)
        {
            if (ColumnsStyle.ContainsKey(columnName))
                ColumnsStyle[columnName] = cssStyle;
            else
                ColumnsStyle.Add(columnName, cssStyle);
        }

        internal string GetColumnStyle()
        {
            var css = new System.Text.StringBuilder();
            foreach (KeyValuePair<string, string> d in ColumnsStyle)
                css.Append(" #GridContainer" + _gridName + " table tbody tr " + " ." + _gridName + d.Key + "{" + d.Value + "} ");
            return css.ToString();
        }

        internal List<string> GetStyledColumns()
        {
            return ColumnsStyle.Select(d => d.Key).ToList();
        }

        public void SetRowCustomStyleByColumnValue(string columnName, string value, string style)
        {
            CustomPropertyRowStyle.Add(new KeyValuePair<string, string>(columnName, value.ToLower()), style);
        }

        public void SetCellCustomStyleByColumnValue(string columnName, string value, string style)
        {
            CustomPropertyCellStyle.Add(new KeyValuePair<string, string>(columnName, value.ToLower()), style);
        }

        internal string GetRowCustomStyleByColumnValue(string columnName, string value)
        {
            return CustomPropertyRowStyle.ContainsKey(new KeyValuePair<string, string>(columnName, value.ToLower())) 
                ? CustomPropertyRowStyle[new KeyValuePair<string, string>(columnName, value.ToLower())] 
                : "";
        }
    }
}
