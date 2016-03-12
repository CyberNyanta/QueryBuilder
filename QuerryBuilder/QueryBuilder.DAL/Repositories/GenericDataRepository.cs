using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using QueryBuilder.DAL.Contexts;
using QueryBuilder.DAL.Contracts;

namespace QueryBuilder.DAL.Repositories
{
    public class GenericDataRepository<T>:IGenericDataRepository<T> where T : class
    {
        private readonly QueryBuilderContext _context;

        public GenericDataRepository(QueryBuilderContext context)
        {
            _context = context;
        }

        public virtual IList<T> GetAll(params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();

            //Apply eager loading
            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            return dbQuery
                .AsNoTracking()
                .ToList<T>();
        }

        public virtual IList<T> GetList(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();

            //Apply eager loading
            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            return dbQuery
                .AsNoTracking()
                .Where(where)
                .ToList<T>();
        }

        public virtual T FindBy(Func<T, bool> where,
             params Expression<Func<T, object>>[] navigationProperties)
        {
            IQueryable<T> dbQuery = _context.Set<T>();

            //Apply eager loading
            foreach (Expression<Func<T, object>> navigationProperty in navigationProperties)
                dbQuery = dbQuery.Include<T, object>(navigationProperty);

            return dbQuery
                .AsNoTracking() //Don't track any changes for the selected item
                .FirstOrDefault(where); //Apply where clause
        }

        public void Add(params T[] items)
        {
            throw new NotImplementedException();
        }

        public void Update(params T[] items)
        {
            throw new NotImplementedException();
        }

        public void Remove(params T[] items)
        {
            throw new NotImplementedException();
        }
    }
}
