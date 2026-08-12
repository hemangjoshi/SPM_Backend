using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
using SPM_Backend.Models;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class ProjectAllocationController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectAllocationController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjectAllocations()
    {
        var allocations = await _context.ProjectAllocations.ToListAsync();
        return Ok(allocations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectAllocation(int id)
    {
        var allocation = await _context.ProjectAllocations.FindAsync(id);

        if (allocation == null)
            return NotFound();

        return Ok(allocation);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SPM_ProjectAllocation allocation)
    {
        _context.ProjectAllocations.Add(allocation);
        await _context.SaveChangesAsync();

        return Ok(allocation);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SPM_ProjectAllocation allocation)
    {
        if (id != allocation.ProjectAllocationID)
            return BadRequest();

        var existing = await _context.ProjectAllocations.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.ProjectID = allocation.ProjectID;
        existing.StudentID = allocation.StudentID;
        existing.FacultyID = allocation.FacultyID;
        existing.AssignedDate = allocation.AssignedDate;
        existing.ProjectStartDate = allocation.ProjectStartDate;
        existing.ProjectEndDate = allocation.ProjectEndDate;
        existing.TotalTasksGiven = allocation.TotalTasksGiven;
        existing.TotalCompletedTasks = allocation.TotalCompletedTasks;
        existing.ProgressPercentage = allocation.ProgressPercentage;
        existing.OverAllGrade = allocation.OverAllGrade;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var allocation = await _context.ProjectAllocations.FindAsync(id);

        if (allocation == null)
            return NotFound();

        _context.ProjectAllocations.Remove(allocation);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
