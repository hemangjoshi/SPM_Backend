using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
using SPM_Backend.Models;
using System.Data;

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
        var roles = await _context.Roles.ToListAsync();
        return Ok(roles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRole(int id)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
        {
            return NotFound();
        }

        return Ok(role);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SPM_Role role)
    {
        _context.Roles.Add(role);
        await _context.SaveChangesAsync();

        return Ok(role);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SPM_Role role)
    {
        if (id != role.RoleID)
            return BadRequest();

        var oldRole = await _context.Roles.FindAsync(id);

        oldRole.RoleName = role.RoleName;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var role = await _context.Roles.FindAsync(id);

        if (role == null)
            return NotFound();

        _context.Roles.Remove(role);
        await _context.SaveChangesAsync();

        return Ok();
    }
}