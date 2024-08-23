namespace ELibrary.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        
        Task<TEntity?> GetById(Guid? id);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);
        
        void Update(TEntity entity);
        
        void UpdateRange(IEnumerable<TEntity> entities);

        void Remove(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);
    }
}