using Microsoft.EntityFrameworkCore;
using SPM_Backend.Models;

namespace SPM_Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<SPM_Role> Roles { get; set; }
        public DbSet<SPM_UserType> UserTypes { get; set; }
        public DbSet<SPM_User> Users { get; set; }
        public DbSet<SPM_UserRole> UserRoles { get; set; }
        public DbSet<SPM_TaskStatus> TaskStatuses { get; set; }
        public DbSet<SPM_TaskPriority> TaskPriorities { get; set; }
        public DbSet<SPM_ProjectMaster> ProjectMasters { get; set; }
        public DbSet<SPM_ProjectAllocation> ProjectAllocations { get; set; }
        public DbSet<SPM_Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var fk in modelBuilder.Model.GetEntityTypes()
                                                 .SelectMany(e => e.GetForeignKeys()))
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }
    }
}
