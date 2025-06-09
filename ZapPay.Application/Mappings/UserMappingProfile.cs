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
                src.UserKycs != null && src.UserKycs.Any() ? src.UserKycs.First().VerificationRemarks : null))
            .ForMember(dest => dest.Vpa, opt => opt.MapFrom(src =>
                src.VirtualPaymentAddresses != null && src.VirtualPaymentAddresses.Any() ? src.VirtualPaymentAddresses.First().Vpa : null));

        CreateMap<User, UserProfileResponseDto>();

        CreateMap<Transaction, SendVpaResponseDto>()
            .ForMember(dest => dest.FromVpa, opt => opt.MapFrom(src => src.SourceIdentifier))
            .ForMember(dest => dest.ToVpa, opt => opt.MapFrom(src => src.DestinationIdentifier))
            .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(src => src.TransactionId))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Remarks, opt => opt.MapFrom(src => src.Remarks));
    }
} 