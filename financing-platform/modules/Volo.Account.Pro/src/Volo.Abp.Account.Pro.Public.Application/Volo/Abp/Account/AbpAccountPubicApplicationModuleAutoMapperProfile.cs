using System;
using AutoMapper;
using Volo.Abp.Account.ExternalProviders;
using Volo.Abp.Identity;

namespace Volo.Abp.Account;

public class AbpAccountPubicApplicationModuleAutoMapperProfile : Profile
{
    public AbpAccountPubicApplicationModuleAutoMapperProfile()
    {
        CreateMap<ExternalProviderSettings, ExternalProviderItemDto>(MemberList.Destination);
        CreateMap<ExternalProviderSettings, ExternalProviderItemWithSecretDto>(MemberList.Destination)
            .ForMember(d => d.Success, opt => opt.MapFrom(x => !x.Name.IsNullOrWhiteSpace()));

        CreateMap<IdentityUser, ProfileDto>()
            .ForMember(dest => dest.HasPassword,
                op => op.MapFrom(src => src.PasswordHash != null))
            .MapExtraProperties();
    }
}
