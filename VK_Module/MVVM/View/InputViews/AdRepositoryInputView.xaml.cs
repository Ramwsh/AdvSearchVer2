using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using VK_Module.Scripts;

namespace VK_Module.MVVM.View.InputViews
{       
    public partial class AdRepositoryInputView : Window, INotifyPropertyChanged
    {
        private AdvRepositoriesService repositoryService;
        private AdvertisementRepository selectedRepository;
        private string repositoryTag;

        public List<AdvertisementRepository> AdvertisementRepositories
        {
            get { return repositoryService.GetAllRepositories(); }
            set { OnPropertyChanged(nameof(AdvertisementRepositories)); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<List<AdvertisementRepository>> CurrentRepositoryChanged;

        public AdRepositoryInputView()
        {            
            InitializeComponent();
            repositoryService = new AdvRepositoriesService();
            DataContext = this;
        }
        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region Add Repository Event
        public void AddRepositoryClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(repositoryTag))
            {
                string repositoryPath = @$"Advertisement Repositories\{repositoryTag}";
                AdvertisementRepository repository = new AdvertisementRepository(repositoryTag, repositoryPath);
                repositoryService.AddRepository(repository);                
                AdvertisementRepositories = repositoryService.GetAllRepositories();
                CurrentRepositoryChanged?.Invoke(this, AdvertisementRepositories);
            }
        }
        #endregion

        #region Remove Repository Event
        private void RemoveRepositoryClick(object sender, RoutedEventArgs e)
        {
            if (selectedRepository != null)
            {
                repositoryService.RemoveRepository(selectedRepository);
                AdvertisementRepositories = repositoryService.GetAllRepositories();
                CurrentRepositoryChanged?.Invoke(this, AdvertisementRepositories);
            }
        }
        #endregion

        #region Select Repository Event
        private void RepositoriesListViewRepositorySelection(object sender, SelectionChangedEventArgs e)
        {            
            selectedRepository = (AdvertisementRepository)RepositoriesListView.SelectedItem;
            
            if (selectedRepository != null)
            {
                Console.WriteLine("Selected Item: " + selectedRepository.Tag + " | " + selectedRepository.Path);
            }
        }
        #endregion        

        #region Repository Name Text Box events
        private void RepositoryTagTextBoxTextChanged(object sender, RoutedEventArgs e)
        {
            repositoryTag = null;
            if (!string.IsNullOrEmpty(RepositoryTagTextBox.Text) && RepositoryTagTextBox.Text != "Имя репозитория")
            {
                repositoryTag = RepositoryTagTextBox.Text;                
            }
        }

        private void RepositoryTagTextBoxLostFocus(object sender, RoutedEventArgs e)
        {
            if (RepositoryTagTextBox.Text == "")
            {
                RepositoryTagTextBox.Text = "Имя репозитория";
                repositoryTag = null;
            }
        }

        private void RepositoryTagTextBoxLMClick(object sender, RoutedEventArgs e)
        {
            if (RepositoryTagTextBox.Text == "Имя репозитория")
            {
                RepositoryTagTextBox.Text = "";
                repositoryTag = null;
            }
        }
        #endregion        
    }
}
