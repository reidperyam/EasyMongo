
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

        public FindAndModifyResult FindAndModify<T>(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.FindAndModify(findAndModifyArgs);
        }

        public FindAndModifyResult FindAndRemove<T>(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            var collection = GetCollection<T>(collectionName);
            return collection.FindAndRemove(findAndRemoveArgs);
        }

        private MongoCollection<T> GetCollection<T>(string collectionName)
        {
            return _databaseConnection.GetCollection<T>(collectionName);
        }
    }

    public class Updater<T> : IUpdater<T>
    {
        private IUpdater _updater;

        public Updater(IUpdater updater)
        {
            _updater = updater;
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query)
        {
            return _updater.Remove<T>(collectionName, query);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags)
        {
            return _updater.Remove<T>(collectionName, query, removeFlags);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, RemoveFlags removeFlags, WriteConcern writeConcern)
        {
            return _updater.Remove<T>(collectionName, query, removeFlags, writeConcern);
        }

        public WriteConcernResult Remove(string collectionName, IMongoQuery query, WriteConcern writeConcern)
        {
            return _updater.Remove<T>(collectionName, query, writeConcern);
        }

        public FindAndModifyResult FindAndModify(string collectionName, FindAndModifyArgs findAndModifyArgs)
        {
            return _updater.FindAndModify<T>(collectionName, findAndModifyArgs);
        }

        public FindAndModifyResult FindAndRemove(string collectionName, FindAndRemoveArgs findAndRemoveArgs)
        {
            return _updater.FindAndRemove<T>(collectionName, findAndRemoveArgs);
        }
    }
}
