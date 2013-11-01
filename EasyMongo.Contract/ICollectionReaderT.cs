using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface ICollectionReader
    {
        event ReadCompletedEvent     AsyncReadCompleted;
        event DistinctCompletedEvent AsyncDistinctCompleted;

        IEnumerable<T> Read<T>(string fieldName, string regexPattern);
        IEnumerable<T> Read<T>(string dateTimeFieldName, DateTime start, DateTime end);
        IEnumerable<T> Read<T>(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        void ReadAsync<T>(string fieldName, string regexPattern);
        void ReadAsync<T>(string dateTimeFieldName, DateTime start, DateTime end);
        void ReadAsync<T>(string fieldName, string regexPattern, string dateTimeFieldName, DateTime start, DateTime end);

        IEnumerable<T> Distinct<T>(string fieldName);
        IEnumerable<T> Distinct<T>(string fieldName, IMongoQuery query);

        void DistinctAsync<T>(string fieldName);
        void DistinctAsync<T>(string fieldName, IMongoQuery query);
    }
}
