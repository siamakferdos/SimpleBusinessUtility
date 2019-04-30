using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.ShonizIdentity.Model;

namespace Shoniz.Common.ShonizIdentity
{
    public class User : ShonizIdentitySetting
    {
        public User(string connectionString, int programId)
            : base(connectionString, programId)
        {

        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
        }

        public UserModel Login(int userId, string password)
        {
            var db = new StoreProcdureManagement();
            db.AddParameter("@ProgramId", ProgramId);
            db.AddParameter("@UserId", userId);
            var user = db.RunSp<UserModel>(ConnectionString, "sp_User");
            if (user.Any())
            {
                if (VerifyHashedPassword(user[0].PasswordHash, password))
                    return user[0];
            }
            return null;
        }

        public List<RoleModel> GetUserRole(int userId)
        {
            var db = new StoreProcdureManagement();
            db.AddParameter("@UserId", userId);
            return db.RunSp<RoleModel>(ConnectionString, "sp_UserRoles");
        }

        public List<UserModel> GetAllUsers()
        {
            var db = new StoreProcdureManagement();
            var users = db.RunSp<UserModel>(ConnectionString, "sp_Users");
            if (users.Any())
                return users;
            return new List<UserModel>();
        }

        public List<PartModel> GetUserPart(int userId, int programId)
        {
            var db = new StoreProcdureManagement();
            db.AddParameter("@UserId", userId);
            db.AddParameter("@ProgramId", programId);
            return db.RunSp<PartModel>(ConnectionString, "sp_UserParts");
        }
        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="programId">The program identifier. If this parameter set as NULL value, then user will return without consider about his/her program.</param>
        /// <returns></returns>
        public UserModel GetUser(int userId, int? programId = null)
        {
            var db = new StoreProcdureManagement();
            db.AddParameter("@UserId", userId);
            db.AddParameter("@ProgramId", programId);
            var user = db.RunSp<UserModel>(ConnectionString, "sp_User");
            if (user.Any())
                return user[0];
            return new UserModel();
        }
    }
}
