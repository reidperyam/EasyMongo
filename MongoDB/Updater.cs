
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting.Messaging;
using MongoDB.Driver;
using MongoDB.Contract;

namespace MongoDB
{
    public class Updater<T> : IUpdater<T> where T : EntryBase
    {
        private IDatabaseConnection<T> _databaseConnection;

        public Updater(IDatabaseConnection<T> databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query)
        {
            var collection = _databaseConnection.GetCollection(collectionName);
            return collection.Remove(query);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            var collection = _databaseConnection.GetCollection(collectionName);
            return collection.Remove(query, removeFlags);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            var collection = _databaseConnection.GetCollection(collectionName);
            return collection.Remove(query, removeFlags, writeConcern);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            var collection = _databaseConnection.GetCollection(collectionName);
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

        public IUpdater<T> Create(IDatabaseConnection<T> databaseConnection)
        {
            return new MongoDB.Updater<T>(databaseConnection);
        }

        private MongoCollection<T> GetCollection(string collectionName)
        {
            return _databaseConnection.GetCollection(collectionName);
        }
    }
}
