using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;

namespace  MongoDB.Contract
{
    public interface IDatabaseConnection<T>
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

        MongoCollection<T> GetCollection(string collectionName);

        List<MongoCollection<T>> GetCollections();

        List<string> GetCollectionNames();

        void ClearCollection(string collectionName);

        void ClearAllCollections();

        void DropCollection(string collectionName);

        void DropAllCollections();

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
