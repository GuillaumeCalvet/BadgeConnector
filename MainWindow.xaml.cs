using System.Windows;
using BadgeConnector.ViewModel;

namespace BadgeConnector
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}