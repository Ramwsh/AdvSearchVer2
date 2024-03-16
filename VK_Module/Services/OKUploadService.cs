using Scripts;
using System.Collections.Generic;
using System.Windows.Threading;
using VK_Module.MVVM.View;
using VK_Module.OK_Mod;

namespace Services
{
    public class OKUploadService
    {
        private List<UploadPackage> packageList;
        private List<string> groupsToUpload;

        public void AttachPackages(List<UploadPackage> packageList)
        {
            this.packageList = packageList;
        }

        public void AttachGroups(List<string> groupsToUpload)
        {
            this.groupsToUpload = groupsToUpload;
        }        

        public void UploadAdvertisements()
        {
            ProgressBarWindow progressBar = new ProgressBarWindow();
            progressBar.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            progressBar.ProgressBar.Minimum = 0;
            progressBar.ProgressBar.Value = 0;
            progressBar.ProgressBar.Maximum = groupsToUpload.Count * packageList.Count;
            progressBar.Show();

            foreach (var group in groupsToUpload)
            {                
                OKFilesUploadManager uploadManager = new OKFilesUploadManager(group);

                foreach (var package in packageList)
                {
                    if (package.GetImagePaths().Count > 0)
                    {
                        uploadManager.PostOnlyWithText(package.GetContent());
                        progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                    }
                    else
                    {
                        uploadManager.UploadByURL(package.GetImagePaths().ToArray(), package.GetContent());
                        progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                    }
                }
            }

            progressBar.Close();
        }
    }
}
