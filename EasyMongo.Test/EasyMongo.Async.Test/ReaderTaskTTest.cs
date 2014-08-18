using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using EasyMongo.Test.Base;

namespace EasyMongo.Async.Test
{
    [TestFixture]
    public class ReaderTaskTTest : IntegrationTestFixture
    {
        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public async void ReadTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            IEnumerable<Entry> results = await _readerTaskT.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summar
        [Test]
        public async void ReadTest2()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            IEnumerable<Entry> results = await _readerTaskT.ReadAsync(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public async void ReadTest3()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            IEnumerable<Entry> results = await _readerTaskT.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public async void ReadTest4()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            IEnumerable<Entry> results = await _readerTaskT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", entryMessage);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            results = await _readerTaskT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", entryMessage);

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage2, results.ElementAt(1).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public async void ReadTest5()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            IEnumerable<Entry> results = await _readerTaskT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            results = await _readerTaskT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage2, results.ElementAt(1).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public async void ReadTest6()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            IEnumerable<Entry> results = await _readerTaskT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            results = await _readerTaskT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage2, results.ElementAt(1).Message);
        }

        /// <summary>
        /// Fail a read operation and examine the resultant _asyncException that's handled by the async callback method
        /// </summary>
        /// <remarks>Requires manual exception generation from within AsyncDelegateReader<T>.Callback() in order to generate exception handling/></remarks>
        [Test, Ignore]
        public async void ReadTest7()
        {
            System.Diagnostics.Debugger.Launch();
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            IEnumerable<Entry> results = await _readerTaskT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(0, results.Count());
            Assert.AreEqual(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException), _asyncException.GetType());
        }

        #region    Disctinct
        [Test]
        public async void DistinctTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            IEnumerable<string> results = await _readerTaskT.DistinctAsync<string>(MONGO_COLLECTION_1_NAME, "Message");

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
            Assert.AreEqual("Two", results.ElementAt(1));
            Assert.AreEqual("Three", results.ElementAt(2));
        }

        [Test]
        public async void DistinctTest2()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            IEnumerable<string> results = await _readerTaskT.DistinctAsync<string>(MONGO_COLLECTION_1_NAME, "Message", searchQuery);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
        }

        [Test]
        public async void DistinctTest3()
        {
            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            IEnumerable<string> results = await _readerTaskT.DistinctAsync<string>(collections, "Message");

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
            Assert.AreEqual("Two", results.ElementAt(1));
            Assert.AreEqual("Three", results.ElementAt(2));
        }

        [Test]
        public async void DistinctTest4()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            IEnumerable<string> results = await _readerTaskT.DistinctAsync<string>(collections, "Message", searchQuery);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
        }
        #endregion Distinct
    }
}
