using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskManagementApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string welcomeMessage = "Chào mừng bạn đến với ứng dụng!1";

        public MainViewModel()
        {
        }
    }
}