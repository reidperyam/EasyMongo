using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract.Deprecated
{
    public delegate void ReadCompletedEvent(object e, Exception ex); // this will necessitate cast to IEnumerable<T> in the handler code in order to retrieve list of read items
    public delegate void DistinctCompletedEvent(object e, Exception ex);

    [Obsolete("This interface is obselete")]
    public interface IReaderAsync
    {
        event ReadCompletedEvent AsyncReadCompleted;
        event DistinctCompletedEvent AsyncDistinctCompleted;

        void ReadAsync<T>(string collectionName, string fieldName, string regexPattern);
        void ReadAsync<T>(string collectionName, string fieldName, DateTime start, DateTime end);
        void ReadAsync<T>(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern);
        void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end);
        void ReadAsync<T>(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        void DistinctAsync<T>(string collectionName, string fieldName);
        void DistinctAsync<T>(string collectionName, string fieldName, IMongoQuery query);
        void DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName);
        void DistinctAsync<T>(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query);
    }
}
