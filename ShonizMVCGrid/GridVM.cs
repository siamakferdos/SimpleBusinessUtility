using System.Collections.Generic;

namespace Shoniz.MVCGrid
{
    public class GridVm
    {
        public GridVm()
        {
            GridList = new List<GridModel>();
        }
        public List<GridModel> GridList { get; set; }
    }
}