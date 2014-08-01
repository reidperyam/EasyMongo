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
    public class DatabaseUpdater : IDatabaseUpdater
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        protected IUpdater _updater;

        protected IUpdaterAsync _updaterAsync;

        public DatabaseUpdater(IUpdater      updater,
                               IUpdaterAsync updaterAsync)
        {
            _updater = updater;
            _updaterAsync = updaterAsync;

            _updaterAsync.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_mongoUpdaterAsync_AsyncFindAndModifyCompleted);
            _updaterAsync.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_mongoUpdaterAsync_AsyncFindAndRemoveCompleted);
        }

        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query)
        {
            return _updater.Remove<T>(collectionName, query);
        }
        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return _updater.Remove<T>(collectionName, query, removeFlags);
        }
        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return _updater.Remove<T>(collectionName, query, writeConcern);
        }
        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _updater.Remove<T>(collectionName, query, removeFlags, writeConcern);
        }
        public void RemoveAsync<T>(string collectionName, IMongoQuery query)
        {
            _updaterAsync.RemoveAsync<T>(collectionName, query);
        }
        public void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            _updaterAsync.RemoveAsync<T>(collectionName, query, removeFlags);
        }
        public void RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            _updaterAsync.RemoveAsync<T>(collectionName, query, writeConcern);
        }
        public void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _updaterAsync.RemoveAsync<T>(collectionName, query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify<T>(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return _updater.FindAndModify<T>(collectionName, findAndModifyArgs);
        }
        public FindAndModifyResult FindAndRemove<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return _updater.FindAndRemove<T>(collectionName, findAndRemoveArgs);
        }

        public void FindAndModifyAsync<T>(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            _updaterAsync.FindAndModifyAsync<T>(collectionName, findAndModifyArgs);
        }
        public void FindAndRemoveAsync<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            _updaterAsync.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs);
        }

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

    public class DatabaseUpdater<T> : IDatabaseUpdater<T>
    {
        IDatabaseUpdater _databaseUpdater;

        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted
        {
            add
            {
                lock (_databaseUpdater)
                {
                    _databaseUpdater.AsyncFindAndModifyCompleted += value;
                }
            }
            remove
            {
                lock (_databaseUpdater)
                {
                    _databaseUpdater.AsyncFindAndModifyCompleted -= value;
                }
            }
        }
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted
        {
            add
            {
                lock (_databaseUpdater)
                {
                    _databaseUpdater.AsyncFindAndRemoveCompleted += value;
                }
            }
            remove
            {
                lock (_databaseUpdater)
                {
                    _databaseUpdater.AsyncFindAndRemoveCompleted -= value;
                }
            }
        }

        public DatabaseUpdater(IDatabaseUpdater databaseUpdater)
        {
            _databaseUpdater = databaseUpdater;
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query)
        {
            return _databaseUpdater.Remove<T>(collectionName, query);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return _databaseUpdater.Remove<T>(collectionName, query, writeConcern);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return _databaseUpdater.Remove<T>(collectionName, query, removeFlags);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _databaseUpdater.Remove<T>(collectionName, query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return _databaseUpdater.FindAndModify<T>(collectionName, findAndModifyArgs);
        }

        public FindAndModifyResult FindAndRemove(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return _databaseUpdater.FindAndRemove<T>(collectionName, findAndRemoveArgs);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query)
        {
            _databaseUpdater.RemoveAsync<T>(collectionName, query);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            _databaseUpdater.RemoveAsync<T>(collectionName, query, writeConcern);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            _databaseUpdater.RemoveAsync<T>(collectionName, query, removeFlags);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _databaseUpdater.RemoveAsync<T>(collectionName, query, removeFlags, writeConcern);
        }

        public void FindAndModifyAsync(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            _databaseUpdater.FindAndModifyAsync<T>(collectionName, findAndModifyArgs);
        }

        public void FindAndRemoveAsync(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            _databaseUpdater.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs);
        }
    }
}
