using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo.Contract;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Collection
{
    public class CollectionUpdater : ICollectionUpdater
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        IDatabaseUpdater _databaseUpdater;
        string _collectionName; 

        public CollectionUpdater(IDatabaseUpdater databaseUpdater, string collectionName)
        {
            _databaseUpdater = databaseUpdater;
            _collectionName = collectionName;

            _databaseUpdater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_mongoUpdaterAsync_AsyncFindAndModifyCompleted);
            _databaseUpdater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_mongoUpdaterAsync_AsyncFindAndRemoveCompleted);
        }

        #region    Synchronous
        public WriteConcernResult Remove<T>(IMongoQuery query)
        {
            return _databaseUpdater.Remove<T>(_collectionName, query);
        }
        public WriteConcernResult Remove<T>(IMongoQuery query, RemoveFlags removeFlags)
        {
            return _databaseUpdater.Remove<T>(_collectionName, query, removeFlags);
        }
        public WriteConcernResult Remove<T>(IMongoQuery query, WriteConcern writeConcern)
        {
            return _databaseUpdater.Remove<T>(_collectionName, query, writeConcern);
        }
        public WriteConcernResult Remove<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _databaseUpdater.Remove<T>(_collectionName, query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify<T>(FindAndModifyArgs findAndModifyArgs)
        {
            return _databaseUpdater.FindAndModify<T>(_collectionName, findAndModifyArgs);
        }

        public FindAndModifyResult FindAndRemove<T>(FindAndRemoveArgs findAndRemoveArgs)
        {
            return _databaseUpdater.FindAndRemove<T>(_collectionName, findAndRemoveArgs);
        }
        #endregion Synchronous

        #region    Asynchronous
        public void RemoveAsync<T>(IMongoQuery query)
        {
            _databaseUpdater.RemoveAsync<T>(_collectionName, query);
        }
        public void RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags)
        {
            _databaseUpdater.RemoveAsync<T>(_collectionName, query, removeFlags);
        }
        public void RemoveAsync<T>(IMongoQuery query, WriteConcern writeConcern)
        {
            _databaseUpdater.RemoveAsync<T>(_collectionName, query, writeConcern);
        }
        public void RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _databaseUpdater.RemoveAsync<T>(_collectionName, query, removeFlags, writeConcern);
        }

        public void FindAndModifyAsync<T>(FindAndModifyArgs findAndModifyArgs)
        {
            _databaseUpdater.FindAndModifyAsync<T>(_collectionName, findAndModifyArgs);
        }
        public void FindAndRemoveAsync<T>(FindAndRemoveArgs findAndRemoveArgs)
        {
            _databaseUpdater.FindAndRemoveAsync<T>(_collectionName, findAndRemoveArgs);
        }
        #endregion Asynchronous

        void _mongoUpdaterAsync_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            if (AsyncFindAndRemoveCompleted != null)
                AsyncFindAndRemoveCompleted(result);
        }

        void _mongoUpdaterAsync_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            if (AsyncFindAndModifyCompleted != null)
                AsyncFindAndModifyCompleted(result);
        }
    }

    public class CollectionUpdater<T> : ICollectionUpdater<T>
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted
        {
            add
            {
                lock (_collectionUpdater)
                {
                    _collectionUpdater.AsyncFindAndModifyCompleted += value;
                }
            }
            remove
            {
                lock (_collectionUpdater)
                {
                    _collectionUpdater.AsyncFindAndModifyCompleted -= value;
                }
            }
        }

        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted
        {
            add
            {
                lock (_collectionUpdater)
                {
                    _collectionUpdater.AsyncFindAndRemoveCompleted += value;
                }
            }
            remove
            {
                lock (_collectionUpdater)
                {
                    _collectionUpdater.AsyncFindAndRemoveCompleted -= value;
                }
            }
        }

        private ICollectionUpdater _collectionUpdater;

        public CollectionUpdater(ICollectionUpdater collectionUpdater)
        {
            _collectionUpdater = collectionUpdater;
        }

        public WriteConcernResult Remove(IMongoQuery query)
        {
            return _collectionUpdater.Remove<T>(query);
        }

        public WriteConcernResult Remove(IMongoQuery query, WriteConcern writeConcern)
        {
            return _collectionUpdater.Remove<T>(query, writeConcern);
        }

        public WriteConcernResult Remove(IMongoQuery query, RemoveFlags removeFlags)
        {
            return _collectionUpdater.Remove<T>(query, removeFlags);
        }

        public WriteConcernResult Remove(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _collectionUpdater.Remove<T>(query, removeFlags, writeConcern);
        }

        public void RemoveAsync(IMongoQuery query)
        {
            _collectionUpdater.RemoveAsync<T>(query);
        }

        public void RemoveAsync(IMongoQuery query, WriteConcern writeConcern)
        {
            _collectionUpdater.RemoveAsync<T>(query, writeConcern);
        }

        public void RemoveAsync(IMongoQuery query, RemoveFlags removeFlags)
        {
            _collectionUpdater.RemoveAsync<T>(query, removeFlags);
        }

        public void RemoveAsync(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _collectionUpdater.RemoveAsync<T>(query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify(FindAndModifyArgs findAndModifyArgs)
        {
            return _collectionUpdater.FindAndModify<T>(findAndModifyArgs);
        }

        public FindAndModifyResult FindAndRemove(FindAndRemoveArgs findAndRemoveArgs)
        {
            return _collectionUpdater.FindAndRemove<T>(findAndRemoveArgs);
        }

        public void FindAndModifyAsync(FindAndModifyArgs findAndModifyArgs)
        {
            _collectionUpdater.FindAndModifyAsync<T>(findAndModifyArgs);
        }

        public void FindAndRemoveAsync(FindAndRemoveArgs findAndRemoveArgs)
        {
            _collectionUpdater.FindAndRemoveAsync<T>(findAndRemoveArgs);
        }
    }
}
