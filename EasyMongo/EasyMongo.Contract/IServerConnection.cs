using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;

namespace EasyMongo.Contract
{
    public enum ConnectionResult : uint { Empty, Success, Failure }

    public interface IServerConnection
    {
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

        void Connect();

        void ConnectAsyncDelegate(Action<ConnectionResult,string> callback);

        void ConnectAsyncTask();

        List<CommandResult> DropAllDatabases();

        CommandResult DropDatabase(MongoDatabase mongoDatabase);

        CommandResult DropDatabase(string mongoDatabaseName);

        List<CommandResult> DropDatabases(IEnumerable<MongoDatabase> dbsToDrop);

        List<CommandResult> DropDatabases(IEnumerable<string> dbsToDrop);

        MongoDatabase GetDatabase(string databaseName, WriteConcern writeConcern);

        MongoDatabase this[string databaseName]
        {
            get;
        }
    }
}
