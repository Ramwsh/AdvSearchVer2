using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Threading;

namespace VK_Module.Cian_Mod
{
    public class CianCataloguePageScraper
    {
        private IWebDriver driver;

        private int countOfAds;

        public CianCataloguePageScraper(int expectedAmountOfAds)
        {            
            countOfAds = expectedAmountOfAds;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--incognito");
            driver = new ChromeDriver(options);            
        }

        public void OpenCianPage(string pageUrl)
        {
            driver.Navigate().GoToUrl(pageUrl);
        }

        public void ScrollToTheBottom()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            long scrollHeight = (long)jsExecutor.ExecuteScript("return Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);");
            while (true)
            {
                jsExecutor.ExecuteScript("window.scrollTo(0, document.documentElement.scrollHeight);");
                Thread.Sleep(1000);
                long newScrollHeight = (long)jsExecutor.ExecuteScript("return Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);");
                if (newScrollHeight == scrollHeight)
                {
                    break;
                }
                scrollHeight = newScrollHeight;
            }
            Thread.Sleep(1000);
        }

        public IReadOnlyCollection<IWebElement> GetCardElements()
        {
            var cards = driver.FindElements(By.CssSelector("article[data-name='CardComponent']._93444fe79c--container--Povoi._93444fe79c--cont--OzgVc"));
            return cards;
        }

        public void ClickOnEachPhoneButton(IReadOnlyCollection<IWebElement> cards)
        {
            int counter = 0;
            foreach (var card in cards)
            {                
                var phoneButtons = card.FindElements(By.CssSelector("button[data-mark='PhoneButton']"));
                foreach (var button in phoneButtons)
                {
                    try
                    {
                        button.Click();
                        Thread.Sleep(3000);                        
                    }                    
                    catch
                    {
                        continue;
                    }                    
                }           
                counter++;
                if (countOfAds != 0)
                {
                    if (counter == countOfAds)
                    {
                        break;
                    }
                }                         
            }
        }

        public string GetPhoneNumberFromCard(IWebElement card)
        {
            var phones = card.FindElements(By.CssSelector("span[data-mark='PhoneValue']"));
            string phoneNumber = "";
            foreach (var phone in phones)
            {
                phoneNumber += phone.Text + "\n";
            }
            return phoneNumber;
        }

        public string GetAdvertisementUrlFromCard(IWebElement card)
        {
            var linkElement = card.FindElement(By.ClassName("_93444fe79c--media--9P6wN"));
            string url = linkElement.GetAttribute("href");
            return url;
        }

        public string GetPublisherStatus(IWebElement card)
        {
            IWebElement element = card.FindElement(By.CssSelector("span._93444fe79c--color_gray60_100--mYFjS._93444fe79c--lineHeight_4u--E1SPG._93444fe79c--fontWeight_bold--BbhnX._93444fe79c--fontSize_10px--c1NGZ._93444fe79c--display_block--KYb25._93444fe79c--text--e4SBY._93444fe79c--text_textTransform__uppercase--JygPH"));
            var text = element.Text;
            return text;
        }

        public string GetId(IWebElement card)
        {
            IWebElement elemnt = card.FindElement(By.CssSelector("span._93444fe79c--color_current_color--vhuGI._93444fe79c--lineHeight_6u--cedXD._93444fe79c--fontWeight_bold--BbhnX._93444fe79c--fontSize_16px--QNYmt._93444fe79c--display_block--KYb25._93444fe79c--text--e4SBY"));
            var id = elemnt.Text;
            return id;            
        }

        public void DestoryDriver()
        {
            driver.Dispose();
        }        
    }
}
