using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using NUnit.Framework;
using System.Threading;
using MongoDB.Database;
using MongoDB.Contract;
using MongoDB.Collection;
using MongoDB.Bson;

namespace MongoDB.Collection.Test
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

            IDatabaseWriter<TestEntry> databaseWriter = new MongoDB.Database.Writer<TestEntry>(MONGO_CONNECTION_STRING, MONGO_DATABASE_1_NAME);
            _writer = new MongoDB.Collection.Writer<TestEntry>(databaseWriter, MONGO_COLLECTION_1_NAME);
            _writer.AsyncWriteCompleted += new WriteCompletedEvent(_writer_AsyncWriteCompleted);

            IDatabaseReader<TestEntry> databaseReader = new MongoDB.Database.Reader<TestEntry>(MONGO_CONNECTION_STRING, MONGO_DATABASE_1_NAME);
            _reader = new MongoDB.Collection.Reader<TestEntry>(databaseReader, MONGO_COLLECTION_1_NAME);
            _reader.AsyncReadCompleted += new ReadCompletedEvent<TestEntry>(_reader_AsyncReadCompleted);
            _reader.AsyncDistinctCompleted += new DistinctCompletedEvent(_reader_AsyncDistinctCompleted);

            IDatabaseUpdater<TestEntry> databaseUpdater = new MongoDB.Database.Updater<TestEntry>(MONGO_CONNECTION_STRING, MONGO_DATABASE_1_NAME);
            _updater = new MongoDB.Collection.Updater<TestEntry>(databaseUpdater, MONGO_COLLECTION_1_NAME);
            _updater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_updater_AsyncFindAndModifyCompleted);
            _updater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_updater_AsyncFindAndRemoveCompleted);

            _beforeTest = DateTime.Now;
            Assert.AreEqual(0, _results.Count());
            Assert.IsNull(_updaterResult);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoServerConnection.Connect();
            _mongoServerConnection.DropDatabases(new List<string>() { MONGO_DATABASE_1_NAME, MONGO_DATABASE_2_NAME, MONGO_DATABASE_3_NAME });
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            _results.Clear();
            _asyncReadResults.Clear();
            _asyncDistinctResults.Clear();
            _readerAutoResetEvent.Reset();
            _updaterResult = null;
            _WriteConcernResult = null;
            _asyncException = null;
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }
        #endregion Setup

        protected IServerConnection _mongoServerConnection;
        protected IDatabaseConnection<TestEntry> _mongoDatabaseConnection;
        protected ICollectionReader<TestEntry>  _reader;
        protected ICollectionWriter<TestEntry>  _writer;
        protected ICollectionUpdater<TestEntry> _updater;

        protected DateTime        _beforeTest;
        protected WriteConcern    _writeConcern         = WriteConcern.Acknowledged;
        protected List<TestEntry> _results              = new List<TestEntry>();
        protected List<TestEntry> _asyncReadResults     = new List<TestEntry>();
        protected List<BsonValue> _asyncDistinctResults = new List<BsonValue>();
        protected Exception       _asyncException       = null;
        protected FindAndModifyResult _updaterResult = null;
        protected WriteConcernResult _WriteConcernResult = null;
        protected AutoResetEvent  _readerAutoResetEvent = new AutoResetEvent(false);
        protected AutoResetEvent  _writerAutoResetEvent = new AutoResetEvent(false);
        protected AutoResetEvent _updaterAutoResetEvent = new AutoResetEvent(false);

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
            _writer.Write(mongoEntry);
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
            
            _writer.WriteAsync(mongoEntry);
            _readerAutoResetEvent.WaitOne(2000,true);// wait two seconds for the autoresetevent to be signaled in _mongoWriterAsync_WriteCompletedEvent
            _readerAutoResetEvent.Reset();
        }

        /// <summary>
        /// Event handler for MongoWriterAsync.WriteCompleted ; invoked after asynchronous writing has completed
        /// we use it here to signal the AutoResetEvent that writing has completed and that the blocked thread 
        /// can continue
        /// </summary>
        /// <param name="sender">null</param>
        void _writer_AsyncWriteCompleted(object sender)
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
        void _reader_AsyncReadCompleted(IEnumerable<TestEntry> e, Exception ex)
        {
            _asyncException = ex;
            _asyncReadResults.AddRange(e);
            _readerAutoResetEvent.Set();
        }

        void _reader_AsyncDistinctCompleted(IEnumerable<Bson.BsonValue> e, Exception ex)
        {
            _asyncException = ex;
            _asyncDistinctResults.AddRange(e);
            _readerAutoResetEvent.Set();
        }

        void _updater_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            _WriteConcernResult = result;
            _updaterAutoResetEvent.Set();
            _readerAutoResetEvent.Reset();
        }

        void _updater_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            _updaterResult = result;
            _updaterAutoResetEvent.Set();
            _readerAutoResetEvent.Reset();
        }

        #endregion Helper Methods
    }
}
