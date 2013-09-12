using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo.Database;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Collection
{
    public class Updater<T> : ICollectionUpdater<T> where T : IEasyMongoEntry
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        IDatabaseUpdater<T> _mongoDBUpdater;
        string _collectionName; 

        public Updater(IDatabaseUpdater<T> mongoDatabaseUpdater, string collectionName)
        {
            _mongoDBUpdater = mongoDatabaseUpdater;
            _collectionName = collectionName;

            // hook class events to base class' members'
            _mongoDBUpdater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_mongoUpdaterAsync_AsyncFindAndModifyCompleted);
            _mongoDBUpdater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_mongoUpdaterAsync_AsyncFindAndRemoveCompleted);
        }

        public void Remove(IMongoQuery query)
        {
            _mongoDBUpdater.Remove(_collectionName, query);
        }
        public void Remove(IMongoQuery query, RemoveFlags removeFlags)
        {
            _mongoDBUpdater.Remove(_collectionName, query, removeFlags);
        }
        public void Remove(IMongoQuery query, WriteConcern writeConcern)
        {
            _mongoDBUpdater.Remove(_collectionName, query, writeConcern);
        }
        public void Remove(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _mongoDBUpdater.Remove(_collectionName, query, removeFlags, writeConcern);
        }

        public void RemoveAsync(IMongoQuery query)
        {
            _mongoDBUpdater.RemoveAsync(_collectionName, query);
        }
        public void RemoveAsync(IMongoQuery query, RemoveFlags removeFlags)
        {
            _mongoDBUpdater.RemoveAsync(_collectionName, query, removeFlags);
        }
        public void RemoveAsync(IMongoQuery query, WriteConcern writeConcern)
        {
            _mongoDBUpdater.RemoveAsync(_collectionName, query, writeConcern);
        }
        public void RemoveAsync(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _mongoDBUpdater.RemoveAsync(_collectionName, query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            return _mongoDBUpdater.FindAndModify(_collectionName, mongoQuery, mongoSortBy, mongoUpdate);
        }
        public FindAndModifyResult FindAndModify(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            return _mongoDBUpdater.FindAndModify(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }
        public FindAndModifyResult FindAndModify(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            return _mongoDBUpdater.FindAndModify(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }
        public FindAndModifyResult FindAndModify(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            return _mongoDBUpdater.FindAndModify(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }
        public FindAndModifyResult FindAndRemove(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            return _mongoDBUpdater.FindAndRemove(_collectionName, mongoQuery, mongoSortBy);
        }

        public void FindAndModifyAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            _mongoDBUpdater.FindAndModifyAsync(_collectionName,mongoQuery, mongoSortBy, mongoUpdate);
        }
        public void FindAndModifyAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            _mongoDBUpdater.FindAndModifyAsync(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }
        public void FindAndModifyAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            _mongoDBUpdater.FindAndModifyAsync(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }
        public void FindAndModifyAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            _mongoDBUpdater.FindAndModifyAsync(_collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }
        public void FindAndRemoveAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            _mongoDBUpdater.FindAndRemoveAsync(_collectionName, mongoQuery, mongoSortBy);
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
