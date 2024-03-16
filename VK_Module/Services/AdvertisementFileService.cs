using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using VK_Module.Scripts;

namespace Services
{
    public class AdvertisementFileService
    {
        private string textDescription;
        private List<Image> images;
        
        public AdvertisementFileService AttachDescription(string textDescription)
        {
            this.textDescription = textDescription;
            return this;
        }

        public AdvertisementFileService AttachImages(List<Image> images)
        {
            this.images = images;
            return this;
        }

        public void CreateAdvertisementFile(AdvertisementRepository repository, Advertisement advertisement)
        {            
            if (Directory.Exists(repository.Path))
            {
                string advertisementPath = Path.Combine(repository.Path, advertisement.Id.ToString());
                Directory.CreateDirectory(advertisementPath);

                string textDirPath = Path.Combine(advertisementPath, "Текст");
                Directory.CreateDirectory(textDirPath);
                File.WriteAllText(Path.Combine(textDirPath, "Текст.txt"), textDescription);                

                int photoNum = 1;

                try
                {
                    if (images != null)
                    {
                        string photoDirPath = Path.Combine(advertisementPath, "Фото");
                        Directory.CreateDirectory(photoDirPath);

                        foreach (var image in images)
                        {
                            string photoFilePath = Path.Combine(photoDirPath, $"Фото_{photoNum}.jpg");
                            using (Bitmap bitmap = new Bitmap(image))
                            {
                                bitmap.Save(photoFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                                bitmap.Dispose();
                                photoNum++;
                            }
                        }
                    }
                }
                catch
                {

                }                
            }
        }
    }
}
