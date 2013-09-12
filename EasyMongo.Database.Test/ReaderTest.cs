using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using EasyMongo.Base.Test;

namespace EasyMongo.Database.Test
{
    [TestFixture]
    public class ReaderTest : TestBase
    {
        [Test]
        public void ConstructorTest()
        {
            _databaseReader = new Reader<TestEntry>(MONGO_CONNECTION_STRING, MONGO_DATABASE_1_NAME);
        }

        [Test]
        public void ReadAsyncTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsync(message:entryMessage);
            _databaseReader.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }

        [Test]
        public void ReadAsyncTest2()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsync(message: entryMessage);
            _databaseReader.ReadAsync(MONGO_COLLECTION_1_NAME, "Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }

        [Test]
        public void ReadAsyncTest3()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _databaseReader.ReadAsync(_mongoDatabaseConnection.GetCollectionNames(), "Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            _databaseReader.ReadAsync(_mongoDatabaseConnection.GetCollectionNames(), "Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(2, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage2, _asyncReadResults[1].Message);
        }

        [Test]
        public void ReadAsyncTest4()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _databaseReader.ReadAsync(_mongoDatabaseConnection.GetCollectionNames(), "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            _databaseReader.ReadAsync(_mongoDatabaseConnection.GetCollectionNames(), "Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(2, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage2, _asyncReadResults[1].Message);
        }

        [Test]
        public void ReadAsyncTest5()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _databaseReader.ReadAsync(_mongoDatabaseConnection.GetCollectionNames(), "Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            _databaseReader.ReadAsync(_mongoDatabaseConnection.GetCollectionNames(), "Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(2, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage2, _asyncReadResults[1].Message);
        }

        [Test]
        public void ReadAsyncTest6()
        {
            string entryMessage = "Hello World";
            AddMongoEntry(entryMessage, MONGO_COLLECTION_1_NAME);
            _databaseReader.ReadAsync(_mongoDatabaseConnection.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);

            _asyncReadResults.Clear();

            string entryMessage2 = "Hello World Again";
            AddMongoEntry(entryMessage2, MONGO_COLLECTION_2_NAME);
            _databaseReader.ReadAsync(_mongoDatabaseConnection.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(2, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage2, _asyncReadResults[1].Message);
        }

        [Test]
        public void ReadTest1()
        {
            AddMongoEntry();

            _results.AddRange(_databaseReader.Read(MONGO_COLLECTION_1_NAME, "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest2()
        {
            AddMongoEntry();

            _results.AddRange(_databaseReader.Read(MONGO_COLLECTION_1_NAME, "Message", "Hello"));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest3()
        {
            AddMongoEntry();

            _results.AddRange(_databaseReader.Read(MONGO_COLLECTION_1_NAME, "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest4()
        {
            AddMongoEntry();

            _results.AddRange(_databaseReader.Read(_mongoDatabaseConnection.GetCollectionNames(), "Message", "Hello", "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest5()
        {
            AddMongoEntry();

            _results.AddRange(_databaseReader.Read(_mongoDatabaseConnection.GetCollectionNames(), "Message", "Hello"));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest6()
        {
            AddMongoEntry();

            _results.AddRange(_databaseReader.Read(_mongoDatabaseConnection.GetCollectionNames(), "TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void Distinct1Test()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");
            List<BsonValue> list = new List<BsonValue>(_databaseReader.Distinct(MONGO_COLLECTION_1_NAME, "Message"));
            Assert.AreEqual(3, list.Count());
            Assert.AreEqual("One", list[0].AsString);
            Assert.AreEqual("Two", list[1].AsString);
            Assert.AreEqual("Three", list[2].AsString);
        }

        [Test]
        public void Distinct2Test()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");
            List<BsonValue> list = new List<BsonValue>(_databaseReader.Distinct(MONGO_COLLECTION_1_NAME, "Message", searchQuery));
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("One", list[0].AsString);
        }

        [Test]
        public void Distinct3Test()
        {
            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            List<BsonValue> list = new List<BsonValue>(_databaseReader.Distinct(collections, "Message"));
            Assert.AreEqual(3, list.Count());
            Assert.AreEqual("One", list[0].AsString);
            Assert.AreEqual("Two", list[1].AsString);
            Assert.AreEqual("Three", list[2].AsString);
        }

        [Test]
        public void Distinct4Test()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            List<BsonValue> list = new List<BsonValue>(_databaseReader.Distinct(collections, "Message", searchQuery));
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("One", list[0].AsString);
        }

        [Test]
        public void DistinctAsync1Test()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            _databaseReader.DistinctAsync(MONGO_COLLECTION_1_NAME, "Message");
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(3, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
            Assert.AreEqual("Two", _asyncDistinctResults[1].AsString);
            Assert.AreEqual("Three", _asyncDistinctResults[2].AsString);
        }

        [Test]
        public void DistinctAsync2Test()
        {
            //System.Diagnostics.Debugger.Launch();
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            _databaseReader.DistinctAsync(MONGO_COLLECTION_1_NAME, "Message", searchQuery);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
        }

        [Test]
        public void DistinctAsync3Test()
        {
            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            _databaseReader.DistinctAsync(collections, "Message");
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(3, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
            Assert.AreEqual("Two", _asyncDistinctResults[1].AsString);
            Assert.AreEqual("Three", _asyncDistinctResults[2].AsString);
        }

        [Test]
        public void DistinctAsync4Test()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("One", MONGO_COLLECTION_2_NAME);
            AddMongoEntry("Two", MONGO_COLLECTION_1_NAME);
            AddMongoEntry("Three", MONGO_COLLECTION_2_NAME);

            List<string> collections = new List<string>() { MONGO_COLLECTION_1_NAME, MONGO_COLLECTION_2_NAME };
            _databaseReader.DistinctAsync(collections, "Message", searchQuery);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
        } 

    }
}
