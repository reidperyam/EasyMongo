using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface ICollectionReader
    {
        IEnumerable<T> Read<T>();
        IEnumerable<T> Read<T>(string fieldName, string regexPattern);
        IEnumerable<T> Read<T>(string dateTimeFieldName, DateTime start, DateTime end);
        IEnumerable<T> Read<T>(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        Task<IEnumerable<T>> ReadAsync<T>();
        Task<IEnumerable<T>> ReadAsync<T>(string fieldName, string regexPattern);
        Task<IEnumerable<T>> ReadAsync<T>(string dateTimeFieldName, DateTime start, DateTime end);
        Task<IEnumerable<T>> ReadAsync<T>(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        IEnumerable<T> Distinct<T>(string fieldName);
        IEnumerable<T> Distinct<T>(string fieldName, IMongoQuery query);

        Task<IEnumerable<T>> DistinctAsync<T>(string fieldName);
        Task<IEnumerable<T>> DistinctAsync<T>(string fieldName, IMongoQuery query);

        IEnumerable<T> Execute<T>(IMongoQuery mongoQuery);
        IEnumerable<T> ExecuteAnds<T>(IEnumerable<IMongoQuery> mongoQueries);
        IEnumerable<T> ExecuteOrs<T>(IEnumerable<IMongoQuery> mongoQueries);

        Task<IEnumerable<T>> ExecuteAsync<T>(IMongoQuery mongoQuery);
        Task<IEnumerable<T>> ExecuteAndsAsync<T>(IEnumerable<IMongoQuery> mongoQueries);
        Task<IEnumerable<T>> ExecuteOrsAsync<T>(IEnumerable<IMongoQuery> mongoQueries);
    }
}
