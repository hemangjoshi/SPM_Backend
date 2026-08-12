using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPM_Backend.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SPM_ProjectMaster",
                columns: table => new
                {
                    ProjectID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_ProjectMaster", x => x.ProjectID);
                });

            migrationBuilder.CreateTable(
                name: "SPM_Role",
                columns: table => new
                {
                    RoleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_Role", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "SPM_TaskPriority",
                columns: table => new
                {
                    TaskPriorityID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskPriorityName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TaskPriortyCssClass = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_TaskPriority", x => x.TaskPriorityID);
                });

            migrationBuilder.CreateTable(
                name: "SPM_TaskStatus",
                columns: table => new
                {
                    TaskStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskStatusName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TaskStatusCssClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_TaskStatus", x => x.TaskStatusID);
                });

            migrationBuilder.CreateTable(
                name: "SPM_UserType",
                columns: table => new
                {
                    UserTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_UserType", x => x.UserTypeID);
                });

            migrationBuilder.CreateTable(
                name: "SPM_User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTypeID = table.Column<int>(type: "int", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UserCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MobileNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    ProfilePicturePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_SPM_User_SPM_UserType_UserTypeID",
                        column: x => x.UserTypeID,
                        principalTable: "SPM_UserType",
                        principalColumn: "UserTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SPM_ProjectAllocation",
                columns: table => new
                {
                    ProjectAllocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<int>(type: "int", nullable: false),
                    StudentID = table.Column<int>(type: "int", nullable: false),
                    FacultyID = table.Column<int>(type: "int", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProjectStartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ProjectEndDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TotalTasksGiven = table.Column<int>(type: "int", nullable: false),
                    TotalCompletedTasks = table.Column<int>(type: "int", nullable: false),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    OverAllGrade = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_ProjectAllocation", x => x.ProjectAllocationID);
                    table.ForeignKey(
                        name: "FK_SPM_ProjectAllocation_SPM_ProjectMaster_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "SPM_ProjectMaster",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_ProjectAllocation_SPM_User_FacultyID",
                        column: x => x.FacultyID,
                        principalTable: "SPM_User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_ProjectAllocation_SPM_User_StudentID",
                        column: x => x.StudentID,
                        principalTable: "SPM_User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SPM_UserRole",
                columns: table => new
                {
                    RolePermissionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_UserRole", x => x.RolePermissionID);
                    table.ForeignKey(
                        name: "FK_SPM_UserRole_SPM_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "SPM_Role",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_UserRole_SPM_User_UserID",
                        column: x => x.UserID,
                        principalTable: "SPM_User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SPM_Task",
                columns: table => new
                {
                    TaskID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectAllocationID = table.Column<int>(type: "int", nullable: false),
                    TaskTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskStatusID = table.Column<int>(type: "int", nullable: false),
                    TaskPriorityID = table.Column<int>(type: "int", nullable: false),
                    AssignedScore = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    EarnedScore = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ProgressPercentage = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    TaskAssignedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TaskStartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TaskDueDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TaskCompletedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    NextFollowUpDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    FacultyRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StudentRemarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SPM_Task", x => x.TaskID);
                    table.ForeignKey(
                        name: "FK_SPM_Task_SPM_ProjectAllocation_ProjectAllocationID",
                        column: x => x.ProjectAllocationID,
                        principalTable: "SPM_ProjectAllocation",
                        principalColumn: "ProjectAllocationID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_Task_SPM_TaskPriority_TaskPriorityID",
                        column: x => x.TaskPriorityID,
                        principalTable: "SPM_TaskPriority",
                        principalColumn: "TaskPriorityID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SPM_Task_SPM_TaskStatus_TaskStatusID",
                        column: x => x.TaskStatusID,
                        principalTable: "SPM_TaskStatus",
                        principalColumn: "TaskStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SPM_ProjectAllocation_FacultyID",
                table: "SPM_ProjectAllocation",
                column: "FacultyID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_ProjectAllocation_ProjectID",
                table: "SPM_ProjectAllocation",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_ProjectAllocation_StudentID",
                table: "SPM_ProjectAllocation",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Task_ProjectAllocationID",
                table: "SPM_Task",
                column: "ProjectAllocationID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Task_TaskPriorityID",
                table: "SPM_Task",
                column: "TaskPriorityID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_Task_TaskStatusID",
                table: "SPM_Task",
                column: "TaskStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_User_Email",
                table: "SPM_User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SPM_User_UserTypeID",
                table: "SPM_User",
                column: "UserTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_UserRole_RoleID",
                table: "SPM_UserRole",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_SPM_UserRole_UserID",
                table: "SPM_UserRole",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SPM_Task");

            migrationBuilder.DropTable(
                name: "SPM_UserRole");

            migrationBuilder.DropTable(
                name: "SPM_ProjectAllocation");

            migrationBuilder.DropTable(
                name: "SPM_TaskPriority");

            migrationBuilder.DropTable(
                name: "SPM_TaskStatus");

            migrationBuilder.DropTable(
                name: "SPM_Role");

            migrationBuilder.DropTable(
                name: "SPM_ProjectMaster");

            migrationBuilder.DropTable(
                name: "SPM_User");

            migrationBuilder.DropTable(
                name: "SPM_UserType");
        }
    }
}
