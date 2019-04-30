

using System.ComponentModel;

namespace Shoniz.Common.UserManagement.Model
{
    public class UserModel
    {
        [DisplayName("کد کاربری")]
        public int UserId { get; set; }
        [DisplayName("کد پرسنلی")]
        public int EmployeeId { get; set; }
        [DisplayName("کد قسمت")]
        public int PartId { get; set; }
        [DisplayName("نام قسمت")]
        public string PartName { get; set; }
        [DisplayName("نام کاربر")]
        public string FullName { get; set; }
        [DisplayName("کد ملی")]
        public string NationalCode { get; set; }
        [DisplayName("جنسیت")]
        public GenderEnum Gender { get; set; }
        [DisplayName("دسترسی ها")]
        public string RoleList { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SystemUserName { get; set; }
        public string ComputerName { get; set; }
    }
}
