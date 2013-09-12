using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Async;

namespace EasyMongo.Base.Test
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
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            #region    EasyMongo.Test
            _reader  = new Reader<TestEntry>(_mongoDatabaseConnection);
            _writer  = new Writer<TestEntry>(_mongoDatabaseConnection);
            _updater = new Updater<TestEntry>(_mongoDatabaseConnection);
            #endregion EasyMongo.Test

            #region    EasyMongo.Async.Test
            _readerAsync = new ReaderAsync<TestEntry>(_reader);
            _readerAsync.AsyncReadCompleted += new ReadCompletedEvent<TestEntry>(_reader_AsyncReadCompleted);
            _readerAsync.AsyncDistinctCompleted += new DistinctCompletedEvent(_readerAsync_AsyncDistinctCompleted);

            _writerAsync = new WriterAsync<TestEntry>(_writer);
            _writerAsync.AsyncWriteCompleted += new WriteCompletedEvent(_writer_AsyncWriteCompleted);

            _updaterAsync = new UpdaterAsync<TestEntry>(_updater);
            _updaterAsync.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_updaterAsync_AsyncFindAndModifyCompleted);
            _updaterAsync.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_updaterAsync_AsyncFindAndRemoveCompleted);
            #endregion EasyMongo.Async.Test

            #region    EasyMongo.Database.Test
            _databaseReader = new Database.Reader<TestEntry>(MONGO_CONNECTION_STRING, MONGO_DATABASE_1_NAME);
            _databaseReader.AsyncReadCompleted += new ReadCompletedEvent<TestEntry>(_databaseReader_AsyncReadCompleted);
            _databaseReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_databaseReader_AsyncDistinctCompleted);

            _databaseWriter = new Database.Writer<TestEntry>(MONGO_CONNECTION_STRING, MONGO_DATABASE_1_NAME);
            _databaseWriter.AsyncWriteCompleted += new WriteCompletedEvent(_databaseWriter_AsyncWriteCompleted);

            _databaseUpdater = new Database.Updater<TestEntry>(MONGO_CONNECTION_STRING, MONGO_DATABASE_1_NAME);
            _databaseUpdater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_databaseUpdater_AsyncFindAndModifyCompleted);
            _databaseUpdater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_databaseUpdater_AsyncFindAndRemoveCompleted);
            #endregion EasyMongo.Database.Test

            #region    EasyMongo.Collection.Test
            _collectionWriter = new Collection.Writer<TestEntry>(_databaseWriter, MONGO_COLLECTION_1_NAME);
            _collectionWriter.AsyncWriteCompleted += new WriteCompletedEvent(_collectionWriter_AsyncWriteCompleted);

            _collectionReader = new Collection.Reader<TestEntry>(_databaseReader, MONGO_COLLECTION_1_NAME);
            _collectionReader.AsyncReadCompleted += new ReadCompletedEvent<TestEntry>(_collectionReader_AsyncReadCompleted);
            _collectionReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_collectionReader_AsyncDistinctCompleted);

            _collectionUpdater = new Collection.Updater<TestEntry>(_databaseUpdater, MONGO_COLLECTION_1_NAME);
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
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();
            _mongoDatabaseConnection.ClearAllCollections();
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

        protected IServerConnection              _mongoServerConnection;
        protected IDatabaseConnection<TestEntry> _mongoDatabaseConnection;

        protected IDatabaseReader<TestEntry> _databaseReader;
        protected IDatabaseWriter<TestEntry> _databaseWriter;
        protected IDatabaseUpdater<TestEntry> _databaseUpdater;

        protected ICollectionReader<TestEntry> _collectionReader;
        protected ICollectionWriter<TestEntry> _collectionWriter;
        protected ICollectionUpdater<TestEntry> _collectionUpdater;

        protected IReader<TestEntry> _reader;
        protected IWriter<TestEntry> _writer;
        protected IUpdater<TestEntry> _updater;

        protected IReaderAsync<TestEntry> _readerAsync;
        protected IWriterAsync<TestEntry> _writerAsync;
        protected IUpdaterAsync<TestEntry> _updaterAsync;

        protected DateTime        _beforeTest;
        protected WriteConcern    _writeConcern          = WriteConcern.Acknowledged;
        protected List<TestEntry> _results               = new List<TestEntry>();
        protected List<TestEntry> _asyncReadResults      = new List<TestEntry>();
        protected List<BsonValue> _asyncDistinctResults  = new List<BsonValue>();
        protected Exception       _asyncException        = null;
        protected FindAndModifyResult _findAndModifyResult = null;
        protected WriteConcernResult _writeConcernResult = null;
        protected ConnectionResult _serverConnectionResult = ConnectionResult.Empty;
        protected ConnectionResult _databaseConnectionResult = ConnectionResult.Empty;
        protected AutoResetEvent  _readerAutoResetEvent  = new AutoResetEvent(false);
        protected AutoResetEvent  _writerAutoResetEvent  = new AutoResetEvent(false);
        protected AutoResetEvent _updaterAutoResetEvent  = new AutoResetEvent(false);
        protected AutoResetEvent _serverConnectionAutoResetEvent = new AutoResetEvent(false);
        protected AutoResetEvent _databaseConnectionAutoResetEvent = new AutoResetEvent(false);
        protected string _serverConnectionReturnMessage = string.Empty;
        protected string _databaseConnectionReturnMessage = string.Empty;

        protected const string MONGO_CONNECTION_STRING_BAD = "mongodb://67.190.39.219";
        protected const string MONGO_CONNECTION_STRING     = "mongodb://localhost";
        protected const string MONGO_DATABASE_1_NAME       = "TEST_DB_1";
        protected const string MONGO_DATABASE_2_NAME       = "TEST_DB_2";
        protected const string MONGO_DATABASE_3_NAME       = "TEST_DB_3";
        protected const string MONGO_COLLECTION_1_NAME     = "MONGO_READER_TESTS";
        protected const string MONGO_COLLECTION_2_NAME     = "MONGO_READER_TESTS_2";
        protected const string MONGO_EDITED_TEXT           = "EDITED";

        #region    Helper Methods
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

        protected void _reader_AsyncReadCompleted(IEnumerable<TestEntry> e, Exception ex)
        {
            _asyncException = ex;
            _asyncReadResults.AddRange(e);
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
        protected void _databaseReader_AsyncReadCompleted(IEnumerable<TestEntry> e, Exception ex)
        {
            _asyncException = ex;
            _asyncReadResults.AddRange(e);
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

        #region    EasyMongo.Collection
        protected void _collectionWriter_AsyncWriteCompleted(object sender)
        {
            _writerAutoResetEvent.Set();// allow the thread in AddMongoEntryAsync to continue
        }

        protected void _collectionReader_AsyncReadCompleted(IEnumerable<TestEntry> e, Exception ex)
        {
            _asyncException = ex;
            _asyncReadResults.AddRange(e);
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
