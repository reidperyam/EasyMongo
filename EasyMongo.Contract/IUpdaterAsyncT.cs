﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Contract
{
    public interface IUpdaterAsync<T>
    {
        event FindAndModifyCompletedEvent AsyncFindAndModifyCompleted;
        event FindAndRemoveCompletedEvent AsyncFindAndRemoveCompleted;

        void RemoveAsync(string collectionName, IMongoQuery query);
        void RemoveAsync(string collectionName, IMongoQuery query, WriteConcern writeConcern);
        void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags);
        void RemoveAsync(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern);

        void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate);
        void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew);
        void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew,      bool upsert);
        void FindAndModifyAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert);

        void FindAndRemoveAsync(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy);
    }
}