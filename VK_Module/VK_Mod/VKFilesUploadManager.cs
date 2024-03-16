using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VK_Module.Config;
using System.Net.Http.Headers;
using System.Net.Http;
using System.IO;
using VkNet;
using VkNet.Model;


namespace VK_Module.VK_Mod
{
    public class VKFilesUploadManager
    {        
        private string _response;
        private HttpResponseMessage _Uresponse;
        private string _Uresult;               
        
        public async Task PushOnServer(string content, string[] imgpaths, int vkGroupUrl)
        {
            var api = new VkApi();
            api.Authorize(new ApiAuthParams { AccessToken = ConfigurationManager._vkAccToken });

            var photoAttachments = new List<MediaAttachment>();

            foreach (string img in imgpaths)
            {
                var uploadServer = api.Photo.GetWallUploadServer(vkGroupUrl);
                while(true)
                {
                    string response = UploadFile(uploadServer.UploadUrl, img, "jpg").Result;
                    if (!string.IsNullOrEmpty(response))
                    {
                        _response = response;
                        break;
                    }
                }                
                var photo = api.Photo.SaveWallPhoto(_response, (ulong?)api.UserId.Value, (ulong)vkGroupUrl);
                photoAttachments.Add(photo[0]);
            }

            var postID = api.Wall.Post(new WallPostParams
            {
                OwnerId = -vkGroupUrl,
                Attachments = photoAttachments,
                Message = content
            });
            api.Dispose();            
        }       
        
        public void PushTextPost(string content, int vkGroupUrl)
        {
            var api = new VkApi();
            api.Authorize(new ApiAuthParams { AccessToken = ConfigurationManager._vkAccToken });

            var photoAttachments = new List<MediaAttachment>();

            var postID = api.Wall.Post(new WallPostParams
            {
                OwnerId = -vkGroupUrl,
                Message = content
            });
            api.Dispose();            
        }
        
        private async Task<string> UploadFile(string serverUrl, string file, string fileExtension)
        {
            var data = GetBytes(file);

            using (var client = new HttpClient())
            {
                var requestContent = new MultipartFormDataContent();
                var content = new ByteArrayContent(data);
                content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                requestContent.Add(content, "file", $"file.{fileExtension}");

                while(true)
                {
                    var response = client.PostAsync(serverUrl, requestContent).Result;
                    if (response != null)
                    {
                        _Uresponse = response;
                        break;
                    }
                }
                
                while(true)
                {
                    byte[] _Udata = _Uresponse.Content.ReadAsByteArrayAsync().Result;
                    if (_Udata != null)
                    {
                        _Uresult = Encoding.Default.GetString(_Udata);
                        break;
                    }
                }
                client.Dispose();
                return _Uresult;
            }
        }
        
        private byte[] GetBytes(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }    
}
