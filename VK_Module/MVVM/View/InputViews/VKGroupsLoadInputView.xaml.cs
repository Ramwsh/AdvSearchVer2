using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using VK_Module.Databases.VKGroupsLoadDbModel;

namespace VK_Module.MVVM.View.InputViews
{    
    public partial class VKGroupsLoadInputView : Window
    {        
        private List<string> vkGroups;
        private VKGroupsLoadDbModel vkGroupsLoadDatabase;
        private string vkGroupToAdd;
        private string selectedVkGroup;

        public List<string> VkGroups
        {
            get { return vkGroups; }
            set { OnPropertyChanged(nameof(vkGroups)); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public VKGroupsLoadInputView()
        {
            SetDbConnection();
            GetCurrentGroupLinks();
            InitializeComponent();
            if (vkGroups != null)
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
            vkGroupsLoadDatabase = new VKGroupsLoadDbModel(@"Databases\VKgroupsLoadDb.db");
        }

        private void AddGroupLink()
        {
            if (!string.IsNullOrEmpty(vkGroupToAdd))
            {
                try
                {
                    vkGroupsLoadDatabase.AddGroupLink(vkGroupToAdd);
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
            vkGroups = vkGroupsLoadDatabase.GetAllGroupLinks();
            UpdateListView();
            CurrentGroupsUpdated?.Invoke(this, vkGroups);
        }

        private void UpdateListView()
        {
            if (vkGroups != null && GroupLinksListView != null)
            {
                GroupLinksListView.Items.Clear();
                foreach (var groupLink in vkGroups)
                {
                    GroupLinksListView.Items.Add(groupLink);
                }
            }
        }

        private void OnTargetGroupLinkChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GroupLink.Text))
            {
                vkGroupToAdd = GroupLink.Text;
            }
        }

        private void OnTargetGroupLostFocus(object sender, RoutedEventArgs e)
        {
            if (GroupLink.Text == "")
            {
                GroupLink.Text = "Ссылка на группу ВКонтакте";
                vkGroupToAdd = null;
            }
        }

        private void OnTargetGroupLMClick(object sender, RoutedEventArgs e)
        {
            if (GroupLink.Text == "Ссылка на группу ВКонтакте")
            {
                GroupLink.Text = "";
                vkGroupToAdd = null;
            }
        }

        private void AddGroupLinkButtonClick(object sender, RoutedEventArgs e)
        {
            AddGroupLink();
        }

        private void RemoveGroupLink()
        {
            if (!string.IsNullOrEmpty(selectedVkGroup))
            {
                vkGroupsLoadDatabase.RemoveGroupLink(selectedVkGroup);
                GetCurrentGroupLinks();
            }
        }

        private void RemoveGroupLinkButtonClick(object sender, RoutedEventArgs e)
        {
            RemoveGroupLink();
        }

        private void SelectGroupLinkClick(object sender, RoutedEventArgs e)
        {
            selectedVkGroup = GroupLinksListView.SelectedItem as string;
        }

        public event EventHandler<List<string>> CurrentGroupsUpdated;
    }
}
