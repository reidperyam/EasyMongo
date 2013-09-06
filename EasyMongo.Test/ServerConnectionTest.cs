using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver;
using EasyMongo.Contract;

namespace EasyMongo.Test
{
    [TestFixture]
    public class ServerConnectionTest : TestBase
    {
        #region Synchronous
        [Test,ExpectedException(typeof(MongoConnectionException))]
        public void ConstructorBadConnStringTest()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);
            _mongoServerConnection.Connect();
        }

        [Test]
        public void ConstructorBadConnStringTestAsync()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);
            _mongoServerConnection.ConnectAsync(new Action<ConnectionResult,string>(_mongoServerConnection_Connected));
            _serverConnectionAutoResetEvent.WaitOne();
            Assert.AreEqual(ConnectionState.NotConnected,_mongoServerConnection.ConnectionState);/**/
            Assert.AreEqual(ConnectionResult.Failure, _serverConnectionResult);/**/
            Assert.IsNotNull(_serverConnectionReturnMessage);
        }

        [Test]
        public void ConnectionStateTest()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(ConnectionState.NotConnected, _mongoServerConnection.ConnectionState);
            Assert.IsTrue(_mongoServerConnection.CanConnect());

            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoServerConnection.Connect();

            Assert.AreEqual(ConnectionState.Connected, _mongoServerConnection.ConnectionState);

            Assert.IsTrue(_mongoServerConnection.CanConnect());
        }

        [Test]
        public void GetDatabaseNamesForConnectionTest()
        {
            List<string> databaseNames = _mongoServerConnection.GetDbNamesForConnection();
            Assert.AreEqual(0, databaseNames.Count, "There should be no mongo databases on the server");

            _mongoWriter.Write(MONGO_COLLECTION_1_NAME, new TestEntry());//create a db in the process of writing an entry to a child collection

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();
            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            _mongoWriter.Write(MONGO_COLLECTION_1_NAME, new TestEntry());//create a db in the process of writing an entry to a child collection

            databaseNames = _mongoServerConnection.GetDbNamesForConnection();
            Assert.AreEqual(2, databaseNames.Count, "There should be two mongo databases on the server");
            Assert.AreEqual(MONGO_DATABASE_1_NAME, databaseNames[0], MONGO_DATABASE_1_NAME + " is missing; was expected to be created");
            Assert.AreEqual(MONGO_DATABASE_2_NAME, databaseNames[1], MONGO_DATABASE_2_NAME + " is missing; was expected to be created");
       }

        [Test]
        public void GetDatabaseTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry();// uses MONGO_DATABASE_1_NAME

            MongoDatabase mongoDatabase = _mongoServerConnection.GetDatabase(MONGO_DATABASE_1_NAME, _writeConcern);
            Assert.IsNotNull(mongoDatabase);
            Assert.AreEqual(MONGO_DATABASE_1_NAME, mongoDatabase.Name);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry();// uses MONGO_DATABASE_2_NAME

            mongoDatabase = _mongoServerConnection.GetDatabase(MONGO_DATABASE_2_NAME, _writeConcern);
            Assert.IsNotNull(mongoDatabase);
            Assert.AreEqual(MONGO_DATABASE_2_NAME, mongoDatabase.Name);
        }

        [Test]
        public void DropDatabaseTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry(collectionName:MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_1_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            List<MongoCollection<TestEntry>> mongoCollections = _mongoDatabaseConnection.GetCollections();
            Assert.AreEqual(3, mongoCollections.Count());// with system.indexes too

            CommandResult commandResult = _mongoServerConnection.DropDatabase(MONGO_DATABASE_1_NAME);
            Assert.IsNotNull(commandResult);
            Assert.IsTrue(commandResult.Ok);
            Assert.IsNull(commandResult.ErrorMessage);

            // the collections that we added will be removed with the database drop
            mongoCollections =  _mongoDatabaseConnection.GetCollections();
            Assert.AreEqual(0, mongoCollections.Count());

            // this is confusing functionality; the MongoDatabase object returned does not actually exist on the server
            // but will when it is written against
            MongoDatabase mongoDatabase = _mongoServerConnection.GetDatabase(MONGO_DATABASE_1_NAME, _writeConcern);
            Assert.IsNotNull(mongoDatabase);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();
            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_2_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            mongoCollections = _mongoDatabaseConnection.GetCollections();
            Assert.AreEqual(3, mongoCollections.Count());//system.indexes too

            commandResult = _mongoServerConnection.DropDatabase(MONGO_DATABASE_2_NAME);
            Assert.IsNotNull(commandResult);
            Assert.IsTrue(commandResult.Ok);
            Assert.IsNull(commandResult.ErrorMessage);

            // the collections that we added will be removed with the database drop
            mongoCollections = _mongoDatabaseConnection.GetCollections();
            Assert.AreEqual(0, mongoCollections.Count());

            // see comments above
            mongoDatabase = _mongoServerConnection.GetDatabase(MONGO_DATABASE_2_NAME, _writeConcern);
            Assert.IsNotNull(mongoDatabase);
        }

        [Test]
        public void DropAllDatabasesTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_1_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();
            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_2_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_3_NAME);
            _mongoDatabaseConnection.Connect();

            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

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
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_1_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();
            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_2_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_3_NAME);
            _mongoDatabaseConnection.Connect();

            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

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
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_1_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_2_NAME);
            _mongoDatabaseConnection.Connect();

            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);// uses MONGO_DATABASE_2_NAME
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_3_NAME);
            _mongoDatabaseConnection.Connect();

            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoWriter = new Writer<TestEntry>(_mongoDatabaseConnection);// need to reinitialize writer when we change the DatabaseConnection

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
        public void CanConnectTest()
        {
            Assert.IsTrue(_mongoServerConnection.CanConnect(), "Cannot connect to " + MONGO_CONNECTION_STRING);

              // try connecting to a random ip address
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);

            Assert.IsFalse(_mongoServerConnection.CanConnect(), "Cannot connect to " + MONGO_CONNECTION_STRING);
        }

        [Test]
        public void IndexerTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);//write to the database in order to prove that the fetched database is as expected
            MongoDatabase mongoDatabase = _mongoServerConnection[MONGO_DATABASE_1_NAME];

            Assert.IsNotNull(mongoDatabase);
            List<string> collectionNames = new List<string>(mongoDatabase.GetCollectionNames());
            Assert.AreEqual(2, collectionNames.Count());
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collectionNames[0]);
            Assert.AreEqual("system.indexes", collectionNames[1]);
        }
        #endregion Synchronous

        #region Asynchronous

        [Test]
        public void AsynchronousTest1()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(ConnectionState.NotConnected, _mongoServerConnection.ConnectionState);
            Assert.AreEqual(ConnectionResult.Empty,_serverConnectionResult);
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            Assert.AreEqual(ConnectionState.Connecting,_mongoServerConnection.ConnectionState);
            Assert.AreEqual(ConnectionResult.Empty,_serverConnectionResult);
            _serverConnectionAutoResetEvent.WaitOne();
            Assert.AreEqual(ConnectionState.Connected,_mongoServerConnection.ConnectionState);/**/
            Assert.AreEqual(ConnectionResult.Success, _serverConnectionResult);/**/
            Assert.IsNotNull(_serverConnectionReturnMessage);
        }

        [Test]
        public void AsynchronousTest2()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(ConnectionResult.Empty, _serverConnectionResult);
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);

            List<string> returned = _mongoServerConnection.GetDbNamesForConnection();
            Assert.AreEqual(ConnectionResult.Success, _serverConnectionResult);
            Assert.IsTrue(_mongoServerConnection.ConnectionState == ConnectionState.Connected);
            Assert.IsNotNull(_serverConnectionReturnMessage);

            Assert.AreEqual(0, returned.Count(),"no database names since nothing has been written yet");
            AddMongoEntry();

            returned = _mongoServerConnection.GetDbNamesForConnection();

            Assert.AreEqual(1, returned.Count(), "only the driver-created database, local, should exist");
            Assert.AreEqual(MONGO_DATABASE_1_NAME, returned[0]);
        }

        [Test]
        public void AsynchronousTest3()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _serverConnectionAutoResetEvent.WaitOne();

            Assert.AreEqual(ConnectionResult.Failure, _serverConnectionResult);
            Assert.IsTrue(_mongoServerConnection.ConnectionState == ConnectionState.NotConnected);
            Assert.IsNotNull(_serverConnectionReturnMessage);
        }

        #endregion Asynchronous
    }
}
