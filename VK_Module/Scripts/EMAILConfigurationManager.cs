using System;
using System.IO;
using System.Linq;

namespace VK_Module.Scripts
{
    public class EMAILConfigurationManager
    {
        // email отправителя
        public static string _email { get; private set; }
        // smpt пароль почты отправителя
        public static string _smptemailpassword { get; private set; }

        private string configContent;

        public void SetAccessToken(string email, string smptemailpassword)
        {
            _email = email;
            _smptemailpassword = smptemailpassword;

            configContent = $@"SenderEmail={_email}
SMTPPassword={_smptemailpassword}";

            CreateEmailConfiguration();
        }

        // Сделать конфигурационный файл
        private void CreateEmailConfiguration()
        {
            string path = $@"{Environment.CurrentDirectory}\Конфигурация\EMAIL_Конфигурация.txt";

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
            InitializeEMAILSettings();
        }

        // проинициализировать конфигурационный файл
        public static void InitializeEMAILSettings()
        {
            string path = $@"{Environment.CurrentDirectory}\Конфигурация\EMAIL_Конфигурация.txt";
            if (File.Exists(path))
            {
                string[] textSplitted = File.ReadAllText(path).Split('\n').ToArray();
                _email = textSplitted[0].Substring(textSplitted[0].IndexOf('=') + 1).Replace("\r", "");
                _smptemailpassword = textSplitted[1].Substring(textSplitted[1].IndexOf('=') + 1).Replace("\r", "");
            }
        }
    }
}
