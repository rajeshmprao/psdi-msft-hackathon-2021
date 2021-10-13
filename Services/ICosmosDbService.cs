namespace PSDIPortal
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PSDIPortal.Models;

    public interface ICosmosDbService
    {
        Task<IEnumerable<T>> GetAsyncByQuery<T>(string query, string container);
        Task<dynamic> GetAsyncById(string id);
        Task AddAsync<T>(dynamic item, string containerName);
        Task UpdateAsync<T>(string id, T item, string containerName);
        Task DeleteAsync(string id);
    }
}