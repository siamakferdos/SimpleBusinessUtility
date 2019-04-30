using System;
using System.Collections.Generic;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.UserManagement.Model;
using Shoniz.Common.Data.DataConvertor;

namespace Shoniz.Common.UserManagement
{
    public class Element : UmSetting
    {
        public Element(string connectionString, int programId)
            : base(connectionString, programId)
        {

        }
        public List<ProgramsModel> GetPrograms()
        {
            var db = new TableBasedSp(RunSpName);
            
            try
            {
                var dt = db.GetFirstTableOfData("uspGetPrograms", ConnectionString);
                var programs = new DataTableToList().Convert<ProgramsModel>(dt);
                return programs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
