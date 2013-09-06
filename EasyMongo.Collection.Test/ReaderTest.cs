using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EasyMongo.Contract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace EasyMongo.Collection.Test
{
    [TestFixture]
    public class ReaderTest : TestBase
    {        
        [Test]
        public void ConstructorTest()
        {
            IDatabaseReader<TestEntry> databaseReader = new Database.Reader<TestEntry>(MONGO_CONNECTION_STRING, MONGO_DATABASE_1_NAME);
            _reader = new Collection.Reader<TestEntry>(databaseReader, MONGO_COLLECTION_1_NAME);
        }

        [Test]
        public void ReadAsyncTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsync(message: entryMessage);
            _reader.ReadAsync("TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }

        [Test]
        public void ReadAsyncTest2()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsync(message: entryMessage);
            _reader.ReadAsync("Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }

        [Test]
        public void ReadAsyncTest3()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsync(message: entryMessage);
            _reader.ReadAsync("Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }

        [Test]
        public void ReadTest1()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest2()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read("Message", "Hello"));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest3()
        {
            AddMongoEntry();

            _results.AddRange(_reader.Read("Message","Hello","TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void Distinct1Test()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");
            List<BsonValue> list = new List<BsonValue>(_reader.Distinct("Message"));
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
            List<BsonValue> list = new List<BsonValue>(_reader.Distinct("Message", searchQuery));
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

            _reader.DistinctAsync("Message");
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(3, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
            Assert.AreEqual("Two", _asyncDistinctResults[1].AsString);
            Assert.AreEqual("Three", _asyncDistinctResults[2].AsString);
        }

        [Test]
        public void DistinctAsync2Test()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            _reader.DistinctAsync("Message", searchQuery);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0].AsString);
        }
    }
}
