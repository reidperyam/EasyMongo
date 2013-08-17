using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using NUnit.Framework;
using MongoDB;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace MongoDB.Test
{
    [TestFixture]
    public class ReaderAsyncTest : TestBase
    {
        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public void ReadTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            _mongoReaderAsync.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
            Assert.IsNull(_asyncException);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public void ReadTest2()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            _mongoReaderAsync.ReadAsync(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
            Assert.IsNull(_asyncException);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against a single collection
        /// </summary>
        [Test]
        public void ReadTest3()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage);
            _mongoReaderAsync.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
            Assert.IsNull(_asyncException);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public void ReadTest4()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _mongoReaderAsync.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
            Assert.IsNull(_asyncException);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            _mongoReaderAsync.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(2, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage2, _asyncReadResults[1].Message);
            Assert.IsNull(_asyncException);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public void ReadTest5()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _mongoReaderAsync.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
            Assert.IsNull(_asyncException);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            _mongoReaderAsync.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(2, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage2, _asyncReadResults[1].Message);
            Assert.IsNull(_asyncException);
        }

        /// <summary>
        /// Writes a MongoTestEntry to a MongoDB and verifies that it was asynchronously retrieved using
        /// Read method to search against multiple collections
        /// </summary>
        [Test]
        public void ReadTest6()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _mongoReaderAsync.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
            Assert.IsNull(_asyncException);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            _mongoReaderAsync.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(2, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage2, _asyncReadResults[1].Message);
            Assert.IsNull(_asyncException);
        }

        /// <summary>
        /// Fail a read operation and examine the resultant _asyncException that's handled by the async callback method
        /// </summary>
        /// <remarks>Requires manual exception generation from within ReaderAsync<T>.Callback() in order to generate exception handling/></remarks>
        [Test, Explicit]
        public void ReadTest7()
        {
            System.Diagnostics.Debugger.Launch();
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _mongoReaderAsync.ReadAsync(_mongoDatabaseConnection.Db.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(0, _asyncReadResults.Count());
            Assert.IsNotNull(_asyncException);
            Assert.AreEqual(typeof(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException), _asyncException.GetType());
        }

        [Test]
        public void DistinctTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            _mongoReaderAsync.DistinctAsync(MONGO_COLLECTION_1_NAME, "Message");
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(3, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
            Assert.AreEqual("Two", _asyncDistinctResults[1].AsString);
            Assert.AreEqual("Three", _asyncDistinctResults[2].AsString);
            Assert.IsNull(_asyncException);
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

            _mongoReaderAsync.DistinctAsync(MONGO_COLLECTION_1_NAME, "Message", searchQuery);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
            Assert.IsNull(_asyncException);
        }

        [Test]
        public void DistinctTest3()
        {
            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            _mongoReaderAsync.DistinctAsync(collections, "Message");
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(3, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
            Assert.AreEqual("Two", _asyncDistinctResults[1].AsString);
            Assert.AreEqual("Three", _asyncDistinctResults[2].AsString);
            Assert.IsNull(_asyncException);
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
            _mongoReaderAsync.DistinctAsync(collections, "Message", searchQuery);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
            Assert.IsNull(_asyncException);
        }  
    }
}
