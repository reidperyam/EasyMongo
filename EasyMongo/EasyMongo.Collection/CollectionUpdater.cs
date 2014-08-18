using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyMongo.Contract;
using MongoDB.Driver;

namespace EasyMongo.Collection
{
    public class CollectionUpdater : ICollectionUpdater
    {
        IDatabaseUpdater _databaseUpdater;
        string _collectionName; 

        public CollectionUpdater(IDatabaseUpdater databaseUpdater, string collectionName)
        {
            _databaseUpdater = databaseUpdater;
            _collectionName = collectionName;
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
        public Task<WriteConcernResult> RemoveAsync<T>(IMongoQuery query)
        {
            return _databaseUpdater.RemoveAsync<T>(_collectionName, query);
        }
        public Task<WriteConcernResult> RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags)
        {
            return _databaseUpdater.RemoveAsync<T>(_collectionName, query, removeFlags);
        }
        public Task<WriteConcernResult> RemoveAsync<T>(IMongoQuery query, WriteConcern writeConcern)
        {
            return _databaseUpdater.RemoveAsync<T>(_collectionName, query, writeConcern);
        }
        public Task<WriteConcernResult> RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
           return  _databaseUpdater.RemoveAsync<T>(_collectionName, query, removeFlags, writeConcern);
        }

        public Task<FindAndModifyResult> FindAndModifyAsync<T>(FindAndModifyArgs findAndModifyArgs)
        {
            return _databaseUpdater.FindAndModifyAsync<T>(_collectionName, findAndModifyArgs);
        }
        public Task<FindAndModifyResult> FindAndRemoveAsync<T>(FindAndRemoveArgs findAndRemoveArgs)
        {
            return _databaseUpdater.FindAndRemoveAsync<T>(_collectionName, findAndRemoveArgs);
        }
        #endregion Asynchronous
    }

    public class CollectionUpdater<T> : ICollectionUpdater<T>
    {
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

        public Task<WriteConcernResult> RemoveAsync(IMongoQuery query)
        {
            return _collectionUpdater.RemoveAsync<T>(query);
        }

        public Task<WriteConcernResult> RemoveAsync(IMongoQuery query, WriteConcern writeConcern)
        {
            return _collectionUpdater.RemoveAsync<T>(query, writeConcern);
        }

        public Task<WriteConcernResult> RemoveAsync(IMongoQuery query, RemoveFlags removeFlags)
        {
            return _collectionUpdater.RemoveAsync<T>(query, removeFlags);
        }

        public Task<WriteConcernResult> RemoveAsync(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _collectionUpdater.RemoveAsync<T>(query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify(FindAndModifyArgs findAndModifyArgs)
        {
            return _collectionUpdater.FindAndModify<T>(findAndModifyArgs);
        }

        public FindAndModifyResult FindAndRemove(FindAndRemoveArgs findAndRemoveArgs)
        {
            return _collectionUpdater.FindAndRemove<T>(findAndRemoveArgs);
        }

        public Task<FindAndModifyResult> FindAndModifyAsync(FindAndModifyArgs findAndModifyArgs)
        {
            return _collectionUpdater.FindAndModifyAsync<T>(findAndModifyArgs);
        }

        public Task<FindAndModifyResult> FindAndRemoveAsync(FindAndRemoveArgs findAndRemoveArgs)
        {
            return _collectionUpdater.FindAndRemoveAsync<T>(findAndRemoveArgs);
        }
    }
}
