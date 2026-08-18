namespace SPM_Backend.DTOs
{
    public class UserDTO
    {
        public int UserID { get; set; }

        public int UserTypeID { get; set; }

        public string? UserTypeName { get; set; }

        public string? FullName { get; set; }

        public string? UserCode { get; set; }

        public string? Email { get; set; }

        public string? MobileNumber { get; set; }

        public string? ProfilePicturePath { get; set; }

        public bool IsActive { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
