using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class ProjectMastersController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectMastersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjectMasters()
    {
        var projects = await _context.ProjectMasters.ToListAsync();
        return Ok(projects);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectMaster(int id)
    {
        var project = await _context.ProjectMasters.FindAsync(id);
        if (project == null) return NotFound();
        return Ok(project);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SPM_ProjectMaster project)
    {
        _context.ProjectMasters.Add(project);
        await _context.SaveChangesAsync();
        return Ok(project);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SPM_ProjectMaster project)
    {
        if (id != project.ProjectID) return BadRequest();

        var existing = await _context.ProjectMasters.FindAsync(id);
        if (existing == null) return NotFound();

        existing.ProjectTitle = project.ProjectTitle;
        existing.Description = project.Description;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.ProjectMasters.FindAsync(id);
        if (project == null) return NotFound();

        _context.ProjectMasters.Remove(project);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
