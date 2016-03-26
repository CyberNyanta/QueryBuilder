using System.Collections.Generic;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.Services.Contracts
{
    public interface IProjectService
    {
        IEnumerable<Project> GetUserProjects(User user);
    }
}