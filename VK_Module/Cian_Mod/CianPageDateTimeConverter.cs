using System.Collections.Generic;
using System;
using System.Linq;

namespace VK_Module.Cian_Mod
{
    public class CianPageDateTimeConverter
    {        
        private DateTime convertedDate;
        
        public DateTime ConvertDate(string unconvertedDate)
        {
            convertedDate = new DateTime(ConvertYear(), ConvertMonth(unconvertedDate), ConvertDay(unconvertedDate));
            return convertedDate;
        }

        private int ConvertDay(string unconvertedDate)
        {
            if (unconvertedDate.Contains("сегодня"))
                return DateTime.Now.Day;
            if (unconvertedDate.Contains("вчера"))
                return DateTime.Now.Day - 1;
            return Convert.ToInt32(new string(unconvertedDate.Where(char.IsDigit).ToArray()));
        }

        private int ConvertYear()
        {
            return DateTime.Now.Year;
        }

        private int ConvertMonth(string unconvertedDate)
        {
            if (unconvertedDate.Contains("сегодня"))
                return DateTime.Now.Month;
            if (unconvertedDate.Contains("вчера"))
                return DateTime.Now.Month;
            Dictionary<string, int> months = new Dictionary<string, int>()
            {
                {"янв", 1 },
                {"фев", 2 },
                {"мар", 3 },
                {"апр", 4 },
                {"май", 5 },
                {"июнь", 6 },
                {"июль", 7 },
                {"авг", 8 },
                {"сен", 9 },
                {"окт", 10 },
                {"ноя", 11 },
                {"дек", 12 },
            };

            foreach (var pair in months)
            {
                if (unconvertedDate.Contains(pair.Key))
                {
                    return pair.Value;
                }
            }
            return 0;
        }
    }
}
