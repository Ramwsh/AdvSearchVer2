using System.Windows;
using System.Windows.Input;
using VK_Module.Config;
using VK_Module.OK_Mod;
using System.IO;
using System;
using VK_Module.Scripts;

namespace VK_Module
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {      
        public MainWindow()
        {
            InitializeComponent();

            ConfigurationManager.InitializeVKSettings();
            OKModuleController.InitializeOKSettings();
            EMAILConfigurationManager.InitializeEMAILSettings();
            WAConfigurationManager.InitializeWASettings();

            CheckConfigFileExists($@"{Environment.CurrentDirectory}\Конфигурация\VK_Конфигурация.txt",
                "ВКонтакте");
            CheckConfigFileExists($@"{Environment.CurrentDirectory}\Конфигурация\OK_Конфигурация.txt",
                "Одноклассники");
            CheckConfigFileExists($@"{Environment.CurrentDirectory}\Конфигурация\EMAIL_Конфигурация.txt",
                "EMAIL");
            CheckConfigFileExists($@"{Environment.CurrentDirectory}\Конфигурация\WA_Конфигурация.txt",
                "WhatsApp");
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Application_Quit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MovingWindow(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void CheckConfigFileExists(string path, string modulename)
        {
            if (File.Exists(path))
            {
                
            }
            else
            {
                MessageBox.Show($"Конфигурация {modulename} не существует.\nНастройте приложение", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
