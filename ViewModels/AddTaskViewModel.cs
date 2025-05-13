using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TaskManagementApp.Models;
using TaskManagementApp.ViewModels;
using TaskManagementApp.Views;

namespace TaskManagementApp.ViewModels
{
    public partial class AddTaskViewModel : ObservableObject
    {
        private readonly ManagerViewModel _managerViewModel;
        private readonly AdminViewModel _adminViewModel;

        [ObservableProperty]
        private string newTaskTitle;

        [ObservableProperty]
        private string newTaskDescription;

        [ObservableProperty]
        private string newTaskStatus;

        [ObservableProperty]
        private string newTaskDueDate;

        [ObservableProperty]
        private Project selectedProject;

        [ObservableProperty]
        private Employee selectedEmployee;

        [ObservableProperty]
        private ObservableCollection<Project> projects;

        [ObservableProperty]
        private ObservableCollection<Employee> employees;

        [ObservableProperty]
        private ObservableCollection<string> statusOptions;

        public AddTaskViewModel(ManagerViewModel managerViewModel = null, AdminViewModel adminViewModel = null)
        {
            _managerViewModel = managerViewModel;
            _adminViewModel = adminViewModel;

            // Sử dụng dữ liệu từ ManagerViewModel hoặc AdminViewModel
            Projects = _managerViewModel?.Projects ?? _adminViewModel?.Projects ?? new ObservableCollection<Project>();
            Employees = _managerViewModel?.Employees ?? _adminViewModel?.Employees ?? new ObservableCollection<Employee>(); // Sửa ở đây

            // Thêm dự án mặc định nếu danh sách trống
            if (Projects.Count == 0)
            {
                Projects.Add(new Project { Id = 1, Name = "Dự án A" });
            }

            // Thêm nhân viên mặc định nếu danh sách trống
            if (Employees.Count == 0)
            {
                Employees.Add(new Employee { Id = 1, Name = "Default Employee" });
            }

            // Danh sách trạng thái
            StatusOptions = new ObservableCollection<string> { "Not Started", "In Progress", "Completed" };
            NewTaskStatus = "Not Started";
        }

        [RelayCommand]
        private void Add()
        {
            if (string.IsNullOrWhiteSpace(NewTaskTitle) || string.IsNullOrWhiteSpace(NewTaskDueDate) ||
                SelectedProject == null || SelectedEmployee == null || string.IsNullOrWhiteSpace(NewTaskStatus))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            var newTask = new WorkTask
            {
                TaskId = (_managerViewModel?.TeamTasks.Count ?? _adminViewModel?.Tasks.Count ?? 0) + 1,
                Title = NewTaskTitle,
                Description = NewTaskDescription ?? string.Empty,
                Status = NewTaskStatus,
                DueDate = NewTaskDueDate,
                AssignedTo = SelectedEmployee.Name,
                ProjectId = SelectedProject.Id
            };

            if (_managerViewModel != null)
            {
                _managerViewModel.TeamTasks.Add(newTask);
                _managerViewModel.FilterTasks();
                _managerViewModel.UpdateDashboardStats();
            }
            else if (_adminViewModel != null)
            {
                _adminViewModel.Tasks.Add(newTask);
                _adminViewModel.FilterTasks();
            }

            MessageBox.Show("Thêm công việc thành công!");
            Application.Current.Windows.OfType<AddTaskWindow>().FirstOrDefault()?.Close();
        }

        [RelayCommand]
        private void Cancel()
        {
            Application.Current.Windows.OfType<AddTaskWindow>().FirstOrDefault()?.Close();
        }
    }
}