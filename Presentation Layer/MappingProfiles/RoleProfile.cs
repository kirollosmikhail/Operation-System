using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole,RoleViewModel>()
                .ForMember(d=>d.RoleName, o=>o.MapFrom(s=>s.Name)).ReverseMap();
        }
    }
}
