using System.Windows;

namespace VK_Module.MVVM.View
{
    public partial class ErrorView : Window
    {
        public ErrorView()
        {
            InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
