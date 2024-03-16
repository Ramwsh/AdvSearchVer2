using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using VK_Module.Databases.NameFilterDbModel;

namespace VK_Module.MVVM.View.InputViews
{    
    public partial class NamesFIlterInputView : Window
    {
        private List<string> names;
        private NameFilterDbModel namesDatabase;
        private string nameToAdd;
        private string selectedName;

        public List<string> Names
        {
            get { return names; }
            set { OnPropertyChanged(nameof(Names)); }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public NamesFIlterInputView()
        {
            SetDbConnection();
            GetCurrentNamesList();
            InitializeComponent();
            if (names != null)
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
            namesDatabase = new NameFilterDbModel(@"Databases\NameFilterDb.db");
        }

        private void AddName()
        {
            if (!string.IsNullOrEmpty(nameToAdd))
            {
                try
                {
                    namesDatabase.AddName(nameToAdd);
                    GetCurrentNamesList();
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

        private void GetCurrentNamesList()
        {
            names = namesDatabase.GetAllNames();
            UpdateListView();
            CurrentNamesUpdated?.Invoke(this, names);

        }

        private void UpdateListView()
        {
            if (names != null && NamesListView != null)
            {
                NamesListView.Items.Clear();
                foreach (string name in names)
                {
                    NamesListView.Items.Add(name);
                }
            }
        }

        private void OnTargetNameChanged(object sender, RoutedEventArgs e)
        {
            nameToAdd = null;
            if (!string.IsNullOrEmpty(Name.Text))
            {
                nameToAdd = Name.Text;
            }
        }

        private void OnTargetNameLostFocus(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "")
            {
                Name.Text = "Никнейм (как в посте)";
                nameToAdd = null;
            }
        }

        private void OnTargetNameLMClick(object sender, RoutedEventArgs e)
        {
            if (Name.Text == "Никнейм (как в посте)")
            {
                Name.Text = "";
                nameToAdd = null;
            }
        }

        private void AddNameButtonClick(object sender, RoutedEventArgs e)
        {
            AddName();
        }

        private void RemoveName()
        {
            if (!string.IsNullOrEmpty(selectedName))
            {
                namesDatabase.RemoveName(selectedName);
                GetCurrentNamesList();
            }
        }

        private void RemoveNameButtonClick(object sender, RoutedEventArgs e)
        {
            RemoveName();
        }

        private void SelectedNameClick(object sender, RoutedEventArgs e)
        {
            selectedName = NamesListView.SelectedItem as string;
        }

        public event EventHandler<List<string>> CurrentNamesUpdated;
    }
}
