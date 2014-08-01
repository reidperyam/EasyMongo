using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;
using System.Threading;
using Microsoft.CSharp.RuntimeBinder;

namespace EasyMongo.Async
{
    public class UpdaterTask : IUpdaterTask
    {
        IUpdater _updater;

        public UpdaterTask(IUpdater updater)
        {
            _updater = updater;
        }

        public async Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query)
        {
            return await Task.Run(() => { return _updater.Remove<T>(collectionName, query); });
        }

        public async Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return await Task.Run(() => { return _updater.Remove<T>(collectionName, query); });
        }

        public async Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return await Task.Run(() => { return _updater.Remove<T>(collectionName, query); });
        }

        public async Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return await Task.Run(() => { return _updater.Remove<T>(collectionName, query); });
        }

        public async Task<FindAndModifyResult> FindAndModifyAsync<T>(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return await Task.Run(() => { return _updater.FindAndModify<T>(collectionName, findAndModifyArgs); });
        }

        public async Task<FindAndModifyResult> FindAndRemoveAsync<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return await Task.Run(() => { return _updater.FindAndRemove<T>(collectionName, findAndRemoveArgs); });
        }
    }

    public class UpdaterTask<T> : IUpdaterTask<T>
    {
        IUpdaterTask _updater;

        public UpdaterTask(IUpdaterTask updater)
        {
            _updater = updater;
        }

        public async Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query)
        {
            return await Task.Run(() => { return _updater.RemoveAsync<T>(collectionName, query); });
        }

        public async Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return await Task.Run(() => { return _updater.RemoveAsync<T>(collectionName, query, writeConcern); });
        }

        public async Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return await Task.Run(() => { return _updater.RemoveAsync<T>(collectionName, query, removeFlags); });
        }

        public async Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return await Task.Run(() => { return _updater.RemoveAsync<T>(collectionName, query, removeFlags, writeConcern); });
        }

        public async Task<FindAndModifyResult> FindAndModifyAsync(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return await Task.Run(() => { return _updater.FindAndModifyAsync<T>(collectionName, findAndModifyArgs); });
        }

        public async Task<FindAndModifyResult> FindAndRemoveAsync(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return await Task.Run(() => { return _updater.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs); });
        }
    }
}
