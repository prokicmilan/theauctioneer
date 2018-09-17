using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    internal interface IRepository<T>
    {
        T GetById(Guid id);

        IQueryable<T> GetAll();

        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
    }
}
