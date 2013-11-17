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

        void ConnectAsync(Action<ConnectionResult,string> callback);

        ConnectionState ConnectionState
        {
            get;
        }

        void CopyDatabase(string to);

        MongoCollection<T> GetCollection<T>(string collectionName);

        List<MongoCollection<T>> GetCollections<T>();

        List<string> GetCollectionNames();

        void ClearCollection<T>(string collectionName);

        void ClearAllCollections<T>();

        void DropCollection<T>(string collectionName);

        void DropAllCollections<T>();

        bool CanConnect();

        IDisposable RequestStart(MongoServerInstance mongoServerInstance);

        IDisposable RequestStart(ReadPreference readPreference);

        void RequestDone();

        GetLastErrorResult GetLastError();

        MongoCollection this[string collectionName]
        {
            get;
        }
    }
}
