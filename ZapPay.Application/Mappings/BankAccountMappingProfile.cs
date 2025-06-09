using AutoMapper;
using ZapPay.Application.DTOs;
using ZapPay.Domain.Entities;

namespace ZapPay.Application.Mappings;

public class BankAccountMappingProfile : Profile
{
    public BankAccountMappingProfile()
    {
        // CreateBankAccountDto -> BankAccount
        CreateMap<CreateBankAccountDto, BankAccount>()
            .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
            .ForMember(dest => dest.Balance, opt => opt.MapFrom(src => src.InitialBalance))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

        // BankAccount -> BankAccountResponseDto
        CreateMap<BankAccount, BankAccountResponseDto>();
    }
} 