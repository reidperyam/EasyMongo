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
using System.Threading.Tasks;
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
                Db = null;
                _databaseConnectionResetEvent.Reset();

                // if the underlying MongoServerConnection is not connected, connect it since we 
                // depend on it to execute queries
                if (MongoServerConnection.State == MongoServerState.Disconnected)
                    MongoServerConnection.Connect();

                // if the database already exists than it is fetched otherwise created
                Db = MongoServerConnection.GetDatabase(_dbName, WriteConcern.Acknowledged);              
            }
            catch (Exception ex)
            {
                // TODO: add ex handling!
                throw ex;
            }
        }

        public void ConnectAsyncDelegate(Action<ConnectionResult,string> callback)
        {
            ConnectAsyncCompleted += callback;// new ConnectAsyncCompletedEvent(callback);

            Db = null;
            _serverConnectionResetEvent.Reset();
            _databaseConnectionResetEvent.Reset();
         
            // invoke GetMongoDatabase() asynchronously via delegate BeginInvoke
            ConnectAsyncDelegatePointer.BeginInvoke(AsyncConnectCallback, null);
        }

        public void ConnectAsyncTask()
        {
            Db = null;
            _serverConnectionResetEvent.Reset();
            _databaseConnectionResetEvent.Reset();

            Task.Run(() => Connect());
        }

        private MongoDatabase ConnectMongoDatabaseAsync()
        {
            try
            {
                // if the underlying server connection is not connected...
                if (MongoServerConnection.State == MongoServerState.Disconnected)
                {
                    // connect to the server asynchronously
                    MongoServerConnection.ConnectAsyncDelegate(serverConnection_AsyncConnectionCompleted);

                    // but wait for the connected event call back to notify us that the server is connected
                    // before proceeding
                    _serverConnectionResetEvent.WaitOne();
                    _serverConnectionResetEvent.Reset();
                }
                else while (MongoServerConnection.State == MongoServerState.Disconnected)  
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

                returnMessage = "Successful connection";
            }
            catch (MongoDatabaseConnectionException mdcx)
            {
                returnMessage = mdcx.Message;
            }
            catch (Exception ex)
            {
                //TODO: add additional exception handling
                returnMessage = ex.Message;
            }
            finally
            {
                _databaseConnectionResetEvent.Set(); // allow dependent process that are waiting via VerifyConnected() to proceed

                if (State == MongoServerState.Disconnected)
                {
                    if (ConnectAsyncCompleted != null)
                        ConnectAsyncCompleted(ConnectionResult.Failure, returnMessage);
                }
                else if (State == MongoServerState.Connected)
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

        public MongoServerState State
        {
            get
            {
                return MongoServerConnection.State;
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
            try
            {
                return Db.GetCollection<T>(collectionName);
            }
            catch(NullReferenceException ex)
            {
                throw new MongoConnectionException("DatabaseConnection not connected",ex);
            }
        }

        public List<MongoCollection<T>> GetCollections<T>()
        {
            try
            {
                List<MongoCollection<T>> toReturn = new List<MongoCollection<T>>();

                foreach (string collectionName in GetCollectionNames())
                {
                    toReturn.Add(GetCollection<T>(collectionName));
                }

                return toReturn;
            }
            catch (NullReferenceException ex)
            {
                throw new MongoConnectionException("DatabaseConnection not connected", ex);
            }
        }

        public List<string> GetCollectionNames()
        {
            try
            {
                List<string> toReturn;
                toReturn = new List<string>( Db.GetCollectionNames());
                return toReturn;
            }
            catch (NullReferenceException ex)
            {
                throw new MongoConnectionException("DatabaseConnection not connected", ex);
            }
        }

        public void ClearCollection<T>(string collectionName)
        {
            try
            {
                var collection = GetCollection<T>(collectionName);
                WriteConcernResult result = collection.RemoveAll(WriteConcern.Acknowledged);
            }
            catch (NullReferenceException ex)
            {
                throw new MongoConnectionException("DatabaseConnection not connected", ex);
            }
        }

        public void ClearAllCollections<T>()
        {
            try
            {
                List<string> connectionNames = GetCollectionNames();
                connectionNames.ForEach(delegate(string collectionName)
                {
                    if (collectionName != "system.indexes")
                        ClearCollection<T>(collectionName);
                });
            }
            catch (NullReferenceException ex)
            {
                throw new MongoConnectionException("DatabaseConnection not connected", ex);
            }
        }

        public void DropCollection<T>(string collectionName)
        {
            try
            {
                var collection = GetCollection<T>(collectionName);
                collection.Drop();
            }
            catch (NullReferenceException ex)
            {
                throw new MongoConnectionException("DatabaseConnection not connected", ex);
            }
        }

        public void DropAllCollections<T>()
        {
            try
            {
                List<string> connectionNames = GetCollectionNames();
                connectionNames.ForEach(delegate(string collectionName)
                {
                    if(collectionName != "system.indexes")
                        DropCollection<T>(collectionName);
                });
            }
            catch (NullReferenceException ex)
            {
                throw new MongoConnectionException("DatabaseConnection not connected", ex);
            }
        }

        public IDisposable RequestStart(MongoServerInstance mongoServerInstance)
        {
            return MongoServerConnection.RequestStart(Db, mongoServerInstance);
        }

        public IDisposable RequestStart(ReadPreference readPreference)
        {
            return MongoServerConnection.RequestStart(Db, readPreference);
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
                return MongoServerConnection.GetDatabase(Db.Name,WriteConcern.Acknowledged).GetCollection(collectionName);
            }
        }
    }
}
