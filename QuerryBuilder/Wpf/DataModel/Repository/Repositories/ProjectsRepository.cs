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
        private SqlConstructorDBEntities context;

        public ProjectsRepository()
        {
            context = new SqlConstructorDBEntities();
        }

        public void Create(Projects item)
        {
            context.Projects.Add(item);
        }

        public Projects GetItemById(int id)
        {
            return context.Projects.Find(id);
        }

        public IEnumerable<Projects> GetList()
        {
            return context.Projects;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(Projects item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            Projects temp = context.Projects.Find(id);
            if (temp != null)
                context.Projects.Remove(temp);
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
