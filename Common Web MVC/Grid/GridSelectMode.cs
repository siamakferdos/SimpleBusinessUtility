using System.ComponentModel;

namespace Shoniz.Common.Web.MVC.Grid
{
    [ReadOnly(true)]
    public enum GridSelectMode
    {
        None = 1,
        Single = 2,
        Multiple = 3
    }
}