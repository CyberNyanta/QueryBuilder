using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wpf.DataModel.Entity;
using Wpf.DataModel.Repository;
using Wpf.DataModel.Repository.Repositories;

namespace Wpf.DataModel
{
    public class EntityManager
    {
        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="user"></param>
        public void RegistrationUser(string firstName, string lastyName, string email, string password)
        {
            Users newUser = new Users
            {
                FirstName = firstName,
                LastName = lastyName,
                Email=email,
                PasswordHash=GetHashString(password)
            };

            UsersRepository users = new UsersRepository();
            users.Create(newUser);
            users.Save();
        }

        public Users LoginUser(string email, string password)
        {
            Users logUser = new Users();

            if (ValidationUser(email, password, ref logUser))
            {
                //ICollection<Projects> MyProjects = LoadMyProject(email);
                //IEnumerable<Projects> ShareProjects = GetShareProject(email);

                return logUser;
            }
            throw new ArgumentException();
        }

        /// <summary>
        /// Если проект новый - добавляет новую запись в БД
        /// Если же идет сохранение ранее созданого проекта, то обновляет запись в БД.
        /// </summary>
        /// <param name="project"></param>
        public void SaveProject(Projects project)
        {

        }

        /// <summary>
        /// загрузка проектов залогиненного пользователя
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        //public ICollection<Projects> LoadMyProject(string email)
        //{
        //    ProjectsRepository projects = new ProjectsRepository();
        //    var getProjects = projects.GetList().Where(p => p.ProjectOwner.Equals(email)).ToList();

        //    return getProjects;
        //}

        public IEnumerable<Projects> GetShareProject(string email)
        {
            ProjectsRepository project = new ProjectsRepository();
            ProjectsShareRepository share = new ProjectsShareRepository();
            var getShareProject = from p in project.GetList()
                                  from ps in share.GetList()
                                  where (ps.ProjectID == p.ProjectID) && (ps.SharedEmail.Equals(email))
                                  select p;
            return getShareProject;
        }

        /// <summary>
        /// параметр id-ConnectionOwner
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ConnectionDB GetConnectionByProjectID(int id)
        {
            ConnectionDBRepository connectionRepo = new ConnectionDBRepository();

            return connectionRepo.GetItemById(id);
        }

        private bool ValidationUser(string email, string password, ref Users user)
        {
            UsersRepository repository = new UsersRepository();

            user = repository.GetList().Where(e => e.Email.Equals(email)).First();

            return user.PasswordHash.Equals(GetHashString(password));
        }


        public Guid GetHashString(string pass)
        {

            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return new Guid(hash);
        }
    }
}
