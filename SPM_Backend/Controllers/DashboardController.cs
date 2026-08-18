using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Common;
using SPM_Backend.Data;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    // Q1 - Total number of students
    [HttpGet("total-students")]
    public async Task<IActionResult> TotalStudents()
    {
        var totalStudents = await _context.Users
            .CountAsync(x => x.UserType.UserTypeName == "Student");

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Total Students Retrieved Successfully",
            Data    = new { TotalStudents = totalStudents }
        });
    }

    // Q2 - Total number of faculty members
    [HttpGet("total-faculty")]
    public async Task<IActionResult> TotalFaculty()
    {
        var totalFaculty = await _context.Users.CountAsync(x => x.UserType.UserTypeName == "Faculty");

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Total Faculty Retrieved Successfully",
            Data    = new { TotalFaculty = totalFaculty }
        });
    }

    // Q3 - Total number of projects
    [HttpGet("total-projects")]
    public async Task<IActionResult> TotalProjects()
    {
        var totalProjects = await _context.ProjectMasters.CountAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Total Projects Retrieved Successfully",
            Data    = new { TotalProjects = totalProjects }
        });
    }

    // Q4 - Task count by status
    [HttpGet("task-status-summary")]
    public async Task<IActionResult> TaskStatusSummary()
    {
        var result = await _context.Tasks
            .GroupBy(t => t.TaskStatus.TaskStatusName)
            .Select(g => new
            {
                TaskStatus = g.Key,
                TotalTasks = g.Count()
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Task Status Summary Retrieved Successfully",
            Data    = result
        });
    }

    // Q5 - Task count by priority
    [HttpGet("task-priority-summary")]
    public async Task<IActionResult> TaskPrioritySummary()
    {
        var result = await _context.Tasks
            .GroupBy(t => t.TaskPriority.TaskPriorityName)
            .Select(g => new
            {
                Priority   = g.Key,
                TotalTasks = g.Count()
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Task Priority Summary Retrieved Successfully",
            Data    = result
        });
    }

    // Q6 - Projects assigned to each faculty
    [HttpGet("faculty-workload")]
    public async Task<IActionResult> FacultyWorkload()
    {
        var result = await _context.ProjectAllocations
            .GroupBy(p => p.Faculty.FullName)
            .Select(g => new
            {
                FacultyName   = g.Key,
                TotalProjects = g.Count()
            })
            .OrderByDescending(x => x.TotalProjects)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Faculty Workload Retrieved Successfully",
            Data    = result
        });
    }

    // Q7 - Tasks assigned to each student
    [HttpGet("student-task-count")]
    public async Task<IActionResult> StudentTaskCount()
    {
        var result = await _context.Tasks
            .GroupBy(t => t.ProjectAllocation.Student.FullName)
            .Select(g => new
            {
                StudentName = g.Key,
                TotalTasks  = g.Count()
            })
            .OrderByDescending(x => x.TotalTasks)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Student Task Count Retrieved Successfully",
            Data    = result
        });
    }

    // Q8 - Top 10 students by average earned score
    [HttpGet("top-students")]
    public async Task<IActionResult> TopStudents()
    {
        var result = await _context.Tasks
            .Where(t => t.EarnedScore != null)
            .GroupBy(t => t.ProjectAllocation.Student.FullName)
            .Select(g => new
            {
                StudentName  = g.Key,
                AverageScore = g.Average(t => t.EarnedScore)
            })
            .OrderByDescending(x => x.AverageScore)
            .Take(10)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Top Students Retrieved Successfully",
            Data    = result
        });
    }

    // Q9 - Bottom 10 students by average earned score
    [HttpGet("bottom-students")]
    public async Task<IActionResult> BottomStudents()
    {
        var result = await _context.Tasks
            .Where(t => t.EarnedScore != null)
            .GroupBy(t => t.ProjectAllocation.Student.FullName)
            .Select(g => new
            {
                StudentName  = g.Key,
                TotalTasks   = g.Count(),
                AverageScore = g.Average(t => t.EarnedScore)
            })
            .OrderBy(x => x.AverageScore)
            .Take(10)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Bottom Students Retrieved Successfully",
            Data    = result
        });
    }

    // Q10 - Overdue tasks (due date passed, not completed)
    [HttpGet("overdue-tasks")]
    public async Task<IActionResult> OverdueTasks()
    {
        var result = await _context.Tasks
            .Where(t =>
                t.TaskDueDate < DateTime.Now && t.TaskStatus.TaskStatusName != "Completed")
            .Select(t => new
            {
                t.TaskID,t.TaskTitle,
                Student     = t.ProjectAllocation.Student.FullName,
                Faculty     = t.ProjectAllocation.Faculty.FullName,
                t.TaskDueDate,DaysOverdue = EF.Functions.DateDiffDay(t.TaskDueDate, DateTime.Now)
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Overdue Tasks Retrieved Successfully",
            Data    = result
        });
    }

    // Q11 - Tasks with follow-up dates in next 7 days
    [HttpGet("upcoming-followups")]
    public async Task<IActionResult> UpcomingFollowUps()
    {
        var result = await _context.Tasks
            .Where(t =>
                t.NextFollowUpDate >= DateTime.Today && t.NextFollowUpDate <= DateTime.Today.AddDays(7))
            .Select(t => new
            {
                t.TaskTitle,
                Student           = t.ProjectAllocation.Student.FullName,
                Faculty           = t.ProjectAllocation.Faculty.FullName,
                t.NextFollowUpDate
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Upcoming Follow-Ups Retrieved Successfully",
            Data    = result
        });
    }

    // Q12 - Grade distribution (students per grade)
    [HttpGet("grade-distribution")]
    public async Task<IActionResult> GradeDistribution()
    {
        var result = await _context.ProjectAllocations
            .GroupBy(p => p.OverAllGrade)
            .Select(g => new
            {
                Grade    = g.Key,
                Students = g.Count()
            })
            .OrderBy(x => x.Grade)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Grade Distribution Retrieved Successfully",
            Data    = result
        });
    }

    // Q13 - Month-wise completed task count (with month name)
    [HttpGet("monthly-completion")]
    public async Task<IActionResult> MonthlyCompletion()
    {
        var result = await _context.Tasks
            .Where(t => t.TaskCompletedDate != null)
            .GroupBy(t => new
            {
                Year  = t.TaskCompletedDate!.Value.Year,
                Month = t.TaskCompletedDate!.Value.Month
            })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                TotalCompletedTasks = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Monthly Completion Data Retrieved Successfully",
            Data    = result
        });
    }

    // Q14 - Role-wise active user count
    [HttpGet("role-active-users")]
    public async Task<IActionResult> RoleActiveUsers()
    {
        var result = await _context.UserRoles
            .Where(x => x.User.IsActive)
            .GroupBy(x => x.Role.RoleName)
            .Select(g => new
            {
                RoleName    = g.Key,
                ActiveUsers = g.Count()
            })
            .OrderByDescending(x => x.ActiveUsers)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Role Active Users Retrieved Successfully",
            Data    = result
        });
    }

    // Q15 - Each role with users assigned to it
    [HttpGet("role-users")]
    public async Task<IActionResult> RoleUsers()
    {
        var result = await _context.UserRoles
            .GroupBy(x => x.Role.RoleName)
            .Select(g => new
            {
                RoleName = g.Key,
                Users    = g.Select(x => x.User.FullName).ToList()
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Role Users Retrieved Successfully",
            Data    = result
        });
    }

    // Q16 - Roles having more than 10 users
    [HttpGet("roles-more-than-10-users")]
    public async Task<IActionResult> RolesMoreThan10Users()
    {
        var result = await _context.UserRoles
            .GroupBy(x => x.Role.RoleName)
            .Select(g => new
            {
                RoleName   = g.Key,
                TotalUsers = g.Count()
            })
            .Where(x => x.TotalUsers > 10)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Roles With More Than 10 Users Retrieved Successfully",
            Data    = result
        });
    }

    // Q17 - Role statistics (total, active, inactive)
    [HttpGet("role-statistics")]
    public async Task<IActionResult> RoleStatistics()
    {
        var result = await _context.UserRoles
            .GroupBy(x => x.Role.RoleName)
            .Select(g => new
            {
                RoleName      = g.Key,
                TotalUsers    = g.Count(),
                ActiveUsers   = g.Count(x => x.User.IsActive),
                InactiveUsers = g.Count(x => !x.User.IsActive)
            })
            .OrderByDescending(x => x.TotalUsers)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Role Statistics Retrieved Successfully",
            Data    = result
        });
    }

    // Q18 - Tasks due within next 7 days
    [HttpGet("tasks-due-soon")]
    public async Task<IActionResult> TasksDueSoon()
    {
        var result = await _context.Tasks
            .Where(x =>
                x.TaskDueDate >= DateTime.Today &&
                x.TaskDueDate <= DateTime.Today.AddDays(7))
            .Select(x => new
            {
                x.TaskID,
                x.TaskTitle,
                Project       = x.ProjectAllocation.Project.ProjectTitle,
                Student       = x.ProjectAllocation.Student.FullName,
                x.TaskDueDate,
                DaysRemaining = EF.Functions.DateDiffDay(DateTime.Today, x.TaskDueDate)
            })
            .OrderBy(x => x.TaskDueDate)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Tasks Due Soon Retrieved Successfully",
            Data    = result
        });
    }

    // Q19 - Project-wise task summary with average progress
    [HttpGet("project-task-summary")]
    public async Task<IActionResult> ProjectTaskSummary()
    {
        var result = await _context.Tasks
            .GroupBy(x => x.ProjectAllocation.Project.ProjectTitle)
            .Select(g => new
            {
                Project         = g.Key,
                TotalTasks      = g.Count(),
                CompletedTasks  = g.Count(x => x.TaskStatus.TaskStatusName == "Completed"),
                PendingTasks    = g.Count(x => x.TaskStatus.TaskStatusName == "Pending"),
                AverageProgress = g.Average(x => x.ProgressPercentage)
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Project Task Summary Retrieved Successfully",
            Data    = result
        });
    }

    // Q20 - Project-wise score summary (assigned, earned, percentage)
    [HttpGet("project-score-summary")]
    public async Task<IActionResult> ProjectScoreSummary()
    {
        var result = await _context.Tasks
            .GroupBy(x => x.ProjectAllocation.Project.ProjectTitle)
            .Select(g => new
            {
                Project            = g.Key,
                TotalAssignedScore = g.Sum(x => x.AssignedScore),
                TotalEarnedScore   = g.Sum(x => x.EarnedScore ?? 0),
                ScorePercentage    = g.Sum(x => x.AssignedScore) == 0
                    ? 0
                    : (g.Sum(x => x.EarnedScore ?? 0) / g.Sum(x => x.AssignedScore)) * 100
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Project Score Summary Retrieved Successfully",
            Data    = result
        });
    }

    // Q21 - Top 10 projects by average earned score
    [HttpGet("top-projects")]
    public async Task<IActionResult> TopProjects()
    {
        var result = await _context.Tasks
            .Where(x => x.EarnedScore != null)
            .GroupBy(x => x.ProjectAllocation.Project.ProjectTitle)
            .Select(g => new
            {
                Project      = g.Key,
                AverageScore = g.Average(x => x.EarnedScore)
            })
            .OrderByDescending(x => x.AverageScore)
            .Take(10)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Top Projects Retrieved Successfully",
            Data    = result
        });
    }

    // Q22 - Faculty-wise project count, task count, average progress
    [HttpGet("faculty-summary")]
    public async Task<IActionResult> FacultySummary()
    {
        var result = await _context.ProjectAllocations
            .GroupBy(x => x.Faculty.FullName)
            .Select(g => new
            {
                Faculty         = g.Key,
                TotalProjects   = g.Count(),
                TotalTasks      = g.Sum(x => x.TotalTasksGiven),
                AverageProgress = g.Average(x => x.ProgressPercentage)
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Faculty Summary Retrieved Successfully",
            Data    = result
        });
    }

    // Q23 - Student-wise task completion stats and average score
    [HttpGet("student-summary")]
    public async Task<IActionResult> StudentSummary()
    {
        var result = await _context.Tasks
            .GroupBy(x => x.ProjectAllocation.Student.FullName)
            .Select(g => new
            {
                Student        = g.Key,
                TotalTasks     = g.Count(),
                CompletedTasks = g.Count(x => x.TaskStatus.TaskStatusName == "Completed"),
                PendingTasks   = g.Count(x => x.TaskStatus.TaskStatusName == "Pending"),
                AverageScore   = g.Average(x => x.EarnedScore)
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Student Summary Retrieved Successfully",
            Data    = result
        });
    }

    // Q24 - Projects past expected completion date but still incomplete
    [HttpGet("overdue-projects")]
    public async Task<IActionResult> OverdueProjects()
    {
        var result = await _context.ProjectAllocations
            .Where(x =>
                x.ProjectEndDate < DateTime.Now &&
                x.ProgressPercentage < 100)
            .Select(x => new
            {
                Project             = x.Project.ProjectTitle,
                Student             = x.Student.FullName,
                Faculty             = x.Faculty.FullName,
                x.ProjectEndDate,
                x.ProgressPercentage
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Overdue Projects Retrieved Successfully",
            Data    = result
        });
    }

    // Q25 - Month-wise completed task count (numeric month)
    [HttpGet("monthly-task-count")]
    public async Task<IActionResult> MonthlyTaskCount()
    {
        var result = await _context.Tasks
            .Where(x => x.TaskCompletedDate != null)
            .GroupBy(x => new
            {
                Year  = x.TaskCompletedDate!.Value.Year,
                Month = x.TaskCompletedDate!.Value.Month
            })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                CompletedTasks = g.Count()
            })
            .OrderBy(x => x.Year)
            .ThenBy(x => x.Month)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Monthly Task Count Retrieved Successfully",
            Data    = result
        });
    }

    // Q26 - Faculty ranked by average project progress
    [HttpGet("faculty-ranking")]
    public async Task<IActionResult> FacultyRanking()
    {
        var result = await _context.ProjectAllocations
            .GroupBy(x => x.Faculty.FullName)
            .Select(g => new
            {
                Faculty         = g.Key,
                AverageProgress = g.Average(x => x.ProgressPercentage)
            })
            .OrderByDescending(x => x.AverageProgress)
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Faculty Ranking Retrieved Successfully",
            Data    = result
        });
    }

    // Q27 - Task statistics for every project (total, completed, pending, overdue)
    [HttpGet("project-task-statistics")]
    public async Task<IActionResult> ProjectTaskStatistics()
    {
        var result = await _context.Tasks
            .GroupBy(x => x.ProjectAllocation.Project.ProjectTitle)
            .Select(g => new
            {
                Project        = g.Key,
                TotalTasks     = g.Count(),
                CompletedTasks = g.Count(x => x.TaskStatus.TaskStatusName == "Completed"),
                PendingTasks   = g.Count(x => x.TaskStatus.TaskStatusName == "Pending"),
                OverdueTasks   = g.Count(x =>
                    x.TaskDueDate < DateTime.Now &&
                    x.TaskStatus.TaskStatusName != "Completed")
            })
            .ToListAsync();

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Project Task Statistics Retrieved Successfully",
            Data    = result
        });
    }
}
