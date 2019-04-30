
using System.Collections.Generic;
using System.Data;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.UserManagement.Model;


namespace Shoniz.Common.UserManagement
{
    public class Menu: UmSetting
    {
        public Menu(string connectionString, int programId)
            : base(connectionString, programId)
        {
            
        }

         public DataTable GetMenu(int userId)
        {
            var db = new TableBasedSp(RunSpName);
             var parameters = new Dictionary<string, string>
             {
                 {"ProgramId", ProgramId.ToString()},
                 {"UserId", userId.ToString()}
             };
             return db.GetFirstTableOfData("uspGetUserMenu", ConnectionString, parameters);
        }

         public List<WebMenusModel> GetWebMenu(int userId)
         {
             return new ElementAccess<WebMenusModel>(ConnectionString, ProgramId).GetFullElementWithSetting(
                     ElementTypeEnum.Menu, userId);
         }

         public static string GetPhoto(int userId)
         {
             var storeProcdureManagement = new StoreProcdureManagement();
             storeProcdureManagement.AddParameter("@UserId", userId);

             return storeProcdureManagement.RunSp(ConnectionNameEnum.LaboratoryConnectionString,
                 "sp_GetBase64Photo").ToString();
         }
    }
}
