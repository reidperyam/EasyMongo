using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EasyMongo.Contract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using EasyMongo.Test.Base;

namespace EasyMongo.Collection.Test
{
    [TestFixture]
    public class CollectionReaderTTest : IntegrationTestFixture
    {        
        [Test]
        public void ConstructorTest()
        {
            // construct a collection reader from a database reader and a collection name
            _collectionReaderT = new CollectionReader<Entry>(_collectionReader);
            Assert.IsNotNull(_collectionReader);
        }

        [Test]
        public void ReadAsyncTest1()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsyncDelegate(message: entryMessage);
            _collectionReaderT.ReadAsync("TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }

        [Test]
        public void ReadAsyncTest2()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsyncDelegate(message: entryMessage);
            _collectionReaderT.ReadAsync("Message", entryMessage);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }

        [Test]
        public void ReadAsyncTest3()
        {
            string entryMessage = "Hello World";
            AddMongoEntryAsyncDelegate(message: entryMessage);
            _collectionReaderT.ReadAsync("Message", entryMessage, "TimeStamp", _beforeTest, DateTime.Now);
            _readerAutoResetEvent.WaitOne();
            Assert.AreEqual(1, _asyncReadResults.Count());
            Assert.AreEqual(entryMessage, _asyncReadResults[0].Message);
        }

        [Test]
        public void ReadTest1()
        {
            AddMongoEntry();

            _results.AddRange(_collectionReaderT.Read("TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest2()
        {
            AddMongoEntry();

            _results.AddRange(_collectionReaderT.Read("Message", "Hello"));
            Assert.AreEqual(1, _results.Count());
        }

        [Test]
        public void ReadTest3()
        {
            AddMongoEntry();

            _results.AddRange(_collectionReaderT.Read("Message","Hello","TimeStamp", _beforeTest, DateTime.Now));
            Assert.AreEqual(1, _results.Count());
        }

        #region    Distinct T
        [Test]
        public void DistinctTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            List<string> list = new List<string>(_collectionReaderT.Distinct<string>("Message"));

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

            List<string> list = new List<string>(_collectionReaderT.Distinct<string>("Message", searchQuery));

            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("One", list[0]);
        }

        [Test]
        public void DistinctAsyncTest1()
        {
            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            _collectionReaderT.DistinctAsync<string>("Message");
            _readerAutoResetEvent.WaitOne();

            Assert.AreEqual(3, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0]);
            Assert.AreEqual("Two", _asyncDistinctResults[1]);
            Assert.AreEqual("Three", _asyncDistinctResults[2]);
        }

        [Test]
        public void DistinctAsyncTest2()
        {
            // get distinct message values that are not "Two" or "Three"
            var searchQuery = Query.And(Query.NE("Message", "Two"), Query.NE("Message", "Three"));

            AddMongoEntry("One");
            AddMongoEntry("One");
            AddMongoEntry("Two");
            AddMongoEntry("Three");

            _collectionReaderT.DistinctAsync<string>("Message", searchQuery);
            _readerAutoResetEvent.WaitOne();

            Assert.AreEqual(1, _asyncDistinctResults.Count());
            Assert.AreEqual("One", _asyncDistinctResults[0]);
        }
        #endregion Distinct T
    }
}
