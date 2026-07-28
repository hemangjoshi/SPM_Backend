using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPM_Backend.Models
{
    [Table("SPM_UserRole")]
    public class SPM_UserRole
    {
        [Key]
        public int RolePermissionID { get; set; }

        [Required]
        public int RoleID { get; set; }

        [ForeignKey(nameof(RoleID))]
        public SPM_Role? Role { get; set; }

        [Required]
        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public SPM_User? User { get; set; }
    }
}
