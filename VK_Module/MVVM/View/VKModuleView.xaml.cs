using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Services;
using System.Windows.Media;
using VK_Module.MVVM.View.InputViews;
using VK_Module.Databases.VKGroupsLoadDbModel;
using VK_Module.Databases.NameFilterDbModel;
using VK_Module.Databases.VKGroupsUploadDbModel;
using Scripts;

namespace VK_Module.MVVM.View
{    
    public partial class VKModuleView : UserControl
    {                             
        private VKGroupsLoadDbModel groupsLoadDatabase;
        private NameFilterDbModel nameFilterDatabase;
        private VKGroupsUploadDbModel groupsUploadDatabase;

        public bool _UseNameFilter;
        public bool _UseDateFilter;

        private string dateFrom;
        private string dateTo;

        private int advertisementCount;
        
        public List<string> VKGroups = null;
        public List<string> NamesFilter = null;
        public List<string> VKGroupsUpload = null;

        public VKModuleView()
        {            
            groupsLoadDatabase = new VKGroupsLoadDbModel(@"Databases\VKgroupsLoadDb.db");            
            nameFilterDatabase = new NameFilterDbModel(@"Databases\NameFilterDb.db");
            groupsUploadDatabase = new VKGroupsUploadDbModel(@"Databases\VKgroupsUploadDb.db");            
            VKGroups = groupsLoadDatabase.GetAllGroupLinks();
            NamesFilter = nameFilterDatabase.GetAllNames();
            VKGroupsUpload = groupsUploadDatabase.GetAllGroupLinks();
            InitializeComponent();
            UpdateGroupLinks(VKGroups);            
            UpdateNamesList(NamesFilter);
            UpdateGroupsUpload(VKGroupsUpload);
            _UseDateFilter = false;
            _UseNameFilter = false;                        
        }

        #region From Date Events
        private void FromDateTextBoxTextChanged(object sender, RoutedEventArgs e)
        {
            dateFrom = null;
            if (!string.IsNullOrEmpty(FromDateTextBox.Text) && FromDateTextBox.Text != "Дата от ДД/ММ/ГГГГ")
            {
                dateFrom = FromDateTextBox.Text;
            }
        }

        private void FromDateTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (FromDateTextBox.Text == "")
            {
                FromDateTextBox.Text = "Дата от ДД/ММ/ГГГГ";
                dateFrom = null;
            }
        }

        private void FromDateTextBoxLMClick(object sender, RoutedEventArgs e)
        {
            if (FromDateTextBox.Text == "Дата от ДД/ММ/ГГГГ")
            {
                FromDateTextBox.Text = "";
                dateFrom = null;
            }
        }
        #endregion

        #region To Date Events
        private void ToDateTextBoxTextChanged(object sender, RoutedEventArgs e)
        {
            dateTo = null;
            if (!string.IsNullOrEmpty(ToDateTextBox.Text) && ToDateTextBox.Text != "Дата до ДД/ММ/ГГГГ")
            {
                dateTo = ToDateTextBox.Text;
            }
        }

        private void ToDateTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (ToDateTextBox.Text == "")
            {
                ToDateTextBox.Text = "Дата до ДД/ММ/ГГГГ";
                dateTo = null;
            }
        }

        private void ToDateTextBoxLMClick(object sender, RoutedEventArgs e)
        {
            if (ToDateTextBox.Text == "Дата до ДД/ММ/ГГГГ")
            {
                ToDateTextBox.Text = "";
                dateTo = null;
            }
        }
        #endregion

        #region Advertisement Count events
        private void AdvertisementCountTextChanged(object sender, RoutedEventArgs e)
        {
            advertisementCount = 100;
            if (!string.IsNullOrEmpty(AdvertisementCountTextBox.Text) && AdvertisementCountTextBox.Text != "Желаемое число объявлений")
            {
                advertisementCount = Convert.ToInt32(AdvertisementCountTextBox.Text);
            }
        }

        private void AdvertisementCountLostFocus(object sender, RoutedEventArgs e)
        {
            if (AdvertisementCountTextBox.Text == "")
            {
                AdvertisementCountTextBox.Text = "Желаемое число объявлений";
                advertisementCount = 100;
            }
        }

        private void AdvertisementCountLMClick(object sender, RoutedEventArgs e)
        {
            if (AdvertisementCountTextBox.Text == "Желаемое число объявлений")
            {
                AdvertisementCountTextBox.Text = "";
                advertisementCount = 100;
            }
        }
        #endregion

        #region Filter button events
        private void UseNameFilterClick(object sender, RoutedEventArgs e)
        {
            if (_UseNameFilter == false)
            {
                NamesFilter = nameFilterDatabase.GetAllNames();
                _UseNameFilter = true;
                UseNameBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#583CB2"));
            }
            else
            {
                NamesFilter = null;
                _UseNameFilter = false;
                UseNameBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#B7A3FF"));
            }
        }

        private void UseDateFilterClick(object sender, RoutedEventArgs e)
        {
            if (_UseDateFilter == false)
            {                
                if (!string.IsNullOrEmpty(FromDateTextBox.Text))
                {
                    dateFrom = FromDateTextBox.Text;
                }
                if (!string.IsNullOrEmpty(ToDateTextBox.Text))
                {
                    dateTo = ToDateTextBox.Text;
                }
                _UseDateFilter = true;                
                UseDateBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#583CB2"));
            }
            else
            {
                dateFrom = string.Empty;
                dateTo = string.Empty;
                _UseDateFilter = false;                
                UseDateBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#B7A3FF"));
            }
        }
        #endregion

        #region Select button events

        private void SelectNamesFilterClick(object sender, RoutedEventArgs e)
        {
            OpenNamesListInputView();
        }

        private void SelectGroupLinksLoadClick(object sender, RoutedEventArgs e)
        {
            OpenGroupLinksInputView();
        }         
        
        private void SelectGroupLinksUploadClick(object sender, RoutedEventArgs e)
        {
            OpenGroupLinksUploadInputView();
        }

        private void OpenNamesListInputView()
        {
            NamesFIlterInputView inputView = new NamesFIlterInputView();
            inputView.CurrentNamesUpdated += InputView_CurrentNamesFilterUpdated;
            inputView.WindowStartupLocation = WindowStartupLocation.CenterScreen;            
            inputView.Show();
        }        

        private void OpenGroupLinksInputView()
        {
            VKGroupsLoadInputView inputView = new VKGroupsLoadInputView();
            inputView.CurrentGroupsUpdated += InputView_CurrentVKGroupsUpdated;
            inputView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inputView.Show();
        }

        private void OpenGroupLinksUploadInputView()
        {
            VKGroupsUploadInputView inputView = new VKGroupsUploadInputView();
            inputView.CurrentGroupsUpdated += InputView_CurrentVKGroupsUploadUpdated;
            inputView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inputView.Show();
        }
        #endregion

        #region Current state of input data        

        public void UpdateGroupLinks(List<string> newGroupLinks)
        {
            VKGroups = newGroupLinks;
            {
                if (VKGroups.Count > 0)
                {
                    GroupsState.Foreground = new SolidColorBrush(Colors.ForestGreen);
                    GroupsState.Text = $"Состояние: Присутствуют ({VKGroups.Count})";
                }
                else
                {                    
                    GroupsState.Foreground = new SolidColorBrush(Colors.DarkViolet);
                    GroupsState.Text = $"Состояние: Отсутствуют ({VKGroups.Count})";
                }
            }
        }

        private void UpdateNamesList(List<string> newNames)
        {
            NamesFilter = newNames;
            if (NamesFilter.Count > 0)
            {
                NamesState.Foreground = new SolidColorBrush(Colors.ForestGreen);
                NamesState.Text = $"Состояние: Присутствуют ({NamesFilter.Count})";
            }
            else
            {
                NamesState.Foreground = new SolidColorBrush(Colors.DarkViolet);
                NamesState.Text = $"Состояние: Отсутствуют ({NamesFilter.Count})";
            }
        }      
        
        private void UpdateGroupsUpload(List<string> newGroupsUpload)
        {
            VKGroupsUpload = newGroupsUpload;
            if (VKGroupsUpload.Count > 0)
            {
                ExportLinksState.Foreground = new SolidColorBrush(Colors.ForestGreen);
                ExportLinksState.Text = $"Состояние: Присутствуют ({VKGroupsUpload.Count})";
            }
            else
            {
                ExportLinksState.Foreground = new SolidColorBrush(Colors.DarkViolet);
                ExportLinksState.Text = $"Состояние: Отсуствуют ({NamesFilter.Count})";
            }
        }

        private void InputView_CurrentNamesFilterUpdated(object sender, List<string> newNames)
        {
            UpdateNamesList(newNames);
        }

        private void InputView_CurrentVKGroupsUpdated(object sender, List<string> newGroupLinks)
        {
            UpdateGroupLinks(newGroupLinks);
        }        

        private void InputView_CurrentVKGroupsUploadUpdated(object sender, List<string> newGroupsUpload)
        {
            UpdateGroupsUpload(newGroupsUpload);
        }
        #endregion

        #region VK Client realisation
        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            if (VKGroups.Count > 0)
            {                
                LoadAdvertisements();             
            }            
        }

        private void LoadAdvertisements()
        {
            if (_UseNameFilter == false)
            {
                NamesFilter = null;
            }
            if (_UseDateFilter == false)
            {
                dateTo = null;
                dateFrom = null;
            }            

            VKService client = new VKService(VKGroups, NamesFilter, dateTo, dateFrom, advertisementCount);            
            client.GetAdvertisements();            
        }
        #endregion;                

        #region VK Uploading realisation

        public void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            UploadPackageRepository packageRepository = new UploadPackageRepository();
            packageRepository.SelectAdvertisementDirecotires();
            if (packageRepository.GetPackages().Count > 0)
            {
                VKUploadService uploadService = new VKUploadService();
                uploadService.AttachPackages(packageRepository.GetPackages());
                uploadService.AttachGroups(VKGroupsUpload);
                uploadService.UploadAdvertisements();
            }
        }

        #endregion
    }
}
