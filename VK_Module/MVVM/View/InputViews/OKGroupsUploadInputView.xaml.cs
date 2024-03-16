using System.Collections.Generic;
using System.ComponentModel;
using System;
using System.Windows;
using VK_Module.Databases.OKGroupsUploadDbModel;

namespace VK_Module.MVVM.View.InputViews
{
    public partial class OKGroupsUploadInputView : Window
    {
        private List<string> okGroups;
        private OKGroupsUploadDbModel okGroupsUploadDatabase;
        private string okGroupToAdd;
        private string selectedOkGroup;

        List<string> OkGroups
        {
            get { return okGroups; }
            set { OnPropertyChanged(nameof(OkGroups)); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public OKGroupsUploadInputView()
        {
            SetDbConnection();
            GetCurrentGroupLinks();
            InitializeComponent();
            if (okGroups != null)
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
            okGroupsUploadDatabase = new OKGroupsUploadDbModel(@"Databases\OKgroupsUploadDb.db");
        }

        private void AddGroupLink()
        {
            if (!string.IsNullOrEmpty(okGroupToAdd))
            {
                try
                {
                    okGroupsUploadDatabase.AddGroupLink(okGroupToAdd);
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
            okGroups = okGroupsUploadDatabase.GetAllGroupLinks();
            UpdateListView();
            CurrentGroupsUpdated?.Invoke(this, okGroups);
        }

        private void UpdateListView()
        {
            if (okGroups != null && GroupLinksListView != null)
            {
                GroupLinksListView.Items.Clear();
                foreach (var groupLink in okGroups)
                {
                    GroupLinksListView.Items.Add(groupLink);
                }
            }
        }

        private void OnTargetGroupLinkChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(GroupLink.Text))
            {
                okGroupToAdd = GroupLink.Text;
            }
        }

        private void OnTargetGroupLostFocus(object sender, RoutedEventArgs e)
        {
            if (GroupLink.Text == "")
            {
                GroupLink.Text = "Ссылка на группу Одноклассники";
                okGroupToAdd = null;
            }
        }

        private void OnTargetGroupLMClick(object sender, RoutedEventArgs e)
        {
            if (GroupLink.Text == "Ссылка на группу Одноклассники")
            {
                GroupLink.Text = "";
                okGroupToAdd = null;
            }
        }

        private void AddGroupLinkButtonClick(object sender, RoutedEventArgs e)
        {
            AddGroupLink();
        }

        private void RemoveGroupLink()
        {
            if (!string.IsNullOrEmpty(selectedOkGroup))
            {
                okGroupsUploadDatabase.RemoveGroupLink(selectedOkGroup);
                GetCurrentGroupLinks();
            }
        }

        private void RemoveGroupLinkButtonClick(object sender, RoutedEventArgs e)
        {
            RemoveGroupLink();
        }

        private void SelectGroupLinkClick(object sender, RoutedEventArgs e)
        {
            selectedOkGroup = GroupLinksListView.SelectedItem as string;
        }

        public event EventHandler<List<string>> CurrentGroupsUpdated;
    }
}
