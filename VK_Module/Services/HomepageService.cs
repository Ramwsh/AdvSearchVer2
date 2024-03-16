using System;
using System.Collections.Generic;
using System.Linq;
using VK_Module.Databases.AdvertisementsDbModel;
using VK_Module.Scripts;

namespace Services
{
    public class HomepageService
    {
        private AdvertisementsDbModel advertisementsDatabase;

        public HomepageService()
        {
            advertisementsDatabase = new AdvertisementsDbModel(@"Databases\AdvertisementsDb.db");
        }
        
        public List<Advertisement> GetAllAdvertisements()
        {
            return advertisementsDatabase.GetAllAdvertisements();
        }

        public int GetAdvertisementCountByState(string state)
        {
            var advertisements = GetAllAdvertisements();
            int count = advertisements.Where(adv => adv.State == state).Count();
            return count;
        }        

        public int GetAdvertisementCountByHousingType(string housingType)
        {
            return GetAllAdvertisements().Where(adv => adv.HousingType == housingType).Count();
        }        

        public void CleanDatabase()
        {
            advertisementsDatabase.CleanDatabase();
        }

        public void RemoveAdvertisementById(Advertisement advertisement)
        {
            advertisementsDatabase.RemoveAdvertisementById(advertisement);            
        }

        public void UpdateAdvertisementById(Advertisement advertisement)
        {
            advertisementsDatabase.UpdateAdvertisementById(advertisement);
        }        
    }
}
