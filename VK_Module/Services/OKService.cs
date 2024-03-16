using System.Collections.Generic;
using System.Linq;
using VK_Module.Databases.AdvertisementsDbModel;
using VK_Module.OK_Mod;
using VK_Module.Scripts;

namespace Services
{
    public class OKService : ServiceInputData, IServiceLoader
    {        
        public OKService(List<string> groups, List<string> ignoreWords, string fromDate, string toDate, int amountOfAds)
            : base(groups, ignoreWords, fromDate, toDate, amountOfAds) { }        

        public void GetAdvertisements()
        {
            foreach (var group in groups)
            {
                OKFilesLoadManager oKFilesLoadManager = new OKFilesLoadManager();
                List<Advertisement> advertisements = oKFilesLoadManager.GetAdvertisements(group, amountOfAds, toDate, fromDate, ignoreWords);                
                advertisements = advertisements.Select(adv => adv).Where(adv => !adv.HousingType.Equals("Не определен")).ToList();                
                if (advertisements != null)
                {
                    AddAdvertisementsToDatabase(advertisements);
                }
            }
        }

        public void AddAdvertisementsToDatabase(List<Advertisement> advertisements)
        {
            AdvertisementsDbModel advertisementsDb = new AdvertisementsDbModel(@"Databases\AdvertisementsDb.db");
            foreach (var advertisement in advertisements)
            {
                advertisementsDb.AddAdvertisement(advertisement);
            }            
        }
    }
}
