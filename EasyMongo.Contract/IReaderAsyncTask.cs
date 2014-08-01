using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IReaderAsyncTask
    {
        Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, string regexPattern);
        Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, DateTime start, DateTime end);
        Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern);
        Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end);
        Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        Task<IEnumerable<T>> DistinctAsync<T>(string collectionName, string fieldName);
        Task<IEnumerable<T>> DistinctAsync<T>(string collectionName, string fieldName, IMongoQuery query);
        Task<IEnumerable<T>> DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName);
        Task<IEnumerable<T>> DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query);
    }
}
