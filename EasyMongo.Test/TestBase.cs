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

namespace EasyMongo.Test
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
            _mongoUpdater = new Updater<TestEntry>(_mongoDatabaseConnection);

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
            _updaterResult = null;
            _WriteConcernResult = null;
            _serverConnectionResult = ConnectionResult.Empty;
            _databaseConnectionResult = ConnectionResult.Empty;
            _databaseConnectionReturnMessage = string.Empty;
            _serverConnectionReturnMessage = string.Empty;
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {

        }
        #endregion Setup

        protected IServerConnection              _mongoServerConnection;
        protected IDatabaseConnection<TestEntry> _mongoDatabaseConnection;
        protected IReader<TestEntry>             _mongoReader;
        protected IWriter<TestEntry>             _mongoWriter;
        protected IUpdater<TestEntry>            _mongoUpdater;

        protected DateTime        _beforeTest;
        protected WriteConcern    _writeConcern          = WriteConcern.Acknowledged;
        protected List<TestEntry> _results               = new List<TestEntry>();
        protected FindAndModifyResult _updaterResult     = null;
        protected WriteConcernResult         _WriteConcernResult = null;
        protected ConnectionResult _serverConnectionResult   = ConnectionResult.Empty;
        protected ConnectionResult _databaseConnectionResult = ConnectionResult.Empty;
        protected string           _serverConnectionReturnMessage = string.Empty;
        protected string           _databaseConnectionReturnMessage = string.Empty;

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
        #endregion Helper Methods
    }
}
