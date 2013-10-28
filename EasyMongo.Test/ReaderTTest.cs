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
    public class ReaderTTest : IntegrationTestFixture
    {
        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public void ReadTest1()
        {
            AddMongoEntry();

            _results.AddRange(_readerT.Read(MONGO_COLLECTION_1_NAME, "Message", "Hello"));
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

            _results.AddRange(_readerT.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
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

            _results.AddRange(_readerT.Read(MONGO_COLLECTION_1_NAME, "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now));
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

            _results.AddRange(_readerT.Read(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello"));
            Assert.AreEqual(1, _results.Count());

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            _results.Clear();

            _results.AddRange(_readerT.Read(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello"));
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

            _results.AddRange(_readerT.Read(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);
            _results.Clear();

            _results.AddRange(_readerT.Read(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now));
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

            _results.AddRange(_readerT.Read(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
            _results.Clear();

            AddMongoEntry(collectionName: MONGO_COLLECTION_2_NAME);

            _results.AddRange(_readerT.Read(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(2, _results.Count());
        }

        #region    Distinct 
        [Test]
        public void DistinctTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            List<String> list = new List<String>(_readerT.Distinct<String>(MONGO_COLLECTION_1_NAME, "Message"));
            Assert.AreEqual(3, list.Count());
            Assert.AreEqual("One", list[0]);
            Assert.AreEqual("Two", list[1]);
            Assert.AreEqual("Three", list[2]);
        }

        [Test]
        public void DistinctTest2()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));
            
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            List<String> list = new List<String>(_readerT.Distinct<String>(MONGO_COLLECTION_1_NAME, "Message", searchQuery));
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("One", list[0]);
        }

        [Test]
        public void DistinctTest3()
        {
            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            List<String> list = new List<String>(_readerT.Distinct<String>(collections, "Message"));
            Assert.AreEqual(3, list.Count());
            Assert.AreEqual("One", list[0]);
            Assert.AreEqual("Two", list[1]);
            Assert.AreEqual("Three", list[2]);
        }

        [Test]
        public void DistinctTest4()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            List<String> list = new List<String>(_readerT.Distinct<String>(collections, "Message", searchQuery));
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("One", list[0]);
        }
        #endregion Distinct
    }
}
