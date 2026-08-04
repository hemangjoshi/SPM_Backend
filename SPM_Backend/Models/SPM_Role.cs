using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPM_Backend.Models
{
    [Table("SPM_Role")]
    public class SPM_Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = null!;

        [MaxLength(250)]
        public string? Description { get; set; }

        public ICollection<SPM_UserRole> UserRoles { get; set; } = new List<SPM_UserRole>();
    }
}
