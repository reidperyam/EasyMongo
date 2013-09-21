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
    public class Updater : Adapter, IDatabaseUpdater
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        public Updater(string        connectionString, 
                       string        databaseName,
                       IReader       reader, 
                       IWriter       writer, 
                       IUpdater      updater,
                       IReaderAsync  readerAsync, 
                       IWriterAsync  writerAsync,
                       IUpdaterAsync updaterAsync)
            : base(reader,
                   writer,
                   updater,
                   readerAsync,
                   writerAsync,
                   updaterAsync)
        {
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
        public FindAndModifyResult FindAndRemove<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            return _mongoUpdater.FindAndRemove<T>(collectionName, mongoQuery, mongoSortBy);
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
        public void FindAndRemoveAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            _mongoUpdaterAsync.FindAndRemoveAsync<T>(collectionName, mongoQuery, mongoSortBy);
        }

        public IUpdater Create(IDatabaseConnection databaseConnection)
        {
            return new EasyMongo.Updater(databaseConnection);
        }

        public IUpdaterAsync Create(IUpdater updater)
        {
            return new UpdaterAsync(updater);
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
