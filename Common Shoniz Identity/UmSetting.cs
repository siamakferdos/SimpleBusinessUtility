namespace Shoniz.Common.ShonizIdentity
{
    public class ShonizIdentitySetting
    {
        public string ConnectionString { get; set; }
        public int ProgramId { get; set; }
        public string RunSpName { get; set; }

        public ShonizIdentitySetting(string connectionString, int programId)
        {
            ConnectionString = connectionString;
            RunSpName = "RunSp";
            ProgramId = programId;
        }


    }
}
