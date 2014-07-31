using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Database
{
    public class DatabaseUpdater : IDatabaseUpdater
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        protected IUpdater _mongoUpdater;

        protected IUpdaterAsync _mongoUpdaterAsync;

        public DatabaseUpdater(IUpdater      updater,
                               IUpdaterAsync updaterAsync)
        {
            _mongoUpdater = updater;
            _mongoUpdaterAsync = updaterAsync;

            _mongoUpdaterAsync.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_mongoUpdaterAsync_AsyncFindAndModifyCompleted);
            _mongoUpdaterAsync.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_mongoUpdaterAsync_AsyncFindAndRemoveCompleted);
        }

        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query)
        {
            return _mongoUpdater.Remove<T>(collectionName, query);
        }
        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return _mongoUpdater.Remove<T>(collectionName, query, removeFlags);
        }
        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return _mongoUpdater.Remove<T>(collectionName, query, writeConcern);
        }
        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _mongoUpdater.Remove<T>(collectionName, query, removeFlags, writeConcern);
        }
        public void RemoveAsync<T>(string collectionName, IMongoQuery query)
        {
            _mongoUpdaterAsync.RemoveAsync<T>(collectionName, query);
        }
        public void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            _mongoUpdaterAsync.RemoveAsync<T>(collectionName, query, removeFlags);
        }
        public void RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            _mongoUpdaterAsync.RemoveAsync<T>(collectionName, query, writeConcern);
        }
        public void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _mongoUpdaterAsync.RemoveAsync<T>(collectionName, query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            return _mongoUpdater.FindAndModify<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }
        public FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            return _mongoUpdater.FindAndModify<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }
        public FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            return _mongoUpdater.FindAndModify<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }
        public FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            return _mongoUpdater.FindAndModify<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }
        public FindAndModifyResult FindAndRemove<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return _mongoUpdater.FindAndRemove<T>(collectionName, findAndRemoveArgs);
        }

        public void FindAndModifyAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            _mongoUpdaterAsync.FindAndModifyAsync<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }
        public void FindAndModifyAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            _mongoUpdaterAsync.FindAndModifyAsync<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }
        public void FindAndModifyAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            _mongoUpdaterAsync.FindAndModifyAsync<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }
        public void FindAndModifyAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            _mongoUpdaterAsync.FindAndModifyAsync<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }
        public void FindAndRemoveAsync<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            _mongoUpdaterAsync.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs);
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

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            return _databaseUpdater.FindAndModify<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            return _databaseUpdater.FindAndModify<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            return _databaseUpdater.FindAndModify<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            return _databaseUpdater.FindAndModify<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
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

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            _databaseUpdater.FindAndModifyAsync<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            _databaseUpdater.FindAndModifyAsync<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            _databaseUpdater.FindAndModifyAsync<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            _databaseUpdater.FindAndModifyAsync<T>(collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }

        public void FindAndRemoveAsync(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            _databaseUpdater.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs);
        }
    }
}
