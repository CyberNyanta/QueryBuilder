using System;
using System.Collections.Generic;
using System.Linq;
using Wpf.DataModel.Entity;
using Wpf.DataModel.Repository.Repositories;
using Wpf.Exceptions;
using ServicesLib;
using Wpf.Properties;
using System.Diagnostics;

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

        public int LastIdProjects
        {
            get
            {
                var lastIdProjects = 0;
                var projectsRepository = new ProjectsRepository(_context);
                var lastProject = projectsRepository.GetList().OrderByDescending(u => u.ProjectID).FirstOrDefault();

                if (lastProject != null)
                    lastIdProjects = lastProject.ProjectID;

                projectsRepository.Dispose();

                return lastIdProjects;
            }
        }

        public int LastIdConnection
        {
            get
            {
                var lastIdConnection = 0;
                var connectionRepo = new ConnectionDBRepository(_context);
                var lastConnection = connectionRepo.GetList().OrderByDescending(u => u.ConnectionID).FirstOrDefault();

                if (lastConnection != null)
                    lastIdConnection = lastConnection.ConnectionID;

                connectionRepo.Dispose();

                return lastIdConnection;
            }
        }

        public EntityManager()
        {
            _context = new SqlConstructorDBEntities();
            User = new Users();
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="user"></param>
        public Users RegistrationUser(string firstName, string lastyName, string email, string password)
        {
            SmtpMailer mailer = SmtpMailer.Instance();
            if (!CheckEmail(email))
            {
                Users newUser = new Users
                {
                    FirstName = firstName,
                    LastName = lastyName,
                    Email = email,
                    PasswordHash = Scrambler.GetPassHash(password)
                };
                
                UsersRepository user = new UsersRepository(_context);
                user.Create(newUser);
                user.Save();
                mailer.SentRegisterNotification(email);
                user.Dispose();

                MainWindowData.CurrentUser = newUser;

                return newUser;

            }
            throw new CustomException(Resources.ExistEmailError);
        }

        public bool CheckEmail(string email)
        {
            bool result = false;
            UsersRepository users = new UsersRepository(_context);

            result = users.GetList().Any(e => e.Email.Equals(email) && e.Delflag == 0);

            return result;
        }


        public Users LoginUser(string email, string password)
        {
            if (ValidationUser(email, password, ref _user))
            {
                MainWindowData.CurrentUser = User;
                return User;
            }
            throw new ArgumentException();
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

        /// <summary>
        /// при обновлении данных получить проекты залогиненного юзера
        /// или получить проекты которыми делился залогиненный юзер
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Projects> GetUserProjects()
        {            
            return User.Projects;
        }

        /// <summary>
        /// параметр id-ConnectionOwner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ConnectionDB GetConnectionByProjectID(int id)
        {
            ConnectionDBRepository connectionRepo = new ConnectionDBRepository(_context);

            return connectionRepo.GetItemById(id);
        }

        private bool ValidationUser(string email, string password, ref Users user)
        {
            UsersRepository repository = new UsersRepository(_context);

            var logUser = repository.GetList().FirstOrDefault(e => e.Email.Equals(email) && e.Delflag == 0);

                if (logUser != null)
                    user = logUser;

            repository.Dispose();
            return user.PasswordHash.Equals(Scrambler.GetPassHash(password));
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
