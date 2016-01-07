using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.DataModel.Entity;

namespace Wpf.DataModel.Repository.Repositories
{
    class UsersRepository : IRepository<Users>
    {
        private SqlConstructorDBEntities context;

        public UsersRepository()
        {
            context = new SqlConstructorDBEntities();
        }

        public void Create(Users item)
        {
            context.Users.Add(item);
        }

        public Users GetItemById(int id)
        {
            return context.Users.Find(id);
        }

        public IEnumerable<Users> GetList()
        {
            return context.Users;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Users item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Users temp = context.Users.Find(id);
            if (temp != null)
                context.Users.Remove(temp);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
