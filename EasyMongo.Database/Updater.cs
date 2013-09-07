using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Async;

namespace EasyMongo.Database
{
    public class Updater<T> : Adapter<T>, IDatabaseUpdater<T> where T : EntryBase
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        public Updater(string connectionString, string databaseName)
            : base(new ServerConnection(connectionString), databaseName)
        {
            // hook class events to base class' members'
            _mongoUpdaterAsync.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_mongoUpdaterAsync_AsyncFindAndModifyCompleted);
            _mongoUpdaterAsync.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_mongoUpdaterAsync_AsyncFindAndRemoveCompleted);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query)
        {
            return _mongoUpdater.Remove(collectionName, query);
        }
        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return _mongoUpdater.Remove(collectionName, query, removeFlags);
        }
        public WriteConcernResult Remove(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return _mongoUpdater.Remove(collectionName, query, writeConcern);
        }
        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _mongoUpdater.Remove(collectionName, query, removeFlags, writeConcern);
        }
        public void RemoveAsync(string collectionName, IMongoQuery query)
        {
            _mongoUpdaterAsync.RemoveAsync(collectionName, query);
        }
        public void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            _mongoUpdaterAsync.RemoveAsync(collectionName, query, removeFlags);
        }
        public void RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            _mongoUpdaterAsync.RemoveAsync(collectionName, query, writeConcern);
        }
        public void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _mongoUpdaterAsync.RemoveAsync(collectionName, query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            return _mongoUpdater.FindAndModify(collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }
        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            return _mongoUpdater.FindAndModify(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }
        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            return _mongoUpdater.FindAndModify(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }
        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            return _mongoUpdater.FindAndModify(collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }
        public FindAndModifyResult FindAndRemove(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            return _mongoUpdater.FindAndRemove(collectionName, mongoQuery, mongoSortBy);
        }

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            _mongoUpdaterAsync.FindAndModifyAsync(collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }
        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            _mongoUpdaterAsync.FindAndModifyAsync(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }
        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            _mongoUpdaterAsync.FindAndModifyAsync(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }
        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            _mongoUpdaterAsync.FindAndModifyAsync(collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }
        public void FindAndRemoveAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            _mongoUpdaterAsync.FindAndRemoveAsync(collectionName, mongoQuery, mongoSortBy);
        }

        public IUpdater<T> Create(IDatabaseConnection<T> databaseConnection)
        {
            return new EasyMongo.Updater<T>(databaseConnection);
        }

        public IUpdaterAsync<T> Create(IUpdater<T> updater)
        {
            return new UpdaterAsync<T>(updater);
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
}
