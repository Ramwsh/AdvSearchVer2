using System;
using VK_Module.Config;
using System.Net.Http;
using Newtonsoft.Json.Linq;

namespace VK_API
{
    class VKPoster
    {
        private readonly string accessToken = ConfigurationManager._vkAccToken;
        private readonly string serviceAccessToken = ConfigurationManager._vkServAccToken;

        private string firstName;
        private string lastName;

        private string groupName;

        public void GetUserName(string id)
        {
            if (Convert.ToInt64(id) < 0)
            {
                HttpClient httpClient = new HttpClient();
                string url = $"https://api.vk.com/method/groups.getById?group_id={Convert.ToInt64(id)*-1}&access_token={accessToken}&v=5.131";
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                string _groupName = json.response[0].name;

                groupName = _groupName;

                httpClient.Dispose();
            }
            else
            {
                HttpClient httpClient = new HttpClient();
                string url = $"https://api.vk.com/method/users.get?user_ids={id}&fields=bdate,city,country&access_token={accessToken}&v=5.131";
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string responseBody = response.Content.ReadAsStringAsync().Result;

                dynamic json = JObject.Parse(responseBody);
                string _firstName = json.response[0].first_name;
                string _lastName = json.response[0].last_name;

                firstName = _firstName;
                lastName = _lastName;

                httpClient.Dispose();
            }                        
        }

        public string GetPosterNameData()
        {
            if (!string.IsNullOrEmpty(firstName) && !string.IsNullOrEmpty(lastName))
            {
                return $"{firstName} {lastName}";
            }
            else
            {
                return $"{groupName}";
            }
        }
    }
}
