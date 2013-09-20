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
        private string _collectionName;
        private IDatabaseReader<T> _mongoDBReader;

        public Reader(IDatabaseReader<T> mongoDatabaseReader, string collectionName)
        {
            _mongoDBReader  = mongoDatabaseReader;
            _collectionName = collectionName;
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
    }
}
