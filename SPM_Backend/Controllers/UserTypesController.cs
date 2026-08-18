using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPM_Backend.Common;
using SPM_Backend.Data;
using SPM_Backend.DTOs;
using SPM_Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class UserTypesController : ControllerBase
{
    private readonly AppDbContext _context;

    public UserTypesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserTypes()
    {
        var userTypes = await _context.UserTypes
            .Select(x => new UserTypeDTO
            {
                UserTypeID   = x.UserTypeID,
                UserTypeName = x.UserTypeName,
                Description  = x.Description
            })
            .ToListAsync();

        return Ok(new ApiResponse<List<UserTypeDTO>>
        {
            Success = true,
            Message = "User Types Retrieved Successfully",
            Data    = userTypes
        });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserType(int id)
    {
        var userType = await _context.UserTypes
            .Where(x => x.UserTypeID == id)
            .Select(x => new UserTypeDTO
            {
                UserTypeID   = x.UserTypeID,
                UserTypeName = x.UserTypeName,
                Description  = x.Description
            })
            .FirstOrDefaultAsync();

        if (userType == null)
            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "User Type Not Found",
                Errors  = new List<string> { $"No user type found with Id {id}" }
            });

        return Ok(new ApiResponse<UserTypeDTO>
        {
            Success = true,
            Message = "User Type Retrieved Successfully",
            Data    = userType
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserTypeDTO dto)
    {
        try
        {
            var userType = new SPM_UserType
            {
                UserTypeName = dto.UserTypeName!,
                Description  = dto.Description
            };

            _context.UserTypes.Add(userType);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<UserTypeDTO>
            {
                Success = true,
                Message = "User Type Added Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while adding user type",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UserTypeDTO dto)
    {
        if (id != dto.UserTypeID)
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "ID Mismatch",
                Errors  = new List<string> { "Route ID does not match the DTO UserTypeID" }
            });

        try
        {
            var existing = await _context.UserTypes.FindAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User Type Not Found",
                    Errors  = new List<string> { $"No user type found with Id {id}" }
                });

            existing.UserTypeName = dto.UserTypeName!;
            existing.Description  = dto.Description;
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<UserTypeDTO>
            {
                Success = true,
                Message = "User Type Updated Successfully",
                Data    = dto
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while updating user type",
                Errors  = new List<string> { ex.Message }
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userType = await _context.UserTypes.FindAsync(id);

            if (userType == null)
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Message = "User Type Not Found",
                    Errors  = new List<string> { $"No user type found with Id {id}" }
                });

            _context.UserTypes.Remove(userType);
            await _context.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "User Type Deleted Successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Error occurred while deleting user type",
                Errors  = new List<string> { ex.Message }
            });
        }
    }
}
