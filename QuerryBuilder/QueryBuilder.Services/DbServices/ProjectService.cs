using System.Collections.Generic;
using QueryBuilder.DAL.Contracts;
using QueryBuilder.DAL.Models;
using QueryBuilder.Services.Contracts;

namespace QueryBuilder.Services.DbServices
{
    public class ProjectService: IProjectService
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        public ProjectService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public IEnumerable<Project> GetUserProjects(User user)
        {
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                return unitOfWork.Projects.GetMany(p => (p.Delflag == 0 && p.Users == user));
            }
        }
    }
}