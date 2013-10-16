using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using MongoDB.Driver.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.InteropServices;
using Microsoft.CSharp.RuntimeBinder;
using EasyMongo.Contract;

namespace EasyMongo.Async
{
    public class ReaderAsync : IReaderAsync
    {
        private IReader _mongoReader;

        public ReaderAsync(IReader mongoReader)
        {
            _mongoReader = mongoReader;
        }

        public event ReadCompletedEvent AsyncReadCompleted;
        public event DistinctBSONCompletedEvent AsyncDistinctBSONCompleted;
        public event DistinctCompletedEvent AsyncDistinctCompleted;

        #region    Async methods
        public void ReadAsync<T>(string collectionName, string fieldName, string regexPattern)
        {
            new Func<string, string, string, IEnumerable<T>>(_mongoReader.Read<T>).BeginInvoke(collectionName, fieldName, regexPattern, Callback<T>, null);
        }
        public void ReadAsync<T>(string collectionName, string dateTimeFieldName, DateTime start, DateTime end)
        {
            new Func<string, string, DateTime, DateTime, IEnumerable<T>>(_mongoReader.Read<T>).BeginInvoke(collectionName, dateTimeFieldName, start, end, Callback<T>, null);
        }
        public void ReadAsync<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            new Func<string, string, string, string, DateTime, DateTime, IEnumerable<T>>(_mongoReader.Read<T>).BeginInvoke(collectionName, fieldName, regexPattern, dateTimeFieldName, start, end, Callback<T>, null);
        }

        public void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern)
        {
            new Func<IEnumerable<string>, string, string, IEnumerable<T>>(_mongoReader.Read<T>).BeginInvoke(collectionNames, fieldName, regexPattern, Callback<T>, null);
        }
        public void ReadAsync<T>(IEnumerable<string> collectionNames, string dateTimeFieldName, DateTime start, DateTime end)
        {
            new Func<IEnumerable<string>, string, DateTime, DateTime, IEnumerable<T>>(_mongoReader.Read<T>).BeginInvoke(collectionNames, dateTimeFieldName, start, end, Callback<T>, null);
        }
        public void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end)
        {
            new Func<IEnumerable<string>, string, string, string, DateTime, DateTime, IEnumerable<T>>(_mongoReader.Read<T>).BeginInvoke(collectionNames, fieldName, regexPattern, dateTimeFieldName, start, end, Callback<T>, null);
        }
        #region Distinct Across Collection BSON
        public void DistinctAsync(string collectionName, string fieldName)
        {
            new Func<string, string, IEnumerable<BsonValue>>(_mongoReader.Distinct).BeginInvoke(collectionName, fieldName, CallbackDistinctBson, null);
        }
        public void DistinctAsync(string collectionName, string fieldName, IMongoQuery query)
        {
            new Func<string, string, IMongoQuery, IEnumerable<BsonValue>>(_mongoReader.Distinct).BeginInvoke(collectionName, fieldName, query, CallbackDistinctBson, null);
        }
        #endregion Distinct Across Collection BSON
        #region Distinct Across Collection T
        public void DistinctAsync<T>(string collectionName, string fieldName)
        {
            new Func<string, string, IEnumerable<T>>(_mongoReader.Distinct<T>).BeginInvoke(collectionName, fieldName, CallbackDistinct<T>, null);
        }
        public void DistinctAsync<T>(string collectionName, string fieldName, IMongoQuery query)
        {
            new Func<string, string, IMongoQuery, IEnumerable<T>>(_mongoReader.Distinct<T>).BeginInvoke(collectionName, fieldName, query, CallbackDistinct<T>, null);
        }
        #endregion Distinct Across Collection T
        #region Distinct Across Multiple Collections BSON
        public void DistinctAsync(IEnumerable<string> collectionNames, string fieldName)
        {
            new Func<IEnumerable<string>, string, IEnumerable<BsonValue>>(_mongoReader.Distinct).BeginInvoke(collectionNames, fieldName, CallbackDistinctBson, null);
        }
        public void DistinctAsync(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            new Func<IEnumerable<string>, string, IMongoQuery, IEnumerable<BsonValue>>(_mongoReader.Distinct).BeginInvoke(collectionNames, fieldName, query, CallbackDistinctBson, null);
        }
        #endregion Distinct Across Multiple Collections BSON
        #region Distinct Across Multiple Collections T
        public void DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName)
        {
            new Func<IEnumerable<string>, string, IEnumerable<T>>(_mongoReader.Distinct<T>).BeginInvoke(collectionNames, fieldName, CallbackDistinct<T>, null);
        }
        public void DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query)
        {
            new Func<IEnumerable<string>, string, IMongoQuery, IEnumerable<T>>(_mongoReader.Distinct<T>).BeginInvoke(collectionNames, fieldName, query, CallbackDistinct<T>, null);
        }
        #endregion Distinct Across Multiple Collections T

        #endregion Async methods

        #region    Callback methods

        protected void Callback<T>(IAsyncResult asyncRes)
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

        protected void CallbackDistinct<T>(IAsyncResult asyncRes)
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

        protected void CallbackDistinctBson(IAsyncResult asyncRes)
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
                    if (AsyncDistinctBSONCompleted != null)
                        AsyncDistinctBSONCompleted(result, exception);
                }
            }
            catch (RuntimeBinderException ex)
            {
                if (AsyncDistinctBSONCompleted != null)
                    AsyncDistinctBSONCompleted(result, ex);
            }
        }
        #endregion Callback methods
    }
}
