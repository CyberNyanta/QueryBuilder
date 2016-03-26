using QueryBuilder.DAL.Contracts;
using QueryBuilder.Services.Contracts;

namespace QueryBuilder.Services.DbServices
{
    public class ProjectsShareService: IProjectsShareService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ProjectsShareService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }
    }
}