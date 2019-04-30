

namespace Shoniz.Common.ShonizIdentity.Model
{
    public class PartModel
    {
        public int PartId { get; set; }
        public int PartTypeId { get; set; }
        public string PartName { get; set; }
        public int MainPartId { get; set; }
    }

    public class PartTypeModel
    {
        public int PartTypeId { get; set; }
        public string PartTypeName { get; set; }
    }

}
