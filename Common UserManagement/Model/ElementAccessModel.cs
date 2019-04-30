
namespace Shoniz.Common.UserManagement.Model
{
    public class ElementAccessModel
    {
        public int RoleId { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsRead { get; set; }
        public bool IsCreate { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEnable { get; set; }
    }
}
