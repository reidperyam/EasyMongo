using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IReaderAsync<T>
    {
        event ReadCompletedEvent AsyncReadCompleted;
        event DistinctBSONCompletedEvent AsyncDistinctBSONCompleted;
        event DistinctCompletedEvent AsyncDistinctCompleted;

        void ReadAsync(string collectionName, string fieldName, string regexPattern);
        void ReadAsync(string collectionName, string fieldName, DateTime start, DateTime end);
        void ReadAsync(string collectionName, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        void ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern);
        void ReadAsync(IEnumerable<string> collectionNames, string fieldName, DateTime start, DateTime end);
        void ReadAsync(IEnumerable<string> collectionNames, string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        //void DistinctAsync(string collectionName, string fieldName);
        //void DistinctAsync(string collectionName, string fieldName, IMongoQuery query);
        //void DistinctAsync(IEnumerable<string> collectionNames, string fieldName);
        //void DistinctAsync(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query);

        void DistinctAsync(string collectionName, string fieldName);
        void DistinctAsync(string collectionName, string fieldName, IMongoQuery query);
        void DistinctAsync(IEnumerable<string> collectionNames, string fieldName);
        void DistinctAsync(IEnumerable<string> collectionNames, string fieldName, IMongoQuery query);
    }
}
