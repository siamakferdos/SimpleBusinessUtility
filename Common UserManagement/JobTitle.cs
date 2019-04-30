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
    public class JobTitle : UmSetting
    {
         public JobTitle(string connectionString, int programId)
            : base(connectionString, programId)
        {
            
        }

        public List<JobTitleModel> GetUserJobTitles(int userId)
        {
            var db = new TableBasedSp(RunSpName);
            var parameters = new Dictionary<string, string>
            {
                {"ProgramId", ProgramId.ToString()},
                {"UserId", userId.ToString()}
            };
            try
            {
                var dt = db.GetFirstTableOfData("uspUserJobTitle", ConnectionString, parameters);
                var jobTitles = new DataTableToList().Convert<JobTitleModel>(dt);
                return jobTitles;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
