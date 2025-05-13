using System.Windows;
using TaskManagementApp.ViewModels;

namespace TaskManagementApp.Views
{
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}