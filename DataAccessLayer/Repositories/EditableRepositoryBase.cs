using System.Data.Entity.Migrations;
using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class EditableRepositoryBase<TEntity> : ReadOnlyRepositoryBase<TEntity>, IEditableRepository<TEntity> where TEntity : class
    {
        public void Save(TEntity entity)
        {
            context.Set<TEntity>().AddOrUpdate(entity);
            context.SaveChanges();
        }

        public void Remove(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();
        }
    }
}
