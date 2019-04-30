using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.ShonizIdentity.Model;

namespace Shoniz.Common.ShonizIdentity
{
    public class Part : ShonizIdentitySetting
    {
        public Part(string connectionString, int programId)
            : base(connectionString, programId)
        {

        }
        public List<PartModel> GetParts(PartTypeEnum partTypeEnum)
        {
            var db = new StoreProcdureManagement();
            db.AddParameter("@PartTypeId", (int)partTypeEnum);
            return db.RunSp<PartModel>(ConnectionString, "sp_Parts");
        }

        public List<PartTypeModel> GetPartTypes()
        {
            var db = new StoreProcdureManagement();
            return db.RunSp<PartTypeModel>(ConnectionString, "sp_PartTypes");
        }
    }
}
