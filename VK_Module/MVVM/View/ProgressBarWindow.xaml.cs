using Services;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Threading;
using System;

namespace VK_Module.MVVM.View
{    
    public partial class ProgressBarWindow : Window
    {
        private DispatcherTimer dispatcher;

        public ProgressBarWindow()
        {            
            InitializeComponent();
        }        
    }
}
