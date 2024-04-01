using AutoMapper;
using Data_Access_Layer.Models;
using Presentation_Layer.ViewModels;

namespace Presentation_Layer.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }


    }
}
