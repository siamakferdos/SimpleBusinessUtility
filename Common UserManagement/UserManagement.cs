//using Shoniz.Common.Web.MVC.Grid;
//using System.ComponentModel;

//namespace Shoniz.Labroatory.Models.ViewModels
//{
//    public class UserVm
//    {
//        [DisplayName("شماره پرسنلی")]
//        public int UserId { get; set; }
//        [DisplayName("نام کاربر")]
//        public string FullName { get; set; }
//        [DisplayName("جنسیت")]
//        [GridCustom(Hidden = false)]
//        public string Gender { get; set; }

//        [DisplayName("دسترسی ها")]
//        public string RoleName { get; set; }

//    }

//    public class RoleVm
//    {
//        [GridCustom(Hidden = false)]
//        public int RoleId { get; set; }
//        [DisplayName("دسترسی")]
//        public string Name { get; set; }
//        public bool IsUpdate { get; set; }
//        public bool IsDelete { get; set; }
//        public bool IsRead { get; set; }
//        public bool IsCreate { get; set; }
//    }

//    public class MenuVm
//    {
//        public int UserId { get; set; }
//        public int MenuId { get; set; }
//        public string MenuName { get; set; }
//        public short? ParentMenuId { get; set; }
//        public decimal MenuOrder { get; set; }
//        public string Url { get; set; }
//        public string PageName { get; set; }
//        public string MenuIcon { get; set; }
//        public decimal Order { get; set; }
//        public int ParentId { get; set; }
//    }

//}
