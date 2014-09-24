using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Test.Base;

namespace EasyMongo.Test
{
    [TestFixture]
    public class ServerConnectionTest : IntegrationTestFixture
    {
        #region Synchronous
        [Test, ExpectedException(typeof(MongoConnectionException))]
        public void ConstructorBadConnStringTest()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);
            _mongoServerConnection.Connect();
        }

        [Test]
        public void ConnectionStateTest()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(MongoServerState.Disconnected, _mongoServerConnection.State);

            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoServerConnection.Connect();

            Assert.AreEqual(MongoServerState.Connected, _mongoServerConnection.State);
        }

        [Test]
        public void GetDatabaseNamesForConnectionTest()
        {
            List<string> databaseNames = _mongoServerConnection.GetDbNamesForConnection();
            Assert.AreEqual(0, databaseNames.Count, "There should be no mongo databases on the server");

            _writer.Write(MONGO_COLLECTION_1_NAME, new Entry());//create a db in the process of writing an entry to a child collection

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();
            _reader = new Reader(_mongoDatabaseConnection);
            _writer = new Writer(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            _writer.Write(MONGO_COLLECTION_1_NAME, new Entry());//create a db in the process of writing an entry to a child collection

            databaseNames = _mongoServerConnection.GetDbNamesForConnection();
            Assert.AreEqual(2, databaseNames.Count, "There should be two mongo databases on the server");
            Assert.AreEqual(MONGO_DATABASE_1_NAME, databaseNames[0], MONGO_DATABASE_1_NAME + " is missing; was expected to be created");
            Assert.AreEqual(MONGO_DATABASE_2_NAME, databaseNames[1], MONGO_DATABASE_2_NAME + " is missing; was expected to be created");
       }

        [Test]
        public void GetDatabaseTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry();// uses MONGO_DATABASE_1_NAME

            MongoDatabase mongoDatabase = _mongoServerConnection.GetDatabase(MONGO_DATABASE_1_NAME, _writeConcern);
            Assert.IsNotNull(mongoDatabase);
            Assert.AreEqual(MONGO_DATABASE_1_NAME, mongoDatabase.Name);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry();// uses MONGO_DATABASE_2_NAME

            mongoDatabase = _mongoServerConnection.GetDatabase(MONGO_DATABASE_2_NAME, _writeConcern);
            Assert.IsNotNull(mongoDatabase);
            Assert.AreEqual(MONGO_DATABASE_2_NAME, mongoDatabase.Name);
        }

        [Test]
        public void DropDatabase1Test()
        {
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry(collectionName:MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_1_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            List<MongoCollection<Entry>> mongoCollections = _mongoDatabaseConnection.GetCollections<Entry>();
            Assert.AreEqual(3, mongoCollections.Count());// with system.indexes too

            CommandResult commandResult = _mongoServerConnection.DropDatabase(MONGO_DATABASE_1_NAME);
            Assert.IsNotNull(commandResult);
            Assert.IsTrue(commandResult.Ok);
            Assert.IsNull(commandResult.ErrorMessage);

            // the collections that we added will be removed with the database drop
            mongoCollections = _mongoDatabaseConnection.GetCollections<Entry>();
            Assert.AreEqual(0, mongoCollections.Count());

            // this is confusing functionality; the MongoDatabase object returned does not actually exist on the server
            // but will when it is written against
            MongoDatabase mongoDatabase = _mongoServerConnection.GetDatabase(MONGO_DATABASE_1_NAME, _writeConcern);
            Assert.IsNotNull(mongoDatabase);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();
            _reader = new Reader(_mongoDatabaseConnection);
            _writer = new Writer(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_2_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            mongoCollections = _mongoDatabaseConnection.GetCollections<Entry>();
            Assert.AreEqual(3, mongoCollections.Count());//system.indexes too

            commandResult = _mongoServerConnection.DropDatabase(MONGO_DATABASE_2_NAME);
            Assert.IsNotNull(commandResult);
            Assert.IsTrue(commandResult.Ok);
            Assert.IsNull(commandResult.ErrorMessage);

            // the collections that we added will be removed with the database drop
            mongoCollections = _mongoDatabaseConnection.GetCollections<Entry>();
            Assert.AreEqual(0, mongoCollections.Count());

            // see comments above
            mongoDatabase = _mongoServerConnection.GetDatabase(MONGO_DATABASE_2_NAME, _writeConcern);
            Assert.IsNotNull(mongoDatabase);
        }

        [Test]
        public void DropDatabase2Test()
        {
            MongoDatabase mongoDatabase = _mongoServerConnection[MONGO_DATABASE_1_NAME];

            Assert.IsNotNull(mongoDatabase);

            AddMongoEntry();

            Assert.AreEqual(1, _mongoServerConnection.GetDbNamesForConnection().Count());
            CommandResult commandResult = _mongoServerConnection.DropDatabase(mongoDatabase);

            Assert.IsNotNull(commandResult);
            Assert.IsTrue(commandResult.Ok);
            Assert.IsNullOrEmpty(commandResult.ErrorMessage);

            Assert.AreEqual(0, _mongoServerConnection.GetDbNamesForConnection().Count());
        }

        [Test]
        public void DropAllDatabasesTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_1_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();
            _reader = new Reader(_mongoDatabaseConnection);
            _writer = new Writer(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_2_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_3_NAME);
            _mongoDatabaseConnection.Connect();

            _reader = new Reader(_mongoDatabaseConnection);
            _writer = new Writer(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_3_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            List<string> results = _mongoServerConnection.GetDbNamesForConnection();
            Assert.IsTrue(results.Contains(MONGO_DATABASE_1_NAME));
            Assert.IsTrue(results.Contains(MONGO_DATABASE_2_NAME));
            Assert.IsTrue(results.Contains(MONGO_DATABASE_3_NAME));

            List<CommandResult> commandResults = _mongoServerConnection.DropAllDatabases();
            foreach (CommandResult commandResult in commandResults)
            {
                Assert.IsNotNull(commandResult);
                Assert.IsTrue(commandResult.Ok);
                Assert.IsNull(commandResult.ErrorMessage);
            }

            results = _mongoServerConnection.GetDbNamesForConnection();
            Assert.IsFalse(results.Contains(MONGO_DATABASE_1_NAME));
            Assert.IsFalse(results.Contains(MONGO_DATABASE_2_NAME));
            Assert.IsFalse(results.Contains(MONGO_DATABASE_3_NAME));
        }

        [Test]
        public void DropDatabases2Test()
        {
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_1_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();
            _reader = new Reader(_mongoDatabaseConnection);
            _writer = new Writer(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_2_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_3_NAME);
            _mongoDatabaseConnection.Connect();

            _reader = new Reader(_mongoDatabaseConnection);
            _writer = new Writer(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_3_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            List<string> results = _mongoServerConnection.GetDbNamesForConnection();
            Assert.IsTrue(results.Contains(MONGO_DATABASE_1_NAME));
            Assert.IsTrue(results.Contains(MONGO_DATABASE_2_NAME));
            Assert.IsTrue(results.Contains(MONGO_DATABASE_3_NAME));

            List<CommandResult> commandResults = _mongoServerConnection.DropDatabases(new List<string>() { MONGO_DATABASE_1_NAME, MONGO_DATABASE_3_NAME });

            foreach (CommandResult commandResult in commandResults)
            {
                Assert.IsNotNull(commandResult);
                Assert.IsTrue(commandResult.Ok);
                Assert.IsNull(commandResult.ErrorMessage);
            }

            results = _mongoServerConnection.GetDbNamesForConnection();
            Assert.IsFalse(results.Contains(MONGO_DATABASE_1_NAME));
            Assert.IsTrue(results.Contains(MONGO_DATABASE_2_NAME));
            Assert.IsFalse(results.Contains(MONGO_DATABASE_3_NAME));
        }

        [Test]
        public void DropDatabases3Test()
        {
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_1_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();

            _reader = new Reader(_mongoDatabaseConnection);
            _writer = new Writer(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_2_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_3_NAME);
            _mongoDatabaseConnection.Connect();

            _reader = new Reader(_mongoDatabaseConnection);
            _writer = new Writer(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_3_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            List<string> results = _mongoServerConnection.GetDbNamesForConnection();
            Assert.IsTrue(results.Contains(MONGO_DATABASE_1_NAME));
            Assert.IsTrue(results.Contains(MONGO_DATABASE_2_NAME));
            Assert.IsTrue(results.Contains(MONGO_DATABASE_3_NAME));

            MongoDatabase mongoDB1 = _mongoServerConnection.GetDatabase(MONGO_DATABASE_1_NAME, _writeConcern);
            MongoDatabase mongoDB2 = _mongoServerConnection.GetDatabase(MONGO_DATABASE_2_NAME, _writeConcern);
            MongoDatabase mongoDB3 = _mongoServerConnection.GetDatabase(MONGO_DATABASE_3_NAME, _writeConcern);

            List<CommandResult> commandResults = _mongoServerConnection.DropDatabases(new List<MongoDatabase>() { mongoDB1, mongoDB3 });

            foreach (CommandResult commandResult in commandResults)
            {
                Assert.IsNotNull(commandResult);
                Assert.IsTrue(commandResult.Ok);
                Assert.IsNull(commandResult.ErrorMessage);
            }

            results = _mongoServerConnection.GetDbNamesForConnection();
            Assert.IsFalse(results.Contains(MONGO_DATABASE_1_NAME));
            Assert.IsTrue(results.Contains(MONGO_DATABASE_2_NAME));
            Assert.IsFalse(results.Contains(MONGO_DATABASE_3_NAME));
        } 

        [Test]
        public void IndexerTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            MongoDatabase mongoDatabase = _mongoServerConnection[MONGO_DATABASE_1_NAME];

            Assert.IsNotNull(mongoDatabase);
            List<string> collectionNames = new List<string>(mongoDatabase.GetCollectionNames());
            Assert.AreEqual(2, collectionNames.Count());
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collectionNames[0]);
            Assert.AreEqual("system.indexes", collectionNames[1]);
        }

        [Test]
        public void GetLastErrorTest()
        {
            MongoDatabase mongoDatabase = _mongoServerConnection[MONGO_DATABASE_1_NAME];
            _mongoServerConnection.RequestStart(mongoDatabase);

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);

            GetLastErrorResult getLastErrorResult = _mongoServerConnection.GetLastError();

            Assert.IsNotNull(getLastErrorResult);
            Assert.IsTrue(getLastErrorResult.Ok);
            Assert.IsNullOrEmpty(getLastErrorResult.LastErrorMessage);
            Assert.IsFalse(getLastErrorResult.HasLastErrorMessage);

            _mongoServerConnection.RequestDone();
        }
        #endregion Synchronous
    }
}
