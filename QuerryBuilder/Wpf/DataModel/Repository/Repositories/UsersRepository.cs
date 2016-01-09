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
        private SqlConstructorDBEntities _context;

        public UsersRepository(SqlConstructorDBEntities context)
        {
            _context = context;
        }

        public void Create(Users item)
        {
            _context.Users.Add(item);
        }

        public Users GetItemById(int id)
        {
            return _context.Users.Find(id);
        }

        public IEnumerable<Users> GetList()
        {
            return _context.Users;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Users item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Users temp = _context.Users.Find(id);
            if (temp != null)
                _context.Users.Remove(temp);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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
