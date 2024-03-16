using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace VK_Module.OK_Mod
{
    class OKModuleController
    {
        public static string OKAccessToken { get; private set; }
        public static string OKApplicationKey { get; private set; }

        private string configContent;

        public static List<string> _OKGrpsToUpload { get; private set; } = new List<string>();
       
        public void SetAccessToken(string _accessToken, string _applicationKey)
        {
            OKAccessToken = _accessToken;
            OKApplicationKey = _applicationKey;

            configContent = $@"OKAccessToken={OKAccessToken}
OKApplicationKey={OKApplicationKey}";

            CreateOKConfiguration();
        }
        
        private void CreateOKConfiguration()
        {
            string path = $@"{Environment.CurrentDirectory}\Конфигурация\OK_Конфигурация.txt";

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
            InitializeOKSettings();
        }
        
        public static void InitializeOKSettings()
        {
            string path = $@"{Environment.CurrentDirectory}\Конфигурация\OK_Конфигурация.txt";
            if (File.Exists(path))
            {
                string[] textSplitted = File.ReadAllText(path).Split('\n').ToArray();
                OKAccessToken = textSplitted[0].Substring(textSplitted[0].IndexOf('=') + 1).Replace("\r", "");
                OKApplicationKey = textSplitted[1].Substring(textSplitted[1].IndexOf('=') + 1).Replace("\r", "");
            }
        }

        public static void InitializeOkGrpsToUpload(string pathGrpstoUpload)
        {
            string txtRead = File.ReadAllText(pathGrpstoUpload);
            _OKGrpsToUpload = txtRead.Split('\n').ToList();            
        }
    }
}
