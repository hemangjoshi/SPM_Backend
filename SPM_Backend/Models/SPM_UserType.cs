using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SPM_Backend.Models
{
    [Table("SPM_UserType")]
    public class SPM_UserType
    {
        [Key]
        public int UserTypeID { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserTypeName { get; set; } = null!;

        [MaxLength(250)]
        public string? Description { get; set; }

        // Inverse Navigation Properties
        public ICollection<SPM_User> Users { get; set; } = new List<SPM_User>();
    }
}
