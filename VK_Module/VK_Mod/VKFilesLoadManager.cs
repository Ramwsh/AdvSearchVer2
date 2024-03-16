using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using VK_Module.Scripts;
using VK_Module.VK_Mod;
using VK_Module.Config;
using System.Threading;
using System.Linq;
using VK_Module.HousingTypeClassifier;
using VK_Module.MVVM.View;
using System.Windows.Threading;
using System.Windows;

namespace VK_API
{
    public class VKFilesLoadManager
    {
        private readonly HttpClient client;
        private string access_token = ConfigurationManager._vkServAccToken;
        private string version = ConfigurationManager._vkVersion;

        public VKFilesLoadManager()
        {
            client = new HttpClient();                                    
        }

        public int GetGroupOwnerID(string groupUrl)
        {
            try
            {
                int groupId;
                string url = $@"https://api.vk.com/method/utils.resolveScreenName?screen_name={ScreenNameParser.GetScreenNameFromUrl(groupUrl)}&access_token={access_token}&v={version}";

                using (var client = new WebClient())
                {
                    string response = client.DownloadString(url);
                    var result = JsonConvert.DeserializeObject<VKResolveScreenNameResult>(response);
                    if (result.response?.type == "group")
                    {
                        groupId = result.response.object_id;
                        return groupId;
                    }
                    if (result.response?.type == "page")
                    {
                        groupId = result.response.object_id;
                        return groupId;
                    }
                }
                return Convert.ToInt32(null);
            }
            catch
            {
                return Convert.ToInt32(null);
            }
        }

        private async Task<List<VKWallPost>> GetWallPosts(int groupId, string accessToken, int amountOfAds)
        {            
            try
            {                                
                string url = $"https://api.vk.com/method/wall.get?count={amountOfAds}&owner_id=-{groupId}&extended=0&access_token={access_token}&v={version}";

                HttpResponseMessage response = client.GetAsync(url).Result;
                string responseMessage = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<VKWallGetResponse>(responseMessage);                
                return result.Response.Items;
            }
            catch
            {                
                return null;
            }
        }        

        public List<Advertisement> GetAdvertisements(string groupUrl, int amountOfAds, string dateFrom, string dateTo, List<string> ignoreNames)
        {            
            HousingTypeClassifierClient classifier = new HousingTypeClassifierClient();
            RoomsCountEstimator estimator = new RoomsCountEstimator();
            DateManager dateManager = new DateManager();
            List<Advertisement> advertisements = new List<Advertisement>();            
            List<VKWallPost> posts = GetWallPosts(GetGroupOwnerID(groupUrl), access_token, amountOfAds).Result;

            List<string> AdvWithNamesToRemove = null;
            if (ignoreNames != null)
            {
                AdvWithNamesToRemove = ignoreNames;
            }
            DateTime dateStart = (default(DateTime));
            DateTime dateEnd = (default(DateTime));
            if (dateFrom != null && dateTo != null)
            {
                dateStart = dateManager.GetFormattedDateFromForm(dateTo);
                dateEnd = dateManager.GetFormattedDateFromForm(dateFrom);
            }

            ProgressBarWindow progressBar = new ProgressBarWindow();
            progressBar.ProgressBar.Value = 0;
            progressBar.ProgressBar.Minimum = 0;
            progressBar.ProgressBar.Maximum = posts.Count;
            progressBar.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            progressBar.Show();

            foreach (var post in posts)
            {
                try
                {
                    if (string.IsNullOrEmpty(post.Text))
                    {
                        progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                        continue;
                    }
                    if (dateStart != default(DateTime) && dateEnd != default(DateTime))
                    {
                        var postDate = dateManager.FromUnixTimestamp(post.Date);
                        postDate = new DateTime(postDate.Year, postDate.Month, postDate.Day);
                        if (postDate >= dateStart && postDate <= dateEnd)
                        {
                            
                        }
                        else
                        {                            
                            progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                            continue;
                        }
                    }
                    VKPoster poster = new VKPoster();
                    poster.GetUserName(post.From_Id);
                    Thread.Sleep(2000);
                    var posterName = poster.GetPosterNameData();
                    if (AdvWithNamesToRemove.Any(name => name.Equals(posterName, StringComparison.OrdinalIgnoreCase)))
                    {
                        progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                        continue;
                    }
                    classifier = classifier.SetText(post.Text);                                        
                    AdvertisementBuilder builder = new AdvertisementBuilder();
                    advertisements.Add(builder.SetHousingType(classifier.Classify()).
                        SetRoomCount(estimator.Estimate(post.Text)).
                        SetDescription(post.Text).
                        SetPoster(posterName).
                        SetDate(dateManager.FromUnixTimestamp(post.Date)).
                        SetLink($@"{groupUrl}?w=wall-{GetGroupOwnerID(groupUrl)}_{post.Id}%2Fall").
                        SetId("VK" + GetGroupOwnerID(groupUrl) + post.Id.ToString()).
                        SetImageLinks(GetBestAvaiablePhoto(post)).SetState("Загружено")
                        .Build());
                    progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                }
                catch
                {
                    progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                    continue;
                }
            }
            client.Dispose();
            progressBar.Close();
            return advertisements;
        }

        private string GetBestAvaiablePhoto(VKWallPost post)
        {
            string urls = "";
            foreach (var attachment in post.Attachments)
            {
                if (attachment.Type == "photo")
                {
                    var photo = attachment.Photo;
                    urls += photo.Sizes.Last().Url + ";";
                }
            }
            return urls;
        }        
    }
}
