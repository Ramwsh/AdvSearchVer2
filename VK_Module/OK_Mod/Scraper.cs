using HtmlAgilityPack;
using System.Collections.Generic;

namespace VK_Module.OK_Mod
{
    public class Scraper : IScraper
    {
        private readonly string url;
        private readonly HtmlWeb htmlWeb;
        private readonly HtmlDocument doc;

        public Scraper(string _url)
        {
            url = _url;
            htmlWeb = new HtmlWeb();
            doc = htmlWeb.Load(url);
        }

        public string ScrapeText()
        {            
            HtmlNode divTextElement = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'media-text_cnt_tx')]");
            if (divTextElement != null)
            {
                return divTextElement.InnerText;
            }
            return "empty";
        }

        public string ScrapeAuthorName()
        {
            try
            {                
                HtmlNode divAuthorElement = doc.DocumentNode.SelectSingleNode("//div[@class='inline__kzqdm']");
                if (divAuthorElement != null)
                {
                    return divAuthorElement.InnerText;
                }
                return "Unknown";
            }
            catch
            {
                return "Unknown";
            }            
        }

        public string ScrapeDate()
        {            
            HtmlNode dateElement = doc.DocumentNode.SelectSingleNode("//span[contains(@class, 'group-author-bottom_item__kzqdm')]/time");
            if (dateElement != null)
            {
                return dateElement.InnerText;
            }
            return "01:01";
        }

        public List<string> ScrapeImages()
        {
            List<string> imageUrls = new List<string>();            
            HtmlNodeCollection imgElements = doc.DocumentNode.SelectNodes("//img[contains(@class, 'media-photos_img')]");
            if (imgElements != null)
            {
                foreach (HtmlNode imgElement in imgElements)
                {
                    if (imgElement != null)
                    {
                        imageUrls.Add(imgElement.GetAttributeValue("src", ""));
                    }
                }
            }            
            return imageUrls;
        }        
    }
}
