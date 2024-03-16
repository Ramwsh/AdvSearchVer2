using System.Collections.Generic;
using VK_Module.Databases.AdvertisementsDbModel;
using VK_Module.MVVM.View;
using VK_Module.Scripts;
using VK_API;
using System.Linq;

namespace Services
{
    public class VKService : ServiceInputData, IServiceLoader
    {        
        public VKService(List<string> groups, List<string> ignoreWords, string fromDate, string toDate, int amountOfAds)
            : base(groups, ignoreWords, fromDate, toDate, amountOfAds) { }        

        public void GetAdvertisements()
        {
            foreach (var group in groups)
            {
                VKFilesLoadManager loader = new VKFilesLoadManager();
                List<Advertisement> advertisements = loader.GetAdvertisements(group, amountOfAds, fromDate, toDate, ignoreWords);
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

            // code to remove
            // write dada in txt for future retraining

            //List<string> data = new List<string>();

            //foreach (var advertisement in advertisements)
            //{
            //    string text = advertisement.Description;
            //    text = text.Replace("  ", " ").Trim();
            //    text = text.Replace("\n", " ").Trim();
            //    text = text.ToLower();
            //    string type = advertisement.HousingType;
            //    data.Add($"{text},{type}");
            //}
            //File.AppendAllLines(@"C:\Users\Ramwsh\Desktop\datatext\dataforretraining.txt", data);
        }
    }
}
