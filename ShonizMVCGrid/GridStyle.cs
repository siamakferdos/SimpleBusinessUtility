using System.Collections.Generic;
using System.Linq;

namespace Shoniz.MVCGrid
{
    public class GridStyle
    {
        private string _gridName;

        public GridStyle(string gridName)
        {
            _gridName = gridName;
        }

        private readonly Dictionary<string, string> _styles =
            new Dictionary<string, string>();

        private readonly Dictionary<string, string> _columns =
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
        public void SetGridStyle(ShonizGridClasses shonizGridClass, string cssStyle)
        {
            if (_styles.ContainsKey(shonizGridClass.ToString()))
            {
                _styles[shonizGridClass.ToString()] = cssStyle;
            }
            else
            {
                _styles.Add(shonizGridClass.ToString(), cssStyle);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Return: The CSS style of getted class in CSS originall format.</returns>
        public string GetGridStyle(ShonizGridClasses shonizGridClass)
        {
            if (_styles.ContainsKey(shonizGridClass.ToString()))
            {
                return " GridContainer" + _gridName + " ." + shonizGridClass.ToString() + "{" + _styles[shonizGridClass.ToString()] + "}";
            }
            else
                return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Return: The CSS style of all classes of grid in CSS originall format.</returns>
        public string GetGridStyle()
        {
            var css = new System.Text.StringBuilder();
            foreach (KeyValuePair<string, string> style in _styles)
                if (style.Key == ShonizGridClasses.AlternativeRow.ToString())
                    css.Append(" ." + _gridName + style.Key + " tr:nth-child(even){" + style.Value + "} ");
                else if (style.Key == ShonizGridClasses.Pager.ToString())
                    css.Append(" #GridContainer" + _gridName + " ." + _gridName + style.Key + " span{" + style.Value + "} ");
                else
                    css.Append(" #GridContainer" + _gridName + " ." + _gridName + style.Key + "{" + style.Value + "} ");
            css.Append(GetColumnStyle());
            return css.ToString();
        }

        public void SetColumnsStyle(string columnName, string cssStyle)
        {
            if (_columns.ContainsKey(columnName))
                _columns[columnName] = cssStyle;
            else
                _columns.Add(columnName, cssStyle);
        }

        public string GetColumnStyle()
        {
            var css = new System.Text.StringBuilder();
            foreach (KeyValuePair<string, string> d in _columns)
                css.Append(" #GridContainer" + _gridName + " table tbody tr " + " ." + _gridName + d.Key + "{" + d.Value + "} ");
            return css.ToString();
        }

        public List<string> GetStyledColumns()
        {
            return _columns.Select(d => d.Key).ToList();
        }
    }
}
