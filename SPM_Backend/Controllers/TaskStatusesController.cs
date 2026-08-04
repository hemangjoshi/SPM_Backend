using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
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
        var statuses = await _context.TaskStatuses.ToListAsync();
        return Ok(statuses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTaskStatus(int id)
    {
        var status = await _context.TaskStatuses.FindAsync(id);
        if (status == null) return NotFound();
        return Ok(status);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SPM_TaskStatus taskStatus)
    {
        _context.TaskStatuses.Add(taskStatus);
        await _context.SaveChangesAsync();
        return Ok(taskStatus);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SPM_TaskStatus taskStatus)
    {
        if (id != taskStatus.TaskStatusID) return BadRequest();

        var existing = await _context.TaskStatuses.FindAsync(id);
        if (existing == null) return NotFound();

        existing.TaskStatusName = taskStatus.TaskStatusName;
        existing.TaskStatusCssClass = taskStatus.TaskStatusCssClass;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var status = await _context.TaskStatuses.FindAsync(id);
        if (status == null) return NotFound();

        _context.TaskStatuses.Remove(status);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
