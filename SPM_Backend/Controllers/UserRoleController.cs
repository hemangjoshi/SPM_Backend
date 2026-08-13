using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserRoleController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserRoles()
    {
        var userRoles = await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .Select(x => new UserRoleDTO
            {
                RolePermissionID = x.RolePermissionID,
                UserID           = x.UserID,
                UserName         = x.User!.FullName,
                RoleID           = x.RoleID,
                RoleName         = x.Role!.RoleName
            })
            .ToListAsync();

        return Ok(userRoles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserRole(int id)
    {
        var userRole = await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .Where(x => x.RolePermissionID == id)
            .Select(x => new UserRoleDTO
            {
                RolePermissionID = x.RolePermissionID,
                UserID           = x.UserID,
                UserName         = x.User!.FullName,
                RoleID           = x.RoleID,
                RoleName         = x.Role!.RoleName
            })
            .FirstOrDefaultAsync();

        if (userRole == null)
            return NotFound();

        return Ok(userRole);
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserRoleDTO dto)
    {
        var userRole = new SPM_UserRole
        {
            UserID = dto.UserID,
            RoleID = dto.RoleID
        };

        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();

        return Ok(new { Status = "Success", Message = "Record Inserted" });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UserRoleDTO dto)
    {
        if (id != dto.RolePermissionID)
            return BadRequest();

        var existing = await _context.UserRoles.FindAsync(id);
        if (existing == null)
            return NotFound();

        existing.UserID = dto.UserID;
        existing.RoleID = dto.RoleID;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userRole = await _context.UserRoles.FindAsync(id);

        if (userRole == null)
            return NotFound();

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();

        return Ok();
    }
}
