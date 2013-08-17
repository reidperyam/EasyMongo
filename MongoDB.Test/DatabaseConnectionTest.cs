using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Contract;

namespace MongoDB.Test
{
    [TestFixture]
    public class DatabaseConnectionTest : TestBase
    {
        [Test]
        public void ConnectionStateTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(ConnectionState.NotConnected, _mongoDatabaseConnection.ConnectionState);

            Assert.IsTrue(_mongoDatabaseConnection.CanConnect());

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);

            Assert.IsTrue(_mongoDatabaseConnection.CanConnect());
        }

        [Test]
        public void GetCollectionNamesForDatabaseTest()
        {
            List<string> collectionNames = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(0, collectionNames.Count, "The database should be empty of collections right now");

            _mongoWriter.Write(MONGO_COLLECTION_1_NAME, new TestEntry());
            collectionNames = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(2, collectionNames.Count, "The database should have one collection");
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collectionNames[0], MONGO_COLLECTION_1_NAME + " is missing");
            Assert.AreEqual("system.indexes", collectionNames[1], "system.indexes missing; was expected to be created");

            _mongoWriter.Write(MONGO_COLLECTION_2_NAME, new TestEntry());
            collectionNames = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(3, collectionNames.Count, "The database should have two collections");
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collectionNames[0], MONGO_COLLECTION_1_NAME + " is missing");
            Assert.AreEqual(MONGO_COLLECTION_2_NAME, collectionNames[1], MONGO_COLLECTION_2_NAME + " is missing");
            Assert.AreEqual("system.indexes", collectionNames[2], "system.indexes missing; was expected to be created");
        }

        [Test]
        public void GetCollectionTest()
        {
            MongoCollection<TestEntry> collection =  _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(0,collection.Count(),"Expected no collections at beginning of test");

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);

            collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(1, collection.Count(), "Expected a collection to be returned");
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collection.Name);

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_2_NAME);
            Assert.AreEqual(1, collection.Count(), "Expected a collection to be returned");
            Assert.AreEqual(MONGO_COLLECTION_2_NAME, collection.Name);
        }

        [Test]
        public void GetCollectionsTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            List<MongoCollection<TestEntry>> results = _mongoDatabaseConnection.GetCollections();

            List<string> collectionNames = _mongoDatabaseConnection.GetCollectionNames();

            Assert.AreEqual(collectionNames.Count(), results.Count(),"Number of named collections should match number of retrieved collections");

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collectionNames[0]);
            Assert.AreEqual(10, results[0].Count());
            Assert.AreEqual(MONGO_COLLECTION_2_NAME, collectionNames[1]);
            Assert.AreEqual(9, results[1].Count());
            Assert.AreEqual("system.indexes", collectionNames[2]);
        }

        [Test]
        public void GetCollectionNamesTest()
        {
            List<string> collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(0, collection.Count(), "Expected no collections at beginning of test");

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);

            collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(2, collection.Count(), "Expected a collection to be returned");
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collection[0]);
            Assert.AreEqual("system.indexes", collection[1]);

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(3, collection.Count(), "Expected a collection to be returned");
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collection[0]);
            Assert.AreEqual(MONGO_COLLECTION_2_NAME, collection[1]);
            Assert.AreEqual("system.indexes", collection[2]);
        }

        [Test]
        public void DropCollectionTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection.DropCollection(MONGO_COLLECTION_1_NAME);

            List<string> collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(2, collection.Count());
            Assert.AreEqual(MONGO_COLLECTION_2_NAME, collection[0]);
            Assert.AreEqual("system.indexes", collection[1]);

            _mongoDatabaseConnection.DropCollection(MONGO_COLLECTION_2_NAME);

            collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(1, collection.Count());
            Assert.AreEqual("system.indexes", collection[0]);
        }

        [Test]
        public void DropAllCollectionsTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection.DropAllCollections();

            List<string> collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(1, collection.Count());//system.indexes remains
        }

        [Test]
        public void ClearCollectionTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);

            MongoCollection<TestEntry> collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);

            Assert.AreEqual(1, collection.Count());

            _mongoDatabaseConnection.ClearCollection(MONGO_COLLECTION_1_NAME);

            collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);

            Assert.AreEqual(0, collection.Count());
        }

        [Test]
        public void ClearAllCollectionsTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            MongoCollection<TestEntry> collection1 = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            MongoCollection<TestEntry> collection2 = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_2_NAME);

            Assert.AreEqual(1, collection1.Count());
            Assert.AreEqual(1, collection2.Count());

            _mongoDatabaseConnection.ClearAllCollections();

            collection1 = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            collection2 = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_2_NAME);

            Assert.AreEqual(0, collection1.Count());
            Assert.AreEqual(0, collection2.Count());
        }

        [Test]
        public void CanConnectTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.IsTrue(_mongoDatabaseConnection.CanConnect(), "Cannot connect to " + MONGO_CONNECTION_STRING);

            _mongoDatabaseConnection.Connect();
            Assert.IsTrue(_mongoDatabaseConnection.CanConnect(), "Cannot connect to " + MONGO_CONNECTION_STRING);
        }

        [Test]
        public void DbTest()
        {
            Assert.IsNotNull(_mongoDatabaseConnection.Db, "Database is null");
            Assert.AreEqual(MONGO_DATABASE_1_NAME, _mongoDatabaseConnection.Db.Name);

            // assign a bad server connection and verify Db is null
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.IsNull(_mongoDatabaseConnection.Db, "Database is null when not connected");

            _mongoDatabaseConnection.Connect();
            Assert.IsNotNull(_mongoDatabaseConnection.Db, "Database is not null");
        }

        [Test]
        public void IndexerTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);//write to the database in order to prove that the fetched database is as expected
            MongoCollection mongoCollection = _mongoDatabaseConnection[MONGO_COLLECTION_1_NAME];

            Assert.IsNotNull(mongoCollection);
            List<string> collectionNames = new List<string>(_mongoDatabaseConnection.GetCollectionNames());
            Assert.AreEqual(2, collectionNames.Count());
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collectionNames[0]);
            Assert.AreEqual("system.indexes", collectionNames[1]);
        }

        [Test]
        public void BadConnectionStringAsync()
        {
            //System.Diagnostics.Debugger.Launch();
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.NotConnected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.Connecting);

            _databaseConnectionAutoResetEvent.WaitOne(); // wait for the async operation to complete to verify it's results
            
            Assert.AreEqual(ConnectionState.NotConnected,_mongoDatabaseConnection.ConnectionState);/**/
            Assert.AreEqual(ConnectionResult.Failure, _databaseConnectionResult);/**/
            Assert.IsNotNull(_serverConnectionReturnMessage);
        }

        // connect to a database asynchronously using an asynchronous server connection
        [Test]
        public void AsynchronousTest1()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(_mongoServerConnection.ConnectionState, ConnectionState.NotConnected);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.NotConnected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.Connecting);

            _databaseConnectionAutoResetEvent.WaitOne();// wait for the async operation to complete so that we can compare the connection state
           
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.Connected);/**/
            Assert.AreEqual(_databaseConnectionResult, ConnectionResult.Success);/**/
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);
        }

        // test an unconnected asynch serverConn injected into an unconnected asynch DatabaseConnection
        [Test]
        public void AsynchronousTest2()
        {         
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(_mongoServerConnection.ConnectionState, ConnectionState.NotConnected);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.NotConnected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.Connecting);

            _databaseConnectionAutoResetEvent.WaitOne();// wait for the async operation to complete so that we can compare the connection state
            
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.Connected);/**/
            Assert.AreEqual(_databaseConnectionResult, ConnectionResult.Success);/**/
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);
        }

        // test a connected asynch serverConn injected into an unconnected asynch DatabaseConnection
        // that is then leveraged by direct usage
        [Test, ExpectedException(typeof(MongoConnectionException), ExpectedMessage = "DatabaseConnection is not connected")]
        public void AsynchronousTest3()
        {
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);

            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            MongoCollection<TestEntry> collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.Fail("The line above should have generated an exception since the DatabaseConnection was not connected");
        }

        // test a connected asynch serverConn injected into a connected asynch DatabaseConnection
        // that is then leveraged by direct usage
        [Test]
        public void AsynchronousTest4()
        {
            //System.Diagnostics.Debugger.Launch();
            // create our asynchronous server connection
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);

            MongoCollection<TestEntry> collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(ConnectionState.Connected, _mongoServerConnection.ConnectionState);
            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);
            Assert.AreEqual(0, collection.Count());
        }

        [Test]
        public void AsynchronousTest5()
        {
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);

            MongoCollection<TestEntry> collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            //Assert.AreEqual(ConnectionResult.Success, _databaseConnectionResult);/**/
            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);
        }

        [Test, ExpectedException(typeof(MongoConnectionException), ExpectedMessage = "DatabaseConnection is not connected")]
        public void AsynchronousTest6()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);/**/
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);

            // once the async operation completes, because the connection string is bad, there is no connection
            // -- attempting to use the connection results in a MongoConnectionException
            MongoCollection<TestEntry> collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
        }

        [Test]
        public void AsynchronousTest7()
        {
            //System.Diagnostics.Debugger.Launch();
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);

            MongoCollection<TestEntry> collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);
            Assert.AreEqual(0, collection.Count());

            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);
            
            collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);
            Assert.AreEqual(0, collection.Count());
        }

        [Test]
        public void AsynchronousTest8()
        {
            //System.Diagnostics.Debugger.Launch();
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);

            MongoCollection<TestEntry> collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);
            Assert.AreEqual(0, collection.Count());

            _mongoServerConnection.Connect();
            _mongoDatabaseConnection.Connect();

            collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);
            Assert.AreEqual(0, collection.Count());

            _mongoServerConnection.Connect();
            _mongoDatabaseConnection.Connect();
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);
            _mongoServerConnection.Connect();
            _mongoDatabaseConnection.Connect();
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);

            collection = _mongoDatabaseConnection.GetCollection(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);
            Assert.AreEqual(0, collection.Count());
        }

        // no thread wait for asynchronous call to complete
        [Test]
        public void Asynchronous_GetInsertedTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(message: entryMessage);

            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);// asynchronous connection 

            _databaseConnectionAutoResetEvent.WaitOne(); // pause here until the asyncConnection completes to allow for linear testability

            Assert.AreEqual(_databaseConnectionResult, ConnectionResult.Success);/**/
            Assert.AreEqual(_mongoDatabaseConnection.ConnectionState, ConnectionState.Connected);
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);
            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            
            // this call doesn't wait for asynchronous connection to finish
           _results.AddRange(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now));
           Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void Asynchronous_GetInsertedTest2()
        {
            //System.Diagnostics.Debugger.Launch();
            string entryMessage = "Hello World";
            AddMongoEntry(message: entryMessage);

            // create our asynchronous server connection
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);

            _databaseConnectionAutoResetEvent.WaitOne();// pause here until the asyncConnection completes to allow for linear testability

            Assert.AreEqual(ConnectionResult.Success, _databaseConnectionResult);/**/
            Assert.AreEqual(ConnectionState.Connected, _mongoDatabaseConnection.ConnectionState);
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);

            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);

            // this call doesn't wait for asynchronous connection to finish
            _results.AddRange(_mongoReader.Read(MONGO_COLLECTION_1_NAME, "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now));

            Assert.AreEqual(1, _results.Count());        
        }

        [Test]
        public void Asynchronous_DependentProcesses1()
        {
            //System.Diagnostics.Debugger.Launch();
            string entryMessage = "Hello World";
            AddMongoEntry(message: entryMessage);

            // create our asynchronous server connection
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection<TestEntry>(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            _mongoReader = new Reader<TestEntry>(_mongoDatabaseConnection);
            _mongoReaderAsync = new ReaderAsync<TestEntry>(_mongoReader);
            _mongoReaderAsync.AsyncReadCompleted += new ReadCompletedEvent<TestEntry>(_mongoReaderAsync_ReadCompleted);

            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsync(_mongoDatabaseConnection_Connected);

            // this call doesn't wait for asynchronous connection to complete
            _mongoReaderAsync.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage);

            _readerAutoResetEvent.WaitOne();// wait for async read to return
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }
    }
}
