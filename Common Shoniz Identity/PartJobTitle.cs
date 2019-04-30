using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.ShonizIdentity.Model;

namespace Shoniz.Common.ShonizIdentity
{
    public class PartJobTitle : ShonizIdentitySetting
    {
        public PartJobTitle(string connectionString, int programId)
            : base(connectionString, programId)
        {

        }
        public List<UserJobTitleModel> GetPartJobTitleUsers(int partId, JobTitleEnum jobTitle)
        {
            var db = new StoreProcdureManagement();
            db.AddParameter("@JobTitleId", (int)jobTitle);
            db.AddParameter("@PartId", partId);
            return db.RunSp<UserJobTitleModel>(ConnectionString, "sp_PartJobTitleUser");
        }

        public List<UserBasicInfoModel> GetUserJobTitles(int userId, int programId)
        {
            var db = new StoreProcdureManagement();
            db.AddParameter("@ProgramId", programId);
            db.AddParameter("@UserId", userId);
            return db.RunSp<UserBasicInfoModel>(ConnectionString, "sp_UserJobTitles");
        }
    }
}
