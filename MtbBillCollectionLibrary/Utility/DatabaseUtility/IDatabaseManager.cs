using System.Data;

namespace MtbBillCollectionLibrary.Utility.DatabaseUtility
{
    public interface IDatabaseManager
    {
        string SetConnectionString { set; }
        string UserName { get; set; }
        string DatabaseName { get; set; }
        string ServerName { get; set; }
        string Password { set; }
        void BuildConnectionString();
        void OpenDatabaseConnection();
        void CloseDatabaseConnection();
        DataTable GetDataTable(string query);
        string GetSingleString(string query);
        bool ExcecuteCommand(string commandText);
        void BeginTransaction();
        void Commit();
        void RollBack();
    }
}
