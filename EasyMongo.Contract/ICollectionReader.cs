using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface ICollectionReader<T>
    {
       // event ReadCompletedEvent<T> AsyncReadCompleted;
       // event DistinctCompletedEvent AsyncDistinctCompleted;

        IEnumerable<T> Read(string fieldName, string regexPattern);
        IEnumerable<T> Read(string dateTimeFieldName, DateTime start, DateTime end);
        IEnumerable<T> Read(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        void ReadAsync(string fieldName, string regexPattern);
        void ReadAsync(string dateTimeFieldName, DateTime start, DateTime end);        
        void ReadAsync(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        IEnumerable<BsonValue> Distinct(string fieldName);
        IEnumerable<BsonValue> Distinct(string fieldName, IMongoQuery query);

        void DistinctAsync(string fieldName);
        void DistinctAsync(string fieldName, IMongoQuery query);
    }
}
