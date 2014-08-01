using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Contract
{
    public interface ICollectionReader<T>
    {
        event ReadCompletedEvent     AsyncReadCompleted;
        event DistinctCompletedEvent AsyncDistinctCompleted;

        IEnumerable<T> Read(string fieldName, string regexPattern);
        IEnumerable<T> Read(string dateTimeFieldName, DateTime start, DateTime end);
        IEnumerable<T> Read(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        void ReadAsync(string fieldName, string regexPattern);
        void ReadAsync(string dateTimeFieldName, DateTime start, DateTime end);
        void ReadAsync(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        IEnumerable<Y> Distinct<Y>(string fieldName);
        IEnumerable<Y> Distinct<Y>(string fieldName, IMongoQuery query);

        void DistinctAsync<Y>(string fieldName);
        void DistinctAsync<Y>(string fieldName, IMongoQuery query);
    }
}
