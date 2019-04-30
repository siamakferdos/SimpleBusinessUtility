using System.ComponentModel;

namespace Shoniz.Common.Web.MVC.Grid
{
    [ReadOnly(true)]
    public enum GridClasses
    {
        Table = 1,
        Header = 2,
        Body = 3,
        Footer = 4,
        AlternativeRow = 5,
        Pager = 6,
        SelectedRow = 7,
        MouseoverRow = 8
    }
}