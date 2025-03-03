﻿using HedgePlatform.DAL.Interfaces;
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

        public IEnumerable<T> GetAll() => _db.AsNoTracking().ToList();        

        public T Get(int id) => _db.Find(id);
        
        public T GetOneWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return query.Where(predicate).FirstOrDefault();
        }

        public T Create(T item)
        {
             _db.Add(item);
            return item;
        }
    
        public T Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            return item;
        }

        public IEnumerable<T> Find(Func<T, Boolean> predicate) => _db.AsNoTracking().Where(predicate).ToList();
        

        public T FindFirst(Func<T, Boolean> predicate) => _db.AsNoTracking().Where(predicate).ToList().FirstOrDefault();        

        public void Delete(int id) => _db.Remove(Get(id));        

        public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties) => Include(includeProperties).ToList();
        

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
