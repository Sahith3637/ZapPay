 using Microsoft.AspNetCore.Mvc;
using ZapPay.Application.DTOs;
using ZapPay.Application.Interfaces;
using System.Security.Claims;

namespace ZapPay.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ISendMoneyService _sendMoneyService;

    public TransactionsController(ISendMoneyService sendMoneyService)
    {
        _sendMoneyService = sendMoneyService;
    }

    [HttpPost("send-vpa")]
    public async Task<ActionResult<SendVpaResponseDto>> SendByVpa([FromBody] SendVpaRequestDto dto)
    {
        try
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var systemUserId))
                return Unauthorized();

            var result = await _sendMoneyService.SendByVpaAsync(dto, systemUserId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
