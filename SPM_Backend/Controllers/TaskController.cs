using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Common;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var tasks = await _context.Tasks
            .Include(x => x.ProjectAllocation).ThenInclude(x => x!.Project)
            .Include(x => x.ProjectAllocation).ThenInclude(x => x!.Student)
            .Include(x => x.TaskStatus)
            .Include(x => x.TaskPriority)
            .Select(x => new TaskDTO
            {
                TaskID              = x.TaskID,
                ProjectAllocationID = x.ProjectAllocationID,
                ProjectTitle        = x.ProjectAllocation!.Project!.ProjectTitle,
                StudentName         = x.ProjectAllocation!.Student!.FullName,
                TaskTitle           = x.TaskTitle,
                TaskDescription     = x.TaskDescription,
                TaskStatusID        = x.TaskStatusID,
                TaskStatusName      = x.TaskStatus!.TaskStatusName,
                TaskPriorityID      = x.TaskPriorityID,
                TaskPriorityName    = x.TaskPriority!.TaskPriorityName,
                AssignedScore       = x.AssignedScore,
                EarnedScore         = x.EarnedScore,
                ProgressPercentage  = x.ProgressPercentage,
                TaskAssignedDate    = x.TaskAssignedDate,
                TaskStartDate       = x.TaskStartDate,
                TaskDueDate         = x.TaskDueDate,
                TaskCompletedDate   = x.TaskCompletedDate,
                NextFollowUpDate    = x.NextFollowUpDate,
                FacultyRemarks      = x.FacultyRemarks,
                StudentRemarks      = x.StudentRemarks
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<TaskDTO>>
        {
            Success = true,
            Message = "Tasks Retrieved Successfully",
            Data    = tasks
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _context.Tasks
            .Include(x => x.ProjectAllocation).ThenInclude(x => x!.Project)
            .Include(x => x.ProjectAllocation).ThenInclude(x => x!.Student)
            .Include(x => x.TaskStatus)
            .Include(x => x.TaskPriority)
            .Where(x => x.TaskID == id)
            .Select(x => new TaskDTO
            {
                TaskID              = x.TaskID,
                ProjectAllocationID = x.ProjectAllocationID,
                ProjectTitle        = x.ProjectAllocation!.Project!.ProjectTitle,
                StudentName         = x.ProjectAllocation!.Student!.FullName,
                TaskTitle           = x.TaskTitle,
                TaskDescription     = x.TaskDescription,
                TaskStatusID        = x.TaskStatusID,
                TaskStatusName      = x.TaskStatus!.TaskStatusName,
                TaskPriorityID      = x.TaskPriorityID,
                TaskPriorityName    = x.TaskPriority!.TaskPriorityName,
                AssignedScore       = x.AssignedScore,
                EarnedScore         = x.EarnedScore,
                ProgressPercentage  = x.ProgressPercentage,
                TaskAssignedDate    = x.TaskAssignedDate,
                TaskStartDate       = x.TaskStartDate,
                TaskDueDate         = x.TaskDueDate,
                TaskCompletedDate   = x.TaskCompletedDate,
                NextFollowUpDate    = x.NextFollowUpDate,
                FacultyRemarks      = x.FacultyRemarks,
                StudentRemarks      = x.StudentRemarks
            })
            .FirstOrDefaultAsync();

        if (task == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Task Not Found",
                Errors  = new List<string> { $"No task found with Id {id}" }
            });

        return Ok(new ApiResponse<TaskDTO>
        {
            Success = true,
            Message = "Task Retrieved Successfully",
            Data    = task
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskDTO dto)
    {
        try
        {
            var task = new SPM_Task
            {
                ProjectAllocationID = dto.ProjectAllocationID,
                TaskTitle           = dto.TaskTitle!,
                TaskDescription     = dto.TaskDescription,
                TaskStatusID        = dto.TaskStatusID,
                TaskPriorityID      = dto.TaskPriorityID,
                AssignedScore       = dto.AssignedScore,
                EarnedScore         = dto.EarnedScore,
                ProgressPercentage  = dto.ProgressPercentage,
                TaskAssignedDate    = dto.TaskAssignedDate,
                TaskStartDate       = dto.TaskStartDate,
                TaskDueDate         = dto.TaskDueDate,
                TaskCompletedDate   = dto.TaskCompletedDate,
                NextFollowUpDate    = dto.NextFollowUpDate,
                FacultyRemarks      = dto.FacultyRemarks,
                StudentRemarks      = dto.StudentRemarks
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<TaskDTO>
            {
                Success = true,
                Message = "Task Added Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while adding task",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TaskDTO dto)
    {
        if (id != dto.TaskID)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "ID Mismatch",
                Errors  = new List<string> { "Route ID does not match the DTO TaskID" }
            });

        try
        {
            var existing = await _context.Tasks.FindAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Task Not Found",
                    Errors  = new List<string> { $"No task found with Id {id}" }
                });

            existing.ProjectAllocationID = dto.ProjectAllocationID;
            existing.TaskTitle           = dto.TaskTitle!;
            existing.TaskDescription     = dto.TaskDescription;
            existing.TaskStatusID        = dto.TaskStatusID;
            existing.TaskPriorityID      = dto.TaskPriorityID;
            existing.AssignedScore       = dto.AssignedScore;
            existing.EarnedScore         = dto.EarnedScore;
            existing.ProgressPercentage  = dto.ProgressPercentage;
            existing.TaskAssignedDate    = dto.TaskAssignedDate;
            existing.TaskStartDate       = dto.TaskStartDate;
            existing.TaskDueDate         = dto.TaskDueDate;
            existing.TaskCompletedDate   = dto.TaskCompletedDate;
            existing.NextFollowUpDate    = dto.NextFollowUpDate;
            existing.FacultyRemarks      = dto.FacultyRemarks;
            existing.StudentRemarks      = dto.StudentRemarks;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<TaskDTO>
            {
                Success = true,
                Message = "Task Updated Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while updating task",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Task Not Found",
                    Errors  = new List<string> { $"No task found with Id {id}" }
                });

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task Deleted Successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while deleting task",
                Errors  = new List<string> { ex.Message }
            });
        }
    }
}
