using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core
{
    public interface IAsyncDataRepository<T>
    {
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> InsertAsync(T obj);
        Task<int> InsertAsync(IEnumerable<T> list);
        Task<bool> UpdateAsync(T obj);
        Task<bool> UpdateAsync(IEnumerable<T> list);
        Task<bool> DeleteAsync(T obj);
        Task<bool> DeleteAsync(IEnumerable<T> list);
        Task<bool> DeleteAllAsync();
        Task<T> GetByForeignKeyAsync(int foreignKeyValue, string foreignKeyColumn, int pk);
        Task<IEnumerable<T>> GetFilterByForeignKey(int foreignKeyValue, string foreignKeyColumn);
        Task<T> GetFilterByInheritedAsync(Type type, int id);
        Task<IEnumerable<T>> GetFilterByInheritedAsync(Type type);
    }
}
