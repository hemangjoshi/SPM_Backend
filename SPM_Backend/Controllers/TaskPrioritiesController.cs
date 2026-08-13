using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
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
        var priorities = await _context.TaskPriorities.ToListAsync();
        return Ok(priorities);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTaskPriority(int id)
    {
        var priority = await _context.TaskPriorities.FindAsync(id);
        if (priority == null) return NotFound();
        return Ok(priority);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SPM_TaskPriority taskPriority)
    {
        _context.TaskPriorities.Add(taskPriority);
        await _context.SaveChangesAsync();
        return Ok(taskPriority);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SPM_TaskPriority taskPriority)
    {
        if (id != taskPriority.TaskPriorityID) return BadRequest();

        var existing = await _context.TaskPriorities.FindAsync(id);
        if (existing == null) return NotFound();

        existing.TaskPriorityName = taskPriority.TaskPriorityName;
        existing.TaskPriortyCssClass = taskPriority.TaskPriortyCssClass;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var priority = await _context.TaskPriorities.FindAsync(id);
        if (priority == null) return NotFound();

        _context.TaskPriorities.Remove(priority);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
