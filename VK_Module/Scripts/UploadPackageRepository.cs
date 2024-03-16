using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Scripts
{
    public class UploadPackageRepository
    {
        private List<UploadPackage> packageList;
        private string[] selectedDirectories;
        public UploadPackageRepository()
        {
            packageList = new List<UploadPackage>();
        }

        public void SelectAdvertisementDirecotires()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                selectedDirectories = dialog.FileNames.ToArray();
                foreach (var selectedDirectory in selectedDirectories)
                {
                    UploadPackage package = new UploadPackage();
                    var fileDirectories = Directory.GetDirectories(selectedDirectory);
                    foreach (var fileDirectory in fileDirectories)
                    {
                        var files = Directory.GetFiles(fileDirectory);
                        foreach(var file in files)
                        {
                            if (IsImageExtension(file))
                            {
                                package.AttachImages(file);
                            }
                            if (IsTextExtension(file))
                            {
                                package.AttachTextContent(file);
                            }
                        }
                    }
                    packageList.Add(package);
                }
            }            
        }        

        public List<UploadPackage> GetPackages()
        {
            return packageList;
        }

        private bool IsImageExtension(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };                        
            return imageExtensions.Any(ext => filePath.Contains(ext));
        }

        private bool IsTextExtension(string filePath)
        {
            string[] imageExtensions = { ".txt" };
            return imageExtensions.Any(ext => filePath.Contains(ext));
        }
    }
}
