using System.Windows;
using System.Windows.Input;
using VK_Module.Scripts;
using Winforms = System.Windows.Forms;
using System.ComponentModel;
using Services;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using Scripts;
using System.Windows.Forms;

namespace VK_Module.MVVM.View
{    
    public partial class AdvertisementView : Window, INotifyPropertyChanged
    {
        private Advertisement advertisement;
        private AdvertisementRepository repository;
        private List<string> images;
        private string defaultText;

        public Advertisement Advertisement
        {
            get { return advertisement; }
            set { OnPropertyChanged(nameof(advertisement)); }
        }

        public List<string> Images
        {
            get { return images; }
            set { OnPropertyChanged(nameof(images)); }
        }

        #region Property Change realisation

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
        
        public AdvertisementView(Advertisement advertisement, AdvertisementRepository repository)
        {            
            InitializeComponent();            
            if (advertisement != null)
            {
                this.advertisement = advertisement;
            }
            if (repository != null)
            {
                this.repository = repository;
                CurrentRepositoryTextBox.Text = repository.Tag;
            }                        
            DataContext = this;
            ImageLoaderService service = new ImageLoaderService(advertisement);
            images = service.GetLinks();
            defaultText = advertisement.Description;
        }

        #region Window events
        private void MovingWindow(object sender, RoutedEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void Application_Quit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        #endregion

        private void SendEmailButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(EMAILConfigurationManager._email) && !string.IsNullOrEmpty(EMAILConfigurationManager._smptemailpassword))
                {
                    string subject = EmailSubjectTextBox.Text;
                    string recipientMail = EmailTextBox.Text;
                    string content = AdvContentTextBox.Text;

                    //Thread sendEmailThread = new Thread(() => {
                    //    EmailSender emailSender = new EmailSender();
                    //    emailSender.SendMessage(recipientMail, subject, content, ImagesList);
                    //});
                    //sendEmailThread.Start();
                    Winforms.MessageBox.Show($"EMAIL Сообщение на почту отправлено {recipientMail}.", "Уведомление",
                    Winforms.MessageBoxButtons.OK, Winforms.MessageBoxIcon.Information);
                }
                else
                {
                    Winforms.MessageBox.Show($"Конфигурация для отправки EMAIL не настроена.", "Ошибка",
                    Winforms.MessageBoxButtons.OK, Winforms.MessageBoxIcon.Error);
                }
            }
            catch
            {

            }            
        }

        #region WhatsApp, Email textboxes events

        private void SubjectTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (EmailSubjectTextBox.Text == "Тема письма")
            {
                EmailSubjectTextBox.Text = "";
            }
        }
        
        private void SubjectTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (EmailSubjectTextBox.Text == "")
            {
                EmailSubjectTextBox.Text = "Тема письма";
            }
        }

        private void EmailTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "E-MAIL")
            {
                EmailTextBox.Text = "";
            }
        }

        private void EmailTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (EmailTextBox.Text == "")
            {
                EmailTextBox.Text = "E-MAIL";
            }
        }

        private void WhatsAppTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (WhatsAppTextBox.Text == "Номер")
            {
                WhatsAppTextBox.Text = "";
            }
        }

        private void WhatsAppTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (WhatsAppTextBox.Text == "")
            {
                WhatsAppTextBox.Text = "Номер";
            }
        }
        #endregion

        private void SendWATextMessage(object sender, RoutedEventArgs e)
        {
            UploadPackageRepository packageRepository = new UploadPackageRepository();
            packageRepository.SelectAdvertisementDirecotires();
            if (packageRepository.GetPackages().Count > 0)
            {
                var packageRepositories = packageRepository.GetPackages();
                foreach (var package in packageRepositories)
                {
                    var ImagesList = package.GetImagePaths();
                    var Content = package.GetContent();
                    try
                    {
                        if (!string.IsNullOrEmpty(WAConfigurationManager._idInstance) && !string.IsNullOrEmpty(WAConfigurationManager._whatsappApiInstance))
                        {
                            WhatappSender WAsender = new WhatappSender();
                            string caption = "";
                            int maxCounter = -1;
                            if (ImagesList != null)
                            {
                                maxCounter = ImagesList.Count;
                            }
                            int Counter = 1;
                            if (ImagesList != null)
                            {
                                foreach (var image in ImagesList)
                                {
                                    if (Counter == maxCounter)
                                    {
                                        caption = AdvContentTextBox.Text;
                                        WAsender.SendImage(WhatsAppTextBox.Text, image, caption);
                                        break;
                                    }
                                    WAsender.SendImage(WhatsAppTextBox.Text, image, caption);
                                    Counter++;
                                }
                            }
                            else
                            {
                                WAsender.SendTextMessage(WhatsAppTextBox.Text.Replace("\r", ""), AdvContentTextBox.Text.Replace("\n", "").Trim());
                            }
                            Winforms.MessageBox.Show($"WhatsApp Сообщение на номер отправлено {WhatsAppTextBox.Text}.", "Уведомление",
                            Winforms.MessageBoxButtons.OK, Winforms.MessageBoxIcon.Information);
                        }
                        else
                        {
                            Winforms.MessageBox.Show($"Конфигурация для отправки WhatsApp не настроена.", "Ошибка",
                            Winforms.MessageBoxButtons.OK, Winforms.MessageBoxIcon.Error);
                        }
                    }
                    catch
                    {

                    }
                }                
            }            
        }

        private void LinkTextBlockClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(advertisement.Link))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = advertisement.Link,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }    
        }

        public event EventHandler AdvertisementChanged;
        public event EventHandler<Advertisement> AdvertisementRemoved;

        private void SaveAdvertisementButtonClick(object sender, RoutedEventArgs e)
        {
            if (defaultText != AdvContentTextBox.Text)
            {
                HomepageService homepageService = new HomepageService();
                advertisement.State = "Изменено";                
                homepageService.UpdateAdvertisementById(advertisement);
                AdvertisementChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            HomepageService homepageService = new HomepageService();
            homepageService.RemoveAdvertisementById(advertisement);   
            AdvertisementRemoved?.Invoke(this, advertisement);            
            ErrorView notifyView = new ErrorView();
            notifyView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            notifyView.ErrorText.Text = "Объявление удалено из базы";
            notifyView.Show();
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            if (repository != null)
            {
                AdvertisementFileService AFservice = new AdvertisementFileService();
                ImageLoaderService ILservice = new ImageLoaderService(advertisement);
                TextLoaderService TLservice = new TextLoaderService(advertisement);
                var advertisementImages = ILservice.LoadImages();
                if (advertisementImages != null && advertisementImages.Count != 0)
                {
                    AFservice = AFservice.AttachImages(advertisementImages);
                }
                var advertisementDescription = TLservice.LoadTextData();
                if (!string.IsNullOrEmpty(advertisementDescription))
                {
                    AFservice = AFservice.AttachDescription(advertisementDescription);
                }
                AFservice.CreateAdvertisementFile(repository, advertisement);

                ErrorView notifyView = new ErrorView();
                notifyView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                notifyView.ErrorText.Text = "Сохранено в репозиторий";
                notifyView.Show();
            }            
        }
    }
}
