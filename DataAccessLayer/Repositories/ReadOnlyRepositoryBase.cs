using System;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Classes;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class ReadOnlyRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DataModel context = new DataModel();

        public TEntity GetById(int id)
        {
            return context.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return context.Set<TEntity>();
        }

        public IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return context.Set<TEntity>().Where(predicate);
        }
    }
}
