using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using VK_Module.Databases.CianPagesLoadDbModel;
using VK_Module.Databases.NameFilterDbModel;
using VK_Module.MVVM.View.InputViews;
using Services;

namespace VK_Module.MVVM.View
{    
    public partial class CianView : UserControl
    {        
        private CianPagesLoadDbModel pagesLoadDataBase;
        private NameFilterDbModel nameFilterDatabase;

        public bool _UseNameFilter;
        public bool _UseDateFilter;

        private string dateFrom;
        private string dateTo;

        private int advertisementCount;
        private int pagesCount;
        
        public List<string> CianPages = null;
        public List<string> NamesFilter = null;

        public CianView()
        {
            pagesLoadDataBase = new CianPagesLoadDbModel(@"Databases\CianPagesLoadDb.db");            
            nameFilterDatabase = new NameFilterDbModel(@"Databases\NameFilterDb.db");            
            CianPages = pagesLoadDataBase.GetAllPageLinks();
            NamesFilter = nameFilterDatabase.GetAllNames();
            InitializeComponent();
            UpdateGroupLinks(CianPages);            
            UpdateNamesList(NamesFilter);
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
            advertisementCount = 0;
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
                advertisementCount = 0;
            }
        }

        private void AdvertisementCountLMClick(object sender, RoutedEventArgs e)
        {
            if (AdvertisementCountTextBox.Text == "Желаемое число объявлений")
            {
                AdvertisementCountTextBox.Text = "";
                advertisementCount = 0;
            }
        }
        #endregion

        #region Pages Count events
        private void PagesCountTextChanged(object sender, RoutedEventArgs e)
        {
            pagesCount = 1;
            if (!string.IsNullOrEmpty(PagesCountTextBox.Text) && PagesCountTextBox.Text != "Число страниц")
            {
                pagesCount = Convert.ToInt32(PagesCountTextBox.Text);
            }
        }

        private void PagesCountLostFocus(object sender, RoutedEventArgs e)
        {
            if (PagesCountTextBox.Text == "")
            {
                PagesCountTextBox.Text = "Число страниц";
                pagesCount = 1;
            }
        }

        private void PagesCountLMClick(object sender, RoutedEventArgs e)
        {
            if (PagesCountTextBox.Text == "Число страниц")
            {
                PagesCountTextBox.Text = "";
                pagesCount = 1;
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

        private void OpenNamesListInputView()
        {
            NamesFIlterInputView inputView = new NamesFIlterInputView();
            inputView.CurrentNamesUpdated += InputView_CurrentNamesFilterUpdated;
            inputView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inputView.Show();
        }        

        private void OpenGroupLinksInputView()
        {
            CianPagesLoadInputView inputView = new CianPagesLoadInputView();
            inputView.CurrentGroupsUpdated += InputView_CurrentCianPagesUpdated;
            inputView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            inputView.Show();
        }
        #endregion

        #region Current state of input data        

        public void UpdateGroupLinks(List<string> newGroupLinks)
        {
            CianPages = newGroupLinks;
            {
                if (CianPages.Count > 0)
                {
                    GroupsState.Foreground = new SolidColorBrush(Colors.ForestGreen);
                    GroupsState.Text = $"Состояние: Присутствуют ({CianPages.Count})";
                }
                else
                {
                    GroupsState.Foreground = new SolidColorBrush(Colors.DarkViolet);
                    GroupsState.Text = $"Состояние: Отсутствуют ({CianPages.Count})";
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

        private void InputView_CurrentNamesFilterUpdated(object sender, List<string> newNames)
        {
            UpdateNamesList(newNames);
        }

        private void InputView_CurrentCianPagesUpdated(object sender, List<string> newGroupLinks)
        {
            UpdateGroupLinks(newGroupLinks);
        }
        
        #endregion

        #region Cian Client realisation
        private void LoadButtonClick(object sender, RoutedEventArgs e)
        {
            if (CianPages.Count > 0)
            {
                LoadAdvertisements();
            }
        }

        private void LoadAdvertisements()
        {
            CianService client = new CianService(CianPages, NamesFilter, dateTo, dateFrom, advertisementCount, pagesCount);
            client.GetAdvertisements();
        }
        #endregion

        #region Cian Logo Remover realisation

        private void RemoveWatermark_Click(object sender, RoutedEventArgs e)
        {
            CianLogoRemoverService service = new CianLogoRemoverService();
            service.SelectPhotos();              
            service.RemoveWatermark();
        }

        #endregion
    }
}
