using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserRolesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserRoles()
    {
        var userRoles = await _context.UserRoles.ToListAsync();
        return Ok(userRoles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserRole(int id)
    {
        var userRole = await _context.UserRoles.FindAsync(id);
        if (userRole == null) return NotFound();
        return Ok(userRole);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SPM_UserRole userRole)
    {
        _context.UserRoles.Add(userRole);
        await _context.SaveChangesAsync();
        return Ok(userRole);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SPM_UserRole userRole)
    {
        if (id != userRole.RolePermissionID) return BadRequest();

        var existing = await _context.UserRoles.FindAsync(id);
        if (existing == null) return NotFound();

        existing.RoleID = userRole.RoleID;
        existing.UserID = userRole.UserID;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userRole = await _context.UserRoles.FindAsync(id);
        if (userRole == null) return NotFound();

        _context.UserRoles.Remove(userRole);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
