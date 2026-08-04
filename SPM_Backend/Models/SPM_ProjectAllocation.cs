using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPM_Backend.Models
{
    [Table("SPM_ProjectAllocation")]
    public class SPM_ProjectAllocation
    {
        [Key]
        public int ProjectAllocationID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [ForeignKey(nameof(ProjectID))]
        public SPM_ProjectMaster? Project { get; set; }

        [Required]
        public int StudentID { get; set; }

        [ForeignKey(nameof(StudentID))]
        public SPM_User? Student { get; set; }

        [Required]
        public int FacultyID { get; set; }

        [ForeignKey(nameof(FacultyID))]
        public SPM_User? Faculty { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime AssignedDate { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ProjectStartDate { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ProjectEndDate { get; set; }

        [Required]
        public int TotalTasksGiven { get; set; }

        [Required]
        public int TotalCompletedTasks { get; set; }

        [Required]
        [Column(TypeName = "decimal(5,2)")]
        public decimal ProgressPercentage { get; set; }

        [MaxLength(1)]
        public string? OverAllGrade { get; set; }

        public ICollection<SPM_Task> Tasks { get; set; } = new List<SPM_Task>();
    }
}
