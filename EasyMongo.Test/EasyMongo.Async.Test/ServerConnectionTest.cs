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

namespace EasyMongo.Async.Test
{
    [TestFixture]
    public class ServerConnectionTest : IntegrationTestFixture
    {
        #region Asynchronous

        [Test]
        public void AsynchronousTest1()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(MongoServerState.Disconnected, _mongoServerConnection.State);
            _mongoServerConnection.ConnectAsyncTask();
            System.Threading.Thread.Sleep(200);
            Assert.AreEqual(MongoServerState.Connected, _mongoServerConnection.State);
        }

        [Test]
        public void AsynchronousTest2()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING);
            Assert.AreEqual(ConnectionResult.Empty, _serverConnectionResult);
            _mongoServerConnection.ConnectAsyncTask();
            System.Threading.Thread.Sleep(200);
            List<string> returned = _mongoServerConnection.GetDbNamesForConnection();
            Assert.AreEqual(MongoServerState.Connected, _mongoServerConnection.State);
            Assert.AreEqual(0,returned.Count()) ;
            AddMongoEntry();

            returned = _mongoServerConnection.GetDbNamesForConnection();

            Assert.AreEqual(1,returned.Count());
        }

        [Test]
        public void AsynchronousTest3()
        {
            _mongoServerConnection = new ServerConnection(MONGO_CONNECTION_STRING_BAD);
            _mongoServerConnection.ConnectAsyncTask();

            Assert.AreEqual(MongoServerState.Disconnected, _mongoServerConnection.State);
            Assert.IsNotNull(_serverConnectionReturnMessage);
        }

        #endregion Asynchronous
    }
}
