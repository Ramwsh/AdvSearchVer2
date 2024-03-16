using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using VK_Module.Scripts;

namespace VK_Module.Databases.AdRepositoriesDbModel
{
    public class AdRepositoriesDbModel : DbConnectionInitializer, IAdRepositories
    {        
        public AdRepositoriesDbModel(string dbPath) : base(dbPath) { }        

        public void AddRepository(AdvertisementRepository repository)
        {
            string tag = repository.Tag;            
            string path = repository.Path;

            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO Repositories (path,name) VALUES ('{path}','{tag}')";
                command.ExecuteNonQuery();                
            }
        }

        public List<AdvertisementRepository> GetAllRepositories()
        {
            List<AdvertisementRepository> repositories = new List<AdvertisementRepository>();
            using (connection)
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = "SELECT path, name FROM Repositories";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string path = reader.GetString(0);
                        string name = reader.GetString(1);
                        AdvertisementRepository repository = new AdvertisementRepository(name, path);
                        repositories.Add(repository);
                    }
                }
            }
            return repositories;
        }

        public void RemoveRepository(AdvertisementRepository repository)
        {
            using (connection)
            {
                string path = repository.Path;
                string tag = repository.Tag;
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $"DELETE FROM Repositories WHERE name = '{tag}' AND path = '{path}'";
                command.ExecuteNonQuery();
            }
        }        
    }
}
