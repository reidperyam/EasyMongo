using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using EasyMongo.Contract;
using MongoDB.Driver;
using System.Threading;
using Microsoft.CSharp.RuntimeBinder;

namespace EasyMongo.Async
{
    public class UpdaterAsync<T> : IUpdaterAsync<T> where T : EntryBase
    {
        public event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        public event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        IUpdater<T> _mongoUpdater;

        public UpdaterAsync(IUpdater<T> mongoUpdater)
        {
            _mongoUpdater = mongoUpdater;
        }

        public void RemoveAsync(string collectionName, IMongoQuery query)
        {
            new Func<string, IMongoQuery, WriteConcernResult>(_mongoUpdater.Remove).BeginInvoke(collectionName, query, CallbackwriteConcernResult, null);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            new Func<string, IMongoQuery, RemoveFlags, WriteConcernResult>(_mongoUpdater.Remove).BeginInvoke(collectionName, query, removeFlags, CallbackwriteConcernResult, null);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            new Func<string, IMongoQuery, RemoveFlags, WriteConcern, WriteConcernResult>(_mongoUpdater.Remove).BeginInvoke(collectionName, query, removeFlags, writeConcern, CallbackwriteConcernResult, null);
        }

        public void RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            new Func<string, IMongoQuery, WriteConcern, WriteConcernResult>(_mongoUpdater.Remove).BeginInvoke(collectionName, query, writeConcern, CallbackwriteConcernResult, null);
        }

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            new Func<string, IMongoQuery, IMongoSortBy, IMongoUpdate, FindAndModifyResult>(_mongoUpdater.FindAndModify).BeginInvoke(collectionName, mongoQuery, mongoSortBy, mongoUpdate, CallbackFindAndModify, null);
        }

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            new Func<string, IMongoQuery, IMongoSortBy, IMongoUpdate, bool, FindAndModifyResult>(_mongoUpdater.FindAndModify).BeginInvoke(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, CallbackFindAndModify, null);
        }

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            new Func<string, IMongoQuery, IMongoSortBy, IMongoUpdate, bool, bool, FindAndModifyResult>(_mongoUpdater.FindAndModify).BeginInvoke(collectionName, mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert, CallbackFindAndModify, null);
        }

        public void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            new Func<string, IMongoQuery, IMongoSortBy, IMongoUpdate, IMongoFields, bool, bool, FindAndModifyResult>(_mongoUpdater.FindAndModify).BeginInvoke(collectionName, mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert, CallbackFindAndModify, null);
        }

        public void FindAndRemoveAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            new Func<string, IMongoQuery, IMongoSortBy, FindAndModifyResult>(_mongoUpdater.FindAndRemove).BeginInvoke(collectionName, mongoQuery, mongoSortBy, CallbackFindAndModify, null);
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
                if (AsyncFindAndModifyCompleted != null)
                    AsyncFindAndModifyCompleted(result);
            }
        }

        public IUpdaterAsync<T> Create(IUpdater<T> updater)
        {
            return new UpdaterAsync<T>(updater);
        }
    }
}
