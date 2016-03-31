using System.Collections.Generic;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.Services.Contracts
{
    public interface IProjectService
    {
        IEnumerable<Project> GetProjects();

        IEnumerable<Project> GetUserProjects(ApplicationUser user);

        void SaveProject(Project project);
    }
}