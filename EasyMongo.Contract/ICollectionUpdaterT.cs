using System;
using MongoDB.Driver;

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

        FindAndModifyResult FindAndModify(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate);
        FindAndModifyResult FindAndModify(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew);
        FindAndModifyResult FindAndModify(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert);
        FindAndModifyResult FindAndModify(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert);

        FindAndModifyResult FindAndRemove(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy);

        void FindAndModifyAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate);
        void FindAndModifyAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew);
        void FindAndModifyAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert);
        void FindAndModifyAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert);

        void FindAndRemoveAsync(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy);
    }
}
