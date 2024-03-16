using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VK_Module.Scripts;
using System.Text.RegularExpressions;
using System.IO;

using Winforms = System.Windows.Forms;

namespace VK_Module.MVVM.View
{    
    public partial class SearchView : UserControl
    {
        public static string FilesRootDirectory;
        private SearchViewConfig SearchViewFiles;
        private string FilterParameter;
        private List<SearchViewConfig> AdvFiles;        

        public SearchView()
        {
            InitializeComponent();

            if (!string.IsNullOrEmpty(SearchViewConfig.FilesRootDirectory))
            {
                FilesRootDirectory = SearchViewConfig.FilesRootDirectory;
                LastSaveDataDirectoryTextBox.Text = FilesRootDirectory;
                SearchViewFiles = new SearchViewConfig();
                AdvFiles = SearchViewFiles.GetAdvDirNames();
                FillFilesListView();
            }                        
        }

        private void FillFilesListView()
        {   
            if (AdvFiles != null)
            {
                foreach (var AdvFile in AdvFiles)
                {
                    string AdvFileName = $"{AdvFiles.IndexOf(AdvFile)}\t{AdvFile.AdvName}";
                    FilesListView.Items.Add(AdvFileName);
                }                
            }            
        }

        private void FilterFilesListView()
        {
            FilesListView.Items.Clear();
            string parameter = FilterParameter;
            foreach (var AdvFile in AdvFiles)
            {
                Match match = Regex.Match(AdvFile.AdvName, parameter, RegexOptions.IgnoreCase);
                if (match.Success)
                {                                     
                    string AdvFileName = $"{AdvFiles.IndexOf(AdvFile)}\t{AdvFile.AdvName}";
                    FilesListView.Items.Add(AdvFileName);
                }
            }
        }

        private void SearchBarFilterTextBoxClick(object sender, RoutedEventArgs e)
        {
            if (SearchBarFilterTextBox.Text == "Параметр поиска (например квартира 4)")
            {
                sender.GetType().GetProperty("Text").SetValue(sender, "");
            }            
        }

        private void SearchBarFilterTextLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchBarFilterTextBox.Text))
            {
                sender.GetType().GetProperty("Text").SetValue(sender, "Параметр поиска (например квартира 4)");
            }            
        }

        private void SearchBarFilterTextLostTextInput(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchBarFilterTextBox.Text) & SearchBarFilterTextBox.Text != "Параметр поиска (например квартира 4)")
            {
                FilterParameter = SearchBarFilterTextBox.Text;
                FilterFilesListView();
            }
            else
            {
                
            }
        }

        private void FilesListViewDoubleClick(object sender, RoutedEventArgs e)
        {
            int index = Convert.ToInt32(FilesListView.SelectedItem.ToString().Split('\t').First());
            string[] TextPhotoDirectories = Directory.GetDirectories(AdvFiles[index].AdvPath);
            if (TextPhotoDirectories.Length == 2 || TextPhotoDirectories.Length == 1)
            {
                string[] photoDirectory = null;
                string[] textDirectory = Directory.GetFiles(TextPhotoDirectories[0]);
                if (TextPhotoDirectories.Length == 2)
                {
                    photoDirectory = Directory.GetFiles(TextPhotoDirectories[1]);
                }                
                //AdvertisementView advertisementView = new AdvertisementView(textDirectory, photoDirectory);
                //advertisementView.Show();   
            }
            else
            {
                string textDirectory = TextPhotoDirectories[0];
            }
        }

        private void Set_Path_For_Request_Data(object sender, RoutedEventArgs e)
        {            
            Winforms.FolderBrowserDialog fbd = new Winforms.FolderBrowserDialog();
            fbd.ShowDialog();
            if (fbd.SelectedPath != "" & string.IsNullOrEmpty(SearchViewConfig.FilesRootDirectory))
            {
                Winforms.MessageBox.Show("Каталог с сохраненной информацией выбран.", "Уведомление",
                Winforms.MessageBoxButtons.OK, Winforms.MessageBoxIcon.Information);
                FilesListView.Items.Clear();
                SearchViewConfig.FilesRootDirectory = fbd.SelectedPath;                
                FilesRootDirectory = fbd.SelectedPath;
                LastSaveDataDirectoryTextBox.Text = fbd.SelectedPath;                               
                UpdateSearchViewConfig();                
            }
            else
            {
                Winforms.MessageBox.Show("Каталог с сохраненной информацией выбран.", "Уведомление",
                Winforms.MessageBoxButtons.OK, Winforms.MessageBoxIcon.Information);
                FilesListView.Items.Clear();
                SearchViewConfig.FilesRootDirectory = fbd.SelectedPath;
                LastSaveDataDirectoryTextBox.Text = fbd.SelectedPath;
                UpdateSearchViewConfig();
            }
        }

        private void UpdateSearchViewConfig()
        {
            FilesRootDirectory = SearchViewConfig.FilesRootDirectory;
            LastSaveDataDirectoryTextBox.Text = FilesRootDirectory;
            SearchViewFiles = new SearchViewConfig();
            AdvFiles = SearchViewFiles.GetAdvDirNames();
            FillFilesListView();
        }
    }
}
