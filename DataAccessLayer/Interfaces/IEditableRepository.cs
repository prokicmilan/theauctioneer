namespace DataAccessLayer.Interfaces
{
    internal interface IEditableRepository<T>
    {
        void Save(T entity);

        void Remove(T entity);
    }
}
