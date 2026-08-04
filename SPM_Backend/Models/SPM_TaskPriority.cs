using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPM_Backend.Models
{
    [Table("SPM_TaskPriority")]
    public class SPM_TaskPriority
    {
        [Key]
        public int TaskPriorityID { get; set; }

        [Required]
        [MaxLength(20)]
        public string TaskPriorityName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        [Column("TaskPriortyCssClass")]
        public string TaskPriortyCssClass { get; set; } = null!;

        // Inverse Navigation Properties
        public ICollection<SPM_Task> Tasks { get; set; } = new List<SPM_Task>();
    }
}
