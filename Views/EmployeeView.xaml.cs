using System.Windows;
using TaskManagementApp.ViewModels;

namespace TaskManagementApp.Views
{
    public partial class EmployeeView : Window
    {
        public EmployeeView()
        {
            InitializeComponent();
            DataContext = new EmployeeViewModel();
        }
    }
}