using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

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
        public IEnumerable<T> Read<T>()
        {
            return _databaseReader.Read<T>(_collectionName);
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
        #region    Execute
        public IEnumerable<T> Execute<T>(IMongoQuery mongoQuery)
        {
            return _databaseReader.Execute<T>(_collectionName, mongoQuery);
        }
        public IEnumerable<T> ExecuteAnds<T>(IEnumerable<IMongoQuery> mongoQueries)
        {
            return _databaseReader.ExecuteAnds<T>(_collectionName, mongoQueries);
        }
        public IEnumerable<T> ExecuteOrs<T>(IEnumerable<IMongoQuery> mongoQueries)
        {
            return _databaseReader.ExecuteOrs<T>(_collectionName, mongoQueries);
        }
        #endregion Execute
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
        public Task<IEnumerable<T>> ReadAsync<T>()
        {
            return _databaseReader.ReadAsync<T>(_collectionName);
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
        #region    Execute
        public Task<IEnumerable<T>> ExecuteAsync<T>(IMongoQuery mongoQuery)
        {
            return _databaseReader.ExecuteAsync<T>(_collectionName, mongoQuery);
        }
        public Task<IEnumerable<T>> ExecuteAndsAsync<T>(IEnumerable<IMongoQuery> mongoQueries)
        {
            return _databaseReader.ExecuteAndsAsync<T>(_collectionName, mongoQueries);
        }
        public Task<IEnumerable<T>> ExecuteOrsAsync<T>(IEnumerable<IMongoQuery> mongoQueries)
        {
            return _databaseReader.ExecuteOrsAsync<T>(_collectionName, mongoQueries);
        }
        #endregion Execute
        #endregion Asynchronous
    }

    public class CollectionReader<T> : ICollectionReader<T>
    {
        private ICollectionReader _collectionReader;

        public CollectionReader(ICollectionReader collectionReader)
        {
            _collectionReader = collectionReader;
        }

        public IEnumerable<T> Read()
        {
            return _collectionReader.Read<T>();
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

        public Task<IEnumerable<T>> ReadAsync()
        {
            return _collectionReader.ReadAsync<T>();
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

        public IEnumerable<T> Execute(IMongoQuery mongoQuery)
        {
            return _collectionReader.Execute<T>(mongoQuery);
        }

        public Task<IEnumerable<T>> ExecuteAsync(IMongoQuery mongoQuery)
        {
            return _collectionReader.ExecuteAsync<T>(mongoQuery);
        }

        public IEnumerable<T> ExecuteAnds(IEnumerable<IMongoQuery> mongoQueries)
        {
            return _collectionReader.ExecuteAnds<T>(mongoQueries);
        }

        public Task<IEnumerable<T>> ExecuteAndsAsync(IEnumerable<IMongoQuery> mongoQueries)
        {
            return _collectionReader.ExecuteAndsAsync<T>(mongoQueries);
        }

        public IEnumerable<T> ExecuteOrs(IEnumerable<IMongoQuery> mongoQueries)
        {
            return _collectionReader.ExecuteOrs<T>(mongoQueries);
        }

        public Task<IEnumerable<T>> ExecuteOrsAsync(IEnumerable<IMongoQuery> mongoQueries)
        {
            return _collectionReader.ExecuteOrsAsync<T>(mongoQueries);
        }
    }
}
