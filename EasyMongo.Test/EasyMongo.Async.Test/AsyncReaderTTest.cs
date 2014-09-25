using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using EasyMongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using EasyMongo.Test.Base;

namespace EasyMongo.Async.Test
{
    [TestFixture]
    public class AsyncReaderTTest : IntegrationTestFixture
    {
        #region    ReadAsync
        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public async void ReadAsyncTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summar
        [Test]
        public async void ReadAsyncTest2()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public async void ReadAsyncTest3()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public async void ReadAsyncTest4()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", entryMessage);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            results = await _asyncReaderT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", entryMessage);

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage2, results.ElementAt(1).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public async void ReadAsyncTest5()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            results = await _asyncReaderT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage2, results.ElementAt(1).Message);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public async void ReadAsyncTest6()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(entryMessage, results.ElementAt(0).Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            results = await _asyncReaderT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(2, results.Count());
            Assert.AreEqual(entryMessage2, results.ElementAt(1).Message);
        }

        /// <summary>
        /// Fail a read operation and examine the resultant _asyncException that's handled by the async callback method
        /// </summary>
        /// <remarks>Requires manual exception generation from within AsyncDelegateReader<T>.Callback() in order to generate exception handling/></remarks>
        [Test, Ignore]
        public async void ReadAsyncTest7()
        {
            System.Diagnostics.Debugger.Launch();
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);

            Assert.AreEqual(0, results.Count());
            Assert.AreEqual(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException), _asyncException.GetType());
        }

        /// <summary>
        /// Writes a MongoTestEntry within two collections and verifies both retrieved using
        /// Read method to retrieve all records within multiple collections
        /// </summary>
        [Test]
        public async void ReadAsyncTest8()
        {
            AddMongoEntry("Hello World 1", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Hello World 2", MONGO_COLLECTION_2_NAME);

            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(new string[] { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME });
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual("Hello World 1", results.ElementAt(0).Message);
            Assert.AreEqual("Hello World 2", results.ElementAt(1).Message);
        }

        /// <summary>
        /// Writes MongoTestEntrys within a collection and verifies both are retrieved using
        /// Read method to retrieve all records within a collection
        /// </summary>
        [Test]
        public async void ReadAsyncTest9()
        {
            AddMongoEntry("Hello World 1", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Hello World 2", MONGO_COLLECTION_1_NAME);

            IEnumerable<Entry> results = await _asyncReaderT.ReadAsync(MONGO_COLLECTION_1_NAME);
            Assert.AreEqual(2, results.Count());
            Assert.AreEqual("Hello World 1", results.ElementAt(0).Message);
            Assert.AreEqual("Hello World 2", results.ElementAt(1).Message);
        }
        #endregion ReadAsync

        #region    DistinctAsync
        [Test]
        public async void DistinctAsyncTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            IEnumerable<string> results = await _asyncReaderT.DistinctAsync<string>(MONGO_COLLECTION_1_NAME, "Message");

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
            Assert.AreEqual("Two", results.ElementAt(1));
            Assert.AreEqual("Three", results.ElementAt(2));
        }

        [Test]
        public async void DistinctAsyncTest2()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            IEnumerable<string> results = await _asyncReaderT.DistinctAsync<string>(MONGO_COLLECTION_1_NAME, "Message", searchQuery);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
        }

        [Test]
        public async void DistinctAsyncTest3()
        {
            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            IEnumerable<string> results = await _asyncReaderT.DistinctAsync<string>(collections, "Message");

            Assert.AreEqual(3, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
            Assert.AreEqual("Two", results.ElementAt(1));
            Assert.AreEqual("Three", results.ElementAt(2));
        }

        [Test]
        public async void DistinctAsyncTest4()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            IEnumerable<string> results = await _asyncReaderT.DistinctAsync<string>(collections, "Message", searchQuery);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual("One", results.ElementAt(0));
        }
        #endregion DistinctAsync

        #region    ExecuteAsync
        [Test]
        public async void ExecuteAsyncTest1()
        {
            AddMongoEntry("Hello World 1");

            IMongoQuery query = Query.Matches("Message", new BsonRegularExpression("WORLD", "i"));

            _results.AddRange(await _asyncReaderT.ExecuteAsync(MONGO_COLLECTION_1_NAME, query));
            Assert.AreEqual(1, _results.Count());
            Assert.AreEqual("Hello World 1", _results[0].Message);
        }
        #endregion ExecuteAsync
    }
}
