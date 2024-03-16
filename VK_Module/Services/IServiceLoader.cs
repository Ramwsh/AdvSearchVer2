using System.Collections.Generic;
using VK_Module.Scripts;

namespace Services
{
    public interface IServiceLoader
    {
        public void GetAdvertisements();
        public void AddAdvertisementsToDatabase(List<Advertisement> advertisements);
    }
}
