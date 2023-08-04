using System.Security.Claims;
using AutoMapper;


namespace IdentityServer.Auth.Mapping_Profile
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityServer4.EntityFramework.Entities.ClientProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<IdentityServer4.EntityFramework.Entities.Client, IdentityServer4.Models.Client>()
                .ForMember(dest => dest.ProtocolType, opt => opt.Condition(srs => srs != null))
                .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedIdentityTokenSigningAlgorithms))
                .ReverseMap()
                .ForMember(x => x.AllowedIdentityTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedIdentityTokenSigningAlgorithms));

            CreateMap<IdentityServer4.EntityFramework.Entities.ClientCorsOrigin, string>()
                .ConstructUsing(src => src.Origin)
                .ReverseMap()
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src));

            CreateMap<IdentityServer4.EntityFramework.Entities.ClientIdPRestriction, string>()
                .ConstructUsing(src => src.Provider)
                .ReverseMap()
                .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src));

            CreateMap<IdentityServer4.EntityFramework.Entities.ClientClaim, IdentityServer4.Models.ClientClaim>(MemberList.None)
                .ConstructUsing(src => new IdentityServer4.Models.ClientClaim(src.Type, src.Value, ClaimValueTypes.String))
                .ReverseMap();

            CreateMap<IdentityServer4.EntityFramework.Entities.ClientScope, string>()
                .ConstructUsing(src => src.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));

            CreateMap<IdentityServer4.EntityFramework.Entities.ClientPostLogoutRedirectUri, string>()
                .ConstructUsing(src => src.PostLogoutRedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.PostLogoutRedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<IdentityServer4.EntityFramework.Entities.ClientRedirectUri, string>()
                .ConstructUsing(src => src.RedirectUri)
                .ReverseMap()
                .ForMember(dest => dest.RedirectUri, opt => opt.MapFrom(src => src));

            CreateMap<IdentityServer4.EntityFramework.Entities.ClientGrantType, string>()
                .ConstructUsing(src => src.GrantType)
                .ReverseMap()
                .ForMember(dest => dest.GrantType, opt => opt.MapFrom(src => src));

            CreateMap<IdentityServer4.EntityFramework.Entities.ClientSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();
            CreateMap<IdentityServer4.EntityFramework.Entities.ApiResourceProperty, KeyValuePair<string, string>>()
                .ReverseMap();

            CreateMap<IdentityServer4.EntityFramework.Entities.ApiResource, IdentityServer4.Models.ApiResource>(MemberList.Destination)
                .ConstructUsing(src => new IdentityServer4.Models.ApiResource())
                .ForMember(x => x.ApiSecrets, opts => opts.MapFrom(x => x.Secrets))
                .ForMember(x => x.AllowedAccessTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedAccessTokenSigningAlgorithms))
                .ReverseMap()
                .ForMember(x => x.AllowedAccessTokenSigningAlgorithms, opts => opts.ConvertUsing(AllowedSigningAlgorithmsConverter.Converter, x => x.AllowedAccessTokenSigningAlgorithms));

            CreateMap<IdentityServer4.EntityFramework.Entities.ApiResourceClaim, string>()
                .ConstructUsing(x => x.Type)
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<IdentityServer4.EntityFramework.Entities.ApiResourceSecret, IdentityServer4.Models.Secret>(MemberList.Destination)
                .ForMember(dest => dest.Type, opt => opt.Condition(srs => srs != null))
                .ReverseMap();

            CreateMap<IdentityServer4.EntityFramework.Entities.ApiResourceScope, string>()
                .ConstructUsing(x => x.Scope)
                .ReverseMap()
                .ForMember(dest => dest.Scope, opt => opt.MapFrom(src => src));
            CreateMap<IdentityServer4.EntityFramework.Entities.ApiScopeProperty, KeyValuePair<string, string>>()
    .ReverseMap();

            CreateMap<  IdentityServer4.EntityFramework.Entities.ApiScopeClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

            CreateMap<IdentityServer4.EntityFramework.Entities.ApiScope, IdentityServer4.Models.ApiScope>(MemberList.Destination)
                .ConstructUsing(src => new IdentityServer4.Models.ApiScope())
                .ForMember(x => x.Properties, opts => opts.MapFrom(x => x.Properties))
                .ForMember(x => x.UserClaims, opts => opts.MapFrom(x => x.UserClaims))
                .ReverseMap();
            CreateMap<IdentityServer4.EntityFramework.Entities.IdentityResourceProperty, KeyValuePair<string, string>>()
    .ReverseMap();

            CreateMap<IdentityServer4.EntityFramework.Entities.IdentityResource, IdentityServer4.Models.IdentityResource>(MemberList.Destination)
                .ConstructUsing(src => new IdentityServer4.Models.IdentityResource())
                .ReverseMap();

            CreateMap<IdentityServer4.EntityFramework.Entities.IdentityResourceClaim, string>()
               .ConstructUsing(x => x.Type)
               .ReverseMap()
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src));

        }
        class AllowedSigningAlgorithmsConverter :
    IValueConverter<ICollection<string>, string>,
    IValueConverter<string, ICollection<string>>
        {
            public static AllowedSigningAlgorithmsConverter Converter = new AllowedSigningAlgorithmsConverter();

            public string Convert(ICollection<string> sourceMember, ResolutionContext context)
            {
                if (sourceMember == null || !sourceMember.Any())
                {
                    return null;
                }
                return sourceMember.Aggregate((x, y) => $"{x},{y}");
            }

            public ICollection<string> Convert(string sourceMember, ResolutionContext context)
            {
                var list = new HashSet<string>();
                if (!String.IsNullOrWhiteSpace(sourceMember))
                {
                    sourceMember = sourceMember.Trim();
                    foreach (var item in sourceMember.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Distinct())
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
        }

    }
}
