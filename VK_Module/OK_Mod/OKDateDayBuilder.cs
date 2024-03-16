using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VK_Module.OK_Mod
{
    class OKDateDayBuilder
    {        
        private List<Tuple<int, string>> TodayYesterdayPatterns = new List<Tuple<int, string>>()
        {            
            Tuple.Create(DateTime.Now.Day,@"\d\d:\d\d"),
        };

        private string[] DayofMonthPatterns = {
                @"\d+(\s)"
            };

        public int BuildDay(string stringDate)
        {
            if (stringDate.Contains("вчера"))
            {
                return DateTime.Now.Day-1;
            }
            bool IsToday = false;
            IsToday = IsCurrentDay(stringDate);
            if (IsToday)
            {
                return DateTime.Now.Day;
            }
            return DayOfMonth(stringDate);
        }

        private int DayOfMonth(string stringDate)
        {
            foreach (string pattern in DayofMonthPatterns)
            {
                Match match = Regex.Match(stringDate, pattern);
                if (match.Success)
                {
                    return Convert.ToInt32(match.Value);
                }
            }
            return DateTime.Now.Day;
        }

        private bool IsCurrentDay(string stringDate)
        {
            for (int i = 0; i<TodayYesterdayPatterns.Count; i++)
            {
                Match match = Regex.Match(stringDate, TodayYesterdayPatterns[i].Item2, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    int result = TodayYesterdayPatterns[i].Item1;
                    return true;
                }
            }
            return false;
        }
    }
}
