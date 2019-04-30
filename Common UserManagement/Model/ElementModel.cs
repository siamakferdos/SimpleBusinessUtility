namespace Shoniz.Common.UserManagement.Model
{
    public class ElementsModel
    {
        public byte? ElementTypeId { get; set; }
        public int ElementId { get; set; }
        public int? ParentId { get; set; }
    }

    public class ColumnNamesModel : ElementsModel
    {
        public int? ProgramId { get; set; }
        public int ColumnId { get; set; }
        public string FarsiName { get; set; }
        public string EnglishName { get; set; }
    }

    public class MenusModel : ElementsModel
    {
        public int? ProgramId { get; set; }
        public int MenuId { get; set; }
        public string Name { get; set; }
        public decimal? Order { get; set; }
        public byte[] Icon { get; set; }
    }

    public class WebMenusModel : MenusModel
    {
        public string MenuIcon { get; set; }
        public string PageName { get; set; }
        public string Url { get; set; }
        public string ClassName { get; set; }
        public string Attr { get; set; }
        
    }

    public class FormsModel : ElementsModel
    {
        public int? ProgramId { get; set; }
        public int FormId { get; set; }
        public string FarsiName { get; set; }
        public string EnglishName { get; set; }
    }

    public class ControlsModel : ElementsModel
    {
        public int? ProgramId { get; set; }
        public int ControlId { get; set; }
        public string FarsiName { get; set; }
        public string EnglishName { get; set; }
    }

    public class DataBasesModel : ElementsModel
    {
        public int? ProgramId { get; set; }
        public int DatabaseId { get; set; }
        public string ServerName { get; set; }
        public string Name { get; set; }
        public bool? IsRemote { get; set; }
        public bool? AuthenticateMode { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool? Status { get; set; }
    }

    public class ImagesModel : ElementsModel
    {
        public int? ProgramId { get; set; }
        public int ImageId { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }
    }

    public class PluginsModel : ElementsModel
    {
        public int? ProgramId { get; set; }
        public int PluginId { get; set; }
        public string ClassName { get; set; }
        public string EnglishName { get; set; }
        public string FarsiName { get; set; }
    }

    public class ProgramsModel : ElementsModel
    {
        public int ProgramId { get; set; }
        public string FarsiName { get; set; }
        public string EnglishName { get; set; }
        public string DefaultAccess { get; set; }
        public string CollisionDefault { get; set; }
        public string Desription { get; set; }
    }

    public class StoredProcedureModel : ElementsModel
    {
        public int? ProgramId { get; set; }
        public int StoredProcedureId { get; set; }
        public string Name { get; set; }
    }
}
