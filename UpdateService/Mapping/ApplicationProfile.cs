using AutoMapper;
using UpdateService.Model.Domain;
using UpdateService.Model.Transfer;

namespace UpdateService.Mapping;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Application, ApplicationDto>()
            .ForMember(a => a.Versions,
                opt => opt.MapFrom(a => a.Versions.Select(v => v.FileVersion).ToList()));
    }
    
}