using System;
using System.Collections.Generic;
using Shoniz.Common.Data.DataConvertor;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.ShonizIdentity.Model;

namespace Shoniz.Common.ShonizIdentity
{
    public class Role : ShonizIdentitySetting
    {
        public Role(string connectionString, int programId)
            : base(connectionString, programId)
        {

        }
        public List<RoleModel> GetProgramRoles(int programId)
        {
            var db = new StoreProcdureManagement();

            db.AddParameter("@ProgramId", ProgramId.ToString());
            var roles = db.RunSp<RoleModel>(ConnectionString, "sp_ProgramRoles");
            return roles;
        }
    }
}
