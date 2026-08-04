using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPM_Backend.Models
{
    [Table("SPM_TaskStatus")]
    public class SPM_TaskStatus
    {
        [Key]
        public int TaskStatusID { get; set; }

        [Required]
        [MaxLength(20)]
        public string TaskStatusName { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string TaskStatusCssClass { get; set; } = null!;

        // Inverse Navigation Properties
        public ICollection<SPM_Task> Tasks { get; set; } = new List<SPM_Task>();
    }
}
