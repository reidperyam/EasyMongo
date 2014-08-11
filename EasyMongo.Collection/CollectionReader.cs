using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Contract.Delegates;

namespace EasyMongo.Collection
{
    public class CollectionReader: ICollectionReader
    {
        private string _collectionName;
        private IDatabaseReader _databaseReader;

        public CollectionReader(IDatabaseReader databaseReader, string collectionName)
        {
            _databaseReader  = databaseReader;
            _collectionName = collectionName;
        }

        #region   Synchronous
        public IEnumerable<T> Read<T>(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.Read<T>(_collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.Read<T>(_collectionName, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(string fieldName, string regexPattern)
        {
            return _databaseReader.Read<T>(_collectionName, fieldName, regexPattern);
        }
        #region Distinct T
        public IEnumerable<T> Distinct<T>(string fieldName)
        {
            return _databaseReader.Distinct<T>(_collectionName, fieldName);
        }
        public IEnumerable<T> Distinct<T>(string fieldName, IMongoQuery query)
        {
            return _databaseReader.Distinct<T>(_collectionName, fieldName, query);
        }
        #endregion Distinct T
        #endregion Synchronous

        #region    Asynchronous
        public Task<IEnumerable<T>> ReadAsync<T>(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.ReadAsync<T>(_collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _databaseReader.ReadAsync<T>(_collectionName, dateTimeFieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync<T>(string fieldName, string regexPattern)
        {
            return _databaseReader.ReadAsync<T>(_collectionName, fieldName, regexPattern);
        }
        #region Distinct T
        public Task<IEnumerable<T>> DistinctAsync<T>(string fieldName)
        {
            return _databaseReader.DistinctAsync<T>(_collectionName, fieldName);
        }
        public Task<IEnumerable<T>> DistinctAsync<T>(string fieldName, IMongoQuery query)
        {
            return _databaseReader.DistinctAsync<T>(_collectionName, fieldName, query);
        }
        #endregion Distinct T
        #endregion Asynchronous
    }

    public class CollectionReader<T> : ICollectionReader<T>
    {
        private ICollectionReader _collectionReader;

        public CollectionReader(ICollectionReader collectionReader)
        {
            _collectionReader = collectionReader;
        }

        public IEnumerable<T> Read(string fieldName, string regexPattern)
        {
            return _collectionReader.Read<T>(fieldName, regexPattern);
        }

        public IEnumerable<T> Read(string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _collectionReader.Read<T>(dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _collectionReader.Read<T>(fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync(string fieldName, string regexPattern)
        {
            return _collectionReader.ReadAsync<T>(fieldName, regexPattern);
        }

        public Task<IEnumerable<T>> ReadAsync(string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _collectionReader.ReadAsync<T>(dateTimeFieldName, start, end);
        }

        public Task<IEnumerable<T>> ReadAsync(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _collectionReader.ReadAsync<T>(fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<Y> Distinct<Y>(string fieldName)
        {
            return _collectionReader.Distinct<Y>(fieldName);
        }

        public IEnumerable<Y> Distinct<Y>(string fieldName, IMongoQuery query)
        {
            return _collectionReader.Distinct<Y>(fieldName, query);
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string fieldName)
        {
            return _collectionReader.DistinctAsync<Y>(fieldName);
        }

        public Task<IEnumerable<Y>> DistinctAsync<Y>(string fieldName, IMongoQuery query)
        {
            return _collectionReader.DistinctAsync<Y>(fieldName, query);
        }
    }
}
