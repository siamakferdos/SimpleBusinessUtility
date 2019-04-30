using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoniz.Common.UserManagement
{
    public enum ElementTypeEnum
    {
        Program = 1,
        SubProgram = 2,
        Plugin = 3,
        Database = 4,
        Image = 5,
        Form = 6,
        Control = 7,
        Menu = 8,
        Column = 9,
        StoredProcedre = 10
    }

    public enum UserStatusEnum
    {
        Disable = 0,
        Enable = 1,
        All = 2
    }

    public enum GenderEnum
    {
        Female,
        Male
    }
}
