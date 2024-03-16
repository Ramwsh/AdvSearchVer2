using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Windows.Threading;
using VK_Module.MVVM.View;
using VK_Module.Scripts;

namespace Services
{
    public class ImageLoaderService
    {
        private Advertisement advertisement;

        public ImageLoaderService(Advertisement adv)
        {
            advertisement = adv;
        }

        public List<string> GetLinks()
        {
            if (!string.IsNullOrEmpty(advertisement.ImageLinks))
            {
                var links = advertisement.ImageLinks.Split(";").ToList();
                links.RemoveAt(links.Count - 1);
                return links;
            }
            return null;
        }        

        public List<Image> LoadImages()
        {
            var imageLinks = GetLinks();
            List<Image> loadedImages = new List<Image>();

            if (imageLinks != null)
            {
                ProgressBarWindow progressBar = new ProgressBarWindow();
                progressBar.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                progressBar.ProgressBar.Value = 0;
                progressBar.ProgressBar.Minimum = 0;
                progressBar.ProgressBar.Maximum = imageLinks.Count;
                progressBar.Show();
                foreach (var imageLink in imageLinks)
                {
                    try
                    {
                        using (HttpClient client = new HttpClient())
                        {
                            using (HttpResponseMessage response = client.GetAsync(imageLink).Result)
                            {
                                if (response.IsSuccessStatusCode)
                                {
                                    using (Stream stream = response.Content.ReadAsStreamAsync().Result)
                                    {
                                        var image = Image.FromStream(stream);
                                        loadedImages.Add(image);
                                        progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }                    
                }
                progressBar.Close();
                return loadedImages;
            }            
            return null;            
        }
    }
}
