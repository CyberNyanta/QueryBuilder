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
        private SqlConstructorDBEntities context;

        public ConnectionDBRepository()
        {
            context = new SqlConstructorDBEntities();
        }

        public void Create(ConnectionDB item)
        {
            context.ConnectionDB.Add(item);
        }

        public ConnectionDB GetItemById(int id)
        {
            return context.ConnectionDB.Find(id);
        }

        public IEnumerable<ConnectionDB> GetList()
        {
            return context.ConnectionDB;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(ConnectionDB item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ConnectionDB temp = context.ConnectionDB.Find(id);
            if (temp != null)
                context.ConnectionDB.Remove(temp);
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
