using AutoMapper;
using ZapPay.Application.DTOs;
using ZapPay.Domain.Entities;

namespace ZapPay.Application.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, RegisterUserResponseDto>()
            .ForMember(dest => dest.KYCStatus, opt => opt.MapFrom(src => src.Kycstatus))
            .ForMember(dest => dest.KYCVerificationRemarks, opt => opt.MapFrom(src =>
                src.UserKycs.Count > 0 ? src.UserKycs.FirstOrDefault().VerificationRemarks : null));
    }
} 