using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VK_Module.OK_Mod
{
    class OKDateMonthBuilder
    {
        private int month;

        private List<Tuple<int, string>> monthPatterns = new List<Tuple<int, string>>()
        {
            Tuple.Create(1,@"янв"),
            Tuple.Create(2,@"фев"),
            Tuple.Create(3,@"мар"),
            Tuple.Create(4,@"апр"),
            Tuple.Create(5,@"мая"),
            Tuple.Create(6,@"июн"),
            Tuple.Create(7,@"июл"),
            Tuple.Create(8,@"авг"),
            Tuple.Create(9,@"сен"),
            Tuple.Create(10,@"окт"),
            Tuple.Create(11,@"ноя"),
            Tuple.Create(12,@"дек")
        };

        public int BuildMonth(string stringDate)
        {
            if (stringDate.Contains("янв"))
                return 1;
            if (stringDate.Contains("фев"))
                return 2;
            if (stringDate.Contains("мар"))
                return 3;
            if (stringDate.Contains("апр"))
                return 4;
            if (stringDate.Contains("мая"))
                return 5;
            if (stringDate.Contains("июн"))
                return 6;
            if (stringDate.Contains("июл"))
                return 7;
            if (stringDate.Contains("авг"))
                return 8;
            if (stringDate.Contains("сен"))
                return 9;
            if (stringDate.Contains("окт"))
                return 10;
            if (stringDate.Contains("ноя"))
                return 11;
            if (stringDate.Contains("дек"))
                return 12;
            return DateTime.Now.Month;
        }
    }
}
