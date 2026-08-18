using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Common;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class TaskPrioritiesController : ControllerBase
{
    private readonly AppDbContext _context;

    public TaskPrioritiesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTaskPriorities()
    {
        var priorities = await _context.TaskPriorities
            .Select(x => new TaskPriorityDTO
            {
                TaskPriorityID      = x.TaskPriorityID,
                TaskPriorityName    = x.TaskPriorityName,
                TaskPriortyCssClass = x.TaskPriortyCssClass
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<TaskPriorityDTO>>
        {
            Success = true,
            Message = "Task Priorities Retrieved Successfully",
            Data    = priorities
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTaskPriority(int id)
    {
        var priority = await _context.TaskPriorities
            .Where(x => x.TaskPriorityID == id)
            .Select(x => new TaskPriorityDTO
            {
                TaskPriorityID      = x.TaskPriorityID,
                TaskPriorityName    = x.TaskPriorityName,
                TaskPriortyCssClass = x.TaskPriortyCssClass
            })
            .FirstOrDefaultAsync();

        if (priority == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Task Priority Not Found",
                Errors  = new List<string> { $"No task priority found with Id {id}" }
            });

        return Ok(new ApiResponse<TaskPriorityDTO>
        {
            Success = true,
            Message = "Task Priority Retrieved Successfully",
            Data    = priority
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskPriorityDTO dto)
    {
        try
        {
            var taskPriority = new SPM_TaskPriority
            {
                TaskPriorityName    = dto.TaskPriorityName!,
                TaskPriortyCssClass = dto.TaskPriortyCssClass!
            };

            _context.TaskPriorities.Add(taskPriority);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<TaskPriorityDTO>
            {
                Success = true,
                Message = "Task Priority Added Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while adding task priority",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TaskPriorityDTO dto)
    {
        if (id != dto.TaskPriorityID)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "ID Mismatch",
                Errors  = new List<string> { "Route ID does not match the DTO TaskPriorityID" }
            });

        try
        {
            var existing = await _context.TaskPriorities.FindAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Task Priority Not Found",
                    Errors  = new List<string> { $"No task priority found with Id {id}" }
                });

            existing.TaskPriorityName    = dto.TaskPriorityName!;
            existing.TaskPriortyCssClass = dto.TaskPriortyCssClass!;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<TaskPriorityDTO>
            {
                Success = true,
                Message = "Task Priority Updated Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while updating task priority",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var priority = await _context.TaskPriorities.FindAsync(id);

            if (priority == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Task Priority Not Found",
                    Errors  = new List<string> { $"No task priority found with Id {id}" }
                });

            _context.TaskPriorities.Remove(priority);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task Priority Deleted Successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while deleting task priority",
                Errors  = new List<string> { ex.Message }
            });
        }
    }
}
