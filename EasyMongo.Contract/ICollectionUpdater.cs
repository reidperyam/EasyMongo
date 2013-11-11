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

        FindAndModifyResult FindAndModify<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate);
        FindAndModifyResult FindAndModify<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew);
        FindAndModifyResult FindAndModify<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert);
        FindAndModifyResult FindAndModify<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert);
        FindAndModifyResult FindAndRemove<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy);

        void FindAndModifyAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate);
        void FindAndModifyAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew);
        void FindAndModifyAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert);
        void FindAndModifyAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert);

        void FindAndRemoveAsync<T>(IMongoQuery mongoQuery, IMongoSortBy mongoSortBy);
    }
}
