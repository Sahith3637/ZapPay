using Microsoft.AspNetCore.Mvc;
using ZapPay.Application.DTOs;
using ZapPay.Application.Interfaces;
using ZapPay.Infrastructure.Interfaces;

namespace ZapPay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
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
        // Assume you have a repository or service to get the KYC record
        var kyc = (await _userService.GetKycByIdAsync(kycId));
        if (kyc == null || string.IsNullOrEmpty(kyc.DocumentFilePath) || !System.IO.File.Exists(kyc.DocumentFilePath))
            return NotFound("KYC document not found.");

        var fileBytes = await System.IO.File.ReadAllBytesAsync(kyc.DocumentFilePath);
        var fileName = Path.GetFileName(kyc.DocumentFilePath);
        var contentType = "application/octet-stream";
        return File(fileBytes, contentType, fileName);
    }
} 