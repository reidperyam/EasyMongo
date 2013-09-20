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
    public abstract class IntegrationTestBase
    {
        /// <summary>
        /// Object managing dependency injection using Ninject
        /// </summary>
        protected Configurator _configurator;

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
            #region    Ninject Loading
            ////System.Diagnostics.Debugger.Launch();
            //_mongoServerConnection_ninject = _configurator.TryGet<IServerConnection>();
            //_mongoDatabaseConnection_ninject = _configurator.TryGet<IDatabaseConnection<TestEntry>>();
            //_mongoDatabaseConnection_ninject.Connect();

            //_reader = _configurator.TryGet<IReader<IEasyMongoEntry>>();
            //_writer_ninject = _configurator.TryGet<IWriter<IEasyMongoEntry>>();
            //_updater_ninject = _configurator.TryGet<IUpdater<IEasyMongoEntry>>();

            //_readerAsync_ninject = _configurator.TryGet<IReaderAsync<IEasyMongoEntry>>();
            //_writerAsync_ninject = _configurator.TryGet<IWriterAsync<IEasyMongoEntry>>();
            //_updaterAsync_ninject = _configurator.TryGet<IUpdaterAsync<IEasyMongoEntry>>();

            //_databaseReader_ninject = _configurator.TryGet<IDatabaseReader<IEasyMongoEntry>>();
            //_databaseWriter_ninject = _configurator.TryGet<IDatabaseWriter<IEasyMongoEntry>>();
            //_databaseUpdater_ninject = _configurator.TryGet<IDatabaseUpdater<IEasyMongoEntry>>();

            //_collectionReader_ninject = _configurator.TryGet<ICollectionReader<IEasyMongoEntry>>();
            //_collectionWriter_ninject = _configurator.TryGet<ICollectionWriter<IEasyMongoEntry>>();
            //_collectionUpdater_ninject = _configurator.TryGet<ICollectionUpdater<IEasyMongoEntry>>();

            #endregion Ninject Loading


            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            #region    EasyMongo.Test
            _reader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _writer = new Writer<TestEntry>(_mongoDatabaseConnection);
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

            _collectionReader = new Collection.Reader<TestEntry>(_databaseReader, MONGO_COLLECTION_1_NAME);

            _collectionUpdater = new Collection.Updater<TestEntry>(_databaseUpdater, MONGO_COLLECTION_1_NAME);
            #endregion EasyMongo.Collection.Test

            _beforeTest = DateTime.Now;
            Assert.AreEqual(0, _results.Count());
            Assert.IsNull(_findAndModifyResult);
        }

        [TearDown]
        public void TearDown()
        {
            #region    Ninject Loading
            //_mongoServerConnection = _configurator.TryGet<IServerConnection>();
            //_mongoDatabaseConnection_ninject = _configurator.Kernel.TryGet<IDatabaseConnection<TestEntry>>();
            //_mongoDatabaseConnection.Connect();
            //_mongoDatabaseConnection.ClearAllCollections();
            //_mongoServerConnection.Connect();
            //_mongoServerConnection.DropDatabases(new List<string>() { MONGO_DATABASE_1_NAME, MONGO_DATABASE_2_NAME, MONGO_DATABASE_3_NAME });
            #endregion Ninject Loading


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

        #region    Ninject loaded

        //protected IServerConnection _mongoServerConnection_ninject;
        //protected IDatabaseConnection<IEasyMongoEntry> _mongoDatabaseConnection_ninject;

        //protected IWriter<TestEntry> _writer_ninject;
        //protected IUpdater<TestEntry> _updater_ninject;

        //protected IDatabaseReader<IEasyMongoEntry> _databaseReader_ninject;
        //protected IDatabaseWriter<IEasyMongoEntry> _databaseWriter_ninject;
        //protected IDatabaseUpdater<IEasyMongoEntry> _databaseUpdater_ninject;

        //protected ICollectionReader<IEasyMongoEntry> _collectionReader_ninject;
        //protected ICollectionWriter<IEasyMongoEntry> _collectionWriter_ninject;
        //protected ICollectionUpdater<IEasyMongoEntry> _collectionUpdater_ninject;

        //protected IReaderAsync<IEasyMongoEntry> _readerAsync_ninject;
        //protected IWriterAsync<IEasyMongoEntry> _writerAsync_ninject;
        //protected IUpdaterAsync<IEasyMongoEntry> _updaterAsync_ninject;
        #endregion Ninject loaded

        protected IServerConnection              _mongoServerConnection;
        protected IDatabaseConnection<TestEntry> _mongoDatabaseConnection;

        //protected IReader<TestEntry> _reader;
        protected IWriter<TestEntry> _writer;
        protected IUpdater<TestEntry> _updater;
        protected IReader<TestEntry> _reader;



        protected IReaderAsync<TestEntry> _readerAsync;
        protected IWriterAsync<TestEntry> _writerAsync;
        protected IUpdaterAsync<TestEntry> _updaterAsync;

        protected IDatabaseReader<TestEntry> _databaseReader;
        protected IDatabaseWriter<TestEntry> _databaseWriter;
        protected IDatabaseUpdater<TestEntry> _databaseUpdater;

        protected ICollectionReader<TestEntry> _collectionReader;
        protected ICollectionWriter<TestEntry> _collectionWriter;
        protected ICollectionUpdater<TestEntry> _collectionUpdater;



        protected DateTime            _beforeTest;
        protected WriteConcern        _writeConcern                     = WriteConcern.Acknowledged;
        protected List<IEasyMongoEntry> _results = new List<IEasyMongoEntry>();
        protected List<IEasyMongoEntry> _asyncReadResults = new List<IEasyMongoEntry>();
        protected List<BsonValue>     _asyncDistinctResults             = new List<BsonValue>();
        protected Exception           _asyncException                   = null;
        protected FindAndModifyResult _findAndModifyResult              = null;
        protected WriteConcernResult  _writeConcernResult               = null;
        protected ConnectionResult    _serverConnectionResult           = ConnectionResult.Empty;
        protected ConnectionResult    _databaseConnectionResult         = ConnectionResult.Empty;
        protected AutoResetEvent      _readerAutoResetEvent             = new AutoResetEvent(false);
        protected AutoResetEvent      _writerAutoResetEvent             = new AutoResetEvent(false);
        protected AutoResetEvent      _updaterAutoResetEvent            = new AutoResetEvent(false);
        protected AutoResetEvent      _serverConnectionAutoResetEvent   = new AutoResetEvent(false);
        protected AutoResetEvent      _databaseConnectionAutoResetEvent = new AutoResetEvent(false);
        protected string              _serverConnectionReturnMessage    = string.Empty;
        protected string              _databaseConnectionReturnMessage  = string.Empty;

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

        #endregion Helper Methods

        /// <summary>
        /// TODO - fix this test so that the ninject types are loaded with implementations of their generic, interface argument types 
        /// </summary>
        [Explicit, Test]
        public void NinjectDIBindingTest()
        {
             IServerConnection _mongoServerConnection_ninject;
             IDatabaseConnection<IEasyMongoEntry> _mongoDatabaseConnection_ninject;

             IWriter<IEasyMongoEntry> _writer_ninject;
             IUpdater<IEasyMongoEntry> _updater_ninject;
             IReader<IEasyMongoEntry> _reader_ninject;

             IDatabaseReader<IEasyMongoEntry> _databaseReader_ninject;
             IDatabaseWriter<IEasyMongoEntry> _databaseWriter_ninject;
             IDatabaseUpdater<IEasyMongoEntry> _databaseUpdater_ninject;

             ICollectionReader<IEasyMongoEntry> _collectionReader_ninject;
             ICollectionWriter<IEasyMongoEntry> _collectionWriter_ninject;
             ICollectionUpdater<IEasyMongoEntry> _collectionUpdater_ninject;

             IReaderAsync<IEasyMongoEntry> _readerAsync_ninject;
             IWriterAsync<IEasyMongoEntry> _writerAsync_ninject;
             IUpdaterAsync<IEasyMongoEntry> _updaterAsync_ninject;

            _mongoServerConnection_ninject = _configurator.TryGet<IServerConnection>();
            var _mongoDatabaseConnection_ninjet = _configurator.Kernel.TryGet<IDatabaseConnection<IEasyMongoEntry>>();
            //_mongoDatabaseConnection_ninjet.Connect();

            //_reader_ninject = _configurator.TryGet<IReader<IEasyMongoEntry>>();
            //_writer_ninject = _configurator.TryGet<IWriter<IEasyMongoEntry>>();
            //_updater_ninject = _configurator.TryGet<IUpdater<IEasyMongoEntry>>();

            //_readerAsync_ninject = _configurator.TryGet<IReaderAsync<IEasyMongoEntry>>();
            //_writerAsync_ninject = _configurator.TryGet<IWriterAsync<IEasyMongoEntry>>();
            //_updaterAsync_ninject = _configurator.TryGet<IUpdaterAsync<IEasyMongoEntry>>();

            //_databaseReader_ninject = _configurator.TryGet<IDatabaseReader<IEasyMongoEntry>>();
            //_databaseWriter_ninject = _configurator.TryGet<IDatabaseWriter<IEasyMongoEntry>>();
            //_databaseUpdater_ninject = _configurator.TryGet<IDatabaseUpdater<IEasyMongoEntry>>();

            //_collectionReader_ninject = _configurator.TryGet<ICollectionReader<IEasyMongoEntry>>();
            //_collectionWriter_ninject = _configurator.TryGet<ICollectionWriter<IEasyMongoEntry>>();
            //_collectionUpdater_ninject = _configurator.TryGet<ICollectionUpdater<IEasyMongoEntry>>();

            System.Diagnostics.Debugger.Launch();
            var reader_ninject = _configurator.Kernel.TryGet(typeof(IReader<IEasyMongoEntry>));
            _writer_ninject = _configurator.TryGet<IWriter<IEasyMongoEntry>>();
            _updater_ninject = _configurator.TryGet<IUpdater<IEasyMongoEntry>>();

            _readerAsync_ninject = _configurator.TryGet<IReaderAsync<IEasyMongoEntry>>();
            _writerAsync_ninject = _configurator.TryGet<IWriterAsync<IEasyMongoEntry>>();
            _updaterAsync_ninject = _configurator.TryGet<IUpdaterAsync<IEasyMongoEntry>>();

            _databaseReader_ninject = _configurator.TryGet<IDatabaseReader<IEasyMongoEntry>>();
            _databaseWriter_ninject = _configurator.TryGet<IDatabaseWriter<IEasyMongoEntry>>();
            _databaseUpdater_ninject = _configurator.TryGet<IDatabaseUpdater<IEasyMongoEntry>>();

            _collectionReader_ninject = _configurator.TryGet<ICollectionReader<IEasyMongoEntry>>();
            _collectionWriter_ninject = _configurator.TryGet<ICollectionWriter<IEasyMongoEntry>>();
            _collectionUpdater_ninject = _configurator.TryGet<ICollectionUpdater<IEasyMongoEntry>>();


            string nullErrorFormatString = "{0} did not bind to implementation class as expected";

            IEasyMongoEntry _iEasyMongoEntry = _configurator.TryGet<IEasyMongoEntry>();
            Assert.IsNotNull(_iEasyMongoEntry, string.Format(nullErrorFormatString, "IEasyMongoEntry"));
            Assert.IsInstanceOf<TestEntry>(_iEasyMongoEntry);

            Assert.IsNotNull(_mongoServerConnection_ninject, string.Format(nullErrorFormatString, "IServerConnection"));
            Assert.IsInstanceOf<ServerConnection>(_mongoServerConnection_ninject);

            //Assert.IsNotNull(_mongoDatabaseConnection_ninject, string.Format(nullErrorFormatString, "IDatabaseConnection<IEasyMongoEntry>"));
            //Assert.IsInstanceOf<DatabaseConnection<TestEntry>>(_mongoDatabaseConnection_ninject);

            #region    EasyMongo
            Assert.IsNotNull(_reader, string.Format(nullErrorFormatString, "IReader<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Reader<IEasyMongoEntry>>(_reader);

            Assert.IsNotNull(_writer_ninject, string.Format(nullErrorFormatString, "IWriter<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Writer<IEasyMongoEntry>>(_writer_ninject);

            Assert.IsNotNull(_updater_ninject, string.Format(nullErrorFormatString, "IUpdater<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Updater<IEasyMongoEntry>>(_updater_ninject);
            #endregion EasyMongo

            #region    EasyMongo.Async
            Assert.IsNotNull(_readerAsync_ninject, string.Format(nullErrorFormatString, "IReaderAsync<IEasyMongoEntry>"));
            Assert.IsInstanceOf<ReaderAsync<IEasyMongoEntry>>(_readerAsync_ninject);

            Assert.IsNotNull(_writerAsync_ninject, string.Format(nullErrorFormatString, "IWriterAsync<IEasyMongoEntry>"));
            Assert.IsInstanceOf<WriterAsync<IEasyMongoEntry>>(_writerAsync_ninject);

            Assert.IsNotNull(_updaterAsync_ninject, string.Format(nullErrorFormatString, "IUpdaterAsync<IEasyMongoEntry>"));
            Assert.IsInstanceOf<UpdaterAsync<IEasyMongoEntry>>(_updaterAsync_ninject);
            #endregion EasyMongo.Async

            #region    EasyMongo.Database
            Assert.IsNotNull(_databaseReader_ninject, string.Format(nullErrorFormatString, "IDatabaseReader<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Database.Reader<IEasyMongoEntry>>(_databaseReader_ninject);

            Assert.IsNotNull(_databaseWriter_ninject, string.Format(nullErrorFormatString, "IDatabaseWriter<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Database.Writer<IEasyMongoEntry>>(_databaseWriter_ninject);

            Assert.IsNotNull(_databaseUpdater_ninject, string.Format(nullErrorFormatString, "IDatabaseUpdater<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Database.Updater<IEasyMongoEntry>>(_databaseUpdater_ninject);
            #endregion EasyMongo.Database

            #region    EasyMongo.Collection
            Assert.IsNotNull(_collectionReader_ninject, string.Format(nullErrorFormatString, " ICollectionReader<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Collection.Reader<IEasyMongoEntry>>(_collectionReader_ninject);

            Assert.IsNotNull(_collectionWriter_ninject, string.Format(nullErrorFormatString, "ICollectionWriter<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Collection.Writer<IEasyMongoEntry>>(_collectionWriter_ninject);

            Assert.IsNotNull(_collectionUpdater_ninject, string.Format(nullErrorFormatString, "ICollectionUpdater<IEasyMongoEntry>"));
            Assert.IsInstanceOf<Collection.Updater<IEasyMongoEntry>>(_collectionUpdater_ninject);
            #endregion EasyMongo.Collection
        }
    }
}
