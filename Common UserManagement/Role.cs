using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoniz.Common.Data.DataConvertor;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.UserManagement.Model;

namespace Shoniz.Common.UserManagement
{
    public class Role : UmSetting
    {
        public Role(string connectionString, int programId)
            : base(connectionString, programId)
        {
            
        }
        public List<ProgramRoleModel> GetProgramRoles(int programId)
        {
            var db = new TableBasedSp(RunSpName);
            var parameters = new Dictionary<string, string>
            {
                {"ProgramId", ProgramId.ToString()}
            };
            try
            {
                var dt = db.GetFirstTableOfData("uspGetProgramRoles", ConnectionString, parameters);
                var roles = new DataTableToList().Convert<ProgramRoleModel>(dt);
                return roles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        
        }
    }
}
