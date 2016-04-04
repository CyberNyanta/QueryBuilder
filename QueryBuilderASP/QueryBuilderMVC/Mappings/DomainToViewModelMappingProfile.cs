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
            Mapper.CreateMap<Project, ProjectViewModel>();

        }
    }
}