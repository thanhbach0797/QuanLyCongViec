using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TaskManagementApp.Models;
using TaskManagementApp.Views;

namespace TaskManagementApp.ViewModels
{
    public partial class AdminViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> users;

        [ObservableProperty]
        private ObservableCollection<User> filteredUsers;

        [ObservableProperty]
        private ObservableCollection<Project> projects;

        [ObservableProperty]
        private ObservableCollection<Project> filteredProjects;

        [ObservableProperty]
        private ObservableCollection<WorkTask> tasks;

        [ObservableProperty]
        private ObservableCollection<WorkTask> filteredTasks;

        [ObservableProperty]
        private ObservableCollection<Employee> employees;

        [ObservableProperty]
        private ObservableCollection<string> roleFilters;

        [ObservableProperty]
        private string selectedRoleFilter;

        [ObservableProperty]
        private string projectStartDateFilter;

        [ObservableProperty]
        private ObservableCollection<string> statusFilters;

        [ObservableProperty]
        private string selectedStatusFilter;

        [ObservableProperty]
        private Project selectedProjectFilter;

        [ObservableProperty]
        private string currentView = "ManageUsers";

        public AdminViewModel()
        {
            // Dữ liệu giả lập
            Users = new ObservableCollection<User>
            {
                new User { Id = 1, Username = "admin", Role = "Admin", Email = "admin@example.com" },
                new User { Id = 2, Username = "manager", Role = "Manager", Email = "manager@example.com" },
                new User { Id = 3, Username = "employee1", Role = "Employee", Email = "employee1@example.com" }
            };
            FilteredUsers = new ObservableCollection<User>(Users);

            Projects = new ObservableCollection<Project>
            {
                new Project { Id = 0, Name = "All" },
                new Project { Id = 1, Name = "Dự án A", Description = "Dự án phát triển phần mềm", StartDate = "2025-01-01", EndDate = "2025-06-30" },
                new Project { Id = 2, Name = "Dự án B", Description = "Dự án nghiên cứu thị trường", StartDate = "2025-03-01", EndDate = "2025-09-30" }
            };
            SelectedProjectFilter = Projects[0];

            FilteredProjects = new ObservableCollection<Project>(Projects);

            Tasks = new ObservableCollection<WorkTask>
            {
                new WorkTask { TaskId = 1, Title = "Hoàn thành báo cáo", Description = "Báo cáo hàng tuần", Status = "In Progress", DueDate = "2025-05-20", AssignedTo = "employee1", ProjectId = 1 },
                new WorkTask { TaskId = 2, Title = "Gặp khách hàng", Description = "Thảo luận yêu cầu dự án", Status = "Not Started", DueDate = "2025-05-22", AssignedTo = "employee1", ProjectId = 2 },
                new WorkTask { TaskId = 3, Title = "Nộp tài liệu", Description = "Tài liệu dự án", Status = "Completed", DueDate = "2025-05-10", AssignedTo = "employee1", ProjectId = 1 }
            };
            FilteredTasks = new ObservableCollection<WorkTask>(Tasks);

            Employees = new ObservableCollection<Employee>
            {
                new Employee { Id = 1, Name = "John Doe" },
                new Employee { Id = 2, Name = "Jane Smith" },
                new Employee { Id = 3, Name = "Employee1" }
            };

            RoleFilters = new ObservableCollection<string> { "All", "Admin", "Manager", "Employee" };
            SelectedRoleFilter = "All";

            StatusFilters = new ObservableCollection<string> { "All", "Not Started", "In Progress", "Completed" };
            SelectedStatusFilter = "All";
        }

        [RelayCommand]
        private void NavigateToManageUsers()
        {
            CurrentView = "ManageUsers";
            FilterUsers();
        }

        [RelayCommand]
        private void NavigateToManageProjects()
        {
            CurrentView = "ManageProjects";
            FilterProjects();
        }

        [RelayCommand]
        private void NavigateToManageTasks()
        {
            CurrentView = "ManageTasks";
            FilterTasks();
        }

        [RelayCommand]
        private void Logout()
        {
            var loginView = new LoginView();
            loginView.Show();
            Application.Current.Windows[0]?.Close();
        }

        [RelayCommand]
        private void FilterUsers()
        {
            if (SelectedRoleFilter == "All")
            {
                FilteredUsers = new ObservableCollection<User>(Users);
            }
            else
            {
                FilteredUsers = new ObservableCollection<User>(Users.Where(u => u.Role == SelectedRoleFilter));
            }
        }

        [RelayCommand]
        private void FilterProjects()
        {
            if (string.IsNullOrWhiteSpace(ProjectStartDateFilter))
            {
                FilteredProjects = new ObservableCollection<Project>(Projects);
            }
            else
            {
                FilteredProjects = new ObservableCollection<Project>(
                    Projects.Where(p => p.StartDate.Contains(ProjectStartDateFilter))
                );
            }
        }

        [RelayCommand]
        public void FilterTasks() // Thay đổi thành public
        {
            var filtered = Tasks.AsEnumerable();

            if (SelectedProjectFilter != null && SelectedProjectFilter.Id != 0)
            {
                filtered = filtered.Where(t => t.ProjectId == SelectedProjectFilter.Id);
            }

            if (SelectedStatusFilter != "All")
            {
                filtered = filtered.Where(t => t.Status == SelectedStatusFilter);
            }

            FilteredTasks = new ObservableCollection<WorkTask>(filtered);
        }

        [RelayCommand]
        private void EditUser(User user)
        {
            MessageBox.Show($"Chỉnh sửa người dùng: {user.Username} (chưa triển khai).");
        }

        [RelayCommand]
        private void DeleteUser(User user)
        {
            Users.Remove(user);
            FilterUsers();
            MessageBox.Show($"Đã xóa người dùng: {user.Username}");
        }

        [RelayCommand]
        private void EditProject(Project project)
        {
            MessageBox.Show($"Chỉnh sửa dự án: {project.Name} (chưa triển khai).");
        }

        [RelayCommand]
        private void DeleteProject(Project project)
        {
            Projects.Remove(project);
            FilterProjects();
            MessageBox.Show($"Đã xóa dự án: {project.Name}");
        }

        [RelayCommand]
        private void ViewTask(WorkTask task)
        {
            MessageBox.Show($"Xem chi tiết công việc: {task.Title}\nMô tả: {task.Description}\nGiao cho: {task.AssignedTo}");
        }

        [RelayCommand]
        private void EditTask(WorkTask task)
        {
            MessageBox.Show($"Chỉnh sửa công việc: {task.Title} (chưa triển khai).");
        }

        [RelayCommand]
        private void AddTask()
        {
            var addTaskWindow = new AddTaskWindow(null, this);
            addTaskWindow.ShowDialog();
        }
    }
}