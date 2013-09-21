using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson.Serialization;
using EasyMongo.Contract;
using System.Threading;
using System.Runtime.Remoting.Messaging;

namespace EasyMongo
{
    /// <summary>
    /// TODO: BsonClassMap.RegisterClassMap<T>(); investigation -- is this thing needed?
    /// </summary>
    public class DatabaseConnection : IDatabaseConnection
    {
        static readonly object _object = new object();
        //private event ConnectAsyncCompletedEvent ConnectAsyncCompleted;
        private event Action<ConnectionResult,string> ConnectAsyncCompleted;
        private AutoResetEvent _serverConnectionResetEvent = new AutoResetEvent(false);
        private AutoResetEvent _databaseConnectionResetEvent = new AutoResetEvent(false);
        protected delegate MongoDatabase AsyncConnectDelegate();
        protected AsyncConnectDelegate ConnectAsyncDelegatePointer;
        private string _dbName;

        public DatabaseConnection(IServerConnection serverConnection, string databaseName) 
        {
            ConnectionState = ConnectionState.NotConnected;
            _dbName = databaseName;
            ConnectAsyncDelegatePointer = ConnectMongoDatabaseAsync;
            MongoServerConnection = serverConnection;
        }

        /// <summary>
        /// Synchronous connection
        /// </summary>
        public void Connect()
        {
            try
            {
                ConnectionState = ConnectionState.Connecting;
                Db = null;
                _databaseConnectionResetEvent.Reset();

                // if the underlying MongoServerConnection is not connected, connect it since we 
                // depend on it to execute queries
                if (MongoServerConnection.ConnectionState == ConnectionState.NotConnected)
                    MongoServerConnection.Connect();

                // if the database already exists than it is fetched otherwise created
                Db = MongoServerConnection.GetDatabase(_dbName, WriteConcern.Acknowledged);
                ConnectionState = ConnectionState.Connected;              
            }
            catch (Exception ex)
            {
                ConnectionState = ConnectionState.NotConnected;
            }
        }

        public void ConnectAsync(Action<ConnectionResult,string> callback)
        {
            ConnectAsyncCompleted += callback;// new ConnectAsyncCompletedEvent(callback);

            ConnectionState = ConnectionState.Connecting;
            Db = null;
            _serverConnectionResetEvent.Reset();
            _databaseConnectionResetEvent.Reset();
         
            // invoke GetMongoDatabase() asynchronously via delegate BeginInvoke
            ConnectAsyncDelegatePointer.BeginInvoke(AsyncConnectCallback, null);
        }

        private MongoDatabase ConnectMongoDatabaseAsync()
        {
            try
            {
                // if the underlying server connection is not connected...
                if (MongoServerConnection.ConnectionState == ConnectionState.NotConnected)
                {
                    // connect to the server asynchronously
                    MongoServerConnection.ConnectAsync(serverConnection_AsyncConnectionCompleted);

                    // but wait for the connected event call back to notify us that the server is connected
                    // before proceeding
                    _serverConnectionResetEvent.WaitOne();
                    _serverConnectionResetEvent.Reset();
                }
                else while(MongoServerConnection.ConnectionState == ConnectionState.Connecting)  
                {
                    ;
                    // spin until the server gets connected
                    // this case should only happen when a server connection asynchronously is connecting, 
                    // and then a DatabaseConnection is created using it
                }
 
                // if DatabaseConnection failed to connect this will raise an exception
                return MongoServerConnection.GetDatabase(_dbName, WriteConcern.Acknowledged);
            }
            catch (Exception ex)
            {
                // probably illegal conn string or db name
                throw new MongoDatabaseConnectionException("Error connecting to " + _dbName + " database", ex);
            }
        }

        /// <summary>
        /// Call back method for ServerConnection.Connected event
        /// </summary>
        void serverConnection_AsyncConnectionCompleted(ConnectionResult result, string message)
        {            
            // notify our separate thread that the ServerConnection is connected
            _serverConnectionResetEvent.Set();

            //TODO: if result == failure -- do something to graciously fail
        }

        private void AsyncConnectCallback(IAsyncResult ar)
        {
            string returnMessage = string.Empty;

            try
            {
                // Retrieve the delegate.
                AsyncResult result = (AsyncResult)ar;
                AsyncConnectDelegate caller = (AsyncConnectDelegate)result.AsyncDelegate;
                // if the database already exists than it is fetched otherwise created

                // Call EndInvoke to retrieve the results. 
                Db = caller.EndInvoke(ar);

                if (Db == null)
                {
                    throw new MongoServerConnectionException("Asynchronous server connection failed");
                }

                ConnectionState = ConnectionState.Connected;
                returnMessage = "Successful connection";
            }
            catch (MongoDatabaseConnectionException mdcx)
            {
                ConnectionState = ConnectionState.NotConnected;
                returnMessage = mdcx.Message;
            }
            catch (Exception ex)
            {
                //TODO: add additional exception handling

                ConnectionState = ConnectionState.NotConnected;
                returnMessage = ex.Message;
            }
            finally
            {
                _databaseConnectionResetEvent.Set(); // allow dependent process that are waiting via VerifyConnected() to proceed

                if( ConnectionState == ConnectionState.NotConnected)
                {
                    if (ConnectAsyncCompleted != null)
                        ConnectAsyncCompleted(ConnectionResult.Failure, returnMessage);
                }
                else if (ConnectionState == ConnectionState.Connected)
                {
                    if (ConnectAsyncCompleted != null)
                        ConnectAsyncCompleted(ConnectionResult.Success, returnMessage);
                }
                else
                {
                    if (ConnectAsyncCompleted != null)
                        ConnectAsyncCompleted(ConnectionResult.Failure, returnMessage);
                }
            }
        }

        private ConnectionState _connectionState = ConnectionState.NotConnected;
        public ConnectionState ConnectionState
        {
            get
            {
                return _connectionState;
            }
            private set
            {
                lock (_object)
                {
                    _connectionState = value;
                }
            }
        }

        public IServerConnection MongoServerConnection
        {
            get;
            protected set;
        }

        public MongoDatabase Db
        {
            get;
            set;
        }

        public MongoCollection<T> GetCollection<T>(string collectionName)
        {
            VerifyConnected();
            return Db.GetCollection<T>(collectionName);
        }

        public List<MongoCollection<T>> GetCollections<T>()
        {
            VerifyConnected();
            List<MongoCollection<T>> toReturn = new List<MongoCollection<T>>();

            foreach (string collectionName in GetCollectionNames())
            {
                toReturn.Add(GetCollection<T>(collectionName));
            }

            return toReturn;
        }

        public List<string> GetCollectionNames()
        {
            VerifyConnected();
            List<string> toReturn;
            toReturn = new List<string>( Db.GetCollectionNames());
            return toReturn;
        }

        public void ClearCollection<T>(string collectionName)
        {
            VerifyConnected();
            var collection = GetCollection<T>(collectionName);
            WriteConcernResult result = collection.RemoveAll(WriteConcern.Acknowledged);
        }

        public void ClearAllCollections<T>()
        {
            VerifyConnected();
            List<string> connectionNames = GetCollectionNames();
            connectionNames.ForEach(delegate(string collectionName)
            {
                if (collectionName != "system.indexes")
                    ClearCollection<T>(collectionName);
            });
        }

        public void DropCollection<T>(string collectionName)
        {
            VerifyConnected();
            var collection = GetCollection<T>(collectionName);
            collection.Drop();
        }

        public void DropAllCollections<T>()
        {
            VerifyConnected();
            List<string> connectionNames = GetCollectionNames();
            connectionNames.ForEach(delegate(string collectionName)
            {
                if(collectionName != "system.indexes")
                    DropCollection<T>(collectionName);
            });
        }

        public bool CanConnect()
        {
            return MongoServerConnection.CanConnect();
        }

        public MongoDatabaseSettings CreateDatabaseSettings()
        {
            return MongoServerConnection.CreateDatabaseSettings(Db.Name);
        }

        public void CopyDatabase(string to)
        {
            MongoServerConnection.CopyDatabase(Db.Name, to);
        }

        public IDisposable RequestStart(MongoServerInstance mongoServerInstance)
        {
            return MongoServerConnection.RequestStart(Db, mongoServerInstance);
        }

        public IDisposable RequestStart(bool slaveOk)
        {
            return MongoServerConnection.RequestStart(Db, slaveOk);
        }

        public void RequestDone()
        {
            MongoServerConnection.RequestDone();
        }

        public GetLastErrorResult GetLastError()
        {
            return MongoServerConnection.GetLastError();
        }

        // Define the indexer, which will allow client code 
        // to use [] notation on the class instance itself.         
        public MongoCollection this[string collectionName]
        {
            get
            {
                // This indexer is very simple, and just returns 
                // the corresponding collection from the internal array. 
                return MongoServerConnection[Db.Name][collectionName];
            }
        }

        private void VerifyConnected()
        {
            do
            {
                switch (ConnectionState)
                {
                    case ConnectionState.Connected: break;
                    case ConnectionState.Connecting: _databaseConnectionResetEvent.WaitOne(); // wait for the DatabaseConnection to connect
                        break;
                    case ConnectionState.NotConnected: /*Connect();*/ //break;
                    throw new MongoConnectionException("DatabaseConnection is not connected");
                }
            } while (ConnectionState != ConnectionState.Connected);
        }
    }
}
