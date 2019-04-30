using System.ComponentModel;

namespace Shoniz.Common.Web.MVC.Grid
{
    [ReadOnly(true)]
    public enum GridEditMode
    {
        None = 1,
        Single = 2,
        All = 3
    }
}