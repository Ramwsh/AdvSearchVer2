using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using VK_Module.Config;
using VK_Module.OK_Mod;
using VK_Module.Scripts;

namespace VK_Module.MVVM.View
{    
    public partial class ConfigurationView : UserControl
    {        
        public ConfigurationView()
        {
            InitializeComponent();            
            if (!string.IsNullOrEmpty(ConfigurationManager._vkServAccToken))
            {
                VKServiceAccesTokenTextBox.Text = ConfigurationManager._vkServAccToken;
            }
            if (!string.IsNullOrEmpty(ConfigurationManager._vkAccToken))
            {
                VKAccesTokenTextBox.Text = ConfigurationManager._vkAccToken;
            }                     
            if (!string.IsNullOrEmpty(EMAILConfigurationManager._email))
            {
                EmailTextBox.Text = EMAILConfigurationManager._email;
            }
            if (!string.IsNullOrEmpty(EMAILConfigurationManager._smptemailpassword))
            {
                SMPTPasswordTextBox.Text = EMAILConfigurationManager._smptemailpassword;
            }
            if (!string.IsNullOrEmpty(WAConfigurationManager._idInstance))
            {
                WAInstanceIDTextBox.Text = WAConfigurationManager._idInstance;
            }
            if (!string.IsNullOrEmpty(WAConfigurationManager._whatsappApiInstance))
            {
                WAApiTextBox.Text = WAConfigurationManager._whatsappApiInstance;
            }
            if (!string.IsNullOrEmpty(OKModuleController.OKAccessToken))
            {
                OKAccesTokenTextBox.Text = OKModuleController.OKAccessToken;
            }
            if (!string.IsNullOrEmpty(OKModuleController.OKApplicationKey))
            {
                OKApplicationKeyTextBox.Text = OKModuleController.OKApplicationKey;
            }
        }

        private void EMAILApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text != "" && EmailTextBox.Text != "Почта отправителя" && SMPTPasswordTextBox.Text != "" && SMPTPasswordTextBox.Text != "SMPT пароль почты отправителя")
            {
                EMAILConfigurationManager EMAILConfigManager = new EMAILConfigurationManager();
                EMAILConfigManager.SetAccessToken(EmailTextBox.Text, SMPTPasswordTextBox.Text);

                MessageBox.Show("Конфигурация отправки объявлений по EMAIL создана", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void WAApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (WAApiTextBox.Text != "" && WAApiTextBox.Text != "API токен WhatsApp" && WAInstanceIDTextBox.Text != "" && WAInstanceIDTextBox.Text != "ID инстанса Green API")
            {
                WAConfigurationManager WAConfigManager = new WAConfigurationManager();
                WAConfigManager.SetAccessToken(WAApiTextBox.Text, WAInstanceIDTextBox.Text);

                MessageBox.Show("Конфигурация отправки объявлений по WhatsApp создана", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void VKApplyBtnClick(object sender, RoutedEventArgs e)
        {
            if (VKServiceAccesTokenTextBox.Text != "" && VKServiceAccesTokenTextBox.Text != "Токен сервисного доступа приложения ВКонтакте" && VKAccesTokenTextBox.Text != "" && VKAccesTokenTextBox.Text != "Токен доступа приложения ВКонтакте")
            {
                ConfigurationManager vkConfigManager = new ConfigurationManager();
                vkConfigManager.SetAccessToken(VKServiceAccesTokenTextBox.Text, VKAccesTokenTextBox.Text);

                MessageBox.Show("Конфигурация ВКонтакте создана", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void GetAccesTokenBtnClick(object sender, RoutedEventArgs e)
        {
            string url = "https://oauth.vk.com/authorize?client_id=6178269&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=friends,notify,photos,wall,email,mail,groups,stats,offline&response_type=token&v=5.154";

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true
            };
            Process.Start(startInfo);
        }

        private void VKAccesTokenTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (VKAccesTokenTextBox.Text == "Токен доступа приложения ВКонтакте")
            {
                VKAccesTokenTextBox.Text = "";
            }
        }
        
        private void VKAccessTokenTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (VKAccesTokenTextBox.Text == "")
            {
                VKAccesTokenTextBox.Text = "Токен доступа приложения ВКонтакте";
            }
        }

        private void VKServiceAccesTokenTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (VKServiceAccesTokenTextBox.Text == "Токен сервисного доступа приложения ВКонтакте")
            {
                VKServiceAccesTokenTextBox.Text = "";
            }
        }

        private void VKServiceAccesTokenTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (VKServiceAccesTokenTextBox.Text == "")
            {
                VKServiceAccesTokenTextBox.Text = "Токен сервисного доступа приложения ВКонтакте";
            }
        }

        private void EmailTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "Почта отправителя")
            {
                EmailTextBox.Text = "";
            }
        }

        private void EmailTextBoxClickLostFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "")
            {
                EmailTextBox.Text = "Почта отправителя";
            }
        }

        private void SMTPTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (SMPTPasswordTextBox.Text == "SMPT пароль почты отправителя")
            {
                SMPTPasswordTextBox.Text = "";
            }
        }

        private void SMTPTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (SMPTPasswordTextBox.Text == "")
            {
                SMPTPasswordTextBox.Text = "SMPT пароль почты отправителя";
            }
        }

        private void WAInstanceIDTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (WAInstanceIDTextBox.Text == "ID инстанса Green API")
            {
                WAInstanceIDTextBox.Text = "";
            }
        }

        private void WAInstanceIDTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if ( WAInstanceIDTextBox.Text == "")
            {
                WAInstanceIDTextBox.Text = "ID инстанса Green API";
            }
        }

        private void WAApiTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (WAApiTextBox.Text == "API токен WhatsApp")
            {
                WAApiTextBox.Text = "";
            }
        }

        private void WAApiTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (WAApiTextBox.Text == "")
            {
                WAApiTextBox.Text = "API токен WhatsApp";
            }
        }        

        private void OKApplyButtonClick(object sender, RoutedEventArgs e)
        {
            if (OKAccesTokenTextBox.Text != "" && OKAccesTokenTextBox.Text != "Токен приложения Одноклассники")
            {
                OKModuleController OKconfigcreater = new OKModuleController();
                OKconfigcreater.SetAccessToken(OKAccesTokenTextBox.Text, OKApplicationKeyTextBox.Text);

                MessageBox.Show("Конфигурация одноклассников создана", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }            
        }

        private void OKAccesTokenTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (OKAccesTokenTextBox.Text == "Токен приложения Одноклассники")
            {
                OKAccesTokenTextBox.Text = "";
            }    
        }

        private void OKAccesTokenTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (OKAccesTokenTextBox.Text == "")
            {
                OKAccesTokenTextBox.Text = "Токен приложения Одноклассники";
            }
        }

        private void OKApplicationKeyTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (OKApplicationKeyTextBox.Text == "Публичный ключ")
            {
                OKApplicationKeyTextBox.Text = "";
            }
        }

        private void OKApplicationKeyTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (OKApplicationKeyTextBox.Text == "")
            {
                OKApplicationKeyTextBox.Text = "Публичный ключ";
            }
        }
    }
}
