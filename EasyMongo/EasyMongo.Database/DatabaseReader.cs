using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Contract.Delegates;

namespace EasyMongo.Database
{
    public class DatabaseReader : IDatabaseReader
    {
        protected IReader _reader;

        protected IReaderTask _readerTask;

        protected MongoCollection _mongoCollection;

        public DatabaseReader(IReader     reader, 
                              IReaderTask readerTask)
        {
            _reader = reader;
            _readerTask = readerTask;
        }

        #region    Synchronous
        #region Read
        public IEnumerable<T> Read<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _reader.Read<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _reader.Read<T>(collectionName, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(string collectionName, string fieldName, string regexPattern)
        {
            return _reader.Read<T>(collectionName, fieldName, regexPattern);
        }

        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _reader.Read<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return _reader.Read<T>(collectionNames, fieldName, regexPattern);
        }

        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _reader.Read<T>(collectionNames, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region Distinct T
        public IEnumerable<T> Distinct<T>(string collectionName, string fieldName)
        {
            return _reader.Distinct<T>(collectionName, fieldName);
        }
        public IEnumerable<T> Distinct<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            return _reader.Distinct<T>(collectionName, fieldName, query);
        }
        public IEnumerable<T> Distinct<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            return _reader.Distinct<T>(collectionNames, fieldName);
        }
        public IEnumerable<T> Distinct<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return _reader.Distinct<T>(collectionNames, fieldName, query);
        }
        #endregion Distinct T
        #endregion Synchronous

        #region    Asynchronous
        #region Read
        public Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _readerTask.ReadAsync<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _readerTask.ReadAsync<T>(collectionName, dateTimeFieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string collectionName, string fieldName, string regexPattern)
        {
            return _readerTask.ReadAsync<T>(collectionName, fieldName, regexPattern);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _readerTask.ReadAsync<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return _readerTask.ReadAsync<T>(collectionNames, fieldName, regexPattern);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _readerTask.ReadAsync<T>(collectionNames, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region Distinct T
        public Task<IEnumerable<T>> DistinctAsync<T>(string collectionName, string fieldName)
        {
            return _readerTask.DistinctAsync<T>(collectionName, fieldName);
        }
        public Task<IEnumerable<T>> DistinctAsync<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            return _readerTask.DistinctAsync<T>(collectionName, fieldName, query);
        }
        public Task<IEnumerable<T>> DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            return _readerTask.DistinctAsync<T>(collectionNames, fieldName);
        }
        public Task<IEnumerable<T>> DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return _readerTask.DistinctAsync<T>(collectionNames, fieldName, query);
        }
        #endregion Distinct T
        #endregion Asynchronous
    }

    public class DatabaseReader<T> : IDatabaseReader<T> where T : class
    {
        IDatabaseReader _databaseReader;

        public DatabaseReader(IDatabaseReader databaseReader)
        {
            _databaseReader = databaseReader;
        }

        #region    Synchronous
        #region    Read
        public IEnumerable<T> Read(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.Read<T>(collectionName, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.Read<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(string collectionName, string fieldName, string regexPattern)
        {
            return _databaseReader.Read<T>(collectionName, fieldName, regexPattern);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            return _databaseReader.Read<T>(collectionNames, fieldName, start, end);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.Read<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return _databaseReader.Read<T>(collectionNames, fieldName, regexPattern);
        }
        #endregion Read
        #region    Distinct
        public IEnumerable<Y> Distinct<Y>(string collectionName, string fieldName)
        {
            return _databaseReader.Distinct<Y>(collectionName, fieldName);
        }

        public IEnumerable<Y> Distinct<Y>(string collectionName, string fieldName, IMongoQuery query)
        {
            return _databaseReader.Distinct<Y>( collectionName, fieldName, query);
        }

        public IEnumerable<Y> Distinct<Y>(IEnumerable<string> collectionNames, string fieldName)
        {
            return _databaseReader.Distinct<Y>(collectionNames, fieldName);
        }

        public IEnumerable<Y> Distinct<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return _databaseReader.Distinct<Y>(collectionNames, fieldName, query);
        }
        #endregion Distinct
        #endregion Synchronous
        #region    Asynchronous
        #region    Read
        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern)
        {
            return _databaseReader.ReadAsync<T>(collectionName, fieldName, regexPattern);
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, DateTime start, DateTime end)
        {
            return _databaseReader.ReadAsync<T>(collectionName, fieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.ReadAsync<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return _databaseReader.ReadAsync<T>(collectionNames, fieldName, regexPattern);
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            return _databaseReader.ReadAsync<T>(collectionNames, fieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.ReadAsync<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region    Distinct
        public Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName)
        {
            return _databaseReader.DistinctAsync<Y>(collectionName, fieldName);
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string collectionName, string fieldName, IMongoQuery query)
        {
            return _databaseReader.DistinctAsync<Y>(collectionName, fieldName, query);
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName)
        {
            return _databaseReader.DistinctAsync<Y>(collectionNames, fieldName);
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return _databaseReader.DistinctAsync<Y>(collectionNames, fieldName, query);
        }
        #endregion Distinct
        #endregion Asynchronous
    }
}
