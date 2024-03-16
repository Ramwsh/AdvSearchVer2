using System;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Net;

namespace VK_Module.Scripts
{
    public class WhatappSender
    {        
        public void SendImage(string phone, string imagefilepath, string caption)
        {   
            try
            {
                phone += "@c.us";
                string filePath = imagefilepath;

                var httpClient = new HttpClient();
                var requestUrl = $"https://api.green-api.com/waInstance{WAConfigurationManager._whatsappApiInstance}/sendFileByUpload/{WAConfigurationManager._idInstance}";

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(phone), "chatId");
                formData.Add(new StringContent(caption), "caption");
                formData.Add(new StreamContent(File.OpenRead(filePath)), "file", "file.jpg");
                var response = httpClient.PostAsync(requestUrl, formData).Result;
                var responseBody = response.Content.ReadAsStringAsync().Result;
            }
            catch
            {

            }           
        }

        public async void SendTextMessage(string phone, string text)
        {            
            try
            {
                string jsonBody = string.Format("{{ \"chatId\": \"{0}@c.us\", \"message\": \"{1}\" }}", phone, text);
                string url = $"https://api.green-api.com/waInstance{WAConfigurationManager._whatsappApiInstance}/sendMessage/{WAConfigurationManager._idInstance}";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] data = Encoding.UTF8.GetBytes(jsonBody);
                using (var stream = request.GetRequestStreamAsync().Result)
                {
                    stream.Write(data, 0, data.Length);
                }
                request.GetResponse().Close();
                var response = request.GetResponse();
                var responseBody = response.ResponseUri.ToString();
            }
            catch
            {

            }
        }
    }
}
