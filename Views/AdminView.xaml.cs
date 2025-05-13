using System.Windows;
using TaskManagementApp.ViewModels;

namespace TaskManagementApp.Views
{
    public partial class AdminView : Window
    {
        public AdminView()
        {
            InitializeComponent();
            DataContext = new AdminViewModel();
        }
    }
}