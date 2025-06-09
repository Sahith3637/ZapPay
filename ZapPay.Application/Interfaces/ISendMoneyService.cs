using System.Threading.Tasks;
using ZapPay.Application.DTOs;

namespace ZapPay.Application.Interfaces;

public interface ISendMoneyService
{
    Task<SendVpaResponseDto> SendByVpaAsync(SendVpaRequestDto dto, Guid systemUserId);
} 