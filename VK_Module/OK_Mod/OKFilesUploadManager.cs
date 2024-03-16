using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.IO;

namespace VK_Module.OK_Mod
{
    public class OKFilesUploadManager
    {
        private string accessToken;
        private string applicationKey;
        private string groupId;

        public OKFilesUploadManager(string url)
        {
            this.groupId = this.GetGroupIdByUrl(url);
            this.accessToken = OKModuleController.OKAccessToken;
            this.applicationKey = OKModuleController.OKApplicationKey;
        }

        public void PostOnlyWithText(string message)
        {
            message = message.Replace("\r", "");
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.ok.ru/");
            FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>()
        {
        { "application_key", OKModuleController.OKApplicationKey },
        { "method", "mediatopic.post" },
        { "gid", this.groupId },
        { "type", "GROUP_THEME" },
        { "attachment", "{\"media\": [{\"type\": \"text\", \"text\": \"" + message + "\"}]}" },
        { "format", "json" },
        { "access_token", OKModuleController.OKAccessToken }
        });
            string result = httpClient.PostAsync("/fb.do", (HttpContent)content).Result.Content.ReadAsStringAsync().Result;
        }

        public string GetUploadURL()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.ok.ru/");
            FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>()
        {
        { "gid", this.groupId },
        { "application_key", OKModuleController.OKApplicationKey },
        { "method", "photosV2.getUploadUrl" },
        { "format", "json" },
        { "access_token", OKModuleController.OKAccessToken }
        });
            return JsonConvert.DeserializeObject<PhotoIdes>(httpClient.PostAsync("/fb.do", (HttpContent)content).Result.Content.ReadAsStringAsync().Result).upload_url;
        }

        public void UploadByURL(string[] imagePaths, string message)
        {
            HttpClient httpClient = new HttpClient();
            List<string> tokens = new List<string>();
            int num = 1;
            foreach (string imagePath in imagePaths)
            {
                try
                {
                    string uploadUrl = this.GetUploadURL();
                    MultipartFormDataContent content1 = new MultipartFormDataContent();
                    byte[] content2 = File.ReadAllBytes(imagePath);
                    content1.Add((HttpContent)new ByteArrayContent(content2), string.Format("pic{0}", (object)num), Path.GetFileName(imagePath) ?? "");
                    HttpResponseMessage result = httpClient.PostAsync(uploadUrl, (HttpContent)content1).Result;
                    result.EnsureSuccessStatusCode();
                    ResponseData responseData = JsonConvert.DeserializeObject<ResponseData>(result.Content.ReadAsStringAsync().Result);
                    KeyValuePair<string, PhotoData> keyValuePair = responseData.photos.First<KeyValuePair<string, PhotoData>>();
                    string key = keyValuePair.Key;
                    keyValuePair = responseData.photos.First<KeyValuePair<string, PhotoData>>();
                    string token = keyValuePair.Value.token;
                    tokens.Add(token);
                    ++num;
                }
                catch
                {
                
                }
            }
            try
            {
                this.CreateMediaTopic(tokens, message);
            }
            catch
            {
                
            }
        }

        private void CreateMediaTopic(List<string> tokens, string message)
        {
            message = message.Replace("\r", "");
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.ok.ru/");
            FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>()
        {
        { "application_key", OKModuleController.OKApplicationKey },
        { "method", "mediatopic.post" },
        { "gid", this.groupId },
        { "type", "GROUP_THEME" },
        { "attachment", JsonConvert.SerializeObject((object) new
        {
            media = new List<object>()
            {
                (object) new{ type = "text", text = message },
                (object) new
                {
                    type = "photo",
                    list = new List<object>((IEnumerable<object>) tokens.Select(id => new
                    {
                        id = id
                    }))
                }
            }
        }) },
        { "format", "json" },
        { "access_token", OKModuleController.OKAccessToken }
        });
            string result = httpClient.PostAsync("/fb.do", (HttpContent)content).Result.Content.ReadAsStringAsync().Result;
        }

        private string GetGroupIdByLink(string groupUrl)
        {
            string[] strArray = groupUrl.Split('/');
            return strArray.Length != 0 ? strArray[strArray.Length - 2].Replace("group", "") : (string)null;
        }

        public string GetGroupIdByUrl(string groupUrl)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.ok.ru/");
            FormUrlEncodedContent content = new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)new Dictionary<string, string>()
        {
        { "url", groupUrl },
        { "application_key", OKModuleController.OKApplicationKey },
        { "access_token", OKModuleController.OKAccessToken },
        { "method", "url.getInfo" },
        { "format", "json" }
        });
            return (string)JObject.Parse(httpClient.PostAsync("/fb.do", (HttpContent)content).Result.Content.ReadAsStringAsync().Result)["objectId"];
        }
    }
}
