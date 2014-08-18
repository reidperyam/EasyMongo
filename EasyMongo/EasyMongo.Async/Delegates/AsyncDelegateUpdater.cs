using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;
using System.Threading;
using Microsoft.CSharp.RuntimeBinder;

namespace EasyMongo.Async.Delegates
{
    [Obsolete("This class is obselete")]
    public class AsyncDelegateUpdater : IAsyncDelegateUpdater
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        IUpdater _updater;

        public AsyncDelegateUpdater(IUpdater updater)
        {
            _updater = updater;
        }

        public void RemoveAsync<T>(string collectionName, IMongoQuery query)
        {
            new Func<string, IMongoQuery, WriteConcernResult>(_updater.Remove<T>).BeginInvoke(collectionName, query, CallbackwriteConcernResult, null);
        }

        public void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            new Func<string, IMongoQuery, RemoveFlags, WriteConcernResult>(_updater.Remove<T>).BeginInvoke(collectionName, query, removeFlags, CallbackwriteConcernResult, null);
        }

        public void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            new Func<string, IMongoQuery, RemoveFlags, WriteConcern, WriteConcernResult>(_updater.Remove<T>).BeginInvoke(collectionName, query, removeFlags, writeConcern, CallbackwriteConcernResult, null);
        }

        public void RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            new Func<string, IMongoQuery, WriteConcern, WriteConcernResult>(_updater.Remove<T>).BeginInvoke(collectionName, query, writeConcern, CallbackwriteConcernResult, null);
        }

        public void FindAndModifyAsync<T>(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            new Func<string, FindAndModifyArgs, FindAndModifyResult>(_updater.FindAndModify<T>).BeginInvoke(collectionName, findAndModifyArgs, CallbackFindAndModify, null);
        }

        public void FindAndRemoveAsync<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            new Func<string, FindAndRemoveArgs, FindAndModifyResult>(_updater.FindAndRemove<T>).BeginInvoke(collectionName, findAndRemoveArgs, CallbackFindAndModify, null);
        }

        protected void CallbackwriteConcernResult(IAsyncResult asyncRes)
        {
            WriteConcernResult result = null;
            Exception exception = null;

            try
            {
                try
                {
                    AsyncResult ares = (AsyncResult)asyncRes;
                    var delg = (dynamic)ares.AsyncDelegate;
                    result = delg.EndInvoke(asyncRes);
                }
                catch (Exception ex)
                {
                    // if an exception is caught here the dynamic binding of the "result" variable is unresolved thus causing a 
                    // RuntimeTypeBindingException to be raised. The problem is that in this scenario - complicated to determine which
                    // sort of action originally invoked the async call -- making notification of the error problematic...
                    exception = ex;
                }
                finally
                {
                    if (AsyncFindAndRemoveCompleted != null)
                        AsyncFindAndRemoveCompleted(result);
                }
            }
            catch (RuntimeBinderException ex)
            {
                // TODO: add ex handling!
                if (AsyncFindAndRemoveCompleted != null)
                    AsyncFindAndRemoveCompleted(result);
            }
        }

        protected void CallbackFindAndModify(IAsyncResult asyncRes)
        {
            FindAndModifyResult result = null;
            Exception exception = null;

            try
            {
                try
                {
                    AsyncResult ares = (AsyncResult)asyncRes;
                    var delg = (dynamic)ares.AsyncDelegate;
                    result = delg.EndInvoke(asyncRes);
                }
                catch (Exception ex)
                {
                    // if an exception is caught here the dynamic binding of the "result" variable is unresolved thus causing a 
                    // RuntimeTypeBindingException to be raised. The problem is that in this scenario - complicated to determine which
                    // sort of action originally invoked the async call -- making notification of the error problematic...
                    exception = ex;
                }
                finally
                {
                    if (AsyncFindAndModifyCompleted != null)
                        AsyncFindAndModifyCompleted(result);
                }
            }
            catch (RuntimeBinderException ex)
            {
                // TODO: add ex handling!
                if (AsyncFindAndModifyCompleted != null)
                    AsyncFindAndModifyCompleted(result);
            }
        }
    }

    [Obsolete("This class is obselete")]
    public class UpdaterAsync<T> : IAsyncDelegateUpdater<T>
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted
        {
            add
            {
                lock (_mongoUpdater)
                {
                    _mongoUpdater.AsyncFindAndModifyCompleted += value;
                }
            }
            remove
            {
                lock (_mongoUpdater)
                {
                    _mongoUpdater.AsyncFindAndModifyCompleted -= value;
                }
            }
        }

        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted
        {
            add
            {
                lock (_mongoUpdater)
                {
                    _mongoUpdater.AsyncFindAndRemoveCompleted += value;
                }
            }
            remove
            {
                lock (_mongoUpdater)
                {
                    _mongoUpdater.AsyncFindAndRemoveCompleted -= value;
                }
            }
        }

        IAsyncDelegateUpdater _mongoUpdater;

        public UpdaterAsync(IAsyncDelegateUpdater mongoUpdater)
        {
            _mongoUpdater = mongoUpdater;
        }

        public void RemoveAsync(string collectionName, IMongoQuery query)
        {
            _mongoUpdater.RemoveAsync<T>(collectionName, query);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            _mongoUpdater.RemoveAsync<T>(collectionName, query, removeFlags);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            _mongoUpdater.RemoveAsync<T>(collectionName, query, removeFlags, writeConcern);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            _mongoUpdater.RemoveAsync<T>(collectionName, query, writeConcern);
        }

        public void FindAndModifyAsync(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            _mongoUpdater.FindAndModifyAsync<T>(collectionName, findAndModifyArgs);
        }

        public void FindAndRemoveAsync(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            _mongoUpdater.FindAndRemoveAsync<T>(collectionName, findAndRemoveArgs);
        }
    }
}
