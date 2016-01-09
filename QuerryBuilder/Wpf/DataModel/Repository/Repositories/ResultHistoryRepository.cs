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
        private SqlConstructorDBEntities _context;

        public ResultHistoryRepository(SqlConstructorDBEntities context)
        {
            _context = context;
        }

        public void Create(ResultHistory item)
        {
            _context.ResultHistory.Add(item);
        }

        public ResultHistory GetItemById(int id)
        {
            return _context.ResultHistory.Find(id);
        }

        public IEnumerable<ResultHistory> GetList()
        {
            return _context.ResultHistory;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(ResultHistory item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            ResultHistory temp = _context.ResultHistory.Find(id);
            if (temp != null)
                _context.ResultHistory.Remove(temp);
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
