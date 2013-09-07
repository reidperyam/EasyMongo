using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyMongo;
using EasyMongo.Async;
using MongoDB.Driver;
using NUnit.Framework;
using System.Threading;
using EasyMongo.Contract;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EasyMongo.Async.Test
{
    /// <summary>
    /// TODO: use shared class instead of rewriting within every test assembly
    /// </summary>
    [TestFixture]
    public abstract class TestBase
    {
        #region   Setup
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {

        }

        [SetUp]
        public void Setup()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoServerConnection.Connect();
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();
            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);
            _mongoReaderAsync = new ReaderAsync<TestEntry>(_mongoReader);
            _mongoReaderAsync.AsyncReadCompleted += new ReadCompletedEvent<TestEntry>(_mongoReaderAsync_ReadCompleted);
            _mongoReaderAsync.AsyncDistinctCompleted += new DistinctCompletedEvent(_mongoReaderAsync_AsyncDistinctCompleted);
            _mongoWriterAsync = new WriterAsync<TestEntry>(_mongoWriter);
            _mongoWriterAsync.AsyncWriteCompleted += new WriteCompletedEvent(_mongoWriterAsync_WriteCompletedEvent);
            _mongoUpdater = new Updater<TestEntry>(_mongoDatabaseConnection);
            _mongoUpdaterAsync = new UpdaterAsync<TestEntry>(_mongoUpdater);
            _mongoUpdaterAsync.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_mongoUpdaterAsync_AsyncFindAndModifyCompleted);
            _mongoUpdaterAsync.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_mongoUpdaterAsync_AsyncFindAndRemoveCompleted);

            //assure that all dbs are removed as upon 
            _mongoServerConnection.DropDatabase("local");

            _beforeTest = DateTime.Now;
            Assert.AreEqual(0, _results.Count());
            Assert.IsNull(_updaterResult);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoServerConnection.Connect();
            _mongoServerConnection.DropDatabases(new List<string>() {MONGO_DATABASE_1_NAME, MONGO_DATABASE_2_NAME, MONGO_DATABASE_3_NAME });
            _results.Clear();
            _asyncReadResults.Clear();
            _updaterResult = null;
            _WriteConcernResult = null;
            _asyncException = null;
            _serverConnectionResult = ConnectionResult.Empty;
            _databaseConnectionResult = ConnectionResult.Empty;
            _databaseConnectionReturnMessage = string.Empty;
            _serverConnectionReturnMessage = string.Empty;
            _asyncDistinctResults.Clear();
            _serverConnectionAutoResetEvent.Reset();
            _databaseConnectionAutoResetEvent.Reset();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }
        #endregion Setup

        protected IServerConnection              _mongoServerConnection;
        protected IDatabaseConnection<TestEntry> _mongoDatabaseConnection;
        protected IReader<TestEntry>             _mongoReader;
        protected IReaderAsync<TestEntry>        _mongoReaderAsync;
        protected IWriter<TestEntry>             _mongoWriter;
        protected IWriterAsync<TestEntry>        _mongoWriterAsync;
        protected IUpdater<TestEntry>            _mongoUpdater;
        protected IUpdaterAsync<TestEntry>       _mongoUpdaterAsync;

        protected DateTime        _beforeTest;
        protected WriteConcern    _writeConcern          = WriteConcern.Acknowledged;
        protected List<TestEntry> _results               = new List<TestEntry>();
        protected List<TestEntry> _asyncReadResults      = new List<TestEntry>();
        protected List<BsonValue> _asyncDistinctResults  = new List<BsonValue>();
        protected Exception       _asyncException        = null;
        protected FindAndModifyResult _updaterResult     = null;
        protected WriteConcernResult  _WriteConcernResult = null;
        protected ConnectionResult _serverConnectionResult   = ConnectionResult.Empty;
        protected ConnectionResult _databaseConnectionResult = ConnectionResult.Empty;
        protected AutoResetEvent  _readerAutoResetEvent  = new AutoResetEvent(false);
        protected AutoResetEvent  _writerAutoResetEvent  = new AutoResetEvent(false);
        protected AutoResetEvent  _updaterAutoResetEvent = new AutoResetEvent(false);
        protected AutoResetEvent  _serverConnectionAutoResetEvent = new AutoResetEvent(false);
        protected AutoResetEvent  _databaseConnectionAutoResetEvent = new AutoResetEvent(false);
        protected string          _serverConnectionReturnMessage = string.Empty;
        protected string          _databaseConnectionReturnMessage = string.Empty;

        protected const string MONGO_CONNECTION_STRING_BAD = "mongodb://67.190.39.219";
        protected const string MONGO_CONNECTION_STRING     = "mongodb://localhost";
        protected const string MONGO_DATABASE_1_NAME       = "TEST_DB_1";
        protected const string MONGO_DATABASE_2_NAME       = "TEST_DB_2";
        protected const string MONGO_DATABASE_3_NAME       = "TEST_DB_3"; 
        protected const string MONGO_COLLECTION_1_NAME     = "MONGO_READER_TESTS";
        protected const string MONGO_COLLECTION_2_NAME     = "MONGO_READER_TESTS_2";
        protected const string MONGO_EDITED_TEXT           = "EDITED";

        #region    Helper Methods
        /// <summary>
        /// Method useful for synchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoWriter class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntry(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            TestEntry mongoEntry = new TestEntry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;
            _mongoWriter.Write(collectionName, mongoEntry);
        }

        /// <summary>
        ///  Method useful for asynchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoWriterAsync class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryAsync(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            TestEntry mongoEntry = new TestEntry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;
            
            _mongoWriterAsync.WriteAsync(collectionName, mongoEntry);
            _writerAutoResetEvent.WaitOne();
        }

        protected void ServerConnectionAsync()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _serverConnectionAutoResetEvent.WaitOne();
        }

        protected void DatabaseConnectionAsync()
        {
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);
            _databaseConnectionAutoResetEvent.WaitOne();
        }

        /// <summary>
        /// Event handler for MongoWriterAsync.WriteCompleted ; invoked after asynchronous writing has completed
        /// we use it here to signal the AutoResetEvent that writing has completed and that the blocked thread 
        /// can continue
        /// </summary>
        /// <param name="sender">null</param>
        void _mongoWriterAsync_WriteCompletedEvent(object sender)
        {
            _writerAutoResetEvent.Set();// allow the thread in AddMongoEntryAsync to continue
        }

        /// <summary>
        /// Event handler for MongoReaderAsync.ReadCompleted ; invoked after asynchronous reading has completed
        /// we use it here to signal the AutoResetEvent that reading has completed and that the blocked thread 
        /// can continue
        /// </summary>
        /// <param name="sender">null</param>
        /// <param name="e">The results from the read of the MongoDB</param>
        protected void _mongoReaderAsync_ReadCompleted(IEnumerable<TestEntry> e, Exception ex)
        {
            _asyncException = ex;

            if(e != null)
                _asyncReadResults.AddRange(e);

            _readerAutoResetEvent.Set();
        }

        protected void _mongoReaderAsync_AsyncDistinctCompleted(IEnumerable<BsonValue> e, Exception ex)
        {
            _asyncException = ex;

            if (e != null)
                _asyncDistinctResults.AddRange(e);

            _readerAutoResetEvent.Set();
        }

        protected void _mongoUpdaterAsync_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            _WriteConcernResult = result;
            _updaterAutoResetEvent.Set();
        }

        protected void _mongoUpdaterAsync_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            _updaterResult = result;
            _updaterAutoResetEvent.Set();
        }

        protected void _mongoServerConnection_Connected(ConnectionResult result, string message)
        {
            _serverConnectionReturnMessage = message;
            _serverConnectionResult = result;
            _serverConnectionAutoResetEvent.Set();         
        }

        protected void _mongoDatabaseConnection_Connected(ConnectionResult result, string message)
        {        
            _databaseConnectionReturnMessage = message;
            _databaseConnectionResult = result;
            _databaseConnectionAutoResetEvent.Set();
        }
        #endregion Helper Methods
    }
}
