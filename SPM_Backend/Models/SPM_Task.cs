using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPM_Backend.Models
{
    [Table("SPM_Task")]
    public class SPM_Task
    {
        [Key]
        public int TaskID { get; set; }

        [Required]
        public int ProjectAllocationID { get; set; }

        [ForeignKey(nameof(ProjectAllocationID))]
        public SPM_ProjectAllocation? ProjectAllocation { get; set; }

        [Required]
        [MaxLength(200)]
        public string TaskTitle { get; set; } = null!;

        public string? TaskDescription { get; set; }

        [Required]
        public int TaskStatusID { get; set; }

        [ForeignKey(nameof(TaskStatusID))]
        public SPM_TaskStatus? TaskStatus { get; set; }

        [Required]
        public int TaskPriorityID { get; set; }

        [ForeignKey(nameof(TaskPriorityID))]
        public SPM_TaskPriority? TaskPriority { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal AssignedScore { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? EarnedScore { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime TaskAssignedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? TaskStartDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? TaskDueDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? TaskCompletedDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? NextFollowUpDate { get; set; }

        [MaxLength(500)]
        public string? FacultyRemarks { get; set; }

        [MaxLength(500)]
        public string? StudentRemarks { get; set; }
    }
}
