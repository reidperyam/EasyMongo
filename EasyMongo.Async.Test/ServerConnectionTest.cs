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
            Assert.AreEqual(ConnectionState.NotConnected, _mongoServerConnection.ConnectionState);
            Assert.AreEqual(ConnectionResult.Empty,_serverConnectionResult);
            _mongoServerConnection.ConnectAsync(_mongoServerConnection_Connected);
            Assert.IsTrue(_mongoServerConnection.ConnectionState == ConnectionState.Connecting || // these asserts are variable becuase this is a race condition...
                          _mongoServerConnection.ConnectionState == ConnectionState.Connected);   // crappy test design but it's meant to give a view into use case
            Assert.IsTrue(_serverConnectionResult == ConnectionResult.Empty ||                    // considerations
                          _serverConnectionResult == ConnectionResult.Success);
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
