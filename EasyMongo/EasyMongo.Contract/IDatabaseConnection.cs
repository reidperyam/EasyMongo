using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace  EasyMongo.Contract
{
    public interface IDatabaseConnection
    {
        IServerConnection MongoServerConnection
        {
            get;
        }

        MongoDatabase Db
        {
            get;
            set;
        }

        void Connect();

        void ConnectAsyncDelegate(Action<ConnectionResult,string> callback);

        void ConnectAsyncTask();

        MongoServerState State 
        {
            get;
        }

        MongoCollection<T> GetCollection<T>(string collectionName);

        List<MongoCollection<T>> GetCollections<T>();

        List<string> GetCollectionNames();

        void ClearCollection<T>(string collectionName);

        void ClearAllCollections<T>();

        void DropCollection<T>(string collectionName);

        void DropAllCollections<T>();

        MongoCollection this[string collectionName]
        {
            get;
        }
    }
}
