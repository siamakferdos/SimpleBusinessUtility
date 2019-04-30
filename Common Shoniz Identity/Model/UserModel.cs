

using System.ComponentModel;

namespace Shoniz.Common.ShonizIdentity.Model
{
    public class UserModel : UserBasicInfoModel
    {
        [DisplayName("ایمیل")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        [DisplayName("کلمه عبور")]
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        [DisplayName("تلفن")]
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        [DisplayName("نام کاربری")]
        public string UserName { get; set; }
    }

    public class UserBasicInfoModel
    {
        [DisplayName("کد کاربر")]
        public int UserId { get; set; }
        [DisplayName("نام")]
        public string FirstName { get; set; }
        [DisplayName("نام خانوادگی")]
        public string LastName { get; set; }
    }
}
