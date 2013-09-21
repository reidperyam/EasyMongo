using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Contract
{
    public delegate void FindAndRemoveCompletedEvent(WriteConcernResult result);
    public delegate void FindAndModifyCompletedEvent(FindAndModifyResult result);

    public interface IUpdaterAsync
    {
        event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        void RemoveAsync<T>(string collectionName, IMongoQuery query);
        void RemoveAsync<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        void RemoveAsync<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        void FindAndModifyAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate);
        void FindAndModifyAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew);
        void FindAndModifyAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert);
        void FindAndModifyAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert);

        void FindAndRemoveAsync<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy);
    }
}
