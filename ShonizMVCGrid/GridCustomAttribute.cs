using System;

namespace Shoniz.MVCGrid
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class GridCustomAttribute : Attribute
    {
        public bool PrimaryKey { get; set; }
        public bool CanEdit { get; set; }
        public bool Hidden { get; set; }
        public bool Excluded { get; set; }
    }
}