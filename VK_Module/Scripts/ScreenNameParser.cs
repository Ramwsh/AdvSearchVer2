using System;

namespace VK_Module.Scripts
{
    public class ScreenNameParser
    {        
        public static string GetScreenNameFromUrl(string url)
        {
            var uri = new Uri(url);
            string path = uri.AbsolutePath;
            return path.Trim('/');
        }
    }
}
