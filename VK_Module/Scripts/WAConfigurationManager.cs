using System;
using System.IO;
using System.Linq;

namespace VK_Module.Scripts
{
    public class WAConfigurationManager
    {        
        public static string _idInstance { get; private set; }
        public static string _whatsappApiInstance { get; private set; }

        private string configContent;

        public void SetAccessToken(string idInstance, string whatsappApiInstance)
        {
            _idInstance = idInstance;
            _whatsappApiInstance = whatsappApiInstance;

            configContent = $@"WAIdInstance={_idInstance}
WAIdInstanceApi={_whatsappApiInstance}";

            CreateWAConfiguration();
        }

        // Сделать конфигурационный файл
        private void CreateWAConfiguration()
        {
            string path = $@"{Environment.CurrentDirectory}\Конфигурация\WA_Конфигурация.txt";

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
            InitializeWASettings();
        }

        // проинициализировать конфигурационный файл
        public static void InitializeWASettings()
        {
            string path = $@"{Environment.CurrentDirectory}\Конфигурация\WA_Конфигурация.txt";
            if (File.Exists(path))
            {
                string[] textSplitted = File.ReadAllText(path).Split('\n').ToArray();
                _idInstance = textSplitted[0].Substring(textSplitted[0].IndexOf('=') + 1).Replace("\r", "");
                _whatsappApiInstance = textSplitted[1].Substring(textSplitted[1].IndexOf('=') + 1).Replace("\r", "");
            }
        }
    }
}
