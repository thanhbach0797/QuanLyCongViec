using System.Windows;
using TaskManagementApp.ViewModels;

namespace TaskManagementApp.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
        }
    }
}