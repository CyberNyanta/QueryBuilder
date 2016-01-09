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
    class EntityManager
    {
        private EntityManager() { }


        /// <summary>
        /// Регистрация нового пользователя
        /// Валидация вводимых параметров (имя, мыло) происходит во ViewModel
        /// </summary>
        /// <param name="user"></param>
        public void RegistrationUser(Users user)
        {
            UsersRepository temp = new UsersRepository();
            temp.Create(user);
            temp.Save();
        }

        public Users LoginUser(string email, string password)
        {
            //if (ValidationUser(email, password))
            //{
            //    IEnumerable<Projects> MyProjects = LoadMyProject(email);
            //    IEnumerable<Projects> ShareProjects = GetShareProject(email);

                Users user = new Users { Email = email };
                return user;
            //}
            //throw new ArgumentException();
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
        public IEnumerable<Projects> LoadMyProject(string email)
        {
            //UsersRepository users = new UsersRepository();
            //var getProjects = from a in users.GetList()
            //                  from p in a.Projects
            //                  where a.Email.Equals(email)
            //                  select (p);
            ProjectsRepository projects = new ProjectsRepository();
            var getProjects = projects.GetList().Where(p => p.ProjectOwner.Equals(email));

            return getProjects;
        }

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

        //private bool ValidationUser(string email, string password)
        //{
        //    bool result = false;

        //    using (DbConnection conn = _connection)
        //    {
        //        conn.ConnectionString = Resource.ConnectionString;
        //        IDbCommand cmd = conn.CreateCommand();
        //        cmd.CommandText = string.Format("USE SqlConstructorDB SELECT Email, PasswordHash FROM Users WHERE Email ='{0}'", email);
        //        conn.Open();

        //        IDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            string tempMail = reader.GetString(0);
        //            string tempPass = reader.GetGuid(1).ToString();
        //            result = (email.Equals(tempMail) && GetHashString(password).ToString().Equals(tempPass));
        //        }
        //        reader.Close();

        //    }
        //    return result;
        //}
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
