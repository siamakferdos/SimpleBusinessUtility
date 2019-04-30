using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.ShonizIdentity.Model;

namespace Shoniz.Common.ShonizIdentity
{
    public class Branch : ShonizIdentitySetting
    {
        public Branch(string connectionString, int programId) : base(connectionString, programId)
        {
        }

        public List<BranchModel> GetBranches()
        {
            var db = new StoreProcdureManagement();
            return db.RunSp<BranchModel>(ConnectionString, "sp_Branches");
        }
    }
}
