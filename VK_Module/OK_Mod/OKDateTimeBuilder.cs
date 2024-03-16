using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_Module.OK_Mod
{
    class OKDateTimeBuilder
    {
        OKDateDayBuilder dayBuilder;
        OKDateMonthBuilder monthBuilder;
        OKDateYearBuilder yearBuilder;

        public OKDateTimeBuilder()
        {
            dayBuilder = new OKDateDayBuilder();
            monthBuilder = new OKDateMonthBuilder();
            yearBuilder = new OKDateYearBuilder();
        }

        public DateTime ConvertFromOkDateToDateTime(string stringDate)
        {
            return new DateTime(yearBuilder.BuildYear(stringDate), monthBuilder.BuildMonth(stringDate), dayBuilder.BuildDay(stringDate));
        }
    }
}
