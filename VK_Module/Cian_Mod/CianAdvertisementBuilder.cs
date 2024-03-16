using System.Collections.Generic;
using System.Linq;
using VK_Module.Scripts;
using System;
using System.Text;
using VK_Module.HousingTypeClassifier;
using VK_Module.MVVM.View;
using System.Windows;
using System.Windows.Threading;

namespace VK_Module.Cian_Mod
{
    public class CianAdvertisementBuilder
    {
        private List<string> phones;
        private List<string> links;
        
        public CianAdvertisementBuilder(List<string> phones, List<string> links)
        {
            this.phones = phones;
            this.links = links;
        }
        
        public List<Advertisement> ConstructAdvertisements(string dateTo, string dateFrom)
        {
            DateTime dateStart = default(DateTime);
            DateTime dateEnd = default(DateTime);            
            if (!string.IsNullOrEmpty(dateTo) && !string.IsNullOrEmpty(dateFrom))
            {
                DateManager dateManager = new DateManager();
                dateStart = dateManager.GetFormattedDateFromForm(dateTo);
                dateEnd = dateManager.GetFormattedDateFromForm(dateFrom);
            }            

            List<Advertisement> advertisements = new List<Advertisement>();

            ProgressBarWindow progressBarWindow = new ProgressBarWindow();
            progressBarWindow.ProgressBar.Minimum = 0;
            progressBarWindow.ProgressBar.Value = 0;
            progressBarWindow.ProgressBar.Maximum = links.Count;
            progressBarWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;            
            progressBarWindow.Show();

            foreach (var item in links.Zip(phones, (link, phone) => new { Link = link, Phone = phone }))
            {
                string link = item.Link;
                string phone = item.Phone.Replace("\n", " ").Trim();
                CianAdPageScraper pageScraper = new CianAdPageScraper();
                pageScraper.LoadHtmlDocumentByUrl(link);
                DateTime date = pageScraper.ScrapeDate();
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
                string title = pageScraper.ScrapeAdTitle().Trim().Replace("\n", " ").Trim();                
                string price = pageScraper.ScrapePrice().Trim().Replace("\n", " ").Trim();
                string address = pageScraper.ScrapeAddress().Trim().Replace("\n", " ").Trim();
                string factoidItems = pageScraper.ScrapeFactoidItems().Trim().Replace("\n", " ").Trim();
                string description = pageScraper.ScrapeDescription().Trim().Replace("\n", " ").Trim();
                string summaryItems = pageScraper.ScrapeOfferSumary().Trim().Replace("\n", " ").Trim();
                string photoLinks = pageScraper.ScrapeEstatePhotos().Trim().Replace("\n", " ").Trim();

                string id = "CIAN" + ConstructIdFromLink(link);
                string roomsCount = ConstructAdvertisementRoomsCount(description);
                string housingType = ConstructAdvertisementHousingType(title);
                string content = ConstructAdvertisementContent(description, summaryItems, address, phone, price, id, date.ToString());
                advertisements.Add(ConstructAdvertisement(id, link, phone, date, photoLinks, content, housingType, roomsCount));
                progressBarWindow.ProgressBar.Dispatcher.Invoke(() => progressBarWindow.ProgressBar.Value++, DispatcherPriority.Background);
            }
            progressBarWindow.Close();
            return advertisements;
        }

        private Advertisement ConstructAdvertisement(string id, string link, string phone, DateTime date, string photoLinks, string content, string housingType, string roomsCount)
        {
            AdvertisementBuilder builder = new AdvertisementBuilder();
            return builder.SetId(id).
                SetLink(link).
                SetPoster(phone).
                SetDate(date).
                SetImageLinks(photoLinks).
                SetDescription(content).
                SetHousingType(housingType).
                SetRoomCount(roomsCount).
                SetState("Загружено").
                Build();            
        }

        private string ConstructAdvertisementRoomsCount(string description)
        {
            RoomsCountEstimator estimator = new RoomsCountEstimator();
            return estimator.Estimate(description);
        }
        
        private string ConstructAdvertisementHousingType(string descrprtion)
        {
            HousingTypeClassifierClient classifier = new HousingTypeClassifierClient();
            return classifier.SetText(descrprtion).Classify();
        }

        private string ConstructAdvertisementContent(params string[] args)
        {
            StringBuilder contentBuiler = new StringBuilder();
            foreach (var arg in args)
            {
                contentBuiler.AppendLine(arg);
            }
            return contentBuiler.ToString();
        }

        private string ConstructIdFromLink(string urlLink)
        {
            string result = string.Join("", urlLink.Where(ch => char.IsDigit(ch)));
            return result;
        }

    }
}
