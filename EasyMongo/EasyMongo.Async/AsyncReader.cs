using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder;
using EasyMongo.Contract;

namespace EasyMongo.Async
{
    public class AsyncReader : IAsyncReader
    {
        private IReader _reader;

        public AsyncReader(IReader reader)
        {
            _reader = reader;
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string collectionName)
        {
            return Task.Run(() => { return _reader.Read<T>(collectionName); });
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, string regexPattern)
        {
            return Task.Run(() => { return _reader.Read<T>(collectionName, fieldName, regexPattern); });
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _reader.Read<T>(collectionName, fieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _reader.Read<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames)
        {
            return Task.Run(() => { return _reader.Read<T>(collectionNames); });
        }

        public Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return Task.Run(() => { return _reader.Read<T>(collectionNames, fieldName, regexPattern); });
        }

        public Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _reader.Read<T>(collectionNames, fieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _reader.Read<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName)
        {
            return Task.Run(() => { return _reader.Distinct<Y>(collectionName, fieldName); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName, IMongoQuery query)
        {
            return Task.Run(() => { return _reader.Distinct<Y>(collectionName, fieldName, query); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName)
        {
            return Task.Run(() => { return _reader.Distinct<Y>(collectionNames, fieldName); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return Task.Run(() => { return _reader.Distinct<Y>(collectionNames, fieldName, query); });
        }

        public Task<IEnumerable<T>> ExecuteAsync<T>(string collectionName, IMongoQuery mongoQuery)
        {
            return Task.Run(() => { return _reader.Execute<T>(collectionName, mongoQuery); });
        }

        public Task<IEnumerable<T>> ExecuteAndsAsync<T>(string collectionName, IEnumerable<IMongoQuery> mongoQueries)
        {
            return Task.Run(() => { return _reader.ExecuteAnds<T>(collectionName, mongoQueries); });
        }

        public Task<IEnumerable<T>> ExecuteOrsAsync<T>(string collectionName, IEnumerable<IMongoQuery> mongoQueries)
        {
            return Task.Run(() => { return _reader.ExecuteOrs<T>(collectionName, mongoQueries); });
        }
    }

    public class AsyncReader<T> : IAsyncReader<T>
    {
        private IAsyncReader _asyncReader;

        public AsyncReader(IAsyncReader asyncReader)
        {
            _asyncReader = asyncReader;
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName)
        {
            return Task.Run(() => { return _asyncReader.ReadAsync<T>(collectionName); });
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern)
        {
            return Task.Run(() => { return _asyncReader.ReadAsync<T>(collectionName, fieldName, regexPattern); });
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _asyncReader.ReadAsync<T>(collectionName, fieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _asyncReader.ReadAsync<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames)
        {
            return Task.Run(() => { return _asyncReader.ReadAsync<T>(collectionNames); });
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return Task.Run(() => { return _asyncReader.ReadAsync<T>(collectionNames, fieldName, regexPattern); });
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _asyncReader.ReadAsync<T>(collectionNames, fieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _asyncReader.ReadAsync<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName)
        {
            return Task.Run(() => { return _asyncReader.DistinctAsync<Y>(collectionName, fieldName); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName, IMongoQuery query)
        {
            return Task.Run(() => { return _asyncReader.DistinctAsync<Y>(collectionName, fieldName, query); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName)
        {
            return Task.Run(() => { return _asyncReader.DistinctAsync<Y>(collectionNames, fieldName); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return Task.Run(() => { return _asyncReader.DistinctAsync<Y>(collectionNames, fieldName, query); });
        }

        public Task<IEnumerable<T>> ExecuteAsync(string collectionName, IMongoQuery mongoQuery)
        {
            return Task.Run(() => { return _asyncReader.ExecuteAsync<T>(collectionName, mongoQuery); });
        }

        public Task<IEnumerable<T>> ExecuteAndsAsync(string collectionName, IEnumerable<IMongoQuery> mongoQueries)
        {
            return Task.Run(() => { return _asyncReader.ExecuteAndsAsync<T>(collectionName, mongoQueries); });
        }

        public Task<IEnumerable<T>> ExecuteOrsAsync(string collectionName, IEnumerable<IMongoQuery> mongoQueries)
        {
            return Task.Run(() => { return _asyncReader.ExecuteOrsAsync<T>(collectionName, mongoQueries); });
        }
    }
}
