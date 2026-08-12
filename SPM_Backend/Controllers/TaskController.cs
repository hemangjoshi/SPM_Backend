using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
using SPM_Backend.Models;
using System.Data;

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
        var tasks = await _context.Tasks.ToListAsync();
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
            return NotFound();

        return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SPM_Task task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SPM_Task task)
    {
        if (id != task.TaskID)
            return BadRequest();

        var existing = await _context.Tasks.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.ProjectAllocationID = task.ProjectAllocationID;
        existing.TaskTitle = task.TaskTitle;
        existing.TaskDescription = task.TaskDescription;
        existing.TaskStatusID = task.TaskStatusID;
        existing.TaskPriorityID = task.TaskPriorityID;
        existing.AssignedScore = task.AssignedScore;
        existing.EarnedScore = task.EarnedScore;
        existing.ProgressPercentage = task.ProgressPercentage;
        existing.TaskAssignedDate = task.TaskAssignedDate;
        existing.TaskStartDate = task.TaskStartDate;
        existing.TaskDueDate = task.TaskDueDate;
        existing.TaskCompletedDate = task.TaskCompletedDate;
        existing.NextFollowUpDate = task.NextFollowUpDate;
        existing.FacultyRemarks = task.FacultyRemarks;
        existing.StudentRemarks = task.StudentRemarks;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task == null)
            return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
