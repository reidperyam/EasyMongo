using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using EasyMongo.Test.Base;
using EasyMongo.Test.Model;

namespace EasyMongo.Test
{
    [TestFixture]
    public class ReaderTest : IntegrationTestFixture
    {
        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public void ReadTest1()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read<TestEntry>(MONGO_COLLECTION_1_NAME, "Message", "Hello"));
            Assert.AreEqual(1, _results.Count());
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public void ReadTest2()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read<TestEntry>(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public void ReadTest3()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read<TestEntry>(MONGO_COLLECTION_1_NAME, "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public void ReadTest4()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read<TestEntry>(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello"));
            Assert.AreEqual(1, _results.Count());

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            _results.Clear();

            _results.AddRange(_reader.Read<TestEntry>(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello"));
            Assert.AreEqual(2, _results.Count());
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public void ReadTest5()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read<TestEntry>(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            _results.Clear();

            _results.AddRange(_reader.Read<TestEntry>(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, _results.Count());
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public void ReadTest6()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read<TestEntry>(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
            _results.Clear();

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _results.AddRange(_reader.Read<TestEntry>(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, _results.Count());
        }

        [Test]
        public void DistinctBSONTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");
            List<BsonValue> list = new List<BsonValue>(_reader.Distinct(MONGO_COLLECTION_1_NAME,"Message"));
            Assert.AreEqual(3, list.Count());
            Assert.AreEqual("One",   list[0].AsString);
            Assert.AreEqual("Two",   list[1].AsString);
            Assert.AreEqual("Three", list[2].AsString);
        }

        [Test]
        public void DistinctBSONTest2()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");
            List<BsonValue> list = new List<BsonValue>(_reader.Distinct(MONGO_COLLECTION_1_NAME, "Message", searchQuery));
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("One", list[0].AsString);
        }

        [Test]
        public void DistinctBSONTest3()
        {
            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            List<BsonValue> list = new List<BsonValue>(_reader.Distinct(collections, "Message"));
            Assert.AreEqual(3, list.Count());
            Assert.AreEqual("One", list[0].AsString);
            Assert.AreEqual("Two", list[1].AsString);
            Assert.AreEqual("Three", list[2].AsString);
        }

        [Test]
        public void DistinctBSONTest4()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            List<BsonValue> list = new List<BsonValue>(_reader.Distinct(collections, "Message", searchQuery));
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("One", list[0].AsString);
        }       
    }
}
