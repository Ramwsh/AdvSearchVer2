using VK_Module.Core;

namespace VK_Module.MVVM.Viewmodel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand VKViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }
        public RelayCommand ConfigurationViewCommand { get; set; }
        public RelayCommand SearchViewCommand { get; set; }
        public RelayCommand OKViewCommand { get; set; }
        public RelayCommand CianViewCommand { get; set; }
        public RelayCommand HomepageViewCommand { get; set; }

        public OKViewModel OKVM { get; set; }
        public VKViewModel VKVM { get; set; }
        public DiscoveryViewModel DiscoveryVM { get; set; }
        public ConfigurationView ConfigurationVM { get; set; }        
        public SearchViewModel SearchVM { get; set; }
        public CianViewModel CianVM { get; set; }
        public HomepageViewModel HomepageVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            DiscoveryVM = new DiscoveryViewModel();
            VKVM = new VKViewModel();
            ConfigurationVM = new ConfigurationView();
            SearchVM = new SearchViewModel();
            OKVM = new OKViewModel();
            CianVM = new CianViewModel();
            HomepageVM = new HomepageViewModel();

            CurrentView = HomepageVM;

            VKViewCommand = new RelayCommand(o =>
            {
                CurrentView = VKVM;
            });

            DiscoveryViewCommand = new RelayCommand(o =>
            {
                CurrentView = DiscoveryVM;
            });

            ConfigurationViewCommand = new RelayCommand(o =>
            {
                CurrentView = ConfigurationVM;
            });

            SearchViewCommand = new RelayCommand(o =>
            {
                CurrentView = SearchVM;
            });

            OKViewCommand = new RelayCommand(o => { 
                
                CurrentView = OKVM;

            });

            CianViewCommand = new RelayCommand(o =>
            {
                CurrentView = CianVM;
            });

            HomepageViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomepageVM;
            });
        }
    }
}
