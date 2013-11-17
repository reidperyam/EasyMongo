using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    //public delegate void ConnectAsyncCompletedEvent(ConnectionResult result);

    public enum ConnectionState  : uint { NotConnected, Connecting, Connected }
    public enum ConnectionResult : uint { Empty, Success, Failure }

    public interface IServerConnection
    {
        //event ConnectAsyncCompletedEvent ConnectAsyncCompleted;//now private
        bool CanConnect();
        List<string> GetDbNamesForConnection();

        string ConnectionString
        {
            get;
            set;
        }

        MongoServerSettings Settings
        {
            get;
        }

        MongoServerState State
        {
            get;
        }

        ConnectionState ConnectionState
        {
            get;
        }

        void Connect();

        void ConnectAsync(Action<ConnectionResult,string> callaback);

        void CopyDatabase(string from, string to);

        List<CommandResult> DropAllDatabases();

        CommandResult DropDatabase(MongoDatabase mongoDatabase);

        CommandResult DropDatabase(string mongoDatabaseName);

        List<CommandResult> DropDatabases(IEnumerable<MongoDatabase> dbsToDrop);

        List<CommandResult> DropDatabases(IEnumerable<string> dbsToDrop);

        MongoDatabase GetDatabase(string databaseName, WriteConcern writeConcern);

        IDisposable RequestStart(MongoDatabase mongoDatabase);

        IDisposable RequestStart(MongoDatabase mongoDatabase, MongoServerInstance mongoServerInstance);

        IDisposable RequestStart(MongoDatabase mongoDatabase, ReadPreference readPreference);

        void RequestDone();

        GetLastErrorResult GetLastError();

        MongoDatabase this[string databaseName]
        {
            get;
        }

    }
}
