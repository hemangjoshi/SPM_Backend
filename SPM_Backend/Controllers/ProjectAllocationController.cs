using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
using SPM_Backend.Models;

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
        var allocations = await _context.ProjectAllocations
            .Include(x => x.Project)
            .Include(x => x.Student)
            .Include(x => x.Faculty)
            .Select(x => new ProjectAllocationDTO
            {
                ProjectAllocationID  = x.ProjectAllocationID,
                ProjectID            = x.ProjectID,
                ProjectTitle         = x.Project!.ProjectTitle,
                StudentID            = x.StudentID,
                StudentName          = x.Student!.FullName,
                FacultyID            = x.FacultyID,
                FacultyName          = x.Faculty!.FullName,
                AssignedDate         = x.AssignedDate,
                ProjectStartDate     = x.ProjectStartDate,
                ProjectEndDate       = x.ProjectEndDate,
                TotalTasksGiven      = x.TotalTasksGiven,
                TotalCompletedTasks  = x.TotalCompletedTasks,
                ProgressPercentage   = x.ProgressPercentage,
                OverAllGrade         = x.OverAllGrade
            })
            .ToListAsync();

        return Ok(allocations);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectAllocation(int id)
    {
        var allocation = await _context.ProjectAllocations
            .Include(x => x.Project)
            .Include(x => x.Student)
            .Include(x => x.Faculty)
            .Where(x => x.ProjectAllocationID == id)
            .Select(x => new ProjectAllocationDTO
            {
                ProjectAllocationID  = x.ProjectAllocationID,
                ProjectID            = x.ProjectID,
                ProjectTitle         = x.Project!.ProjectTitle,
                StudentID            = x.StudentID,
                StudentName          = x.Student!.FullName,
                FacultyID            = x.FacultyID,
                FacultyName          = x.Faculty!.FullName,
                AssignedDate         = x.AssignedDate,
                ProjectStartDate     = x.ProjectStartDate,
                ProjectEndDate       = x.ProjectEndDate,
                TotalTasksGiven      = x.TotalTasksGiven,
                TotalCompletedTasks  = x.TotalCompletedTasks,
                ProgressPercentage   = x.ProgressPercentage,
                OverAllGrade         = x.OverAllGrade
            })
            .FirstOrDefaultAsync();

        if (allocation == null)
            return NotFound();

        return Ok(allocation);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProjectAllocationDTO dto)
    {
        var allocation = new SPM_ProjectAllocation
        {
            ProjectID           = dto.ProjectID,
            StudentID           = dto.StudentID,
            FacultyID           = dto.FacultyID,
            AssignedDate        = dto.AssignedDate,
            ProjectStartDate    = dto.ProjectStartDate,
            ProjectEndDate      = dto.ProjectEndDate,
            TotalTasksGiven     = dto.TotalTasksGiven,
            TotalCompletedTasks = dto.TotalCompletedTasks,
            ProgressPercentage  = dto.ProgressPercentage,
            OverAllGrade        = dto.OverAllGrade
        };

        _context.ProjectAllocations.Add(allocation);
        await _context.SaveChangesAsync();

        return Ok(new { Status = "Success", Message = "Record Inserted" });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProjectAllocationDTO dto)
    {
        if (id != dto.ProjectAllocationID)
            return BadRequest();

        var existing = await _context.ProjectAllocations.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.ProjectID           = dto.ProjectID;
        existing.StudentID           = dto.StudentID;
        existing.FacultyID           = dto.FacultyID;
        existing.AssignedDate        = dto.AssignedDate;
        existing.ProjectStartDate    = dto.ProjectStartDate;
        existing.ProjectEndDate      = dto.ProjectEndDate;
        existing.TotalTasksGiven     = dto.TotalTasksGiven;
        existing.TotalCompletedTasks = dto.TotalCompletedTasks;
        existing.ProgressPercentage  = dto.ProgressPercentage;
        existing.OverAllGrade        = dto.OverAllGrade;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
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
