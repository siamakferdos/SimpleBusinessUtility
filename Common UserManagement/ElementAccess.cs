using System;
using System.Collections.Generic;
using System.Data;
using Shoniz.Common.Core;
using Shoniz.Common.Data.DataConvertor;
using Shoniz.Common.Data.SqlServer;
using Shoniz.Common.UserManagement.Model;

namespace Shoniz.Common.UserManagement
{
    public class ElementAccess : UmSetting
    {
        public ElementAccess(string connectionString, int programId)
            : base(connectionString, programId)
        {
            
        }

        public ElementAccessModel GetElementAccess(int elementId, int userId)
        {
            var db = new TableBasedSp(RunSpName);
            var parameters = new Dictionary<string, string>
            {
                {"ProgramId", ProgramId.ToString()},
                {"ElementId", elementId.ToString()},
                {"UserId", userId.ToString()}
            };
            try
            {
                var dt = db.GetFirstTableOfData("[uspGetUserAccessForElement]", ConnectionString, parameters);
                var elements = new DataTableToList().Convert<ElementAccessModel>(dt);
                return elements[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ElementAccessModel> GetElementAccess(ElementTypeEnum elementType, int userId)
        {
            var db = new TableBasedSp(RunSpName);
            var parameters = new Dictionary<string, string>
            {
                {"ProgramId", ProgramId.ToString()},
                {"ElementTypeId", ((int)elementType).ToStringVar()},
                {"UserId", userId.ToString()}
            };
            try
            {
                var dt = db.GetFirstTableOfData("uspVisibleElementDataForUser", ConnectionString, parameters);
                var elements = new DataTableToList().Convert<ElementAccessModel>(dt);
                return elements;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public DataTable GetFullElementWithSetting(ElementTypeEnum elementType, int userId)
        {
            var db = new TableBasedSp(RunSpName);
            var parameters = new Dictionary<string, string>
            {
                {"ProgramId", ProgramId.ToString()},
                {"ElementTypeId", ((int)elementType).ToStringVar()},
                {"UserId", userId.ToString()}
            };
            try
            {
                return db.GetFirstTableOfData("uspVisibleElementDataForUser", ConnectionString, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<ColumnNamesModel> GetColumnNames(int userId)
        {
            return new ElementAccess<ColumnNamesModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.Column,
                userId);
        }
        public List<ControlsModel> GetControls(int userId)
        {
            return new ElementAccess<ControlsModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.Control,
                userId);
        }
        public List<DataBasesModel> GetDatabase(int userId)
        {
            return new ElementAccess<DataBasesModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.Database,
                userId);
        }
        public List<FormsModel> GetForms(int userId)
        {
            return new ElementAccess<FormsModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.Form,
                userId);
        }
        public List<ImagesModel> GetImages(int userId)
        {
            return new ElementAccess<ImagesModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.Image,
                userId);
        }
        public List<MenusModel> GetMenus(int userId)
        {
            return new ElementAccess<MenusModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.Menu,
                userId);
        }
        public List<PluginsModel> GetPlugings(int userId)
        {
            return new ElementAccess<PluginsModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.Plugin,
                userId);
        }
        public List<ProgramsModel> GetPrograms(int userId)
        {
            return new ElementAccess<ProgramsModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.Program,
                userId);
        }
        public List<ProgramsModel> GetStoredProcdures(int userId)
        {
            return new ElementAccess<ProgramsModel>(ConnectionString, ProgramId).GetFullElementWithSetting(ElementTypeEnum.StoredProcedre,
                userId);
        }
    }

    public class ElementAccess<T> : UmSetting
    {
        public ElementAccess(string connectionString, int programId)
            : base(connectionString, programId)
        {

        }

        public List<T> GetFullElementWithSetting(ElementTypeEnum elementType, int userId)
        {
            var db = new TableBasedSp(RunSpName);
            var parameters = new Dictionary<string, string>
            {
                {"ProgramId", ProgramId.ToString()},
                {"ElementTypeId", ((int)elementType).ToStringVar()},
                {"UserId", userId.ToString()}
            };
            try
            {
                var dt = db.GetFirstTableOfData("uspVisibleElementDataForUser", ConnectionString, parameters);
                var elements = new DataTableToList().Convert<T>(dt);
                return elements;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
