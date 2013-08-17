using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB;
using MongoDB.Database;
using MongoDB.Contract;

namespace MongoDB.Database
{
    public class Reader<T> : Adapter<T>, IDatabaseReader<T> where T : EntryBase
    {
        public event ReadCompletedEvent<T> AsyncReadCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;

        public Reader(string connectionString, string databaseName)
            : base(new ServerConnection(connectionString), databaseName)
        {
            _mongoReaderAsync.AsyncReadCompleted += new ReadCompletedEvent<T>(_mongoReaderAsync_ReadCompleted);
            _mongoReaderAsync.AsyncDistinctCompleted += new DistinctCompletedEvent(_mongoReaderAsync_AsyncDistinctCompleted);
        }

        #region    Synchronous
        #region Read
        public IEnumerable<T> Read(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoReader.Read(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoReader.Read(collectionName, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(string collectionName, string fieldName, string regexPattern)
        {
            return _mongoReader.Read(collectionName, fieldName, regexPattern);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoReader.Read(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return _mongoReader.Read(collectionNames, fieldName, regexPattern);
        }

        public IEnumerable<T> Read(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoReader.Read(collectionNames, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region Distinct
        public IEnumerable<BsonValue> Distinct(string collectionName, string fieldName)
        {
            return _mongoReader.Distinct(collectionName, fieldName);
        }
        public IEnumerable<BsonValue> Distinct(string collectionName, string fieldName, IMongoQuery query)
        {
            return _mongoReader.Distinct(collectionName, fieldName, query);
        }
        public IEnumerable<BsonValue> Distinct(IEnumerable<string> collectionNames, string fieldName)
        {
            return _mongoReader.Distinct(collectionNames, fieldName);
        }
        public IEnumerable<BsonValue> Distinct(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return _mongoReader.Distinct(collectionNames, fieldName, query);
        }
        #endregion Distinct
        #endregion Synchronous

        #region    Asynchronous
        #region Read
        public void ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoReaderAsync.ReadAsync(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoReaderAsync.ReadAsync(collectionName, dateTimeFieldName, start, end);
        }

        public void ReadAsync(string collectionName, string fieldName, string regexPattern)
        {
            _mongoReaderAsync.ReadAsync(collectionName, fieldName, regexPattern);
        }

        public void ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoReaderAsync.ReadAsync(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            _mongoReaderAsync.ReadAsync(collectionNames, fieldName, regexPattern);
        }

        public void ReadAsync(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoReaderAsync.ReadAsync(collectionNames, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region Distinct
        public void DistinctAsync(string collectionName, string fieldName)
        {
            _mongoReaderAsync.DistinctAsync(collectionName, fieldName);
        }
        public void DistinctAsync(string collectionName, string fieldName, IMongoQuery query)
        {
            _mongoReaderAsync.DistinctAsync(collectionName, fieldName, query);
        }
        public void DistinctAsync(IEnumerable<string> collectionNames, string fieldName)
        {
            _mongoReaderAsync.DistinctAsync(collectionNames, fieldName);
        }
        public void DistinctAsync(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            _mongoReaderAsync.DistinctAsync(collectionNames, fieldName, query);
        }
        #endregion Distinct
        #endregion Asynchronous

        public IReader<T> Create(IDatabaseConnection<T> databaseConnection)
        {
            return new MongoDB.Reader<T>(databaseConnection);
        }

        public IReaderAsync<T> Create(IReader<T> reader)
        {
            return new ReaderAsync<T>(reader);
        }

        void _mongoReaderAsync_ReadCompleted(IEnumerable<T> e, Exception ex)
        {
            if (AsyncReadCompleted != null)
                AsyncReadCompleted(e, ex);
        }

        void _mongoReaderAsync_AsyncDistinctCompleted(IEnumerable<BsonValue> e, Exception ex)
        {
            if (AsyncDistinctCompleted != null)
                AsyncDistinctCompleted(e, ex);
        }
    }
}
