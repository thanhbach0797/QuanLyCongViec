using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using System.Windows;
using TaskManagementApp.Services;
using TaskManagementApp.Views;

namespace TaskManagementApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage;

        private readonly ApiService _apiService;

        public LoginViewModel()
        {
            _apiService = new ApiService();
            LoginCommand = new AsyncRelayCommand(OnLoginAsync);
        }

        public IAsyncRelayCommand LoginCommand { get; }

        private async Task OnLoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                ErrorMessage = "Vui lòng nhập tên đăng nhập.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Vui lòng nhập mật khẩu.";
                return;
            }

            try
            {
                var user = await _apiService.AuthenticateAsync(Username, Password);

                if (user != null)
                {
                    ErrorMessage = string.Empty;

                    // Phân quyền dựa trên vai trò
                    Window nextView = user.Role switch
                    {
                        "Admin" => new AdminView(),
                        "Manager" => new ManagerView(),
                        "Employee" => new EmployeeView(),
                        _ => throw new NotSupportedException("Vai trò không được hỗ trợ.")
                    };

                    nextView.Show();
                    Application.Current.Windows[0]?.Close();
                }
                else
                {
                    ErrorMessage = "Tên đăng nhập hoặc mật khẩu không đúng.";
                }
            }
            catch (System.Net.Http.HttpRequestException)
            {
                ErrorMessage = "Không thể kết nối đến server. Vui lòng kiểm tra mạng.";
            }
            catch
            {
                ErrorMessage = "Đã xảy ra lỗi không mong muốn. Vui lòng thử lại.";
            }
        }
    }
}