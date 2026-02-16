using System.Windows;
using Presentation.Wpf.ViewModels;

namespace Presentation.Wpf
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}
