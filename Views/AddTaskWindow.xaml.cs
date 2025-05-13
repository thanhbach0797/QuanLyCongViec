using System.Windows;
using TaskManagementApp.ViewModels;

namespace TaskManagementApp.Views
{
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow(ManagerViewModel managerViewModel = null, AdminViewModel adminViewModel = null)
        {
            InitializeComponent();
            DataContext = new AddTaskViewModel(managerViewModel, adminViewModel);
        }
    }
}