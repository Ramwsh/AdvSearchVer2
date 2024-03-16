using System.Collections.Generic;
using System.IO;
using VK_Module.Databases.AdRepositoriesDbModel;
using VK_Module.Scripts;

namespace Services
{
    public class AdvRepositoriesService
    {
        private AdRepositoriesDbModel repositoriesDb;
        public AdvRepositoriesService()
        {
            repositoriesDb = new AdRepositoriesDbModel(@"Databases\AdvertisementRepositoriesDb.db");
        }

        public List<AdvertisementRepository> GetAllRepositories()
        {
            return repositoriesDb.GetAllRepositories();
        }

        public void RemoveRepository(AdvertisementRepository repository)
        {
            if (Directory.Exists(repository.Path))
            {
                Directory.Delete(repository.Path, true);
                repositoriesDb.RemoveRepository(repository);
            }            
        }

        public void AddRepository(AdvertisementRepository repository)
        {
            if (!Directory.Exists(repository.Path))
            {
                Directory.CreateDirectory(repository.Path);
                repositoriesDb.AddRepository(repository);
            }            
        }
    }
}
