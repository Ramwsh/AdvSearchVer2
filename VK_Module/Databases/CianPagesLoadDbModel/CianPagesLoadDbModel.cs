using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace VK_Module.Databases.CianPagesLoadDbModel
{
    public class CianPagesLoadDbModel : DbConnectionInitializer, ICianPagesLoadRepository
    {
        public CianPagesLoadDbModel(string dbPath) : base(dbPath) { }

        public void AddPageLink(string pageLink)
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO Pages(Page) VALUES ('{pageLink}')";
                command.ExecuteNonQuery();
            }
        }

        public void RemovePageLink(string pageLink)
        {
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM Pages WHERE Page = '{pageLink}'";
                command.ExecuteNonQuery();
            }
        }

        public List<string> GetAllPageLinks()
        {
            List<string> groupLinks = new List<string>();
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT Page FROM Pages";
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
