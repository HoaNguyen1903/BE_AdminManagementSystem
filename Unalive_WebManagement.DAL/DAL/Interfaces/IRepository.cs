namespace Unalive_WebManagement.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(params object[] keyValues);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(params object[] keyValues);
        Task<bool> ExistsAsync(params object[] keyValues);
    }
}