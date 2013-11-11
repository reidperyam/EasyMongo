using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Collection
{
    public class CollectionReader: ICollectionReader
    {
        public event ReadCompletedEvent     AsyncReadCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;

        private string _collectionName;
        private IDatabaseReader _mongoDBReader;

        public CollectionReader(IDatabaseReader mongoDatabaseReader, string collectionName)
        {
            _mongoDBReader  = mongoDatabaseReader;
            _collectionName = collectionName;

            _mongoDBReader.AsyncReadCompleted     += new ReadCompletedEvent(_mongoDBReader_AsyncReadCompleted);
            _mongoDBReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_mongoDBReader_AsyncDistinctCompleted);
        }

        #region   Synchronous
        public IEnumerable<T> Read<T>(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoDBReader.Read<T>(_collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(string dateTimeFieldName, DateTime start, DateTime end)
        {
            return _mongoDBReader.Read<T>(_collectionName, dateTimeFieldName, start, end);
        }

        public IEnumerable<T> Read<T>(string fieldName, string regexPattern)
        {
            return _mongoDBReader.Read<T>(_collectionName, fieldName, regexPattern);
        }
        #region Distinct T
        public IEnumerable<T> Distinct<T>(string fieldName)
        {
            return _mongoDBReader.Distinct<T>(_collectionName, fieldName);
        }
        public IEnumerable<T> Distinct<T>(string fieldName, IMongoQuery query)
        {
            return _mongoDBReader.Distinct<T>(_collectionName, fieldName, query);
        }
        #endregion Distinct T
        #endregion Synchronous

        #region    Asynchronous
        public void ReadAsync<T>(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoDBReader.ReadAsync<T>(_collectionName, fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public void ReadAsync<T>(string dateTimeFieldName, DateTime start, DateTime end)
        {
            _mongoDBReader.ReadAsync<T>(_collectionName, dateTimeFieldName, start, end);
        }

        public void ReadAsync<T>(string fieldName, string regexPattern)
        {
            _mongoDBReader.ReadAsync<T>(_collectionName, fieldName, regexPattern);
        }
        #region Distinct T
        public void DistinctAsync<T>(string fieldName)
        {
            _mongoDBReader.DistinctAsync<T>(_collectionName, fieldName);
        }
        public void DistinctAsync<T>(string fieldName, IMongoQuery query)
        {
            _mongoDBReader.DistinctAsync<T>(_collectionName, fieldName, query);
        }
        #endregion Distinct T
        #endregion Asynchronous


        void _mongoDBReader_AsyncReadCompleted(object e, Exception ex)
        {
            if (AsyncReadCompleted != null)
                AsyncReadCompleted(e, ex);
        }

        void _mongoDBReader_AsyncDistinctCompleted(object e, Exception ex)
        {
            if (AsyncDistinctCompleted != null)
                AsyncDistinctCompleted(e, ex);
        }
    }

    public class CollectionReader<T> : ICollectionReader<T>
    {
        public event ReadCompletedEvent AsyncReadCompleted
        {
            add
            {
                lock (_collectionReader)
                {
                    _collectionReader.AsyncReadCompleted += value;
                }
            }
            remove
            {
                lock (_collectionReader)
                {
                    _collectionReader.AsyncReadCompleted -= value;
                }
            }
        }

        public event DistinctCompletedEvent AsyncDistinctCompleted
        {
            add
            {
                lock (_collectionReader)
                {
                    _collectionReader.AsyncDistinctCompleted += value;
                }
            }
            remove
            {
                lock (_collectionReader)
                {
                    _collectionReader.AsyncDistinctCompleted -= value;
                }
            }
        }

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

        public void ReadAsync(string fieldName, string regexPattern)
        {
            _collectionReader.ReadAsync<T>(fieldName, regexPattern);
        }

        public void ReadAsync(string dateTimeFieldName, DateTime start, DateTime end)
        {
            _collectionReader.ReadAsync<T>(dateTimeFieldName, start, end);
        }

        public void ReadAsync(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            _collectionReader.ReadAsync<T>(fieldName, regexPattern, dateTimeFieldName, start, end);
        }

        public IEnumerable<Y> Distinct<Y>(string fieldName)
        {
            return _collectionReader.Distinct<Y>(fieldName);
        }

        public IEnumerable<Y> Distinct<Y>(string fieldName, IMongoQuery query)
        {
            return _collectionReader.Distinct<Y>(fieldName, query);
        }

        public void DistinctAsync<Y>(string fieldName)
        {
            _collectionReader.DistinctAsync<Y>(fieldName);
        }

        public void DistinctAsync<Y>(string fieldName, IMongoQuery query)
        {
            _collectionReader.DistinctAsync<Y>(fieldName, query);
        }
    }
}
