using Microsoft.Data.Sqlite;

namespace VK_Module.Databases
{
    public class DbConnectionInitializer
    {
        protected SqliteConnection connection;        

        protected DbConnectionInitializer(string dbPath)
        {
            connection = new SqliteConnection("Data Source=" + dbPath);            
        }        
    }
}
