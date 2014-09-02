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
    }

    public class AsyncReader<T> : IAsyncReader<T>
    {
        private IAsyncReader _reader;

        public AsyncReader(IAsyncReader reader)
        {
            _reader = reader;
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern)
        {
            return Task.Run(() => { return _reader.ReadAsync<T>(collectionName, fieldName, regexPattern); });
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _reader.ReadAsync<T>(collectionName, fieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _reader.ReadAsync<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return Task.Run(() => { return _reader.ReadAsync<T>(collectionNames, fieldName, regexPattern); });
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _reader.ReadAsync<T>(collectionNames, fieldName, start, end); });
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return Task.Run(() => { return _reader.ReadAsync<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName)
        {
            return Task.Run(() => { return _reader.DistinctAsync<Y>(collectionName, fieldName); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName, IMongoQuery query)
        {
            return Task.Run(() => { return _reader.DistinctAsync<Y>(collectionName, fieldName, query); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName)
        {
            return Task.Run(() => { return _reader.DistinctAsync<Y>(collectionNames, fieldName); });
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return Task.Run(() => { return _reader.DistinctAsync<Y>(collectionNames, fieldName, query); });
        }
    }
}
