using System.Collections.Generic;
using System.Linq;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.ShonizIdentity.Model;

namespace Shoniz.Common.ShonizIdentity
{
    public class Program: ShonizIdentitySetting
    {
        public Program(string connectionString, int programId)
            : base(connectionString, programId)
        {

        }
        public List<ProgramModel> GetPrograms()
        {
            var db = new StoreProcdureManagement();
            var programs = db.RunSp<ProgramModel>(ConnectionString, "sp_Programs");
            return programs;
        }

        public ProgramModel GetProgram(int programId)
        {
            var db = new StoreProcdureManagement();

            db.AddParameter("@ProgramId", ProgramId.ToString());
            var program = db.RunSp<ProgramModel>(ConnectionString, "sp_Program");
            if(program.Any())
                return program[0];
            return new ProgramModel();
        }

    }
}
