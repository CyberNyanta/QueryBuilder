using System;
using System.Collections.Generic;
using System.Linq;
using QueryBuilder.Constants;
using QueryBuilder.DAL.Contracts;
using QueryBuilder.DAL.Models;
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

        public void AddUserToProjectsShare(Project project, ApplicationUser user, int userRole)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (userRole < UserRoleProjectsShareConstants.Invited && userRole > UserRoleProjectsShareConstants.Owner)
            {
                throw new ArgumentNullException(nameof(userRole));
            }

            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var projectsShare = unitOfWork.ProjectsShares.GetAll().FirstOrDefault(c => c.ProjectId.Equals(project.ProjectID)
                                            && c.UserId.Equals(user.Id));

                if (projectsShare != null)
                {
                    if (projectsShare.UserRole > UserRoleProjectsShareConstants.Invited || userRole == UserRoleProjectsShareConstants.Owner)
                    {
                        throw new ArgumentException("User exists in projects share.");
                    }

                    projectsShare.UserRole = userRole;
                    unitOfWork.ProjectsShares.Update(projectsShare);
                }
                else
                {
                    var newprojectsShare = new ProjectsShare
                    {
                        ProjectId = project.ProjectID,
                        UserId = user.Id,
                        UserRole = userRole
                    };

                    unitOfWork.ProjectsShares.Create(newprojectsShare);
                }
              
                unitOfWork.Save();
            }
        }

        public IEnumerable<Project> GetUserProjects(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                return unitOfWork.ProjectsShares.GetMany(p => p.UserId == user.Id).Select(f => f.Project).
                                ToList().OrderByDescending(g => g.CreatedDate);
            }
        }

        //public IEnumerable<Project> GetTop10UserProjects(ApplicationUser user)
        //{
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    return GetUserProjects(user).Take(10).ToList();
        //}
    }
}