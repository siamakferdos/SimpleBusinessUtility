using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoniz.Common.Web.MVC.Grid
{
    public class GridColumnImage
    {
        private Dictionary<string, string> _columnValueImage = new Dictionary<string, string>();
       
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets or sets the column value image.
        /// </summary>
        /// <value>
        /// The column value image. The key is value of column and value of dictionary is the path of image for current value
        /// </value>
        public Dictionary<string, string> ColumnValueImage
        {
            get { return _columnValueImage; }
            set { _columnValueImage = value; }
        }
    }
}
