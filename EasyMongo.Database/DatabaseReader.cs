using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Database
{
    public class DatabaseReader : IDatabaseReader
    {
        public event ReadCompletedEvent AsyncReadCompleted;
        public event DistinctBSONCompletedEvent AsyncDistinctBSONCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;

        protected IReader _mongoReader;

        protected IReaderAsync _mongoReaderAsync;

        protected MongoCollection _mongoCollection;

        public DatabaseReader(IReader      reader, 
                              IReaderAsync readerAsync)
        {
            _mongoReader = reader;
            _mongoReaderAsync = readerAsync;

            _mongoReaderAsync.AsyncReadCompleted         += new ReadCompletedEvent(_mongoReaderAsync_ReadCompleted);
            _mongoReaderAsync.AsyncDistinctBSONCompleted += new DistinctBSONCompletedEvent(_mongoReaderAsync_AsyncDistinctCompleted);
            _mongoReaderAsync.AsyncDistinctCompleted     += new DistinctCompletedEvent(_mongoReaderAsync_AsyncDistinctCompleted);
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
        #region Distinct BSON
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
        #endregion Distinct BSON
        #region Distinct T
        public IEnumerable<T> Distinct<T>(string collectionName, string fieldName)
        {
            return _mongoReader.Distinct<T>(collectionName, fieldName);
        }
        public IEnumerable<T> Distinct<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            return _mongoReader.Distinct<T>(collectionName, fieldName, query);
        }
        public IEnumerable<T> Distinct<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            return _mongoReader.Distinct<T>(collectionNames, fieldName);
        }
        public IEnumerable<T> Distinct<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            return _mongoReader.Distinct<T>(collectionNames, fieldName, query);
        }
        #endregion Distinct T
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
        #region Distinct BSON
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
        #endregion Distinct BSON
        #region Distinct T
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
        #endregion Distinct T
        #endregion Asynchronous

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
            if (AsyncDistinctBSONCompleted != null)
                AsyncDistinctBSONCompleted(e, ex);
        }

        void _mongoReaderAsync_AsyncDistinctCompleted(object e, Exception ex)
        {
            if (AsyncDistinctCompleted != null)
                AsyncDistinctCompleted(e, ex);
        }
    }
}
