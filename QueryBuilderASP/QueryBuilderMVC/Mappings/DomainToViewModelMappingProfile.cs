using AutoMapper;
using QueryBuilder.DAL.Models;
using QueryBuilderMVC.Models;

namespace QueryBuilderMVC.Mappings
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ConnectionDB,ConnectionViewModel>();
            
            Mapper.CreateMap<Project, ProjectViewModel>()
               .ForMember("IdCurrentProject", opt => opt.MapFrom(src => src.ProjectID))
               .ForMember("Name", opt => opt.MapFrom(src => src.ProjectName))
               .ForMember("Description", opt => opt.MapFrom(src => src.ProjectDescription));
        }
    }
}