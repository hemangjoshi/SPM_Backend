using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Common;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class TaskStatusesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskStatusesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaskStatuses()
    {
        var statuses = await _context.TaskStatuses
            .Select(x => new TaskStatusDTO
            {
                TaskStatusID       = x.TaskStatusID,
                TaskStatusName     = x.TaskStatusName,
                TaskStatusCssClass = x.TaskStatusCssClass
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<TaskStatusDTO>>
        {
            Success = true,
            Message = "Task Statuses Retrieved Successfully",
            Data    = statuses
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTaskStatus(int id)
    {
        var status = await _context.TaskStatuses
            .Where(x => x.TaskStatusID == id)
            .Select(x => new TaskStatusDTO
            {
                TaskStatusID       = x.TaskStatusID,
                TaskStatusName     = x.TaskStatusName,
                TaskStatusCssClass = x.TaskStatusCssClass
            })
            .FirstOrDefaultAsync();

        if (status == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Task Status Not Found",
                Errors  = new List<string> { $"No task status found with Id {id}" }
            });

        return Ok(new ApiResponse<TaskStatusDTO>
        {
            Success = true,
            Message = "Task Status Retrieved Successfully",
            Data    = status
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskStatusDTO dto)
    {
        try
        {
            var taskStatus = new SPM_TaskStatus
            {
                TaskStatusName     = dto.TaskStatusName!,
                TaskStatusCssClass = dto.TaskStatusCssClass!
            };

            _context.TaskStatuses.Add(taskStatus);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<TaskStatusDTO>
            {
                Success = true,
                Message = "Task Status Added Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while adding task status",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TaskStatusDTO dto)
    {
        if (id != dto.TaskStatusID)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "ID Mismatch",
                Errors  = new List<string> { "Route ID does not match the DTO TaskStatusID" }
            });

        try
        {
            var existing = await _context.TaskStatuses.FindAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Task Status Not Found",
                    Errors  = new List<string> { $"No task status found with Id {id}" }
                });

            existing.TaskStatusName     = dto.TaskStatusName!;
            existing.TaskStatusCssClass = dto.TaskStatusCssClass!;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<TaskStatusDTO>
            {
                Success = true,
                Message = "Task Status Updated Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while updating task status",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var status = await _context.TaskStatuses.FindAsync(id);

            if (status == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Task Status Not Found",
                    Errors  = new List<string> { $"No task status found with Id {id}" }
                });

            _context.TaskStatuses.Remove(status);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task Status Deleted Successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while deleting task status",
                Errors  = new List<string> { ex.Message }
            });
        }
    }
}
