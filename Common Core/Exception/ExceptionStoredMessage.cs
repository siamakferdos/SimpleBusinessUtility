using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoniz.Common.Core.Exception
{
    public static class ExceptionStoredMessage
    {
        public static string DataConverting
        {
            get { return "خطا در واکشی اطلاعات از جداول چندگانه"; }
        }
        public static string InvalidUserNameOrPassword
        {
            get { return "نام کاربری و/یا کلمه عبور اشتباه میباشد"; }
        }
        public static string UserNotFound
        {
            get { return "کاربری یافت نشد"; }
        }
        public static string NotAppliedShonizStandard
        {
            get { return "استاندارد های طراحی دیتابیس مدیریت کاربران شونیز رعایت نشده است"; }
        }
    }
}
