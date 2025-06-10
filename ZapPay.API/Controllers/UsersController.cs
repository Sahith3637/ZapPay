using Microsoft.AspNetCore.Mvc;
using ZapPay.Application.DTOs;
using ZapPay.Application.Interfaces;
using ZapPay.Infrastructure.Interfaces;
using System.Security.Claims;
using Serilog;
using FluentValidation;
  using AutoMapper;
using Microsoft.AspNetCore.StaticFiles;

namespace ZapPay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;  
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterUserResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<RegisterUserResponseDto>> Register(RegisterUserRequestDto request)
    {
        try
        {
            var result = await _userService.RegisterUserAsync(request);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "An error occurred while processing your request." });
        }
    }

    [HttpPut("{userId}/kyc/verify")]
    public async Task<ActionResult<RegisterUserResponseDto>> VerifyKyc(Guid userId, [FromBody] VerifyKycRequestDto request)
    {
        try
        {
            var result = await _userService.VerifyKycAsync(userId, request.VerificationStatus, request.VerificationRemarks);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{userId}/kyc/resubmit")]
    public async Task<ActionResult<RegisterUserResponseDto>> ResubmitKyc(Guid userId, [FromBody] ResubmitKycRequestDto request)
    {
        try
        {
            var result = await _userService.ResubmitKycAsync(userId, request);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("{userId}/kyc/upload")]
    public async Task<IActionResult> AddKycWithFile(
        Guid userId,
        [FromForm] AddKycWithFileRequestDto request,
        [FromServices] IFileStorageService fileStorageService)
    {
        var filePath = await fileStorageService.SaveFileAsync(request.DocumentFile);
        await _userService.AddKycWithFileAsync(userId, request, filePath);
        return Ok(new { message = "KYC document uploaded successfully." });
    }

    [HttpGet("admin/kyc/{kycId}/download")]
    public async Task<IActionResult> DownloadKycDocument(Guid kycId)
{
    var kyc = await _userService.GetKycByIdAsync(kycId);
    if (kyc == null || string.IsNullOrEmpty(kyc.DocumentFilePath) || !System.IO.File.Exists(kyc.DocumentFilePath))
        return NotFound("KYC document not found.");

    var fileBytes = await System.IO.File.ReadAllBytesAsync(kyc.DocumentFilePath);
    var fileName = Path.GetFileName(kyc.DocumentFilePath);

    // Determine correct content-type
    var provider = new FileExtensionContentTypeProvider();
    string contentType;

    if (!provider.TryGetContentType(fileName, out contentType))
    {
        contentType = "application/octet-stream"; // fallback
    }

    return File(fileBytes, contentType, fileName);
}

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("admin/kyc")]
    public async Task<IActionResult> GetAllKyc()
    {
        var kycs = await _userService.GetAllKycAsync();
        return Ok(kycs);
    }

    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileResponseDto>> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return Unauthorized();

            var updatedUser = await _userService.UpdateProfileAsync(userId, dto);
            var responseDto = _mapper.Map<UserProfileResponseDto>(updatedUser);
            return Ok(responseDto);
        }
        catch (KeyNotFoundException ex)
        {
            Serilog.Log.Warning(ex, "User not found during profile update");
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            Serilog.Log.Warning(ex, "Validation failed during profile update");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error updating user profile");
            return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [HttpPut("profile/{userId}")]
    public async Task<ActionResult<UserProfileResponseDto>> UpdateProfileById(Guid userId, [FromBody] UpdateProfileDto dto)
    {
        try
        {
            // Optional: Only allow self or admin
            var jwtUserId = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.Claims.Any(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == "Admin");
            if (!isAdmin && (jwtUserId == null || jwtUserId != userId.ToString()))
                return Forbid();

            var updatedUser = await _userService.UpdateProfileAsync(userId, dto);
            var responseDto = _mapper.Map<UserProfileResponseDto>(updatedUser);
            return Ok(responseDto);
        }
        catch (KeyNotFoundException ex)
        {
            Serilog.Log.Warning(ex, "User not found during profile update by ID");
            return NotFound(new { message = ex.Message });
        }
        catch (ValidationException ex)
        {
            Serilog.Log.Warning(ex, "Validation failed during profile update by ID");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error updating user profile by ID");
            return StatusCode(500, new { message = "An error occurred while updating the profile" });
        }
    }
} 