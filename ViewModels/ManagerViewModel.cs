using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using TaskManagementApp.Models;
using TaskManagementApp.Views;

namespace TaskManagementApp.ViewModels
{
    public partial class ManagerViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<WorkTask> teamTasks;

        [ObservableProperty]
        private ObservableCollection<WorkTask> filteredTasks;

        [ObservableProperty]
        private ObservableCollection<Employee> employees;

        [ObservableProperty]
        private ObservableCollection<Project> projects;

        [ObservableProperty]
        private Project selectedProjectFilter;

        [ObservableProperty]
        private ObservableCollection<string> statusFilters;

        [ObservableProperty]
        private string selectedStatusFilter;

        [ObservableProperty]
        private string currentView = "Dashboard";

        [ObservableProperty]
        private int teamInProgressCount;

        [ObservableProperty]
        private int teamCompletedCount;

        [ObservableProperty]
        private int teamOverdueCount;

        // Thuộc tính cho form giao việc
        [ObservableProperty]
        private string newTaskTitle;

        [ObservableProperty]
        private string newTaskDescription;

        [ObservableProperty]
        private string newTaskDueDate;

        [ObservableProperty]
        private Employee selectedEmployee;

        public ManagerViewModel()
        {
            // Dữ liệu giả lập
            TeamTasks = new ObservableCollection<WorkTask>
            {
                new WorkTask { TaskId = 1, Title = "Hoàn thành báo cáo", Description = "Báo cáo hàng tuần", Status = "In Progress", DueDate = "2025-05-20", AssignedTo = "John Doe", ProjectId = 1 },
                new WorkTask { TaskId = 2, Title = "Gặp khách hàng", Description = "Thảo luận yêu cầu dự án", Status = "Not Started", DueDate = "2025-05-22", AssignedTo = "Jane Smith", ProjectId = 2 },
                new WorkTask { TaskId = 3, Title = "Nộp tài liệu", Description = "Tài liệu dự án", Status = "Completed", DueDate = "2025-05-10", AssignedTo = "John Doe", ProjectId = 1 }
            };
            FilteredTasks = new ObservableCollection<WorkTask>(TeamTasks);

            Employees = new ObservableCollection<Employee>
            {
                new Employee { Id = 1, Name = "John Doe" },
                new Employee { Id = 2, Name = "Jane Smith" }
            };

            Projects = new ObservableCollection<Project>
            {
                new Project { Id = 0, Name = "All" }, // Thêm option "All" với ProjectId = 0
                new Project { Id = 1, Name = "Dự án A" },
                new Project { Id = 2, Name = "Dự án B" }
            };
            SelectedProjectFilter = Projects[0]; // Mặc định chọn "All"

            StatusFilters = new ObservableCollection<string> { "All", "Not Started", "In Progress", "Completed" };
            SelectedStatusFilter = "All";

            UpdateDashboardStats();
        }

        public void UpdateDashboardStats()
        {
            var filtered = TeamTasks.AsEnumerable();
            if (SelectedProjectFilter != null && SelectedProjectFilter.Id != 0) // Chỉ lọc nếu không phải "All"
            {
                filtered = filtered.Where(t => t.ProjectId == SelectedProjectFilter.Id);
            }

            TeamInProgressCount = filtered.Count(t => t.Status == "In Progress");
            TeamCompletedCount = filtered.Count(t => t.Status == "Completed");
            TeamOverdueCount = filtered.Count(t => DateTime.Parse(t.DueDate) < DateTime.Now && t.Status != "Completed");
        }

        [RelayCommand]
        private void NavigateToDashboard()
        {
            CurrentView = "Dashboard";
            FilterDashboard();
        }

        [RelayCommand]
        private void NavigateToManageTasks()
        {
            CurrentView = "ManageTasks";
            FilterTasks();
        }

        [RelayCommand]
        private void NavigateToProfile()
        {
            MessageBox.Show("Chuyển đến Thông tin Cá nhân (chưa triển khai).");
        }

        [RelayCommand]
        private void Logout()
        {
            var loginView = new LoginView();
            loginView.Show();
            Application.Current.Windows[0]?.Close();
        }

        [RelayCommand]
        private void FilterDashboard()
        {
            UpdateDashboardStats();
        }

        [RelayCommand]
        public void FilterTasks()
        {
            var filtered = TeamTasks.AsEnumerable();

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
        private void ViewTask(WorkTask workTask)
        {
            MessageBox.Show($"Xem chi tiết công việc: {workTask.Title}\nMô tả: {workTask.Description}\nGiao cho: {workTask.AssignedTo}\nDự án: {workTask.ProjectId}");
        }

        [RelayCommand]
        private void EditTask(WorkTask workTask)
        {
            MessageBox.Show($"Chỉnh sửa công việc: {workTask.Title} (chưa triển khai).");
        }

        [RelayCommand]
        private void AddTask()
        {
            var addTaskWindow = new AddTaskWindow(this);
            addTaskWindow.ShowDialog();
        }

        [RelayCommand]
        private void AssignTask()
        {
            if (string.IsNullOrWhiteSpace(NewTaskTitle) || string.IsNullOrWhiteSpace(NewTaskDescription) ||
                string.IsNullOrWhiteSpace(NewTaskDueDate) || SelectedEmployee == null)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                return;
            }

            var newTask = new WorkTask
            {
                TaskId = TeamTasks.Count + 1,
                Title = NewTaskTitle,
                Description = NewTaskDescription,
                DueDate = NewTaskDueDate,
                Status = "Not Started",
                AssignedTo = SelectedEmployee.Name,
                ProjectId = SelectedProjectFilter?.Id ?? 1 // Giả định dự án mặc định nếu không chọn
            };

            TeamTasks.Add(newTask);
            FilterTasks();
            UpdateDashboardStats();

            // Reset form
            NewTaskTitle = string.Empty;
            NewTaskDescription = string.Empty;
            NewTaskDueDate = string.Empty;
            SelectedEmployee = null;

            MessageBox.Show("Giao việc thành công!");
        }
    }
}