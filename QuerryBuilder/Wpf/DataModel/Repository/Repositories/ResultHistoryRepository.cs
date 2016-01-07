using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.DataModel.Entity;

namespace Wpf.DataModel.Repository.Repositories
{
    class ResultHistoryRepository : IRepository<ResultHistory>
    {
        private SqlConstructorDBEntities context;

        public ResultHistoryRepository()
        {
            context = new SqlConstructorDBEntities();
        }

        public void Create(ResultHistory item)
        {
            context.ResultHistory.Add(item);
        }

        public ResultHistory GetItemById(int id)
        {
            return context.ResultHistory.Find(id);
        }

        public IEnumerable<ResultHistory> GetList()
        {
            return context.ResultHistory;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Update(ResultHistory item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ResultHistory temp = context.ResultHistory.Find(id);
            if (temp != null)
                context.ResultHistory.Remove(temp);
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
