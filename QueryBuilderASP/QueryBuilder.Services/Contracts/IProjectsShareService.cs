using System.Collections.Generic;
using QueryBuilder.DAL.Models;

namespace QueryBuilder.Services.Contracts
{
    public interface IProjectsShareService
    {
        void AddUserToProjectsShare(Project project, ApplicationUser user, int userRole);

        IEnumerable<Project> GetUserProjects(ApplicationUser user);

        IEnumerable<ApplicationUser> GetProjectUsers(Project project);

        IEnumerable<ApplicationUser> GetUsersForSharedProject(Project project);

        int GetUserRole(ApplicationUser user, int projectId);
    }
}