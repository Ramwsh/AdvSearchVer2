using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;

namespace VK_Module.Cian_Mod
{
    public class CianAdPageScraper
    {
        private HtmlDocument document;                
        
        public void LoadHtmlDocumentByUrl(string url)
        {
            //ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--incognito");
            //IWebDriver driver = new ChromeDriver();
            //driver.Navigate().GoToUrl(url);
            //ScrollToTheBottom(driver);
            //string pageSource = driver.PageSource;            
            //driver.Quit();
            //driver.Dispose();
            //document = new HtmlDocument();
            //document.LoadHtml(pageSource);
            document = new HtmlDocument();
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            web.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0";
            document = web.Load(url);
        }
        
        public string ScrapeAdTitle()
        {
            HtmlNode element = document.DocumentNode.SelectSingleNode("//h1[contains(@class, 'a10a3f92e9--title--vlZwT')]");            
            if (element != null)
            {
                string titleText = element.InnerText;
                titleText = titleText.Substring(0, titleText.Length - 1) + 2;
                return titleText;
            }
            return "";
        }

        public string ScrapePrice()
        {
            HtmlNode element = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'a10a3f92e9--price--Pg6fn')]");
            if (element != null)
            {
                HtmlNode priceElement = element.SelectSingleNode(".//span");
                string priceText = priceElement.InnerText;
                priceText = priceText.Substring(0, priceText.Length - 1) + "Р";
                return priceText;
            }
            return "";
        }

        public DateTime ScrapeDate()
        {
            HtmlNode element = document.DocumentNode.SelectSingleNode("//div[@data-testid='metadata-added-date']");
            if (element != null)
            {
                HtmlNode spanElement = element.SelectSingleNode(".//span");
                string dateText = spanElement.InnerText;
                dateText = dateText.Split(',').First();
                return new CianPageDateTimeConverter().ConvertDate(dateText);
            }            
            return DateTime.Now;
        }

        public string ScrapeAddress()
        {            
            HtmlNode element = document.DocumentNode.SelectSingleNode("//div[@data-name='AddressContainer']");
            if (element != null )
            {
                IEnumerable<HtmlNode> addressItems = element.SelectNodes(".//a[@data-name='AddressItem']");
                string address = string.Join(", ", addressItems.Select(address => address.InnerText));
                return address;
            }
            return "";
        }

        public string ScrapeFactoidItems()
        {
            HtmlNode element = document.DocumentNode.SelectSingleNode("//div[@data-name='ObjectFactoids']");
            if (element != null)
            {
                StringBuilder scrapedText = new StringBuilder();
                IEnumerable<HtmlNode> factoidItem = element.SelectNodes(".//div[@data-name='ObjectFactoidsItem']");

                foreach (HtmlNode item in factoidItem)
                {
                    IEnumerable<HtmlNode> spanElements = item.Descendants("span");
                    foreach (HtmlNode spanElement in spanElements)
                    {
                        scrapedText.AppendLine(spanElement.InnerText);
                    }
                    scrapedText.AppendLine();
                }
                string scrapedItems = scrapedText.ToString();
                return scrapedItems;
            }
            return "";
        }

        public string ScrapeDescription()
        {
            HtmlNode element = document.DocumentNode.SelectSingleNode("//div[@data-name='Description']");            
            if (element != null)
            {
                string description = element.InnerText;
                return description;
            }
            return "";
        }

        public string ScrapeOfferSumary()
        {
            StringBuilder scrapedText = new StringBuilder();
            HtmlNodeCollection elements = document.DocumentNode.SelectNodes("//div[@data-name='OfferSummaryInfoItem']");
            if (elements != null)
            {
                foreach (var element in elements)
                {
                    IEnumerable<HtmlNode> spanElements = element.Descendants("span");
                    if (spanElements != null)
                    {
                        foreach (HtmlNode spanElement in spanElements)
                        {
                            scrapedText.Append(spanElement.InnerText + " ");
                        }
                        scrapedText.AppendLine();
                    }                    
                }
                return scrapedText.ToString();
            }
            return "";
        }

        public string ScrapeEstatePhotos()
        {
            string photoUrls = "";
            HtmlNodeCollection imgNodes = document.DocumentNode.SelectNodes("//img[contains(@class, 'a10a3f92e9--container--KIwW4')]");
            if (imgNodes != null)
            {
                foreach (var imgNode in imgNodes)
                {
                    string imageUrl = imgNode?.GetAttributeValue("src", null);
                    photoUrls += imageUrl + ";";
                }
            }
            return photoUrls;
        }

        //public void ScrollToTheBottom(IWebDriver driver)
        //{
        //    IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
        //    long scrollHeight = (long)jsExecutor.ExecuteScript("return Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);");
        //    while (true)
        //    {
        //        jsExecutor.ExecuteScript("window.scrollTo(0, document.documentElement.scrollHeight);");
        //        Thread.Sleep(1000);
        //        long newScrollHeight = (long)jsExecutor.ExecuteScript("return Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);");
        //        if (newScrollHeight == scrollHeight)
        //        {
        //            break;
        //        }
        //        scrollHeight = newScrollHeight;
        //    }
        //    Thread.Sleep(1000);
        //}
    }
}
