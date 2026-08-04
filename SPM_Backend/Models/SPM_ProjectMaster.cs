using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPM_Backend.Models
{
    [Table("SPM_ProjectMaster")]
    public class SPM_ProjectMaster
    {
        [Key]
        public int ProjectID { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProjectTitle { get; set; } = null!;

        public string? Description { get; set; }

        public ICollection<SPM_ProjectAllocation> ProjectAllocations { get; set; } = new List<SPM_ProjectAllocation>();
    }
}
