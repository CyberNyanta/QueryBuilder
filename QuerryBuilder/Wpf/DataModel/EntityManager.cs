using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wpf.DataModel.Entity;
using Wpf.DataModel.Repository;
using Wpf.DataModel.Repository.Repositories;
using Wpf.Exceptions;
using ServicesLib;
using Wpf.Properties;

namespace Wpf.DataModel
{
    public class EntityManager
    {

        private SqlConstructorDBEntities _context;

        private Users _user;
        public EntityManager()
        {
            _context = new SqlConstructorDBEntities();
            _user = new Users();
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

                UsersRepository users = new UsersRepository(_context);
                users.Create(newUser);
                users.Save();
                mailer.SentRegisterNotification(email);
                users.Dispose();
                return newUser;
            }
            else
                throw new CustomException(Resources.ExistEmailError);
        }

        public bool CheckEmail(string email)
        {
            bool result = false;
            UsersRepository users = new UsersRepository(_context);
            try {
                result = users.GetList().Any(e => e.Email.Equals(email));
                //result = true;
            }
            catch
            {
                result = false;
            }
            return result;
        }




        public Users LoginUser(string email, string password)
        {
            if (ValidationUser(email, password, ref _user))
            {
                return _user;
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// сохранить проект в бд
        /// или обновить существующий
        /// </summary>
        /// <returns></returns>
        public void SaveProject(Projects project)
        {

            ProjectsRepository projRepo = new ProjectsRepository(_context);

            var proj = (from p in _user.Projects
                       where p.ProjectID.Equals(project.ProjectID) &&
                              p.ProjectName.Equals(project.ProjectName) &&
                              p.ProjectOwner.Equals(project.ProjectOwner)
                      select p).First();


            if (proj != null) {
                projRepo.Update(proj);
            }
            else 
                projRepo.Create(proj);
          
        }

        /// <summary>
        /// при обновлении данных получить проекты залогиненного юзера
        /// или получить проекты которыми делился залогиненный юзер
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Projects> GetUserProjects()
        {            
            return _user.Projects;
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

            var logUser = repository.GetList().Where(e => e.Email.Equals(email)).First();

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

            var connId = connectionRepo.GetList().Any(c=>c.ConnectionID.Equals(connectionID));
                        
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
    }
}
