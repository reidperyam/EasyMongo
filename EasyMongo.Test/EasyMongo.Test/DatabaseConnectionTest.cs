using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver;
using EasyMongo.Contract;
using EasyMongo.Async;
using EasyMongo.Test.Base;

namespace EasyMongo.Test
{
    [TestFixture]
    public class DatabaseConnectionTest : IntegrationTestFixture
    {
        [Test]
        public void ConnectionStateTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.IsNull(_mongoDatabaseConnection.Db);

            Assert.IsTrue(_mongoDatabaseConnection.CanConnect());

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
            _mongoDatabaseConnection.Connect();

            Assert.AreEqual(MongoServerState.Connected, _mongoDatabaseConnection.State);
            Assert.IsNotNull(_mongoDatabaseConnection.Db);

            Assert.IsTrue(_mongoDatabaseConnection.CanConnect());
        }

        [Test]
        public void GetCollectionNamesForDatabaseTest()
        {
            List<string> collectionNames = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(0, collectionNames.Count, "The database should be empty of collections right now");

            _writer.Write(MONGO_COLLECTION_1_NAME, new Entry());
            collectionNames = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(2, collectionNames.Count, "The database should have one collection");
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collectionNames[0], MONGO_COLLECTION_1_NAME + " is missing");
            Assert.AreEqual("system.indexes", collectionNames[1], "system.indexes missing; was expected to be created");

            _writer.Write(MONGO_COLLECTION_2_NAME, new Entry());
            collectionNames = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(3, collectionNames.Count, "The database should have two collections");
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collectionNames[0], MONGO_COLLECTION_1_NAME + " is missing");
            Assert.AreEqual(MONGO_COLLECTION_2_NAME, collectionNames[1], MONGO_COLLECTION_2_NAME + " is missing");
            Assert.AreEqual("system.indexes", collectionNames[2], "system.indexes missing; was expected to be created");
        }

        [Test]
        public void GetCollectionTest()
        {
            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(0,collection.Count(),"Expected no collections at beginning of test");

            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);

            collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(1, collection.Count(), "Expected a collection to be returned");
            Assert.AreEqual(MONGO_COLLECTION_1_NAME, collection.Name);

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_2_NAME);
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

            List<MongoCollection<Entry>> results = _mongoDatabaseConnection.GetCollections<Entry>();

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

            _mongoDatabaseConnection.DropCollection<Entry>(MONGO_COLLECTION_1_NAME);

            List<string> collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(2, collection.Count());
            Assert.AreEqual(MONGO_COLLECTION_2_NAME, collection[0]);
            Assert.AreEqual("system.indexes", collection[1]);

            _mongoDatabaseConnection.DropCollection<Entry>(MONGO_COLLECTION_2_NAME);

            collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(1, collection.Count());
            Assert.AreEqual("system.indexes", collection[0]);
        }

        [Test]
        public void DropAllCollectionsTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _mongoDatabaseConnection.DropAllCollections<Entry>();

            List<string> collection = _mongoDatabaseConnection.GetCollectionNames();
            Assert.AreEqual(1, collection.Count());//system.indexes remains
        }

        [Test]
        public void ClearCollectionTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);

            MongoCollection<Entry> collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);

            Assert.AreEqual(1, collection.Count());

            _mongoDatabaseConnection.ClearCollection<Entry>(MONGO_COLLECTION_1_NAME);

            collection = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);

            Assert.AreEqual(0, collection.Count());
        }

        [Test]
        public void ClearAllCollectionsTest()
        {
            AddMongoEntry(collectionName: MONGO_COLLECTION_1_NAME);
            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            MongoCollection<Entry> collection1 = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            MongoCollection<Entry> collection2 = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_2_NAME);

            Assert.AreEqual(1, collection1.Count());
            Assert.AreEqual(1, collection2.Count());

            _mongoDatabaseConnection.ClearAllCollections<Entry>();

            collection1 = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_1_NAME);
            collection2 = _mongoDatabaseConnection.GetCollection<Entry>(MONGO_COLLECTION_2_NAME);

            Assert.AreEqual(0, collection1.Count());
            Assert.AreEqual(0, collection2.Count());
        }

        [Test]
        public void CanConnectTest()
        {
            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
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

            _mongoDatabaseConnection = new DatabaseConnection(_mongoServerConnection, MONGO_DATABASE_1_NAME);
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
    }
}
