using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Database
{
    public class DatabaseReader : IDatabaseReader
    {
        public event ReadCompletedEvent AsyncReadCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;

        protected IReader _reader;

        protected IReaderAsync _readerAsync;

        protected MongoCollection _mongoCollection;

        public DatabaseReader(IReader      reader, 
                              IReaderAsync readerAsync)
        {
            _reader = reader;
            _readerAsync = readerAsync;

            _readerAsync.AsyncReadCompleted     += new ReadCompletedEvent(_mongoReaderAsync_ReadCompleted);
            _readerAsync.AsyncDistinctCompleted += new DistinctCompletedEvent(_mongoReaderAsync_AsyncDistinctCompleted);
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
        public void ReadAsync<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _readerAsync.ReadAsync<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _readerAsync.ReadAsync<T>(collectionName, dateTimeFieldName, start, end);
        }

        public void ReadAsync<T>(string collectionName, string fieldName, string regexPattern)
        {
            _readerAsync.ReadAsync<T>(collectionName, fieldName, regexPattern);
        }

        public void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _readerAsync.ReadAsync<T>(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            _readerAsync.ReadAsync<T>(collectionNames, fieldName, regexPattern);
        }

        public void ReadAsync<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _readerAsync.ReadAsync<T>(collectionNames, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region Distinct T
        public void DistinctAsync<T>(string collectionName, string fieldName)
        {
            _readerAsync.DistinctAsync<T>(collectionName, fieldName);
        }
        public void DistinctAsync<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            _readerAsync.DistinctAsync<T>(collectionName, fieldName, query);
        }
        public void DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            _readerAsync.DistinctAsync<T>(collectionNames, fieldName);
        }
        public void DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            _readerAsync.DistinctAsync<T>(collectionNames, fieldName, query);
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

        void _mongoReaderAsync_AsyncDistinctCompleted(object e, Exception ex)
        {
            if (AsyncDistinctCompleted != null)
                AsyncDistinctCompleted(e, ex);
        }
    }

    public class DatabaseReader<T> : IDatabaseReader<T> where T : class
    {
        public event ReadCompletedEvent AsyncReadCompleted
        {
            add
            {
                lock (_databaseReader)
                {
                    _databaseReader.AsyncReadCompleted += value;
                }
            }
            remove
            {
                lock (_databaseReader)
                {
                    _databaseReader.AsyncReadCompleted -= value;
                }
            }
        }

        public event DistinctCompletedEvent AsyncDistinctCompleted
        {
            add
            {
                lock (_databaseReader)
                {
                    _databaseReader.AsyncDistinctCompleted += value;
                }
            }
            remove
            {
                lock (_databaseReader)
                {
                    _databaseReader.AsyncDistinctCompleted -= value;
                }
            }
        }

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
        public void ReadAsync(string collectionName, string fieldName, string regexPattern)
        {
            _databaseReader.ReadAsync<T>(collectionName, fieldName, regexPattern);
        }

        public void ReadAsync(string collectionName, string fieldName, DateTime start, DateTime end)
        {
            _databaseReader.ReadAsync<T>(collectionName, fieldName, start, end);
        }

        public void ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _databaseReader.ReadAsync<T>(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            _databaseReader.ReadAsync<T>(collectionNames, fieldName, regexPattern);
        }

        public void ReadAsync(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end)
        {
            _databaseReader.ReadAsync<T>( collectionNames, fieldName, start, end);
        }

        public void ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _databaseReader.ReadAsync<T>( collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end);
        }
        #endregion Read
        #region    Distinct
        public void DistinctAsync<Y>(string collectionName, string fieldName)
        {
            _databaseReader.DistinctAsync<Y>(collectionName, fieldName);
        }

        public void DistinctAsync<Y>(string collectionName, string fieldName, IMongoQuery query)
        {
            _databaseReader.DistinctAsync<Y>(collectionName, fieldName, query);
        }

        public void DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName)
        {
            _databaseReader.DistinctAsync<Y>(collectionNames, fieldName);
        }

        public void DistinctAsync<Y>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            _databaseReader.DistinctAsync<Y>(collectionNames, fieldName, query);
        }
        #endregion Distinct
        #endregion Asynchronous
    }
}
