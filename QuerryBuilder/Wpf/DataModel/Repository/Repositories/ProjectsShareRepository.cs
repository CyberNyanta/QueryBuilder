using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.DataModel.Entity;

namespace Wpf.DataModel.Repository.Repositories
{
    class ProjectsShareRepository : IRepository<ProjectsShare>
    {
        private SqlConstructorDBEntities _context;

        public ProjectsShareRepository(SqlConstructorDBEntities context)
        {
            _context = context;
        }

        public void Create(ProjectsShare item)
        {
            _context.ProjectsShare.Add(item);
        }

        public ProjectsShare GetItemById(int id)
        {
            return _context.ProjectsShare.Find(id);
        }

        public IEnumerable<ProjectsShare> GetList()
        {
            return _context.ProjectsShare;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ProjectsShare item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ProjectsShare temp = _context.ProjectsShare.Find(id);
            if (temp != null)
                _context.ProjectsShare.Remove(temp);
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
