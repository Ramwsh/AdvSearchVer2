using System.Collections.Generic;
using VK_API;
using VK_Module.VK_Mod;
using Scripts;
using VK_Module.MVVM.View;
using System.Windows.Threading;

namespace Services
{
    public class VKUploadService
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

        private int GetGroupOwnerID(string groupUrl)
        {
            VKFilesLoadManager loadManager = new VKFilesLoadManager();
            return loadManager.GetGroupOwnerID(groupUrl);
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
                int groupId = GetGroupOwnerID(group);
                VKFilesUploadManager uploadManager = new VKFilesUploadManager();                

                foreach (var package in packageList)
                {
                    if (package.GetImagePaths().Count > 0)
                    {
                        uploadManager.PushOnServer(package.GetContent(), package.GetImagePaths().ToArray(), groupId).Wait();
                        progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                    }                    
                    else
                    {
                        uploadManager.PushTextPost(package.GetContent(), groupId);
                        progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, DispatcherPriority.Background);
                    }
                }
            }

            progressBar.Close();
        }
    }
}
