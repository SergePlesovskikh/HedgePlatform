using HedgePlatform.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace HedgePlatform.DAL.Repositories
{
    public class AbstractRepository<T> : IRepository<T> where T : class 
    {
        DbContext _context;
        DbSet<T> _db;

        public AbstractRepository(DbContext context)
        {
            _context = context;
            _db = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return _db.AsNoTracking().ToList();
        }

        public T Get(int id)
        {
            return _db.Find(id);
        }

        public void Create(T item)
        {
            _db.Add(item);
        }
    
        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<T> Find(Func<T, Boolean> predicate)
        {
            return _db.AsNoTracking().Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            _db.Remove(Get(id));
        }

        public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public IEnumerable<T> GetWithInclude(Func<T, bool> predicate,
            params Expression<Func<T, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).ToList();
        }

        private IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _db.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}
