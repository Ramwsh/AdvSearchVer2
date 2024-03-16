using VK_Module.Databases.NameFilterDbModel;
using System.Collections.Generic;
using System.Linq;
using System;

namespace VK_Module.Cian_Mod
{
    public class CianAdvertisementsCardBuilder
    {
        List<string> advertisementLinks = new List<string>();
        List<string> advertisementPhones = new List<string>();

        private string cataloguePageUrl;

        private int expectedAmountOfAds;

        public CianAdvertisementsCardBuilder(string pageUrl, int expectedAmountOfAds)
        {
            this.expectedAmountOfAds = expectedAmountOfAds;
            cataloguePageUrl = pageUrl;
            ConstructPhonesAndLinks(this.expectedAmountOfAds);
        }

        private void ConstructPhonesAndLinks(int amountOfAds)
        {
            int expectedAmountOfAds = amountOfAds;
            CianCataloguePageScraper catalogueScraper = new CianCataloguePageScraper(expectedAmountOfAds);
            catalogueScraper.OpenCianPage(cataloguePageUrl);
            catalogueScraper.ScrollToTheBottom();
            var cards = catalogueScraper.GetCardElements();
            catalogueScraper.ClickOnEachPhoneButton(cards);
            List<string> ignoreIds = new NameFilterDbModel("Databases\\NameFilterDb.db").GetAllNames();            
            foreach (var card in cards)
            {
                string publisherStatus = catalogueScraper.GetPublisherStatus(card);
                if (publisherStatus == "РИЭЛТОР" || publisherStatus == "АГЕНТСТВО НЕДВИЖИМОСТИ")
                {
                    continue;
                }
                string id = catalogueScraper.GetId(card);
                if (ignoreIds.Any(ignoreId => ignoreId.Equals(id, StringComparison.OrdinalIgnoreCase)))
                {
                    var detected = ignoreIds.Select(ignoreId => ignoreId).Where(ignoreId => ignoreId.Equals(id)).First();                    
                    continue;
                }
                advertisementLinks.Add(catalogueScraper.GetAdvertisementUrlFromCard(card));
                advertisementPhones.Add(catalogueScraper.GetPhoneNumberFromCard(card));
            }
            catalogueScraper.DestoryDriver();
        }

        public List<string> GetAdvertisementPhones()
        {
            return advertisementPhones;
        }

        public List<string> GetAdvertisementLinks()
        {
            return advertisementLinks;
        }
    }
}
