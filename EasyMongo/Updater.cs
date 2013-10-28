
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo
{
    public class Updater : IUpdater
    {
        private IDatabaseConnection _databaseConnection;

        public Updater(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.Remove(query);
        }

        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.Remove(query, removeFlags);
        }

        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.Remove(query, removeFlags, writeConcern);
        }

        public WriteConcernResult Remove<T>(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.Remove(query, writeConcern);
        }

        public FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.FindAndModify(mongoQuery, mongoSortBy, mongoUpdate);
        }

        public FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.FindAndModify(mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }

        public FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.FindAndModify(mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }

        public FindAndModifyResult FindAndModify<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.FindAndModify(mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }

        public FindAndModifyResult FindAndRemove<T>(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.FindAndRemove(mongoQuery, mongoSortBy);
        }

        private MongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _databaseConnection.GetCollection<T>(collectionName);
        }
    }

    public class Updater<T> : IUpdater<T>
    {
        private IDatabaseConnection _databaseConnection;

        public Updater(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query)
        {
            var collection = GetCollection(collectionName);
            return collection.Remove(query);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            var collection = GetCollection(collectionName);
            return collection.Remove(query, removeFlags);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            var collection = GetCollection(collectionName);
            return collection.Remove(query, removeFlags, writeConcern);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            var collection = GetCollection(collectionName);
            return collection.Remove(query, writeConcern);
        }

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate)
        {
            var collection = GetCollection(collectionName);
            return collection.FindAndModify(mongoQuery, mongoSortBy, mongoUpdate);
        }

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew)
        {
            var collection = GetCollection(collectionName);
            return collection.FindAndModify(mongoQuery, mongoSortBy, mongoUpdate, returnNew);
        }

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, bool returnNew, bool upsert)
        {
            var collection = GetCollection(collectionName);
            return collection.FindAndModify(mongoQuery, mongoSortBy, mongoUpdate, returnNew, upsert);
        }

        public FindAndModifyResult FindAndModify(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy, IMongoUpdate mongoUpdate, IMongoFields fields, bool returnNew, bool upsert)
        {
            var collection = GetCollection(collectionName);
            return collection.FindAndModify(mongoQuery, mongoSortBy, mongoUpdate, fields, returnNew, upsert);
        }

        public FindAndModifyResult FindAndRemove(string collectionName, IMongoQuery mongoQuery, IMongoSortBy mongoSortBy)
        {
            var collection = GetCollection(collectionName);
            return collection.FindAndRemove(mongoQuery, mongoSortBy);
        }

        private MongoCollection<T> GetCollection(string collectionName)
        {
            return _databaseConnection.GetCollection<T>(collectionName);
        }
    }
}
