using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SPM_Backend.Models
{
    [Table("SPM_User")]
    [Index(nameof(Email), IsUnique = true)]
    public class SPM_User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        public int UserTypeID { get; set; }

        [ForeignKey(nameof(UserTypeID))]
        public SPM_UserType? UserType { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = null!;

        [MaxLength(100)]
        public string? UserCode { get; set; }

        [Required]
        [MaxLength(150)]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        [Required]
        [MaxLength(15)]
        public string MobileNumber { get; set; } = null!;

        [Required]
        [MaxLength(500)]
        public string ProfilePicturePath { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public ICollection<SPM_UserRole> UserRoles { get; set; } = new List<SPM_UserRole>();

        [InverseProperty(nameof(SPM_ProjectAllocation.Student))]
        public ICollection<SPM_ProjectAllocation> StudentAllocations { get; set; } = new List<SPM_ProjectAllocation>();

        [InverseProperty(nameof(SPM_ProjectAllocation.Faculty))]
        public ICollection<SPM_ProjectAllocation> FacultyAllocations { get; set; } = new List<SPM_ProjectAllocation>();
    }
}
