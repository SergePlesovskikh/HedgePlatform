using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace HedgePlatform.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {        
        IEnumerable<T> GetAll();
        T Get(int id);
        T GetOneWithInclude(Func<T, bool> predicate, params Expression<Func<T, object>>[] includeProperties);
        public IEnumerable<T> GetWithInclude(params Expression<Func<T, object>>[] includeProperties);
        public IEnumerable<T> GetWithInclude(Func<T, bool> predicate,
           params Expression<Func<T, object>>[] includeProperties);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        T FindFirst(Func<T, Boolean> predicate);
        T Create(T item);
        T Update(T item);
        void Delete(int id);
    }
}
