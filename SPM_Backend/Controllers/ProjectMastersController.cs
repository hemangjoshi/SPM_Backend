using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Common;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
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
        var projects = await _context.ProjectMasters
            .Select(x => new ProjectMasterDTO
            {
                ProjectID    = x.ProjectID,
                ProjectTitle = x.ProjectTitle,
                Description  = x.Description
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<ProjectMasterDTO>>
        {
            Success = true,
            Message = "Projects Retrieved Successfully",
            Data    = projects
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectMaster(int id)
    {
        var project = await _context.ProjectMasters
            .Where(x => x.ProjectID == id)
            .Select(x => new ProjectMasterDTO
            {
                ProjectID    = x.ProjectID,
                ProjectTitle = x.ProjectTitle,
                Description  = x.Description
            })
            .FirstOrDefaultAsync();

        if (project == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Project Not Found",
                Errors  = new List<string> { $"No project found with Id {id}" }
            });

        return Ok(new ApiResponse<ProjectMasterDTO>
        {
            Success = true,
            Message = "Project Retrieved Successfully",
            Data    = project
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProjectMasterDTO dto)
    {
        try
        {
            var project = new SPM_ProjectMaster
            {
                ProjectTitle = dto.ProjectTitle!,
                Description  = dto.Description
            };

            _context.ProjectMasters.Add(project);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<ProjectMasterDTO>
            {
                Success = true,
                Message = "Project Added Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while adding project",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProjectMasterDTO dto)
    {
        if (id != dto.ProjectID)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "ID Mismatch",
                Errors  = new List<string> { "Route ID does not match the DTO ProjectID" }
            });

        try
        {
            var existing = await _context.ProjectMasters.FindAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Project Not Found",
                    Errors  = new List<string> { $"No project found with Id {id}" }
                });

            existing.ProjectTitle = dto.ProjectTitle!;
            existing.Description  = dto.Description;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<ProjectMasterDTO>
            {
                Success = true,
                Message = "Project Updated Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while updating project",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var project = await _context.ProjectMasters.FindAsync(id);

            if (project == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Project Not Found",
                    Errors  = new List<string> { $"No project found with Id {id}" }
                });

            _context.ProjectMasters.Remove(project);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Project Deleted Successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while deleting project",
                Errors  = new List<string> { ex.Message }
            });
        }
    }
}
