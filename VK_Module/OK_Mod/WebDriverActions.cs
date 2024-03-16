using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using VK_Module.Scripts;
using System.Linq;
using System.Collections.ObjectModel;
using VK_Module.HousingTypeClassifier;
using VK_Module.MVVM.View;
using System.Windows;
using System.Windows.Threading;
using System;

namespace VK_Module.OK_Mod
{
    class WebDriverActions
    {
        private IWebDriver _driver;        
        private OKDateTimeBuilder dateTimeBuilder;

        public WebDriverActions()
        {
            dateTimeBuilder = new OKDateTimeBuilder();                        
            ChromeOptions options = new ChromeOptions();            
            options.AddArguments("--incognito");
            _driver = new ChromeDriver(options);                                    
        }

        public void NavigateAndScroll(string url)
        {            
            _driver.Navigate().GoToUrl(url);
            Thread.Sleep(5000);
            ScrollToTheBottom();                        
        }

        private ReadOnlyCollection<IWebElement> GetTextElements()
        {
            return _driver.FindElements(By.ClassName("media-text_cnt_tx"));
        }

        public List<string> GetTopicUrls()
        {
            List<string> topicUrls = new List<string>();
            ReadOnlyCollection<IWebElement> textElements = GetTextElements();
            foreach (var textElement in textElements)
            {
                IWebElement linkElement = textElement.FindElement(By.ClassName("media-text_a"));
                topicUrls.Add(linkElement.GetAttribute("href"));
            }
            DestroyDriver();
            return topicUrls;
        }

        private void DestroyDriver()
        {
            _driver.Quit();
            _driver.Dispose();
        }

        public List<Advertisement> GetAdvertisements(List<string> topicUrls, string dateTo, string dateFrom, List<string> ignoreNames)
        {
            DateTime dateStart = (default(DateTime));
            DateTime dateEnd = (default(DateTime));
            List<string> AdvNamesToRemove = null;
            if (!string.IsNullOrEmpty(dateTo) && !string.IsNullOrEmpty(dateFrom))
            {
                DateManager datemanager = new DateManager();
                dateStart = datemanager.GetFormattedDateFromForm(dateTo);
                dateEnd = datemanager.GetFormattedDateFromForm(dateFrom);
            }
            if (ignoreNames != null)
            {
                AdvNamesToRemove = ignoreNames;
            }
            HousingTypeClassifierClient classifier = new HousingTypeClassifierClient();
            RoomsCountEstimator estimator = new RoomsCountEstimator();
            List<Advertisement> advertisements = new List<Advertisement>();

            ProgressBarWindow progressBarWindow = new ProgressBarWindow();
            progressBarWindow.ProgressBar.Value = 0;
            progressBarWindow.ProgressBar.Minimum = 0;
            progressBarWindow.ProgressBar.Maximum = topicUrls.Count;
            progressBarWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            progressBarWindow.Show();

            foreach (var topicUrl in topicUrls)
            {
                Scraper scraper = new Scraper(topicUrl);                
                var date  = dateTimeBuilder.ConvertFromOkDateToDateTime(scraper.ScrapeDate());
                date = new DateTime(date.Year, date.Month, date.Day);
                if (dateStart != default(DateTime) && dateEnd != default(DateTime))
                {
                    if (date >= dateStart && date <= dateEnd)
                    {
                        
                    }
                    else
                    {                        
                        progressBarWindow.ProgressBar.Dispatcher.Invoke(() => progressBarWindow.ProgressBar.Value++, DispatcherPriority.Background);
                        continue;
                    }
                }
                string author = scraper.ScrapeAuthorName();
                if (AdvNamesToRemove.Any(name => name.Equals(author, StringComparison.OrdinalIgnoreCase)))
                {                    
                    progressBarWindow.ProgressBar.Dispatcher.Invoke(() => progressBarWindow.ProgressBar.Value++, DispatcherPriority.Background);
                    continue;
                }
                string description = scraper.ScrapeText();
                classifier = classifier.SetText(description);
                AdvertisementBuilder builder = new AdvertisementBuilder();
                advertisements.Add(builder.SetLink(topicUrl).
                    SetDescription(description).
                    SetPoster(scraper.ScrapeAuthorName()).
                    SetId("OK"+topicUrl.Split('/').Last()).
                    SetDate(dateTimeBuilder.ConvertFromOkDateToDateTime(scraper.ScrapeDate())).
                    SetImageLinks(string.Join(';', scraper.ScrapeImages())).SetHousingType(classifier.Classify()).
                    SetRoomCount(estimator.Estimate(description)).SetState("Загружено").Build());
                progressBarWindow.ProgressBar.Dispatcher.Invoke(() => progressBarWindow.ProgressBar.Value++, DispatcherPriority.Background);
            }
            progressBarWindow.Close();
            return advertisements;
        }

        public List<string> ReduceUrlsCollection(List<string> urls, int expectedAmountOfAds)
        {
            if (urls.Count > expectedAmountOfAds)
            {
                urls = urls.Take(expectedAmountOfAds).ToList();
            }
            return urls;
        }        

        private void ScrollToTheBottom()
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;
            long scrollHeight = (long)jsExecutor.ExecuteScript("return Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);");
            while (true)
            {
                jsExecutor.ExecuteScript("window.scrollTo(0, document.documentElement.scrollHeight);");
                Thread.Sleep(2000);
                long newScrollHeight = (long)jsExecutor.ExecuteScript("return Math.max(document.documentElement.scrollHeight, document.body.scrollHeight);");
                if (newScrollHeight == scrollHeight)
                {
                    break;
                }
                scrollHeight = newScrollHeight;
            }
            Thread.Sleep(2000);
        }        
    }
}
