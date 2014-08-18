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
    public class AsyncUpdater : IAsyncUpdater
    {
        IUpdater _updater;

        public AsyncUpdater(IUpdater updater)
        {
            _updater = updater;
        }

        public Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query)
        {
            return Task.Run(() => { return _updater.Remove<T>(collectionName, query); });
        }

        public Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return Task.Run(() => { return _updater.Remove<T>(collectionName, query, writeConcern); });
        }

        public Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return Task.Run(() => { return _updater.Remove<T>(collectionName, query, removeFlags); });
        }

        public Task<WriteConcernResult> RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return Task.Run(() => { return _updater.Remove<T>(collectionName, query, removeFlags, writeConcern); });
        }

        public Task<FindAndModifyResult> FindAndModifyAsync<T>(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return Task.Run(() => { return _updater.FindAndModify<T>(collectionName, findAndModifyArgs); });
        }

        public Task<FindAndModifyResult> FindAndRemoveAsync<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return Task.Run(() => { return _updater.FindAndRemove<T>(collectionName, findAndRemoveArgs); });
        }
    }

    public class UpdaterTask<T> : IAsyncUpdater<T>
    {
        IAsyncUpdater _updater;

        public UpdaterTask(IAsyncUpdater updater)
        {
            _updater = updater;
        }

        public Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query)
        {
            return Task.Run(() => { return _updater.RemoveAsync<T>(collectionName, query); });
        }

        public Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return Task.Run(() => { return _updater.RemoveAsync<T>(collectionName, query, writeConcern); });
        }

        public Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return Task.Run(() => { return _updater.RemoveAsync<T>(collectionName, query, removeFlags); });
        }

        public Task<WriteConcernResult> RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return Task.Run(() => { return _updater.RemoveAsync<T>(collectionName, query, removeFlags, writeConcern); });
        }

        public Task<FindAndModifyResult> FindAndModifyAsync(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return Task.Run(() => { return _updater.FindAndModifyAsync<T>(collectionName, findAndModifyArgs); });
        }

        public Task<FindAndModifyResult> FindAndRemoveAsync(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return Task.Run(() => { return _updater.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs); });
        }
    }
}
