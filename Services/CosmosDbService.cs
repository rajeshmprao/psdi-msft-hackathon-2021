namespace PSDIPortal
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using PSDIPortal.Models;
    using Microsoft.Azure.Cosmos;
    using Microsoft.Azure.Cosmos.Fluent;
    using Microsoft.Extensions.Configuration;

    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;
        private CosmosClient _dbClient;
        private string _databaseName;
        private string _containerName;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName
            )
        {
            this._databaseName = databaseName;
            this._dbClient = dbClient;
        }

        public async Task AddAsync<T>(dynamic item, string containerName)
        {
            this._container = this._dbClient.GetContainer(this._databaseName, containerName);
            await this._container.CreateItemAsync<T>(item, new PartitionKey(item.Id));
        }

        public async Task DeleteAsync(string id)
        {
            await this._container.DeleteItemAsync<dynamic>(id, new PartitionKey(id));
        }

        public async Task<dynamic> GetAsyncById(string id)
        {
            try
            {
                ItemResponse<dynamic> response = await this._container.ReadItemAsync<dynamic>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<T>> GetAsyncByQuery<T>(string queryString, string containerName)
        {
            this._container = this._dbClient.GetContainer(this._databaseName, containerName);
            var query = this._container.GetItemQueryIterator<T>(new QueryDefinition(queryString));
            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateAsync<T>(string id, T item, string containerName)
        {
            this._container = this._dbClient.GetContainer(this._databaseName, containerName);
            await this._container.UpsertItemAsync<T>(item, new PartitionKey(id));
        }
    }
}