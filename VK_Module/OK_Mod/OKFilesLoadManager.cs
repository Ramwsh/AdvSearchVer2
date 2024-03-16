using System.Collections.Generic;
using VK_Module.Scripts;

namespace VK_Module.OK_Mod
{
    class OKFilesLoadManager
    {                
        public List<Advertisement> GetAdvertisements(string url, int expectedAmountOfAds, string dateTo, string dateFrom, List<string> ignoreNames)
        {
            WebDriverActions actions = new WebDriverActions();
            actions.NavigateAndScroll(url);
            List<string> urls = actions.GetTopicUrls();
            if (urls.Count > expectedAmountOfAds)
            {
                urls = actions.ReduceUrlsCollection(urls, expectedAmountOfAds);
            }
            return actions.GetAdvertisements(urls, dateTo, dateFrom, ignoreNames);
        }           
    }
}
