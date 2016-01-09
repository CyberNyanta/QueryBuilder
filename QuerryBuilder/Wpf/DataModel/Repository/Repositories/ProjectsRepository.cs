using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.DataModel.Entity;

namespace Wpf.DataModel.Repository.Repositories
{
    class ProjectsRepository : IRepository<Projects>
    {
        private SqlConstructorDBEntities _context;

        public ProjectsRepository(SqlConstructorDBEntities context)
        {
            _context = context;
        }

        public void Create(Projects item)
        {
            _context.Projects.Add(item);
        }

        public Projects GetItemById(int id)
        {
            return _context.Projects.Find(id);
        }

        public IEnumerable<Projects> GetList()
        {
            return _context.Projects;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Projects item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Projects temp = _context.Projects.Find(id);
            if (temp != null)
                _context.Projects.Remove(temp);
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
