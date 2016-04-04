using AutoMapper;
using QueryBuilder.DAL.Models;
using QueryBuilder.Utils;
using QueryBuilderMVC.Models;

namespace QueryBuilderMVC.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ProjectViewModel, Project>()
                .ForMember("ProjectID", opt => opt.MapFrom(src => src.IdCurrentProject))
                .ForMember("ProjectName", opt => opt.MapFrom(src => src.Name))
                .ForMember("ProjectDescription", opt => opt.MapFrom(src => src.Description));
            Mapper.CreateMap<ConnectionViewModel, ConnectionDB>()
                .ForMember("PasswordDB", opt => opt.MapFrom(src => Scrambler.GetPassHash(src.PasswordDB)));
        }
    }
}