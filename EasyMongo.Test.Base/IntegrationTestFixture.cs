using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Async;
using Ninject;
using Ninject.Extensions.EasyMongo;
using EasyMongo.Test.Model;

namespace EasyMongo.Test.Base
{
    /// <summary>
    /// Contains shared code for creating, managing server, database connection objects used for creating and destroying objects against
    /// a locally executing MongoDB for the purpose of integration testing
    /// </summary>
    [TestFixture]
    public abstract class IntegrationTestFixture
    {
        public const string MONGO_CONNECTION_STRING     = "mongodb://localhost";
        public const string MONGO_CONNECTION_STRING_BAD = "mongodb://67.190.39.219";
        public const string MONGO_DATABASE_1_NAME       = "TEST_DB_1";
        public const string MONGO_DATABASE_2_NAME       = "TEST_DB_2";
        public const string MONGO_DATABASE_3_NAME       = "TEST_DB_3";
        public const string MONGO_COLLECTION_1_NAME     = "MONGO_READER_TESTS";
        public const string MONGO_COLLECTION_2_NAME     = "MONGO_READER_TESTS_2";
        public const string MONGO_EDITED_TEXT           = "EDITED";

        #region   Setup
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
           // System.Diagnostics.Debugger.Launch();
            // binding logic for Ninject happens here
            _configurator = new Configurator();   
        }

        [SetUp]
        public void Setup()
        {
            _mongoServerConnection   = _configurator.Kernel.TryGet<IServerConnection>();
            _mongoDatabaseConnection = _configurator.Kernel.TryGet<IDatabaseConnection>();
            _mongoDatabaseConnection.Connect();
 
            #region    EasyMongo.Test
            _reader  = _configurator.Kernel.TryGet<IReader>();
            _writer  = _configurator.Kernel.TryGet<IWriter>();
            _updater = _configurator.Kernel.TryGet<IUpdater>();
            #endregion EasyMongo.Test

            #region    EasyMongo.Async.Test
            _readerAsync = _configurator.Kernel.TryGet<IReaderAsync>();
            _readerAsync.AsyncReadCompleted += new ReadCompletedEvent(_reader_AsyncReadCompleted);
            _readerAsync.AsyncDistinctCompleted += new DistinctCompletedEvent(_readerAsync_AsyncDistinctCompleted);

            _writerAsync = _configurator.Kernel.TryGet<IWriterAsync>();
            _writerAsync.AsyncWriteCompleted += new WriteCompletedEvent(_writer_AsyncWriteCompleted);

            _updaterAsync = _configurator.Kernel.TryGet<IUpdaterAsync>();
            _updaterAsync.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_updaterAsync_AsyncFindAndModifyCompleted);
            _updaterAsync.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_updaterAsync_AsyncFindAndRemoveCompleted);
            #endregion EasyMongo.Async.Test

            #region    EasyMongo.Database.Test
            _databaseReader = _configurator.Kernel.TryGet<IDatabaseReader>();
            _databaseReader.AsyncReadCompleted += new ReadCompletedEvent(_databaseReader_AsyncReadCompleted);
            _databaseReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_databaseReader_AsyncDistinctCompleted);

            _databaseWriter = _configurator.Kernel.TryGet<IDatabaseWriter>();
            _databaseWriter.AsyncWriteCompleted += new WriteCompletedEvent(_databaseWriter_AsyncWriteCompleted);

            _databaseUpdater = _configurator.Kernel.TryGet<IDatabaseUpdater>();
            _databaseUpdater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_databaseUpdater_AsyncFindAndModifyCompleted);
            _databaseUpdater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_databaseUpdater_AsyncFindAndRemoveCompleted);
            #endregion EasyMongo.Database.Test

            #region    EasyMongo.Collection.Test
            _collectionReader = _configurator.Kernel.TryGet<ICollectionReader>();
            _collectionReader.AsyncReadCompleted += new ReadCompletedEvent(_collectionReader_AsyncReadCompleted);
            _collectionReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_collectionReader_AsyncDistinctCompleted);

            _collectionWriter = _configurator.Kernel.TryGet<ICollectionWriter>();
            _collectionWriter.AsyncWriteCompleted += new WriteCompletedEvent(_collectionWriter_AsyncWriteCompleted);

            _collectionUpdater = _configurator.Kernel.TryGet<ICollectionUpdater>();
            _collectionUpdater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_collectionUpdater_AsyncFindAndModifyCompleted);
            _collectionUpdater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_collectionUpdater_AsyncFindAndRemoveCompleted);
            #endregion EasyMongo.Collection.Test

            _beforeTest = DateTime.Now;
            Assert.AreEqual(0, _results.Count());
            Assert.IsNull(_findAndModifyResult);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoServerConnection   = _configurator.Kernel.TryGet<IServerConnection>();
            _mongoDatabaseConnection = _configurator.Kernel.TryGet<IDatabaseConnection>();
            _mongoDatabaseConnection.Connect();
            _mongoDatabaseConnection.ClearAllCollections<TestEntry>();
            _mongoServerConnection.Connect();
            _mongoServerConnection.DropDatabases(new List<string>() { MONGO_DATABASE_1_NAME, MONGO_DATABASE_2_NAME, MONGO_DATABASE_3_NAME });

            _results.Clear();
            _asyncReadResults.Clear();
            _asyncDistinctResults.Clear();
            _readerAutoResetEvent.Reset();
            _findAndModifyResult = null;
            _writeConcernResult = null;
            _asyncException = null;

            _serverConnectionResult = ConnectionResult.Empty;
            _databaseConnectionResult = ConnectionResult.Empty;
            _databaseConnectionReturnMessage = string.Empty;
            _serverConnectionReturnMessage = string.Empty;
            _asyncDistinctResults.Clear();

            _serverConnectionAutoResetEvent.Reset();
            _databaseConnectionAutoResetEvent.Reset();
            _updaterAutoResetEvent.Reset();
            _readerAutoResetEvent.Reset();
            _writerAutoResetEvent.Reset();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }
        #endregion Setup

        protected IServerConnection   _mongoServerConnection;
        protected IDatabaseConnection _mongoDatabaseConnection;

        protected IWriter _writer;
        protected IUpdater _updater;
        protected IReader _reader;

        protected IReaderAsync _readerAsync;
        protected IWriterAsync _writerAsync;
        protected IUpdaterAsync _updaterAsync;

        protected IDatabaseReader _databaseReader;
        protected IDatabaseWriter _databaseWriter;
        protected IDatabaseUpdater _databaseUpdater;

        protected ICollectionReader _collectionReader;
        protected ICollectionWriter _collectionWriter;
        protected ICollectionUpdater _collectionUpdater;

        protected DateTime              _beforeTest;
        protected WriteConcern          _writeConcern                     = WriteConcern.Acknowledged;
        protected List<IEasyMongoEntry> _results                          = new List<IEasyMongoEntry>();
        protected List<IEasyMongoEntry> _asyncReadResults                 = new List<IEasyMongoEntry>();
        protected List<BsonValue>       _asyncDistinctResults             = new List<BsonValue>();
        protected Exception             _asyncException                   = null;
        protected FindAndModifyResult   _findAndModifyResult              = null;
        protected WriteConcernResult    _writeConcernResult               = null;
        protected ConnectionResult      _serverConnectionResult           = ConnectionResult.Empty;
        protected ConnectionResult      _databaseConnectionResult         = ConnectionResult.Empty;
        protected AutoResetEvent        _readerAutoResetEvent             = new AutoResetEvent(false);
        protected AutoResetEvent        _writerAutoResetEvent             = new AutoResetEvent(false);
        protected AutoResetEvent        _updaterAutoResetEvent            = new AutoResetEvent(false);
        protected AutoResetEvent        _serverConnectionAutoResetEvent   = new AutoResetEvent(false);
        protected AutoResetEvent        _databaseConnectionAutoResetEvent = new AutoResetEvent(false);
        protected string                _serverConnectionReturnMessage    = string.Empty;
        protected string                _databaseConnectionReturnMessage  = string.Empty;
        protected Configurator          _configurator;

        #region    Helper Methods
        protected void ServerConnectionAsync()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _serverConnectionAutoResetEvent.WaitOne();
        }

        protected void DatabaseConnectionAsync()
        {
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);
            _databaseConnectionAutoResetEvent.WaitOne();
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
            _writer.Write(collectionName, mongoEntry);
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

            _writerAsync.WriteAsync(collectionName, mongoEntry);
            _writerAutoResetEvent.WaitOne();
        }

        #region    EasyMongo.Async
        protected void _updaterAsync_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            _writeConcernResult = result;
            _updaterAutoResetEvent.Set();
        }

        protected void _updaterAsync_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            _findAndModifyResult = result;
            _updaterAutoResetEvent.Set();
        }

        protected void _writer_AsyncWriteCompleted(object sender)
        {
            _writerAutoResetEvent.Set();// allow the thread in AddMongoEntryAsync to continue
        }

        protected void _reader_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<TestEntry> results = (IEnumerable<TestEntry>)e;
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }

        protected void _reader_AsyncDistinctCompleted(IEnumerable<BsonValue> e, Exception ex)
        {
            _asyncException = ex;
            _asyncDistinctResults.AddRange(e);
            _readerAutoResetEvent.Set();
        }

        protected void _readerAsync_AsyncDistinctCompleted(IEnumerable<BsonValue> e, Exception ex)
        {
            _asyncException = ex;

            if (e != null)
                _asyncDistinctResults.AddRange(e);

            _readerAutoResetEvent.Set();
        }
        #endregion EasyMongo.Async

        #region    EasyMongo.Database
        protected void _databaseReader_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<TestEntry> results = (IEnumerable<TestEntry>)e;
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }

        protected void _databaseReader_AsyncDistinctCompleted(IEnumerable<BsonValue> e, Exception ex)
        {
            _asyncException = ex;
            _asyncDistinctResults.AddRange(e);
            _readerAutoResetEvent.Set();
        }

        protected void _databaseWriter_AsyncWriteCompleted(object sender)
        {
            _writerAutoResetEvent.Set();// allow the thread in AddMongoEntryAsync to continue
        }

        protected void _databaseUpdater_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            _writeConcernResult = result;
            _updaterAutoResetEvent.Set();
        }

        protected void _databaseUpdater_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            _findAndModifyResult = result;
            _updaterAutoResetEvent.Set();
        }
        #endregion EasyMongo.Database

        #region EasyMongo.Collection
        protected void _collectionWriter_AsyncWriteCompleted(object sender)
        {
            _writerAutoResetEvent.Set();// allow the thread in AddMongoEntryAsync to continue
        }

        protected void _collectionReader_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<TestEntry> results = (IEnumerable<TestEntry>)e;
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }

        protected void _collectionReader_AsyncDistinctCompleted(IEnumerable<BsonValue> e, Exception ex)
        {
            _asyncException = ex;
            _asyncDistinctResults.AddRange(e);
            _readerAutoResetEvent.Set();
        }

        protected void _collectionUpdater_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            _writeConcernResult = result;
            _updaterAutoResetEvent.Set();
        }

        protected void _collectionUpdater_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            _findAndModifyResult = result;
            _updaterAutoResetEvent.Set();
        }
        #endregion EasyMongo.Collection

        #endregion Helper Methods
    }
}
