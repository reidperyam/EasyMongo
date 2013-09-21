using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo.Database;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Collection
{
    public class Updater : ICollectionUpdater
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        IDatabaseUpdater _mongoDBUpdater;
        string _collectionName; 

        public Updater(IDatabaseUpdater mongoDatabaseUpdater, string collectionName)
        {
            _mongoDBUpdater = mongoDatabaseUpdater;
            _collectionName = collectionName;

            _mongoDBUpdater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_mongoUpdaterAsync_AsyncFindAndModifyCompleted);
            _mongoDBUpdater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_mongoUpdaterAsync_AsyncFindAndRemoveCompleted);
        }

        #region    Synchronous
        public void Remove<T>(IMongoQuery query)
        {
            _mongoDBUpdater.Remove<T>(_collectionName, query);
        }
        public void Remove<T>(IMongoQuery query, RemoveFlags removeFlags)
        {
            _mongoDBUpdater.Remove<T>(_collectionName, query, removeFlags);
        }
        public void Remove<T>(IMongoQuery query, WriteConcern writeConcern)
        {
            _mongoDBUpdater.Remove<T>(_collectionName, query, writeConcern);
        }
        public void Remove<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _mongoDBUpdater.Remove<T>(_collectionName, query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            return _mongoDBUpdater.FindAndModify<T>(_collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }
        public FindAndModifyResult FindAndModify<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            return _mongoDBUpdater.FindAndModify<T>(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }
        public FindAndModifyResult FindAndModify<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            return _mongoDBUpdater.FindAndModify<T>(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }
        public FindAndModifyResult FindAndModify<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            return _mongoDBUpdater.FindAndModify<T>(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }
        public FindAndModifyResult FindAndRemove<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            return _mongoDBUpdater.FindAndRemove<T>(_collectionName, mongoQuery, mongoSortBy);
        }
        #endregion Synchronous

        #region    Asynchronous
        public void RemoveAsync<T>(IMongoQuery query)
        {
            _mongoDBUpdater.RemoveAsync<T>(_collectionName, query);
        }
        public void RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags)
        {
            _mongoDBUpdater.RemoveAsync<T>(_collectionName, query, removeFlags);
        }
        public void RemoveAsync<T>(IMongoQuery query, WriteConcern writeConcern)
        {
            _mongoDBUpdater.RemoveAsync<T>(_collectionName, query, writeConcern);
        }
        public void RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _mongoDBUpdater.RemoveAsync<T>(_collectionName, query, removeFlags, writeConcern);
        }

        public void FindAndModifyAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            _mongoDBUpdater.FindAndModifyAsync<T>(_collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }
        public void FindAndModifyAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            _mongoDBUpdater.FindAndModifyAsync<T>(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }
        public void FindAndModifyAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            _mongoDBUpdater.FindAndModifyAsync<T>(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }
        public void FindAndModifyAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            _mongoDBUpdater.FindAndModifyAsync<T>(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }
        public void FindAndRemoveAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            _mongoDBUpdater.FindAndRemoveAsync<T>(_collectionName, mongoQuery, mongoSortBy);
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
}
