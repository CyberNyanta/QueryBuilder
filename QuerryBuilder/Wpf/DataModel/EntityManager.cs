using System;
using System.Collections.Generic;
using System.Linq;
using Wpf.DataModel.Entity;
using Wpf.DataModel.Repository.Repositories;
using Wpf.Exceptions;
using QueryBuilder.Utils;
using Wpf.Properties;
using System.Diagnostics;
using QueryBuilder.Services.DbServices;

namespace Wpf.DataModel
{
    public class EntityManager
    {

        private SqlConstructorDBEntities _context;

        public EntityManager()
        {
            _context = new SqlConstructorDBEntities();
        }

        public bool SaveEmailToProjectsShare(Projects project, string email, bool delFlag)
        {
            bool result = false;

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Empty email.");
            }

            var projectsShareRepo = new ProjectsShareRepository(_context);

            var projectsShare = projectsShareRepo.GetList().FirstOrDefault(c => project != null && 
                                    (c.ProjectID.Equals(project.ProjectID) && c.SharedEmail.Equals(email) && c.Delflag == 0));

            if (projectsShare == null && !delFlag)
            {
                var newprojectsShare = new ProjectsShare
                {
                    ProjectID = project.ProjectID,
                    SharedEmail = email
                };

                projectsShareRepo.Create(newprojectsShare);
                projectsShareRepo.Save();
                result = true;
            }
            else if (projectsShare != null && delFlag)
            {
                projectsShare.Delflag = 1;
                projectsShareRepo.Update(projectsShare);
                projectsShareRepo.Save();
                result = true;
            }
            projectsShareRepo.Dispose();

            return result;
        }

    }
}
