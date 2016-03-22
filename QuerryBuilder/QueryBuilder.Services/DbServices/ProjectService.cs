using System.Collections.Generic;
using QueryBuilder.DAL.Contracts;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.Services.DbServices
{
    public class ProjectService
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