using System.Windows;
using TaskManagementApp.ViewModels;

namespace TaskManagementApp.Views
{
    public partial class ManagerView : Window
    {
        public ManagerView()
        {
            InitializeComponent();
            DataContext = new ManagerViewModel();
        }
    }
}