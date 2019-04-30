using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace Shoniz.Common.UserManagement
{
    public class UmSetting
    {
        public string ConnectionString { get; set; }
        public int ProgramId { get; set; }
        public string RunSpName { get; set; }

        public UmSetting(string connectionString, int programId)
        {
            ConnectionString = connectionString;
            RunSpName = "RunSp";
            ProgramId = programId;
        }


    }
}
