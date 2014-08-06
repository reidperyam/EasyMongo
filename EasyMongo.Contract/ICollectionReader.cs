using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Contract
{
    public interface ICollectionReader<T>
    {
        IEnumerable<T> Read(string fieldName, string regexPattern);
        IEnumerable<T> Read(string dateTimeFieldName, DateTime start, DateTime end);
        IEnumerable<T> Read(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        Task<IEnumerable<T>> ReadAsync(string fieldName, string regexPattern);
        Task<IEnumerable<T>> ReadAsync(string dateTimeFieldName, DateTime start, DateTime end);
        Task<IEnumerable<T>> ReadAsync(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        IEnumerable<Y> Distinct<Y>(string fieldName);
        IEnumerable<Y> Distinct<Y>(string fieldName, IMongoQuery query);

        Task<IEnumerable<Y>> DistinctAsync<Y>(string fieldName);
        Task<IEnumerable<Y>> DistinctAsync<Y>(string fieldName, IMongoQuery query);
    }
}
