using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace VK_Module.Databases.VKGroupsLoadDbModel
{
    public class VKGroupsLoadDbModel : DbConnectionInitializer, IVKGroupsLoadRepository
    {
        public VKGroupsLoadDbModel(string dbPath) : base(dbPath) { }

        public void AddGroupLink(string groupLink)
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO GroupLinks(GroupLink) VALUES ('{groupLink}')";
                command.ExecuteNonQuery();                
            }
        }

        public void RemoveGroupLink(string groupLink)
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM GroupLinks WHERE GroupLink = '{groupLink}'";
                command.ExecuteNonQuery();                
            }
        }

        public List<string> GetAllGroupLinks()
        {
            List<string> groupLinks = new List<string>();
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT GroupLink FROM GroupLinks";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string groupLink = reader.GetString(0);
                        groupLinks.Add(groupLink);
                    }
                }
            }
            return groupLinks;
        }
    }
}
