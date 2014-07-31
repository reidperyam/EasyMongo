using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public interface IUpdater
    {
        WriteConcernResult Remove<T>(string collectionName, IMongoQuery query);
        WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate);
        FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew);
        FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert);
        FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert);

        FindAndModifyResult FindAndRemove<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs);
    }
}
