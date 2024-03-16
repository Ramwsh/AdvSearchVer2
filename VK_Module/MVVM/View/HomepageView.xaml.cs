using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using VK_Module.Scripts;
using Services;
using System.ComponentModel;
using VK_Module.MVVM.View.InputViews;
using System.Collections.ObjectModel;
using Scripts;
using System.Diagnostics;

namespace VK_Module.MVVM.View
{
    public partial class HomepageView : UserControl, INotifyPropertyChanged
    {        
        #region Variables
        
        private bool ShowOnlyUnchanged = false;
        private bool ShowOnlyChanged = false;
        private bool ShowOnlyWithPhotos = false;
        private HomepageService homepageService;
        private AdvertisementsCollectionFilterMode mode;
        private AdvertisementRepository selectedRepository;

        private string houseType;
        private string date;
        private string roomsCount;        

        #endregion

        #region Binding for Advertisements ListView items source        
        public ObservableCollection<Advertisement> Advertisements { get; set; }
        #endregion

        #region Binding for Advertisements Repository ListView items source
        public List<AdvertisementRepository> AdvertisementRepositories
        {
            get { return new AdvRepositoriesService().GetAllRepositories(); }
            set { OnPropertyChanged(nameof(AdvertisementRepositories)); }
        }        
        #endregion        

        public HomepageView()
        {
            InitializeComponent();
            homepageService = new HomepageService();
            DataContext = this;
            Advertisements = new ObservableCollection<Advertisement>(homepageService.GetAllAdvertisements());
            mode = new AdvertisementsCollectionFilterMode();
        }

        #region Property Change realisation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Advertisement Repositories Communication between forms
        private void OpenRepositoryListInputView(object sender, EventArgs e)
        {
            AdRepositoryInputView inputView = new AdRepositoryInputView();
            inputView.CurrentRepositoryChanged += InputView_RepositoriesListViewUpdated;
            inputView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inputView.Show();
        }

        private void InputView_RepositoriesListViewUpdated(object sender, List<AdvertisementRepository> newRepositories)
        {
            UpdateRepositoriesListView(newRepositories);
        }

        private void UpdateRepositoriesListView(List<AdvertisementRepository> newRepositories)
        {
            AdvertisementRepositories = newRepositories;
        }
        #endregion

        #region Clean database

        private void CleanDatabaseClick(object sender, RoutedEventArgs e)
        {
            homepageService.CleanDatabase();
            Advertisements.Clear();
            AdvertisementsListView.Items.Refresh();
            ICollectionView view = CollectionViewSource.GetDefaultView(AdvertisementsListView.ItemsSource);            
            view.Refresh();
        }

        #endregion                            

        #region Listview Items Button events       

        private void AdvertisementItemToSave(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            ListViewItem listViewItem = FindVisualParent<ListViewItem>(button);
            if (listViewItem != null)
            {                   
                Advertisement selectedAdvertisement = (Advertisement)listViewItem.DataContext;
                if (selectedRepository != null)
                {
                    AdvertisementFileService AFservice = new AdvertisementFileService();
                    ImageLoaderService ILservice = new ImageLoaderService(selectedAdvertisement);
                    TextLoaderService TLservice = new TextLoaderService(selectedAdvertisement);
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
                    AFservice.CreateAdvertisementFile(selectedRepository, selectedAdvertisement);

                    ErrorView notifyView = new ErrorView();
                    notifyView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    notifyView.ErrorText.Text = "Сохранено в репозиторий";
                    notifyView.Show();
                }
            }
        }

        private void AdvertisementItemToRemoveAdd(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;            
            ListViewItem listViewItem = FindVisualParent<ListViewItem>(button);            
            if (listViewItem != null)
            {                                
                Advertisement selectedAdvertisement = (Advertisement)listViewItem.DataContext;                                
                homepageService.RemoveAdvertisementById(selectedAdvertisement);
                Advertisements.Remove(selectedAdvertisement);
                AdvertisementsListView.Items.Refresh();
            }            
        }        

        private void AdvertisementToOpen(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            ListViewItem listViewItem = FindVisualParent<ListViewItem>(button);
            if (listViewItem != null)
            {
                Advertisement selectedAdvertisement = (Advertisement)listViewItem.DataContext;                
                AdvertisementView view = new AdvertisementView(selectedAdvertisement, selectedRepository);
                view.AdvertisementChanged += UpdateAdvertisementsListViewFromAdvertisementWindow;
                view.AdvertisementRemoved += RemoveAdvertisementsFromAdvertisementView;                
                view.WindowStartupLocation = WindowStartupLocation.CenterScreen;                
                view.Show();
            }
        }        

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObj = VisualTreeHelper.GetParent(child);

            if (parentObj == null)
                return null;

            if (parentObj is T parent)
                return parent;

            return FindVisualParent<T>(parentObj);
        }
        #endregion

        #region updateListViewEvents

        private void UpdateAdvertisementsListView()
        {            
            Advertisements = new ObservableCollection<Advertisement>(homepageService.GetAllAdvertisements());            
            AdvertisementsListView.Items.Refresh();
            ICollectionView view = CollectionViewSource.GetDefaultView(AdvertisementsListView.ItemsSource);
            view.Refresh();
        }

        private void RemoveAdvertisementsListView(Advertisement removedAdvertisement)
        {
            Advertisements.Remove(removedAdvertisement);
            AdvertisementsListView.Items.Refresh();
        }

        private void UpdateAdvertisementsListViewFromAdvertisementWindow(object sender, EventArgs e)
        {
            UpdateAdvertisementsListView();
        }

        private void RemoveAdvertisementsFromAdvertisementView(object sender, Advertisement removedAdvertisement)
        {
            RemoveAdvertisementsListView(removedAdvertisement);
        }

        #endregion

        #region Show only "Загружено"

        private void ShowOnlyUnchangedButtonClick(object sender, RoutedEventArgs e)
        {
            if (ShowOnlyUnchanged == false)
            {
                ShowOnlyUnchanged = true;
                ShowOnlyUnchangedBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#E6D9FF"));
                ShowOnlyUnchangedListView();
            }
            else
            {
                ShowOnlyUnchanged = false;
                ShowOnlyUnchangedBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#B7A3FF"));
                ShowAnyStateListView();
            }
        }

        private void ShowOnlyUnchangedListView()
        {
            if (ShowOnlyUnchanged == true && ShowOnlyChanged == false)
            {                
                AdvertisementsListView.ItemsSource = homepageService.GetAllAdvertisements().Select(adv => adv).Where(adv => adv.State == "Загружено");
                AdvertisementsListView.Items.Refresh();
            }
        }

        #endregion

        #region Show only "Изменено"

        private void ShowOnlyChangedButtonClick(object sender, RoutedEventArgs e)
        {
            if (ShowOnlyChanged == false)
            {
                ShowOnlyChanged = true;
                ShowOnlyChangedBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#E6D9FF"));
                ShowOnlyChangedListView();
            }
            else
            {
                ShowOnlyChanged = false;
                ShowOnlyChangedBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#B7A3FF"));
                ShowAnyStateListView();
            }
        }

        private void ShowOnlyChangedListView()
        {
            if (ShowOnlyChanged == true && ShowOnlyUnchanged == false)
            {
                AdvertisementsListView.ItemsSource = homepageService.GetAllAdvertisements().Select(adv => adv).Where(adv => adv.State == "Изменено");
                AdvertisementsListView.Items.Refresh();
            }            
        }

        #endregion

        #region Show items only with photos

        private void ShowOnlyWithPhotosButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowOnlyWithPhotos == false)
            {
                ShowOnlyWithPhotos = true;
                ShowOnlyWithPhotosBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#E6D9FF"));
                ShowOnlyWithPhotosListView();
            }
            else
            {
                ShowOnlyWithPhotos = false;
                ShowOnlyWithPhotosBorder.Background = (Brush)(new BrushConverter().ConvertFrom("#B7A3FF"));
                ShowAnyStateListView();
            }
        }

        private void ShowOnlyWithPhotosListView()
        {
            AdvertisementsListView.ItemsSource = homepageService.GetAllAdvertisements().Select(adv => adv).Where(adv => adv.ImageLinks.Split(";").Length > 0);
            AdvertisementsListView.Items.Refresh();
        }

        #endregion

        #region Show any state advertisements

        private void ShowAnyStateListView()
        {
            if (ShowOnlyChanged == false && ShowOnlyUnchanged == false)
            {
                AdvertisementsListView.ItemsSource = homepageService.GetAllAdvertisements();
                AdvertisementsListView.Items.Refresh();
            }            
        }

        #endregion

        #region Date Filter Text Box Events

        private void DateFilterTextBox_Click(object sender, RoutedEventArgs e)
        {
            if (DateFilterTextBox.Text == "Фильтрация по дате")
            {
                DateFilterTextBox.Text = "";
                date = null;
            }
        }

        private void DateFilterTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DateFilterTextBox.Text == "")
            {
                DateFilterTextBox.Text = "Фильтрация по дате";
                date = null;
            }
        }

        private void DateFilterTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            date = null;
            if (DateFilterTextBox.Text != "Фильтрация по дате" && DateFilterTextBox.Text != "")
            {
                date = DateFilterTextBox.Text;
            }
        }

        #endregion

        #region Housing Type Text Box Events

        private void HouseTypeFilterTextBox_Click(object sender, RoutedEventArgs e)
        {
            if (HouseTypeFilterTextBox.Text == "Фильтрация по типу жилья")
            {
                HouseTypeFilterTextBox.Text = "";
            }
        }

        private void HouseTypeFilterTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (HouseTypeFilterTextBox.Text == "")
            {
                HouseTypeFilterTextBox.Text = "Фильтрация по типу жилья";
            }
        }

        private void HouseTypeFilterTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            houseType = null;
            if (HouseTypeFilterTextBox.Text != "Фильтрация по типу жилья" && HouseTypeFilterTextBox.Text != "")
            {
                houseType = HouseTypeFilterTextBox.Text;
            }
        }

        #endregion

        #region Rooms Count Text Box Events

        private void RoomsCountFilterTextBox_Click(object sender, RoutedEventArgs e)
        {
            if (RoomsCountFilterTextBox.Text == "Фильтрация по числу комнат")
            {
                roomsCount = null;
                RoomsCountFilterTextBox.Text = "";
            }
        }

        private void RoomsCountFilterTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (RoomsCountFilterTextBox.Text == "")
            {
                roomsCount = null;
                RoomsCountFilterTextBox.Text = "Фильтрация по числу комнат";
            }
        }

        private void RoomsCountFilterTextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            roomsCount = null;
            if (RoomsCountFilterTextBox.Text != "Фильтрация по числу комнат" && RoomsCountFilterTextBox.Text != "")
            {
                roomsCount = RoomsCountFilterTextBox.Text;
            }
        }

        #endregion

        #region Reset Filters Button Event

        private void RemoveFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            // обновление полей в коде
            date = null;
            houseType = null;
            roomsCount = null;
            DateFilterTextBox.Text = "Фильтрация по дате";
            HouseTypeFilterTextBox.Text = "Фильтрация по типу жилья";
            RoomsCountFilterTextBox.Text = "Фильтрация по числу комнат";
            mode = new AdvertisementsCollectionFilterMode();

            // обновление интерфейса ( возврат в исходное состояние )
            AdvertisementsListView.ItemsSource = homepageService.GetAllAdvertisements();
            AdvertisementsListView.Items.Refresh();
        }

        #endregion

        #region Repository Selection Event

        private void RepositoryListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RepositoriesListView.SelectedItem != null)
            {
                selectedRepository = RepositoriesListView.SelectedItem as AdvertisementRepository;                
            }
        }

        #endregion

        #region Apply filter button events

        private void AddFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
            AdvertisementsCollectionFilter collectionFilter = new AdvertisementsCollectionFilter(mode);
            var filteredAdvertisementCollection = collectionFilter.FilterAdvertisements();
            Advertisements = new ObservableCollection<Advertisement>(filteredAdvertisementCollection);
            AdvertisementsListView.ItemsSource = filteredAdvertisementCollection;
            AdvertisementsListView.Items.Refresh();
        }

        private void ApplyFilters()
        {
            if (!string.IsNullOrEmpty(houseType))
            {
                mode.SetHouseType(houseType);
            }
            if (!string.IsNullOrEmpty(date))
            {
                mode.SetDates(date);
            }
            if (!string.IsNullOrEmpty(roomsCount))
            {
                mode.SetRoomsCount(roomsCount);
            }
        }

        #endregion

        #region Open Repository Folder Event

        private void OpenRepositoryFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (selectedRepository != null)
            {
                Process.Start("explorer.exe", $"{selectedRepository.Path}");
            }
            else
            {
                ErrorView errorView = new ErrorView();
                errorView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                errorView.ErrorText.Text = "Не выбран репозиторий";
                errorView.Show();
            }
        }

        #endregion
    }
}
