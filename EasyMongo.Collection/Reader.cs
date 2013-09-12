using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Collection;
using EasyMongo.Database;

namespace EasyMongo.Collection
{
    public class Reader<T> : ICollectionReader<T> where T : IEasyMongoEntry
    {
        public event ReadCompletedEvent<T> AsyncReadCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;
        private string _collectionName;
        private IDatabaseReader<T> _mongoDBReader;

        public Reader(IDatabaseReader<T> mongoDatabaseReader, string collectionName)
        {
            _mongoDBReader  = mongoDatabaseReader;
            _collectionName = collectionName;

            // TODO - check to see if the existing event is null before adding new handler

            // why on earth is this downstream class subscribing to upstream events???
            // EVENTUALLY probably need to remove this handler and the methods...

           // _mongoDBReader.AsyncReadCompleted += new ReadCompletedEvent<T>(_mongoDBReader_AsyncReadCompleted);
           // _mongoDBReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_mongoDBReader_AsyncDistinctCompleted);
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
