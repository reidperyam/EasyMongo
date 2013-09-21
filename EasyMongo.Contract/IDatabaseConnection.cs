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
        //event ConnectAsyncCompletedEvent ConnectAsyncCompleted;//now private

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

        MongoDatabaseSettings CreateDatabaseSettings();

        MongoCollection<T> GetCollection<T>(string collectionName);

        List<MongoCollection<T>> GetCollections<T>();

        List<string> GetCollectionNames();

        void ClearCollection<T>(string collectionName);

        void ClearAllCollections<T>();

        void DropCollection<T>(string collectionName);

        void DropAllCollections<T>();

        bool CanConnect();

        IDisposable RequestStart(MongoServerInstance mongoServerInstance);

        IDisposable RequestStart(bool slaveOk);

        void RequestDone();

        GetLastErrorResult GetLastError();

        MongoCollection this[string collectionName]
        {
            get;
        }
    }
}
