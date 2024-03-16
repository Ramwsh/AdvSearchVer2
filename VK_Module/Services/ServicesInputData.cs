using System.Collections.Generic;

namespace Services
{
    public class ServiceInputData
    {
        protected List<string> groups;        
        protected List<string> ignoreWords;

        protected string fromDate;
        protected string toDate;

        protected int amountOfAds;

        protected ServiceInputData(List<string> groups, List<string> ignorewords, string fromDate, string toDate, int amountOfAds)
        {
            this.groups = groups;            
            this.ignoreWords = ignorewords;
            this.amountOfAds = amountOfAds;

            if (fromDate != null && toDate != null)
            {
                this.fromDate = fromDate;
                this.toDate = toDate;
            }
        }
    }
}
