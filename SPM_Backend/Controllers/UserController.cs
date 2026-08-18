using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Common;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _context.Users
            .Include(x => x.UserType)
            .Select(x => new UserDTO
            {
                UserID             = x.UserID,
                UserTypeID         = x.UserTypeID,
                UserTypeName       = x.UserType!.UserTypeName,
                FullName           = x.FullName,
                UserCode           = x.UserCode,
                Email              = x.Email,
                MobileNumber       = x.MobileNumber,
                ProfilePicturePath = x.ProfilePicturePath,
                IsActive           = x.IsActive,
                IsDeleted          = x.IsDeleted
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<UserDTO>>
        {
            Success = true,
            Message = "Users Retrieved Successfully",
            Data    = users
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users
            .Include(x => x.UserType)
            .Where(x => x.UserID == id)
            .Select(x => new UserDTO
            {
                UserID             = x.UserID,
                UserTypeID         = x.UserTypeID,
                UserTypeName       = x.UserType!.UserTypeName,
                FullName           = x.FullName,
                UserCode           = x.UserCode,
                Email              = x.Email,
                MobileNumber       = x.MobileNumber,
                ProfilePicturePath = x.ProfilePicturePath,
                IsActive           = x.IsActive,
                IsDeleted          = x.IsDeleted
            })
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "User Not Found",
                Errors  = new List<string> { $"No user found with Id {id}" }
            });

        return Ok(new ApiResponse<UserDTO>
        {
            Success = true,
            Message = "User Retrieved Successfully",
            Data    = user
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserDTO dto)
    {
        try
        {
            var user = new SPM_User
            {
                UserTypeID         = dto.UserTypeID,
                FullName           = dto.FullName!,
                UserCode           = dto.UserCode,
                Email              = dto.Email!,
                Password           = string.Empty,   // set password separately
                MobileNumber       = dto.MobileNumber!,
                ProfilePicturePath = dto.ProfilePicturePath!,
                IsActive           = dto.IsActive,
                IsDeleted          = dto.IsDeleted
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<UserDTO>
            {
                Success = true,
                Message = "User Added Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while adding user",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UserDTO dto)
    {
        if (id != dto.UserID)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "ID Mismatch",
                Errors  = new List<string> { "Route ID does not match the DTO UserID" }
            });

        try
        {
            var existing = await _context.Users.FindAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User Not Found",
                    Errors  = new List<string> { $"No user found with Id {id}" }
                });

            existing.UserTypeID         = dto.UserTypeID;
            existing.FullName           = dto.FullName!;
            existing.UserCode           = dto.UserCode;
            existing.Email              = dto.Email!;
            existing.MobileNumber       = dto.MobileNumber!;
            existing.ProfilePicturePath = dto.ProfilePicturePath!;
            existing.IsActive           = dto.IsActive;
            existing.IsDeleted          = dto.IsDeleted;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<UserDTO>
            {
                Success = true,
                Message = "User Updated Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while updating user",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User Not Found",
                    Errors  = new List<string> { $"No user found with Id {id}" }
                });

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User Deleted Successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while deleting user",
                Errors  = new List<string> { ex.Message }
            });
        }
    }
}
