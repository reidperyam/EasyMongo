using System;
using System.Collections.Generic;
using System.Text;
using EasyMongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder;
using EasyMongo.Contract;

namespace EasyMongo
{
    public class ReaderAsync<T> : IReaderAsync<T> where T : EntryBase
    {
        private IReader<T> _mongoReader;

        public ReaderAsync(IReader<T> mongoReader)
        {
            _mongoReader = mongoReader;
        }

        public event ReadCompletedEvent<T> AsyncReadCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;

        #region    Async methods
        public void ReadAsync(string collectionName, string fieldName, string regexPattern)
        {
            new Func<string, string, string, IEnumerable<T>>(_mongoReader.Read).BeginInvoke(collectionName, fieldName, regexPattern, Callback, null);
        }
        public void ReadAsync(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            new Func<string, string, DateTime, DateTime, IEnumerable<T>>(_mongoReader.Read).BeginInvoke(collectionName, dateTimeFieldName, start, end, Callback, null);
        }
        public void ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            new Func<string, string, string, string, DateTime, DateTime, IEnumerable<T>>(_mongoReader.Read).BeginInvoke(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end, Callback, null);
        }

        public void ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            new Func<IEnumerable<string>, string, string, IEnumerable<T>>(_mongoReader.Read).BeginInvoke(collectionNames, fieldName, regexPattern, Callback, null);
        }
        public void ReadAsync(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            new Func<IEnumerable<string>, string, DateTime, DateTime, IEnumerable<T>>(_mongoReader.Read).BeginInvoke(collectionNames, dateTimeFieldName, start, end, Callback, null);
        }
        public void ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            new Func<IEnumerable<string>, string, string, string, DateTime, DateTime, IEnumerable<T>>(_mongoReader.Read).BeginInvoke(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end, Callback, null);
        }
        #region Distinct Across Collection
        public void DistinctAsync(string collectionName, string fieldName)
        {
            new Func<string, string, IEnumerable<BsonValue>>(_mongoReader.Distinct).BeginInvoke(collectionName, fieldName, CallbackBson, null);
        }
        public void DistinctAsync(string collectionName, string fieldName, IMongoQuery query)
        {
            new Func<string, string, IMongoQuery, IEnumerable<BsonValue>>(_mongoReader.Distinct).BeginInvoke(collectionName, fieldName, query, CallbackBson, null);
        }
        #endregion Distinct Across Collection
        #region Distinct Across Multiple Collections
        public void DistinctAsync(IEnumerable<string> collectionNames, string fieldName)
        {
            new Func<IEnumerable<string>, string, IEnumerable<BsonValue>>(_mongoReader.Distinct).BeginInvoke(collectionNames, fieldName, CallbackBson, null);
        }
        public void DistinctAsync(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            new Func<IEnumerable<string>, string, IMongoQuery, IEnumerable<BsonValue>>(_mongoReader.Distinct).BeginInvoke(collectionNames, fieldName, query, CallbackBson, null);
        }
        #endregion Distinct Across Multiple Collections

        #endregion Async methods

        #region    Callback methods

        protected void Callback(IAsyncResult asyncRes)
        {
            IEnumerable<T> result = null;
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
                    if (AsyncReadCompleted != null)
                        AsyncReadCompleted(result, exception);
                }
            }
            catch (RuntimeBinderException ex)
            {
                if (AsyncReadCompleted != null)
                    AsyncReadCompleted(result, ex);
            }
        }

        protected void CallbackBson(IAsyncResult asyncRes)
        {
            IEnumerable<BsonValue> result = null;
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
                    if (AsyncDistinctCompleted != null)
                        AsyncDistinctCompleted(result, exception);
                }
            }
            catch (RuntimeBinderException ex)
            {
                if (AsyncDistinctCompleted != null)
                    AsyncDistinctCompleted(result, ex);
            }
        }
        #endregion Callback methods

        public IReaderAsync<T> Create(IReader<T> reader)
        {
            return new ReaderAsync<T>(reader);
        }
    }
}
