using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace VK_Module.Databases.NameFilterDbModel
{
    public class NameFilterDbModel : DbConnectionInitializer, INameFilterRepository
    {
        public NameFilterDbModel(string dbPath) : base(dbPath) { }

        public void AddName(string name)
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO Names (Name) VALUES ('{name}')";
                command.ExecuteNonQuery();
            }
        }

        public List<string> GetAllNames()
        {            
            List<string> names = new List<string>();
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Name FROM Names";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string name = reader.GetString(0);
                        names.Add(name);
                    }
                }
            }
            return names;
        }

        public void RemoveName(string name)
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM Names WHERE Name='{name}'";
                command.ExecuteNonQuery();
            }
        }
    }
}
