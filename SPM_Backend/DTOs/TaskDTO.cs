namespace SPM_Backend.DTOs
{
    public class TaskDTO
    {
        public int TaskID { get; set; }

        public int ProjectAllocationID { get; set; }

        public string? ProjectTitle { get; set; }

        public string? StudentName { get; set; }

        public string? TaskTitle { get; set; }

        public string? TaskDescription { get; set; }

        public int TaskStatusID { get; set; }

        public string? TaskStatusName { get; set; }

        public int TaskPriorityID { get; set; }

        public string? TaskPriorityName { get; set; }

        public decimal AssignedScore { get; set; }

        public decimal? EarnedScore { get; set; }

        public decimal ProgressPercentage { get; set; }

        public DateTime TaskAssignedDate { get; set; }

        public DateTime? TaskStartDate { get; set; }

        public DateTime? TaskDueDate { get; set; }

        public DateTime? TaskCompletedDate { get; set; }

        public DateTime? NextFollowUpDate { get; set; }

        public string? FacultyRemarks { get; set; }

        public string? StudentRemarks { get; set; }
    }
}
