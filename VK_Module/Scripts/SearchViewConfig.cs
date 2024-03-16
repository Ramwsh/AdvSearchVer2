using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace VK_Module.Scripts
{
    class SearchViewConfig
    {
        public static string FilesRootDirectory;

        public string AdvName { get; set; }
        public string AdvPath { get; set; }

        public SearchViewConfig()
        {
            
        }

        private SearchViewConfig(string AdvPath)
        {
            this.AdvPath = AdvPath;
            AdvName = GetAdvName(AdvPath);
        }

        public List<SearchViewConfig> GetAdvDirNames()
        {
            List<SearchViewConfig> advertisementPaths = new List<SearchViewConfig>();
            string[] dirNames;
            if (!string.IsNullOrEmpty(FilesRootDirectory))
            {
                dirNames = Directory.GetDirectories(FilesRootDirectory);                
                foreach (var dirName in dirNames)
                {
                    advertisementPaths.Add(new SearchViewConfig(dirName));
                }
            }                
            return advertisementPaths;
        }

        private string GetAdvName(string AdvPath)
        {
            string AdvName = AdvPath.Split('\\').Last();
            AdvName = AdvName.Replace("Тип", "");
            return AdvName;
        }        
    }
}
