using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Common;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public RolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles
            .Select(x => new RolesDTO
            {
                RoleID      = x.RoleID,
                RoleName    = x.RoleName,
                Description = x.Description
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<RolesDTO>>
        {
            Success = true,
            Message = "Roles Retrieved Successfully",
            Data    = roles
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await _context.Roles
            .Where(x => x.RoleID == id)
            .Select(x => new RolesDTO
            {
                RoleID      = x.RoleID,
                RoleName    = x.RoleName,
                Description = x.Description
            })
            .FirstOrDefaultAsync();

        if (role == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Role Not Found",
                Errors  = new List<string> { $"No role found with Id {id}" }
            });

        return Ok(new ApiResponse<RolesDTO>
        {
            Success = true,
            Message = "Role Retrieved Successfully",
            Data    = role
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(RolesDTO dto)
    {
        try
        {
            var role = new SPM_Role
            {
                RoleName    = dto.RoleName!,
                Description = dto.Description
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<RolesDTO>
            {
                Success = true,
                Message = "Role Added Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while adding role",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, RolesDTO dto)
    {
        if (id != dto.RoleID)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "ID Mismatch",
                Errors  = new List<string> { "Route ID does not match the DTO RoleID" }
            });

        try
        {
            var existing = await _context.Roles.FindAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Role Not Found",
                    Errors  = new List<string> { $"No role found with Id {id}" }
                });

            existing.RoleName    = dto.RoleName!;
            existing.Description = dto.Description;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<RolesDTO>
            {
                Success = true,
                Message = "Role Updated Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while updating role",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Role Not Found",
                    Errors  = new List<string> { $"No role found with Id {id}" }
                });

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Role Deleted Successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while deleting role",
                Errors  = new List<string> { ex.Message }
            });
        }
    }
}
