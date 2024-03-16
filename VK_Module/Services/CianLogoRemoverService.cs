using Microsoft.WindowsAPICodePack.Dialogs;
using System.Linq;
using System.Windows;
using VK_Module.Cian_Mod.CianwatermarkDetection;
using VK_Module.MVVM.View;

namespace Services
{
    public class CianLogoRemoverService
    {
        private string[] imagePaths;

        public void SelectPhotos()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                imagePaths = dialog.FileNames.ToArray();
            }
        }
        
        public void RemoveWatermark()
        {
            if (imagePaths != null)
            {
                ProgressBarWindow progressBar = new ProgressBarWindow();
                progressBar.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                progressBar.ProgressBar.Minimum = 0;
                progressBar.ProgressBar.Maximum = imagePaths.Length;
                progressBar.ProgressBar.Value = 0;
                progressBar.Show();

                foreach (var image in imagePaths)
                {
                    CianDetectionClient client = new CianDetectionClient();
                    client = client.SetImage(image).SetInput().SetOutput();
                    var predictedData = client.GetPredictionData();
                    CianwatermarkRemover remover = new CianwatermarkRemover(image, predictedData);
                    remover.RemoveLogo();                                        
                    progressBar.ProgressBar.Dispatcher.Invoke(() => progressBar.ProgressBar.Value++, System.Windows.Threading.DispatcherPriority.Background);
                }

                progressBar.Close();
            }
        }
    }
}
