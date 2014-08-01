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
using EasyMongo.Test.Base.Ninject;

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
            _kernel = new StandardKernel();
        }

        [SetUp]
        public void Setup()
        {
            _mongoServerConnection = _kernel.TryGet<IServerConnection>();
            _mongoDatabaseConnection = _kernel.TryGet<IDatabaseConnection>();
            _mongoDatabaseConnection.Connect();

            #region    EasyMongo.Test
            _reader = _kernel.TryGet<IReader>();
            _writer = _kernel.TryGet<IWriter>();
            _updater = _kernel.TryGet<IUpdater>();

            // generic classes
            _readerT = _kernel.TryGet<IReader<Entry>>();
            _writerT = _kernel.TryGet<IWriter<Entry>>();
            _updaterT = _kernel.TryGet<IUpdater<Entry>>();
            #endregion EasyMongo.Test

            #region    EasyMongo.Async.Test
            _readerAsync = _kernel.TryGet<IReaderAsync>();
            _readerAsync.AsyncReadCompleted += new ReadCompletedEvent(_reader_AsyncReadCompleted);
            _readerAsync.AsyncDistinctCompleted += new DistinctCompletedEvent(_readerAsync_AsyncDistinctCompleted);

            _writerAsync = _kernel.TryGet<IWriterAsync>();
            _writerAsync.AsyncWriteCompleted += new WriteCompletedEvent(_writer_AsyncWriteCompleted);

            _updaterAsync = _kernel.TryGet<IUpdaterAsync>();
            _updaterAsync.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_updaterAsync_AsyncFindAndModifyCompleted);
            _updaterAsync.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_updaterAsync_AsyncFindAndRemoveCompleted);

            // generic classes
            _readerAsyncT = _kernel.TryGet<IReaderAsync<Entry>>();
            _readerAsyncT.AsyncReadCompleted += new ReadCompletedEvent(_readerAsyncT_AsyncReadCompleted);
            _readerAsyncT.AsyncDistinctCompleted += new DistinctCompletedEvent(_readerAsyncT_AsyncDistinctCompleted);

            _writerAsyncT = _kernel.TryGet<IWriterAsync<Entry>>();
            _writerAsyncT.AsyncWriteCompleted += new WriteCompletedEvent(_writerAsyncT_AsyncWriteCompleted);

            _updaterAsyncT = _kernel.TryGet<IUpdaterAsync<Entry>>();
            _updaterAsyncT.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_updaterAsyncT_AsyncFindAndModifyCompleted);
            _updaterAsyncT.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_updaterAsyncT_AsyncFindAndRemoveCompleted);

            #endregion EasyMongo.Async.Test

            #region    EasyMongo.Database.Test
            _databaseReader = _kernel.TryGet<IDatabaseReader>();
            _databaseReader.AsyncReadCompleted += new ReadCompletedEvent(_databaseReader_AsyncReadCompleted);
            _databaseReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_databaseReader_AsyncDistinctCompleted);

            _databaseWriter = _kernel.TryGet<IDatabaseWriter>();
            _databaseWriter.AsyncWriteCompleted += new WriteCompletedEvent(_databaseWriter_AsyncWriteCompleted);

            _databaseUpdater = _kernel.TryGet<IDatabaseUpdater>();
            _databaseUpdater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_databaseUpdater_AsyncFindAndModifyCompleted);
            _databaseUpdater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_databaseUpdater_AsyncFindAndRemoveCompleted);

            // generic classes
            _databaseReaderT = _kernel.TryGet<IDatabaseReader<Entry>>();
            _databaseReaderT.AsyncReadCompleted += new ReadCompletedEvent(_databaseReaderT_AsyncReadCompleted);
            _databaseReaderT.AsyncDistinctCompleted += new DistinctCompletedEvent(_databaseReaderT_AsyncDistinctCompleted);

            _databaseWriterT = _kernel.TryGet<IDatabaseWriter<Entry>>();
            _databaseWriterT.AsyncWriteCompleted += new WriteCompletedEvent(_databaseWriterT_AsyncWriteCompleted);

            _databaseUpdaterT = _kernel.TryGet<IDatabaseUpdater<Entry>>();
            _databaseUpdaterT.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_databaseUpdaterT_AsyncFindAndModifyCompleted);
            _databaseUpdaterT.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_databaseUpdaterT_AsyncFindAndRemoveCompleted);
            #endregion EasyMongo.Database.Test

            #region    EasyMongo.Collection.Test
            _collectionReader = _kernel.TryGet<ICollectionReader>();
            _collectionReader.AsyncReadCompleted += new ReadCompletedEvent(_collectionReader_AsyncReadCompleted);
            _collectionReader.AsyncDistinctCompleted += new DistinctCompletedEvent(_collectionReader_AsyncDistinctCompleted);

            _collectionWriter = _kernel.TryGet<ICollectionWriter>();
            _collectionWriter.AsyncWriteCompleted += new WriteCompletedEvent(_collectionWriter_AsyncWriteCompleted);

            _collectionUpdater = _kernel.TryGet<ICollectionUpdater>();
            _collectionUpdater.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_collectionUpdater_AsyncFindAndModifyCompleted);
            _collectionUpdater.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_collectionUpdater_AsyncFindAndRemoveCompleted);

            // generic classes
            _collectionReaderT = _kernel.TryGet<ICollectionReader<Entry>>();
            _collectionReaderT.AsyncReadCompleted += new ReadCompletedEvent(_collectionReaderT_AsyncReadCompleted);
            _collectionReaderT.AsyncDistinctCompleted += new DistinctCompletedEvent(_collectionReaderT_AsyncDistinctCompleted);

            _collectionWriterT = _kernel.TryGet<ICollectionWriter<Entry>>();
            _collectionWriterT.AsyncWriteCompleted += new WriteCompletedEvent(_collectionWriterT_AsyncWriteCompleted);

            _collectionUpdaterT = _kernel.TryGet<ICollectionUpdater<Entry>>();
            _collectionUpdaterT.AsyncFindAndModifyCompleted += new FindAndModifyCompletedEvent(_collectionUpdaterT_AsyncFindAndModifyCompleted);
            _collectionUpdaterT.AsyncFindAndRemoveCompleted += new FindAndRemoveCompletedEvent(_collectionUpdaterT_AsyncFindAndRemoveCompleted);
            #endregion EasyMongo.Collection.Test

            _beforeTest = DateTime.Now;
            Assert.AreEqual(0, _results.Count());
            Assert.IsNull(_findAndModifyResult);
        }

        [TearDown]
        public void TearDown()
        {
            _mongoServerConnection   = _kernel.TryGet<IServerConnection>();
            _mongoDatabaseConnection = _kernel.TryGet<IDatabaseConnection>();
            _mongoDatabaseConnection.Connect();
            _mongoDatabaseConnection.ClearAllCollections<Entry>();
            _mongoServerConnection.Connect();
            _mongoServerConnection.DropDatabases(new List<string>() { MONGO_DATABASE_1_NAME, MONGO_DATABASE_2_NAME, MONGO_DATABASE_3_NAME });

            _results.Clear();
            _asyncReadResults.Clear();
            _asyncDistinctBSONResults.Clear();
            _asyncDistinctResults.Clear();
            _readerAutoResetEvent.Reset();
            _findAndModifyResult = null;
            _writeConcernResult = null;
            _asyncException = null;

            _serverConnectionResult = ConnectionResult.Empty;
            _databaseConnectionResult = ConnectionResult.Empty;
            _databaseConnectionReturnMessage = string.Empty;
            _serverConnectionReturnMessage = string.Empty;         

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

        protected IKernel _kernel;

        protected IServerConnection   _mongoServerConnection;
        protected IDatabaseConnection _mongoDatabaseConnection;

        protected IWriter _writer;
        protected IUpdater _updater;
        protected IReader _reader;

        protected IWriter<Entry> _writerT;
        protected IUpdater<Entry> _updaterT;
        protected IReader<Entry> _readerT;

        protected IReaderAsync _readerAsync;
        protected IWriterAsync _writerAsync;
        protected IUpdaterAsync _updaterAsync;

        protected IReaderAsync<Entry> _readerAsyncT;
        protected IWriterAsync<Entry> _writerAsyncT;
        protected IUpdaterAsync<Entry> _updaterAsyncT;

        protected IDatabaseReader _databaseReader;
        protected IDatabaseWriter _databaseWriter;
        protected IDatabaseUpdater _databaseUpdater;

        protected IDatabaseReader<Entry> _databaseReaderT;
        protected IDatabaseWriter<Entry> _databaseWriterT;
        protected IDatabaseUpdater<Entry> _databaseUpdaterT;

        protected ICollectionReader _collectionReader;
        protected ICollectionWriter _collectionWriter;
        protected ICollectionUpdater _collectionUpdater;

        protected ICollectionReader<Entry> _collectionReaderT;
        protected ICollectionWriter<Entry> _collectionWriterT;
        protected ICollectionUpdater<Entry> _collectionUpdaterT;

        protected DateTime              _beforeTest;
        protected WriteConcern          _writeConcern                     = WriteConcern.Acknowledged;
        protected List<Entry>           _results                          = new List<Entry>();
        protected List<Entry>           _asyncReadResults                 = new List<Entry>();
        protected List<BsonValue>       _asyncDistinctBSONResults         = new List<BsonValue>();
        protected List<string>          _asyncDistinctResults             = new List<string>();
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
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;
            _writer.Write<Entry>(collectionName, mongoEntry);
        }

        /// <summary>
        /// Method useful for synchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoWriter<T> class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryT(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;
            _writerT.Write(collectionName, mongoEntry);
        }

        /// <summary>
        ///  Method useful for asynchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoWriterAsync class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryAsync(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _writerAsync.WriteAsync<Entry>(collectionName, mongoEntry);
            _writerAutoResetEvent.WaitOne();
        }

        /// <summary>
        ///  Method useful for asynchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoWriterAsync<T> class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryAsyncT(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _writerAsyncT.WriteAsync(collectionName, mongoEntry);
            _writerAutoResetEvent.WaitOne();
        }

        /// <summary>
        ///  Method useful for asynchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoDataBaseWriter class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryDatabaseAsync(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _databaseWriter.WriteAsync(collectionName, mongoEntry);
            _writerAutoResetEvent.WaitOne();
        }

        /// <summary>
        ///  Method useful for adding a MongoTestEntry object to MongoDB using the TestFixture's MongoDataBaseWriter class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryDatabase(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _databaseWriter.Write(collectionName, mongoEntry);
        }

        /// <summary>
        ///  Method useful for asynchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoDataBaseWriter<T> class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryDatabaseAsyncT(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _databaseWriterT.WriteAsync(collectionName, mongoEntry);
            _writerAutoResetEvent.WaitOne();
        }

        /// <summary>
        ///  Method useful for adding a MongoTestEntry object to MongoDB using the TestFixture's MongoDataBaseWriter<T> class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryDatabaseT(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _databaseWriterT.Write(collectionName, mongoEntry);
        }

        /// <summary>
        ///  Method useful for asynchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoCollectionWriter class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryCollectionAsync(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _collectionWriter.WriteAsync<Entry>(mongoEntry);
            _writerAutoResetEvent.WaitOne();
        }

        /// <summary>
        ///  Method useful for adding a MongoTestEntry object to MongoDB using the TestFixture's MongoCollectionWriter class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryCollection(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _collectionWriter.Write<Entry>(mongoEntry);
        }

        /// <summary>
        ///  Method useful for asynchronously adding a MongoTestEntry object to MongoDB using the TestFixture's MongoCollectionWriter<T> class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryCollectionAsyncT(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _collectionWriterT.WriteAsync(mongoEntry);
            _writerAutoResetEvent.WaitOne();
        }

        /// <summary>
        ///  Method useful for adding a MongoTestEntry object to MongoDB using the TestFixture's MongoCollectionWriter<T> class
        /// </summary>
        /// <param name="message"></param>
        /// <param name="collectionName"></param>
        protected void AddMongoEntryCollectionT(string message = "Hello World", string collectionName = MONGO_COLLECTION_1_NAME)
        {
            Entry mongoEntry = new Entry();
            mongoEntry.Message = message;
            mongoEntry.TimeStamp = DateTime.Now;

            _collectionWriterT.Write(mongoEntry);
        }

        protected IEnumerable<T> ReadMongoEntry<T>(string collectionName, string field, string fieldName = "Message")
        {
            return _reader.Read<T>(collectionName, fieldName, field);
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
            _writerAutoResetEvent.Set();
        }

        protected void _reader_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<Entry> results = (IEnumerable<Entry>)e;
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }

        protected void _readerAsync_AsyncDistinctCompleted(object e, Exception ex)
        {
            _asyncException = ex;

            if (e != null)
                _asyncDistinctResults.AddRange((IEnumerable<string>)e);

            _readerAutoResetEvent.Set();
        }

        #region    Generics
        void _readerAsyncT_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<Entry> results = (IEnumerable<Entry>)e;
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }
        void _readerAsyncT_AsyncDistinctCompleted(object e, Exception ex)
        {
            _asyncException = ex;

            if (e != null)
                _asyncDistinctResults.AddRange((IEnumerable<string>)e);

            _readerAutoResetEvent.Set();
        }
        void _updaterAsyncT_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            _writeConcernResult = result;
            _updaterAutoResetEvent.Set();
        }
        void _updaterAsyncT_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            _findAndModifyResult = result;
            _updaterAutoResetEvent.Set();
        }
        void _writerAsyncT_AsyncWriteCompleted(object sender)
        {
            _writerAutoResetEvent.Set();
        }
        #endregion Generics

        #endregion EasyMongo.Async

        #region    EasyMongo.Database
        protected void _databaseReader_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<Entry> results = (IEnumerable<Entry>)e;
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }

        protected void _databaseReader_AsyncDistinctCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            _asyncDistinctResults.AddRange((IEnumerable<string>)e);
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
        #region    Generics
        protected void _databaseReaderT_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<Entry> results = (IEnumerable<Entry>)e;
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }

        protected void _databaseReaderT_AsyncDistinctCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            _asyncDistinctResults.AddRange((IEnumerable<string>)e);
            _readerAutoResetEvent.Set();
        }

        protected void _databaseWriterT_AsyncWriteCompleted(object sender)
        {
            _writerAutoResetEvent.Set();// allow the thread in AddMongoEntryAsync to continue
        }

        protected void _databaseUpdaterT_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            _writeConcernResult = result;
            _updaterAutoResetEvent.Set();
        }

        protected void _databaseUpdaterT_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            _findAndModifyResult = result;
            _updaterAutoResetEvent.Set();
        }
        #endregion Generics
        #endregion EasyMongo.Database

        #region EasyMongo.Collection
        protected void _collectionWriter_AsyncWriteCompleted(object sender)
        {
            _writerAutoResetEvent.Set();// allow the thread in AddMongoEntryAsync to continue
        }

        // TODO - Refactor the cast to not define impl class...
        protected void _collectionReader_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<Entry> results = (IEnumerable<Entry>)e;// HEY! Can this instead be IEnumerable<Entry>
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }

        protected void _collectionReader_AsyncDistinctCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            _asyncDistinctResults.AddRange((IEnumerable<string>)e);
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

        #region    Generics
        protected void _collectionWriterT_AsyncWriteCompleted(object sender)
        {
            _writerAutoResetEvent.Set();// allow the thread in AddMongoEntryAsync to continue
        }

        protected void _collectionReaderT_AsyncReadCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            IEnumerable<Entry> results = (IEnumerable<Entry>)e;// HEY! Can this instead be IEnumerable<Entry>
            _asyncReadResults.AddRange(results);
            _readerAutoResetEvent.Set();
        }

        protected void _collectionReaderT_AsyncDistinctCompleted(object e, Exception ex)
        {
            _asyncException = ex;
            _asyncDistinctResults.AddRange((IEnumerable<string>)e);
            _readerAutoResetEvent.Set();
        }

        protected void _collectionUpdaterT_AsyncFindAndRemoveCompleted(WriteConcernResult result)
        {
            _writeConcernResult = result;
            _updaterAutoResetEvent.Set();
        }

        protected void _collectionUpdaterT_AsyncFindAndModifyCompleted(FindAndModifyResult result)
        {
            _findAndModifyResult = result;
            _updaterAutoResetEvent.Set();
        }
        #endregion Generics
        #endregion EasyMongo.Collection

        #endregion Helper Methods
    }
}
