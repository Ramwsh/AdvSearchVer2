using System;
using System.Collections.Generic;
using System.Linq;
using VK_Module.Cian_Mod;
using VK_Module.Databases.AdvertisementsDbModel;
using VK_Module.Scripts;

namespace Services
{
    public class CianService : ServiceInputData, IServiceLoader
    {        
        public CianService(List<string> groups, List<string> ignoreWords, string fromDate, string toDate, int amountOfAds, int pagesCount)
            : base(groups, ignoreWords, fromDate, toDate, amountOfAds)
        {
            if (pagesCount > 1)
            {
                AdjustPagesLinkByPagesCount(pagesCount);                
            }
        }

        public void GetAdvertisements()
        {            
            foreach (var group in groups)
            {                
                CianAdvertisementsCardBuilder cardBuilder = new CianAdvertisementsCardBuilder(group, amountOfAds);
                var phones = cardBuilder.GetAdvertisementPhones();
                if (amountOfAds != 0)
                {
                    phones = phones.Take(amountOfAds).ToList();
                }                                
                var links = cardBuilder.GetAdvertisementLinks();
                if (amountOfAds != 0)
                {
                    links = links.Take(amountOfAds).ToList();
                }                                
                CianAdvertisementBuilder cianAdvertisementBuilder = new CianAdvertisementBuilder(phones, links);
                List<Advertisement> advertisements = cianAdvertisementBuilder.ConstructAdvertisements(toDate, fromDate);
                advertisements = advertisements.Where(adv => !string.IsNullOrEmpty(adv.Id)).ToList();                
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

        private void AdjustPagesLinkByPagesCount(int pagesCount)
        {
            string url = groups[0];
            groups.Clear();
            for (int i = 1; i <= pagesCount; i++)
            {
                string newUrl = url.Substring(0, url.Length - 1) + i;
                groups.Add(newUrl);
            }            
        }
    }
}
