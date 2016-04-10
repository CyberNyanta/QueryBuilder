using AutoMapper;
using QueryBuilder.DAL.Models;
using QueryBuilder.Utils.Encryption;
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
            Mapper.CreateMap<ConnectionDB, ConnectionViewModel>()
                .ForMember("ConnectionName", opt => opt.MapFrom(src => src.ConnectionName))
                .ForMember("ConnectionID", opt => opt.MapFrom(src => src.ConnectionID))
                .ForMember("ServerName", opt => opt.MapFrom(src => src.ServerName))
                .ForMember("LoginDB", opt => opt.MapFrom(src => src.LoginDB))
                //.ForMember("PasswordDB", opt => opt.MapFrom(src => Rijndael.DecryptStringFromBytes(src.PasswordDB)))
                .ForMember("ConnectionOwner", opt => opt.MapFrom(src => src.ConnectionOwner))
                .ForMember("DatabaseName", opt => opt.MapFrom(src => src.DatabaseName));

            Mapper.CreateMap<Project, ProjectViewModel>()
               .ForMember("IdCurrentProject", opt => opt.MapFrom(src => src.ProjectID))
               .ForMember("Name", opt => opt.MapFrom(src => src.ProjectName))
               .ForMember("Description", opt => opt.MapFrom(src => src.ProjectDescription));

            Mapper.CreateMap<Project, ProjectsListViewModel>();

            Mapper.CreateMap<ApplicationUser, UserViewModel>();

            Mapper.CreateMap<ApplicationUser, UsersListViewModel>();
        }
    }
}