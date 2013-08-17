using System;
using MongoDB.Driver;

namespace MongoDB.Contract
{
    public interface ICollectionWriter<T>
    {
        event WriteCompletedEvent AsyncWriteCompleted;
        void Write(T entry);
        void WriteAsync(T entry);

        // Are the WriteConcern overloads required or are they defined in embedded objects?
        //void Remove(IMongoQuery query);
        //void Remove(IMongoQuery query, WriteConcern writeConcern);
        //void Remove(IMongoQuery query, RemoveFlags removeFlags);    
        //void Remove(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        //void RemoveAsync(IMongoQuery query);
        //void RemoveAsync(IMongoQuery query, WriteConcern writeConcern);
        //void RemoveAsync(IMongoQuery query, RemoveFlags removeFlags);
        //void RemoveAsync(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern); 
    }
}
