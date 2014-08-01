using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IReaderTask<T>
    {
        Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern);
        Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, DateTime start, DateTime end);
        Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern);
        Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end);
        Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName);
        Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName, IMongoQuery query);
        Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName);
        Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query);
    }
}
