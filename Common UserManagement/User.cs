using System;
using System.Collections.Generic;
using System.Data;
using Shoniz.Common.Core.Exception;
using Shoniz.Common.Data.DataConvertor;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.UserManagement.Model;
using DataException = Shoniz.Common.Data.SqlServer.DataException;

namespace Shoniz.Common.UserManagement
{
    public class User : UmSetting
    {
        public User(string connectionString, int programId)
            : base(connectionString, programId)
        {
            
        }

        public UserModel Login(string userId, string password)
        {
            var db = new TableBasedSp(RunSpName);
            var parameters = new Dictionary<string, string>
            {
                {"ProgramId", ProgramId.ToString()},
                {"UserId", userId},
                {"Password", password}
            };
            var dt = db.GetFirstTableOfData("uspLoginInProgram", ConnectionString, parameters);
            if (dt == null || dt.Rows.Count < 1) return null;
            var user = new DataTableToList().Convert<UserModel>(dt);
            return user[0];
        }

        public List<UserModel> GetProgramUsers(UserStatusEnum userStatusEnum = UserStatusEnum.All)
        {
            var db = new TableBasedSp(RunSpName);
            var parameters = new Dictionary<string, string>
            {
                {"ProgramId", ProgramId.ToString()},
                {"Enable", ((int)userStatusEnum).ToString()}
            };
            var dt = db.GetFirstTableOfData("uspGetProgramUsers", ConnectionString, parameters);
            var users = new DataTableToList().Convert<UserModel>(dt);
            return users;
        }

        public List<UserModel> GetAllUsers()
        {
            var db = new TableBasedSp(RunSpName);
            var dt = db.GetFirstTableOfData("uspGetUsers", ConnectionString);
            var users = new DataTableToList().Convert<UserModel>(dt);
            return users;
        }
    
    }
}
