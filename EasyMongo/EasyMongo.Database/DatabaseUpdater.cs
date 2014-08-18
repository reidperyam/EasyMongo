using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Database
{
    public class DatabaseUpdater : IDatabaseUpdater
    {
        protected IUpdater _updater;

        protected IUpdaterTask _updaterTask;

        public DatabaseUpdater(IUpdater     updater,
                               IUpdaterTask updaterTask)
        {
            _updater = updater;
            _updaterTask = updaterTask;
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
        public Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query)
        {
            return _updaterTask.RemoveAsync<T>(collectionName, query);
        }
        public Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return _updaterTask.RemoveAsync<T>(collectionName, query, removeFlags);
        }
        public Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return _updaterTask.RemoveAsync<T>(collectionName, query, writeConcern);
        }
        public Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _updaterTask.RemoveAsync<T>(collectionName, query, removeFlags, writeConcern);
        }

        public FindAndModifyResult FindAndModify<T>(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return _updater.FindAndModify<T>(collectionName, findAndModifyArgs);
        }
        public FindAndModifyResult FindAndRemove<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return _updater.FindAndRemove<T>(collectionName, findAndRemoveArgs);
        }

        public Task<FindAndModifyResult> FindAndModifyAsync<T>(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return _updaterTask.FindAndModifyAsync<T>(collectionName, findAndModifyArgs);
        }
        public Task<FindAndModifyResult> FindAndRemoveAsync<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return _updaterTask.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs);
        }
    }

    public class DatabaseUpdater<T> : IDatabaseUpdater<T>
    {
        IDatabaseUpdater _databaseUpdater;

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

        public Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query)
        {
            return _databaseUpdater.RemoveAsync<T>(collectionName, query);
        }

        public Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return _databaseUpdater.RemoveAsync<T>(collectionName, query, writeConcern);
        }

        public Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return _databaseUpdater.RemoveAsync<T>(collectionName, query, removeFlags);
        }

        public Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _databaseUpdater.RemoveAsync<T>(collectionName, query, removeFlags, writeConcern);
        }

        public Task<FindAndModifyResult> FindAndModifyAsync(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return _databaseUpdater.FindAndModifyAsync<T>(collectionName, findAndModifyArgs);
        }

        public Task<FindAndModifyResult> FindAndRemoveAsync(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return _databaseUpdater.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs);
        }
    }
}
