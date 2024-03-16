using System.Collections.Generic;

namespace VK_Module.Databases.AdvertisementsDbModel
{
    public interface IAdvertisementsRepository<T>
    {
        void AddAdvertisement(T adExemplar);
        void RemoveAdvertisementById(T adExemplar);
        T GetAdvertisementById(T adExemplar);

        void UpdateAdvertisementById(T adExemplar);

        List<T> GetAllAdvertisements();
    }
}
