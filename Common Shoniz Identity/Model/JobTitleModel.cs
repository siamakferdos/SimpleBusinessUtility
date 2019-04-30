
using System.ComponentModel;

namespace Shoniz.Common.ShonizIdentity.Model
{
    public class JobTitleModel
    {
        public int JobTitleId { get; set; }
        public int ParentId { get; set; }
        public int PartId { get; set; }
        public string PartName { get; set; }
    }

    public class UserJobTitleModel : UserBasicInfoModel
    {
        public int ParentId { get; set; }
    }
}
