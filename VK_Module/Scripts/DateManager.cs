using System;
using System.Globalization;
using System.Linq;

namespace VK_Module.Scripts
{
    public class DateManager
    {                
        public DateTime ParseTextDate(string strDate)
        {
            try
            {
                string[] splittedDate = strDate.Split('/');
                int day = Convert.ToInt32(splittedDate[0]);
                int month = Convert.ToInt32(splittedDate[1]);
                int year = Convert.ToInt32(splittedDate[2]);
                DateTime result = new DateTime(year, month, day);
                return result;
            }
            catch
            {
                return DateTime.Now;
            }
        }
        
        public double ToUnixTimestamp(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }
        
        public DateTime FromUnixTimestamp(int timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }
        
        public bool CompareDates(DateTime startDate, DateTime endDate, DateTime target)
        {
            if (target >= startDate && target <= endDate)
            {
                return true;
            }
            return false;
        }

        public DateTime[] GetFormattedDateFromHomepage(string homepageDate)
        {
            if (homepageDate.Contains("-"))
            {
                var DateRange = homepageDate.Split("-");
                DateTime startDate = new DateTime(Convert.ToInt32(DateRange[0].Split(".")[2]),
                                                  Convert.ToInt32(DateRange[0].Split(".")[1]),
                                                  Convert.ToInt32(DateRange[0].Split(".")[0])
                                                  );
                DateTime endDate = new DateTime(
                                                  Convert.ToInt32(DateRange[1].Split(".")[2]),
                                                  Convert.ToInt32(DateRange[1].Split(".")[1]),
                                                  Convert.ToInt32(DateRange[1].Split(".")[0])
                                                );
                return new[] { startDate, endDate };
            }
            else
            {
                DateTime startDate = new DateTime(Convert.ToInt32(homepageDate.Split(".")[2]),
                                                  Convert.ToInt32(homepageDate.Split(".")[1]),
                                                  Convert.ToInt32(homepageDate.Split(".")[0])
                                                  );
                return new[] { startDate };
            }
        }



        public DateTime GetFormattedDateFromForm(string unformattedDate)
        {
            int year;
            int month;
            int day;

            string temp = "";
            foreach (var character in unformattedDate)
            {
                if (char.IsDigit(character) || character == '/')
                {
                    temp += character;
                }
            }
            var ddmmyyyy = temp.Split('/').ToArray();
            day = Convert.ToInt32(ddmmyyyy[0]);
            month = Convert.ToInt32(ddmmyyyy[1]);
            year = Convert.ToInt32(ddmmyyyy[2]);

            return new DateTime(year, month, day);
        }
    }
}
