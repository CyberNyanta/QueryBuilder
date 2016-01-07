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
        private SqlConstructorDBEntities context;

        public ProjectsShareRepository()
        {
            context = new SqlConstructorDBEntities();
        }

        public void Create(ProjectsShare item)
        {
            context.ProjectsShare.Add(item);
        }

        public ProjectsShare GetItemById(int id)
        {
            return context.ProjectsShare.Find(id);
        }

        public IEnumerable<ProjectsShare> GetList()
        {
            return context.ProjectsShare;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(ProjectsShare item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ProjectsShare temp = context.ProjectsShare.Find(id);
            if (temp != null)
                context.ProjectsShare.Remove(temp);
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
