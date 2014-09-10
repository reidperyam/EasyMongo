using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using EasyMongo.Contract;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace EasyMongo
{
    public class ServerConnection : IServerConnection 
    {
        private event Action<ConnectionResult,string> ConnectAsyncCompleted;
        static readonly object _object = new object();

        private MongoServer _mongoServer;
        public AutoResetEvent _serverConnectionResetEvent = new AutoResetEvent(false);

        protected delegate MongoServer AsyncConnectDelegate();
        protected AsyncConnectDelegate CreateMongoServerAsyncDelegatePointer;

        public ServerConnection(string connectionString)
        {
            ConnectionString = connectionString;

            CreateMongoServerAsyncDelegatePointer = ConnectMongoServerAsync;
        }

        /// <summary>
        /// Synchronous connection. This method or <see cref="ConnectAsyncDelegate"/> must be called prior to utilizing the ServerConnection.
        /// </summary>
        public void Connect()
        {          
            _mongoServer = null;
            MongoClient client = new MongoClient(ConnectionString);
            _mongoServer = client.GetServer();
        }

        /// <summary>
        /// Asynchronous connection. This method or <see cref="Connect"/> must be called prior to utilizing the ServerConnection.
        /// </summary>
        /// <param name="callback">The method to invoke to handle the ConnectAsyncCompleted event when it is invoked</param>
        public void ConnectAsyncDelegate(Action<ConnectionResult,string> callback)
        {
            ConnectAsyncCompleted += callback;// new ConnectAsyncCompletedEvent(callback);
            _mongoServer = null;
            _serverConnectionResetEvent.Reset();
            CreateMongoServerAsyncDelegatePointer.BeginInvoke(AsyncConnectCallback, null);
        }

        public async void ConnectAsyncTask()
        {
            _mongoServer = null;
            MongoClient client = new MongoClient(ConnectionString);
            _mongoServer = await Task.Run(() => client.GetServer());
        }

        /// <summary>
        /// This method is executed asynchronously via AsyncConnectDelegate.BeginInvoke
        /// </summary>
        /// <remarks>This is useful for non-blocking creation of a MongoServer</remarks>
        /// <returns>MongoServer from ConnectionString</returns>
        private MongoServer ConnectMongoServerAsync()
        {
            try
            {
                MongoClient client = new MongoClient(ConnectionString);
                MongoServer server = client.GetServer();
                server.Connect();
                return server;
            }
            catch (MongoConnectionException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Call back method invoked after CreateMongoServerAsync() has completed. Invokes the Connected event to
        /// notify subscribers that asynchronous server connection has completed and ServerConnection is ready for use.
        /// </summary>
        /// <param name="ar"></param>
        private void AsyncConnectCallback(IAsyncResult ar)
        {
            string returnMessage = string.Empty;

            try
            {
                // Retrieve the delegate.
                AsyncResult result = (AsyncResult)ar;
                AsyncConnectDelegate caller = (AsyncConnectDelegate)result.AsyncDelegate;

                // Call EndInvoke to retrieve the results. 
                _mongoServer = caller.EndInvoke(ar);

                if (_mongoServer == null)
                {
                    returnMessage = "Failure";
                }
                else
                {
                    returnMessage = "Successful";
                }
            }
            catch (MongoDatabaseConnectionException mdcx)
            {
                returnMessage = mdcx.Message;
            }
            catch (Exception ex)
            {
                returnMessage = ex.Message;
            }
            finally
            {
                _serverConnectionResetEvent.Set(); // allow dependent process that are waiting via VerifyConnected() to proceed

                if (_mongoServer == null)
                {
                    if (ConnectAsyncCompleted != null)
                        ConnectAsyncCompleted(ConnectionResult.Failure, returnMessage);
                }
                else
                {
                    if (ConnectAsyncCompleted != null)
                        ConnectAsyncCompleted(ConnectionResult.Success, returnMessage);
                }
            }
        }

        public string ConnectionString
        {
            get;
            set;
        }

        public List<string> GetDbNamesForConnection()
        {
            VerifyConnected();
            List<string> dbs = new List<string>();

            foreach (string dbName in _mongoServer.GetDatabaseNames())
                dbs.Add(dbName);

            return dbs;
        }

        public List<CommandResult> DropAllDatabases()
        {
            VerifyConnected();
            return DropDatabases(GetDbNamesForConnection());
        }

        public CommandResult DropDatabase(MongoDatabase mongoDatabase)
        {
            VerifyConnected();
            return DropDatabase(mongoDatabase.Name);
        }

        public List<CommandResult> DropDatabases(IEnumerable<string> dbsToDrop)
        {
            VerifyConnected();
            List<CommandResult> toReturn = new List<CommandResult>();

            foreach (string dbName in dbsToDrop)
            {
                CommandResult commandResult = DropDatabase(dbName);
                toReturn.Add(commandResult);
            }

            return toReturn;
        }

        public List<CommandResult> DropDatabases(IEnumerable<MongoDatabase> dbsToDrop)
        {
            VerifyConnected();
            List<CommandResult> toReturn = new List<CommandResult>();

            foreach (MongoDatabase mongoDatabase in dbsToDrop)
            {
                CommandResult commandResult = DropDatabase(mongoDatabase.Name);
                toReturn.Add(commandResult);
            }

            return toReturn;
        }

        private void VerifyConnected()
        {
            switch (State)
            {
                case MongoServerState.Connected: break;
                case MongoServerState.Connecting: _serverConnectionResetEvent.WaitOne(); // wait for the ServerConnection to connect
                    break;
                case MongoServerState.Disconnected: throw new MongoConnectionException("DatabaseConnection is not connected");
            }
        }

        public MongoServerSettings Settings
        {
            get
            {
                return _mongoServer.Settings;
            }
        }

        public MongoServerState State
        {
            get
            {
                if (_mongoServer == null)
                    return MongoServerState.Disconnected;
                else
                    return _mongoServer.State;
            }
        }

        #region   MongoDB Driver pass-throughs
        public CommandResult DropDatabase(string mongoDatabaseName)
        {
            return _mongoServer.DropDatabase(mongoDatabaseName);
        }

        public MongoDatabase GetDatabase(string databaseName, WriteConcern writeConcern)
        {
            return _mongoServer.GetDatabase(databaseName, writeConcern);
        }

        public IDisposable RequestStart(MongoDatabase mongoDatabase)
        {
            return _mongoServer.RequestStart(mongoDatabase);
        }

        public IDisposable RequestStart(MongoDatabase mongoDatabase, MongoServerInstance mongoServerInstance)
        {
            return _mongoServer.RequestStart(mongoDatabase, mongoServerInstance);
        }

        public IDisposable RequestStart(MongoDatabase mongoDatabase, ReadPreference readPreference)
        {
            return _mongoServer.RequestStart(mongoDatabase, readPreference);
        }

        public void RequestDone()
        {
            _mongoServer.RequestDone();
        }

        public GetLastErrorResult GetLastError()
        {
            return _mongoServer.GetLastError();
        }
        #endregion   MongoDB Driver pass-throughs

        // Define the indexer, which will allow client code 
        // to use [] notation on the class instance itself.         
        public MongoDatabase this[string databaseName]
        {
            get
            {
                // This indexer is very simple, and just returns 
                // the corresponding database from the internal array. 
                return _mongoServer.GetDatabase(databaseName);
            }
        }
    }
}
