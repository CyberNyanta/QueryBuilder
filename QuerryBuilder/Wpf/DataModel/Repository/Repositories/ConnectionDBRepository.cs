using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.DataModel.Entity;

namespace Wpf.DataModel.Repository.Repositories
{
    class ConnectionDBRepository : IRepository<ConnectionDB>
    {
        private SqlConstructorDBEntities _context;

        public ConnectionDBRepository(SqlConstructorDBEntities context)
        {
            _context = context;
        }

        public void Create(ConnectionDB item)
        {
            _context.ConnectionDB.Add(item);
        }

        public ConnectionDB GetItemById(int id)
        {
            return _context.ConnectionDB.Find(id);
        }

        public IEnumerable<ConnectionDB> GetList()
        {
            return _context.ConnectionDB;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ConnectionDB item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ConnectionDB temp = _context.ConnectionDB.Find(id);
            if (temp != null)
                _context.ConnectionDB.Remove(temp);
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
