using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Contract.Delegates;
using EasyMongo.Async;
using EasyMongo.Async.Delegates;
using EasyMongo.Test.Base;

namespace EasyMongo.Async.Delegates.Test
{
    [TestFixture]
    public class DatabaseConnectionTest : IntegrationTestFixture
    {
        [Test]
        public void BadConnectionStringAsync()
        {
            //System.Diagnostics.Debugger.Launch();
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(MongoServerState.Disconnected, _mongoDatabaseConnection.State);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);
            Assert.AreEqual(MongoServerState.Disconnected, _mongoDatabaseConnection.State);

            _databaseConnectionAutoResetEvent.WaitOne(); // wait for the async operation to complete to verify it's results

            Assert.AreEqual(MongoServerState.Disconnected, _mongoDatabaseConnection.State);/**/
            Assert.AreEqual(ConnectionResult.Failure, _databaseConnectionResult);/**/
            Assert.IsNotNull(_serverConnectionReturnMessage);
        }

        // connect to a database asynchronously using an asynchronous server connection
        [Test]
        public void AsynchronousTest1()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(MongoServerState.Disconnected, _mongoServerConnection.State);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(_mongoDatabaseConnection.State, MongoServerState.Disconnected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);
            Assert.AreEqual(MongoServerState.Disconnected, _mongoDatabaseConnection.State);

            _databaseConnectionAutoResetEvent.WaitOne();// wait for the async operation to complete so that we can compare the connection state

            Assert.AreEqual(_mongoDatabaseConnection.State, MongoServerState.Connected);/**/
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
            Assert.AreEqual(_mongoServerConnection.State, MongoServerState.Disconnected);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(MongoServerState.Disconnected,_mongoDatabaseConnection.State);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);
            Assert.AreEqual(MongoServerState.Disconnected, _mongoDatabaseConnection.State);

            _databaseConnectionAutoResetEvent.WaitOne();// wait for the async operation to complete so that we can compare the connection state

            Assert.AreEqual(_mongoDatabaseConnection.State, MongoServerState.Connected);/**/
            Assert.AreEqual(_databaseConnectionResult, ConnectionResult.Success);/**/
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);
        }

        // test a connected asynch serverConn injected into an unconnected asynch DatabaseConnection
        // that is then leveraged by direct usage
        [Test, ExpectedException(typeof(MongoConnectionException), ExpectedMessage = "ServerConnection is not connected")]
        public void AsynchronousTest3()
        {
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
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
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);

            _databaseConnectionAutoResetEvent.WaitOne();

            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoServerConnection.State);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);
            Assert.AreEqual(0, collection.Count());
        }

        [Test]
        public void AsynchronousTest5()
        {
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);
            _databaseConnectionAutoResetEvent.WaitOne();
            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            //Assert.AreEqual(ConnectionResult.Success, _databaseConnectionResult);/**/
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
        }

        [Test, ExpectedException(typeof(MongoConnectionException), ExpectedMessage = "ServerConnection is not connected")]
        public void AsynchronousTest6()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);/**/
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);

            // once the async operation completes, because the connection string is bad, there is no connection
            // -- attempting to use the connection results in a MongoConnectionException
            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
        }

        [Test]
        public void AsynchronousTest7()
        {
            //System.Diagnostics.Debugger.Launch();
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);

            _databaseConnectionAutoResetEvent.WaitOne();

            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.AreEqual(0, collection.Count());

            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);

            _databaseConnectionAutoResetEvent.WaitOne();

            collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.AreEqual(0, collection.Count());
        }

        [Test]
        public void AsynchronousTest8()
        {
            //System.Diagnostics.Debugger.Launch();
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);

            _databaseConnectionAutoResetEvent.WaitOne();

            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.AreEqual(0, collection.Count());

            _mongoServerConnection.Connect();
            _mongoDatabaseConnection.Connect();

            collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.AreEqual(0, collection.Count());

            _mongoServerConnection.Connect();
            _mongoDatabaseConnection.Connect();
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);
            _mongoServerConnection.Connect();
            _mongoDatabaseConnection.Connect();
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);

            _databaseConnectionAutoResetEvent.WaitOne();

            collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.AreEqual(0, collection.Count());
        }

        // no thread wait for asynchronous call to complete
        [Test]
        public void Asynchronous_GetInsertedTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(message: entryMessage);

            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);// asynchronous connection 

            _databaseConnectionAutoResetEvent.WaitOne(); // pause here until the asyncConnection completes to allow for linear testability

            Assert.AreEqual(_databaseConnectionResult, ConnectionResult.Success);/**/
            Assert.AreEqual(_mongoDatabaseConnection.State, MongoServerState.Connected);
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);
            _reader = new Reader(_mongoDatabaseConnection);
            
            // this call doesn't wait for asynchronous connection to finish
           _results.AddRange(_reader.Read<Entry>(MONGO_COLLECTION_1_NAME, "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now));
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
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);

            _databaseConnectionAutoResetEvent.WaitOne();// pause here until the asyncConnection completes to allow for linear testability

            Assert.AreEqual(ConnectionResult.Success, _databaseConnectionResult);/**/
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.IsNotNull(_serverConnectionReturnMessage);
            Assert.IsNotNull(_databaseConnectionReturnMessage);

            _reader = new Reader(_mongoDatabaseConnection);

            // this call doesn't wait for asynchronous connection to finish
            _results.AddRange(_reader.Read<Entry>(MONGO_COLLECTION_1_NAME, "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now));

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
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            //_reader = new Reader(_mongoDatabaseConnection);
            //_readerAsync = new ReaderAsync(_reader);
            //_readerAsync.AsyncReadCompleted += new ReadCompletedEvent(_reader_AsyncReadCompleted);

            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncDelegate(_mongoServerConnection_Connected);
            _mongoDatabaseConnection.ConnectAsyncDelegate(_mongoDatabaseConnection_Connected);

            // this call doesn't wait for asynchronous connection to complete
            _readerAsync.ReadAsync<Entry>(MONGO_COLLECTION_1_NAME, "Message", entryMessage);

            _readerAutoResetEvent.WaitOne();// wait for async read to return
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }
    }
}
