using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace INVENTORYWeb.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(object id);
        IEnumerable<T> GetAll(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = null
           );
        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );
        void Save();
        void Add(T entity);
        void Remove(object id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
