using System;
using System.Text.RegularExpressions;

namespace VK_Module.OK_Mod
{
    class OKDateYearBuilder
    {
        private string pattern = @"(\d{4})";

        public int BuildYear(string stringDate)
        {
            Match match = Regex.Match(stringDate, pattern);
            if (match.Success)
            {
                return Convert.ToInt32(match.Value);
            }
            else
            {
                return DateTime.Now.Year;
            }
        }
    }
}
