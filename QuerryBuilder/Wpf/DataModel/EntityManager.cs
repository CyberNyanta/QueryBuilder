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

        private Users _user;

        public Users User
        {
            get
            {
                return _user;
            }

            set
            {
                _user = value;
            }
        }

        public EntityManager()
        {
            _context = new SqlConstructorDBEntities();
            User = new Users();
        }


        /// <summary>
        /// сохранить проект в бд
        /// или обновить существующий
        /// </summary>
        /// <returns></returns>
        public void SaveProject(string projectName, string projectOwner, string projectDesription)
        {
            Projects project = new Projects
            {
                ProjectName = projectName,
                ProjectOwner = projectOwner,
                ProjectDescription=projectDesription
            };

            ProjectsRepository projRepo = new ProjectsRepository(_context);

            var proj = (from p in User.Projects
                       where p.ProjectID.Equals(project.ProjectID) &&
                              p.ProjectName.Equals(project.ProjectName) &&
                              p.ProjectOwner.Equals(project.ProjectOwner) &&
                              p.Delflag == 0
                        select p).FirstOrDefault();


            if (proj != null)
            {
                projRepo.Update(proj);
                projRepo.Save();
            }
            else
            {
                projRepo.Create(project);
                projRepo.Save();
            }
        }

        public bool SaveConnection(int projectID, int connectionID, string connectName, string dataBaseName,
             string serverName, string loginDB, string passwordDB)
        {
            bool result = false;
            ConnectionDBRepository connectionRepo = new ConnectionDBRepository(_context);

            var connId = connectionRepo.GetList().Any(c=>c.ConnectionID.Equals(connectionID) && c.Delflag == 0);
                        
            if (!connId)
            { 
               ConnectionDB newConnection = new ConnectionDB
                    {
                        ConnectionOwner = projectID,
                        ConnectionID = connectionID,
                        ConnectionName = connectName,
                        DatabaseName = dataBaseName,
                        ServerName = serverName,
                        LoginDB = loginDB,
                        PasswordDB = Scrambler.GetPassHash(passwordDB)
                    };
                connectionRepo.Create(newConnection);
                connectionRepo.Save();
                result = true;
           }
            connectionRepo.Dispose();

            return result;
        }

        public void SaveResultHistory(string resultName, string resultOwner, int connectionId, string resultBody)
        {
            var resultHistoryRepo = new ResultHistoryRepository(_context);

            var newResultHistory = new ResultHistory
            {
                ResultName = resultName,
                ResultOwner = resultOwner,
                ConnectionID = connectionId,
                ResultBody = resultBody
            };
            resultHistoryRepo.Create(newResultHistory);
            resultHistoryRepo.Save();
            resultHistoryRepo.Dispose();
        }

        //public List<ConnectionDB> GetUserConnections(Users currentUser)
        //{
        //    var dbConnections = from u in currentUser.Projects
        //                        where u.Delflag == 0
        //                        from c in u.ConnectionDB
        //                        where c.Delflag == 0
        //                        select c;
        //    return dbConnections.ToList();
        //}

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
