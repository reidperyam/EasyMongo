using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Database;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Collection;
using MongoDB.Contract;

namespace MongoDB.Collection
{
    public class Reader<T> : ICollectionReader<T> where T : EntryBase
    {
        public event ReadCompletedEvent<T> AsyncReadCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;
        private string _collectionName;
        private IDatabaseReader<T> _mongoDBReader;

        public Reader(IDatabaseReader<T> mongoDatabaseReader, string collectionName)
        {
            _mongoDBReader  = mongoDatabaseReader;
            _collectionName = collectionName;
            _mongoDBReader.AsyncReadCompleted += new ReadCompletedEvent<T>(_mongoDBReader_AsyncReadCompleted);
            _mongoDBReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_mongoDBReader_AsyncDistinctCompleted);
        }

        #region   Synchronous
        public IEnumerable<T> Read(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoDBReader.Read(_collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoDBReader.Read(_collectionName, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(string fieldName, string regexPattern)
        {
            return _mongoDBReader.Read(_collectionName, fieldName, regexPattern);
        }
        public IEnumerable<BsonValue> Distinct(string fieldName)
        {
            return _mongoDBReader.Distinct(_collectionName, fieldName);
        }
        public IEnumerable<BsonValue> Distinct(string fieldName, IMongoQuery query)
        {
            return _mongoDBReader.Distinct(_collectionName, fieldName, query);
        }

        #endregion Synchronous

        #region    Asynchronous
        public void ReadAsync(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoDBReader.ReadAsync(_collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync(string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoDBReader.ReadAsync(_collectionName, dateTimeFieldName, start, end);
        }

        public void ReadAsync(string fieldName, string regexPattern)
        {
            _mongoDBReader.ReadAsync(_collectionName, fieldName, regexPattern);
        }
        public void DistinctAsync(string fieldName)
        {
            _mongoDBReader.DistinctAsync(_collectionName, fieldName);
        }
        public void DistinctAsync(string fieldName, IMongoQuery query)
        {
            _mongoDBReader.DistinctAsync(_collectionName, fieldName, query);
        }
        #endregion Asynchronous

        void _mongoDBReader_AsyncReadCompleted(IEnumerable<T> e, Exception ex)
        {
            if (AsyncReadCompleted != null)
                AsyncReadCompleted(e, ex);
        }

        void _mongoDBReader_AsyncDistinctCompleted( IEnumerable<BsonValue> e, Exception ex)
        {
            if (AsyncDistinctCompleted != null)
                AsyncDistinctCompleted(e, ex);
        }
    }
}
