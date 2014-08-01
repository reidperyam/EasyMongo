using System;
using MongoDB.Driver;
using EasyMongo.Contract.Deprecated;

namespace EasyMongo.Contract
{
    public interface ICollectionUpdater<T>
    {
        event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        // Are the WriteConcern overloads required or are they defined in embedded objects?
        WriteConcernResult Remove(IMongoQuery query);
        WriteConcernResult Remove(IMongoQuery query, WriteConcern writeConcern);
        WriteConcernResult Remove(IMongoQuery query, RemoveFlags removeFlags);
        WriteConcernResult Remove(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        void RemoveAsync(IMongoQuery query);
        void RemoveAsync(IMongoQuery query, WriteConcern writeConcern);
        void RemoveAsync(IMongoQuery query, RemoveFlags removeFlags);
        void RemoveAsync(IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        FindAndModifyResult FindAndModify(FindAndModifyArgs findAndModifyArgs);

        FindAndModifyResult FindAndRemove(FindAndRemoveArgs findAndRemoveArgs);

        void FindAndModifyAsync(FindAndModifyArgs findAndModifyArgs);

        void FindAndRemoveAsync(FindAndRemoveArgs findAndRemoveArgs);
    }
}
