using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using VK_Module.Databases.CianPagesLoadDbModel;

namespace VK_Module.MVVM.View.InputViews
{    
    public partial class CianPagesLoadInputView : Window
    {
        private List<string> cianPages;
        private CianPagesLoadDbModel cianPagesDataBase;
        private string cianPageToAdd;
        private string selectedCianPage;

        public List<string> CianPages
        {
            get { return cianPages; }
            set { OnPropertyChanged(nameof(CianPages)); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CianPagesLoadInputView()
        {
            SetDbConnection();
            GetCurrentGroupLinks();
            InitializeComponent();
            if (cianPages != null)
            {
                UpdateListView();
            }            
        }

        private void CloseWindowClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetDbConnection()
        {
            cianPagesDataBase = new CianPagesLoadDbModel(@"Databases\CianPagesLoadDb.db");
        }

        private void AddGroupLink()
        {
            if (!string.IsNullOrEmpty(cianPageToAdd))
            {
                try
                {
                    cianPagesDataBase.AddPageLink(cianPageToAdd);
                    GetCurrentGroupLinks();
                }
                catch
                {
                    ErrorView errorView = new ErrorView();
                    errorView.ErrorText.Text = "Должно быть уникальным";
                    errorView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    errorView.Show();
                }
            }
        }

        private void GetCurrentGroupLinks()
        {
            cianPages = cianPagesDataBase.GetAllPageLinks();
            UpdateListView();
            CurrentGroupsUpdated?.Invoke(this, cianPages);
        }

        private void UpdateListView()
        {
            if (cianPages != null && GroupLinksListView != null)
            {
                GroupLinksListView.Items.Clear();
                foreach (var groupLink in cianPages)
                {
                    GroupLinksListView.Items.Add(groupLink);
                }
            }
        }

        private void OnTargetGroupLinkChanged(object sender, EventArgs e)
        {
            cianPageToAdd = null;
            if (!string.IsNullOrEmpty(GroupLink.Text))
            {
                cianPageToAdd = GroupLink.Text;
            }
        }

        private void OnTargetGroupLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(GroupLink.Text) || GroupLink.Text == "")
            {
                GroupLink.Text = "Ссылка на страницу ЦИАН с объявлениями";
                cianPageToAdd = null;
            }
        }

        private void OnTargetGroupLMClick(object sender, RoutedEventArgs e)
        {            
            if (GroupLink.Text == "Ссылка на страницу ЦИАН с объявлениями")
            {
                GroupLink.Text = "";
                cianPageToAdd = null;
            }
        }

        private void AddGroupLinkButtonClick(object sender, RoutedEventArgs e)
        {
            AddGroupLink();
        }

        private void RemoveGroupLink()
        {
            if (!string.IsNullOrEmpty(selectedCianPage))
            {
                cianPagesDataBase.RemovePageLink(selectedCianPage);
                GetCurrentGroupLinks();
            }
        }

        private void RemoveGroupLinkButtonClick(object sender, RoutedEventArgs e)
        {
            RemoveGroupLink();
        }

        private void SelectGroupLinkClick(object sender, RoutedEventArgs e)
        {
            selectedCianPage = GroupLinksListView.SelectedItem as string;
        }

        public event EventHandler<List<string>> CurrentGroupsUpdated;
    }
}
