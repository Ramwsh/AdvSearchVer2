using System;
using System.Collections.Generic;
using System.Linq;

namespace VK_Module.Scripts
{
    class AdvertisementFilter
    {               
        public List<Advertisement> FilterAdvertisementsByKeywords(List<Advertisement> ads, List<string> keywords)
        {
            List<Advertisement> advertisements = ads;
            advertisements.ForEach(ad => { ad.Description = ConvertToLowerCase(ad.Description); });
            keywords = keywords.ConvertAll(kw => kw = ConvertToLowerCase(kw));
            ads.RemoveAll(ad => !keywords.Any(kw => ad.Description.Contains(kw)));
            return advertisements;
        }

        public List<Advertisement> FilterAdvertisementsByNames(List<Advertisement> ads, List<string> filterNames)
        {
            if (filterNames == null || filterNames.Count == 0)
            {
                return ads;
            }
            List<Advertisement> advertisements = ads;
            advertisements.ForEach(ad => { ad.Poster = ConvertToLowerCase(ad.Poster); });
            filterNames = filterNames.ConvertAll(fname => fname = ConvertToLowerCase(fname));
            advertisements.RemoveAll(ad => filterNames.Contains(ad.Poster));
            return advertisements;
        }

        public List<Advertisement> FilterAdvertisementsByDate(List<Advertisement> ads, DateTime fromDate, DateTime toDate)
        {
            List<Advertisement> advertisements = ads;
            advertisements.RemoveAll(ad => ad.Date <= fromDate && ad.Date >= toDate);
            return advertisements;
        }        

        public string ConvertToLowerCase(string text)
        {
            string convertedText = text.ToLowerInvariant();
            return convertedText;
        }                
    }
}
