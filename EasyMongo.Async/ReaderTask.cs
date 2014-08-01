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
    public class ReaderTask : IReaderTask
    {
        private IReader _reader;

        public ReaderTask(IReader reader)
        {
            _reader = reader;
        }

        public async Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, string regexPattern)
        {
            return await Task.Run(() => { return _reader.Read<T>(collectionName, fieldName, regexPattern); });
        }

        public async Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, DateTime start, DateTime end)
        {
            return await Task.Run(() => { return _reader.Read<T>(collectionName, fieldName, start, end); });
        }

        public async Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return await Task.Run(() => { return _reader.Read<T>(collectionName, fieldName, start, end); });
        }

        public async Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return await Task.Run(() => { return _reader.Read<T>(collectionNames, fieldName, regexPattern); });
        }

        public async Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            return await Task.Run(() => { return _reader.Read<T>(collectionNames, fieldName, start, end); });
        }

        public async Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return await Task.Run(() => { return _reader.Read<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public async Task<IEnumerable<T>> DistinctAsync<T>(string collectionName, string fieldName)
        {
            return await Task.Run(() => { return _reader.Distinct<T>(collectionName, fieldName); });
        }

        public async Task<IEnumerable<T>> DistinctAsync<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            return await Task.Run(() => { return _reader.Distinct<T>(collectionName, fieldName, query); });
        }

        public async Task<IEnumerable<T>> DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            return await Task.Run(() => { return _reader.Distinct<T>(collectionNames, fieldName); });
        }

        public async Task<IEnumerable<T>> DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return await Task.Run(() => { return _reader.Distinct<T>(collectionNames, fieldName, query); });
        }
    }

    public class ReaderTask<T> : IReaderTask<T>
    {
        private IReaderTask _reader;

        public ReaderTask(IReaderTask reader)
        {
            _reader = reader;
        }

        public async Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern)
        {
            return await Task.Run(() => { return _reader.ReadAsync<T>(collectionName, fieldName, regexPattern); });
        }

        public async Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, DateTime start, DateTime end)
        {
            return await Task.Run(() => { return _reader.ReadAsync<T>(collectionName, fieldName, start, end); });
        }

        public async Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return await Task.Run(() => { return _reader.ReadAsync<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public async Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return await Task.Run(() => { return _reader.ReadAsync<T>(collectionNames, fieldName, regexPattern); });
        }

        public async Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            return await Task.Run(() => { return _reader.ReadAsync<T>(collectionNames, fieldName, start, end); });
        }

        public async Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return await Task.Run(() => { return _reader.ReadAsync<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end); });
        }

        public async Task<IEnumerable<T>> DistinctAsync<Y>(string collectionName, string fieldName)
        {
            return await Task.Run(() => { return _reader.DistinctAsync<T>(collectionName, fieldName); });
        }

        public async Task<IEnumerable<T>> DistinctAsync<Y>(string collectionName, string fieldName, IMongoQuery query)
        {
            return await Task.Run(() => { return _reader.DistinctAsync<T>(collectionName, fieldName, query); });
        }

        public async Task<IEnumerable<T>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName)
        {
            return await Task.Run(() => { return _reader.DistinctAsync<T>(collectionNames, fieldName); });
        }

        public async Task<IEnumerable<T>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return await Task.Run(() => { return _reader.DistinctAsync<T>(collectionNames, fieldName, query); });
        }
    }
}
