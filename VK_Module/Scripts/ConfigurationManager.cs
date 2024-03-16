using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VK_Module.Config
{
    public class ConfigurationManager
    {
        public static string _vkServAccToken { get; private set; }
        
        public static string _vkAccToken { get; private set; }
        
        public static string _vkVersion = "5.154";

        public static List<string> _vkGrpsToUpload = new List<string>();

        private string configContent;

        public static List<string> _OKGrpsToUpload { get; private set; } = new List<string>();
        
        public void SetAccessToken(string _servAccessToken, string _appAccessToken)
        {
            _vkServAccToken = _servAccessToken;
            _vkAccToken = _appAccessToken;

            configContent = $@"VKAccessToken={_vkAccToken}
VKServiceAccessToken={_vkServAccToken}
VKVersion={_vkVersion}";

            CreateVKConfiguration();
        }

        private void CreateVKConfiguration()
        {
            string path = $@"{Environment.CurrentDirectory}\Конфигурация\VK_Конфигурация.txt";

            if (!Directory.Exists($@"{Environment.CurrentDirectory}\Конфигурация"))
            {
                Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Конфигурация");
            }

            if (!File.Exists(path))
            {
                File.WriteAllText(path, configContent);
            }
            else
            {
                File.Delete(path);
                File.WriteAllText(path, configContent);
            }
            InitializeVKSettings();
        }

        public static void InitializeVKSettings()
        {
            string path = $@"{Environment.CurrentDirectory}\Конфигурация\VK_Конфигурация.txt";
            if (File.Exists(path))
            {
                string[] textSplitted = File.ReadAllText(path).Split('\n').ToArray();
                _vkAccToken = textSplitted[0].Substring(textSplitted[0].IndexOf('=') + 1).Replace("\r", "");
                _vkServAccToken = textSplitted[1].Substring(textSplitted[1].IndexOf('=') + 1).Replace("\r", "");
            }
        }

        public static void InitializeVKGrpsToUpload(string pathGrpstoUpload)
        {
            string txtRead = File.ReadAllText(pathGrpstoUpload);
            _vkGrpsToUpload = txtRead.Split('\n').ToList();
        }
    }
}
