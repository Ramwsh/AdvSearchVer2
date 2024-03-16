using System.Collections.Generic;
using VK_Module.Scripts;

namespace VK_Module.Databases.AdRepositoriesDbModel
{
    public interface IAdRepositories
    {        
        void AddRepository(AdvertisementRepository repository);
        void RemoveRepository(AdvertisementRepository repository);
        List<AdvertisementRepository> GetAllRepositories();
    }
}
