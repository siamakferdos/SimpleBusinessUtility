using System;

namespace Shoniz.Common.Web.MVC.Grid
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class GridCustomAttribute : Attribute
    {
        public bool PrimaryKey { get; set; }
        public bool CanEdit { get; set; }
        public bool Hidden { get; set; }
        public bool Excluded { get; set; }
        public bool RequireOnEdit { get; set; }
        public bool Link { get; set; }
    }
}