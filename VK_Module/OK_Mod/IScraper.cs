using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK_Module.OK_Mod
{
    interface IScraper
    {
        string ScrapeText();
        string ScrapeDate();
        string ScrapeAuthorName();
        List<string> ScrapeImages();
    }
}
