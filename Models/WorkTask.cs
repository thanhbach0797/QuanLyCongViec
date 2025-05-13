namespace TaskManagementApp.Models
{
    public class WorkTask
    {
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string DueDate { get; set; }
        public string AssignedTo { get; set; }
        public int ProjectId { get; set; } // Thêm ProjectId để liên kết với dự án
    }
}