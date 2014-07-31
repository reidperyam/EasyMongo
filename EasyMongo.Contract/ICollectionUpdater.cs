using System;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface ICollectionUpdater
    {
        event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        // Are the WriteConcern overloads required or are they defined in embedded objects?
        WriteConcernResult Remove<T>(IMongoQuery query);
        WriteConcernResult Remove<T>(IMongoQuery query, WriteConcern writeConcern);
        WriteConcernResult Remove<T>(IMongoQuery query, RemoveFlags removeFlags);
        WriteConcernResult Remove<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        void RemoveAsync<T>(IMongoQuery query);
        void RemoveAsync<T>(IMongoQuery query, WriteConcern writeConcern);
        void RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags);
        void RemoveAsync<T>(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        FindAndModifyResult FindAndModify<T>(FindAndModifyArgs findAndModifyArgs);
        FindAndModifyResult FindAndRemove<T>(FindAndRemoveArgs findAndRemoveArgs);

        void FindAndModifyAsync<T>(FindAndModifyArgs findAndModifyArgs);

        void FindAndRemoveAsync<T>(FindAndRemoveArgs findAndRemoveArgs);
    }
}
