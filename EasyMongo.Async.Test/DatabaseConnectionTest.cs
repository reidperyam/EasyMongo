using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Contract.Deprecated;
using EasyMongo.Async;
using EasyMongo.Async.Deprecated;
using EasyMongo.Test.Base;

namespace EasyMongo.Async.Test
{
    [TestFixture]
    public class DatabaseConnectionTest : IntegrationTestFixture
    {
        // connect to a database asynchronously using an asynchronous server connection
        [Test]
        public void AsynchronousTest1()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(_mongoServerConnection.State, MongoServerState.Disconnected);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(_mongoDatabaseConnection.State, MongoServerState.Disconnected);
            _mongoDatabaseConnection.ConnectAsyncTask();
            Thread.Sleep(100);// wait for the async operation to complete so that we can compare the connection state

            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
        }

        // test an unconnected asynch serverConn injected into an unconnected asynch DatabaseConnection
        [Test]
        public void AsynchronousTest2()
        {         
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(_mongoServerConnection.State, MongoServerState.Disconnected);

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(MongoServerState.Disconnected, _mongoDatabaseConnection.State);
            _mongoDatabaseConnection.ConnectAsyncTask();           
            Thread.Sleep(100);// wait for the async operation to complete so that we can compare the connection state

            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
        }

        // test a connected asynch serverConn injected into an unconnected asynch DatabaseConnection
        // that is then leveraged by direct usage
        [Test, ExpectedException(typeof(MongoConnectionException), ExpectedMessage = "DatabaseConnection is not connected")]
        public void AsynchronousTest3()
        {
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.Fail("The line above should have generated an exception since the DatabaseConnection was not connected");
        }

        // test a connected asynch serverConn injected into a connected asynch DatabaseConnection
        // that is then leveraged by direct usage
        [Test]
        public void AsynchronousTest4()
        {
            // create our asynchronous server connection
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            _mongoServerConnection.ConnectAsyncTask();
            _mongoDatabaseConnection.ConnectAsyncTask();

            Thread.Sleep(100);

            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoServerConnection.State);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.AreEqual(0, collection.Count());
        }

        [Test]
        public void AsynchronousTest5()
        {
            _mongoDatabaseConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
        }

        [Test, ExpectedException(typeof(MongoConnectionException), ExpectedMessage = "ServerConnection is not connected")]
        public void AsynchronousTest6()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);/**/
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncTask();
            _mongoDatabaseConnection.ConnectAsyncTask();

            // once the async operation completes, because the connection string is bad, there is no connection
            // -- attempting to use the connection results in a MongoConnectionException
            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
        }

        [Test]
        public void AsynchronousTest7()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);

            _mongoServerConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            _mongoDatabaseConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            

            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.AreEqual(0, collection.Count());

            _mongoServerConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            _mongoDatabaseConnection.ConnectAsyncTask();
            Thread.Sleep(100);

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

            _mongoServerConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            _mongoDatabaseConnection.ConnectAsyncTask();
            Thread.Sleep(100);

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
            _mongoServerConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            _mongoDatabaseConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            _mongoServerConnection.Connect();
            _mongoDatabaseConnection.Connect();
            _mongoServerConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            _mongoDatabaseConnection.ConnectAsyncTask();
            Thread.Sleep(100);

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

            _mongoDatabaseConnection.ConnectAsyncTask();// asynchronous connection 

            Thread.Sleep(100);

            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
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
            _mongoServerConnection.ConnectAsyncTask();
            Thread.Sleep(100);
            _mongoDatabaseConnection.ConnectAsyncTask();
            Thread.Sleep(100);

            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);

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

            _reader = new Reader(_mongoDatabaseConnection);
            _readerAsync = new ReaderAsync(_reader);
            _readerAsync.AsyncReadCompleted += new ReadCompletedEvent(_reader_AsyncReadCompleted);

            // testBase class receives the connection call back after the asynch connection occurs
            _mongoServerConnection.ConnectAsyncTask();
            _mongoDatabaseConnection.ConnectAsyncTask();

            // this call doesn't wait for asynchronous connection to complete
            _readerAsync.ReadAsync<Entry>(MONGO_COLLECTION_1_NAME, "Message", entryMessage);

            _readerAutoResetEvent.WaitOne();// wait for async read to return
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }
    }
}
