using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo;
using EasyMongo.Async;
using EasyMongo.Contract;

namespace EasyMongo.Database
{
    public class Reader : Adapter, IDatabaseReader
    {
        public event ReadCompletedEvent AsyncReadCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;

        public Reader(string        connectionString, 
                      string        databaseName,
                      IReader       reader, 
                      IWriter       writer, 
                      IUpdater      updater,
                      IReaderAsync  readerAsync, 
                      IWriterAsync  writerAsync,
                      IUpdaterAsync updaterAsync)
            : base(reader,
                   writer,
                   updater,
                   readerAsync,
                   writerAsync,
                   updaterAsync)
        {
            _mongoReaderAsync.AsyncReadCompleted     += new ReadCompletedEvent(_mongoReaderAsync_ReadCompleted);
            _mongoReaderAsync.AsyncDistinctCompleted += new DistinctCompletedEvent(_mongoReaderAsync_AsyncDistinctCompleted);
        }

        #region    Synchronous
        #region Read
        public IEnumerable<T> Read<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoReader.Read<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoReader.Read<T>(collectionName, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(string collectionName, string fieldName, string regexPattern)
        {
            return _mongoReader.Read<T>(collectionName, fieldName, regexPattern);
        }

        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoReader.Read<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            return _mongoReader.Read<T>(collectionNames, fieldName, regexPattern);
        }

        public IEnumerable<T> Read<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoReader.Read<T>(collectionNames, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region Distinct
        public IEnumerable<BsonValue> Distinct<T>(string collectionName, string fieldName)
        {
            return _mongoReader.Distinct<T>(collectionName, fieldName);
        }
        public IEnumerable<BsonValue> Distinct<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            return _mongoReader.Distinct<T>(collectionName, fieldName, query);
        }
        public IEnumerable<BsonValue> Distinct<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            return _mongoReader.Distinct<T>(collectionNames, fieldName);
        }
        public IEnumerable<BsonValue> Distinct<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return _mongoReader.Distinct<T>(collectionNames, fieldName, query);
        }
        #endregion Distinct
        #endregion Synchronous

        #region    Asynchronous
        #region Read
        public void ReadAsync<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoReaderAsync.ReadAsync<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoReaderAsync.ReadAsync<T>(collectionName, dateTimeFieldName, start, end);
        }

        public void ReadAsync<T>(string collectionName, string fieldName, string regexPattern)
        {
            _mongoReaderAsync.ReadAsync<T>(collectionName, fieldName, regexPattern);
        }

        public void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoReaderAsync.ReadAsync<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            _mongoReaderAsync.ReadAsync<T>(collectionNames, fieldName, regexPattern);
        }

        public void ReadAsync<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoReaderAsync.ReadAsync<T>(collectionNames, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region Distinct
        public void DistinctAsync<T>(string collectionName, string fieldName)
        {
            _mongoReaderAsync.DistinctAsync<T>(collectionName, fieldName);
        }
        public void DistinctAsync<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            _mongoReaderAsync.DistinctAsync<T>(collectionName, fieldName, query);
        }
        public void DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            _mongoReaderAsync.DistinctAsync<T>(collectionNames, fieldName);
        }
        public void DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            _mongoReaderAsync.DistinctAsync<T>(collectionNames, fieldName, query);
        }
        #endregion Distinct
        #endregion Asynchronous

        public IReader Create(IDatabaseConnection databaseConnection)
        {
            return new EasyMongo.Reader(databaseConnection);
        }

        public IReaderAsync Create(IReader reader)
        {
            return new ReaderAsync(reader);
        }

        /// <summary>
        /// NOTICE - the object returned must be cast to IEnumerable<T> in order to retrieve read results
        /// </summary>
        /// <param name="e"></param>
        /// <param name="ex"></param>
        void _mongoReaderAsync_ReadCompleted(object e, Exception ex)
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
