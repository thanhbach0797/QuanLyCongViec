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
    public partial class EmployeeViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<WorkTask> tasks;

        [ObservableProperty]
        private ObservableCollection<WorkTask> filteredTasks;

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
        private int inProgressCount;

        [ObservableProperty]
        private int completedCount;

        [ObservableProperty]
        private int overdueCount;

        public EmployeeViewModel()
        {
            // Dữ liệu giả lập
            Tasks = new ObservableCollection<WorkTask>
            {
                new WorkTask { TaskId = 1, Title = "Hoàn thành báo cáo", Description = "Báo cáo hàng tuần", Status = "In Progress", DueDate = "2025-05-20", AssignedTo = "Employee1", ProjectId = 1 },
                new WorkTask { TaskId = 2, Title = "Gặp khách hàng", Description = "Thảo luận yêu cầu dự án", Status = "Not Started", DueDate = "2025-05-22", AssignedTo = "Employee1", ProjectId = 2 },
                new WorkTask { TaskId = 3, Title = "Nộp tài liệu", Description = "Tài liệu dự án", Status = "Completed", DueDate = "2025-05-10", AssignedTo = "Employee1", ProjectId = 1 }
            };
            FilteredTasks = new ObservableCollection<WorkTask>(Tasks);

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

        private void UpdateDashboardStats()
        {
            var filtered = Tasks.AsEnumerable();
            if (SelectedProjectFilter != null && SelectedProjectFilter.Id != 0) // Chỉ lọc nếu không phải "All"
            {
                filtered = filtered.Where(t => t.ProjectId == SelectedProjectFilter.Id);
            }

            InProgressCount = filtered.Count(t => t.Status == "In Progress");
            CompletedCount = filtered.Count(t => t.Status == "Completed");
            OverdueCount = filtered.Count(t => DateTime.Parse(t.DueDate) < DateTime.Now && t.Status != "Completed");
        }

        [RelayCommand]
        private void NavigateToDashboard()
        {
            CurrentView = "Dashboard";
            FilterDashboard();
        }

        [RelayCommand]
        private void NavigateToMyTasks()
        {
            CurrentView = "MyTasks";
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
        private void FilterTasks()
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
        private void ViewTask(WorkTask workTask)
        {
            MessageBox.Show($"Xem chi tiết công việc: {workTask.Title}\nMô tả: {workTask.Description}");
        }

        [RelayCommand]
        private void EditTask(WorkTask workTask)
        {
            MessageBox.Show($"Chỉnh sửa công việc: {workTask.Title} (chưa triển khai).");
        }

        [RelayCommand]
        private void AddTask()
        {
            var addTaskWindow = new AddTaskWindow(null, null); // Employee không có quyền sửa danh sách chung
            addTaskWindow.ShowDialog();
        }
    }
}