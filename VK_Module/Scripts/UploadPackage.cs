using System.Collections.Generic;
using System.IO;

namespace Scripts
{
    public class UploadPackage
    {
        private List<string> imagePaths;
        private string content;

        public UploadPackage()
        {
            imagePaths = new List<string>();
        }

        public UploadPackage AttachImages(string imagePath)
        {
            if (!string.IsNullOrEmpty(imagePath))
            {                
                imagePaths.Add(imagePath);
            }
            return this;
        }        

        public UploadPackage AttachTextContent(string textPath)
        {
            if (!string.IsNullOrEmpty(textPath))
            {
                content = File.ReadAllText(textPath);
            }            
            return this;
        }

        public List<string> GetImagePaths()
        {
            return imagePaths;
        }

        public string GetContent()
        {
            return content;
        }
    }
}
