using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Data;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(SPM_User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SPM_User user)
    {
        if (id != user.UserID) return BadRequest();

        var existing = await _context.Users.FindAsync(id);
        if (existing == null) return NotFound();

        existing.UserTypeID = user.UserTypeID;
        existing.FullName = user.FullName;
        existing.UserCode = user.UserCode;
        existing.Email = user.Email;
        existing.Password = user.Password;
        existing.MobileNumber = user.MobileNumber;
        existing.ProfilePicturePath = user.ProfilePicturePath;
        existing.IsActive = user.IsActive;
        existing.IsDeleted = user.IsDeleted;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
